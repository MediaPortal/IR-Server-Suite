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
      int lastError;

      // Get the HID Guid
      Guid classGuid = new Guid();
      NativeMethods.HidD_GetHidGuid(ref classGuid);

      // 0x12 = DIGCF_PRESENT | DIGCF_DEVICEINTERFACE
      IntPtr handle = NativeMethods.SetupDiGetClassDevs(ref classGuid, "", IntPtr.Zero, 0x12);
      lastError = Marshal.GetLastWin32Error();

      if (handle.ToInt32() == -1)
        throw new Win32Exception(lastError);

      for (int deviceIndex = 0; ; deviceIndex++)
      {
        NativeMethods.DeviceInfoData deviceInfoData = new NativeMethods.DeviceInfoData();
        deviceInfoData.Size = Marshal.SizeOf(deviceInfoData);

        if (NativeMethods.SetupDiEnumDeviceInfo(handle, deviceIndex, ref deviceInfoData) == false)
        {

          // out of devices or do we have an error?
          /*
          lastError = Marshal.GetLastWin32Error();
          if (lastError != 0x0103 && lastError != 0x007E)
          {
            NativeMethods.SetupDiDestroyDeviceInfoList(handle);
            throw new Win32Exception(Marshal.GetLastWin32Error());
          }
          */

          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          break;
        }

        NativeMethods.DeviceInterfaceData deviceInterfaceData = new NativeMethods.DeviceInterfaceData();
        deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

        if (NativeMethods.SetupDiEnumDeviceInterfaces(handle, ref deviceInfoData, ref classGuid, 0, ref deviceInterfaceData) == false)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          //throw new Win32Exception(Marshal.GetLastWin32Error());
          continue;
        }

        uint cbData = 0;

        if (NativeMethods.SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, IntPtr.Zero, 0, ref cbData, IntPtr.Zero) == false && cbData == 0)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          //throw new Win32Exception(Marshal.GetLastWin32Error());
          continue;
        }

        NativeMethods.DeviceInterfaceDetailData deviceInterfaceDetailData = new NativeMethods.DeviceInterfaceDetailData();
        deviceInterfaceDetailData.Size = 5;

        if (NativeMethods.SetupDiGetDeviceInterfaceDetail(handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, cbData, IntPtr.Zero, IntPtr.Zero) == false)
        {
          NativeMethods.SetupDiDestroyDeviceInfoList(handle);
          //throw new Win32Exception(Marshal.GetLastWin32Error());
          continue;
        }

        string devicePath = deviceInterfaceDetailData.DevicePath;

        if (!String.IsNullOrEmpty(devicePath))
        {
          string friendlyName = GetFriendlyName(devicePath);
          if (String.IsNullOrEmpty(friendlyName))
            friendlyName = "Unknown";

          DeviceDetails deviceDetails = new DeviceDetails();
          deviceDetails.Name = friendlyName;
          deviceDetails.ID = devicePath;
          _devices.Add(deviceDetails);
        }

        NativeMethods.SetupDiDestroyDeviceInfoList(handle);
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

    string GetFriendlyName(string deviceID)
    {
      try
      {
        RegistryKey USBEnum = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\USB");

        string vidAndPid = GetVidAndPid(deviceID);

        foreach (string usbSubKey in USBEnum.GetSubKeyNames())
        {
          if (usbSubKey.IndexOf(vidAndPid, StringComparison.InvariantCultureIgnoreCase) == -1)
            continue;

          RegistryKey currentKey = USBEnum.OpenSubKey(usbSubKey);

          string[] vidAndPidSubKeys = currentKey.GetSubKeyNames();

          foreach (string vidAndPidSubKey in vidAndPidSubKeys)
          {
            RegistryKey subKey = currentKey.OpenSubKey(vidAndPidSubKey);
            string parentIdPrefix = subKey.GetValue("ParentIdPrefix", null) as string;

            if (String.IsNullOrEmpty(parentIdPrefix))
              continue;

            if (deviceID.Contains(parentIdPrefix))
              return subKey.GetValue("DeviceDesc", null) as string + " (" + subKey.GetValue("LocationInformation", null) as string + ")";
          }
        }
      }
      catch
      {
      }

      return null;
    }

    string GetVidAndPid(string deviceID)
    {
      // \\?\hid#vid_0fe9&pid_9010#6&162bd6c4&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}

      int vidStart = deviceID.IndexOf("vid_", StringComparison.InvariantCultureIgnoreCase);

      if (vidStart != -1)
        return deviceID.Substring(vidStart, 17);
      else
        return null;
    }

    private void buttonAdvanced_Click(object sender, EventArgs e)
    {

    }



  }

}
