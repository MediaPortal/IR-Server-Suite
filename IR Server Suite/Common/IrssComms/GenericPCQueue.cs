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
using System.Threading;

namespace IrssComms
{

  #region Delegates

  /// <summary>
  /// Delegate for GenericPCQueue sink.
  /// </summary>
  /// <param name="obj">Generic object to process.</param>
  public delegate void GenericPCQueueSink<T>(T obj);

  #endregion Delegates

  /// <summary>
  /// Implements a thread-safe Producer/Consumer Queue for generics.
  /// </summary>
  public class GenericPCQueue<T> : IDisposable where T : class
  {
    #region Variables

    private readonly GenericPCQueueSink<T> _sink;
    private volatile bool _processQueue;
    private Queue<T> _queue;
    private object _queueLock;
    private EventWaitHandle _queueWaitHandle;
    private Thread _workerThread;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Create a new MessageQueue.
    /// </summary>
    /// <param name="sink">Where to send dequeued messages.</param>
    public GenericPCQueue(GenericPCQueueSink<T> sink)
    {
      if (sink == null)
        throw new ArgumentNullException("sink");

      _sink = sink;

      // Create locking and control mechanisms ...
      _queueLock = new object();
      _queueWaitHandle = new AutoResetEvent(false);

      // Create FIFO generic queue
      _queue = new Queue<T>();
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

        _workerThread = null;

        _queue.Clear();
        _queue = null;

        _queueLock = null;

        _queueWaitHandle.Close();
        _queueWaitHandle = null;
      }

      // Free native resources ...
    }

    #endregion IDisposable

    #region Implementation

    /// <summary>
    /// Start processing the queue.
    /// </summary>
    public void Start()
    {
      if (_processQueue)
        return;

      _processQueue = true;

      // Create the worker thread  ...
      _workerThread = new Thread(WorkerThread);
      _workerThread.Name = "GenericPCQueue";
      _workerThread.IsBackground = true;

      _workerThread.Start();
    }

    /// <summary>
    /// Stop processing the queue.
    /// </summary>
    public void Stop()
    {
      if (!_processQueue)
        return;

      // Signal the worker thread to stop ...
      _processQueue = false;
      _queueWaitHandle.Set();

      // Join the worker thread and wait for it to finish ...
      if (_workerThread != null && _workerThread.IsAlive && !_workerThread.Join(1000))
      {
        _workerThread.Abort();
        _workerThread.Join();
      }

      _workerThread = null;
    }

    /// <summary>
    /// Add a generic object to the queue.
    /// </summary>
    /// <param name="obj">Generic object to place in the queue.</param>
    public void Enqueue(T obj)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");

      lock (_queueLock)
      {
        _queue.Enqueue(obj);

        _queueWaitHandle.Set();
      }
    }

    /// <summary>
    /// Clears the queue of any messages.
    /// </summary>
    public void ClearQueue()
    {
      lock (_queueLock)
      {
        _queue.Clear();
      }
    }

    /// <summary>
    /// Queue processing worker thread.
    /// </summary>
    private void WorkerThread()
    {
      try
      {
        T obj = default(T);
        bool didDequeue = false;

        while (_processQueue)
        {
          lock (_queueLock)
          {
            if (_queue.Count > 0)
            {
              obj = _queue.Dequeue();
              didDequeue = true;
            }
          }

          if (didDequeue)
          {
            _sink(obj);
            obj = default(T);
            didDequeue = false;
          }
          else
          {
            _queueWaitHandle.WaitOne();
          }
        }
      }
#if TRACE
      catch (ThreadAbortException threadAbortException)
      {
        Trace.WriteLine(threadAbortException.ToString());
      }
#else
      catch (ThreadAbortException)
      {
      }
#endif
    }

    #endregion Implementation
  }
}