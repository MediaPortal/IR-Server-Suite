using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

namespace CustomHIDReceiver
{

  struct DeviceDetails
  {
    public string Name;
    public string ID;
  }

  public partial class DeviceSelect : Form
  {

    #region Variables

    List<DeviceDetails> _devices;

    #endregion Variables

    #region Properties

    public string DeviceID
    {
      get
      {
        if (listViewDevices.SelectedItems.Count == 1)
          return listViewDevices.SelectedItems[0].SubItems[1].Text;
        else
          return String.Empty;
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
        FindHIDDevices();
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
        if (listViewDevices.Items[index].SubItems[1].Text.Equals(deviceID, StringComparison.InvariantCultureIgnoreCase))
        {
          listViewDevices.Items[index].Selected = true;
          break;
        }
      }
    }

    #endregion Constructors

    void FindHIDDevices()
    {
      uint deviceCount = 0;
      int dwSize = (Marshal.SizeOf(typeof(NativeMethods.RAWINPUTDEVICELIST)));

      // Get the number of raw input devices in the list,
      // then allocate sufficient memory and get the entire list
      if (NativeMethods.GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
      {
        IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
        NativeMethods.GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

        // Iterate through the list, discarding undesired items
        // and retrieving further information on keyboard devices
        for (int i = 0; i < deviceCount; i++)
        {
          string deviceName;
          uint pcbSize = 0;

          NativeMethods.RAWINPUTDEVICELIST rid = (NativeMethods.RAWINPUTDEVICELIST)Marshal.PtrToStructure(
                                     new IntPtr((pRawInputDeviceList.ToInt32() + (dwSize * i))),
                                     typeof(NativeMethods.RAWINPUTDEVICELIST));

          NativeMethods.GetRawInputDeviceInfo(rid.hDevice, NativeMethods.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

          if (pcbSize > 0)
          {
            IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
            NativeMethods.GetRawInputDeviceInfo(rid.hDevice, NativeMethods.RIDI_DEVICENAME, pData, ref pcbSize);
            deviceName = (string)Marshal.PtrToStringAnsi(pData);

            // Drop the "root" keyboard and mouse devices used for Terminal 
            // Services and the Remote Desktop
            if (deviceName.ToUpperInvariant().Contains("ROOT"))
              continue;

            // Get Detailed Info ...
            uint size = (uint)Marshal.SizeOf(typeof(NativeMethods.DeviceInfo));
            NativeMethods.DeviceInfo di = new NativeMethods.DeviceInfo();
            di.Size = Marshal.SizeOf(typeof(NativeMethods.DeviceInfo));
            NativeMethods.GetRawInputDeviceInfo(rid.hDevice, NativeMethods.RIDI_DEVICEINFO, ref di, ref size);

            di = new NativeMethods.DeviceInfo();
            di.Size = Marshal.SizeOf(typeof(NativeMethods.DeviceInfo));
            NativeMethods.GetRawInputDeviceInfo(rid.hDevice, NativeMethods.RIDI_DEVICEINFO, ref di, ref size);

            DeviceDetails deviceDetails = new DeviceDetails();
            switch (di.Type)
            {
              case NativeMethods.RawInputType.HID:
                {
                  string vidAndPid = String.Format("Vid_{0:x4}&Pid_{1:x4}", di.HIDInfo.VendorID, di.HIDInfo.ProductID);
                  deviceDetails.Name = String.Format("HID: {0}", GetFriendlyName(vidAndPid));
                  deviceDetails.ID = deviceName;
                  break;
                }

              case NativeMethods.RawInputType.Keyboard:
                {
                  deviceDetails.Name = "Keyboard";
                  deviceDetails.ID = deviceName;
                  break;
                }

              case NativeMethods.RawInputType.Mouse:
                {
                  deviceDetails.Name = "Mouse";
                  deviceDetails.ID = deviceName;
                  break;
                }
            }
            _devices.Add(deviceDetails);

            Marshal.FreeHGlobal(pData);
          }
        }

        Marshal.FreeHGlobal(pRawInputDeviceList);
      }
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

    string GetFriendlyName(string vidAndPid)
    {
      try
      {
        RegistryKey USBEnum = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\USB");

        foreach (string usbSubKey in USBEnum.GetSubKeyNames())
        {
          if (usbSubKey.IndexOf(vidAndPid, StringComparison.InvariantCultureIgnoreCase) == -1)
            continue;

          RegistryKey currentKey = USBEnum.OpenSubKey(usbSubKey);

          string[] vidAndPidSubKeys = currentKey.GetSubKeyNames();

          foreach (string vidAndPidSubKey in vidAndPidSubKeys)
          {
            RegistryKey subKey = currentKey.OpenSubKey(vidAndPidSubKey);

            return subKey.GetValue("LocationInformation", null) as string;
          }
        }
      }
      catch
      {
      }

      return null;
    }

    private void buttonAdvanced_Click(object sender, EventArgs e)
    {

    }



  }

}
