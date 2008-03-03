using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX.DirectInput;

namespace InputService.Plugin
{

  partial class Configure : Form
  {

    #region Variables

    DirectInputListener _diListener;
    Device _device;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or sets the device GUID.
    /// </summary>
    /// <value>The device GUID.</value>
    public string DeviceGuid
    {
      get
      {
        if (listViewDevices.SelectedItems.Count == 1)
          return listViewDevices.SelectedItems[0].Tag as string;
        else
          return null;
      }
      set
      {
        if (String.IsNullOrEmpty(value))
          return;

        foreach (ListViewItem item in listViewDevices.Items)
        {
          if (value.Equals(item.Tag as string, StringComparison.OrdinalIgnoreCase))
          {
            item.Selected = true;
            break;
          }
        }
      }
    }

    #endregion Properties

    #region Constructor

    public Configure(DeviceList deviceList)
    {
      InitializeComponent();

      _diListener = new DirectInputListener();
      _diListener.Delay = 150;

      foreach (DeviceInstance di in deviceList)
      {
        ListViewItem item = new ListViewItem(
          new string[] { di.InstanceName, di.ProductName }
        );

        item.Tag = di.InstanceGuid.ToString();

        listViewDevices.Items.Add(item);
      }

      
    }

    #endregion Constructor

    private void Configure_FormClosed(object sender, FormClosedEventArgs e)
    {
      _diListener.DeInitDevice();
      _diListener = null;
    }
    
    private void listViewDevices_SelectedIndexChanged(object sender, EventArgs e)
    {
      string guid = this.DeviceGuid;

      if (String.IsNullOrEmpty(guid))
        return;

      Guid deviceGuid = new Guid(guid);

      _diListener.DeInitDevice();
      _diListener.InitDevice(deviceGuid);

      _device = _diListener.SelectedDevice;
    }

    #region Buttons

    private void buttonConfigure_Click(object sender, EventArgs e)
    {
      if (_device != null)
        _device.RunControlPanel();
    }

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

    private void groupBoxMouseButtons_Enter(object sender, EventArgs e)
    {

    }



  }

}
