using System;
using System.Collections.Generic;
using System.Threading;

namespace NamedPipes
{

  #region Delegates

  /// <summary>
  /// Delegate for MessageQueue sink.
  /// </summary>
  /// <param name="message">Message to process.</param>
  public delegate void MessageQueueSink(string message);

  #endregion Delegates

  /// <summary>
  /// Implements a thread-safe Producer/Consumer Queue for messages.
  /// </summary>
  public class MessageQueue : IDisposable
  {

    #region Variables

    //bool disposed = false;

    Thread _workerThread;
    Queue<string> _queue;
    object _queueLock;
    EventWaitHandle _queueWaitHandle;
    volatile bool _processQueue;

    MessageQueueSink _messageSink;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Create a new MessageQueue.
    /// </summary>
    /// <param name="sink">Where to send dequeued messages.</param>
    public MessageQueue(MessageQueueSink sink)
    {
      if (sink == null)
        throw new ArgumentNullException("sink");

      _messageSink = sink;

      // Create locking and control mechanisms ...
      _queueLock = new object();
      _queueWaitHandle = new AutoResetEvent(false);

      // Create FIFO message queue
      _queue = new Queue<string>();
    }

    #endregion Constructor

    #region IDisposable Members

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      if (_processQueue)
        Stop();

      _workerThread = null;

      _queue.Clear();
      _queue = null;

      _queueLock = null;

      _queueWaitHandle.Close();
    }

    #endregion IDisposable Members

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
      _workerThread.IsBackground = true;
      _workerThread.Name = "Message Queue";

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
      if (_workerThread.IsAlive && !_workerThread.Join(1000))
      {
        _workerThread.Abort();
        _workerThread.Join();
      }

      _workerThread = null;
    }
    
    /// <summary>
    /// Add a message to the queue.
    /// </summary>
    /// <param name="message">Message to place in the queue.</param>
    public void Enqueue(string message)
    {
      if (String.IsNullOrEmpty(message))
        return;

      lock (_queueLock)
        _queue.Enqueue(message);

      _queueWaitHandle.Set();
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
        string message;

        while (_processQueue)
        {
          message = null;

          lock (_queueLock)
          {
            if (_queue.Count > 0)
              message = _queue.Dequeue();
          }

          if (String.IsNullOrEmpty(message))
            _queueWaitHandle.WaitOne();
          else
            _messageSink(message);
        }
      }
      catch (ThreadAbortException)
      {

      }
    }

    #endregion Implementation

  }

}
