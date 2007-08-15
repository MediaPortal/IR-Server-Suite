using System;
using System.Collections.Generic;

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// Encapsulates an MCE compatible IR code.
  /// </summary>
  public class MceIrCode
  {

    #region Constants

    public const int DefaultCarrierFrequency = 38000;

    #endregion Constants

    #region Member Variables

    int _carrier;
    byte[] _data;

    #endregion Member Variables

    #region Properties

    /// <summary>
    /// The IR carrier frequency.
    /// </summary>
    public int Carrier
    {
      get { return _carrier; }
      set { _carrier = value; }
    }

    /// <summary>
    /// The IR data.
    /// </summary>
    public byte[] Data
    {
      get { return _data; }
      set { _data = value; }
    }

    #endregion Properties

    #region Constructors

    public MceIrCode() : this(MceIrCode.DefaultCarrierFrequency, null) { }
    public MceIrCode(int carrier) : this(carrier, null) { }
    public MceIrCode(byte[] data) : this(MceIrCode.DefaultCarrierFrequency, data) { }
    public MceIrCode(int carrier, byte[] data)
    {
      _carrier  = carrier;
      _data     = data;
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Get the packet to set the IR carrier frequency.
    /// </summary>
    /// <returns>The packet to set the IR carrier frequency.</returns>
    public byte[] GetCarrierPacket()
    {
      byte[] carrierPacket = new byte[] { 0x9F, 0x06, 0x01, 0x80 };

      if (_carrier <= 0)
        return carrierPacket;

      for (int scaler = 1; scaler <= 4; scaler++)
      {
        int divisor = (10000000 >> (2 * scaler)) / _carrier;

        if (divisor <= 0xFF)
        {
          carrierPacket[2] = (byte)scaler;
          carrierPacket[3] = (byte)divisor;
          return carrierPacket;
        }
      }
      
      return null;
    }

    /// <summary>
    /// Get the packet data in a transmissible form.
    /// </summary>
    /// <returns>The packet data ready to be sent directly to the IR device.</returns>
    public byte[] GetDataPacket()
    {
      if (_data == null)
        return null;

      int dataIndex = 0;
      List<byte> packet = new List<byte>();

      for (int counter = 0; counter < _data.Length; counter += 4)
      {
        byte copy = (byte)(_data.Length - counter < 4 ? _data.Length - counter : 0x04);

        packet.Add((byte)(0x80 | copy));

        for (int index = 0; index < copy; index++)
          packet.Add(_data[dataIndex + index]);

        dataIndex += copy;
      }

      packet.Add(0x80);

      return packet.ToArray();
    }

    /// <summary>
    /// Determine the time this packet will take to transmit.
    /// </summary>
    /// <returns>The required transmission time in microseconds.</returns>
    public int GetPacketTransmitTime()
    {
      int accTime = 0;

      foreach (byte dataByte in _data)
        accTime += ((dataByte & 0x7F) * 50);

      return accTime;
    }

    #endregion Methods

  }

}
