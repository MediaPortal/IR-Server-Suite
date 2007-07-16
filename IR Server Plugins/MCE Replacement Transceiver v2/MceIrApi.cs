using System;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.Win32.SafeHandles;

namespace MceReplacementTransceiver
{

  /// <summary>
  /// This class provides access to the MceIr.dll file functions.
  /// Expects MceIr.dll extended by Aaron Dinnage.
  /// </summary>
  public static class MceIrApi
  {

    #region Constants

//    public const int ID_MCEIR_KEYCODE = 0x37FF0;

    #endregion

    #region Enumerations

    /// <summary>
    /// The blaster port to send IR codes to
    /// </summary>
    public enum BlasterPort
    {
      /// <summary>
      /// Send IR codes to both blaster ports
      /// </summary>
      Both = 0,
      /// <summary>
      /// Send IR codes to blaster port 1 only
      /// </summary>
      Port_1 = 1,
      /// <summary>
      /// Send IR codes to blaster port 2 only
      /// </summary>
      Port_2 = 2
    }

    /// <summary>
    /// Type of blaster in use
    /// </summary>
    public enum BlasterType
    {
      /// <summary>
      /// Device is a first party Microsoft MCE transceiver
      /// </summary>
      Microsoft = 0,
      /// <summary>
      /// Device is an third party SMK MCE transceiver
      /// </summary>
      SMK = 1
    }

    /// <summary>
    /// Speed to transmit IR codes at
    /// </summary>
    public enum BlasterSpeed
    {
      /// <summary>
      /// None - Do not set the blaster speed
      /// (Note: If an IR code has been sent with a speed setting previously
      /// then that speed setting will continue to take effect, until the
      /// unit's power is cycled)
      /// </summary>
      None    = 0,
      /// <summary>
      /// Fast - Set blaster speed to fast
      /// </summary>
      Fast    = 1,
      /// <summary>
      /// Medium - Set blaster speed to medium
      /// </summary>
      Medium  = 2,
      /// <summary>
      /// Slow - Set blaster speed to slow
      /// </summary>
      Slow    = 3,
    }

    /// <summary>
    /// A list of MCE remote buttons
    /// </summary>
    public enum MceButton
    {
      Custom        = -1,
      None          = 0,
      TV_Power      = 0x7b9a,
      Blue          = 0x7ba1,
      Yellow        = 0x7ba2,
      Green         = 0x7ba3,
      Red           = 0x7ba4,
      Teletext      = 0x7ba5,
      Radio         = 0x7baf,
      Print         = 0x7bb1,
      Videos        = 0x7bb5,
      Pictures      = 0x7bb6,
      Recorded_TV   = 0x7bb7,
      Music         = 0x7bb8,
      TV            = 0x7bb9,
      Guide         = 0x7bd9,
      Live_TV       = 0x7bda,
      DVD_Menu      = 0x7bdb,
      Back          = 0x7bdc,
      OK            = 0x7bdd,
      Right         = 0x7bde,
      Left          = 0x7bdf,
      Down          = 0x7be0,
      Up            = 0x7be1,
      Star          = 0x7be2,
      Hash          = 0x7be3,
      Replay        = 0x7be4,
      Skip          = 0x7be5,
      Stop          = 0x7be6,
      Pause         = 0x7be7,
      Record        = 0x7be8,
      Play          = 0x7be9,
      Rewind        = 0x7bea,
      Forward       = 0x7beb,
      Channel_Down  = 0x7bec,
      Channel_Up    = 0x7bed,
      Volume_Down   = 0x7bee,
      Volume_Up     = 0x7bef,
      Info          = 0x7bf0,
      Mute          = 0x7bf1,
      Start         = 0x7bf2,
      PC_Power      = 0x7bf3,
      Enter         = 0x7bf4,
      Escape        = 0x7bf5,
      Number_9      = 0x7bf6,
      Number_8      = 0x7bf7,
      Number_7      = 0x7bf8,
      Number_6      = 0x7bf9,
      Number_5      = 0x7bfa,
      Number_4      = 0x7bfb,
      Number_3      = 0x7bfc,
      Number_2      = 0x7bfd,
      Number_1      = 0x7bfe,
      Number_0      = 0x7bff,
    }

    #endregion Enumerations

    #region Variables

    static bool _isSuspended  = false;
    static bool _inUse        = false;

    #endregion Variables

    #region Properties

    public static bool InUse
    {
      get { return _inUse; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Register your window handle to receive window messages from the MceIrApi.
    /// </summary>
    /// <param name="windowHandle">Window handle.</param>
    /// <returns>Success.</returns>
    public static bool RegisterEvents(HandleRef windowHandle)
    {
      _inUse = true;
      if (MceIrRegisterEvents(windowHandle))
      {
        Resume();
        return true;
      }

      return false;
    }

    /// <summary>
    /// Unregister from receiving window messages from the MceIrApi.
    /// </summary>
    /// <returns>Success.</returns>
    public static bool UnregisterEvents()
    {
      if (MceIrUnregisterEvents())
      {
        Suspend();
        _inUse = false;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Sets the time between key presses being repeated.
    /// All times are in milliseconds.
    /// </summary>
    /// <param name="firstRepeat">How long after the button is pressed before repeating.</param>
    /// <param name="nextRepeats">How long between repeats.</param>
    public static void SetRepeatTimes(int firstRepeat, int nextRepeats)
    {
      _inUse = true;
      MceIrSetRepeatTimes(firstRepeat, nextRepeats);
    }

    /// <summary>
    /// Record an IR code to file.
    /// </summary>
    /// <param name="fileHandle">Handle to already open file to write to.</param>
    /// <param name="timeout">How long before a timeout occurs (in milliseconds).</param>
    /// <returns>Success.</returns>
    public static bool RecordToFile(SafeFileHandle fileHandle, int timeout)
    {
      _inUse = true;
      return MceIrRecordToFile(fileHandle, timeout);
    }

    /// <summary>
    /// Transmit an IR Code from a file.
    /// </summary>
    /// <param name="fileHandle">Handle to the file containing the IR Code.</param>
    /// <returns>Success.</returns>
    public static bool PlaybackFromFile(SafeFileHandle fileHandle)
    {
      _inUse = true;
      bool returnValue = MceIrPlaybackFromFile(fileHandle);
      Thread.Sleep(250);
      return returnValue;
    }

    /// <summary>
    /// Suspend the MceIrApi.
    /// </summary>
    public static void Suspend()
    {
      if (!_isSuspended)
      {
        _inUse = true;
        _isSuspended = true;
        MceIrSuspend();
      }
    }

    /// <summary>
    /// Resume the MceIrApi.
    /// </summary>
    public static void Resume()
    {
      if (_isSuspended)
      {
        _inUse = true;
        _isSuspended = false;
        MceIrResume();
        Thread.Sleep(250);
      }
    }

    /// <summary>
    /// Select the Blaster port to use for transmitting IR Codes.
    /// </summary>
    /// <param name="port">Port to send to.</param>
    public static void SelectBlaster(BlasterPort port)
    {
      _inUse = true;
      MceIrSelectBlaster((int)port);
    }

    /// <summary>
    /// Check an IR Code file to ensure it is valid.
    /// </summary>
    /// <param name="fileHandle">Handle to file to check.</param>
    /// <returns>True if file is valid, False if file is invalid.</returns>
    public static bool CheckFile(SafeFileHandle fileHandle)
    {
      _inUse = true;
      return MceIrCheckFile(fileHandle);
    }

    /// <summary>
    /// Set the Speed to transmit IR Codes at.
    /// </summary>
    /// <param name="speed">IR Code speed.</param>
    public static void SetBlasterSpeed(BlasterSpeed speed)
    {
      _inUse = true;
      MceIrSetBlasterSpeed((int)speed);
    }

    /// <summary>
    /// Set the Type of MCE unit.
    /// </summary>
    /// <param name="type">Manufacturer of MCE unit.</param>
    public static void SetBlasterType(BlasterType type)
    {
      _inUse = true;
      MceIrSetBlasterType((int)type);
    }

    #endregion Methods

  }

}
