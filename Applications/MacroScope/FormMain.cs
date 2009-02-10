using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;
using IrssUtils.Forms;

namespace MacroScope
{


  public partial class FormMain : Form
  {

    #region Variables

    string _macroFile;
    Commands.VariableList _variables;

    bool _isDebugging;
    int _debugLine;

    #endregion Variables

    #region Constructor

    public FormMain()
    {
      InitializeComponent();

      PopulateCommandList();

      _variables = new Commands.VariableList();
    }

    #endregion Constructor


    /// <summary>
    /// Write the macro in the RichTextBox to a macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to write (macro name, not file path).</param>
    void WriteToFile(string fileName)
    {
      _macroFile = fileName;

      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("macro");

          foreach (string item in richTextBoxMacro.Lines)
          {
            if (String.IsNullOrEmpty(item))
              continue;

            writer.WriteStartElement("item");
            writer.WriteAttributeString("command", item);
            writer.WriteEndElement();
          }

          writer.WriteEndElement();
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    /// <summary>
    /// Read a macro into the listBox from the macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to read (macro name, not file path).</param>
    void ReadFromFile(string fileName)
    {
      _macroFile = fileName;

      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);

        XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("item");

        richTextBoxMacro.Clear();

        foreach (XmlNode item in commandSequence)
          richTextBoxMacro.AppendText(item.Attributes["command"].Value + '\n');
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }


    void ResetVariables()
    {
      _variables.VariableClear();
      listViewVariables.Clear();
    }

    void LoadVariables(string fileName)
    {
      //      Dictionary<string, string>.Enumerator enumerator = _variables.GetEnumerator();
    }

    void SaveVariables(string fileName)
    {

    }

    void ProcessCurrentLine()
    {

    }


    void DebugStep()
    {
      if (_isDebugging)
      {
        ProcessCurrentLine();


        _debugLine++;
        HighlightDebugLine();



      }
      else
      {
        _isDebugging = true;
        _debugLine = 0;

        HighlightDebugLine();
      }
    }

    void DebugReset()
    {
      _isDebugging = true;
      _debugLine = 0;

      HighlightDebugLine();
    }

    void DebugEnd()
    {
      _isDebugging = false;

      HighlightDebugLine();
    }


    void HighlightDebugLine()
    {
      if (_isDebugging)
      {

      }
      else
      {

      }

    }


    void PopulateCommandList()
    {
      TreeNode macroCommands = new TreeNode("Macro Commands");
      macroCommands.Nodes.Add(Common.UITextSetVar);
      macroCommands.Nodes.Add(Common.UITextLabel);
      macroCommands.Nodes.Add(Common.UITextGotoLabel);
      macroCommands.Nodes.Add(Common.UITextIf);
      treeViewCommandList.Nodes.Add(macroCommands);

      TreeNode generalCommands = new TreeNode("General Commands");
      generalCommands.Nodes.Add(Common.UITextBeep);
      generalCommands.Nodes.Add(Common.UITextCloseProgram);
      generalCommands.Nodes.Add(Common.UITextEject);
      generalCommands.Nodes.Add(Common.UITextHibernate);
      generalCommands.Nodes.Add(Common.UITextHttpMsg);
      generalCommands.Nodes.Add(Common.UITextKeys);
      generalCommands.Nodes.Add(Common.UITextLogOff);
      generalCommands.Nodes.Add(Common.UITextMouse);
      generalCommands.Nodes.Add(Common.UITextMouseMode);
      generalCommands.Nodes.Add(Common.UITextPause);
      generalCommands.Nodes.Add(Common.UITextPopup);
      generalCommands.Nodes.Add(Common.UITextReboot);
      generalCommands.Nodes.Add(Common.UITextRun);
      generalCommands.Nodes.Add(Common.UITextSerial);
      generalCommands.Nodes.Add(Common.UITextShutdown);
      generalCommands.Nodes.Add(Common.UITextSmsKB);
      generalCommands.Nodes.Add(Common.UITextSound);
      generalCommands.Nodes.Add(Common.UITextStandby);
      generalCommands.Nodes.Add(Common.UITextTcpMsg);
      generalCommands.Nodes.Add(Common.UITextTranslator);
      generalCommands.Nodes.Add(Common.UITextVirtualKB);
      generalCommands.Nodes.Add(Common.UITextWindowMsg);
      treeViewCommandList.Nodes.Add(generalCommands);

      TreeNode mediaPortalCommands = new TreeNode("MediaPortal Only Commands");
      mediaPortalCommands.Nodes.Add(Common.UITextExit);
      mediaPortalCommands.Nodes.Add(Common.UITextFocus);
      mediaPortalCommands.Nodes.Add(Common.UITextGotoScreen);
      mediaPortalCommands.Nodes.Add(Common.UITextInputLayer);
      mediaPortalCommands.Nodes.Add(Common.UITextMultiMap);
      mediaPortalCommands.Nodes.Add(Common.UITextSendMPAction);
      mediaPortalCommands.Nodes.Add(Common.UITextSendMPMsg);
      treeViewCommandList.Nodes.Add(mediaPortalCommands);      
    }


    void InsertCommand(string commandUiText)
    {
      string newCommand = String.Empty;

      switch (commandUiText)
      {
        case Common.UITextBeep:
          BeepCommand beepCommand = new BeepCommand();
          if (beepCommand.ShowDialog(this) == DialogResult.OK)
            InsertText(beepCommand.CommandString);

          break;




      }

      if (!String.IsNullOrEmpty(newCommand))
        InsertText(newCommand);
    }

    void InsertText(string text)
    {
      richTextBoxMacro.Text.Insert(richTextBoxMacro.SelectionStart, text);
    }

    void UpdateStatus(string text)
    {
      toolStripStatusLabel.Text = text;
    }


    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _macroFile = String.Empty;
      richTextBoxMacro.Clear();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        ReadFromFile(openFileDialog.FileName);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void treeViewCommandList_DoubleClick(object sender, EventArgs e)
    {
      if (treeViewCommandList.SelectedNode != null && treeViewCommandList.SelectedNode.Level == 1)
        InsertCommand(treeViewCommandList.SelectedNode.Text);
    }

    private void stepDebugToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DebugStep();
    }

    private void resetDebugToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DebugReset();
    }

    private void endDebugToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DebugEnd();
    }


  }

}
