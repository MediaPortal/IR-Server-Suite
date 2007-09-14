using System;

namespace IRServer
{

  struct Client
  {

    public string Pipe;
    public string Server;
    
    public Client(string pipe, string server)
    {
      Pipe    = pipe;
      Server  = server;
    }

  }

}
