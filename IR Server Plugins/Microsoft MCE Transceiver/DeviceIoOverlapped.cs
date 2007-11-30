using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MicrosoftMceTransceiver
{

  /// <summary>
  /// 32 and 64-bit compatible NativeOverlapped wrapper for DeviceIoControl calls.
  /// </summary>
  class DeviceIoOverlapped
  {

    #region Variables

    IntPtr _ptrOverlapped         = IntPtr.Zero;

    int _fieldOffsetInternalLow   = 0;
    int _fieldOffsetInternalHigh  = 0;
    int _fieldOffsetOffsetLow     = 0;
    int _fieldOffsetOffsetHigh    = 0;
    int _fieldOffsetEventHandle   = 0;

    #endregion Variables

    #region Constructor / Destructor

    /// <summary>
    /// Create a new managed Native Overlapped object.
    /// </summary>
    public DeviceIoOverlapped()
    {
      // Globally allocate the memory for the overlapped structure
      _ptrOverlapped = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeOverlapped)));

      // Find the structural starting positions in the NativeOverlapped structure.
      _fieldOffsetInternalLow   = Marshal.OffsetOf(typeof(NativeOverlapped), "InternalLow").ToInt32();
      _fieldOffsetInternalHigh  = Marshal.OffsetOf(typeof(NativeOverlapped), "InternalHigh").ToInt32();
      _fieldOffsetOffsetLow     = Marshal.OffsetOf(typeof(NativeOverlapped), "OffsetLow").ToInt32();
      _fieldOffsetOffsetHigh    = Marshal.OffsetOf(typeof(NativeOverlapped), "OffsetHigh").ToInt32();
      _fieldOffsetEventHandle   = Marshal.OffsetOf(typeof(NativeOverlapped), "EventHandle").ToInt32();
    }

    /// <summary>
    /// Destructor.
    /// </summary>
    ~DeviceIoOverlapped()
    {
      if (_ptrOverlapped != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(_ptrOverlapped);
        _ptrOverlapped = IntPtr.Zero;
      }
    }

    #endregion Constructor / Destructor

    #region Properties

    /// <summary>
    /// Gets or Sets the Internal Low value.
    /// </summary>
    public IntPtr InternalLow
    {
      get { return Marshal.ReadIntPtr(_ptrOverlapped, _fieldOffsetInternalLow); }
      set { Marshal.WriteIntPtr(_ptrOverlapped, _fieldOffsetInternalLow, value); }
    }

    /// <summary>
    /// Gets or Sets the Internal High value;
    /// </summary>
    public IntPtr InternalHigh
    {
      get { return Marshal.ReadIntPtr(_ptrOverlapped, _fieldOffsetInternalHigh); }
      set { Marshal.WriteIntPtr(_ptrOverlapped, _fieldOffsetInternalHigh, value); }
    }

    /// <summary>
    /// Gets or Sets the Offset Low value;
    /// </summary>
    public int OffsetLow
    {
      get { return Marshal.ReadInt32(_ptrOverlapped, _fieldOffsetOffsetLow); }
      set { Marshal.WriteInt32(_ptrOverlapped, _fieldOffsetOffsetLow, value); }
    }

    /// <summary>
    /// Gets or Sets the Offset High value;
    /// </summary>
    public int OffsetHigh
    {
      get { return Marshal.ReadInt32(_ptrOverlapped, _fieldOffsetOffsetHigh); }
      set { Marshal.WriteInt32(_ptrOverlapped, _fieldOffsetOffsetHigh, value); }
    }

    /// <summary>
    /// The overlapped event wait hande.
    /// </summary>
    public IntPtr EventHandle
    {
      get { return Marshal.ReadIntPtr(_ptrOverlapped, _fieldOffsetEventHandle); }
      set { Marshal.WriteIntPtr(_ptrOverlapped, _fieldOffsetEventHandle, value); }
    }

    /// <summary>
    /// Pass this into the DeviceIoControl and GetOverlappedResult APIs.
    /// </summary>
    public IntPtr Overlapped
    {
      get { return _ptrOverlapped; }
    }

    #endregion Properties

    #region Methods
    
    /// <summary>
    /// Set the overlapped wait handle and clear out the rest of the structure.
    /// </summary>
    /// <param name="eventHandle"></param>
    public void ClearAndSetEvent(IntPtr eventHandle)
    {
      EventHandle   = eventHandle;
      InternalLow   = IntPtr.Zero;
      InternalHigh  = IntPtr.Zero;
      OffsetLow     = 0;
      OffsetHigh    = 0;
    }

    #endregion Methods

  }

}
