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
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using InputService.Plugin.Properties;
using IrssUtils;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server Plugin for WinLirc.
  /// </summary>
  public class WinLircTransceiver : PluginBase, IConfigure, IRemoteReceiver, ITransmitIR
  {
    #region Constants

    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "WinLirc Receiver.xml");

    #endregion Constants

    #region Variables

    private int _buttonReleaseTime;
    private RemoteHandler _remoteButtonHandler;
    private int _repeatDelay;
    private WinLircServer _server;

    private IPAddress _serverIP;
    private string _serverPath;
    private int _serverPort;
    private bool _startServer;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "WinLirc"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81, original code for MediaPortal by Sven, with contributions by zaphman"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports WinLirc as a Transciever"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    #region IConfigure Members

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.ServerIP = _serverIP;
      config.ServerPort = _serverPort;
      config.StartServer = _startServer;
      config.ServerPath = _serverPath;
      config.ButtonReleaseTime = _buttonReleaseTime;
      config.RepeatDelay = _repeatDelay;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _serverIP = config.ServerIP;
        _serverPort = config.ServerPort;
        _startServer = config.StartServer;
        _serverPath = config.ServerPath;
        _buttonReleaseTime = config.ButtonReleaseTime;
        _repeatDelay = config.RepeatDelay;
        SaveSettings();
      }
    }

    #endregion

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    #endregion

    #region ITransmitIR Members

    /// <summary>
    /// Lists the available blaster ports.
    /// </summary>
    /// <value>The available ports.</value>
    public string[] AvailablePorts
    {
      get { return new string[] {"Default"}; }
    }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Transmit(string port, byte[] data)
    {
      string password, remoteName, buttonName, repeats;

      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(memoryStream);

        password = doc.DocumentElement.Attributes["Password"].Value;
        remoteName = doc.DocumentElement.Attributes["RemoteName"].Value;
        buttonName = doc.DocumentElement.Attributes["ButtonName"].Value;
        repeats = doc.DocumentElement.Attributes["Repeats"].Value;

        string output = String.Format("{0} {1} {2} {3}\n", password, remoteName, buttonName, repeats);
        _server.Transmit(output);
      }

      return true;
    }

    #endregion

    /// <summary>
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
    {
      try
      {
        if( WinLircServer.IsServerRunning())
        {
          return DetectionResult.DevicePresent;
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error("{0} exception: {1}", Name, ex.Message);
        return DetectionResult.DeviceException;
      }

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      if (_startServer && !WinLircServer.StartServer(_serverPath))
        throw new InvalidOperationException("Failed to start server");

      _server = new WinLircServer(_serverIP, _serverPort, TimeSpan.FromMilliseconds(_buttonReleaseTime), _repeatDelay);
      _server.CommandEvent += CommandHandler;
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      //Stop();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      //Start();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      if (_server != null)
      {
        _server.CommandEvent -= CommandHandler;
        _server = null;
      }
    }

    /// <summary>
    /// Loads the settings.
    /// </summary>
    private void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _serverIP = IPAddress.Parse(doc.DocumentElement.Attributes["ServerIP"].Value);
        _serverPort = int.Parse(doc.DocumentElement.Attributes["ServerPort"].Value);
        _startServer = bool.Parse(doc.DocumentElement.Attributes["StartServer"].Value);
        _serverPath = doc.DocumentElement.Attributes["ServerPath"].Value;

        if (!int.TryParse(doc.DocumentElement.Attributes["ButtonReleaseTime"].Value, out _buttonReleaseTime))
          _buttonReleaseTime = 200;

        if (!int.TryParse(doc.DocumentElement.Attributes["RepeatDelay"].Value, out _repeatDelay))
          _repeatDelay = 1;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif

        _serverIP = IPAddress.Loopback;
        _serverPort = 8765;
        _startServer = false;
        _serverPath = "winlirc.exe";
        _buttonReleaseTime = 200;
        _repeatDelay = 1;
      }
    }

    /// <summary>
    /// Saves the settings.
    /// </summary>
    private void SaveSettings()
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

          writer.WriteAttributeString("ServerIP", _serverIP.ToString());
          writer.WriteAttributeString("ServerPort", _serverPort.ToString());
          writer.WriteAttributeString("StartServer", _startServer.ToString());
          writer.WriteAttributeString("ServerPath", _serverPath);
          writer.WriteAttributeString("ButtonReleaseTime", _buttonReleaseTime.ToString());
          writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
      catch
      {
      }
#endif
    }

    /// <summary>
    /// Handles commands.
    /// </summary>
    /// <param name="cmd">The Command.</param>
    private void CommandHandler(WinLircCommand cmd)
    {
      if (_remoteButtonHandler == null)
        return;

      string buttonCode = String.Format("{0}: {1}", cmd.Remote, cmd.Button);

      _remoteButtonHandler(Name, buttonCode);
    }

    #endregion Implementation
  }
}