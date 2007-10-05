using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Util;

using IrssComms;
using IrssUtils;
using MPUtils;

namespace MediaPortal.Plugins
{

  public class MPBlastZonePlugin : GUIWindow, ISetupForm
  {

    #region Skin Elements

    [SkinControlAttribute(2)]
    protected GUILabelControl mainLabel = null;
    [SkinControlAttribute(50)]
    protected GUIFacadeControl facadeView = null;

    #endregion Skin Elements

    #region Constants

    const int WindowID = 248101;

    internal const string PluginVersion = "MP Blast Zone Plugin 1.0.3.4 for IR Server";

    internal static readonly string MenuFile = Common.FolderAppData + "MP Blast Zone Plugin\\Menu.xml";

    internal static readonly string FolderMacros = Common.FolderAppData + "MP Blast Zone Plugin\\Macro\\";

    internal static readonly string MPConfigFile = Config.GetFolder(Config.Dir.Config) + "\\MediaPortal.xml";

    #endregion Constants

    #region Variables

    static Client _client;

    static MenuRoot _menu;

    static string _serverHost;
    static string _learnIRFilename = null;

    static bool _registered;

    static bool _logVerbose;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration;

    static bool _mpBasicHome;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static List<string> _macroStack;

    #endregion Variables

    #region Properties

    internal static MenuRoot Menu
    {
      get { return _menu; }
    }

    internal static bool IsRegistered
    {
      get { return _registered; }
    }

    internal static string ServerHost
    {
      get { return _serverHost; }
      set { _serverHost = value; }
    }
    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
    }

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static bool InConfiguration
    {
      get { return _inConfiguration; }
      set { _inConfiguration = value; }
    }

    // MediaPortal Settings
    internal static bool MP_BasicHome
    {
      get { return _mpBasicHome; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    #endregion Properties

    #region Constructor

    public MPBlastZonePlugin()
    {
      // Load basic settings
      LoadSettings();

      // Setup Menu Details
      _menu = new MenuRoot(MenuFile);
    }

    #endregion Constructor

    #region ISetupForm Members

    public bool CanEnable()       { return true; }
    public bool HasSetup()        { return true; }
    public string PluginName()    { return "MP Blast Zone Plugin for IR Server"; }
    public bool DefaultEnabled()  { return true; }
    public int GetWindowId()      { return WindowID; }
    public string Author()        { return "and-81"; }
    public string Description()   { return "This is a window plugin that uses the IR Server to control various pieces of equipment"; }

    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
    {
      strButtonText       = "Blast Zone";
      strButtonImage      = String.Empty;
      strButtonImageFocus = String.Empty;
      strPictureImage     = "hover_blastzone.png";
      return true;
    }

    public void ShowPlugin()
    {
      try
      {
        InConfiguration = true;

        if (LogVerbose)
          Log.Info("MPBlastZonePlugin: ShowPlugin()");

        SetupForm setupForm = new SetupForm();
        if (setupForm.ShowDialog() == DialogResult.OK)
          SaveSettings();

        StopClient();

        if (LogVerbose)
          Log.Info("MPBlastZonePlugin: ShowPlugin() - End");
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    #endregion

    #region GUIWindow Members

    public override int GetID
    {
      get { return WindowID; }
      set { }
    }

    public override bool Init()
    {
      InConfiguration = false;

      Log.Info("MPBlastZonePlugin: Starting ({0})", PluginVersion);

      IPAddress serverIP = Client.GetIPFromName(MPBlastZonePlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("MPBlastZonePlugin: Failed to start local comms, IR blasting is disabled for this session");

      if (Load(GUIGraphicsContext.Skin + "\\BlastZone.xml"))
      {
        if (LogVerbose)
          Log.Info("MPBlastZonePlugin: Started");

        return true;
      }
      else
      {
        Log.Error("MPBlastZonePlugin: Failed to load skin file.");
        return false;
      }
    }

    public override void DeInit()
    {
      StopClient();

      base.DeInit();

      if (LogVerbose)
        Log.Info("MPBlastZonePlugin: Stopped");
    }

    protected override void OnPageLoad()
    {
      if (facadeView.Count == 0)
        PopulateListControl("\\");
    }

    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      if (control == facadeView)
      {
        if (actionType == Action.ActionType.ACTION_SELECT_ITEM)
        {
          if (facadeView.SelectedListItem.IsFolder)
          {
            if (facadeView.SelectedListItem.Label == "..")
              PopulateListControl("\\");
            else
              PopulateListControl(facadeView.SelectedListItem.Label);
          }
          else
          {
            string command = GetCommand(facadeView.SelectedListItem.Path, facadeView.SelectedListItem.Label);
            ProcessCommand(command);
          }
        }
      }

      base.OnClicked(controlId, control, actionType);
    }

    public override void OnAction(Action action)
    {
      if (action.wID == Action.ActionType.ACTION_PREVIOUS_MENU)
      {
        GUIWindowManager.ShowPreviousWindow();
        return;
      }

      base.OnAction(action);
    }

    #endregion GUIWindow Members

    #region Implementation

    static string GetCommand(string path, string name)
    {
      foreach (string collection in _menu.GetAllItems())
        if (collection == path)
          foreach (string command in _menu.GetItem(collection).GetAllItems())
            if (command == name)
              return _menu.GetItem(collection).GetItem(command).Command;

      return null;
    }

    void PopulateListControl(string path)
    {
      if (path == "\\")
        GUIControl.SetControlLabel(WindowID, mainLabel.GetID, "Blast Zone");
      else
        GUIControl.SetControlLabel(WindowID, mainLabel.GetID, path);

      GUIControl.ClearControl(WindowID, facadeView.GetID);

      GUIListItem item;

      foreach (string collection in _menu.GetAllItems())
      {
        if (path == "\\")
        {
          item = new GUIListItem(collection);
          item.IsFolder = true;
          item.Path = "\\";
          item.IconImage = "defaultFolder.png";
          item.IconImageBig = "defaultFolderBig.png";
          facadeView.Add(item);
        }
        else if (collection == path)
        {
          item = new GUIListItem("..");
          item.IsFolder = true;
          item.Path = "\\";
          item.IconImage = "defaultFolderBack.png";
          item.IconImageBig = "defaultFolderBackBig.png";
          facadeView.Add(item);

          foreach (string command in _menu.GetItem(collection).GetAllItems())
          {
            item = new GUIListItem(command);
            item.IsFolder = false;
            item.Path = collection;
            item.IconImage = "check-box.png";
            facadeView.Add(item);
          }
        }
      }

      //string strObjects = String.Format("{0} {1}", GUIControl.GetItemCount(GetID, (int)Controls.CONTROL_LIST_DIR).ToString(), GUILocalizeStrings.Get(632));
      //GUIPropertyManager.SetProperty("#itemcount", strObjects);
    }

    static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;
      
      if (ex != null)
        Log.Error("MPBlastZonePlugin: Communications failure: {0}", ex.Message);
      else
        Log.Error("MPBlastZonePlugin: Communications failure");

      StopClient();

      Log.Warn("MPBlastZonePlugin: Attempting communications restart ...");

      IPAddress serverIP = Client.GetIPFromName(MPBlastZonePlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, IrssComms.Server.DefaultPort);

      StartClient(endPoint);
    }
    static void Connected(object obj)
    {
      Log.Info("MPBlastZonePlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }
    static void Disconnected(object obj)
    {
      Log.Warn("MPBlastZonePlugin: Communications with server has been lost");

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = new ClientMessageSink(ReceivedMessage);

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback  = new WaitCallback(CommsFailure);
      _client.ConnectCallback       = new WaitCallback(Connected);
      _client.DisconnectCallback    = new WaitCallback(Disconnected);
      
      if (_client.Start())
      {
        return true;
      }
      else
      {
        _client = null;
        return false;
      }
    }
    internal static void StopClient()
    {
      if (_client == null)
        return;

      _client.Dispose();
      _client = null;
    }

    static void ReceivedMessage(IrssMessage received)
    {
      if (LogVerbose)
        Log.Debug("MPBlastZonePlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("MPBlastZonePlugin: Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Warn("MPBlastZonePlugin: Failed to blast IR command");
            }
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              _irServerInfo = IRServerInfo.FromBytes(received.GetDataAsBytes());
              _registered = true;

              if (LogVerbose)
                Log.Info("MPBlastZonePlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              _registered = false;
              Log.Warn("MPBlastZonePlugin: IR Server refused to register");
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              if (LogVerbose)
                Log.Info("MPBlastZonePlugin: Learned IR Successfully");

              byte[] dataBytes = received.GetDataAsBytes();

              using (FileStream file = File.Create(_learnIRFilename))
                file.Write(dataBytes, 0, dataBytes.Length);
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Error("MPBlastZonePlugin: Failed to learn IR command");
            }
            else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
            {
              Log.Error("MPBlastZonePlugin: Learn IR command timed-out");
            }
            
            _learnIRFilename = null;
            break;

          case MessageType.ServerShutdown:
            Log.Warn("MPBlastZonePlugin: IR Server Shutdown - Plugin disabled until IR Server returns");
            _registered = false;
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("MPBlastZonePlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (_handleMessage != null)
          _handleMessage(received);
      }
      catch (Exception ex)
      {
        Log.Error("MPBlastZonePlugin - ReveivedMessage(): {0}", ex.Message);
      }
    }

    static void LoadSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("MPBlastZonePlugin", "ServerHost", String.Empty);

          LogVerbose = xmlreader.GetValueAsBool("MPBlastZonePlugin", "LogVerbose", false);

          // MediaPortal settings ...
          _mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPBlastZonePlugin: LoadSettings() {0}", ex.Message);
      }
    }
    static void SaveSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(MPConfigFile))
        {
          xmlwriter.SetValue("MPBlastZonePlugin", "ServerHost", ServerHost);

          xmlwriter.SetValueAsBool("MPBlastZonePlugin", "LogVerbose", LogVerbose);
        }
      }
      catch (Exception ex)
      {
        Log.Error("MPBlastZonePlugin: SaveSettings() {0}", ex.Message);
      }
    }

    static void MacroStackAdd(string fileName)
    {
      string lowerCasedFileName = fileName.ToLowerInvariant();

      if (_macroStack == null)
      {
        _macroStack = new List<string>();
      }
      else if (_macroStack.Contains(lowerCasedFileName))
      {
        StringBuilder macroStackTrace = new StringBuilder();
        macroStackTrace.AppendLine("Macro infinite loop detected!");
        macroStackTrace.AppendLine();
        macroStackTrace.AppendLine("Stack trace:");

        foreach (string macro in _macroStack)
        {
          if (macro.Equals(lowerCasedFileName))
            macroStackTrace.AppendLine(String.Format("--> {0}", macro));
          else
            macroStackTrace.AppendLine(macro);
        }

        macroStackTrace.AppendLine(String.Format("--> {0}", lowerCasedFileName));

        throw new ApplicationException(macroStackTrace.ToString());
      }

      _macroStack.Add(lowerCasedFileName);
    }
    static void MacroStackRemove(string fileName)
    {
      string lowerCasedFileName = fileName.ToLowerInvariant();

      if (_macroStack.Contains(lowerCasedFileName))
        _macroStack.Remove(lowerCasedFileName);

      if (_macroStack.Count == 0)
        _macroStack = null;
    }

    /// <summary>
    /// Process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    internal static void ProcessMacro(string fileName)
    {
      MacroStackAdd(fileName);

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");
        string commandProperty;
        string commandName;

        foreach (XmlNode item in commandSequence)
        {
          commandName = item.Attributes["command"].Value;
          commandProperty = item.Attributes["cmdproperty"].Value;

          switch (commandName)
          {
            case Common.XmlTagMacro:
              {
                ProcessMacro(FolderMacros + commandProperty + Common.FileExtensionMacro);
                break;
              }

            case Common.XmlTagBlast:
              {
                string[] commands = Common.SplitBlastCommand(commandProperty);
                BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
                break;
              }

            case Common.XmlTagPause:
              {
                int sleep = int.Parse(commandProperty);
                Thread.Sleep(sleep);
                break;
              }

            case Common.XmlTagRun:
              {
                string[] commands = Common.SplitRunCommand(commandProperty);
                Common.ProcessRunCommand(commands);
                break;
              }

            case Common.XmlTagSerial:
              {
                string[] commands = Common.SplitSerialCommand(commandProperty);
                Common.ProcessSerialCommand(commands);
                break;
              }

            case Common.XmlTagGoto:
              {
                if (InConfiguration)
                  MessageBox.Show(commandProperty, "Go To Window", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommands.ProcessGoTo(commandProperty, MP_BasicHome);
                break;
              }

            case Common.XmlTagPopup:
              {
                string[] commands = Common.SplitPopupCommand(commandProperty);

                if (InConfiguration)
                  MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommands.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));

                break;
              }

            case Common.XmlTagWindowMsg:
              {
                string[] commands = Common.SplitWindowMessageCommand(commandProperty);
                Common.ProcessWindowMessageCommand(commands);
                break;
              }

            /*
            case Common.XmlTagWindowState:
              {
                if (InConfiguration)
                {
                  MessageBox.Show("Command to toggle the window state cannot be processed in configuration.", "Window State Toggle Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SWITCH_FULL_WINDOWED, 0, 0, 0, 0, 0, null);

                if (GUIGraphicsContext.DX9Device.PresentationParameters.Windowed)
                  msg.Param1 = 1;

                GUIWindowManager.SendMessage(msg);
                break;
              }
            */
            case Common.XmlTagFocus:
              {
                if (InConfiguration)
                {
                  MessageBox.Show("Command to get focus cannot be processed in configuration.", "Get Focus Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                GUIGraphicsContext.ResetLastActivity();
                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GETFOCUS, 0, 0, 0, 0, 0, null);
                GUIWindowManager.SendThreadMessage(msg);
                break;
              }

            case Common.XmlTagExit:
              {
                if (!InConfiguration)
                  GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_EXIT, 0, 0));
                break;
              }

            case Common.XmlTagStandby:
              {
                if (!InConfiguration)
                {
                  GUIGraphicsContext.ResetLastActivity();
                  // Stop all media before suspending or hibernating
                  g_Player.Stop();

                  GUIMessage msg;

                  if (_mpBasicHome)
                    msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
                  else
                    msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_HOME, 0, null);

                  GUIWindowManager.SendThreadMessage(msg);

                  WindowsController.ExitWindows(RestartOptions.Suspend, true);
                }
                break;
              }

            case Common.XmlTagHibernate:
              {
                if (!InConfiguration)
                {
                  GUIGraphicsContext.ResetLastActivity();
                  // Stop all media before suspending or hibernating
                  g_Player.Stop();

                  GUIMessage msg;

                  if (_mpBasicHome)
                    msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
                  else
                    msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int)GUIWindow.Window.WINDOW_HOME, 0, null);

                  GUIWindowManager.SendThreadMessage(msg);

                  WindowsController.ExitWindows(RestartOptions.Hibernate, true);
                }
                break;
              }

            case Common.XmlTagReboot:
              {
                if (!InConfiguration)
                  GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_REBOOT, 0, 0));
                break;
              }

            case Common.XmlTagShutdown:
              {
                if (!InConfiguration)
                  GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_SHUTDOWN, 0, 0));
                break;
              }
          }
        }
      }
      finally
      {
        MacroStackRemove(fileName);
      }
    }

    /// <summary>
    /// Blast an IR command.
    /// </summary>
    /// <param name="fileName">File to blast (absolute path).</param>
    /// <param name="port">Port to blast to.</param>
    internal static void BlastIR(string fileName, string port)
    {
      if (!_registered)
        throw new ApplicationException("Cannot Blast, not registered to an active IR Server");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure", fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int)file.Length);

        IrssMessage message = new IrssMessage(MessageType.BlastIR, MessageFlags.Request, outData);
        _client.Send(message);
      }
    }

    /// <summary>
    /// Given a command this method processes the request accordingly.
    /// </summary>
    /// <param name="command">Command to process.</param>
    internal static void ProcessCommand(string command)
    {
      if (String.IsNullOrEmpty(command))
        throw new ArgumentNullException("command");

      if (command.StartsWith(Common.CmdPrefixMacro)) // Macro
      {
        string fileName = FolderMacros + command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro;
        ProcessMacro(fileName);
      }
      else if (command.StartsWith(Common.CmdPrefixBlast))  // IR Code
      {
        string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
        BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
      }
      else if (command.StartsWith(Common.CmdPrefixRun)) // External Program
      {
        string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
        Common.ProcessRunCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixSerial)) // Serial Port Command
      {
        string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
        Common.ProcessSerialCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixWindowMsg))  // Message Command
      {
        string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
        Common.ProcessWindowMessageCommand(commands);
      }
      else if (command.StartsWith(Common.CmdPrefixGoto)) // Go To Screen
      {
        MPCommands.ProcessGoTo(command.Substring(Common.CmdPrefixGoto.Length), MP_BasicHome);
      }
      else
      {
        throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
      }
    }

    /// <summary>
    /// Learn an IR Command and put it in a file
    /// </summary>
    /// <param name="fileName">File to place learned IR command in.</param>
    /// <returns>Success.</returns>
    internal static bool LearnIRCommand(string fileName)
    {
      try
      {
        if (String.IsNullOrEmpty(fileName))
        {
          Log.Error("MPBlastZonePlugin: Null or Empty file name for LearnIR()");
          return false;
        }

        if (!_registered)
        {
          Log.Warn("MPBlastZonePlugin: Not registered to an active IR Server");
          return false;
        }

        if (_learnIRFilename != null)
        {
          Log.Warn("MPBlastZonePlugin: Already trying to learn an IR command");
          return false;
        }

        _learnIRFilename = fileName;

        IrssMessage message = new IrssMessage(MessageType.LearnIR, MessageFlags.Request);
        _client.Send(message);

        return true;
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error("MPBlastZonePlugin - LearnIRCommand(): {0}", ex.Message);
        return false;
      }
    }

    /// <summary>
    /// Returns a list of Macros
    /// </summary>
    /// <returns>string[] of Macros.</returns>
    internal static string[] GetMacroList(bool commandPrefix)
    {
      string[] files = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] list = new string[files.Length];

      int i = 0;
      foreach (string file in files)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    /// <summary>
    /// Returns a combined list of IR Commands and Macros
    /// </summary>
    /// <returns>string[] of IR Commands and Macros.</returns>
    internal static string[] GetFileList(bool commandPrefix)
    {
      string[] MacroFiles = Directory.GetFiles(FolderMacros, '*' + Common.FileExtensionMacro);
      string[] IRFiles = Directory.GetFiles(Common.FolderIRCommands, '*' + Common.FileExtensionIR);
      string[] list = new string[MacroFiles.Length + IRFiles.Length];

      int i = 0;
      foreach (string file in MacroFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      foreach (string file in IRFiles)
      {
        if (commandPrefix)
          list[i++] = Common.CmdPrefixBlast + Path.GetFileNameWithoutExtension(file);
        else
          list[i++] = Path.GetFileNameWithoutExtension(file);
      }

      return list;
    }

    #endregion Implementation

  }

}
