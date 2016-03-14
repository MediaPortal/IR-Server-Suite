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
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  /// <summary>
  /// Blast Command form.
  /// </summary>
  public partial class BlastCommand : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get
      {
        return String.Format("{0}|{1}",
                             labelIRCommandFile.Text,
                             comboBoxPort.SelectedItem as string);
      }
    }

    /// <summary>
    /// Gets or sets the blaster port.
    /// </summary>
    /// <value>The blaster port.</value>
    public string BlasterPort
    {
      get { return comboBoxPort.SelectedItem as string; }
      set { comboBoxPort.SelectedItem = value; }
    }

    /// <summary>
    /// Gets a value indicating whether to use this commands details for all in the batch.
    /// </summary>
    /// <value><c>true</c> if use for all; otherwise, <c>false</c>.</value>
    public bool UseForAll
    {
      get { return checkBoxUseForAll.Checked; }
    }

    #endregion Properties

    #region Variables

    private readonly string _baseFolder;

    private readonly BlastIrDelegate _blastIrDelegate;

    #endregion Variables

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BlastCommand"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="baseFolder">The IR Command base folder.</param>
    /// <param name="ports">The available ports.</param>
    private BlastCommand(BlastIrDelegate blastIrDelegate, string baseFolder, string[] ports)
    {
      if (blastIrDelegate == null)
        throw new ArgumentNullException("blastIrDelegate");

      if (String.IsNullOrEmpty(baseFolder))
        throw new ArgumentNullException("baseFolder");

      if (ports == null)
        throw new ArgumentNullException("ports");

      InitializeComponent();

      _blastIrDelegate = blastIrDelegate;
      buttonTest.Enabled = _blastIrDelegate != null;

      _baseFolder = baseFolder;

      comboBoxPort.Items.AddRange(ports);
      comboBoxPort.SelectedIndex = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlastCommand"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="baseFolder">The IR Command base folder.</param>
    /// <param name="ports">The available ports.</param>
    /// <param name="name">The name of the IR Command.</param>
    public BlastCommand(BlastIrDelegate blastIrDelegate, string baseFolder, string[] ports, string name)
      : this(blastIrDelegate, baseFolder, ports)
    {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentNullException("name");

      labelIRCommandFile.Text = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlastCommand"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="baseFolder">The IR Command base folder.</param>
    /// <param name="ports">The available ports.</param>
    /// <param name="commands">The command elements.</param>
    public BlastCommand(BlastIrDelegate blastIrDelegate, string baseFolder, string[] ports, string[] commands)
      : this(blastIrDelegate, baseFolder, ports)
    {
      if (commands == null)
        throw new ArgumentNullException("commands");

      labelIRCommandFile.Text = commands[0];

      if (comboBoxPort.Items.Contains(commands[1]))
        comboBoxPort.SelectedItem = commands[1];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlastCommand"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="baseFolder">The IR Command base folder.</param>
    /// <param name="ports">The available ports.</param>
    /// <param name="name">The name of the IR Command.</param>
    /// <param name="commandCount">The command count for this batch of commands.</param>
    public BlastCommand(BlastIrDelegate blastIrDelegate, string baseFolder, string[] ports, string name,
                        int commandCount)
      : this(blastIrDelegate, baseFolder, ports, name)
    {
      if (commandCount > 1)
      {
        checkBoxUseForAll.Text = String.Format("Use this port for all ({0})", commandCount);
        checkBoxUseForAll.Visible = true;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlastCommand"/> class.
    /// </summary>
    /// <param name="blastIrDelegate">The blast IR delegate.</param>
    /// <param name="baseFolder">The IR Command base folder.</param>
    /// <param name="ports">The available ports.</param>
    /// <param name="commands">The command elements.</param>
    /// <param name="commandCount">The command count for this batch of commands.</param>
    public BlastCommand(BlastIrDelegate blastIrDelegate, string baseFolder, string[] ports, string[] commands,
                        int commandCount)
      : this(blastIrDelegate, baseFolder, ports, commands)
    {
      if (commandCount > 1)
      {
        checkBoxUseForAll.Text = String.Format("Use this port for all ({0})", commandCount);
        checkBoxUseForAll.Visible = true;
      }
    }

    #endregion Constructors

    #region Buttons

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

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string name = labelIRCommandFile.Text.Trim();

      if (name.Length == 0)
        return;

      try
      {
        string fileName = Path.Combine(_baseFolder, name + Common.FileExtensionIR);
        string port = comboBoxPort.SelectedItem as string;

        _blastIrDelegate(fileName, port);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Buttons
  }
}