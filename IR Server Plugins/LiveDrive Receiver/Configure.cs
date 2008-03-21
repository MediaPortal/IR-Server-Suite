using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    /// <summary>
    /// Gets or sets the index of the device.
    /// </summary>
    /// <value>The index of the device.</value>
    public int DeviceIndex
    {
      get
      {
        return comboBoxDevice.SelectedIndex;
      }
      set
      {
        if (comboBoxDevice.Items.Count > value)
          comboBoxDevice.SelectedIndex = value;
      }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Configure"/> class.
    /// </summary>
    public Configure()
    {
      InitializeComponent();

      // TODO: Finish the LiveDrive support!
      
      MidiInCaps midiInCaps = new MidiInCaps();
      ASCIIEncoding encoder = new ASCIIEncoding();

      for (int i = 0; i < LiveDriveReceiver.midiInGetNumDevs(); i++)
      {
        if (midiInGetDevCaps(i, ref midiInCaps, Marshal.SizeOf(typeof(MidiInCaps))) == 0)
        {
          string strName = encoder.GetString(midiInCaps.szPname);
          int intNullIndex = strName.IndexOf((char)0);
          strName = strName.Remove(intNullIndex, MidiInCaps.MAXPNAMELEN - intNullIndex);

          comboBoxDevice.Items.Add(strName);

          //cbxDevices.Items.Add(new MIDIDevice(strName, i));
        }
      }
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
