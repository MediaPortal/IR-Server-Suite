using System;
using System.Collections.Generic;
using System.Text;

namespace IrssUtils
{

  /// <summary>
  /// Holds data on an IR Transceiver Device.
  /// </summary>
  public class TransceiverInfo
  {

    #region Variables

    string[] _ports;
    string[] _speeds;

    string _name;
    bool _canLearn;
    bool _canReceive;
    bool _canTransmit;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Available transmit ports.
    /// </summary>
    public string[] Ports
    {
      get { return _ports; }
      set { _ports = value; }
    }

    /// <summary>
    /// Available transmit speeds.
    /// </summary>
    public string[] Speeds
    {
      get { return _speeds; }
      set { _speeds = value; }
    }

    /// <summary>
    /// Transceiver name.
    /// </summary>
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    /// <summary>
    /// Can this transceiver learn IR Commands.
    /// </summary>
    public bool CanLearn
    {
      get { return _canLearn; }
      set { _canLearn = value; }
    }

    /// <summary>
    /// Can this transceiver receive button presses.
    /// </summary>
    public bool CanReceive
    {
      get { return _canReceive; }
      set { _canReceive = value; }
    }

    /// <summary>
    /// Can this transceiver transmit IR Commands.
    /// </summary>
    public bool CanTransmit
    {
      get { return _canTransmit; }
      set { _canTransmit = value; }
    }

    #endregion Properties

    #region Constructors

    public TransceiverInfo()
    {
      _ports        = new string[] { "None" };
      _speeds       = new string[] { "None" };

      _name         = "None";
      _canLearn     = false;
      _canReceive   = false;
      _canTransmit  = false;
    }

    #endregion Construcors

    #region Static Methods

    /// <summary>
    /// Given a TransceiverInfo object this method returns the byte array representation.
    /// </summary>
    /// <param name="transceiverInfo">TransceiverInfo object to convert.</param>
    /// <returns>Byte array of serialized TransceiverInfo object.</returns>
    public static byte[] ToBytes(TransceiverInfo transceiverInfo)
    {
      try
      {
        int index;

        StringBuilder ports = new StringBuilder();
        for (index = 0; index < transceiverInfo.Ports.Length; index++)
        {
          ports.Append(transceiverInfo.Ports[index]);
          if (index < transceiverInfo.Ports.Length - 1)
            ports.Append(',');
        }

        StringBuilder speeds = new StringBuilder();
        for (index = 0; index < transceiverInfo.Speeds.Length; index++)
        {
          speeds.Append(transceiverInfo.Speeds[index]);
          if (index < transceiverInfo.Speeds.Length - 1)
            speeds.Append(',');
        }

        string data = String.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
          transceiverInfo.Name,           // 0
          transceiverInfo.CanLearn,       // 1
          transceiverInfo.CanReceive,     // 2
          transceiverInfo.CanTransmit,    // 3
          transceiverInfo.Ports.Length,   // 4
          ports.ToString(),               // 5
          transceiverInfo.Speeds.Length,  // 6
          speeds.ToString()               // 7
        );

        return Encoding.ASCII.GetBytes(data);
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// Given a byte array this method returns the deserialized TransceiverInfo object.
    /// </summary>
    /// <param name="dataBytes">Byte array of serialized TransceiverInfo object.</param>
    /// <returns>TransceiverInfo object.</returns>
    public static TransceiverInfo FromBytes(byte[] dataBytes)
    {
      try
      {
        string dataString = Encoding.ASCII.GetString(dataBytes);

        string[] data = dataString.Split(new char[] { ',' });

        TransceiverInfo transceiverInfo = new TransceiverInfo();
        transceiverInfo.Name            = data[0];
        transceiverInfo.CanLearn        = bool.Parse(data[1]);
        transceiverInfo.CanReceive      = bool.Parse(data[2]);
        transceiverInfo.CanTransmit     = bool.Parse(data[3]);

        int index;

        int portIndex = 4;
        int portCount = int.Parse(data[portIndex]);
        transceiverInfo.Ports = new string[portCount];
        for (index = portIndex + 1; index <= portIndex + portCount; index++)
          transceiverInfo.Ports[index - (portIndex + 1)] = data[index];

        int speedIndex = portIndex + portCount + 1;
        int speedCount = int.Parse(data[speedIndex]);
        transceiverInfo.Speeds = new string[speedCount];
        for (index = speedIndex + 1; index <= speedIndex + speedCount; index++)
          transceiverInfo.Speeds[index - (speedIndex + 1)] = data[index];

        return transceiverInfo;
      }
      catch
      {
        return null;
      }
    }

    #endregion Static Methods

  }

}
