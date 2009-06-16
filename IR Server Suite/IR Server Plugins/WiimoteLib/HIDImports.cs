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
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace WiimoteLib
{
  /// <summary>
  /// Win32 import information for use with the Wiimote library
  /// </summary>
  internal class HIDImports
  {
    //
    // Flags controlling what is included in the device information set built
    // by SetupDiGetClassDevs
    //

    #region EFileAttributes enum

    [Flags]
    public enum EFileAttributes : uint
    {
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000
    }

    #endregion

    public const int DIGCF_ALLCLASSES = 0x00000004;

    public const int DIGCF_DEFAULT = 0x00000001; // only valid with DIGCF_DEVICEINTERFACE
    public const int DIGCF_DEVICEINTERFACE = 0x00000010;
    public const int DIGCF_PRESENT = 0x00000002;
    public const int DIGCF_PROFILE = 0x00000008;

    [DllImport(@"hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern void HidD_GetHidGuid(out Guid gHid);

    [DllImport("hid.dll")]
    [return: MarshalAs(UnmanagedType.U1)]
    public static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

    [DllImport("hid.dll")]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static extern bool HidD_SetOutputReport(
      IntPtr HidDeviceObject,
      byte[] lpReportBuffer,
      uint ReportBufferLength);

    [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
      IntPtr hwndParent,
      UInt32 Flags
      );

    [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr hDevInfo,
      //ref SP_DEVINFO_DATA devInfo,
      IntPtr devInvo,
      ref Guid interfaceClassGuid,
      Int32 memberIndex,
      ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
      );

    [DllImport(@"setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr hDevInfo,
      ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
      IntPtr deviceInterfaceDetailData,
      UInt32 deviceInterfaceDetailDataSize,
      out UInt32 requiredSize,
      IntPtr deviceInfoData
      );

    [DllImport(@"setupapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr hDevInfo,
      ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
      ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
      UInt32 deviceInterfaceDetailDataSize,
      out UInt32 requiredSize,
      IntPtr deviceInfoData
      );

    [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern SafeFileHandle CreateFile(
      string fileName,
      [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
      [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
      IntPtr securityAttributes,
      [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
      [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
      IntPtr template);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    #region Nested type: HIDD_ATTRIBUTES

    [StructLayout(LayoutKind.Sequential)]
    public struct HIDD_ATTRIBUTES
    {
      public int Size;
      public short VendorID;
      public short ProductID;
      public short VersionNumber;
    }

    #endregion

    #region Nested type: SP_DEVICE_INTERFACE_DATA

    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DEVICE_INTERFACE_DATA
    {
      public int cbSize;
      public Guid InterfaceClassGuid;
      public int Flags;
      public IntPtr RESERVED;
    }

    #endregion

    #region Nested type: SP_DEVICE_INTERFACE_DETAIL_DATA

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
      public UInt32 cbSize;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string DevicePath;
    }

    #endregion

    #region Nested type: SP_DEVINFO_DATA

    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DEVINFO_DATA
    {
      public uint cbSize;
      public Guid ClassGuid;
      public uint DevInst;
      public IntPtr Reserved;
    }

    #endregion
  }
}