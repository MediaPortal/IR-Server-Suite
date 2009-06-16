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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{
  internal partial class Configure : Form
  {
    #region Interop

    [DllImport("winmm.dll")]
    private static extern int midiInGetDevCaps(int uDeviceID, ref MidiInCaps lpCaps, int uSize);

    #endregion Interop

    #region Structures

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private struct MidiInCaps
    {
      public const int MAXPNAMELEN = 32;

      public Int16 wMid;
      public Int16 wPid;
      public Int32 vDriverVersion;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXPNAMELEN)] public byte[] szPname;
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
      get { return comboBoxDevice.SelectedIndex; }
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
        if (midiInGetDevCaps(i, ref midiInCaps, Marshal.SizeOf(typeof (MidiInCaps))) == 0)
        {
          string strName = encoder.GetString(midiInCaps.szPname);
          int intNullIndex = strName.IndexOf((char) 0);
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
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion Buttons
  }
}