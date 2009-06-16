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
    #region Members

    private byte[] _data;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] private MessageFlags _flags;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] private MessageType _type;

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

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IrssMessage"/> class.
    /// </summary>
    protected IrssMessage()
    {
      _type = MessageType.Unknown;
      _flags = MessageFlags.None;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IrssMessage"/> class.
    /// </summary>
    /// <param name="type">The message type.</param>
    /// <param name="flags">The message flags.</param>
    public IrssMessage(MessageType type, MessageFlags flags)
    {
      _type = type;
      _flags = flags;
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

    #region Implementation

    /// <summary>
    /// Get message data as bytes.
    /// </summary>
    public byte[] GetDataAsBytes()
    {
      return _data;
    }

    /// <summary>
    /// Set message data as bytes.
    /// </summary>
    public void SetDataAsBytes(byte[] data)
    {
      if (data == null)
        _data = null;
      else
        _data = (byte[]) data.Clone();
    }

    /// <summary>
    /// Get message data as string.
    /// </summary>
    public string GetDataAsString()
    {
      if (_data == null)
        return String.Empty;

      return Encoding.ASCII.GetString(_data);
    }

    /// <summary>
    /// Set message data as string.
    /// </summary>
    public void SetDataAsString(string data)
    {
      if (String.IsNullOrEmpty(data))
        _data = null;
      else
        _data = Encoding.ASCII.GetBytes(data);
    }

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

      BitConverter.GetBytes((int) _type).CopyTo(byteArray, 0);
      BitConverter.GetBytes((int) _flags).CopyTo(byteArray, 4);

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

      MessageType type = (MessageType) BitConverter.ToInt32(from, 0);
      MessageFlags flags = (MessageFlags) BitConverter.ToInt32(from, 4);

      if (from.Length == 8)
        return new IrssMessage(type, flags);

      byte[] data = new byte[from.Length - 8];
      Array.Copy(from, 8, data, 0, data.Length);
      return new IrssMessage(type, flags, data);
    }

    #endregion Implementation
  }
}