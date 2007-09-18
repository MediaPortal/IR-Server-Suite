using System;
using System.Collections.Generic;
using System.Text;

namespace IrssComms
{

  #region Enumerations

  public enum MessageType
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
  public enum MessageFlags
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
  /// Message class for passing over network.
  /// </summary>
  public class IrssMessage
  {

    #region Members

    MessageType _type;
    MessageFlags _flags;
    
    byte[] _data;

    #endregion Members

    #region Properties

    /// <summary>
    /// Type of message.
    /// </summary>
    public MessageType Type
    {
      get { return _type; }
      set { _type = value; }
    }

    /// <summary>
    /// Message flags.
    /// </summary>
    public MessageFlags Flags
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
      get
      {
        if (_data == null)
          return String.Empty;
        else
          return Encoding.ASCII.GetString(_data);
      }
      set
      {
        if (String.IsNullOrEmpty(value))
          _data = null;
        else
          _data = Encoding.ASCII.GetBytes(value);
      }
    }

    #endregion Properties

    #region Constructors

    protected IrssMessage()
    {
      _type   = MessageType.Unknown;
      _flags  = MessageFlags.None;
      _data   = null;
    }

    public IrssMessage(MessageType type, MessageFlags flags)
      : this()
    {
      _type   = type;
      _flags  = flags;
    }

    public IrssMessage(MessageType type, MessageFlags flags, byte[] data)
      : this(type, flags)
    {
      _data = data;
    }

    public IrssMessage(MessageType type, MessageFlags flags, string data)
      : this(type, flags)
    {
      _data = Encoding.ASCII.GetBytes(data);
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Turn this Message instance into a byte array.
    /// </summary>
    /// <returns>Byte array representation of this Message instance.</returns>
    public byte[] ToBytes()
    {
      int dataLength = 0;
      if (_data != null)
        dataLength = _data.Length;

      byte[] byteArray = new byte[8 + dataLength];

      BitConverter.GetBytes((int)_type).CopyTo(byteArray, 0);
      BitConverter.GetBytes((int)_flags).CopyTo(byteArray, 4);

      if (_data != null)
        _data.CopyTo(byteArray, 8);

      return byteArray;
    }

    /// <summary>
    /// Create a Pipe Message from the provided byte array.
    /// </summary>
    /// <param name="from">Byte array to generate message from.</param>
    /// <returns>New Message.</returns>
    public static IrssMessage FromBytes(byte[] from)
    {
      if (from == null)
        throw new ArgumentNullException("from");

      if (from.Length < 8)
        throw new ArgumentException("Insufficient bytes to create message", "from");

      MessageType type    = (MessageType)BitConverter.ToInt32(from, 0);
      MessageFlags flags  = (MessageFlags)BitConverter.ToInt32(from, 4);

      if (from.Length == 8)
      {
        return new IrssMessage(type, flags);
      }
      else
      {
        byte[] data = new byte[from.Length - 8];

        Array.Copy(from, 8, data, 0, data.Length);

        return new IrssMessage(type, flags, data);
      }
    }

    #endregion Implementation

  }

}
