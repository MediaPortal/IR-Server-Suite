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
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IrssCommands;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Plugins.IRSS.MPBlastZonePlugin.Forms;
using MediaPortal.Profile;
using MPUtils;
using Action = MediaPortal.GUI.Library.Action;

namespace MediaPortal.Plugins.IRSS.MPBlastZonePlugin
{
  /// <summary>
  /// MediaPortal Blast Zone Plugin for IR Server.
  /// </summary>
  [PluginIcons("MediaPortal.Plugins.IRSS.MPBlastZonePlugin.iconGreen.gif",
    "MediaPortal.Plugins.IRSS.MPBlastZonePlugin.iconGray.gif")]
  public class MPBlastZonePlugin : GUIWindow, ISetupForm
  {
    #region Skin Elements

    /// <summary>
    /// Main GUI Facade View.
    /// </summary>
    [SkinControl(50)] protected GUIFacadeControl facadeView;

    #endregion Skin Elements

    #region Constants

    private const string GUI_PROPERTY_MAIN_LABEL = "#BLAST_ZONE.MAIN_LABEL";

    private const int WindowID = 248101;


    /// <summary>
    /// The plugin version string.
    /// </summary>
    internal const string PluginVersion = "MP Blast Zone Plugin 1.4.2.0 for IR Server";

    private const string ProcessCommandThreadName = "ProcessCommand";
    internal static readonly string FolderMacros = Path.Combine(IrssUtils.Common.FolderAppData, "MP Blast Zone Plugin\\Macro");
    internal static readonly string MenuFile = Path.Combine(IrssUtils.Common.FolderAppData, "MP Blast Zone Plugin\\Menu.xml");

    internal static readonly string[] CommandCategories = new string[]
        {
          Processor.CategoryGeneral, Processor.CategorySpecial, Processor.CategoryMediaPortal
        };

    internal static readonly string[] MacroCategories = new string[]
        {
          Processor.CategoryGeneral, Processor.CategoryIRCommands, Processor.CategoryMacros, Processor.CategoryMediaPortal,
          Processor.CategoryControl, Processor.CategoryMaths, Processor.CategoryStack, Processor.CategoryString, Processor.CategoryVariable
        };

    #endregion Constants

    #region Variables

    private static Client _client;

    private static string _learnIRFilename;
    private static bool _mpBasicHome;

    #endregion Variables

    #region Properties

    internal static MenuRoot Menu { get; private set; }

    internal static bool IsRegistered { get; private set; }

    internal static string ServerHost { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether in configuration.
    /// </summary>
    /// <value><c>true</c> if in configuration; otherwise, <c>false</c>.</value>
    internal static bool InConfiguration { get; set; }

    internal static ClientMessageSink HandleMessage { get; set; }

    internal static IRServerInfo TransceiverInformation { get; private set; }

    internal static Processor CommandProcessor { get; private set; }

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
      Menu = MenuRoot.Load(MenuFile);
    }

    static MPBlastZonePlugin()
    {
      TransceiverInformation = new IRServerInfo();

      // Set directory for command plugin
      CommandProcessor = new Processor(BlastIR, TransceiverInformation.Ports);
      string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      Processor.LibraryFolder = Path.Combine(dllPath, "Commands");
      Processor.MacroFolder = FolderMacros;
    }

    #endregion Constructor

    #region ISetupForm methods

    /// <summary>
    /// Determines whether this plugin can be enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin can be enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool CanEnable()
    {
      return true;
    }

    /// <summary>
    /// Determines whether this plugin has setup.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this plugin has setup; otherwise, <c>false</c>.
    /// </returns>
    public bool HasSetup()
    {
      return true;
    }

    /// <summary>
    /// Gets the plugin name.
    /// </summary>
    /// <returns>The plugin name.</returns>
    public string PluginName()
    {
      return "MP Blast Zone Plugin for IR Server";
    }

    /// <summary>
    /// Defaults enabled.
    /// </summary>
    /// <returns>true if this plugin is enabled by default, otherwise false.</returns>
    public bool DefaultEnabled()
    {
      return true;
    }

    /// <summary>
    /// Gets the window id.
    /// </summary>
    /// <returns>The window id.</returns>
    public int GetWindowId()
    {
      return WindowID;
    }

    /// <summary>
    /// Gets the plugin author.
    /// </summary>
    /// <returns>The plugin author.</returns>
    public string Author()
    {
      return "and-81";
    }

    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    /// <returns>The plugin description.</returns>
    public string Description()
    {
      return "This is a window plugin that uses the IR Server to control various pieces of equipment";
    }

    /// <summary>
    /// Shows the plugin configuration.
    /// </summary>
    public void ShowPlugin()
    {
        InConfiguration = true;

        Log.Debug("MPBlastZonePlugin: ShowPlugin()");

        SetupForm setupForm = new SetupForm();
        if (setupForm.ShowDialog() == DialogResult.OK)
        {
          SaveSettings();
          MenuRoot.Save(Menu, MenuFile);
        }

        StopClient();

        Log.Debug("MPBlastZonePlugin: ShowPlugin() - End");
    }

    /// <summary>
    /// Gets the home screen details for the plugin.
    /// </summary>
    /// <param name="strButtonText">The button text.</param>
    /// <param name="strButtonImage">The button image.</param>
    /// <param name="strButtonImageFocus">The button image focus.</param>
    /// <param name="strPictureImage">The picture image.</param>
    /// <returns>true if the plugin can be seen, otherwise false.</returns>
    public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus,
                        out string strPictureImage)
    {
      strButtonText = "Blast Zone";
      strButtonImage = String.Empty;
      strButtonImageFocus = String.Empty;
      strPictureImage = "hover_blastzone.png";
      return true;
    }

    #endregion ISetupForm methods

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

      IPAddress serverIP = Network.GetIPFromName(ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!StartClient(endPoint))
        Log.Error("MPBlastZonePlugin: Failed to start local comms, IR blasting is disabled for this session");

      string skinFile = Path.Combine(GUIGraphicsContext.Skin, "BlastZone.xml");

      if (Load(skinFile))
      {
        Log.Debug("MPBlastZonePlugin: Started");

        return true;
      }

      Log.Error("MPBlastZonePlugin: Failed to load skin file.");
      return false;
    }

    /// <summary>
    /// Deinitializes this GUI Window.
    /// </summary>
    public override void DeInit()
    {
      StopClient();

      base.DeInit();

      Log.Debug("MPBlastZonePlugin: Stopped");
    }

    /// <summary>
    /// Called when the page loads.
    /// </summary>
    protected override void OnPageLoad()
    {
      if (facadeView.Count == 0)
        PopulateListControl(Menu);
    }

    /// <summary>
    /// Called when a control is clicked.
    /// </summary>
    /// <param name="controlId">The control id.</param>
    /// <param name="control">The control.</param>
    /// <param name="actionType">Type of the action.</param>
    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      if (control == facadeView && actionType == Action.ActionType.ACTION_SELECT_ITEM &&
          facadeView.SelectedListItem.MusicTag != null)
      {
        MenuRoot menu = facadeView.SelectedListItem.MusicTag as MenuRoot;
        if (!ReferenceEquals(menu, null))
        {
          PopulateListControl(menu);
        }

        MenuFolder collection = facadeView.SelectedListItem.MusicTag as MenuFolder;
        if (!ReferenceEquals(collection, null))
        {
          PopulateListControl(collection);
        }

        Command command = facadeView.SelectedListItem.MusicTag as Command;
        if (!ReferenceEquals(command, null))
        {
          CommandProcessor.Execute(command, true);
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

    #region Implementation

    private void PopulateListControl(MenuRoot menu)
    {
      GUIPropertyManager.SetProperty(GUI_PROPERTY_MAIN_LABEL, "Blast Zone");
      GUIControl.ClearControl(WindowID, facadeView.GetID);

      foreach (MenuFolder collection in menu.Items)
      {
        GUIListItem item = new GUIListItem(collection.Name);
        item.IsFolder = true;
        item.Path = "\\";
        item.IconImage = "defaultFolder.png";
        item.IconImageBig = "defaultFolderBig.png";
        item.MusicTag = collection;
        facadeView.Add(item);
      }
    }

    private void PopulateListControl(MenuFolder collection)
    {
      GUIPropertyManager.SetProperty(GUI_PROPERTY_MAIN_LABEL, collection.Name);
      GUIControl.ClearControl(WindowID, facadeView.GetID);

      GUIListItem item = new GUIListItem("..");
      item.IsFolder = true;
      item.Path = "\\";
      item.IconImage = "defaultFolderBack.png";
      item.IconImageBig = "defaultFolderBackBig.png";
      item.MusicTag = Menu;
      facadeView.Add(item);

      foreach (MenuCommand command in collection.Items)
      {
        if (!command.IsCommandAvailable) continue;

        item = new GUIListItem(command.Name);
        item.IsFolder = false;
        item.Path = "\\";
        item.IconImage = "check-box.png";
        item.IconImageBig = "check-box.png";
        item.MusicTag = command.Command;
        facadeView.Add(item);
      }
    }

    private static void CommsFailure(object obj)
    {
      Exception ex = obj as Exception;

      if (ex != null)
        Log.Error("MPBlastZonePlugin: Communications failure: {0}", ex.ToString());
      else
        Log.Error("MPBlastZonePlugin: Communications failure");

      StopClient();

      Log.Warn("MPBlastZonePlugin: Attempting communications restart ...");

      IPAddress serverIP = Network.GetIPFromName(ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      StartClient(endPoint);
    }

    private static void Connected(object obj)
    {
      Log.Info("MPBlastZonePlugin: Connected to server");

      IrssMessage message = new IrssMessage(MessageType.RegisterClient, MessageFlags.Request);
      _client.Send(message);
    }

    private static void Disconnected(object obj)
    {
      Log.Warn("MPBlastZonePlugin: Communications with server has been lost");

      Thread.Sleep(1000);
    }

    internal static bool StartClient(IPEndPoint endPoint)
    {
      if (_client != null)
        return false;

      ClientMessageSink sink = ReceivedMessage;

      _client = new Client(endPoint, sink);
      _client.CommsFailureCallback = CommsFailure;
      _client.ConnectCallback = Connected;
      _client.DisconnectCallback = Disconnected;

      if (_client.Start())
        return true;

      _client = null;
      return false;
    }

    internal static void StopClient()
    {
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
    }

    private static void ReceivedMessage(IrssMessage received)
    {
      Log.Debug("MPBlastZonePlugin: Received Message \"{0}\"", received.Type);

      try
      {
        switch (received.Type)
        {
          case MessageType.BlastIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Log.Debug("MPBlastZonePlugin: Blast successful");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              Log.Warn("MPBlastZonePlugin: Failed to blast IR command");
            }
            break;

          case MessageType.RegisterClient:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              TransceiverInformation = IRServerInfo.FromBytes(received.GetDataAsBytes());
              IsRegistered = true;

              Log.Debug("MPBlastZonePlugin: Registered to IR Server");
            }
            else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
            {
              IsRegistered = false;
              Log.Warn("MPBlastZonePlugin: IR Server refused to register");
            }
            break;

          case MessageType.LearnIR:
            if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
            {
              Log.Debug("MPBlastZonePlugin: Learned IR Successfully");

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
            IsRegistered = false;
            break;

          case MessageType.Error:
            _learnIRFilename = null;
            Log.Error("MPBlastZonePlugin: Received error: {0}", received.GetDataAsString());
            break;
        }

        if (HandleMessage != null)
          HandleMessage(received);
      }
      catch (Exception ex)
      {
        _learnIRFilename = null;
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Learn an IR command.
    /// </summary>
    /// <param name="fileName">File to place learned IR command in (absolute path).</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    internal static bool LearnIR(string fileName)
    {
      try
      {
        if (String.IsNullOrEmpty(fileName))
        {
          Log.Error("MPBlastZonePlugin: Null or Empty file name for LearnIR()");
          return false;
        }

        if (!IsRegistered)
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
        Log.Error(ex);
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
      Log.Debug("MPControlPlugin - BlastIR(): {0}, {1}", fileName, port);

      if (!IsRegistered)
        throw new InvalidOperationException("Cannot Blast, not registered to an active IR Server");

      using (FileStream file = File.OpenRead(fileName))
      {
        if (file.Length == 0)
          throw new IOException(String.Format("Cannot Blast. IR file \"{0}\" has no data, possible IR learn failure",
                                              fileName));

        byte[] outData = new byte[4 + port.Length + file.Length];

        BitConverter.GetBytes(port.Length).CopyTo(outData, 0);
        Encoding.ASCII.GetBytes(port).CopyTo(outData, 4);

        file.Read(outData, 4 + port.Length, (int) file.Length);

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
    internal static void ProcessCommand(Command command, bool async = false)
    {
      CommandProcessor.Execute(command, async);
    }

    ///// <summary>
    ///// Given a command this method processes the request accordingly.
    ///// Throws exceptions only if run synchronously, if async exceptions are logged instead.
    ///// </summary>
    ///// <param name="command">Command to process.</param>
    ///// <param name="async">Process command asynchronously?</param>
    //internal static void ProcessCommand(string command, bool async)
    //{
    //  if (async)
    //  {
    //    try
    //    {
    //      Thread newThread = new Thread(ProcCommand);
    //      newThread.Name = ProcessCommandThreadName;
    //      newThread.IsBackground = true;
    //      newThread.Start(command);
    //    }
    //    catch (Exception ex)
    //    {
    //      Log.Error(ex);
    //    }
    //  }
    //  else
    //  {
    //    ProcCommand(command);
    //  }
    //}

    ///// <summary>
    ///// Used by ProcessCommand to actually handle the command.
    ///// Can be called Synchronously or as a Parameterized Thread.
    ///// </summary>
    ///// <param name="commandObj">Command string to process.</param>
    //private static void ProcCommand(object commandObj)
    //{
    //  try
    //  {
    //    if (commandObj == null)
    //      throw new ArgumentNullException("commandObj");

    //    string command = commandObj as string;

    //    if (String.IsNullOrEmpty(command))
    //      throw new ArgumentException("commandObj translates to empty or null string", "commandObj");

    //    if (command.StartsWith(IrssUtils.Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string fileName = Path.Combine(FolderMacros,
    //                                     command.Substring(IrssUtils.Common.CmdPrefixMacro.Length) + IrssUtils.Common.FileExtensionMacro);
    //      ProcMacro(fileName);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitBlastCommand(command.Substring(IrssUtils.Common.CmdPrefixBlast.Length));
    //      BlastIR(Path.Combine(IrssUtils.Common.FolderIRCommands, commands[0] + IrssUtils.Common.FileExtensionIR), commands[1]);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixSTB, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitBlastCommand(command.Substring(IrssUtils.Common.CmdPrefixSTB.Length));
    //      BlastIR(Path.Combine(IrssUtils.Common.FolderSTB, commands[0] + IrssUtils.Common.FileExtensionIR), commands[1]);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
    //    {
    //      int pauseTime = int.Parse(command.Substring(IrssUtils.Common.CmdPrefixPause.Length));
    //      Thread.Sleep(pauseTime);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitRunCommand(command.Substring(IrssUtils.Common.CmdPrefixRun.Length));
    //      IrssUtils.Common.ProcessRunCommand(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitSerialCommand(command.Substring(IrssUtils.Common.CmdPrefixSerial.Length));
    //      IrssUtils.Common.ProcessSerialCommand(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitWindowMessageCommand(command.Substring(IrssUtils.Common.CmdPrefixWindowMsg.Length));
    //      IrssUtils.Common.ProcessWindowMessageCommand(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitTcpMessageCommand(command.Substring(IrssUtils.Common.CmdPrefixTcpMsg.Length));
    //      IrssUtils.Common.ProcessTcpMessageCommand(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitHttpMessageCommand(command.Substring(IrssUtils.Common.CmdPrefixHttpMsg.Length));
    //      IrssUtils.Common.ProcessHttpCommand(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string keyCommand = command.Substring(IrssUtils.Common.CmdPrefixKeys.Length);
    //      if (InConfiguration)
    //        MessageBox.Show(keyCommand, IrssUtils.Common.UITextKeys, MessageBoxButtons.OK, MessageBoxIcon.Information);
    //      else
    //        IrssUtils.Common.ProcessKeyCommand(keyCommand);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string mouseCommand = command.Substring(IrssUtils.Common.CmdPrefixMouse.Length);
    //      IrssUtils.Common.ProcessMouseCommand(mouseCommand);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string ejectCommand = command.Substring(IrssUtils.Common.CmdPrefixEject.Length);
    //      IrssUtils.Common.ProcessEjectCommand(ejectCommand);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitPopupCommand(command.Substring(IrssUtils.Common.CmdPrefixPopup.Length));
    //      if (InConfiguration)
    //        MessageBox.Show(commands[1], commands[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
    //      else
    //        MPCommon.ShowNotifyDialog(commands[0], commands[1], int.Parse(commands[2]));
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixGotoScreen, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string screenID = command.Substring(IrssUtils.Common.CmdPrefixGotoScreen.Length);

    //      if (InConfiguration)
    //        MessageBox.Show(screenID, IrssUtils.Common.UITextGotoScreen, MessageBoxButtons.OK, MessageBoxIcon.Information);
    //      else
    //        MPCommon.ProcessGoTo(screenID, _mpBasicHome);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixSendMPAction, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitSendMPActionCommand(command.Substring(IrssUtils.Common.CmdPrefixSendMPAction.Length));
    //      if (InConfiguration)
    //        MessageBox.Show(commands[0], IrssUtils.Common.UITextSendMPAction, MessageBoxButtons.OK, MessageBoxIcon.Information);
    //      else
    //        MPCommon.ProcessSendMediaPortalAction(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixSendMPMsg, StringComparison.OrdinalIgnoreCase))
    //    {
    //      string[] commands = IrssUtils.Common.SplitSendMPMsgCommand(command.Substring(IrssUtils.Common.CmdPrefixSendMPMsg.Length));
    //      if (InConfiguration)
    //        MessageBox.Show(commands[0], IrssUtils.Common.UITextSendMPMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
    //      else
    //        MPCommon.ProcessSendMediaPortalMessage(commands);
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixExit, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //        MessageBox.Show("Cannot exit MediaPortal in configuration", IrssUtils.Common.UITextExit, MessageBoxButtons.OK,
    //                        MessageBoxIcon.Information);
    //      else
    //        MPCommon.ExitMP();
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixHibernate, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //        MessageBox.Show("Cannot Hibernate in configuration", IrssUtils.Common.UITextHibernate, MessageBoxButtons.OK,
    //                        MessageBoxIcon.Information);
    //      else
    //        MPCommon.Hibernate();
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixReboot, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //        MessageBox.Show("Cannot Reboot in configuration", IrssUtils.Common.UITextReboot, MessageBoxButtons.OK,
    //                        MessageBoxIcon.Information);
    //      else
    //        MPCommon.Reboot();
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixShutdown, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //        MessageBox.Show("Cannot Shutdown in configuration", IrssUtils.Common.UITextShutdown, MessageBoxButtons.OK,
    //                        MessageBoxIcon.Information);
    //      else
    //        MPCommon.ShutDown();
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixStandby, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //        MessageBox.Show("Cannot enter Standby in configuration", IrssUtils.Common.UITextStandby, MessageBoxButtons.OK,
    //                        MessageBoxIcon.Information);
    //      else
    //        MPCommon.Standby();
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixVirtualKB, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //      {
    //        MessageBox.Show("Cannot show Virtual Keyboard in configuration", IrssUtils.Common.UITextVirtualKB,
    //                        MessageBoxButtons.OK, MessageBoxIcon.Information);
    //      }
    //      else
    //      {
    //        VirtualKeyboard vk = new VirtualKeyboard();
    //        if (vk.ShowDialog() == DialogResult.OK)
    //          Keyboard.ProcessCommand(vk.TextOutput);
    //      }
    //    }
    //    else if (command.StartsWith(IrssUtils.Common.CmdPrefixSmsKB, StringComparison.OrdinalIgnoreCase))
    //    {
    //      if (InConfiguration)
    //      {
    //        MessageBox.Show("Cannot show SMS Keyboard in configuration", IrssUtils.Common.UITextSmsKB, MessageBoxButtons.OK,
    //                        MessageBoxIcon.Information);
    //      }
    //      else
    //      {
    //        SmsKeyboard sms = new SmsKeyboard();
    //        if (sms.ShowDialog() == DialogResult.OK)
    //          Keyboard.ProcessCommand(sms.TextOutput);
    //      }
    //    }
    //    else
    //    {
    //      throw new ArgumentException(String.Format("Cannot process unrecognized command \"{0}\"", command),
    //                                  "commandObj");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    if (!String.IsNullOrEmpty(Thread.CurrentThread.Name) &&
    //        Thread.CurrentThread.Name.Equals(ProcessCommandThreadName, StringComparison.OrdinalIgnoreCase))
    //      Log.Error(ex);
    //    else
    //      throw;
    //  }
    //}

    ///// <summary>
    ///// Called by ProcCommand to process the supplied Macro file.
    ///// </summary>
    ///// <param name="fileName">Macro file to process (absolute path).</param>
    //private static void ProcMacro(string fileName)
    //{
    //  XmlDocument doc = new XmlDocument();
    //  doc.Load(fileName);

    //  XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

    //  foreach (XmlNode item in commandSequence)
    //    ProcCommand(item.Attributes["command"].Value);
    //}

    ///// <summary>
    ///// Returns a list of Macros.
    ///// </summary>
    ///// <param name="commandPrefix">Add the command prefix to each list item.</param>
    ///// <returns>string[] of Macros.</returns>
    //internal static string[] GetMacroList(bool commandPrefix)
    //{
    //  string[] files = Directory.GetFiles(FolderMacros, '*' + IrssUtils.Common.FileExtensionMacro);
    //  string[] list = new string[files.Length];

    //  int i = 0;
    //  foreach (string file in files)
    //  {
    //    if (commandPrefix)
    //      list[i++] = IrssUtils.Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
    //    else
    //      list[i++] = Path.GetFileNameWithoutExtension(file);
    //  }

    //  return list;
    //}

    ///// <summary>
    ///// Returns a combined list of IR Commands and Macros.
    ///// </summary>
    ///// <param name="commandPrefix">Add the command prefix to each list item.</param>
    ///// <returns>string[] of IR Commands and Macros.</returns>
    //internal static string[] GetFileList(bool commandPrefix)
    //{
    //  string[] MacroFiles = Directory.GetFiles(FolderMacros, '*' + IrssUtils.Common.FileExtensionMacro);
    //  string[] IRFiles = Directory.GetFiles(IrssUtils.Common.FolderIRCommands, '*' + IrssUtils.Common.FileExtensionIR);
    //  string[] list = new string[MacroFiles.Length + IRFiles.Length];

    //  int i = 0;
    //  foreach (string file in MacroFiles)
    //  {
    //    if (commandPrefix)
    //      list[i++] = IrssUtils.Common.CmdPrefixMacro + Path.GetFileNameWithoutExtension(file);
    //    else
    //      list[i++] = Path.GetFileNameWithoutExtension(file);
    //  }

    //  foreach (string file in IRFiles)
    //  {
    //    if (commandPrefix)
    //      list[i++] = IrssUtils.Common.CmdPrefixBlast + Path.GetFileNameWithoutExtension(file);
    //    else
    //      list[i++] = Path.GetFileNameWithoutExtension(file);
    //  }

    //  return list;
    //}

    /// <summary>
    /// Loads the settings.
    /// </summary>
    private static void LoadSettings()
    {
      try
      {
        using (Settings xmlreader = new Settings(MPCommon.MPConfigFile))
        {
          ServerHost = xmlreader.GetValueAsString("MPBlastZonePlugin", "ServerHost", "localhost");

          // MediaPortal settings ...
          _mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Saves the settings.
    /// </summary>
    private static void SaveSettings()
    {
      try
      {
        using (Settings xmlwriter = new Settings(MPCommon.MPConfigFile))
        {
          xmlwriter.SetValue("MPBlastZonePlugin", "ServerHost", ServerHost);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    #endregion Implementation
  }
}