using System;
using System.Collections.Generic;
using System.Text;

namespace NamedPipes
{

  #region Enumerations

  public enum PipeMessageType
  {
    Unknown,

    RegisterClient,
    UnregisterClient,

    RegisterRepeater,
    UnregisterRepeater,

    LearnIR,
    BlastIR,

    Error,

    Ping,
    Echo,

    ServerShutdown,

    RemoteEvent,
    KeyboardEvent,
    MouseEvent,

    ForwardRemoteEvent,
    ForwardKeyboardEvent,
    ForwardMouseEvent,
  }

  [Flags]
  public enum PipeMessageFlags
  {
    None            = 0x0000,
    
    Request         = 0x0001,
    Response        = 0x0002,
    Notify          = 0x0004,
    
    Success         = 0x0008,
    Failure         = 0x0010,
    Timeout         = 0x0020,
    //Error           = 0x0040,

    //DataString      = 0x0080,
    //DataBytes       = 0x0100,

    //ForceRespond    = 0x0200,
    ForceNotRespond = 0x0400,
  }

  #endregion Enumerations

  /// <summary>
  /// Message class for passing through named pipes.
  /// </summary>
  public class PipeMessage
  {

    #region Members

    string _fromServer;
    string _fromPipe;
    
    PipeMessageType _type;
    PipeMessageFlags _flags;
    
    byte[] _data;

    #endregion Members

    #region Properties

    /// <summary>
    /// Name of sending server.
    /// </summary>
    public string FromServer
    {
      get { return _fromServer; }
      set { _fromServer = value; }
    }

    /// <summary>
    /// Name of sending pipe.
    /// </summary>
    public string FromPipe
    {
      get { return _fromPipe; }
      set { _fromPipe = value; }
    }
    
    /// <summary>
    /// Type of message.
    /// </summary>
    public PipeMessageType Type
    {
      get { return _type; }
      set { _type = value; }
    }

    /// <summary>
    /// Message flags.
    /// </summary>
    public PipeMessageFlags Flags
    {
      get { return _flags; }
      set { _flags = value; }
    }

    /// <summary>
    /// Message data as bytes.
    /// </summary>
    public byte[] DataAsBytes
    {
      get { return _data; }
      set { _data = value; }
    }

    /// <summary>
    /// Message data as string.
    /// </summary>
    public string DataAsString
    {
      get { return Encoding.ASCII.GetString(_data); }
      set { _data = Encoding.ASCII.GetBytes(value); }
    }

    #endregion Properties

    #region Constructors

    PipeMessage()
    {
      _fromServer = String.Empty;
      _fromPipe   = String.Empty;
      _type       = PipeMessageType.Unknown;
      _flags      = PipeMessageFlags.None;
      _data       = null;
    }

    PipeMessage(string fromServer, string fromPipe, PipeMessageType type)
      : this()
    {
      _fromServer = fromServer;
      _fromPipe   = fromPipe;
      _type       = type;
    }

    public PipeMessage(string fromServer, string fromPipe, PipeMessageType type, PipeMessageFlags flags)
      : this(fromServer, fromPipe, type)
    {
      _flags = flags;
    }

    public PipeMessage(string fromServer, string fromPipe, PipeMessageType type, PipeMessageFlags flags, byte[] data)
      : this(fromServer, fromPipe, type)
    {
      _flags  = flags;
      _data   = data;
    }

    public PipeMessage(string fromServer, string fromPipe, PipeMessageType type, PipeMessageFlags flags, string data)
      : this(fromServer, fromPipe, type)
    {
      _flags  = flags;
      _data   = Encoding.ASCII.GetBytes(data);
    }

    #endregion Constructors

    public override string ToString()
    {
      string data = String.Empty;
      if (_data != null && _data.Length != 0)
        data = ByteArrayToByteString(_data);

      string messageType  = ((int)_type).ToString();
      string flags        = ((int)_flags).ToString();

      return String.Format(
        "{0},{1},{2},{3},{4}",
        _fromServer,
        _fromPipe,
        messageType,
        flags,
        data
      );
    }

    /// <summary>
    /// Create a Pipe Message from the provided string.
    /// </summary>
    /// <param name="from">String to generate message from.</param>
    /// <returns>New Pipe Message.</returns>
    public static PipeMessage FromString(string from)
    {
      try
      {
        if (String.IsNullOrEmpty(from))
          return null;

        string[] stringItems = from.Split(new char[] { ',' }, StringSplitOptions.None);

        if (stringItems.Length != 5)
          return null;

        PipeMessageType type    = (PipeMessageType)int.Parse(stringItems[2]);
        PipeMessageFlags flags  = (PipeMessageFlags)int.Parse(stringItems[3]);

        if (String.IsNullOrEmpty(stringItems[4]))
        {
          return new PipeMessage(stringItems[0], stringItems[1], type, flags);
        }
        else
        {
          byte[] data = ByteStringToByteArray(stringItems[4]);

          return new PipeMessage(stringItems[0], stringItems[1], type, flags, data);
        }
      }
      catch
      {
        return null;
      }
    }

    static string ByteArrayToByteString(byte[] data)
    {
      if (data == null || data.Length == 0)
        throw new ArgumentNullException("data");

      StringBuilder outputString = new StringBuilder(2 * data.Length);

      foreach (byte b in data)
        outputString.Append(b.ToString("X2"));

      return outputString.ToString();
    }

    static byte[] ByteStringToByteArray(string data)
    {
      if (String.IsNullOrEmpty(data))
        throw new ArgumentNullException("data");

      List<byte> byteArray = new List<byte>(data.Length / 2);

      for (int index = 0; index < data.Length; index += 2)
      {
        byte byteValue = byte.Parse(data.Substring(index, 2), System.Globalization.NumberStyles.HexNumber);

        byteArray.Add(byteValue);
      }

      return byteArray.ToArray();
    }

  }

}
