using System;

namespace IRServer
{

  class Client
  {

    #region Variables

    string _pipe;
    string _server;
    
    #endregion

    #region Properties

    public string Pipe
    {
      get { return _pipe; }
    }
    public string Server
    {
      get { return _server; }
    }

    #endregion

    #region Constructor

    public Client(string pipe, string server)
    {
      _pipe = pipe;
      _server = server;
    }

    #endregion Constructor

  }

}
