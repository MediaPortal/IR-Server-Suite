#region Copyright (C) 2005-2009 Team MediaPortal

/* 
 *	Copyright (C) 2005-2009 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace IRServer.Plugin
{
  internal class FireDTVConstants
  {
    #region Constants

    internal const int WM_USER = 0x0400;

    #endregion

    #region FireDTV Enumeration

    internal enum FireDTVWindowMessages
    {
      DeviceAttached = WM_USER + 1,
      DeviceDetached = WM_USER + 2,
      DeviceChanged = WM_USER + 3,
      CIModuleInserted = WM_USER + 4,
      CIModuleReady = WM_USER + 5,
      CIModuleRemoved = WM_USER + 6,
      CIMMI = WM_USER + 7,
      CIDateTime = WM_USER + 8,
      CIPMTReply = WM_USER + 9,
      RemoteControlEvent = WM_USER + 10
    }

    internal enum FireDTVStatusCodes
    {
      Success = 0,
      Error = 1,
      InvalidDeviceHandle = 2,
      InvalidValue = 3,
      AlreadyInUse = 4,
      NotSuppotedByTuner = 5,
    } ;

    #endregion

    #region Structs

    internal struct FireDTV_DRIVER_VERSION
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] public byte[] DriverVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FireDTV_FIRMWARE_VERSION
    {
      internal byte uHWMajor;
      internal byte uHWMiddle;
      internal byte uHWMinor;
      internal byte uSWMajor;
      internal byte uSWMiddle;
      internal byte uSWMinor;
      internal byte uBuildNrMSB;
      internal byte uBuildNrLSB;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FireDTV_SYSTEM_INFO
    {
      internal byte uNrAntennas; //0-3
      internal byte uAntennaInfo; //ANTENNA_FIX, ANTENNA_MOVABLE, ANTENNA_MOBIL
      internal byte uSystem; // 
      internal byte uTransport; //TRANSPORT_SATELLITE, TRANSPORT_CABLE, TRANSPORT_TERRESTRIAL
      internal bool bLists;
    }

    #endregion
  }

  #region FireDTV Exception Class

  internal class FireDTVException : Exception
  {
    internal FireDTVException()
    {
    }

    internal FireDTVException(string message)
      : base(message)
    {
    }

    internal FireDTVException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal FireDTVException(FireDTVConstants.FireDTVStatusCodes status, string message)
    {
      friendlyMessage = message;
      statusCode = status;
    }


    private FireDTVConstants.FireDTVStatusCodes statusCode;
    private string friendlyMessage;

    internal FireDTVConstants.FireDTVStatusCodes StatusCode
    {
      get { return statusCode; }
    }

    public override string Message
    {
      get
      {
        if (friendlyMessage != string.Empty)
        {
          return friendlyMessage;
        }
        else
        {
          switch (statusCode)
          {
            case FireDTVConstants.FireDTVStatusCodes.AlreadyInUse:
              return "DEVICE ALREADY IN USE!";


            case FireDTVConstants.FireDTVStatusCodes.Error:
              return "STATUS ERROR!";


            case FireDTVConstants.FireDTVStatusCodes.InvalidDeviceHandle:
              return "INVALID DEVICE HANDLE!";


            case FireDTVConstants.FireDTVStatusCodes.InvalidValue:
              return "INVALID VALUE!";


            case FireDTVConstants.FireDTVStatusCodes.NotSuppotedByTuner:
              return "NOT SUPPORTED BY TUNER!";


            default:
              return base.Message;
          }
        }
      }
    }
  }

  internal class FireDTVInitializationException : FireDTVException
  {
    internal FireDTVInitializationException()
    {
    }

    internal FireDTVInitializationException(string message)
      : base(message)
    {
    }

    internal FireDTVInitializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }

  internal class FireDTVDeviceOpenException : FireDTVException
  {
    internal FireDTVDeviceOpenException()
    {
    }

    internal FireDTVDeviceOpenException(string message)
      : base(message)
    {
    }

    internal FireDTVDeviceOpenException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal FireDTVDeviceOpenException(FireDTVConstants.FireDTVStatusCodes status, string message)
      : base(status, message)
    {
    }
  }

  #endregion

  /// <summary>
  /// Summary description for FireDTVDefines.
  /// </summary>
  internal class FireDTVAPI
  {
    #region FireDTV API Imports

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_Initialize@@YAKXZ",
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_Initialize();

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_RegisterGeneralNotifications@@YAKPAUHWND__@@@Z",
      SetLastError = true,
      CharSet = CharSet.Unicode,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_RegisterGeneralNotifications(int hWnd);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetNumberOfWDMDevices@@YAKPAK@Z",
      SetLastError = true,
      CharSet = CharSet.Unicode,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetNumberOfWDMDevices(out uint puNumberOfWDMDevices);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetNumberOfBDADevices@@YAKPAK@Z",
      SetLastError = true,
      CharSet = CharSet.Unicode,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetNumberOfBDADevices(out uint puNumberOfWDMDevices);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_OpenWDMDeviceHandle@@YAKKPAK@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_OpenWDMDeviceHandle(uint uWDMDeviceNumber, out uint pDeviceHandle);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_OpenBDADeviceHandle@@YAKKPAK@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_OpenBDADeviceHandle(uint uBDADeviceNumber, out uint DeviceHandle);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_CloseDeviceHandle@@YAKK@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_CloseDeviceHandle(uint DeviceHandle);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_RegisterNotifications@@YAKKPAUHWND__@@@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_RegisterNotifications(uint DeviceHandle, int hWnd);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_UnregisterNotifications@@YAKK@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_UnregisterNotifications(uint DeviceHandle);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetApiVersion@@YAPADXZ",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = true,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern IntPtr FS_GetApiVersion();

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetFriendlyString@@YAKKPAPAD@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = true,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetFriendlyString(uint deviceHandle, out string friendlyName);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetDisplayString@@YAKKPAD@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = true,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetDisplayString(uint DeviceHandle, StringBuilder strDisplayName);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetGUIDString@@YAKKPAD@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetGUIDString(uint DeviceHandle, StringBuilder strGUIDName);

    [DllImport("FiresatApi.dll",
      EntryPoint = "?FS_GetDriverVersion@@YAKKPAU_FIRESAT_DRIVER_VERSION@@@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetDriverVersion(uint DeviceHandle,
                                                  ref FireDTVConstants.FireDTV_DRIVER_VERSION pDriverVersion);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_GetFirmwareVersion@@YAKKPAU_FIRESAT_FIRMWARE_VERSION@@@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetFirmwareVersion(uint DeviceHandle,
                                                    ref FireDTVConstants.FireDTV_FIRMWARE_VERSION Version);

    [DllImport("FiresatApi.dll",
      EntryPoint = "?FS_GetSystemInfo@@YAKKPAU_FIRESAT_SYSTEM_INFO@@@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_GetSystemInfo(uint DeviceHandle, ref FireDTVConstants.FireDTV_SYSTEM_INFO pSystemInfo);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_RemoteControl_Start@@YAKK@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_RemoteControl_Start(uint DeviceHandle);

    [DllImport("FireSATApi.dll",
      EntryPoint = "?FS_RemoteControl_Stop@@YAKK@Z",
      SetLastError = true,
      CharSet = CharSet.Ansi,
      ExactSpelling = true,
      PreserveSig = false,
      CallingConvention = CallingConvention.StdCall)]
    internal static extern uint FS_RemoteControl_Stop(uint DeviceHandle);

    #endregion

    #region Win32 API Imports

    [DllImport("user32.dll", CharSet = CharSet.Ansi)]
    internal static extern int GetActiveWindow();

    #endregion
  } ;
}