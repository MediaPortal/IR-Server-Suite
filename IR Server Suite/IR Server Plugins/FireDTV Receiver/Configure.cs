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
using System.Windows.Forms;

namespace IRServer.Plugin
{
  /// <summary>
  /// Configure the FireDTV receiver
  /// </summary>
  internal partial class Configure : Form
  {
    #region variables

    private FireDTVControl fireDTV;
    private string _deviceName;
    #endregion variables

    #region Properties

    /// <summary>
    /// Gets or sets the device name
    /// </summary>
    /// <value>Use channel control</value>
    public string DeviceName
    {
      get { return _deviceName; }
      set
      {
        _deviceName = value;
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
      try
      {
        fireDTV = new FireDTVControl((IntPtr)0);
        labelFireDTV.Text = "FireDTV API Version:"+fireDTV.APIVersion;
        if (fireDTV.OpenDrivers())
        {
          comboBoxDevice.DataSource = fireDTV.SourceFilters;
          comboBoxDevice.DisplayMember = "FriendlyName";
          comboBoxDevice.ValueMember = "Name";
        }
        if (!string.IsNullOrEmpty(_deviceName))
        {
          comboBoxDevice.SelectedValue = _deviceName;
        }
        comboBoxDevice.Enabled = true;
        buttonOK.Enabled = true;
        labelFireDTV.Visible = true;
      }
      catch (Exception e)
      {
        MessageBox.Show(e.ToString());
        labelFireDTV.Visible = false;
        comboBoxDevice.Enabled = false;
        buttonOK.Enabled = false;
      }

    }

    #endregion Constructor

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (comboBoxDevice.SelectedValue != null)
      {
        _deviceName = comboBoxDevice.SelectedValue.ToString();
      }
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