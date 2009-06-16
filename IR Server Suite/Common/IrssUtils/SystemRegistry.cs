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
using System.Security.Permissions;
using Microsoft.Win32;

namespace IrssUtils
{
  /// <summary>
  /// Used for accessing the Windows System Registry.
  /// </summary>
  [RegistryPermission(SecurityAction.Demand, Read = "HKEY_CURRENT_USER\\SOFTWARE", Write = "HKEY_CURRENT_USER\\SOFTWARE"
    )]
  [RegistryPermission(SecurityAction.Demand, Read = "HKEY_LOCAL_MACHINE\\Software")]
  public static class SystemRegistry
  {
    #region Constants

    private const string AutoRunPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string SoftwarePath = @"HKEY_LOCAL_MACHINE\Software\IR Server Suite";

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

      string value = Registry.GetValue(AutoRunPath, name, null) as string;

      return !String.IsNullOrEmpty(value);
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