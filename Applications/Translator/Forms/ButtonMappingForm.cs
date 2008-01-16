using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

using IrssComms;
using IrssUtils;
using IrssUtils.Forms;

namespace Translator
{

  partial class ButtonMappingForm : Form
  {

    #region Constants

    const string Parameters = 
@"\a = Alert (ascii 7)
\b = Backspace (ascii 8)
\f = Form Feed (ascii 12)
\n = Line Feed (ascii 10)
\r = Carriage Return (ascii 13)
\t = Tab (ascii 9)
\v = Vertical Tab (ascii 11)
\x = Hex Value (\x0Fh = ascii char 15, \x8h = ascii char 8)
\0 = Null (ascii 0)";

    #endregion Constants

    #region Variables

    string _keyCode;
    string _description;
    string _command;

    LearnIR _learnIR;

    #endregion Variables

    #region Properties

    internal string KeyCode
    {
      get { return _keyCode; }
    }
    internal string Description
    {
      get { return _description; }
    }
    internal string Command
    {
      get { return _command; }
    }

    #endregion Properties

    #region Constructors

    public ButtonMappingForm(string keyCode, string description, string command)
    {
      InitializeComponent();
      
      _keyCode      = keyCode;
      _description  = description;
      _command      = command;
    }

    #endregion Constructors

    void SetupIRList()
    {
      comboBoxIRCode.Items.Clear();

      string[] irList = Common.GetIRList(false);
      if (irList != null && irList.Length > 0)
      {
        comboBoxIRCode.Items.AddRange(irList);
        comboBoxIRCode.SelectedIndex = 0;
      }
    }
    void SetupMacroList()
    {
      comboBoxMacro.Items.Clear();

      string[] macroList = IrssMacro.GetMacroList(Program.FolderMacros, false);
      if (macroList != null && macroList.Length > 0)
      {
        comboBoxMacro.Items.AddRange(macroList);
        comboBoxMacro.SelectedIndex = 0;
      }
    }

    void InsertKeystroke(char key)
    {
      textBoxKeys.Paste(key.ToString());
    }
    void InsertKeystroke(string keystroke)
    {
      textBoxKeys.Paste(keystroke);
    }

    private void ButtonMappingForm_Load(object sender, EventArgs e)
    {
      textBoxKeyCode.Text     = _keyCode;
      textBoxButtonDesc.Text  = _description;
      textBoxCommand.Text     = _command;

      // Setup IR Blast tab
      SetupIRList();
      
      // Setup macro tab
      SetupMacroList();
      
      comboBoxPort.Items.Clear();
      comboBoxPort.Items.AddRange(Program.TransceiverInformation.Ports);
      if (comboBoxPort.Items.Count > 0)
        comboBoxPort.SelectedIndex = 0;

      // Setup Serial tab
      comboBoxComPort.Items.Clear();
      comboBoxComPort.Items.AddRange(SerialPort.GetPortNames());
      if (comboBoxComPort.Items.Count > 0)
        comboBoxComPort.SelectedIndex = 0;

      comboBoxParity.Items.Clear();
      comboBoxParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
      comboBoxParity.SelectedIndex = 0;

      comboBoxStopBits.Items.Clear();
      comboBoxStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
      comboBoxStopBits.SelectedIndex = 1;

      // Setup Run tab
      comboBoxWindowStyle.Items.Clear();
      comboBoxWindowStyle.Items.AddRange(Enum.GetNames(typeof(ProcessWindowStyle)));
      comboBoxWindowStyle.SelectedIndex = 0;

      // Setup Misc tab
      comboBoxMiscCommand.Items.Clear();
      comboBoxMiscCommand.Items.Add(Common.UITextTranslator);
      comboBoxMiscCommand.Items.Add(Common.UITextVirtualKB);
      comboBoxMiscCommand.Items.Add(Common.UITextSmsKB);
      comboBoxMiscCommand.Items.Add(Common.UITextTcpMsg);
      comboBoxMiscCommand.Items.Add(Common.UITextHttpMsg);
      comboBoxMiscCommand.Items.Add(Common.UITextEject);
      comboBoxMiscCommand.Items.Add(Common.UITextPopup);
      comboBoxMiscCommand.Items.Add(Common.UITextStandby);
      comboBoxMiscCommand.Items.Add(Common.UITextHibernate);
      comboBoxMiscCommand.Items.Add(Common.UITextReboot);
      comboBoxMiscCommand.Items.Add(Common.UITextShutdown);
      comboBoxMiscCommand.Items.Add(Common.UITextBeep);
      comboBoxMiscCommand.Items.Add(Common.UITextSound);

      if (!String.IsNullOrEmpty(_command))
      {
        string prefix = _command;
        string suffix = String.Empty;

        int find = _command.IndexOf(": ", StringComparison.OrdinalIgnoreCase);

        if (find != -1)
        {
          prefix = _command.Substring(0, find + 2);
          suffix = _command.Substring(find + 2);
        }

        switch (prefix)
        {
          case Common.CmdPrefixBlast:
            {
              string[] commands = Common.SplitBlastCommand(suffix);
              
              tabControl.SelectTab(tabPageBlastIR);
              comboBoxIRCode.SelectedItem = commands[0];
              comboBoxPort.SelectedItem = commands[1];
              break;
            }

          case Common.CmdPrefixMacro:
            {
              tabControl.SelectTab(tabPageMacro);
              comboBoxMacro.SelectedItem = suffix;
              break;
            }

          case Common.CmdPrefixRun:
            {
              string[] commands = Common.SplitRunCommand(suffix);

              tabControl.SelectTab(tabPageProgram);
              textBoxApp.Text = commands[0];
              textBoxAppStartFolder.Text = commands[1];
              textBoxApplicationParameters.Text = commands[2];
              comboBoxWindowStyle.SelectedItem = commands[3];
              checkBoxNoWindow.Checked = bool.Parse(commands[4]);
              checkBoxShellExecute.Checked = bool.Parse(commands[5]);
              break;
            }

          case Common.CmdPrefixSerial:
            {
              string[] commands = Common.SplitSerialCommand(suffix);

              tabControl.SelectTab(tabPageSerial);
              textBoxSerialCommand.Text = commands[0];
              comboBoxComPort.SelectedItem = commands[1];
              numericUpDownBaudRate.Value = decimal.Parse(commands[2]);
              comboBoxParity.SelectedItem = commands[3];
              numericUpDownDataBits.Value = decimal.Parse(commands[4]);
              comboBoxStopBits.SelectedItem = commands[5];
              checkBoxWaitForResponse.Checked = bool.Parse(commands[6]);
              
              break;
            }

          case Common.CmdPrefixWindowMsg:
            {
              string[] commands = Common.SplitWindowMessageCommand(suffix);

              tabControl.SelectTab(tabPageMessage);
              switch (commands[0].ToUpperInvariant())
              {
                case Common.TargetActive:       radioButtonActiveWindow.Checked = true;   break;
                case Common.TargetApplication:  radioButtonApplication.Checked = true;    break;
                case Common.TargetClass:        radioButtonClass.Checked = true;          break;
                case Common.TargetWindow:       radioButtonWindowTitle.Checked = true;    break;
              }

              textBoxMsgTarget.Text     = commands[1];
              numericUpDownMsg.Value    = decimal.Parse(commands[2]);
              numericUpDownWParam.Value = decimal.Parse(commands[3]);
              numericUpDownLParam.Value = decimal.Parse(commands[4]);
              break;
            }

          case Common.CmdPrefixKeys:
            {
              tabControl.SelectTab(tabPageKeystrokes);
              textBoxKeys.Text = suffix;
              break;
            }

          case Common.CmdPrefixMouse:
            {
              tabControl.SelectTab(tabPageMouse);
              switch (suffix)
              {
                case Common.MouseClickLeft:   checkBoxMouseClickLeft.Checked = true;    break;
                case Common.MouseClickMiddle: checkBoxMouseClickMiddle.Checked = true;  break;
                case Common.MouseClickRight:  checkBoxMouseClickRight.Checked = true;   break;
                case Common.MouseScrollDown:  checkBoxMouseScrollDown.Checked = true;   break;
                case Common.MouseScrollUp:    checkBoxMouseScrollUp.Checked = true;     break;

                default:
                  if (suffix.StartsWith(Common.MouseMoveDown, StringComparison.OrdinalIgnoreCase))        checkBoxMouseMoveDown.Checked = true;
                  else if (suffix.StartsWith(Common.MouseMoveLeft, StringComparison.OrdinalIgnoreCase))   checkBoxMouseMoveLeft.Checked = true;
                  else if (suffix.StartsWith(Common.MouseMoveRight, StringComparison.OrdinalIgnoreCase))  checkBoxMouseMoveRight.Checked = true;
                  else if (suffix.StartsWith(Common.MouseMoveUp, StringComparison.OrdinalIgnoreCase))     checkBoxMouseMoveUp.Checked = true;

                  numericUpDownMouseMove.Value = Decimal.Parse(suffix.Substring(suffix.IndexOf(' ')));
                  break;
              }
              break;
            }

          default:
            {
              tabControl.SelectTab(tabPageMisc);
              switch (_command)
              {
                case Common.CmdPrefixHibernate:
                  comboBoxMiscCommand.SelectedItem = Common.UITextHibernate;
                  break;

                case Common.CmdPrefixReboot:
                  comboBoxMiscCommand.SelectedItem = Common.UITextReboot;
                  break;

                case Common.CmdPrefixShutdown:
                  comboBoxMiscCommand.SelectedItem = Common.UITextShutdown;
                  break;

                case Common.CmdPrefixStandby:
                  comboBoxMiscCommand.SelectedItem = Common.UITextStandby;
                  break;

                case Common.CmdPrefixTranslator:
                  comboBoxMiscCommand.SelectedItem = Common.UITextTranslator;
                  break;

                case Common.CmdPrefixVirtualKB:
                  comboBoxMiscCommand.SelectedItem = Common.UITextVirtualKB;
                  break;

                case Common.CmdPrefixSmsKB:
                  comboBoxMiscCommand.SelectedItem = Common.UITextSmsKB;
                  break;

                default:
                  if (prefix.Equals(Common.CmdPrefixEject, StringComparison.OrdinalIgnoreCase))
                    comboBoxMiscCommand.SelectedItem = Common.UITextEject;
                  break;
              }
              break;
            }
        }
      }

    }

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(_keyCode) || String.IsNullOrEmpty(_command))
      {
        MessageBox.Show(this, "You must set a valid button mapping to press OK", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonParamQuestion_Click(object sender, EventArgs e)
    {
      MessageBox.Show(this, Parameters, "Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonSet_Click(object sender, EventArgs e)
    {
      switch (tabControl.SelectedTab.Name)
      {
        case "tabPageBlastIR":
          {
            textBoxCommand.Text = _command =
              String.Format("{0}{1}|{2}",
                Common.CmdPrefixBlast,
                comboBoxIRCode.SelectedItem as string,
                comboBoxPort.SelectedItem as string);
            break;
          }

        case "tabPageMacro":
          {
            textBoxCommand.Text = _command = Common.CmdPrefixMacro + comboBoxMacro.SelectedItem as string;
            break;
          }

        case "tabPageSerial":
          {
            textBoxCommand.Text = _command =
              String.Format("{0}{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                Common.CmdPrefixSerial,
                textBoxSerialCommand.Text,
                comboBoxComPort.SelectedItem as string,
                numericUpDownBaudRate.Value.ToString(),
                comboBoxParity.SelectedItem as string,
                numericUpDownDataBits.Value.ToString(),
                comboBoxStopBits.SelectedItem as string,
                checkBoxWaitForResponse.Checked.ToString());
            break;
          }

        case "tabPageProgram":
          {
            textBoxCommand.Text = _command =
              String.Format("{0}{1}|{2}|{3}|{4}|{5}|{6}|False|{7}",
                Common.CmdPrefixRun,
                textBoxApp.Text,
                textBoxAppStartFolder.Text,
                textBoxApplicationParameters.Text,
                comboBoxWindowStyle.SelectedItem as string,
                checkBoxNoWindow.Checked.ToString(),
                checkBoxShellExecute.Checked.ToString(),
                checkBoxForceFocus.Checked.ToString());
            break;
          }

        case "tabPageMessage":
          {
            string target = "ERROR";

            if (radioButtonActiveWindow.Checked)
            {
              target = Common.TargetActive;
              textBoxMsgTarget.Text = "*";
            }
            else if (radioButtonApplication.Checked)
            {
              target = Common.TargetApplication;
            }
            else if (radioButtonClass.Checked)
            {
              target = Common.TargetClass;
            }
            else if (radioButtonWindowTitle.Checked)
            {
              target = Common.TargetWindow;
            }

            textBoxCommand.Text = _command =
              String.Format("{0}{1}|{2}|{3}|{4}|{5}",
                Common.CmdPrefixWindowMsg,
                target,
                textBoxMsgTarget.Text,
                numericUpDownMsg.Value.ToString(),
                numericUpDownWParam.Value.ToString(),
                numericUpDownLParam.Value.ToString());
            break;
          }

        case "tabPageKeystrokes":
          {
            textBoxCommand.Text = _command = Common.CmdPrefixKeys + textBoxKeys.Text;
            break;
          }

        case "tabPageMouse":
          {
            StringBuilder newCommand = new StringBuilder();
            newCommand.Append(Common.CmdPrefixMouse);

            if (checkBoxMouseClickLeft.Checked)         newCommand.Append(Common.MouseClickLeft);
            else if (checkBoxMouseClickRight.Checked)   newCommand.Append(Common.MouseClickRight);
            else if (checkBoxMouseClickMiddle.Checked)  newCommand.Append(Common.MouseClickMiddle);
            else if (checkBoxMouseScrollUp.Checked)     newCommand.Append(Common.MouseScrollUp);
            else if (checkBoxMouseScrollDown.Checked)   newCommand.Append(Common.MouseScrollDown);
            else
            {
              if (checkBoxMouseMoveUp.Checked)          newCommand.Append(Common.MouseMoveUp);
              else if (checkBoxMouseMoveDown.Checked)   newCommand.Append(Common.MouseMoveDown);
              else if (checkBoxMouseMoveLeft.Checked)   newCommand.Append(Common.MouseMoveLeft);
              else if (checkBoxMouseMoveRight.Checked)  newCommand.Append(Common.MouseMoveRight);
              else break;

              newCommand.Append(numericUpDownMouseMove.Value.ToString());
            }
            
            textBoxCommand.Text = _command = newCommand.ToString();
            break;
          }

        case "tabPageMisc":
          {
            switch (comboBoxMiscCommand.SelectedItem as string)
            {
              case Common.UITextTranslator:
                textBoxCommand.Text = _command = Common.CmdPrefixTranslator;
                break;

              case Common.UITextVirtualKB:
                textBoxCommand.Text = _command = Common.CmdPrefixVirtualKB;
                break;

              case Common.UITextSmsKB:
                textBoxCommand.Text = _command = Common.CmdPrefixSmsKB;
                break;

                //comboBoxMiscCommand.Items.Add(Common.UITextTcpMsg);
                //comboBoxMiscCommand.Items.Add(Common.UITextHttpMsg);

              case Common.UITextEject:
                textBoxCommand.Text = _command = Common.CmdPrefixEject;
                break;

                //comboBoxMiscCommand.Items.Add(Common.UITextPopup);

              case Common.UITextStandby:
                textBoxCommand.Text = _command = Common.CmdPrefixStandby;
                break;

              case Common.UITextHibernate:
                textBoxCommand.Text = _command = Common.CmdPrefixHibernate;
                break;

              case Common.UITextReboot:
                textBoxCommand.Text = _command = Common.CmdPrefixReboot;
                break;

              case Common.UITextShutdown:
                textBoxCommand.Text = _command = Common.CmdPrefixShutdown;
                break;

                //comboBoxMiscCommand.Items.Add(Common.UITextBeep);
                //comboBoxMiscCommand.Items.Add(Common.UITextSound);
            }

            break;
          }
      }
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      if (_command.StartsWith(Common.CmdPrefixKeys, StringComparison.OrdinalIgnoreCase))
      {
        MessageBox.Show(this, "Keystroke commands cannot be tested here", "Cannot test Keystroke command", MessageBoxButtons.OK, MessageBoxIcon.Stop);
      }
      else
      {
        try
        {
          Program.ProcessCommand(_command, false);
        }
        catch (Exception ex)
        {
          MessageBox.Show(this, ex.ToString(), "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void buttonFindMsgTarget_Click(object sender, EventArgs e)
    {
      if (radioButtonApplication.Checked)
      {
        OpenFileDialog find = new OpenFileDialog();
        find.Filter = "All files|*.*";
        find.Multiselect = false;
        find.Title = "Application to send message to";

        if (find.ShowDialog(this) == DialogResult.OK)
          textBoxMsgTarget.Text = find.FileName;
      }
      else if (radioButtonClass.Checked)
      {
        // TODO: Locate Class
      }
      else if (radioButtonWindowTitle.Checked)
      {
        WindowList windowList = new WindowList();
        if (windowList.ShowDialog(this) == DialogResult.OK)
          textBoxMsgTarget.Text = windowList.SelectedWindowTitle;
      }
    }

    private void radioButtonActiveWindow_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = false;
      textBoxMsgTarget.Enabled = false;
    }
    private void radioButtonApplication_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = true;
      textBoxMsgTarget.Enabled = true;
    }
    private void radioButtonClass_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = false;
      textBoxMsgTarget.Enabled = true;
    }
    private void radioButtonWindowTitle_CheckedChanged(object sender, EventArgs e)
    {
      buttonFindMsgTarget.Enabled = true;
      textBoxMsgTarget.Enabled = true;
    }

    private void buttonLocate_Click(object sender, EventArgs e)
    {
      OpenFileDialog find = new OpenFileDialog();
      find.Filter = "All files|*.*";
      find.Multiselect = false;
      find.Title = "Application to launch";

      if (find.ShowDialog(this) == DialogResult.OK)
      {
        textBoxApp.Text = find.FileName;
        if (String.IsNullOrEmpty(textBoxAppStartFolder.Text))
          textBoxAppStartFolder.Text = Path.GetDirectoryName(find.FileName);
      }
    }

    private void buttonStartupFolder_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog find = new FolderBrowserDialog();
      find.Description = "Please specify the starting folder for the application";
      find.ShowNewFolderButton = true;
      if (find.ShowDialog(this) == DialogResult.OK)
        textBoxAppStartFolder.Text = find.SelectedPath;
    }

    private void buttonLearnIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        new LearnIrDelegate(Program.LearnIR),
        new BlastIrDelegate(Program.BlastIR),
        Program.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;

      SetupIRList();
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      SetupMacroList();
    }

    private void buttonKeyHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "Common\\keystrokes_info.html");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void textBoxButtonDesc_TextChanged(object sender, EventArgs e)
    {
      _description = textBoxButtonDesc.Text;
    }

    private void checkBoxMouse_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox origin = (CheckBox)sender;

      if (!origin.Checked)
        return;
      
      if (origin != checkBoxMouseClickLeft)   checkBoxMouseClickLeft.Checked    = false;
      if (origin != checkBoxMouseClickRight)  checkBoxMouseClickRight.Checked   = false;
      if (origin != checkBoxMouseClickMiddle) checkBoxMouseClickMiddle.Checked  = false;
      if (origin != checkBoxMouseMoveUp)      checkBoxMouseMoveUp.Checked       = false;
      if (origin != checkBoxMouseMoveDown)    checkBoxMouseMoveDown.Checked     = false;
      if (origin != checkBoxMouseMoveLeft)    checkBoxMouseMoveLeft.Checked     = false;
      if (origin != checkBoxMouseMoveRight)   checkBoxMouseMoveRight.Checked    = false;
      if (origin != checkBoxMouseScrollUp)    checkBoxMouseScrollUp.Checked     = false;
      if (origin != checkBoxMouseScrollDown)  checkBoxMouseScrollDown.Checked   = false;
    }

    private void KeystrokeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem origin = sender as ToolStripMenuItem;

      if (origin == null)
        return;

      switch (origin.Name)
      {
        case "upToolStripMenuItem":         InsertKeystroke("{UP}"); break;
        case "downToolStripMenuItem":       InsertKeystroke("{DOWN}"); break;
        case "leftToolStripMenuItem":       InsertKeystroke("{LEFT}"); break;
        case "rightToolStripMenuItem":      InsertKeystroke("{RIGHT}"); break;

        case "f1ToolStripMenuItem":         InsertKeystroke("{F1}"); break;
        case "f2ToolStripMenuItem":         InsertKeystroke("{F2}"); break;
        case "f3ToolStripMenuItem":         InsertKeystroke("{F3}"); break;
        case "f4ToolStripMenuItem":         InsertKeystroke("{F4}"); break;
        case "f5ToolStripMenuItem":         InsertKeystroke("{F5}"); break;
        case "f6ToolStripMenuItem":         InsertKeystroke("{F6}"); break;
        case "f7ToolStripMenuItem":         InsertKeystroke("{F7}"); break;
        case "f8ToolStripMenuItem":         InsertKeystroke("{F8}"); break;
        case "f9ToolStripMenuItem":         InsertKeystroke("{F9}"); break;
        case "f10ToolStripMenuItem":        InsertKeystroke("{F10}"); break;
        case "f11ToolStripMenuItem":        InsertKeystroke("{F11}"); break;
        case "f12ToolStripMenuItem":        InsertKeystroke("{F12}"); break;
        case "f13ToolStripMenuItem":        InsertKeystroke("{F13}"); break;
        case "f14ToolStripMenuItem":        InsertKeystroke("{F14}"); break;
        case "f15ToolStripMenuItem":        InsertKeystroke("{F15}"); break;
        case "f16ToolStripMenuItem":        InsertKeystroke("{F16}"); break;

        case "addToolStripMenuItem":        InsertKeystroke("{ADD}"); break;
        case "subtractToolStripMenuItem":   InsertKeystroke("{SUBTRACT}"); break;
        case "multiplyToolStripMenuItem":   InsertKeystroke("{MULTIPLY}"); break;
        case "divideToolStripMenuItem":     InsertKeystroke("{DIVIDE}"); break;

        case "altToolStripMenuItem":        InsertKeystroke(Keyboard.ModifierAlt); break;
        case "controlToolStripMenuItem":    InsertKeystroke(Keyboard.ModifierControl); break;
        case "shiftToolStripMenuItem":      InsertKeystroke(Keyboard.ModifierShift); break;
        case "windowsToolStripMenuItem":    InsertKeystroke(Keyboard.ModifierWinKey); break;

        case "backspaceToolStripMenuItem":  InsertKeystroke("{BACKSPACE}"); break;
        case "breakToolStripMenuItem":      InsertKeystroke("{BREAK}"); break;
        case "capsLockToolStripMenuItem":   InsertKeystroke("{CAPSLOCK}"); break;
        case "delToolStripMenuItem":        InsertKeystroke("{DEL}"); break;

        case "endToolStripMenuItem":        InsertKeystroke("{END}"); break;
        case "enterToolStripMenuItem":      InsertKeystroke("{ENTER}"); break;
        case "escapeToolStripMenuItem":     InsertKeystroke("{ESC}"); break;
        case "helpToolStripMenuItem":       InsertKeystroke("{HELP}"); break;
        case "homeToolStripMenuItem":       InsertKeystroke("{HOME}"); break;
        case "insToolStripMenuItem":        InsertKeystroke("{INS}"); break;
        case "numLockToolStripMenuItem":    InsertKeystroke("{NUMLOCK}"); break;
        case "pageDownToolStripMenuItem":   InsertKeystroke("{PGDN}"); break;
        case "pageUpToolStripMenuItem":     InsertKeystroke("{PGUP}"); break;
        case "scrollLockToolStripMenuItem": InsertKeystroke("{SCROLLLOCK}"); break;
        case "tabToolStripMenuItem":        InsertKeystroke("{TAB}"); break;
        case "windowsKeyToolStripMenuItem": InsertKeystroke("{WIN}"); break;
      }
    }

    private void cutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.Cut();
    }
    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.Copy();
    }
    private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.Paste();
    }
    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.SelectAll();
    }
    private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.SelectionLength = 0;
    }

    #endregion Controls

  }

}
