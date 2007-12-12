using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using IrssComms;

namespace TestServer
{

  class Program
  {

    static int count = 1;

    static Server server;

    static void Main(string[] args)
    {
      ServerMessageSink sink = new ServerMessageSink(MessageSink);

      server = new Server(Server.DefaultPort, sink);
      try
      {
        if (server.Start())
        {
          Console.WriteLine("Server Started");
        }
        else
        {
          Console.WriteLine("Failed to start Server");
        }

        Console.ReadLine();

        server.Dispose();

        Console.WriteLine("Stopped");
      }
      catch (SocketException socketException)
      {
        Console.WriteLine(socketException.ToString());
        Console.ReadLine();
      }
    }

    static void MessageSink(MessageManagerCombo combo)
    {
      IrssMessage response = new IrssMessage(MessageType.Unknown, MessageFlags.None);

      switch (combo.Message.Type)
      {
        case MessageType.RegisterClient:
          Console.WriteLine("Registered: {0}", count);
          
          response.Type = MessageType.RegisterClient;
          response.Flags = MessageFlags.Success;
          response.SetDataAsBytes(BitConverter.GetBytes(count));

          count++;          
          break;

        default:
          Console.WriteLine("Message: {0}, {1}", combo.Message.Type, combo.Message.Flags);
          break;
      }

      //Thread.Sleep(50);

      server.Send(combo.Manager, response);
    }
    
  }

}
