using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LiveDriveReceiver
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


    #endregion Properties

    #region Constructor

    public Configure()
    {
      InitializeComponent();
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
