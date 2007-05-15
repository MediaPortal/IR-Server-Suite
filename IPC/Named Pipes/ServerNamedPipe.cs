using System;
using System.IO;
using System.Text;
using System.Threading;

using AppModule.InterProcessComm;
using AppModule.NamedPipes;

namespace NamedPipes
{

  public sealed class ServerNamedPipe : IDisposable
  {
    internal Thread PipeThread;
    internal ServerPipeConnection PipeConnection;
    internal bool Listen = true;
    internal DateTime LastAction;
    private bool disposed = false;

    private void PipeListener()
    {
      CheckIfDisposed();

      try
      {
        Listen = PipeAccess.ServerPipeManager.Listen;

        while (Listen)
        {
          LastAction = DateTime.Now;
          string message = PipeConnection.Read();
          LastAction = DateTime.Now;
          if (!String.IsNullOrEmpty(message.Trim()))
            PipeAccess.ServerPipeManager.HandleRequest(message);

          LastAction = DateTime.Now;
          PipeConnection.Disconnect();
          if (Listen)
            Connect();

          PipeAccess.ServerPipeManager.WakeUp();
        }
      }
      catch (System.Threading.ThreadAbortException) { }
      catch (System.Threading.ThreadStateException) { }
      catch (Exception)
      {
      }
      finally
      {
        this.Close();
      }
    }

    internal void Connect()
    {
      CheckIfDisposed();
      PipeConnection.Connect();
    }
    internal void Close()
    {
      CheckIfDisposed();
      this.Listen = false;
      PipeAccess.ServerPipeManager.RemoveServerChannel(this.PipeConnection.NativeHandle);
      this.Dispose();
    }
    internal void Start()
    {
      CheckIfDisposed();
      PipeThread.Start();
    }
    private void CheckIfDisposed()
    {
      if (this.disposed)
      {
        throw new ObjectDisposedException("ServerNamedPipe");
      }
    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        PipeConnection.Dispose();
        if (PipeThread != null)
        {
          try
          {
            PipeThread.Abort();
          }
          catch (System.Threading.ThreadAbortException) { }
          catch (System.Threading.ThreadStateException) { }
          catch (Exception)
          {
          }
        }
      }
      disposed = true;
    }


    ~ServerNamedPipe()
    {
      Dispose(false);
    }
    internal ServerNamedPipe(string name, uint outBuffer, uint inBuffer, int maxReadBytes)
    {
      PipeConnection = new ServerPipeConnection(name, outBuffer, inBuffer, maxReadBytes);
      PipeThread = new Thread(new ThreadStart(PipeListener));
      PipeThread.IsBackground = true;
      PipeThread.Name = "Pipe Thread " + this.PipeConnection.NativeHandle.ToString();
      LastAction = DateTime.Now;
    }

  }

}
