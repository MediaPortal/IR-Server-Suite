using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using IrssUtils;
using IrssUtils.Exceptions;
using IrssUtils.Forms;

namespace IrssUtils.Forms
{
    public partial class CommandManager : UserControl
    {


        // --------------------------------------------------------------------------------------------------
        #region Attributes

        private bool _showGeneralCmds;
        private bool _showScriptCmds;
        private bool _showMacroCmds;
        private bool _showMediaPortalCmds;
        private bool _showServerCmds;
        public IRServerInfo IRServer;
        private BlastIrDelegate _BlastFunc;
        private LearnIrDelegate _LearnIrFunc;
        private ProcessCommandDelegate _ProcessCommand;
        internal static readonly string FolderMacros = Path.Combine(Common.FolderAppData, "Translator\\Macro");
        internal static readonly string FolderBlaster = Path.Combine(Common.FolderAppData, "IR Commands");
        private List<TreeNode> _editableGroups = new List<TreeNode>();

        #endregion Attributes


        // --------------------------------------------------------------------------------------------------
        #region Constructor

        public CommandManager(IRServerInfo server = null, BlastIrDelegate BlastFunc = null, LearnIrDelegate LearnIrFunc = null, ProcessCommandDelegate processCommand = null, bool script = false)
        {
            InitializeComponent();

            IRServer = server;
            _BlastFunc = BlastFunc;
            _LearnIrFunc = LearnIrFunc;
            _ProcessCommand = processCommand;

            _showGeneralCmds = true;
            _showScriptCmds = script;
            _showMacroCmds = true;
            _showMediaPortalCmds = false;
            _showServerCmds = BlastFunc != null && LearnIrFunc != null;
            _macroCommands = null;

            learnIRCommandToolStripMenuItem.Enabled = _LearnIrFunc != null;

            PopulateCommandList();
        }

        #endregion Constructor


        // --------------------------------------------------------------------------------------------------
        #region Interfaces

        private TreeNode _macroCommands;
        public void RefreshMacroList(string name="")
        {

            if (_showMacroCmds)
            {
                // Create empty group
                if (_macroCommands == null)
                {
                    _macroCommands = new TreeNode("Macro");
                    treeViewCommandList.Nodes.Add(_macroCommands);
                    _editableGroups.Add(_macroCommands);
                }
                else
                {
                    _macroCommands.Nodes.Clear();
                }

                string[] macroList = IrssMacro.GetMacroList(FolderMacros, true);
                if (macroList != null && macroList.Length > 0)
                {
                    string cmdName = Common.CmdPrefixMacro + name;
                    foreach (string macro in macroList)
                    {
                        if (macro != Common.CmdPrefixMacro)
                        {
                            TreeNode node = _macroCommands.Nodes.Add(macro);
                            if (cmdName == macro) treeViewCommandList.SelectedNode = node;
                        }
                    }
                }
            }
            treeViewCommandList_AfterSelect(null, null);
        }

        private TreeNode _irBlastCommands;
        public void RefreshBlastList(string name = "")
        {

            if (_showServerCmds)
            {
                // Create empty group
                if (_irBlastCommands == null)
                {
                    _irBlastCommands = new TreeNode("Blast");
                    treeViewCommandList.Nodes.Add(_irBlastCommands);
                    _editableGroups.Add(_irBlastCommands);
                }
                else
                {
                    _irBlastCommands.Nodes.Clear();
                }

                string[] irList = Common.GetIRList(true);
                if (irList != null && irList.Length > 0)
                {
                    string cmdName = Common.CmdPrefixBlast + name;
                    foreach (string ir in irList)
                    {
                        if (ir != Common.CmdPrefixBlast)
                        {
                            TreeNode node = _irBlastCommands.Nodes.Add(ir);
                            if (cmdName == ir) treeViewCommandList.SelectedNode = node;
                        }
                    }
                }
            }

            treeViewCommandList_AfterSelect(null, null);
        }


        public void PopulateCommandList()
        {
            treeViewCommandList.Nodes.Clear();

            if (_showGeneralCmds)
            {
                TreeNode generalCommands = new TreeNode("General Commands");

                TreeNode appCommands = new TreeNode("Control Applications");
                appCommands.Nodes.Add(Common.UITextRun);
                appCommands.Nodes.Add(Common.UITextPause);
                appCommands.Nodes.Add(Common.UITextWindowMsg);
                generalCommands.Nodes.Add(appCommands);

                TreeNode comCommands = new TreeNode("Communication & Peripherals");
                comCommands.Nodes.Add(Common.UITextWindowMsg);
                comCommands.Nodes.Add(Common.UITextSerial);
                comCommands.Nodes.Add(Common.UITextTcpMsg);
                comCommands.Nodes.Add(Common.UITextHttpMsg);
                comCommands.Nodes.Add(Common.UITextEject);
                comCommands.Nodes.Add(Common.UITextDisplayMode);
                generalCommands.Nodes.Add(comCommands);

                TreeNode inpCommands = new TreeNode("User Input");
                inpCommands.Nodes.Add(Common.UITextKeys);
                inpCommands.Nodes.Add(Common.UITextMouse);
                inpCommands.Nodes.Add(Common.UITextVirtualKB);
                inpCommands.Nodes.Add(Common.UITextSmsKB);
                inpCommands.Nodes.Add(Common.UITextTranslator);
                generalCommands.Nodes.Add(inpCommands);


                TreeNode feedCommands = new TreeNode("User feedback");
                feedCommands.Nodes.Add(Common.UITextPopup);
                feedCommands.Nodes.Add(Common.UITextBeep);
                feedCommands.Nodes.Add(Common.UITextSound);
                generalCommands.Nodes.Add(feedCommands);

                TreeNode powerCommands = new TreeNode("Power management");
                powerCommands.Nodes.Add(Common.UITextStandby);
                powerCommands.Nodes.Add(Common.UITextHibernate);
                powerCommands.Nodes.Add(Common.UITextReboot);
                powerCommands.Nodes.Add(Common.UITextShutdown);
                generalCommands.Nodes.Add(powerCommands);

                treeViewCommandList.Nodes.Add(generalCommands);
            }

            if(_showMacroCmds){
                TreeNode scriptCommands = new TreeNode("Scripting Commands");
                if (_showScriptCmds)
                {
                    scriptCommands.Nodes.Add(Common.UITextLabel);
                    scriptCommands.Nodes.Add(Common.UITextGotoLabel);
                    scriptCommands.Nodes.Add(Common.UITextIf);
                }
                scriptCommands.Nodes.Add(Common.UITextSetVar);
                scriptCommands.Nodes.Add(Common.UITextClearVars);
                scriptCommands.Nodes.Add(Common.UITextLoadVars);
                scriptCommands.Nodes.Add(Common.UITextSaveVars);
                treeViewCommandList.Nodes.Add(scriptCommands);

                RefreshMacroList();
            }

            if (_showServerCmds)
            {
                RefreshBlastList();
            }

            if(_showMediaPortalCmds){
                TreeNode mediaPortalCommands = new TreeNode("MediaPortal Commands");
                mediaPortalCommands.Nodes.Add(Common.UITextExit);
                mediaPortalCommands.Nodes.Add(Common.UITextFocus);
                mediaPortalCommands.Nodes.Add(Common.UITextGotoScreen);
                mediaPortalCommands.Nodes.Add(Common.UITextInputLayer);
                mediaPortalCommands.Nodes.Add(Common.UITextMultiMap);
                mediaPortalCommands.Nodes.Add(Common.UITextSendMPAction);
                mediaPortalCommands.Nodes.Add(Common.UITextSendMPMsg);
                treeViewCommandList.Nodes.Add(mediaPortalCommands);
            }
        }
        
        public string SelectedCommand
        {
            get
            {
                string cmdName = null;
                // Safeguard
                if (treeViewCommandList.SelectedNode == null) return null;

                // Exclude groups
                if (treeViewCommandList.SelectedNode.Nodes != null && treeViewCommandList.SelectedNode.Nodes.Count>0) return null;
                if (_editableGroups.Contains(treeViewCommandList.SelectedNode)) return null;

                // Detect valid command-node
                cmdName = treeViewCommandList.SelectedNode.Text;
                if (cmdName == null) return null;
                return cmdName;
            }
        }
        
        public event EventHandler<CommandGeneratedEventArgs> CommandGenerated;
        protected virtual void OnCommandGenerated(CommandGeneratedEventArgs e)
        {
            EventHandler<CommandGeneratedEventArgs> handler = CommandGenerated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Interfaces


        // --------------------------------------------------------------------------------------------------
        #region Command Forms calls

        /// <summary>
        /// Show the configuration window for the specified command without its arguments.
        /// </summary>
        /// <param name="cmdName">Command name to be configured. (Default: take the one selected in the command-list)</param>
        /// <returns>command with its arguments, or null if cancel or error</returns>
        public string CommandFetch(string cmdName=null, bool modify=false)
        {
            string newCommand = null;
            if (cmdName == null)   cmdName = SelectedCommand;
            if (cmdName == null)   return newCommand;

            try
            {

                if (cmdName.Equals(Common.UITextIf, StringComparison.OrdinalIgnoreCase))
                {
                    IfCommand ifCommand = new IfCommand();
                    if (ifCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixIf + ifCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextLabel, StringComparison.OrdinalIgnoreCase))
                {
                    LabelNameDialog labelDialog = new LabelNameDialog();
                    if (labelDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixLabel + labelDialog.LabelName;
                }
                else if (cmdName.Equals(Common.UITextGotoLabel, StringComparison.OrdinalIgnoreCase))
                {
                    LabelNameDialog labelDialog = new LabelNameDialog();
                    if (labelDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixGotoLabel + labelDialog.LabelName;
                }
                else if (cmdName.Equals(Common.UITextSetVar, StringComparison.OrdinalIgnoreCase))
                {
                    SetVariableCommand setVariableCommand = new SetVariableCommand();
                    if (setVariableCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSetVar + setVariableCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextClearVars, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixClearVars;
                }
                else if (cmdName.Equals(Common.UITextLoadVars, StringComparison.OrdinalIgnoreCase))
                {
                    VariablesFileDialog varsFileDialog = new VariablesFileDialog();
                    if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixLoadVars + varsFileDialog.FileName;
                }
                else if (cmdName.Equals(Common.UITextSaveVars, StringComparison.OrdinalIgnoreCase))
                {
                    VariablesFileDialog varsFileDialog = new VariablesFileDialog();
                    if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSaveVars + varsFileDialog.FileName;
                }
                else if (cmdName.Equals(Common.UITextRun, StringComparison.OrdinalIgnoreCase))
                {
                    ExternalProgram externalProgram = new ExternalProgram();
                    if (externalProgram.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixRun + externalProgram.CommandString;
                }
                else if (cmdName.Equals(Common.UITextPause, StringComparison.OrdinalIgnoreCase))
                {
                    PauseTime pauseTime = new PauseTime();
                    if (pauseTime.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixPause + pauseTime.Time;
                }
                else if (cmdName.Equals(Common.UITextSerial, StringComparison.OrdinalIgnoreCase))
                {
                    SerialCommand serialCommand = new SerialCommand();
                    if (serialCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSerial + serialCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextWindowMsg, StringComparison.OrdinalIgnoreCase))
                {
                    MessageCommand messageCommand = new MessageCommand();
                    if (messageCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextTcpMsg, StringComparison.OrdinalIgnoreCase))
                {
                    TcpMessageCommand tcpMessageCommand = new TcpMessageCommand();
                    if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextHttpMsg, StringComparison.OrdinalIgnoreCase))
                {
                    HttpMessageCommand httpMessageCommand = new HttpMessageCommand();
                    if (httpMessageCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixHttpMsg + httpMessageCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextKeys, StringComparison.OrdinalIgnoreCase))
                {
                    KeysCommand keysCommand = new KeysCommand();
                    if (keysCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixKeys + keysCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextMouse, StringComparison.OrdinalIgnoreCase))
                {
                    MouseCommand mouseCommand = new MouseCommand();
                    if (mouseCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixMouse + mouseCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextEject, StringComparison.OrdinalIgnoreCase))
                {
                    EjectCommand ejectCommand = new EjectCommand();
                    if (ejectCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixEject + ejectCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextPopup, StringComparison.OrdinalIgnoreCase))
                {
                    PopupMessage popupMessage = new PopupMessage();
                    if (popupMessage.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixPopup + popupMessage.CommandString;
                }
                else if (cmdName.Equals(Common.UITextVirtualKB, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixVirtualKB;
                }
                else if (cmdName.Equals(Common.UITextSmsKB, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixSmsKB;
                }
                else if (cmdName.Equals(Common.UITextBeep, StringComparison.OrdinalIgnoreCase))
                {
                    BeepCommand beepCommand = new BeepCommand();
                    if (beepCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixBeep + beepCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextSound, StringComparison.OrdinalIgnoreCase))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Wave Files|*.wav";
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSound + openFileDialog.FileName;
                }
                else if (cmdName.Equals(Common.UITextDisplayMode, StringComparison.OrdinalIgnoreCase))
                {
                    DisplayModeCommand displayModeCommand = new DisplayModeCommand();
                    if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
                }
                else if (cmdName.Equals(Common.UITextStandby, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixStandby;
                }
                else if (cmdName.Equals(Common.UITextHibernate, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixHibernate;
                }
                else if (cmdName.Equals(Common.UITextReboot, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixReboot;
                }
                else if (cmdName.Equals(Common.UITextShutdown, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixShutdown;
                }
                else if (cmdName.Equals(Common.CmdPrefixTranslator, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = Common.CmdPrefixTranslator;
                } 
                else if (cmdName.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
                {
                    string[] ports = null;
                    if (IRServer != null) ports = IRServer.Ports;
                    string name = cmdName.Substring(Common.CmdPrefixBlast.Length);

                    IREditor learnIR = new IREditor(_LearnIrFunc, _BlastFunc, ports, name);
                    if (learnIR.ShowDialog(this) == DialogResult.OK)
                    {
                        newCommand = Common.CmdPrefixBlast + learnIR.CommandString;
                        name = learnIR.BlastName;
                    }
                   
                    RefreshBlastList(name);
             

                }
                else if (cmdName.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
                {
                    if (modify)
                    {
                        string command = cmdName.Substring(Common.CmdPrefixBlast.Length);
                        string fileName = Path.Combine(FolderMacros, command + Common.FileExtensionMacro);
                        if (File.Exists(fileName))
                        {
                            MacroEditor macroEditor = new MacroEditor(command, IRServer, _BlastFunc, _LearnIrFunc, _ProcessCommand);
                            macroEditor.ShowDialog(this);
                            RefreshMacroList(macroEditor.MacroName);
                            if (macroEditor.DialogResult == DialogResult.OK)
                            {
                                cmdName = Common.CmdPrefixMacro + macroEditor.MacroName;
                            }
                            else
                            {
                                cmdName = null;
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation);
                        }
                    }

                    newCommand = cmdName;
                }
                else
                {
                    throw new CommandStructureException(String.Format("Unknown macro command ({0})", cmdName));
                }

            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                MessageBox.Show(this, ex.Message, "Failed to add macro command", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return newCommand;
        }

        /// <summary>
        /// Show the configuration window for the specified command with its arguments.
        /// </summary>
        /// <param name="cmdArgs">Command and its argument to be edited. </param>
        /// <returns>command with its arguments, or null if cancel or error</returns>
        public string CommandEdit(string cmdArgs)
        {
            string newCommand = null;
            if (String.IsNullOrEmpty(cmdArgs)) return null;

            try
            {
                
                if (cmdArgs.StartsWith(Common.CmdPrefixIf, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitIfCommand(cmdArgs.Substring(Common.CmdPrefixIf.Length));

                    IfCommand ifCommand = new IfCommand(commands);
                    if (ifCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixIf + ifCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixLabel, StringComparison.OrdinalIgnoreCase))
                {
                    LabelNameDialog labelDialog = new LabelNameDialog(cmdArgs.Substring(Common.CmdPrefixLabel.Length));
                    if (labelDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixLabel + labelDialog.LabelName;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixGotoLabel, StringComparison.OrdinalIgnoreCase))
                {
                    LabelNameDialog labelDialog = new LabelNameDialog(cmdArgs.Substring(Common.CmdPrefixGotoLabel.Length));
                    if (labelDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixGotoLabel + labelDialog.LabelName;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixSetVar, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitSetVarCommand(cmdArgs.Substring(Common.CmdPrefixSetVar.Length));

                    SetVariableCommand setVariableCommand = new SetVariableCommand(commands);
                    if (setVariableCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSetVar + setVariableCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixLoadVars, StringComparison.OrdinalIgnoreCase))
                {
                    VariablesFileDialog varsFileDialog =
                      new VariablesFileDialog(cmdArgs.Substring(Common.CmdPrefixLoadVars.Length));
                    if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixLoadVars + varsFileDialog.FileName;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixSaveVars, StringComparison.OrdinalIgnoreCase))
                {
                    VariablesFileDialog varsFileDialog =
                      new VariablesFileDialog(cmdArgs.Substring(Common.CmdPrefixSaveVars.Length));
                    if (varsFileDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSaveVars + varsFileDialog.FileName;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixRun, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitRunCommand(cmdArgs.Substring(Common.CmdPrefixRun.Length));

                    ExternalProgram executeProgram = new ExternalProgram(commands);
                    if (executeProgram.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixRun + executeProgram.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixPause, StringComparison.OrdinalIgnoreCase))
                {
                    PauseTime pauseTime = new PauseTime(int.Parse(cmdArgs.Substring(Common.CmdPrefixPause.Length)));
                    if (pauseTime.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixPause + pauseTime.Time;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixSerial, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitSerialCommand(cmdArgs.Substring(Common.CmdPrefixSerial.Length));

                    SerialCommand serialCommand = new SerialCommand(commands);
                    if (serialCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSerial + serialCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixWindowMsg, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitWindowMessageCommand(cmdArgs.Substring(Common.CmdPrefixWindowMsg.Length));

                    MessageCommand messageCommand = new MessageCommand(commands);
                    if (messageCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixWindowMsg + messageCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixTcpMsg, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitTcpMessageCommand(cmdArgs.Substring(Common.CmdPrefixTcpMsg.Length));

                    TcpMessageCommand tcpMessageCommand = new TcpMessageCommand(commands);
                    if (tcpMessageCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixTcpMsg + tcpMessageCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixHttpMsg, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitHttpMessageCommand(cmdArgs.Substring(Common.CmdPrefixHttpMsg.Length));

                    HttpMessageCommand httpMessageCommand = new HttpMessageCommand(commands);
                    if (httpMessageCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixHttpMsg + httpMessageCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
                {
                    KeysCommand keysCommand = new KeysCommand(cmdArgs.Substring(Common.CmdPrefixKeys.Length));
                    if (keysCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixKeys + keysCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixMouse, StringComparison.OrdinalIgnoreCase))
                {
                    MouseCommand mouseCommand = new MouseCommand(cmdArgs.Substring(Common.CmdPrefixMouse.Length));
                    if (mouseCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixMouse + mouseCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
                {
                    EjectCommand ejectCommand = new EjectCommand(cmdArgs.Substring(Common.CmdPrefixEject.Length));
                    if (ejectCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixEject + ejectCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixPopup, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitPopupCommand(cmdArgs.Substring(Common.CmdPrefixPopup.Length));

                    PopupMessage popupMessage = new PopupMessage(commands);
                    if (popupMessage.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixPopup + popupMessage.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixBeep, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitBeepCommand(cmdArgs.Substring(Common.CmdPrefixBeep.Length));

                    BeepCommand beepCommand = new BeepCommand(commands);
                    if (beepCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixBeep + beepCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixSound, StringComparison.OrdinalIgnoreCase))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Wave Files|*.wav";
                    openFileDialog.Multiselect = false;
                    openFileDialog.FileName = cmdArgs.Substring(Common.CmdPrefixSound.Length);

                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixSound + openFileDialog.FileName;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixDisplayMode, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitDisplayModeCommand(cmdArgs.Substring(Common.CmdPrefixDisplayMode.Length));

                    DisplayModeCommand displayModeCommand = new DisplayModeCommand(commands);
                    if (displayModeCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixDisplayMode + displayModeCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixBlast, StringComparison.OrdinalIgnoreCase))
                {
                    string[] commands = Common.SplitBlastCommand(cmdArgs.Substring(Common.CmdPrefixBlast.Length));
                    string[] ports = null;
                    if (IRServer != null) ports = IRServer.Ports;
                    BlastCommand blastCommand = new BlastCommand(_BlastFunc, Common.FolderIRCommands, ports, commands);

                    if (blastCommand.ShowDialog(this) == DialogResult.OK)
                        newCommand = Common.CmdPrefixBlast + blastCommand.CommandString;
                }
                else if (cmdArgs.StartsWith(Common.CmdPrefixMacro, StringComparison.OrdinalIgnoreCase))
                {
                    newCommand = CommandFetch(cmdArgs, true);
                }
                else
                {
                    throw new CommandStructureException(String.Format("Unknown macro command ({0})", cmdArgs));
                }
            }
            catch (Exception ex)
            {
                IrssLog.Error(ex);
                MessageBox.Show(this, ex.Message, "Failed to edit macro item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return newCommand;
        }

        private void CommandManager_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            IrssHelp.Open(this);
            hlpevent.Handled = true;
        }

        #endregion Command Forms calls


        // --------------------------------------------------------------------------------------------------
        #region Command list callbacks

        private void treeViewCommandList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool cmdSelected = SelectedCommand != null;
            bool cmdEditable = cmdSelected && _editableGroups.Contains(treeViewCommandList.SelectedNode.Parent);
            buttonCommandEdit.Enabled = cmdEditable;
            buttonCommandCopy.Enabled = cmdEditable;
            buttonCommandRemove.Enabled = cmdEditable;

            toolStripMenuAdd.Enabled = _editableGroups.Contains(treeViewCommandList.SelectedNode);
            toolStripMenuEdit.Enabled = cmdEditable;
            toolStripMenuCopy.Enabled = cmdEditable;
            toolStripMenuRemove.Enabled = cmdEditable;
        }
        
        private void toolStripMenuAdd_Click(object sender, EventArgs e)
        {
            if (!_editableGroups.Contains(treeViewCommandList.SelectedNode)) return;
            if (treeViewCommandList.SelectedNode.Text == "Macro")
                macroToolStripMenuItem_Click(null, null);
            else
                learnIRCommandToolStripMenuItem_Click(null, null);
        }

        private void learnIRCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_LearnIrFunc == null) return;

            IREditor learnIR = new IREditor(_LearnIrFunc, _BlastFunc, IRServer.Ports);
            learnIR.ShowDialog(this);
            RefreshBlastList(learnIR.BlastName);
        }

        private void macroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MacroEditor macroEditor = new MacroEditor("", IRServer, _BlastFunc, _LearnIrFunc, _ProcessCommand);
            macroEditor.ShowDialog(this);
            RefreshMacroList(macroEditor.MacroName);
            if (macroEditor.DialogResult == DialogResult.OK)
            {
                CommandGeneratedEventArgs args = new CommandGeneratedEventArgs();
                args.test = false;
                args.command = Common.CmdPrefixMacro + macroEditor.MacroName;
                OnCommandGenerated(args);
            }
        }


        private void buttonCommandEdit_Click(object sender, EventArgs e)
        {
            string newCommand = CommandFetch(null, true);

            if (newCommand != null)
            {
                CommandGeneratedEventArgs args = new CommandGeneratedEventArgs();
                args.test = false;
                args.command = newCommand;
                OnCommandGenerated(args);
            }
        }
        private void toolStripMenuEdit_Click(object sender, EventArgs e)
        {
            buttonCommandEdit_Click(null, null);
        }

        private void buttonCommandCopy_Click(object sender, EventArgs e)
        {
            string cmdSelected = SelectedCommand;
            if (cmdSelected!=null && _editableGroups.Contains(treeViewCommandList.SelectedNode.Parent))
            {
                bool macro = cmdSelected.IndexOf(Common.CmdPrefixMacro) >= 0;

                int idx = cmdSelected.IndexOf(':');
                if (idx < 1 || idx + 3 > cmdSelected.Length) return;

                string srcname = cmdSelected.Substring(idx+2);
                string srcfile;
                if (macro) srcfile = Path.Combine(FolderMacros, srcname + Common.FileExtensionMacro);
                else srcfile = Path.Combine(FolderBlaster, srcname + Common.FileExtensionIR);
                idx = 2;
                if(srcname[srcname.Length-1] == ')' ) {
                    int start = srcname.LastIndexOf(" (");
                    if(start>=0) {
                        string number = srcname.Substring(start+2, srcname.Length-(start+3));
                        try {
                            idx = Convert.ToInt32(number) + 1;
                            srcname = srcname.Substring(0, start);
                        }
                        catch
                        {
                            idx = 2;
                        }
                    }
                }

                string destname;
                string destfile;
                do
                {
                    destname = String.Format("{0} ({1})", srcname, idx++);
                    if (macro) destfile = Path.Combine(FolderMacros, destname + Common.FileExtensionMacro);
                    else destfile = Path.Combine(FolderBlaster, destname + Common.FileExtensionIR);
                } while (File.Exists(destfile));

                try
                {
                    File.Copy(srcfile, destfile);
                    if (macro) RefreshMacroList(destname);
                    else RefreshBlastList(destname);
                }
                catch (Exception ex)
                {
                    IrssLog.Error(ex);
                    MessageBox.Show(this, ex.Message, "Failed creating file: " + destfile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        private void toolStripMenuCopy_Click(object sender, EventArgs e)
        {
            buttonCommandCopy_Click(null, null);
        }

        private void buttonCommandRemove_Click(object sender, EventArgs e)
        {
            string cmdSelected = SelectedCommand;
            if (cmdSelected != null && _editableGroups.Contains(treeViewCommandList.SelectedNode.Parent))
            {
                bool macro = cmdSelected.IndexOf(Common.CmdPrefixMacro) >= 0;
                int idx = cmdSelected.IndexOf(':');
                if (idx < 1 || idx + 3 > cmdSelected.Length) return;

                string srcname = cmdSelected.Substring(idx + 2);
                string srcfile;
                if (macro) srcfile = Path.Combine(FolderMacros, srcname + Common.FileExtensionMacro);
                else srcfile = Path.Combine(FolderBlaster, srcname + Common.FileExtensionIR);
                try
                {
                    File.Delete(srcfile);
                }
                catch (Exception ex)
                {
                    IrssLog.Error(ex);
                    MessageBox.Show(this, ex.Message, "Failed deleting file: " + srcfile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (macro) RefreshMacroList();
                else RefreshBlastList();
            }
        }
        private void toolStripMenuRemove_Click(object sender, EventArgs e)
        {
            buttonCommandRemove_Click(null, null);
        }

        private void treeViewCommandList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) treeViewCommandList.SelectedNode = e.Node;
        }

        #endregion Command list callbacks



    }
}
