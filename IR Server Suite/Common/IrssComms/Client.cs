#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IrssComms
{

  #region Delegates

  /// <summary>
  /// Message handling delegate for client.
  /// </summary>
  /// <param name="message">Message received.</param>
  public delegate void ClientMessageSink(IrssMessage message);

  #endregion Delegates

  /// <summary>
  /// TCP communications client class.
  /// </summary>
  public class Client : IDisposable
  {
    #region Variables

    private readonly GenericPCQueue<IrssMessage> _messageQueue;

    private readonly ClientMessageSink _messageSink;
    private readonly IPEndPoint _serverEndpoint;
    private WaitCallback _commsFailureCallback;

    private WaitCallback _connectCallback;
    private volatile bool _connected;
    private WaitCallback _disconnectCallback;
    private volatile bool _processConnectionThread;
    private Socket _serverSocket;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Is this client connected?
    /// </summary>
    public bool Connected
    {
      get { return _connected; }
    }

    /// <summary>
    /// Gets or Sets the Connect callback.
    /// </summary>
    public WaitCallback ConnectCallback
    {
      get { return _connectCallback; }
      set { _connectCallback = value; }
    }

    /// <summary>
    /// Gets or Sets the Disconnect callback.
    /// </summary>
    public WaitCallback DisconnectCallback
    {
      get { return _disconnectCallback; }
      set { _disconnectCallback = value; }
    }

    /// <summary>
    /// Gets or Sets the Communications Failure callback.
    /// </summary>
    public WaitCallback CommsFailureCallback
    {
      get { return _commsFailureCallback; }
      set { _commsFailureCallback = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Create a TCP communications client.
    /// </summary>
    /// <param name="serverEndPoint">IP Address and Port combination of Server.</param>
    /// <param name="messageSink">The message sink to call for messages.</param>
    public Client(IPEndPoint serverEndPoint, ClientMessageSink messageSink)
    {
      _serverEndpoint = serverEndPoint;

      _messageSink = messageSink;

      _messageQueue = new GenericPCQueue<IrssMessage>(QueueMessageSink);
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
    /// Start the client communications.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Start()
    {
      if (_processConnectionThread)
        return false;

      _processConnectionThread = true;
      _connected = false;

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
        Thread connectionThread = new Thread(ConnectionThread);
        connectionThread.Name = "IrssComms.Client.ConnectionThread";
        connectionThread.IsBackground = true;
        connectionThread.Start();
      }
      catch
      {
        _processConnectionThread = false;
        _messageQueue.Stop();

        throw;
      }

      return true;
    }

    /// <summary>
    /// Stop the client communications.
    /// </summary>
    public void Stop()
    {
      if (!_processConnectionThread)
        return;

      _messageQueue.Stop();

      _processConnectionThread = false;
      _connected = false;

      _serverSocket.Close();
      _serverSocket = null;
    }

    /// <summary>
    /// Send a message to the server.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Send(IrssMessage message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      if (_serverSocket == null)
        return false;

      byte[] data = message.ToBytes();

      byte[] frame = new byte[sizeof(int) + data.Length];
      Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length)), 0, frame, 0, sizeof(int));
      Buffer.BlockCopy(data, 0, frame, sizeof(int), data.Length);

      try
      {
        // Send framed message
        _serverSocket.Send(frame);

        return true;
      }
      catch (SocketException)
      {
        return false;
      }
    }

    private void QueueMessageSink(IrssMessage message)
    {
      _messageSink(message);
    }

    private void ConnectionThread()
    {
      // Outer loop is for reconnection attempts ...
      while (_processConnectionThread)
      {
        _connected = false;

        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        #region Attempt to connect

        while (_processConnectionThread)
        {
          try
          {
            _serverSocket.Connect(_serverEndpoint);
            break;
          }
          catch (SocketException socketException)
          {
            if (!_processConnectionThread)
              return;

            if (socketException.SocketErrorCode == SocketError.ConnectionRefused)
            {
              Thread.Sleep(1000);
              continue;
            }

            if (_commsFailureCallback != null)
              _commsFailureCallback(socketException);
            else
              throw;
          }
          catch (Exception ex)
          {
            if (!_processConnectionThread)
              return;

            if (_commsFailureCallback != null)
              _commsFailureCallback(ex);
            else
              throw;
          }
        }

        #endregion Attempt to connect

        if (!_processConnectionThread)
          return;

        _connected = true;

        if (_connectCallback != null)
          _connectCallback(null);

        #region Read from socket

        try
        {
          byte[] buffer = new byte[4];

          // Read data from socket ...
          while (_processConnectionThread)
          {
            if (!Receive(buffer))
              break;

            byte[] packet = new byte[IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 0))];

            if (!Receive(packet))
              break;

            IrssMessage message = IrssMessage.FromBytes(packet);
            _messageQueue.Enqueue(message);
          }

          if (!_processConnectionThread)
            return;

          if (_disconnectCallback != null)
            _disconnectCallback(null);
        }
        catch (SocketException socketException)
        {
          if (!_processConnectionThread)
            return;

          if (socketException.SocketErrorCode == SocketError.ConnectionReset)
          {
            if (_disconnectCallback != null)
              _disconnectCallback(null);
          }
          else
          {
            if (_commsFailureCallback != null)
              _commsFailureCallback(socketException);
            else
              throw;
          }
        }
        catch (Exception ex)
        {
          if (!_processConnectionThread)
            return;

          if (_commsFailureCallback != null)
            _commsFailureCallback(ex);
          else
            throw;
        }

        #endregion Read from socket
      }
    }

    // Returns true if buffer filled, false if connection has been closed.
    bool Receive(byte[] buffer)
    {
      int bytesRead;
      int bytesReadTotal = 0;
      do
      {
        bytesRead = _serverSocket.Receive(buffer, bytesReadTotal, buffer.Length - bytesReadTotal, SocketFlags.None);
        bytesReadTotal += bytesRead;
      } while ((bytesReadTotal < buffer.Length) && (bytesRead > 0));
      return (bytesReadTotal == buffer.Length);
    }

    #endregion Implementation
  }
}