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
using IrssCommands;
using IrssUtils;

namespace Translator
{
  /// <summary>
  /// Used for sending and receiving Copy Data Windows Messages.
  /// </summary>
  internal class CopyDataWM : NativeWindow, IDisposable
  {
    #region Constants

    /// <summary>
    /// Data value for CopyData messages.
    /// </summary>
    public const int CopyDataID = 24;

    /// <summary>
    /// Window name for CopyData messages.
    /// </summary>
    public const string CopyDataTarget = "Translator CopyData Target";

    #endregion Constants

    #region Constructor / Destructor

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="CopyDataWM"/> is reclaimed by garbage collection.
    /// </summary>
    ~CopyDataWM()
    {
      // Call Dispose with false.  Since we're in the destructor call, the managed resources will be disposed of anyway.
      Dispose(false);
    }

    #endregion Constructor / Destructor

    #region IDisposable Members

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      // Dispose of the managed and unmanaged resources
      Dispose(true);

      // Tell the GC that the Finalize process no longer needs to be run for this object.
      GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources ...

        Stop();
      }

      // Free native resources ...
    }

    /// <summary>
    /// Sends a copy data message.
    /// </summary>
    /// <param name="data">The data.</param>
    public static void SendCopyDataMessage(string data)
    {
      Win32.COPYDATASTRUCT copyData;

      byte[] dataBytes = Encoding.ASCII.GetBytes(data);

      copyData.dwData = CopyDataID;
      copyData.lpData = Win32.VarPtr(dataBytes);
      copyData.cbData = dataBytes.Length;

      IntPtr windowHandle = Win32.FindWindowByTitle(CopyDataTarget);

      if (windowHandle != IntPtr.Zero)
        Win32.SendWindowsMessage(windowHandle, (int) Win32.WindowsMessage.WM_COPYDATA, IntPtr.Zero,
                                 Win32.VarPtr(copyData));
      else
        throw new CommandExecutionException("Could not find running Translator instance to send message to");
    }

    #region Methods

    /// <summary>
    /// Starts this instance.
    /// </summary>
    /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
    public bool Start()
    {
      if (Handle != IntPtr.Zero)
        return false;

      CreateParams createParams = new CreateParams();
      createParams.Caption = CopyDataTarget;
      createParams.ExStyle = 0x80;
      createParams.Style = unchecked((int) 0x80000000);

      CreateHandle(createParams);

      return (Handle != IntPtr.Zero);
    }

    /// <summary>
    /// Stops this instance.
    /// </summary>
    public void Stop()
    {
      if (Handle != IntPtr.Zero)
        DestroyHandle();
    }

    #endregion Methods

    #region Overrides

    /// <summary>
    /// Invokes the default window procedure associated with this window.
    /// </summary>
    /// <param name="m">A <see cref="T:System.Windows.Forms.Message"></see> that is associated with the current Windows message.</param>
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == (int) Win32.WindowsMessage.WM_COPYDATA)
      {
        IrssLog.Info("Received WM_COPYDATA message");

        try
        {
          Win32.COPYDATASTRUCT dataStructure = (Win32.COPYDATASTRUCT) m.GetLParam(typeof (Win32.COPYDATASTRUCT));

          if (dataStructure.dwData != CopyDataID)
          {
            IrssLog.Warn("WM_COPYDATA ID ({0}) does not match expected ID ({1})", dataStructure.dwData, CopyDataID);
            return;
          }

          byte[] dataBytes = new byte[dataStructure.cbData];
          Marshal.Copy(dataStructure.lpData, dataBytes, 0, dataStructure.cbData);
          string strData = Encoding.ASCII.GetString(dataBytes);

          Program.ProcessCommand(strData);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Error processing WM_COPYDATA message: {0}", ex.ToString());
        }
      }

      base.WndProc(ref m);
    }

    #endregion Overrides
  }
}