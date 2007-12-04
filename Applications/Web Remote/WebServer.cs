using System;
#if TRACE
using System.Diagnostics;
#endif
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

using IrssUtils;

namespace WebRemote
{

  /// <summary>
  /// Provides a Web front-end to the Virtual Remote.
  /// </summary>
  public class WebServer
  {

    #region Constants

    const string ServerName = "Web Remote";

    #endregion Constants

    #region Variables

    int _serverPort;

    Socket _serverSocket;
    NetworkStream _networkStream;
    TextReader _networkReader;

    Thread _runningThread;

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

      _runningThread = new Thread(new ThreadStart(RunThread));
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

    void RunThread()
    {
      try
      {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint ipe = new IPEndPoint(IPAddress.Any, _serverPort);
        _serverSocket.Bind(ipe);

        while (true)
        {
          Socket socket = AcceptConnection();
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
        IrssLog.Error(ex.ToString());
      }
      finally
      {
        _serverSocket.Close();
      }
    }

    void DoGet(string argument)
    {
      string url = GetUrl(argument);

      if (url.StartsWith("/"))
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

    string GetUrl(string argument)
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
    
    Socket AcceptConnection()
    {
      _serverSocket.Listen(1);
      Socket socket = _serverSocket.Accept();

      _networkStream = new NetworkStream(socket, FileAccess.ReadWrite, true);
      _networkReader = new StreamReader(_networkStream);
      IPEndPoint ep = (IPEndPoint)socket.RemoteEndPoint;

      return socket;
    }

    string GetCommand()
    {
      string buf;
      string command = String.Empty;
      bool first = true;

      while ((buf = _networkReader.ReadLine()) != null && buf.Length > 0)
      {
        if (first)
        {
          command = buf;
          first = false;
        }
      }

      return command;
    }
    
    string GetCommandElement(string command)
    {
      string[] commandElements = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      return commandElements[1];
    }
        
    void SendFile(string path)
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

    void SendString(string message, params object[] args)
    {
      string output = String.Format(message, args);

      byte[] outputBytes = Encoding.ASCII.GetBytes(output);

      _networkStream.Write(outputBytes, 0, outputBytes.Length);
    }

    void SendError(int errorNumber, string errorString)
    {
      SendString("HTTP/1.1 {0} {1}\r\n", errorNumber, errorString);
      SendString("Date:{0}\r\n", DateTime.Now);
      SendString("Server:{0}\r\n", ServerName);
      SendString("Content-Type: text/html; charset=utf-8\r\n");
      SendString("Connection: close\r\n");
    }

    void SendOK(string contentType)
    {
      SendString("HTTP/1.1 200 OK\r\n");
      SendString("Date:{0}\r\n", DateTime.Now);
      SendString("Server:{0}\r\n", ServerName);
      SendString("Content-Type: {0}\r\n\r\n", contentType);
    }

    void SendRedirect(string newLocation)
    {
      SendString("HTTP/1.1 301 Moved Permanently\r\n");
      SendString("Date:{0}\r\n", DateTime.Now);
      SendString("Server:{0}\r\n", ServerName);
      SendString("Location: {0}\r\n\r\n", newLocation);
    }

    #endregion Implementation

  }

}
