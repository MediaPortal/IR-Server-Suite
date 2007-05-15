using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;

using AppModule.InterProcessComm;
using AppModule.NamedPipes;

namespace NamedPipes
{

  public sealed class PipeManager : IChannelManager
  {
    const int PIPE_MAX_STUFFED_TIME = 5000;
    
    public object SyncRoot = new object();
    public Hashtable Pipes;

    Hashtable _pipes = new Hashtable();


    uint NumberPipes = 5;
    uint OutBuffer = 512;
    uint InBuffer = 512;
    const int MAX_READ_BYTES = 5000;
    bool _listen = true;

    public bool Listen
    {
      get { return _listen; }
      set { _listen = value; }
    }

    int numChannels = 0;
    Thread MainThread;
    string PipeName;
    ManualResetEvent Mre;

    PipeMessageHandler RequestHandler = null;

    public PipeManager(string pipeName, PipeMessageHandler requestHandler)
    {
      if (String.IsNullOrEmpty(pipeName))
        throw new ArgumentNullException("pipeName");

      if (requestHandler == null)
        throw new ArgumentNullException("requestHandler");

      PipeName = pipeName;
      RequestHandler = requestHandler;
    }

    public void Initialize()
    {
      Pipes = Hashtable.Synchronized(_pipes);
      
      Mre = new ManualResetEvent(false);
      
      MainThread = new Thread(new ThreadStart(Start));
      MainThread.IsBackground = true;
      MainThread.Name = "PipeManager Pipe Thread";
      MainThread.Start();
      
      Thread.Sleep(1000);
    }

    public void HandleRequest(string message)
    {
      if (RequestHandler != null)
        RequestHandler(message);
    }

    private void Start()
    {
      try
      {
        while (_listen)
        {
          int[] keys = new int[Pipes.Keys.Count];
          Pipes.Keys.CopyTo(keys, 0);
          foreach (int key in keys)
          {
            ServerNamedPipe serverPipe = (ServerNamedPipe)Pipes[key];
            if (serverPipe != null && DateTime.Now.Subtract(serverPipe.LastAction).Milliseconds > PIPE_MAX_STUFFED_TIME && serverPipe.PipeConnection.GetState() != InterProcessConnectionState.WaitingForClient)
            {
              serverPipe.Listen = false;
              serverPipe.PipeThread.Abort();
              RemoveServerChannel(serverPipe.PipeConnection.NativeHandle);
            }
          }
          if (numChannels <= NumberPipes)
          {
            ServerNamedPipe pipe = new ServerNamedPipe(PipeName, OutBuffer, InBuffer, MAX_READ_BYTES);
            
            try
            {
              pipe.Connect();
              pipe.LastAction = DateTime.Now;
              System.Threading.Interlocked.Increment(ref numChannels);
              pipe.Start();
              Pipes.Add(pipe.PipeConnection.NativeHandle, pipe);
            }
            catch (InterProcessIOException)
            {
              RemoveServerChannel(pipe.PipeConnection.NativeHandle);
              pipe.Dispose();
            }
          }
          else
          {
            Mre.Reset();
            Mre.WaitOne(1000, false);
          }
        }
      }
      catch (Exception)
      {
      }
    }
    public void Stop()
    {
      _listen = false;
      Mre.Set();
      try
      {
        int[] keys = new int[Pipes.Keys.Count];
        Pipes.Keys.CopyTo(keys, 0);

        foreach (int key in keys)
        {
          ((ServerNamedPipe)Pipes[key]).Listen = false;
        }

        int i = numChannels * 3;
        for (int j = 0; j < i; j++)
        {
          StopServerPipe();
        }

        Pipes.Clear();
        Mre.Close();
        Mre = null;
      }
      catch (Exception)
      {
      }
    }

    public void WakeUp()
    {
      if (Mre != null)
      {
        Mre.Set();
      }
    }

    private void StopServerPipe()
    {
      try
      {
        ClientPipeConnection pipe = new ClientPipeConnection(PipeName);

        if (pipe.TryConnect())
        {
          pipe.Close();
        }
      }
      catch (Exception)
      {
      }
    }

    public void RemoveServerChannel(object param)
    {
      int handle = (int)param;
      System.Threading.Interlocked.Decrement(ref numChannels);
      Pipes.Remove(handle);
      this.WakeUp();
    }

  }

}
