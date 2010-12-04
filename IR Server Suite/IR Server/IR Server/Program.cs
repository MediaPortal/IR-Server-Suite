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
using System.Collections;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using IRServer.Plugin;
using IrssUtils;
using System.Windows.Forms;
using Microsoft.Win32;

namespace IRServer
{
  internal static class Program
  {
    #region Constants

    public const string ServiceDescription =
      "The main component of IR Server Suite, the IR Server provides access to your input devices";

    private static IRServer IRServer;

    #endregion Constants

    /// <summary>
    /// The main entry point for the service.
    /// </summary>
    /// <param name="args">Command line parameters.</param>
    [STAThread]
    private static void Main(string[] args)
    {
      if (ProcessHelper.IsProcessAlreadyRunning())
        return;

      Thread.CurrentThread.Name = "Main Thread";
      IrssLog.Open("IR Server.log");

      try
      {
        if (args.Length == 0)
        {
          IRServer = new IRServer();
          if (IRServer.DoStart())
          {
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(OnPowerModeChanged);

            ReceiverWindow receiverWindow = new ReceiverWindow(Shared.ServerWindowName);

            Application.Run();
            SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(OnPowerModeChanged);

            receiverWindow.DestroyHandle();
            receiverWindow = null;
            IRServer.DoStop();
          }
        }
        else
        {
          foreach (string parameter in args)
          {
            switch (parameter.ToUpperInvariant().Replace("-", "/"))
            {
              case "/INSTALL":
                IrssLog.Info("Installing IR Server ...");
                using (TransactedInstaller transactedInstaller = new TransactedInstaller())
                {
                  using (IRServerInstaller IRServerInstaller = new IRServerInstaller())
                  {
                    transactedInstaller.Installers.Add(IRServerInstaller);

                    string path = "/assemblypath=" + Assembly.GetExecutingAssembly().Location;
                    string[] cmdline = { path };

                    InstallContext installContext = new InstallContext(String.Empty, cmdline);
                    transactedInstaller.Context = installContext;

                    transactedInstaller.Install(new Hashtable());
                  }
                }
                break;

              case "/UNINSTALL":
                IrssLog.Info("Uninstalling IR Server ...");
                using (TransactedInstaller transactedInstaller = new TransactedInstaller())
                {
                  using (IRServerInstaller IRServerInstaller = new IRServerInstaller())
                  {
                    transactedInstaller.Installers.Add(IRServerInstaller);

                    string path = "/assemblypath=" + Assembly.GetExecutingAssembly().Location;
                    string[] cmdline = { path };

                    InstallContext installContext = new InstallContext(String.Empty, cmdline);
                    transactedInstaller.Context = installContext;

                    transactedInstaller.Uninstall(null);
                  }
                }
                break;

              case "/START":
                IrssLog.Info("Starting IR Server ...");
                using (ServiceController serviceController = new ServiceController(Shared.ServerName))
                  if (serviceController.Status == ServiceControllerStatus.Stopped)
                    serviceController.Start();
                break;

              case "/STOP":
                IrssLog.Info("Stopping IR Server ...");
                using (ServiceController serviceController = new ServiceController(Shared.ServerName))
                  if (serviceController.Status == ServiceControllerStatus.Running)
                    serviceController.Stop();
                break;

              case "/RESTART":
                IrssLog.Info("Restarting IR Server ...");
                using (ServiceController serviceController = new ServiceController(Shared.ServerName))
                {
                  if (serviceController.Status == ServiceControllerStatus.Running)
                    serviceController.Stop();

                  serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));

                  if (serviceController.Status == ServiceControllerStatus.Stopped)
                    serviceController.Start();
                }
                break;

              case "/SERVICE":
                {
                  IRServer IRServer = new IRServer();
                  ServiceBase.Run(IRServer);
                }
                break;

              default:
                throw new InvalidOperationException(String.Format("Unknown command line parameter \"{0}\"", parameter));
            }
          }

          IrssLog.Info("Done.");
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
      finally
      {
        IrssLog.Close();
      }
    }

    /// <summary>
    /// Retreives a plugin instance given the plugin name.
    /// </summary>
    /// <param name="pluginName">Name of plugin to instantiate.</param>
    /// <returns>Plugin instance.</returns>
    internal static PluginBase GetPlugin(string pluginName)
    {
      if (String.IsNullOrEmpty(pluginName))
        throw new ArgumentNullException("pluginName");

      PluginBase[] serverPlugins = BasicFunctions.AvailablePlugins();
      if (serverPlugins == null)
        throw new FileNotFoundException("No available plugins found");

      foreach (PluginBase plugin in serverPlugins)
        if (plugin.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase))
          return plugin;

      throw new InvalidOperationException(String.Format("Plugin not found ({0})", pluginName));
    }

    private static void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {
      switch (e.Mode)
      {
        case PowerModes.Resume:
          IRServer.DoPowerEvent(PowerBroadcastStatus.ResumeAutomatic);
          break;
        case PowerModes.Suspend:
          IRServer.DoPowerEvent(PowerBroadcastStatus.Suspend);
          break;
      }
    }
  }
}