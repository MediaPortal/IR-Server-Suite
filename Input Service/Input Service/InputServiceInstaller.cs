using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
#if TRACE
using System.Diagnostics;
#endif
using System.Management;
using System.ServiceProcess;

namespace InputService
{

  /// <summary>
  /// Installer for the Input Service.
  /// </summary>
  [RunInstaller(true)]
  class InputServiceInstaller : Installer
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="InputServiceInstaller"/> class.
    /// </summary>
    public InputServiceInstaller()
    {
      this.Committing += new InstallEventHandler(InputServiceInstaller_Committing);
      //this.AfterInstall += new InstallEventHandler(InputServiceInstaller_AfterInstall);

      ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
      ServiceInstaller serviceInstaller = new ServiceInstaller();

      // Service Account Information
      serviceProcessInstaller.Account   = ServiceAccount.LocalSystem;
      serviceProcessInstaller.Username  = null;
      serviceProcessInstaller.Password  = null;

      // Service Information
      serviceInstaller.ServiceName  = Program.ServiceName;
      serviceInstaller.DisplayName  = Program.ServiceDisplayName;
      serviceInstaller.Description  = Program.ServiceDescription;
      serviceInstaller.StartType    = ServiceStartMode.Automatic;
      
      this.Installers.Add(serviceProcessInstaller);
      this.Installers.Add(serviceInstaller);
    }

    /// <summary>
    /// Code to execute after the install has completed.
    /// </summary>
    void InputServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
    {
      // TODO: Set the restart options here.
      
      // Start the service ...
      //using (ServiceController serviceController = new ServiceController(Program.ServiceName))
        //serviceController.Start();
    }

    /// <summary>
    /// Used to set the "Allow service to interact with the desktop" setting.
    /// </summary>
    void InputServiceInstaller_Committing(object sender, InstallEventArgs e)
    {
      ManagementBaseObject inParam  = null;
      ManagementBaseObject outParam = null;

      try
      {
        ConnectionOptions coOptions = new ConnectionOptions();
        coOptions.Impersonation = ImpersonationLevel.Impersonate;
        
        ManagementScope mgmtScope = new ManagementScope(@"root\CIMV2", coOptions);
        mgmtScope.Connect();

        string path = string.Format("Win32_Service.Name='{0}'", Program.ServiceName);

        using (ManagementObject wmiService = new ManagementObject(path))
        {
          inParam = wmiService.GetMethodParameters("Change");
          inParam["DesktopInteract"] = true;
          outParam = wmiService.InvokeMethod("Change", inParam, null);
        }
      }
      catch
      {
        throw;
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
