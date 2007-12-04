using System;
using System.Collections;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
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
  public class GenericPCQueue<T> : IDisposable
  {

    #region Variables

    Thread _workerThread;
    Queue<T> _queue;
    object _queueLock;
    EventWaitHandle _queueWaitHandle;
    volatile bool _processQueue;

    GenericPCQueueSink<T> _sink;

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
      _workerThread = new Thread(new ThreadStart(WorkerThread));
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
    void WorkerThread()
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
