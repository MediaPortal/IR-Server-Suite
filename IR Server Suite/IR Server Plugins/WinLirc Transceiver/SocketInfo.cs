using System.Net.Sockets;

namespace InputService.Plugin
{
  /// <summary>
  /// Class containing information for the data callback function
  /// </summary>
  internal class SocketInfo
  {
    #region Variables

    private readonly byte[] _dataBuffer;
    private readonly Socket _socket;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets the socket.
    /// </summary>
    /// <value>The socket.</value>
    public Socket Sock
    {
      get { return _socket; }
    }

    /// <summary>
    /// Gets the data buffer.
    /// </summary>
    /// <value>The data buffer.</value>
    public byte[] DataBuffer
    {
      get { return _dataBuffer; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketInfo"/> class.
    /// </summary>
    public SocketInfo()
    {
      _dataBuffer = new byte[512];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SocketInfo"/> class.
    /// </summary>
    /// <param name="socket">The socket.</param>
    public SocketInfo(Socket socket) : this()
    {
      _socket = socket;
    }

    #endregion Constructors
  }
}