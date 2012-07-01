using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRServer.Plugin
{
  public partial class PhilipsMceUsbIrReceiverSpinelPlus
  {

    public class Config
    {
      private bool _doRepeats;
      private bool _useSystemRatesDelay;
      private int _firstRepeatDelay;
      private int _heldRepeatDelay;

      public bool doRepeats
      {
        get { return _doRepeats; }
        set { _doRepeats = value; }
      }
      public bool useSystemRatesDelay
      {
        get { return _useSystemRatesDelay; }
        set { _useSystemRatesDelay = value; }
      }
      public int firstRepeatDelay
      {
        get { return _firstRepeatDelay; }
        set { _firstRepeatDelay = value; }
      }
      public int heldRepeatDelay
      {
        get { return _heldRepeatDelay; }
        set { _heldRepeatDelay = value; }
      }

      public Config()
      {
        this.reset();
      }

      public bool reset()
      {
        try
        {
          _doRepeats = false;
          _useSystemRatesDelay = true;
          _firstRepeatDelay = 400;
          _heldRepeatDelay = 100;
          return true;
        }
        catch
        {
          return false;
        }
      }
    }

  }
}
