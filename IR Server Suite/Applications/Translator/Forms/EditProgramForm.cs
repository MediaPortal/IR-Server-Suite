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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IrssCommands;
using IrssUtils;

namespace Translator.Forms
{
  internal partial class EditProgramForm : Form
  {
    #region Properties

    public string DisplayName
    {
      get { return textBoxDisplayName.Text; }
      set { textBoxDisplayName.Text = value; }
    }

    public string Filename
    {
      get { return textBoxApp.Text; }
      set { textBoxApp.Text = value; }
    }

    public string StartupFolder
    {
      get { return textBoxAppStartFolder.Text; }
      set { textBoxAppStartFolder.Text = value; }
    }

    public string Parameters
    {
      get { return textBoxApplicationParameters.Text; }
      set { textBoxApplicationParameters.Text = value; }
    }

    public ProcessWindowStyle StartState
    {
      get
      {
        return
          (ProcessWindowStyle) Enum.Parse(typeof (ProcessWindowStyle), comboBoxWindowStyle.SelectedItem as string, true);
      }
      set { comboBoxWindowStyle.SelectedItem = Enum.GetName(typeof (ProcessWindowStyle), value); }
    }

    public bool UseShellExecute
    {
      get { return checkBoxShellExecute.Checked; }
      set { checkBoxShellExecute.Checked = value; }
    }

    public bool ForceWindowFocus
    {
      get { return checkBoxForceFocus.Checked; }
      set { checkBoxForceFocus.Checked = value; }
    }

    public bool IgnoreSystemWide
    {
      get { return checkBoxIgnoreSystemWide.Checked; }
      set { checkBoxIgnoreSystemWide.Checked = value; }
    }

    #endregion Properties

    #region Constructor

    public EditProgramForm(ProgramSettings progSettings)
    {
      InitializeComponent();

      comboBoxWindowStyle.Items.AddRange(Enum.GetNames(typeof (ProcessWindowStyle)));

      if (progSettings != null)
      {
        DisplayName = progSettings.Name;
        Filename = progSettings.FileName;
        StartupFolder = progSettings.Folder;
        Parameters = progSettings.Arguments;
        StartState = progSettings.WindowState;
        UseShellExecute = progSettings.UseShellExecute;
        ForceWindowFocus = progSettings.ForceWindowFocus;
        IgnoreSystemWide = progSettings.IgnoreSystemWide;
      }
    }

    #endregion Constructor

    #region Buttons

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

        if (String.IsNullOrEmpty(textBoxDisplayName.Text) ||
            textBoxDisplayName.Text.Equals(ProgramSettings.NewProgramName, StringComparison.Ordinal))
          textBoxDisplayName.Text = Path.GetFileNameWithoutExtension(find.FileName);
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

    private void buttonTest_Click(object sender, EventArgs e)
    {
      try
      {
        Command command = Processor.CreateCommand(Common.CLASS_RunCommand, GetRunCommandParameters());
        if (!ReferenceEquals(command,null))
          command.Execute(new VariableList());
      }
      catch (Exception ex)
      {
        IrssLog.Error("Test Application: {0}", ex.ToString());
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion Buttons

    internal string[] GetRunCommandParameters()
    {
      return new[]
        {
          Filename.Trim(),
          StartupFolder.Trim(),
          Parameters.Trim(),
          Enum.GetName(typeof (ProcessWindowStyle), WindowState),
          false.ToString(),
          UseShellExecute.ToString(),
          false.ToString(),
          ForceWindowFocus.ToString()
        };
    }
  }
}