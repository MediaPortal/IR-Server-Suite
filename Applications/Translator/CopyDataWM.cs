using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using IrssUtils;
using IrssUtils.Exceptions;

namespace Translator
{

  class CopyDataWM : NativeWindow, IDisposable
  {

    #region Constants

    /// <summary>
    /// Window name for CopyData messages.
    /// </summary>
    public const string CopyDataTarget = "Translator CopyData Target";

    /// <summary>
    /// Data value for CopyData messages.
    /// </summary>
    public const int CopyDataID = 24;

    #endregion Constants

    #region Constructor / Destructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CopyDataWM"/> class.
    /// </summary>
    public CopyDataWM() { }

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

    #endregion IDisposable Members

    #region Methods

    public bool Start()
    {
      if (Handle != IntPtr.Zero)
        return false;

      CreateParams createParams = new CreateParams();
      createParams.Caption      = CopyDataTarget;
      createParams.ExStyle      = 0x80;
      createParams.Style        = unchecked((int)0x80000000);

      CreateHandle(createParams);

      return (Handle != IntPtr.Zero);
    }

    public void Stop()
    {
      if (Handle != IntPtr.Zero)
        DestroyHandle();
    }

    #endregion Methods

    #region Overrides

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == (int)Win32.WindowsMessage.WM_COPYDATA)
      {
        IrssLog.Info("Received WM_COPYDATA message");

        try
        {
          Win32.COPYDATASTRUCT dataStructure = (Win32.COPYDATASTRUCT)m.GetLParam(typeof(Win32.COPYDATASTRUCT));

          if (dataStructure.dwData != CopyDataID)
          {
            IrssLog.Warn("WM_COPYDATA ID ({0}) does not match expected ID ({1})", dataStructure.dwData, CopyDataID);
            return;
          }

          byte[] dataBytes = new byte[dataStructure.cbData];
          IntPtr lpData = new IntPtr(dataStructure.lpData);
          Marshal.Copy(lpData, dataBytes, 0, dataStructure.cbData);
          string strData = Encoding.Default.GetString(dataBytes);

          Program.ProcessCommand(strData, false);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Error processing WM_COPYDATA message: {0}", ex.ToString());
        }
      }

      base.WndProc(ref m);
    }

    #endregion Overrides

    /// <summary>
    /// Sends a copy data message.
    /// </summary>
    /// <param name="data">The data.</param>
    public static void SendCopyDataMessage(string data)
    {
      Win32.COPYDATASTRUCT copyData;

      byte[] dataBytes = Encoding.Default.GetBytes(data);

      copyData.dwData = CopyDataID;
      copyData.lpData = Win32.VarPtr(dataBytes).ToInt32();
      copyData.cbData = dataBytes.Length;

      IntPtr windowHandle = Win32.FindWindowByTitle(CopyDataTarget);

      if (windowHandle != IntPtr.Zero)
        Win32.SendWindowsMessage(windowHandle, (int)Win32.WindowsMessage.WM_COPYDATA, IntPtr.Zero, Win32.VarPtr(copyData));
      else
        throw new CommandExecutionException("Could not find running Translator instance to send message to");
    }

  }

}
