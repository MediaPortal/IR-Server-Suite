using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Remember: Pulse is Positive, Space is Negative.

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// Encapsulates an MCE compatible IR code.
  /// </summary>
  public class IrCode
  {

    #region Constants
    
    public const int CarrierFrequencyUnknown    = -1;
    public const int CarrierFrequencyPulseMode  = 0;
    public const int CarrierFrequencyDefault    = 36000;

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

    public bool Finalize()
    {
      if (_timingData.Length == 0)
        return false;

      List<int> newData = new List<int>();

      for (int index = 0; index < _timingData.Length; index++)
      {
        int time = _timingData[index];

        if (time < LongestSpace)
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
    /// Add timing data to this IR code.
    /// </summary>
    /// <param name="addTimingData">Addition timing data.</param>
    public void AddTimingData(int[] addTimingData)
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
        _timingData = new int[addTimingData.Length];
        addTimingData.CopyTo(_timingData, 0);
        return;
      }

      if (addTimingData.Length == 0 || index >= _timingData.Length)
      {
        return;
      }


      if (Math.Sign(addTimingData[0]) == Math.Sign(_timingData[index]))
      {
        newTimingData.Add(_timingData[index] + addTimingData[0]);

        for (index = 1; index < addTimingData.Length; index++)
          newTimingData.Add(addTimingData[index]);
      }
      else
      {
        newTimingData.Add(_timingData[index]);
        newTimingData.AddRange(addTimingData);
      }

      _timingData = newTimingData.ToArray();
    }

    public override string ToString()
    {
      ushort[] prontoData = Pronto.ConvertIrCodeToProntoRaw(this);

      StringBuilder output = new StringBuilder();

      for (int index = 0; index < prontoData.Length; index++)
      {
        output.Append(prontoData[index].ToString("X4"));
        if (index != prontoData.Length - 1)
          output.Append(' ');
      }

      return output.ToString();
    }

    #endregion Methods

    #region Static Methods

    /// <summary>
    /// Takes old IR file bytes.
    /// </summary>
    /// <param name="data">IR file bytes.</param>
    /// <returns>New IrCode object.</returns>
    public static IrCode FromBytes(byte[] data)
    {
      List<int> timingData = new List<int>();

      int len = 0;

      for (int index = 0; index < data.Length; index++)
      {
        byte curByte = data[index];

        if ((curByte & 0x80) != 0)
          len += (int)(curByte & 0x7F);
        else
          len -= (int)(curByte & 0x7F);

        if ((curByte & 0x7F) != 0x7F)
        {
          timingData.Add(len * 50);
          len = 0;
        }
      }

      return new IrCode(timingData.ToArray());
    }

    public static IrCode FromString(string code)
    {
      string[] stringData = code.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      ushort[] prontoData = new ushort[stringData.Length];
      for (int i = 0; i < stringData.Length; i++)
        prontoData[i] = ushort.Parse(stringData[i], System.Globalization.NumberStyles.HexNumber);

      return Pronto.ConvertProntoDataToIrCode(prontoData);
    }

    #endregion Static Methods

  }

}
