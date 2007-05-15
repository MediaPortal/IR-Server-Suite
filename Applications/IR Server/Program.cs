using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Win32;

using IRServerPluginInterface;
using IrssUtils;

namespace IRServer
{

  static class Program
  {

    enum ErrorCode : int
    {
      None                = 0,
      InvalidCommandLine  = 1,
      MultipleInstances   = 2,
      FailedToStart       = 3,
      Other               = 127
    }

    /// <summary>
    /// Entry point for IR Server
    /// </summary>
    /// <param name="args">Command line arguments
    /// /c = configure the IR Server
    /// /k = kill the server
    /// /? = show command line parameters
    /// </param>
    /// <returns>Exit code to show what happened
    /// 0 = Successful
    /// 1 = Invalid command line arguments
    /// 2 = Multiple instances
    /// 3 = Failed to start
    /// 4 = Other failure
    /// </returns>
    [STAThread]
    static int Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // Process Command Line Arguments
      if (args.Length == 1)
      {
        switch(args[0])
        {
          case "--help":
          case "-help":
          case "/help":
          case "-?":
          case "/?":
            MessageBox.Show(
@"Command Line Parameters:

/c = configure the IR Server
/k = kill the server
/? = show command line parameters",
            "IR Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return (int)ErrorCode.None;

          case "-configure":
          case "/configure":
          case "-c":
          case "/c":
            return Configure();

          case "-kill":
          case "/kill":
          case "-k":
          case "/k":
            return Kill();

          default:
            return (int)ErrorCode.InvalidCommandLine;
        }
      }
      else if (args.Length > 1)
      {
        return (int)ErrorCode.InvalidCommandLine;
      }

      // Check for multiple instances
      try
      {
        if (IsMultipleInstances())
          return (int)ErrorCode.MultipleInstances;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return (int)ErrorCode.Other;
      }

      // Open log file
      try
      {
        IrssLog.LogLevel = IrssLog.Level.Debug;
        IrssLog.Open(Common.FolderIrssLogs + "IR Server.log");

        IrssLog.Debug("Platform is {0}", (IntPtr.Size == 4 ? "32-bit" : "64-bit"));
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return (int)ErrorCode.Other;
      }

      // Start Server
      IRServer transceiverServer = new IRServer();
      
      if (transceiverServer.Start())
      {
        Application.Run();

        IrssLog.Close();
        return (int)ErrorCode.None;
      }
      else
      {
        MessageBox.Show("Failed to start IR Server, refer to log file for more details.", "IR Server", MessageBoxButtons.OK, MessageBoxIcon.Error);

        IrssLog.Close();
        return (int)ErrorCode.FailedToStart;
      }

    }

    static bool IsMultipleInstances()
    {
      return (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length != 1);
    }

    static int Configure()
    {
      RegistryKey key = null;

      try
      {
        key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\and-81\IRServer");

        Config config = new Config();
        config.Mode = (IRServerMode)key.GetValue("Mode", (int)IRServerMode.ServerMode);
        config.HostComputer = (string)key.GetValue("HostComputer", String.Empty);
        config.Plugin = (string)key.GetValue("Plugin", String.Empty);

        if (config.ShowDialog() == DialogResult.OK)
        {
          key.SetValue("Mode", (int)config.Mode);
          key.SetValue("HostComputer", config.HostComputer);
          key.SetValue("Plugin", config.Plugin);

          if (IsMultipleInstances())
            MessageBox.Show("You must restart the IR Server for any settings changes to take effect.", "IR Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        key.Close();
      }
      catch (Exception ex)
      {
        if (key != null)
          key.Close();

        MessageBox.Show(ex.Message, "IR Server - Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return (int)ErrorCode.Other;
      }

      return (int)ErrorCode.None;
    }
    static int Kill()
    {
      try
      {
        Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
        foreach (Process proc in processes)
          if (proc.Id != Process.GetCurrentProcess().Id)
            proc.Kill();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "IR Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return (int)ErrorCode.Other;
      }

      return (int)ErrorCode.None;
    }
    
    /// <summary>
    /// Retreives a list of available IR Server plugins
    /// </summary>
    /// <returns>Array of plugin instances</returns>
    internal static IIRServerPlugin[] AvailablePlugins()
    {
      try
      {
        List<IIRServerPlugin> plugins = new List<IIRServerPlugin>();

        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\IR Server Suite\\");
        string installFolder = (string)registryKey.GetValue("Install_Dir", String.Empty);
        registryKey.Close();

        if (string.IsNullOrEmpty(installFolder))
          return null;

        string[] files = Directory.GetFiles(installFolder + "\\IR Server Plugins\\", "*.dll", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
          try
          {
            Assembly assembly = Assembly.LoadFrom(file);

            Type[] types = assembly.GetExportedTypes();

            foreach (Type type in types)
            {
              if (type.IsClass && !type.IsAbstract && type.GetInterface(typeof(IIRServerPlugin).Name) == typeof(IIRServerPlugin))
              {
                IIRServerPlugin plugin = (IIRServerPlugin)Activator.CreateInstance(type);
                if (plugin == null)
                  continue;

                plugins.Add(plugin);
              }
            }
          }
          catch (BadImageFormatException)
          {
            // Ignore Bad Image Format Exceptions, just keep checking for IR Server Plugins
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.ToString(), "IR Server Unexpected Error");
          }
        }
        
        return plugins.ToArray();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return null;
      }
    }

  }

}
