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
#if TRACE
using System.Diagnostics;
#endif
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IrssComms
{
  /// <summary>
  /// Manages Server socket connections.
  /// </summary>
  public class ClientManager : IDisposable
  {
    #region Variables

    private Socket _connection;

    private WaitCallback _disconnectCallback;
    private ServerMessageSink _messageSink;
    private bool _processReceiveThread;
    private Thread _receiveThread;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or Sets the Disconnect callback.
    /// </summary>
    public WaitCallback DisconnectCallback
    {
      get { return _disconnectCallback; }
      set { _disconnectCallback = value; }
    }

    #endregion Properties

    #region Constructor

    internal ClientManager(Socket connection, ServerMessageSink sink)
    {
      _connection = connection;
      _messageSink = sink;
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
      }

      // Free native resources ...
    }

    #endregion IDisposable

    #region Implementation

    internal void Start()
    {
      if (_processReceiveThread)
        return;

      _processReceiveThread = true;

      _receiveThread = new Thread(ReceiveThread);
      _receiveThread.Name = "IrssComms.ClientManager";
      _receiveThread.IsBackground = true;
      _receiveThread.Start();
    }

    internal void Stop()
    {
      if (!_processReceiveThread)
        return;

      _processReceiveThread = false;

      if (_connection != null)
      {
        _connection.Close(100);
        _connection = null;
      }

      _messageSink = null;

      //_receiveThread.Abort();
      //_receiveThread.Join();
      _receiveThread = null;
    }

    internal void Send(IrssMessage message)
    {
      byte[] data = message.ToBytes();

      byte[] frame = new byte[sizeof(int) + data.Length];
      Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length)), 0, frame, 0, sizeof(int));
      Buffer.BlockCopy(data, 0, frame, sizeof(int), data.Length);

      // Send framed message
      _connection.Send(frame);
    }

    private void ReceiveThread()
    {
      try
      {
        byte[] buffer = new byte[4];

        while (_processReceiveThread)
        {
          if (!Receive(buffer))
            break;

          byte[] packet = new byte[IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 0))];

          if (!Receive(packet))
            break;

          IrssMessage message = IrssMessage.FromBytes(packet);
          MessageManagerCombo combo = new MessageManagerCombo(message, this);

          if (_messageSink != null)
            _messageSink(combo);
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
      finally
      {
        if (_connection != null)
        {
          _connection.Close(100);
          _connection = null;
        }

        _disconnectCallback(this);
      }
    }

    // Returns true if buffer filled, false if connection has been closed.
    bool Receive(byte[] buffer)
    {
      int bytesRead;
      int bytesReadTotal = 0;
      do
      {
        bytesRead = _connection.Receive(buffer, bytesReadTotal, buffer.Length - bytesReadTotal, SocketFlags.None);
        bytesReadTotal += bytesRead;
      } while ((bytesReadTotal < buffer.Length) && (bytesRead > 0));
      return (bytesReadTotal == buffer.Length);
    }

    #endregion Implementation
  }
}