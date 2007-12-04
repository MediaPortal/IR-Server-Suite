using System;
using System.Collections;
using System.Collections.Generic;
#if TRACE
using System.Diagnostics;
#endif
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

  /// <summary>
  /// MediaPortal Blast Zone Plugin for IR Server.
  /// </summary>
  public class MPBlastZonePlugin : GUIWindow, ISetupForm
  {

    #region Skin Elements

    /// <summary>
    /// Main GUI label.
    /// </summary>
    [SkinControlAttribute(2)]
    protected GUILabelControl mainLabel;
    
    /// <summary>
    /// Main GUI Facade View.
    /// </summary>
    [SkinControlAttribute(50)]
    protected GUIFacadeControl facadeView;

    #endregion Skin Elements

    #region Constants

    const int WindowID = 248101;

    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "MP Blast Zone Plugin 1.0.3.5 for IR Server";

    internal static readonly string MenuFile = Common.FolderAppData + "MP Blast Zone Plugin\\Menu.xml";

    internal static readonly string FolderMacros = Common.FolderAppData + "MP Blast Zone Plugin\\Macro\\";

    const string ProcessCommandThreadName = "ProcessCommand";

    #endregion Constants

    #region Variables

    static Client _client;

    static MenuRoot _menu;

    static string _serverHost;
    static string _learnIRFilename;

    static bool _registered;

    static bool _logVerbose;

    static ClientMessageSink _handleMessage;

    static bool _inConfiguration;

    static bool _mpBasicHome;

    static IRServerInfo _irServerInfo = new IRServerInfo();

    static Hashtable _macroStacks;

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

    /// <summary>
    /// Gets or sets a value indicating whether to log verbosely.
    /// </summary>
    /// <value><c>true</c> if logging is set to verbose; otherwise, <c>false</c>.</value>
    internal static bool LogVerbose
    {
      get { return _logVerbose; }
      set { _logVerbose = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether in configuration.
    /// </summary>
    /// <value><c>true</c> if in configuration; otherwise, <c>false</c>.</value>
    internal static bool InConfiguration
    {
      get { return _inConfiguration; }
      set { _inConfiguration = value; }
    }

    internal static ClientMessageSink HandleMessage
    {
      get { return _handleMessage; }
      set { _handleMessage = value; }
    }

    internal static IRServerInfo TransceiverInformation
    {
      get { return _irServerInfo; }
    }

    /// <summary>
    /// Gets a value indicating whether MediaPortal has basic home enabled.
    /// </summary>
    /// <value><c>true</c> if MediaPortal has basic home enabled; otherwise, <c>false</c>.</value>
    internal static bool MP_BasicHome
    {
      get { return _mpBasicHome; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="MPBlastZonePlugin"/> class.
    /// </summary>
    public MPBlastZonePlugin()
    {
      // Load basic settings
      LoadSettings();

      // Setup Menu Details
      _menu = new MenuRoot(MenuFile);

      // Hashtable for storing active macro stacks in.
      _macroStacks = new Hashtable();
    }

    #endregion Constructor

    #region ISetupForm methods

    /// <summary>
    /// Determines whether this plugin can be enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin can be enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool CanEnable()       { return true; }
    /// <summary>
    /// Determines whether this plugin has setup.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin has setup; otherwise, <c>false</c>.
    /// </returns>
    public bool HasSetup()        { return true; }
    /// <summary>
    /// Gets the plugin name.
    /// </summary>
    /// <returns>The plugin name.</returns>
    public string PluginName()    { return "MP Blast Zone Plugin for IR Server"; }
    /// <summary>
    /// Defaults enabled.
    /// </summary>
    /// <returns>true if this plugin is enabled by default, otherwise false.</returns>
    public bool DefaultEnabled()  { return true; }
    /// <summary>
    /// Gets the window id.
    /// </summary>
    /// <returns>The window id.</returns>
    public int GetWindowId()      { return WindowID; }
    /// <summary>
    /// Gets the plugin author.
    /// </summary>
    /// <returns>The plugin author.</returns>
    public string Author()        { return "and-81"; }
    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    /// <returns>The plugin description.</returns>
    public string Description()   { return "This is a window plugin that uses the IR Server to control various pieces of equipment"; }

    /// <summary>
    /// Shows the plugin configuration.
    /// </summary>
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

    /// <summary>
    /// Gets the home screen details for the plugin.
    /// </summary>
    /// <param name="strButtonText">The button text.</param>
    /// <param name="strButtonImage">The button image.</param>
    /// <param name="strButtonImageFocus">The button image focus.</param>
    /// <param name="strPictureImage">The picture image.</param>
    /// <returns>true if the plugin can be seen, otherwise false.</returns>
    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
    {
      strButtonText       = "Blast Zone";
      strButtonImage      = String.Empty;
      strButtonImageFocus = String.Empty;
      strPictureImage     = "hover_blastzone.png";
      return true;
    }

    #endregion ISetupForm methods

    #region GUIWindow Members

    /// <summary>
    /// Gets the GUI Window ID.
    /// </summary>
    /// <value>The GUI Window ID.</value>
    public override int GetID
    {
      get { return WindowID; }
      set { }
    }

    /// <summary>
    /// Inits this GUI Window.
    /// </summary>
    /// <returns><c>true</c> if successfully initialized, otherwise <c>false</c>.</returns>
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

    /// <summary>
    /// Deinitializes this GUI Window.
    /// </summary>
    public override void DeInit()
    {
      StopClient();

      base.DeInit();

      if (LogVerbose)
        Log.Info("MPBlastZonePlugin: Stopped");
    }

    /// <summary>
    /// Called when the page loads.
    /// </summary>
    protected override void OnPageLoad()
    {
      if (facadeView.Count == 0)
        PopulateListControl("\\");
    }

    /// <summary>
    /// Called when a control is clicked.
    /// </summary>
    /// <param name="controlId">The control id.</param>
    /// <param name="control">The control.</param>
    /// <param name="actionType">Type of the action.</param>
    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      if (control == facadeView)
      {
        if (actionType == Action.ActionType.ACTION_SELECT_ITEM)
        {
          if (facadeView.SelectedListItem.IsFolder)
          {
            if (facadeView.SelectedListItem.Label.Equals("..", StringComparison.Ordinal))
              PopulateListControl("\\");
            else
              PopulateListControl(facadeView.SelectedListItem.Label);
          }
          else
          {
            string command = GetCommand(facadeView.SelectedListItem.Path, facadeView.SelectedListItem.Label);
            ProcessCommand(command, true);
          }
        }
      }

      base.OnClicked(controlId, control, actionType);
    }

    /// <summary>
    /// Called when an action is performed.
    /// </summary>
    /// <param name="action">The action.</param>
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
        if (collection.Equals(path, StringComparison.OrdinalIgnoreCase))
          foreach (string command in _menu.GetItem(collection).GetAllItems())
            if (command.Equals(name, StringComparison.OrdinalIgnoreCase))
              return _menu.GetItem(collection).GetItem(command).Command;

      return null;
    }

    void PopulateListControl(string path)
    {
      if (path.Equals("\\", StringComparison.Ordinal))
        GUIControl.SetControlLabel(WindowID, mainLabel.GetID, "Blast Zone");
      else
        GUIControl.SetControlLabel(WindowID, mainLabel.GetID, path);

      GUIControl.ClearControl(WindowID, facadeView.GetID);

      GUIListItem item;

      foreach (string collection in _menu.GetAllItems())
      {
        if (path.Equals("\\", StringComparison.Ordinal))
        {
          item = new GUIListItem(collection);
          item.IsFolder = true;
          item.Path = "\\";
          item.IconImage = "defaultFolder.png";
          item.IconImageBig = "defaultFolderBig.png";
          facadeView.Add(item);
        }
        else if (collection.Equals(path, StringComparison.OrdinalIgnoreCase))
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

      IPAddress serverIP = Client.GetIPFromName(_serverHost);
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
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
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

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns>true if successful, otherwise false.</returns>
    internal static bool LearnIR(string fileName)
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
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error("MPBlastZonePlugin - LearnIRCommand(): {0}", ex.Message);
        return false;
      }

      return true;
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
    /// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    /// </summary>
    /// <param name="command">Command to process.</param>
    /// <param name="async">Process command asynchronously?</param>
    internal static void ProcessCommand(string command, bool async)
    {
      if (async)
      {
        try
        {
          Thread newThread = new Thread(new ParameterizedThreadStart(ProcCommand));
          newThread.Name = ProcessCommandThreadName;
          newThread.Priority = ThreadPriority.BelowNormal;
          newThread.Start(command);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex.ToString());
        }
      }
      else
      {
        ProcCommand(command);
      }
    }

    /// <summary>
    /// Used by ProcessCommand to actually handle the command.
    /// Can be called Synchronously or as a Parameterized Thread.
    /// </summary>
    /// <param name="commandObj">Command string to process.</param>
    static void ProcCommand(object commandObj)
    {
      try
      {
        if (commandObj == null)
          throw new ArgumentNullException("commandObj");

        string command = commandObj as string;

        if (String.IsNullOrEmpty(command))
          throw new ArgumentException("commandObj translates to empty or null string", "commandObj");

        if (command.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
        {
          string fileName = FolderMacros + command.Substring(Common.CmdPrefixMacro.Length) + Common.FileExtensionMacro;
          ProcMacro(fileName);
        }
        else if (command.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitBlastCommand(command.Substring(Common.CmdPrefixBlast.Length));
          BlastIR(Common.FolderIRCommands + commands[0] + Common.FileExtensionIR, commands[1]);
        }
        else if (command.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitRunCommand(command.Substring(Common.CmdPrefixRun.Length));
          Common.ProcessRunCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitSerialCommand(command.Substring(Common.CmdPrefixSerial.Length));
          Common.ProcessSerialCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
        {
          string[] commands = Common.SplitWindowMessageCommand(command.Substring(Common.CmdPrefixWindowMsg.Length));
          Common.ProcessWindowMessageCommand(commands);
        }
        else if (command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
        {
          string keyCommand = command.Substring(Common.CmdPrefixKeys.Length);
          if (InConfiguration)
            MessageBox.Show(keyCommand, Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            Common.ProcessKeyCommand(keyCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
        {
          string mouseCommand = command.Substring(Common.CmdPrefixMouse.Length);
          Common.ProcessMouseCommand(mouseCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
        {
          string ejectCommand = command.Substring(Common.CmdPrefixEject.Length);
          Common.ProcessEjectCommand(ejectCommand);
        }
        else if (command.StartsWith(Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase))
        {
          if (InConfiguration)
            MessageBox.Show("Cannot Hibernate in configuration", Common.UITextHibernate, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.Hibernate();
        }
        else if (command.StartsWith(Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase))
        {
          if (InConfiguration)
            MessageBox.Show("Cannot Reboot in configuration", Common.UITextReboot, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.Reboot();
        }
        else if (command.StartsWith(Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase))
        {
          if (InConfiguration)
            MessageBox.Show("Cannot Shutdown in configuration", Common.UITextShutdown, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.ShutDown();
        }
        else if (command.StartsWith(Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase))
        {
          if (InConfiguration)
            MessageBox.Show("Cannot enter Standby in configuration", Common.UITextStandby, MessageBoxButtons.OK, MessageBoxIcon.Information);
          else
            MPCommon.Standby();
        }
        else if (command.StartsWith(Common.CmdPrefixGoto, StringComparison.OrdinalIgnoreCase))
        {
          MPCommon.ProcessGoTo(command.Substring(Common.CmdPrefixGoto.Length), MP_BasicHome);
        }
        else
        {
          throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command), "command");
        }
      }
      catch (Exception ex)
      {
        if (Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
          Log.Error(ex.ToString());
        else
          throw ex;
      }
    }

    /// <summary>
    /// Process the supplied Macro file.
    /// </summary>
    /// <param name="fileName">Macro file to process (absolute path).</param>
    static void ProcMacro(string fileName)
    {
      MacroStackAdd(Thread.CurrentThread.ManagedThreadId, fileName);

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        if (doc.DocumentElement.InnerText.Contains(Common.XmlTagBlast) && !_registered)
          throw new ApplicationException("Cannot process Macro with Blast commands when not registered to an active IR Server");

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");
        string commandProperty;

        foreach (XmlNode item in commandSequence)
        {
          commandProperty = item.Attributes["cmdproperty"].Value;

          switch (item.Attributes["command"].Value)
          {
            case Common.XmlTagMacro:
              {
                ProcMacro(FolderMacros + commandProperty + Common.FileExtensionMacro);
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
                  MessageBox.Show(commandProperty, Common.UITextGoto, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.ProcessGoTo(commandProperty, MP_BasicHome);
                break;
              }

            case Common.XmlTagPopup:
              {
                string[] commands = Common.SplitPopupCommand(commandProperty);

                if (InConfiguration)
                  MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));

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
                  MessageBox.Show("Command to toggle the window state cannot be processed in configuration.", Common.UITextWindowState, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                  MessageBox.Show("Command to get focus cannot be processed in configuration.", Common.UITextFocus, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  break;
                }

                GUIGraphicsContext.ResetLastActivity();
                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GETFOCUS, 0, 0, 0, 0, 0, null);
                GUIWindowManager.SendThreadMessage(msg);
                break;
              }

            case Common.XmlTagExit:
              {
                if (InConfiguration)
                  MessageBox.Show("Cannot exit MediaPortal while in configuration", Common.UITextExit, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  GUIGraphicsContext.OnAction(new Action(Action.ActionType.ACTION_EXIT, 0, 0));
                break;
              }

            case Common.XmlTagEject:
              {
                Common.ProcessEjectCommand(commandProperty);
                break;
              }

            case Common.XmlTagStandby:
              {
                if (InConfiguration)
                  MessageBox.Show("Cannot enter Standby in configuration", Common.UITextStandby, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.Standby();
                break;
              }

            case Common.XmlTagHibernate:
              {
                if (InConfiguration)
                  MessageBox.Show("Cannot Hibernate in configuration", Common.UITextHibernate, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.Hibernate();
                break;
              }

            case Common.XmlTagShutdown:
              {
                if (InConfiguration)
                  MessageBox.Show("Cannot ShutDown in configuration", Common.UITextShutdown, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.ShutDown();
                break;
              }

            case Common.XmlTagReboot:
              {
                if (InConfiguration)
                  MessageBox.Show("Cannot Reboot in configuration", Common.UITextReboot, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                  MPCommon.Reboot();
                break;
              }
          }
        }
      }
      finally
      {
        MacroStackRemove(Thread.CurrentThread.ManagedThreadId, fileName);
      }
    }
    
    /// <summary>
    /// Retreives the required Macro Stack from the Hashtable.
    /// </summary>
    /// <param name="hash">Hash table lookup value.</param>
    /// <returns>Macro Stack.</returns>
    static List<string> GetMacroStack(int hash)
    {
      if (_macroStacks.ContainsKey(hash))
      {
        return (List<string>)_macroStacks[hash];
      }
      else
      {
        List<string> newStack = new List<string>();
        _macroStacks.Add(hash, newStack);
        return newStack;
      }
    }

    /// <summary>
    /// Adds to the Macro Stack.
    /// </summary>
    /// <param name="hash">Hash table lookup value.</param>
    /// <param name="fileName">Name of the macro file.</param>
    static void MacroStackAdd(int hash, string fileName)
    {
      List<string> stack = GetMacroStack(hash);

      string upperCasedFileName = fileName.ToUpperInvariant();

      if (stack.Contains(upperCasedFileName))
      {
        StringBuilder macroStackTrace = new StringBuilder();
        macroStackTrace.AppendLine("Macro infinite loop detected!");
        macroStackTrace.AppendLine();
        macroStackTrace.AppendLine("Stack trace:");

        foreach (string macro in stack)
        {
          if (macro.Equals(upperCasedFileName))
            macroStackTrace.AppendLine(String.Format("--> {0}", macro));
          else
            macroStackTrace.AppendLine(macro);
        }

        macroStackTrace.AppendLine(String.Format("--> {0}", upperCasedFileName));

        throw new ApplicationException(macroStackTrace.ToString());
      }

      stack.Add(upperCasedFileName);
    }

    /// <summary>
    /// Removes from the Macro Stack.
    /// </summary>
    /// <param name="hash">Hash table lookup value.</param>
    /// <param name="fileName">Name of the macro file.</param>
    static void MacroStackRemove(int hash, string fileName)
    {
      List<string> stack = GetMacroStack(hash);

      string upperCasedFileName = fileName.ToUpperInvariant();

      if (stack.Contains(upperCasedFileName))
        stack.Remove(upperCasedFileName);

      if (stack.Count == 0)
        _macroStacks.Remove(hash);
    }

    /// <summary>
    /// Returns a list of Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
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
    /// Returns a combined list of IR Commands and Macros.
    /// </summary>
    /// <param name="commandPrefix">Add the command prefix to each list item.</param>
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

    /// <summary>
    /// Loads the settings.
    /// </summary>
    static void LoadSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(MPCommon.MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("MPBlastZonePlugin", "ServerHost", "localhost");

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
    /// <summary>
    /// Saves the settings.
    /// </summary>
    static void SaveSettings()
    {
      try
      {
        using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(MPCommon.MPConfigFile))
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

    #endregion Implementation

  }

}
