using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InputService.Plugin
{
  internal partial class DeviceSelect : Form
  {
    #region Variables

    private readonly List<DeviceDetails> _devices;

    private byte _byteMask;
    private int _inputByte;

    private int _repeatDelay;
    private bool _useAllBytes;

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
            foreach (ListViewItem item in listViewDevices.Items)
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

    public int InputByte
    {
      get { return _inputByte; }
      set { _inputByte = value; }
    }

    public byte ByteMask
    {
      get { return _byteMask; }
      set { _byteMask = value; }
    }

    public bool UseAllBytes
    {
      get { return _useAllBytes; }
      set { _useAllBytes = value; }
    }

    public int RepeatDelay
    {
      get { return _repeatDelay; }
      set { _repeatDelay = value; }
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
          listViewDevices.Items.Add(new ListViewItem(new string[] {details.Name, details.ID}));
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
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }


    private void buttonAdvanced_Click(object sender, EventArgs e)
    {
      AdvancedSettings advancedSettings = new AdvancedSettings();
      advancedSettings.InputByte = _inputByte;
      advancedSettings.ByteMask = _byteMask;
      advancedSettings.UseAllBytes = _useAllBytes;
      advancedSettings.RepeatDelay = _repeatDelay;

      if (advancedSettings.ShowDialog(this) == DialogResult.OK)
      {
        _inputByte = advancedSettings.InputByte;
        _byteMask = advancedSettings.ByteMask;
        _useAllBytes = advancedSettings.UseAllBytes;
        _repeatDelay = advancedSettings.RepeatDelay;
      }
    }
  }
}