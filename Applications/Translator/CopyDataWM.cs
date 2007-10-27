using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Translator
{

  class CopyDataWM : NativeWindow, IDisposable
  {

    #region Constants

    static readonly string CopyDataTarget = "Translator CopyData Target";

    const int CopyDataID = 24;

    #endregion Constants

    #region Constructor / Destructor

    /// <summary>
    /// Initializes a new instance of the <see cref="NotifyWindow"/> class.
    /// </summary>
    public CopyDataWM()
    {
      Create();
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="NotifyWindow"/> is reclaimed by garbage collection.
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

        Destroy();
      }

      // Free native resources ...
    }

    #endregion IDisposable Members

    #region Methods
    
    void Create()
    {
      if (Handle != IntPtr.Zero)
        return;

      CreateParams Params = new CreateParams();
      Params.ExStyle      = 0x80;
      Params.Style        = unchecked((int)0x80000000);
      Params.Caption      = CopyDataTarget;

      CreateHandle(Params);
    }

    void Destroy()
    {
      if (Handle == IntPtr.Zero)
        return;

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
          string strData = Encoding.ASCII.GetString(dataBytes);

          Program.ProcessCommand(strData);
        }
        catch (Exception ex)
        {
          IrssLog.Error("Error processing WM_COPYDATA message: {0}", ex.ToString());
        }
      }
    }

    #endregion Overrides

    internal static void SendCopyDataMessage(string data)
    {
      Win32.COPYDATASTRUCT copyData;

      byte[] dataBytes = Encoding.ASCII.GetBytes(data);

      copyData.dwData = CopyDataID;
      copyData.lpData = Win32.VarPtr(dataBytes).ToInt32();
      copyData.cbData = dataBytes.Length;

      IntPtr windowHandle = Win32.FindWindowByTitle(CopyDataTarget);

      if (windowHandle != IntPtr.Zero)
        Win32.SendWindowsMessage(windowHandle, (int)Win32.WindowsMessage.WM_COPYDATA, IntPtr.Zero, Win32.VarPtr(copyData));
    }

  }

}
