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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IrssUtils;
using IrssUtils.Forms;
using VariableList=Commands.VariableList;

namespace MacroScope
{
  public partial class MainForm : Form
  {
    #region Variables

    private string _macroFile;
    private readonly VariableList _variables;

    private bool _isDebugging;
    private int _debugLine;

    #endregion Variables

    #region Constructor

    public MainForm()
    {
      InitializeComponent();

      SetImages();

      PopulateCommandList();

      _variables = new VariableList();
    }

    private void SetImages()
    {
      this.newToolStripMenuItem.Image = IrssUtils.Properties.Resources.NewDocument;
      this.openToolStripMenuItem.Image = IrssUtils.Properties.Resources.OpenDocument;
      this.closeToolStripMenuItem.Image = IrssUtils.Properties.Resources.CloseDocument;
      this.saveToolStripMenuItem.Image = IrssUtils.Properties.Resources.Save;
      this.saveAsToolStripMenuItem.Image = IrssUtils.Properties.Resources.SaveAs;

      this.contentsToolStripMenuItem.Image = IrssUtils.Properties.Resources.Help;
      this.aboutToolStripMenuItem.Image = IrssUtils.Properties.Resources.Info;
    }

    #endregion Constructor

    /// <summary>
    /// Write the macro in the RichTextBox to a macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to write (macro name, not file path).</param>
    private void WriteToFile(string fileName)
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
    private void ReadFromFile(string fileName)
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


    private void ResetVariables()
    {
      _variables.VariableClear();
      listViewVariables.Clear();
    }

    private void LoadVariables(string fileName)
    {
      //      Dictionary<string, string>.Enumerator enumerator = _variables.GetEnumerator();
    }

    private void SaveVariables(string fileName)
    {
    }

    private void ProcessCurrentLine()
    {
    }


    private void DebugStep()
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

    private void DebugReset()
    {
      _isDebugging = true;
      _debugLine = 0;

      HighlightDebugLine();
    }

    private void DebugEnd()
    {
      _isDebugging = false;

      HighlightDebugLine();
    }


    private void HighlightDebugLine()
    {
      if (_isDebugging)
      {
      }
      else
      {
      }
    }


    private void PopulateCommandList()
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


    private void InsertCommand(string commandUiText)
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

    private void InsertText(string text)
    {
      richTextBoxMacro.Text.Insert(richTextBoxMacro.SelectionStart, text);
    }

    private void UpdateStatus(string text)
    {
      toolStripStatusLabel.Text = text;
    }

    #region Menus

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

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }
    
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
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


    private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    #endregion Menus

    private void treeViewCommandList_DoubleClick(object sender, EventArgs e)
    {
      if (treeViewCommandList.SelectedNode != null && treeViewCommandList.SelectedNode.Level == 1)
        InsertCommand(treeViewCommandList.SelectedNode.Text);
    }
  }
}