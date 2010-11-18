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
#if TRACE
using System.Diagnostics;
#endif
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IrssComms
{

  #region Delegates

  /// <summary>
  /// Message handling delegate for server.
  /// </summary>
  /// <param name="combo">Combination of Message and ClientManager objects.</param>
  public delegate void ServerMessageSink(MessageManagerCombo combo);

  #endregion Delegates

  /// <summary>
  /// TCP communications server class.
  /// </summary>
  public class Server : IDisposable
  {
    #region Constants

    /// <summary>
    /// The default port that Server sockets will be opened on.
    /// </summary>
    public const int DefaultPort = 24000;

    /// <summary>
    /// Backlog of Socket requests that can build up on the server socket.
    /// </summary>
    private const int SocketBacklog = 10;

    #endregion Constants

    #region Variables

    private readonly IPEndPoint _localEndPoint;

    private readonly GenericPCQueue<MessageManagerCombo> _messageQueue;
    private readonly ServerMessageSink _messageSink;

    private WaitCallback _clientDisconnectCallback;
    private List<ClientManager> _clientManagers;
    private Thread _connectionThread;
    private bool _processConnectionThread;
    private Socket _serverSocket;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or Sets the Client Disconnect callback.
    /// </summary>
    public WaitCallback ClientDisconnectCallback
    {
      get { return _clientDisconnectCallback; }
      set { _clientDisconnectCallback = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Create a TCP communications server.
    /// </summary>
    /// <param name="port">Port to open for listening.</param>
    /// <param name="messageSink">Where to send received messages.</param>
    public Server(int port, ServerMessageSink messageSink)
    {
      _localEndPoint = new IPEndPoint(IPAddress.Any, port);

      _messageSink = messageSink;

      _messageQueue = new GenericPCQueue<MessageManagerCombo>(QueueMessageSink);
    }

    #endregion Constructor

    #region IDisposable

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources ...
        Stop();

        _messageQueue.Dispose();
      }

      // Free native resources ...
    }

    #endregion IDisposable

    #region Implementation

    /// <summary>
    /// Start the server.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Start()
    {
      if (_processConnectionThread)
        return false;

      _processConnectionThread = true;

      try
      {
        _messageQueue.ClearQueue();
        _messageQueue.Start();
      }
      catch
      {
        _processConnectionThread = false;

        throw;
      }

      try
      {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(_localEndPoint);
        _serverSocket.Listen(SocketBacklog);
      }
      catch
      {
        _processConnectionThread = false;
        _serverSocket = null;

        throw;
      }

      try
      {
        _clientManagers = new List<ClientManager>();

        _connectionThread = new Thread(ConnectionThread);
        _connectionThread.Name = "IrssComms.Server.ConnectionThread";
        _connectionThread.IsBackground = true;
        _connectionThread.Start();
      }
      catch
      {
        _processConnectionThread = false;
        _serverSocket = null;
        _clientManagers = null;
        _connectionThread = null;

        throw;
      }

      return true;
    }

    /// <summary>
    /// Stop the server.
    /// </summary>
    public void Stop()
    {
      if (!_processConnectionThread)
        return;

      _messageQueue.Stop();

      _processConnectionThread = false;

      _serverSocket.Close();
      _serverSocket = null;

      lock (_clientManagers)
      {
        foreach (ClientManager manager in _clientManagers)
          manager.Dispose();

        _clientManagers.Clear();
        _clientManagers = null;
      }

      //_connectThread.Abort();
      //_connectThread.Join();
      _connectionThread = null;
    }

    /// <summary>
    /// Send a message to a particular client.
    /// </summary>
    /// <param name="sendTo">Client to send to.</param>
    /// <param name="message">Message to send.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Send(ClientManager sendTo, IrssMessage message)
    {
      if (_clientManagers == null) return false;
      if (!_clientManagers.Contains(sendTo)) return false;

      try
      {
        sendTo.Send(message);
        return true;
      }
      catch (SocketException)
      {
        ClientDisconnect(sendTo);
        return false;
      }
    }

    private void ClientManagerMessageSink(MessageManagerCombo combo)
    {
      _messageQueue.Enqueue(combo);
    }

    private void QueueMessageSink(MessageManagerCombo combo)
    {
      _messageSink(combo);
    }

    private void ClientDisconnect(object obj)
    {
      ClientManager clientManager = obj as ClientManager;
      if (clientManager == null)
        return;

      if (_clientManagers != null)
      {
        lock (_clientManagers)
        {
          if (_clientManagers.Contains(clientManager))
          {
            _clientManagers.Remove(clientManager);
          }
        }
      }

      if (_clientDisconnectCallback != null)
        _clientDisconnectCallback(clientManager);

      clientManager.Dispose();
    }

    private void ConnectionThread()
    {
      try
      {
        while (_processConnectionThread)
        {
          Socket socket = _serverSocket.Accept();

          if (_clientManagers == null)
            throw new InvalidOperationException(
              "Cannot accept new connections, _clientManagers object is not initialised.");

          lock (_clientManagers)
          {
            ClientManager manager = new ClientManager(socket, ClientManagerMessageSink);
            manager.DisconnectCallback = ClientDisconnect;

            _clientManagers.Add(manager);

            manager.Start();
          }
        }
      }
#if TRACE
      catch (SocketException socketException)
      {
        Trace.WriteLine(socketException.ToString());
      }
#else
      catch (SocketException)
      {
      }
#endif
    }

    #endregion Implementation
  }
}