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

    private bool _canLearn;
    private bool _canReceive;
    private bool _canTransmit;

    private string[] _ports;

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
      _ports = new string[] {"None"};
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
                                    _canLearn, // 0
                                    _canReceive, // 1
                                    _canTransmit, // 2
                                    _ports.Length, // 3
                                    ports // 4
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

        string[] data = dataString.Split(',');

        IRServerInfo irServerInfo = new IRServerInfo();
        irServerInfo.CanLearn = bool.Parse(data[0]);
        irServerInfo.CanReceive = bool.Parse(data[1]);
        irServerInfo.CanTransmit = bool.Parse(data[2]);

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