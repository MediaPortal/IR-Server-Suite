using System;
using System.Collections.Generic;
using System.Text;

using AppModule.InterProcessComm;
using AppModule.NamedPipes;

namespace NamedPipes
{
  public delegate void PipeMessageHandler(string message);

  public static class PipeAccess
  {
    public static IChannelManager ServerPipeManager = null;

    public static bool ServerRunning = false;

    public static bool StartServer(string name, PipeMessageHandler messageHandler)
    {
      if (ServerRunning)
        return false;

      ServerPipeManager = new PipeManager(name, messageHandler);
      ServerPipeManager.Initialize();

      ServerRunning = true;
      return true;
    }
    
    public static bool StopServer()
    {
      if (!ServerRunning)
        return false;
      
      ServerPipeManager.Stop();

      ServerRunning = false;
      return true;
    }

    public static bool PipeExists(string pipe)
    {
      return NamedPipeNative.WaitNamedPipe(pipe, 2000);
    }

    public static void SendMessage(string pipeName, string server, PipeMessage message)
    {
      string messageString = message.ToString();

      ClientPipeConnection writePipe = new ClientPipeConnection(pipeName, server);
      writePipe.Connect();

      writePipe.Write(messageString);

      writePipe.Close();
    }
    
  }

}
