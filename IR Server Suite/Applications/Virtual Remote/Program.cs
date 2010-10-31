#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace VirtualRemote
{
  internal static class Program
  {
    #region Constants

    private const string DefaultSkin = "MCE";

    private static readonly string ConfigurationFolder = Path.Combine(Common.FolderAppData,
                                                                    "Virtual Remote");
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationFolder,
                                                                    "Virtual Remote.xml");

    #endregion Constants

    #region Variables

    private static RemoteButton[] _buttons;
    private static Client _client;
    private static string _device;

    private static bool _registered;
    private static string _remoteSkin;

    private static string _serverHost;

    private static string _skinsFolder;

    #endregion Variables

    #region Properties

    internal static bool Registered
    {
      get { return _registered; }
    }

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }

    internal static string SkinsFolder
    {
      get { return _skinsFolder; }
    }

    internal static string RemoteSkin
    {
      get { return _remoteSkin; }
      set { _remoteSkin = value; }
    }

    internal static RemoteButton[] Buttons
    {
      get { return _buttons; }
      set { _buttons = value; }
    }

    internal static string Device
    {
      get { return _device; }
      set { _device = value; }
    }

    #endregion Properties

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      IrssLog.LogLevel = IrssLog.Level.Debug;
      IrssLog.Open("Virtual Remote.log");

      Application.ThreadException += Application_ThreadException;

      LoadSettings();

      if (args.Length > 0) // Command Line Start ...
      {
        List<string> virtualButtons = new List<string>();

        try
        {
          for (int index = 0; index < args.Length; index++)
          {
            if (args[index].Equals("-host", StringComparison.OrdinalIgnoreCase))
            {
              _serverHost = args[++index];
              continue;
            }
            else
            {
              virtualButtons.Add(args[index]);
            }
          }
        }
        catch (Exception ex)
        {
          IrssLog.Error("Error processing command line parameters: {0}", ex.ToString());
        }

        IPAddress serverIP = Network.GetIPFromName(_serverHost);
        IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

        if (virtualButtons.Count != 0 && StartClient(endPoint))
        {
          Thread.Sleep(250);

          // Wait for registered ... Give up after 10 seconds ...
          int attempt = 0;
          while (!_registered)
          {
            if (++attempt >= 10)
              break;
            else
              Thread.Sleep(1000);
          }

          if (_registered)
          {
            foreach (string button in virtualButtons)
            {
              if (button.StartsWith("~", StringComparison.OrdinalIgnoreCase))
              {
                Thread.Sleep(button.Length * 500);
              }
              else
              {
                IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Notify, button);
                _client.Send(message);
              }
            }

            Thread.Sleep(500);
          }
          else
          {
            IrssLog.Warn("Failed to register with server host \"{0}\", custom message(s) not sent", ServerHost);
          }
        }
      }
      else // GUI Start ...
      {
        if (String.IsNullOrEmpty(_serverHost))
        {
          ServerAddress serverAddress = new ServerAddress();
          serverAddress.ShowDialog();

          _serverHost = serverAddress.ServerHost;
        }

        bool clientStarted = false;

        IPAddress serverIP = Network.GetIPFromName(_serverHost);
        IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

        try
        {
          clientStarted = StartClient(endPoint);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
          clientStarted = false;
        }

        if (clientStarted)
          Application.Run(new MainForm());

        SaveSettings();
      }

      StopClient();

      Application.ThreadException -= Application_ThreadException;

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }

    private static void LoadSettings()
    {
      try
      {
        _skinsFolder = SystemRegistry.GetInstallFolder();
        if (String.IsNullOrEmpty(_skinsFolder))
          _skinsFolder = ".\\Skins";
        else
          _skinsFolder = Path.Combine(_skinsFolder, "Virtual Remote\\Skins");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _skinsFolder = ".\\Skins";
      }

      XmlDocument doc = new XmlDocument();

      try
      {
        doc.Load(ConfigurationFile);
      }
      catch (DirectoryNotFoundException)
      {
        IrssLog.Warn("Configuration directory not found, using default settings");

        Directory.CreateDirectory(ConfigurationFolder);
        CreateDefaultSettings();
        return;
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using default settings");

        CreateDefaultSettings();
        return;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
        return;
      }

      try
      {
        _serverHost = doc.DocumentElement.Attributes["ServerHost"].Value;
      }
      catch
      {
        _serverHost = "localhost";
      }
      try
      {
        _remoteSkin = doc.DocumentElement.Attributes["RemoteSkin"].Value;
      }
      catch
      {
        _remoteSkin = DefaultSkin;
      }
    }

    private static void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("ServerHost", _serverHost);
          writer.WriteAttributeString("RemoteSkin", _remoteSkin);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void CreateDefaultSettings()
    {
      _serverHost = "localhost";
      _remoteSkin = DefaultSkin;

      SaveSettings();
    }


    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        IrssLog.Error("Communications failure: {0}", ex.Message);
      else
        IrssLog.Error("Communications failure");

      StopClient();

      MessageBox.Show("Please report this error.", "Virtual Remote - Communications failure", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
    }

    private static void Connected(object obj)
    {
      IrssLog.Info("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private static void Disconnected(object obj)
    {
      IrssLog.Warn("Communications with server has been lost");

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

      if (_client.Start())
      {
        return true;
      }
      else
      {
        _client = null;
        return false;
      }
    }

    internal static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;

      _registered = false;
    }

    internal static bool ProcessClick(int x, int y)
    {
      if (_buttons != null)
      {
        foreach (RemoteButton button in _buttons)
        {
          if (y >= button.Top &&
              y < button.Top + button.Height &&
              x >= button.Left &&
              x <= button.Left + button.Width)
          {
            ButtonPress(button.Code);
            return true;
          }
        }
      }

      return false;
    }

    internal static void ButtonPress(string keyCode)
    {
      if (!_registered)
        return;

      byte[] bytes = IrssMessage.EncodeRemoteEventData(_device, keyCode);

      IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request, bytes);
      SendMessage(message);
    }

    private static void SendMessage(IrssMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      if (_client != null)
        _client.Send(message);
    }

    private static void ReceivedMessage(IrssMessage received)
    {
      IrssLog.Debug("Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              //_irServerInfo = IRServerInfo.FromBytes(received.DataAsBytes);
              _registered = true;

              IrssLog.Info("Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              IrssLog.Warn("IR Server refused to register");
            }
            break;

          case MessageType.ServerShutdown:
            IrssLog.Warn("IR Server Shutdown - Virtual Remote disabled until IR Server returns");
            _registered = false;
            break;

          case MessageType.Error:
            IrssLog.Error(received.GetDataAsString());
            break;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }
  }
}