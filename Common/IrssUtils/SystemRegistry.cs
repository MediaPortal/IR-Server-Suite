using System;

using Microsoft.Win32;

namespace IrssUtils
{

  /// <summary>
  /// Used for accessing the Windows System Registry.
  /// </summary>
  public static class SystemRegistry
  {

    #region Constants

    const string AutoRunPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    #endregion Constants

    /// <summary>
    /// Get the install folder for IR Server Suite.
    /// </summary>
    /// <returns>String containing the Install Folder (no trailing slash).</returns>
    public static string GetInstallFolder()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\IR Server Suite\\");
      string installFolder = registryKey.GetValue("Install_Dir", null) as string;
      registryKey.Close();

      return installFolder;
    }

    /// <summary>
    /// Returns a boolean value indicating if there is an auto-run in place for the specified name.
    /// </summary>
    /// <param name="name">Auto-run program name.</param>
    /// <returns>If key of name exists return true else return false.</returns>
    public static bool GetAutoRun(string name)
    {
      RegistryKey key = Registry.CurrentUser.CreateSubKey(AutoRunPath);
      bool autoRun = (key.GetValue(name, null) != null);
      key.Close();

      return autoRun;
    }

    /// <summary>
    /// Setup an auto-run in the system registry.
    /// </summary>
    /// <param name="name">Auto-run program name.</param>
    /// <param name="executablePath">Executable Path for program.</param>
    public static void SetAutoRun(string name, string executablePath)
    {
      RegistryKey key = Registry.CurrentUser.CreateSubKey(AutoRunPath);
      key.SetValue(name, executablePath, RegistryValueKind.String);
      key.Close();
    }

    /// <summary>
    /// Remove an auto-run from the system registry.
    /// </summary>
    /// <param name="name">Auto-run program name.</param>
    public static void RemoveAutoRun(string name)
    {
      RegistryKey key = Registry.CurrentUser.CreateSubKey(AutoRunPath);
      key.DeleteValue(name, false);
      key.Close();
    }
    
  }

}
