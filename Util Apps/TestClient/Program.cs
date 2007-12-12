using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using IrssComms;

namespace TestClient
{

  class Program
  {

    static int clientNumber = -1;

    static Client _client;

    static void Main(string[] args)
    {
      ClientMessageSink sink = new ClientMessageSink(Sink);

      IPAddress serverIP = Client.GetIPFromName("localhost");

      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = new WaitCallback(CommsFailure);
      _client.ConnectCallback       = new WaitCallback(Connected);
      _client.DisconnectCallback    = new WaitCallback(Disconnected);
      

      if (_client.Start())
      {
        Console.WriteLine("Client started");
      }
      else
      {
        Console.WriteLine("Couldn't start communications");
        return;
      }

      Console.ReadLine();

      IrssMessage message = new IrssMessage(MessageType.ForwardRemoteEvent, MessageFlags.Request);

      try
      {
        for (int index = 0; index < 10000; index++)
        {
          message.SetDataAsString(index.ToString());
          _client.Send(message);

          Console.WriteLine("ForwardRemoteEvent: {0}", index);

          Thread.Sleep(10);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }

      Console.ReadLine();

      _client.Send(new IrssMessage(MessageType.UnregisterClient, MessageFlags.Request, String.Format("Unregister {0}", clientNumber)));

      Thread.Sleep(1000);

      _client.Dispose();
    }


    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        Console.WriteLine("Communications failure: {0}", ex.Message);
      else
        Console.WriteLine("Communications failure");
    }
    static void Connected(object obj)
    {
      Console.WriteLine("Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      Console.WriteLine("Communications with server has been lost");

      Thread.Sleep(1000);
    }



    static void Sink(IrssMessage message)
    {
      switch (message.Type)
      {
        case MessageType.RegisterClient:
          clientNumber = BitConverter.ToInt32(message.GetDataAsBytes(), 0);
          Console.WriteLine("Registered: {0}, {1}, {2}", message.Type, message.Flags, clientNumber);
          break;

        case MessageType.ForwardRemoteEvent:
          Console.WriteLine("ForwardRemoteEvent: {0}", message.GetDataAsString());
          break;

        default:
          Console.WriteLine("Message: {0}, {1}", message.Type, message.Flags);
          break;
      }
    }

  }

}
