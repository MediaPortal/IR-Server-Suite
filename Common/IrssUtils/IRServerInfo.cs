using System;
using System.Collections.Generic;
using System.Text;

namespace IrssUtils
{

  /// <summary>
  /// Holds data on an the current capabilities of the IR Server.
  /// This class is used to pass information about the IR Server's current capabilities on to the clients.
  /// </summary>
  public class IRServerInfo
  {

    #region Variables

    bool _canLearn;
    bool _canReceive;
    bool _canTransmit;

    string[] _ports;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Can the IR Server learn IR Commands.
    /// </summary>
    public bool CanLearn
    {
      get { return _canLearn; }
      set { _canLearn = value; }
    }

    /// <summary>
    /// Can the IR Server receive IR commands.
    /// </summary>
    public bool CanReceive
    {
      get { return _canReceive; }
      set { _canReceive = value; }
    }

    /// <summary>
    /// Can the IR Server transmit IR Commands.
    /// </summary>
    public bool CanTransmit
    {
      get { return _canTransmit; }
      set { _canTransmit = value; }
    }

    /// <summary>
    /// Available IR transmit ports.
    /// </summary>
    public string[] Ports
    {
      get { return _ports; }
      set { _ports = value; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IRServerInfo"/> class.
    /// </summary>
    public IRServerInfo()
    {
      _canLearn     = false;
      _canReceive   = false;
      _canTransmit  = false;

      _ports = new string[] { "None" };
    }

    #endregion Construcors

    #region Methods

    /// <summary>
    /// This method returns the byte array representation of this instance.
    /// </summary>
    /// <returns>Byte array of serialized IRServerInfo object.</returns>
    public byte[] ToBytes()
    {
      try
      {
        StringBuilder ports = new StringBuilder();
        for (int index = 0; index < _ports.Length; index++)
        {
          ports.Append(_ports[index]);
          if (index < _ports.Length - 1)
            ports.Append(',');
        }

        string data = String.Format("{0},{1},{2},{3},{4}",
          _canLearn,        // 0
          _canReceive,      // 1
          _canTransmit,     // 2
          _ports.Length,    // 3
          ports.ToString()  // 4
        );

        return Encoding.ASCII.GetBytes(data);
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// Given a byte array this method returns the deserialized IRServerInfo object.
    /// </summary>
    /// <param name="dataBytes">Byte array of serialized IRServerInfo object.</param>
    /// <returns>IRServerInfo object.</returns>
    public static IRServerInfo FromBytes(byte[] dataBytes)
    {
      try
      {
        string dataString = Encoding.ASCII.GetString(dataBytes);

        string[] data = dataString.Split(new char[] { ',' });

        IRServerInfo irServerInfo = new IRServerInfo();
        irServerInfo.CanLearn     = bool.Parse(data[0]);
        irServerInfo.CanReceive   = bool.Parse(data[1]);
        irServerInfo.CanTransmit  = bool.Parse(data[2]);

        int portIndex = 3;
        int portCount = int.Parse(data[portIndex]);
        irServerInfo.Ports = new string[portCount];
        for (int index = portIndex + 1; index <= portIndex + portCount; index++)
          irServerInfo.Ports[index - (portIndex + 1)] = data[index];

        return irServerInfo;
      }
      catch
      {
        return null;
      }
    }

    #endregion Methods

  }

}
