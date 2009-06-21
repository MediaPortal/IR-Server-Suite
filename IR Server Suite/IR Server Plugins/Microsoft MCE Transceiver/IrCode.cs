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
using System.Globalization;
using System.Text;

// Remember: Pulse is Positive, Space is Negative.

namespace InputService.Plugin
{
  /// <summary>
  /// Encapsulates an MCE compatible IR Code.
  /// </summary>
  internal class IrCode
  {
    #region Constants

    /// <summary>
    /// This code does not use a carrier wave.
    /// </summary>
    public const int CarrierFrequencyDCMode = 0;

    /// <summary>
    /// Default carrier frequency, 36kHz (the carrier frequency for RC5, RC6 and RC-MM).
    /// </summary>
    public const int CarrierFrequencyDefault = 36000;

    /// <summary>
    /// The carrier frequency for this code is Unknown.
    /// </summary>
    public const int CarrierFrequencyUnknown = -1;

    /// <summary>
    /// How long the longest IR Code space should be (in microseconds).
    /// </summary>
    private const int LongestSpace = -75000;

    #endregion Constants

    #region Member Variables

    private int _carrier;
    private int[] _timingData;

    #endregion Member Variables

    #region Properties

    /// <summary>
    /// Gets or Sets the IR carrier frequency.
    /// </summary>
    public int Carrier
    {
      get { return _carrier; }
      set { _carrier = value; }
    }

    /// <summary>
    /// Gets or Sets the IR timing data.
    /// </summary>
    public int[] TimingData
    {
      get { return _timingData; }
      set { _timingData = value; }
    }

    #endregion Properties

    #region Constructors

    public IrCode() : this(CarrierFrequencyUnknown, new int[] {})
    {
    }

    public IrCode(int carrier) : this(carrier, new int[] {})
    {
    }

    public IrCode(int[] timingData) : this(CarrierFrequencyUnknown, timingData)
    {
    }

    public IrCode(int carrier, int[] timingData)
    {
      _carrier = carrier;
      _timingData = timingData;
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Locates the gap between button presses and reduces the data down to just the first press.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool FinalizeData()
    {
      if (_timingData.Length == 0)
        return false;

      // Find long spaces and trim the IR code ...
      List<int> newData = new List<int>();
      foreach (int time in _timingData)
      {
        if (time <= LongestSpace)
        {
          newData.Add(LongestSpace);
          break;
        }
        else
        {
          newData.Add(time);
        }
      }

      _timingData = newData.ToArray();
      return true;
    }

    /// <summary>
    /// Add timing data to this IR Code.
    /// </summary>
    /// <param name="timingData">Addition timing data.</param>
    public void AddTimingData(int[] timingData)
    {
      List<int> newTimingData = new List<int>();

      int index = 0;

      if (_timingData.Length > 1)
      {
        for (index = 0; index < _timingData.Length - 1; index++)
          newTimingData.Add(_timingData[index]);
      }
      else if (_timingData.Length == 0)
      {
        _timingData = new int[timingData.Length];
        timingData.CopyTo(_timingData, 0);
        return;
      }

      if (timingData.Length == 0 || index >= _timingData.Length)
        return;

      if (Math.Sign(timingData[0]) == Math.Sign(_timingData[index]))
      {
        newTimingData.Add(_timingData[index] + timingData[0]);

        for (index = 1; index < timingData.Length; index++)
          newTimingData.Add(timingData[index]);
      }
      else
      {
        newTimingData.Add(_timingData[index]);
        newTimingData.AddRange(timingData);
      }

      _timingData = newTimingData.ToArray();
    }

    /// <summary>
    /// Creates a byte array representation of this IR Code.
    /// </summary>
    /// <returns>Byte array representation (internally it is in Pronto format).</returns>
    public byte[] ToByteArray()
    {
      StringBuilder output = new StringBuilder();

      ushort[] prontoData = Pronto.ConvertIrCodeToProntoRaw(this);

      for (int index = 0; index < prontoData.Length; index++)
      {
        output.Append(prontoData[index].ToString("X4"));
        if (index != prontoData.Length - 1)
          output.Append(' ');
      }

      return Encoding.ASCII.GetBytes(output.ToString());
    }

    #endregion Methods

    #region Static Methods

    /// <summary>
    /// Creates an IrCode object from old IR file bytes.
    /// </summary>
    /// <param name="data">IR file bytes.</param>
    /// <returns>New IrCode object.</returns>
    private static IrCode FromOldData(byte[] data)
    {
      List<int> timingData = new List<int>();

      int len = 0;

      for (int index = 0; index < data.Length; index++)
      {
        byte curByte = data[index];

        if ((curByte & 0x80) != 0)
          len += (curByte & 0x7F);
        else
          len -= curByte;

        if ((curByte & 0x7F) != 0x7F)
        {
          timingData.Add(len * 50);
          len = 0;
        }
      }

      if (len != 0)
        timingData.Add(len * 50);

      IrCode newCode = new IrCode(timingData.ToArray());
      newCode.FinalizeData();
      // Seems some old files have excessively long delays in them .. this might fix that problem ...

      return newCode;
    }

    /// <summary>
    /// Creates an IrCode object from Pronto format file bytes.
    /// </summary>
    /// <param name="data">IR file bytes.</param>
    /// <returns>New IrCode object.</returns>
    private static IrCode FromProntoData(byte[] data)
    {
      string code = Encoding.ASCII.GetString(data);

      string[] stringData = code.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

      ushort[] prontoData = new ushort[stringData.Length];
      for (int i = 0; i < stringData.Length; i++)
        prontoData[i] = ushort.Parse(stringData[i], NumberStyles.HexNumber);

      IrCode newCode = Pronto.ConvertProntoDataToIrCode(prontoData);
      if (newCode != null)
        newCode.FinalizeData();
      // Seems some old files have excessively long delays in them .. this might fix that problem ...

      return newCode;
    }

    /// <summary>
    /// Create a new IrCode object from byte array data.
    /// </summary>
    /// <param name="data">Byte array to create from.</param>
    /// <returns>New IrCode object.</returns>
    public static IrCode FromByteArray(byte[] data)
    {
      if (data[4] == ' ')
        return FromProntoData(data);
      else
        return FromOldData(data);
    }

    #endregion Static Methods
  }
}