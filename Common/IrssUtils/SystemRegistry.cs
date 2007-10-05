using System;
using System.Security.Permissions;

using Microsoft.Win32;

namespace IrssUtils
{

  /// <summary>
  /// Used for accessing the Windows System Registry.
  /// </summary>
  [RegistryPermission(SecurityAction.Demand, Read = "HKEY_CURRENT_USER\\SOFTWARE", Write = "HKEY_CURRENT_USER\\SOFTWARE")]
  [RegistryPermission(SecurityAction.Demand, Read = "HKEY_LOCAL_MACHINE\\Software")]
  public static class SystemRegistry
  {

    #region Constants

    const string AutoRunPath  = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    const string SoftwarePath = @"HKEY_LOCAL_MACHINE\Software\IR Server Suite";

    #endregion Constants

    #region Methods

    /// <summary>
    /// Get the install folder for IR Server Suite.
    /// </summary>
    /// <returns>String containing the Install Folder (no trailing slash).</returns>
    public static string GetInstallFolder()
    {
      return Registry.GetValue(SoftwarePath, "Install_Dir", null) as string;
    }

    /// <summary>
    /// Returns a boolean value indicating if there is an auto-run in place for the specified name.
    /// </summary>
    /// <param name="name">Auto-run program name.</param>
    /// <returns>If key of name exists return true else return false.</returns>
    public static bool GetAutoRun(string name)
    {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      return (Registry.GetValue(AutoRunPath, name, null) != null);
    }

    /// <summary>
    /// Setup an auto-run in the system registry.
    /// </summary>
    /// <param name="name">Auto-run program name.</param>
    /// <param name="executablePath">Executable Path for program.</param>
    public static void SetAutoRun(string name, string executablePath)
    {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      if (String.IsNullOrEmpty(executablePath))
        throw new ArgumentNullException("executablePath");

      Registry.SetValue(AutoRunPath, name, executablePath, RegistryValueKind.String);
    }

    /// <summary>
    /// Remove an auto-run from the system registry.
    /// </summary>
    /// <param name="name">Auto-run program name.</param>
    public static void RemoveAutoRun(string name)
    {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      Registry.SetValue(AutoRunPath, name, String.Empty, RegistryValueKind.String);
    }

    #endregion Methods

  }

}
