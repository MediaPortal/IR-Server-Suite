using System;
using System.Runtime.InteropServices;

namespace IrssUtils
{

  /// <summary>
  /// Provides access to Audio functions.
  /// </summary>
  public static class Audio
  {

    #region Interop

    [DllImport("Kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool Beep(
      uint frequency,
      uint duration);

    [DllImport("winmm.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool PlaySound(
        string pszSound,
        IntPtr hMod,
        SoundFlags sf);


    [DllImport("winmm.dll")]
    static extern uint waveOutGetVolume(
      IntPtr hwo,
      uint dwVolume);

    [DllImport("winmm.dll")]
    static extern int waveOutSetVolume(
      IntPtr DeviceID,
      IntPtr Volume);

    [DllImport("winmm.dll")]
    static extern Int32 mixerClose(
      IntPtr hmx);

    [DllImport("winmm.dll")]
    static extern Int32 mixerOpen(
      ref IntPtr phmx,
      uint pMxId,
      IntPtr dwCallback,
      IntPtr dwInstance,
      uint fdwOpen);

    #endregion Interop

    #region Enumerations

    /// <summary>
    /// Flags for playing sounds.  For this example, we are reading the sound from a filename, so we need only specify SND_FILENAME | SND_ASYNC.
    /// </summary>
    [Flags]
    public enum SoundFlags : int
    {
      /// <summary>
      /// Play synchronously (default).
      /// </summary>
      SND_SYNC      = 0x00000000,
      /// <summary>
      /// Play asynchronously.
      /// </summary>
      SND_ASYNC     = 0x00000001,
      /// <summary>
      /// Silence (!default) if sound not found.
      /// </summary>
      SND_NODEFAULT = 0x00000002,
      /// <summary>
      /// pszSound points to a memory file.
      /// </summary>
      SND_MEMORY    = 0x00000004,
      /// <summary>
      /// Loop the sound until next sndPlaySound.
      /// </summary>
      SND_LOOP      = 0x00000008,
      /// <summary>
      /// Don't stop any currently playing sound.
      /// </summary>
      SND_NOSTOP    = 0x00000010,
      /// <summary>
      /// Don't wait if the driver is busy.
      /// </summary>
      SND_NOWAIT    = 0x00002000,
      /// <summary>
      /// Name is a registry alias.
      /// </summary>
      SND_ALIAS     = 0x00010000,
      /// <summary>
      /// Alias is a predefined ID.
      /// </summary>
      SND_ALIAS_ID  = 0x00110000,
      /// <summary>
      /// Name is file name.
      /// </summary>
      SND_FILENAME  = 0x00020000,
      /// <summary>
      /// Name is resource name or atom.
      /// </summary>
      SND_RESOURCE  = 0x00040004,
    }

    #endregion Enumerations

    #region Implementation

    /// <summary>
    /// Play an audio file.
    /// </summary>
    /// <param name="fileName">The file to play.</param>
    /// <param name="async">true to play asynchronously, false to wait for the sound to finish.</param>
    /// <returns>true if played successfully, otherwise false.</returns>
    public static bool PlayFile(string fileName, bool async)
    {
      if (async)
        return PlaySound(fileName, IntPtr.Zero, SoundFlags.SND_ASYNC | SoundFlags.SND_FILENAME);
      else
        return PlaySound(fileName, IntPtr.Zero, SoundFlags.SND_SYNC | SoundFlags.SND_FILENAME);
    }

    /// <summary>
    /// Plays a beep.
    /// </summary>
    /// <param name="frequency">The frequency in hertz.</param>
    /// <param name="duration">The duration in milliseconds.</param>
    /// <returns><c>true</c> if successfuly, otherwise <c>false</c>.</returns>
    public static bool PlayBeep(int frequency, int duration)
    {
      return Beep((uint)frequency, (uint)duration);
    }

    #endregion Implementation

  }

}
