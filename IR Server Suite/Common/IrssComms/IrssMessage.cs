#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IrssComms
{
  #region Enumerations

  /// <summary>
  /// Type of message.
  /// </summary>
  public enum MessageType
  {
    /// <summary>
    /// Unknown message type.
    /// </summary>
    Unknown,

    /// <summary>
    /// Register Client.
    /// </summary>
    RegisterClient,
    /// <summary>
    /// Unregister Client.
    /// </summary>
    UnregisterClient,

    /// <summary>
    /// Register Repeater.
    /// </summary>
    RegisterRepeater,
    /// <summary>
    /// Unregister Repeater.
    /// </summary>
    UnregisterRepeater,

    /// <summary>
    /// Learn IR Command.
    /// </summary>
    LearnIR,
    /// <summary>
    /// Blast IR Command.
    /// </summary>
    BlastIR,

    /// <summary>
    /// Error.
    /// </summary>
    Error,

    /// <summary>
    /// Server Shutdown.
    /// </summary>
    ServerShutdown,
    /// <summary>
    /// Server Suspend.
    /// </summary>
    ServerSuspend,
    /// <summary>
    /// Server Resume
    /// </summary>
    ServerResume,

    /// <summary>
    /// Remote Event.
    /// </summary>
    RemoteEvent,
    /// <summary>
    /// Keyboard Event.
    /// </summary>
    KeyboardEvent,
    /// <summary>
    /// Mouse Event.
    /// </summary>
    MouseEvent,

    /// <summary>
    /// Forward a Remote Event.
    /// </summary>
    ForwardRemoteEvent,
    /// <summary>
    /// Forward a Keyboard Event.
    /// </summary>
    ForwardKeyboardEvent,
    /// <summary>
    /// Forward a Mouse Event.
    /// </summary>
    ForwardMouseEvent,

    /// <summary>
    /// Available Receivers.
    /// </summary>
    AvailableReceivers,
    /// <summary>
    /// Available Blasters.
    /// </summary>
    AvailableBlasters,
    /// <summary>
    /// Active Receivers.
    /// </summary>
    ActiveReceivers,
    /// <summary>
    /// Active Blasters.
    /// </summary>
    ActiveBlasters,
    /// <summary>
    /// Detected Receivers.
    /// </summary>
    DetectedReceivers,
    /// <summary>
    /// Detected Blasters.
    /// </summary>
    DetectedBlasters,
  }

  /// <summary>
  /// Flags to determine more information about the message.
  /// </summary>
  [Flags]
  public enum MessageFlags
  {
    /// <summary>
    /// No Flags.
    /// </summary>
    None = 0x0000,

    /// <summary>
    /// Message is a Request.
    /// </summary>
    Request = 0x0001,
    /// <summary>
    /// Message is a Response to a received Message.
    /// </summary>
    Response = 0x0002,
    /// <summary>
    /// Message is a Notification.
    /// </summary>
    Notify = 0x0004,

    /// <summary>
    /// Operation Success.
    /// </summary>
    Success = 0x0008,
    /// <summary>
    /// Operation Failure.
    /// </summary>
    Failure = 0x0010,
    /// <summary>
    /// Operation Time-Out.
    /// </summary>
    Timeout = 0x0020,

    //Error           = 0x0040,

    //DataString      = 0x0080,
    //DataBytes       = 0x0100,

    //ForceRespond    = 0x0200,

    /// <summary>
    /// Force the recipient not to respond.
    /// </summary>
    ForceNotRespond = 0x0400,
  }

  #endregion Enumerations

  /// <summary>
  /// Message class for passing over network.
  /// </summary>
  [DebuggerDisplay("Type={Type}, Flags={Flags}, Data={GetDataAsString()}")]
  public class IrssMessage
  {
    #region Const

    public const string DATA = "DATA";

    // remote
    public const string DEVICE_NAME = "DEVICE_NAME";
    public const string KEY_CODE = "KEY_CODE";

    // keyboard
    public const string V_KEY = "V_KEY";
    public const string KEY_UP = "KEY_UP";

    // mouse
    public const string DELTA_X = "DELTA_X";
    public const string DELTA_Y = "DELTA_Y";
    public const string BUTTONS = "BUTTONS";

    #endregion

    #region Protected fields

    protected MessageType _messageType;
    protected MessageFlags _messageFlags;
    protected IDictionary<string, object> _messageData = new Dictionary<string, object>();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IrssMessage"/> class.
    /// </summary>
    protected IrssMessage()
    {
      _messageType = MessageType.Unknown;
      _messageFlags = MessageFlags.None;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IrssMessage"/> class.
    /// </summary>
    /// <param name="type">The message type.</param>
    /// <param name="flags">The message flags.</param>
    public IrssMessage(MessageType type, MessageFlags flags)
    {
      _messageType = type;
      _messageFlags = flags;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IrssMessage"/> class.
    /// </summary>
    /// <param name="type">The message type.</param>
    /// <param name="flags">The message flags.</param>
    /// <param name="data">The message data.</param>
    public IrssMessage(MessageType type, MessageFlags flags, byte[] data)
      : this(type, flags)
    {
      SetDataAsBytes(data);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IrssMessage"/> class.
    /// </summary>
    /// <param name="type">The message type.</param>
    /// <param name="flags">The message flags.</param>
    /// <param name="data">The message data.</param>
    public IrssMessage(MessageType type, MessageFlags flags, string data)
      : this(type, flags)
    {
      SetDataAsString(data);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the type of this message.
    /// </summary>
    public MessageType Type
    {
      get { return _messageType; }
    }

    /// <summary>
    /// Message flags.
    /// </summary>
    public MessageFlags Flags
    {
      get { return _messageFlags; }
      set { _messageFlags = value; }
    }

    /// <summary>
    /// Gets or sets the message data. The message data is a generic dictionary special
    /// data entries defined by the message queue.
    /// </summary>
    public IDictionary<string, object> MessageData
    {
      get { return _messageData; }
      set { _messageData = value; }
    }

    #endregion Properties

    #region Implementation

    /// <summary>
    /// Get message data as bytes.
    /// </summary>
    public byte[] GetDataAsBytes()
    {
      if (!_messageData.ContainsKey(DATA)) return null;

      return _messageData[DATA] as byte[];
    }

    /// <summary>
    /// Set message data as bytes.
    /// </summary>
    public void SetDataAsBytes(byte[] data)
    {
      if (data == null)
        _messageData[DATA] = null;
      else
        _messageData[DATA] = (byte[])data.Clone();

      SetMessageData();
    }

    /// <summary>
    /// Get message data as string.
    /// </summary>
    public string GetDataAsString()
    {
      if (GetDataAsBytes() == null) return string.Empty;

      return Encoding.ASCII.GetString(GetDataAsBytes());
    }

    /// <summary>
    /// Set message data as string.
    /// </summary>
    public void SetDataAsString(string data)
    {
      if (String.IsNullOrEmpty(data))
        _messageData[DATA] = null;
      else
        _messageData[DATA] = Encoding.ASCII.GetBytes(data);

      SetMessageData();
    }

    /// <summary>
    /// Converts data for some messages from bytes to data dictionary for easier reading the values.
    /// </summary>
    private void SetMessageData()
    {
      switch (_messageType)
      {
        case MessageType.RemoteEvent:
        case MessageType.ForwardRemoteEvent:
          {
            byte[] data = GetDataAsBytes();

            int deviceNameSize = BitConverter.ToInt32(data, 0);
            _messageData[DEVICE_NAME] = System.Text.Encoding.ASCII.GetString(data, 4, deviceNameSize);

            int keyCodeSize = BitConverter.ToInt32(data, 4 + deviceNameSize);
            _messageData[KEY_CODE] = System.Text.Encoding.ASCII.GetString(data, 8 + deviceNameSize, keyCodeSize);
          }
          break;

        case MessageType.KeyboardEvent:
        case MessageType.ForwardKeyboardEvent:
          {
            byte[] data = GetDataAsBytes();

            _messageData[V_KEY] = BitConverter.ToInt32(data, 0);
            _messageData[KEY_UP] = BitConverter.ToBoolean(data, 4);
          }
          break;

        case MessageType.MouseEvent:
        case MessageType.ForwardMouseEvent:
          {
            byte[] data = GetDataAsBytes();

            _messageData[DELTA_X] = BitConverter.ToInt32(data, 0);
            _messageData[DELTA_Y] = BitConverter.ToInt32(data, 4);
            _messageData[BUTTONS] = BitConverter.ToInt32(data, 8);
          }
          break;
      }
    }


    /// <summary>
    /// Turn this Message instance into a byte array.
    /// </summary>
    /// <returns>Byte array representation of this Message instance.</returns>
    public byte[] ToBytes()
    {
      int dataLength = 0;
      if (GetDataAsBytes() != null)
        dataLength = GetDataAsBytes().Length;

      byte[] byteArray = new byte[8 + dataLength];

      BitConverter.GetBytes((int)_messageType).CopyTo(byteArray, 0);
      BitConverter.GetBytes((int)_messageFlags).CopyTo(byteArray, 4);

      if (GetDataAsBytes() != null)
        GetDataAsBytes().CopyTo(byteArray, 8);

      return byteArray;
    }

    /// <summary>
    /// Create a Pipe Message from the provided byte array.
    /// </summary>
    /// <param name="from">Byte array to generate message from.</param>
    /// <returns>New Message.</returns>
    internal static IrssMessage FromBytes(byte[] from)
    {
      if (from == null)
        throw new ArgumentNullException("from");

      if (from.Length < 8)
        throw new ArgumentException("Insufficient bytes to create message", "from");

      MessageType type = (MessageType)BitConverter.ToInt32(from, 0);
      MessageFlags flags = (MessageFlags)BitConverter.ToInt32(from, 4);

      if (from.Length == 8)
        return new IrssMessage(type, flags);

      byte[] data = new byte[from.Length - 8];
      Array.Copy(from, 8, data, 0, data.Length);

      return new IrssMessage(type, flags, data);
    }

    public static byte[] EncodeRemoteEventData(string deviceName, string keyCode)
    {
      byte[] deviceNameBytes = Encoding.ASCII.GetBytes(deviceName);
      byte[] keyCodeBytes = Encoding.ASCII.GetBytes(keyCode);

      byte[] bytes = new byte[8 + deviceNameBytes.Length + keyCodeBytes.Length];

      BitConverter.GetBytes(deviceNameBytes.Length).CopyTo(bytes, 0);
      deviceNameBytes.CopyTo(bytes, 4);
      BitConverter.GetBytes(keyCodeBytes.Length).CopyTo(bytes, 4 + deviceNameBytes.Length);
      keyCodeBytes.CopyTo(bytes, 8 + deviceNameBytes.Length);

      return bytes;
    }

    public static byte[] EncodeKeyboardEventData(int vKey, bool keyUp)
    {
      byte[] bytes = new byte[8];

      BitConverter.GetBytes(vKey).CopyTo(bytes, 0);
      BitConverter.GetBytes(keyUp).CopyTo(bytes, 4);

      return bytes;
    }

    public static byte[] EncodeMouseEventData(int deltaX, int deltaY, int buttons)
    {
      byte[] bytes = new byte[12];

      BitConverter.GetBytes(deltaX).CopyTo(bytes, 0);
      BitConverter.GetBytes(deltaY).CopyTo(bytes, 4);
      BitConverter.GetBytes(buttons).CopyTo(bytes, 8);

      return bytes;
    }

    #endregion Implementation
  }
}