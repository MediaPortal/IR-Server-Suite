using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Remember: Pulse is Positive, Space is Negative.

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// Encapsulates an MCE compatible IR Code.
  /// </summary>
  class IrCode
  {

    #region Constants

    /// <summary>
    /// The carrier frequency for this code is Unknown.
    /// </summary>
    public const int CarrierFrequencyUnknown  = -1;
    /// <summary>
    /// This code does not use a carrier wave.
    /// </summary>
    public const int CarrierFrequencyDCMode   = 0;
    /// <summary>
    /// Default carrier frequency, 36kHz. (This is the carrier frequency for RC5, RC6 and RC-MM
    /// </summary>
    public const int CarrierFrequencyDefault  = 36000;

    /// <summary>
    /// How long the longest IR Code space should be.
    /// </summary>
    const int LongestSpace = -10000;

    #endregion Constants

    #region Member Variables

    int _carrier;
    int[] _timingData;

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
    /// The IR timing data.
    /// </summary>
    public int[] TimingData
    {
      get { return _timingData; }
      set { _timingData = value; }
    }

    #endregion Properties

    #region Constructors

    public IrCode() : this(CarrierFrequencyUnknown, new int[] { })              { }
    public IrCode(int carrier) : this(carrier, new int[] { })                   { }
    public IrCode(int[] timingData) : this(CarrierFrequencyUnknown, timingData) { }
    public IrCode(int carrier, int[] timingData)
    {
      _carrier    = carrier;
      _timingData = timingData;
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Locates the gap between button presses and reduces the data down to just the first press.
    /// </summary>
    /// <returns>true if successful, otherwise false.</returns>
    public bool FinalizeData()
    {
      if (_timingData.Length == 0)
        return false;

      List<int> newData = new List<int>();

      for (int index = 0; index < _timingData.Length; index++)
      {
        int time = _timingData[index];

        newData.Add(time);

        if (time < LongestSpace)
          break;
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
    /// Creates a byte array representation of this IR Code for sending in a message.
    /// </summary>
    /// <returns>Byte array representation.</returns>
    public byte[] ToByteArray()
    {
      ushort[] prontoData = Pronto.ConvertIrCodeToProntoRaw(this);

      StringBuilder output = new StringBuilder();

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
    /// Takes old IR file bytes.
    /// </summary>
    /// <param name="data">IR file bytes.</param>
    /// <returns>New IrCode object.</returns>
    static IrCode FromOldData(byte[] data)
    {
      List<int> timingData = new List<int>();

      int len = 0;

      for (int index = 0; index < data.Length; index++)
      {
        byte curByte = data[index];

        if ((curByte & 0x80) != 0)
          len += (int)(curByte & 0x7F);
        else
          len -= (int)curByte;

        if ((curByte & 0x7F) != 0x7F)
        {
          timingData.Add(len * 50);
          len = 0;
        }
      }

      if (len != 0)
        timingData.Add(len * 50);

      return new IrCode(timingData.ToArray());
    }

    /// <summary>
    /// Create a new IrCode object from byte array data.
    /// </summary>
    /// <param name="data">Byte array to create from.</param>
    /// <returns>New IrCode.</returns>
    public static IrCode FromByteArray(byte[] data)
    {
      if (data[4] != ' ')
        return FromOldData(data);

      string code = Encoding.ASCII.GetString(data);

      string[] stringData = code.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      ushort[] prontoData = new ushort[stringData.Length];
      for (int i = 0; i < stringData.Length; i++)
        prontoData[i] = ushort.Parse(stringData[i], System.Globalization.NumberStyles.HexNumber);

      return Pronto.ConvertProntoDataToIrCode(prontoData);
    }

    #endregion Static Methods

  }

}
