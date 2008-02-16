using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{
  
  internal partial class DeviceSelect : Form
  {

    #region Variables

    List<DeviceDetails> _devices;

    #endregion Variables

    #region Properties

    public RawInput.RAWINPUTDEVICE SelectedDevice
    {
      get
      {
        if (listViewDevices.SelectedItems.Count == 1)
          foreach (DeviceDetails details in _devices)
          {
            if (details.ID.Equals(listViewDevices.SelectedItems[0].SubItems[1].Text, StringComparison.Ordinal))
            {
              RawInput.RAWINPUTDEVICE device = new RawInput.RAWINPUTDEVICE();
              device.usUsagePage = details.UsagePage;
              device.usUsage = details.Usage;
              return device;
            }
          }

        return new RawInput.RAWINPUTDEVICE();
      }

      set
      {
        listViewDevices.SelectedItems.Clear();

        foreach (DeviceDetails details in _devices)
        {
          if (details.Usage == value.usUsage && details.UsagePage == value.usUsagePage)
          {
            foreach(ListViewItem item in listViewDevices.Items)
            {
              if (details.ID.Equals(item.SubItems[1].Text, StringComparison.Ordinal))
              {
                item.Selected = true;
                return;
              }
            }
            return;
          }
        }
      }
    }

    #endregion Properties

    #region Constructors

    public DeviceSelect()
    {
      InitializeComponent();

      _devices = new List<DeviceDetails>();

      try
      {
        _devices = RawInput.EnumerateDevices();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      if (_devices.Count > 0)
      {
        foreach (DeviceDetails details in _devices)
        {
          listViewDevices.Items.Add(new ListViewItem(new string[] { details.Name, details.ID }));
        }
      }
    }

    public DeviceSelect(string deviceID) : this()
    {
      for (int index = 0; index < listViewDevices.Items.Count; index++)
      {
//        MessageBox.Show(listViewDevices.Items[index].SubItems[1].Text);
        if (listViewDevices.Items[index].SubItems[1].Text.Equals(deviceID, StringComparison.OrdinalIgnoreCase))
        {
          listViewDevices.Items[index].Selected = true;
          break;
        }
      }
    }

    #endregion Constructors

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


    private void buttonAdvanced_Click(object sender, EventArgs e)
    {
      AdvancedSettings advancedSettings = new AdvancedSettings();
      if (advancedSettings.ShowDialog(this) == DialogResult.OK)
      {

      }

    }



  }

}
