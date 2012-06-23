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

using System.ComponentModel;
using System.Configuration.Install;
using System.Management;
using System.Reflection;
using System.ServiceProcess;

namespace IRServer
{
  /// <summary>
  /// Installer for the IR Server.
  /// </summary>
  [RunInstaller(true)]
  internal class IRServerInstaller : Installer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IRServerInstaller"/> class.
    /// </summary>
    public IRServerInstaller()
    {
      Committing += IRServerInstaller_Committing;
      //this.AfterInstall += new InstallEventHandler(IRServerInstaller_AfterInstall);

      ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
      ServiceInstaller serviceInstaller = new ServiceInstaller();

      // Service Account Information
      serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
      serviceProcessInstaller.Username = null;
      serviceProcessInstaller.Password = null;

      // Service Information
      serviceInstaller.ServiceName = Shared.ServerName;
      serviceInstaller.DisplayName = Shared.ServerDisplayName;
      serviceInstaller.Description = Program.ServiceDescription;
      serviceInstaller.StartType = ServiceStartMode.Automatic;

      Installers.Add(serviceProcessInstaller);
      Installers.Add(serviceInstaller);
    }

/*
    /// <summary>
    /// Code to execute after the install has completed.
    /// </summary>
    private void IRServerInstaller_AfterInstall(object sender, InstallEventArgs e)
    {
      // TODO: Set the restart options here.

      // Start the service ...
      //using (ServiceController serviceController = new ServiceController(Program.ServiceName))
      //serviceController.Start();
    }
*/

    /// <summary>
    /// Used to set the "Allow service to interact with the desktop" setting.
    /// </summary>
    private void IRServerInstaller_Committing(object sender, InstallEventArgs e)
    {
      ManagementBaseObject inParam = null;
      ManagementBaseObject outParam = null;

      try
      {
        ConnectionOptions coOptions = new ConnectionOptions();
        coOptions.Impersonation = ImpersonationLevel.Impersonate;

        ManagementScope mgmtScope = new ManagementScope(@"root\CIMV2", coOptions);
        mgmtScope.Connect();

        string path = string.Format("Win32_Service.Name='{0}'", Shared.ServerName);

        using (ManagementObject wmiService = new ManagementObject(path))
        {
            inParam = wmiService.GetMethodParameters("Change");
            inParam["DesktopInteract"] = true;
            inParam["PathName"] = "\"" + Assembly.GetExecutingAssembly().Location + "\" -SERVICE";
            outParam = wmiService.InvokeMethod("Change", inParam, null);
        }
      }
      finally
      {
        if (inParam != null)
          inParam.Dispose();

        if (outParam != null)
          outParam.Dispose();
      }
    }
  }
}
