using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{

  partial class Configure : Form
  {

    #region Interop

    [DllImport("winmm.dll")]
    static extern int midiInGetDevCaps(int uDeviceID, ref MidiInCaps lpCaps, int uSize);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct MidiInCaps
    {
      public const int MAXPNAMELEN = 32;
      
      public Int16 wMid;
      public Int16 wPid;
      public Int32 vDriverVersion;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXPNAMELEN)]
      public byte[] szPname;
      public Int32 dwSupport;
    }

    #endregion Structures


    #region Properties

    public int DeviceIndex
    {
      get { return comboBoxDevice.SelectedIndex; }
      set { comboBoxDevice.SelectedIndex = value; }
    }

    #endregion Properties

    #region Constructor

    public Configure()
    {
      InitializeComponent();
/*
      MidiInCaps midiInCaps = new MidiInCaps();
      ASCIIEncoding encoder = new ASCIIEncoding();

      for (int i = 0; i < m_lirPlugin.MIDIDeviceCount; i++)
      {
        if (midiInGetDevCaps(i, ref midiInCaps, Marshal.SizeOf(typeof(MidiInCaps))) == 0)
        {
          string strName = encoder.GetString(midiInCaps.szPname);
          int intNullIndex = strName.IndexOf((char)0);
          strName = strName.Remove(intNullIndex, MidiInCaps.MAXPNAMELEN - intNullIndex);
          cbxDevices.Items.Add(new MIDIDevice(strName, i));
        }
      }
      */
    }

    #endregion Constructor

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    #endregion Buttons

  }

}
