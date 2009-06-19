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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using IrssUtils;

namespace WebRemote
{
  /// <summary>
  /// Provides a Web front-end to the Virtual Remote.
  /// </summary>
  internal class WebServer
  {
    #region Constants

    private const string ServerName = "Web Remote";

    #endregion Constants

    #region Variables

    private readonly int _serverPort;

    private TextReader _networkReader;
    private NetworkStream _networkStream;

    private Thread _runningThread;
    private Socket _serverSocket;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="WebServer"/> class.
    /// </summary>
    /// <param name="port">The port to serve on.</param>
    public WebServer(int port)
    {
      _serverPort = port;
    }

    #endregion Constructor

    #region Public Methods

    /// <summary>
    /// Runs the Web Server.
    /// </summary>
    public void Run()
    {
      if (_runningThread != null)
        _runningThread.Abort();

      _runningThread = new Thread(RunThread);
      _runningThread.Name = "WebRemote Server";
      _runningThread.IsBackground = true;
      _runningThread.Start();
    }

    /// <summary>
    /// Stops the Web Server.
    /// </summary>
    public void Stop()
    {
      if (_runningThread != null)
      {
        _serverSocket.Close(200);

        try
        {
          _runningThread.Abort();
        }
        finally
        {
          _runningThread = null;
        }
      }
    }

    #endregion Public Methods

    #region Implementation

    private void RunThread()
    {
      try
      {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint ipe = new IPEndPoint(IPAddress.Any, _serverPort);
        _serverSocket.Bind(ipe);

        while (true)
        {
          AcceptConnection();

          string command = HttpUtility.UrlDecode(GetCommand());

          if (command.StartsWith("GET", StringComparison.OrdinalIgnoreCase))
          {
            string commandElement = GetCommandElement(command);
            DoGet(commandElement);
          }

          _networkStream.Flush();
          _networkStream.Close();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
      finally
      {
        _serverSocket.Close();
      }
    }

    private void DoGet(string argument)
    {
      string url = GetUrl(argument);

      if (url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
        url = url.Substring(1);

      if (url.Length == 0)
        url = "INDEX.HTML";

#if TRACE
      Trace.WriteLine(DateTime.Now.TimeOfDay.ToString());
      Trace.WriteLine("Arg: " + argument);
      Trace.WriteLine("Url: " + url);
      Trace.WriteLine("");
#endif

      if (url.StartsWith(Program.ButtonClickPrefix, StringComparison.OrdinalIgnoreCase))
      {
        string command = url.Substring(Program.ButtonClickPrefix.Length);

        Program.ButtonPress(command);

        SendError(404, "File Not Found");
        return;
      }

      switch (url.ToUpperInvariant())
      {
        case "INDEX.HTML":
          SendOK("text/html; charset=utf-8");
          SendString("<HTML>");
          SendFile(Program.WebFile);
          SendString(Program.ImageMap);
          SendString("</HTML>");
          break;

        case "REMOTE_SKIN.PNG":
          SendOK("image/png");
          SendFile(Program.ImageFile);
          break;

        default:
          SendError(404, "File Not Found");
          break;
      }
    }

    private void AcceptConnection()
    {
      _serverSocket.Listen(1);

      Socket socket = _serverSocket.Accept();

      _networkStream = new NetworkStream(socket, FileAccess.ReadWrite, true);
      _networkReader = new StreamReader(_networkStream);
    }

    private string GetCommand()
    {
      string buf;
      string command = String.Empty;
      bool first = true;

      try
      {
        while ((buf = _networkReader.ReadLine()) != null && buf.Length > 0)
        {
          if (first)
          {
            command = buf;
            first = false;
          }
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      return command;
    }

    private void SendFile(string path)
    {
      byte[] buffer = new byte[2048];

      int byteCount;
      using (FileStream fileStream = File.OpenRead(path))
      {
        while ((byteCount = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        {
          _networkStream.Write(buffer, 0, byteCount);
        }
      }
    }

    private void SendString(string message, params object[] args)
    {
      string output = String.Format(message, args);

      byte[] outputBytes = Encoding.ASCII.GetBytes(output);

      _networkStream.Write(outputBytes, 0, outputBytes.Length);
    }

    private void SendError(int errorNumber, string errorString)
    {
      SendString("HTTP/1.1 {0} {1}\r\n", errorNumber, errorString);
      SendString("Date:{0}\r\n", DateTime.Now);
      SendString("Server:{0}\r\n", ServerName);
      SendString("Content-Type: text/html; charset=utf-8\r\n");
      SendString("Connection: close\r\n");
    }

    private void SendOK(string contentType)
    {
      SendString("HTTP/1.1 200 OK\r\n");
      SendString("Date:{0}\r\n", DateTime.Now);
      SendString("Server:{0}\r\n", ServerName);
      SendString("Content-Type: {0}\r\n\r\n", contentType);
    }

    /*
    void SendRedirect(string newLocation)
    {
      SendString("HTTP/1.1 301 Moved Permanently\r\n");
      SendString("Date:{0}\r\n", DateTime.Now);
      SendString("Server:{0}\r\n", ServerName);
      SendString("Location: {0}\r\n\r\n", newLocation);
    }
    */

    private static string GetUrl(string argument)
    {
      StringBuilder outputString = new StringBuilder();

      for (int index = 0; index < argument.Length; index++)
      {
        char curChar = argument[index];

        if (Char.IsWhiteSpace(curChar))
          break;

        outputString.Append(curChar);
      }

      return outputString.ToString();
    }

    private static string GetCommandElement(string command)
    {
      string[] commandElements = command.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
      return commandElements[1];
    }

    #endregion Implementation
  }
}