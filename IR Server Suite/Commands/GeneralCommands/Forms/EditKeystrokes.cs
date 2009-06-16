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
using System.Windows.Forms;
using IrssUtils;

namespace Commands.General
{
  /// <summary>
  /// Edit Command form.
  /// </summary>
  internal partial class EditKeystrokes : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command string.
    /// </summary>
    /// <value>The command string.</value>
    public string CommandString
    {
      get { return textBoxKeys.Text; }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditKeystrokes"/> class.
    /// </summary>
    /// <param name="command">The command.</param>
    public EditKeystrokes(string command)
    {
      InitializeComponent();

      if (!String.IsNullOrEmpty(command))
        textBoxKeys.Text = command;
    }

    #endregion

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

    #endregion Buttons

    private void InsertKeystroke(char key)
    {
      textBoxKeys.Paste(key.ToString());
    }

    private void InsertKeystroke(string keystroke)
    {
      textBoxKeys.Paste(keystroke);
    }

    private void KeystrokeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem origin = sender as ToolStripMenuItem;

      if (origin == null)
        return;

      switch (origin.Name)
      {
        case "upToolStripMenuItem":
          InsertKeystroke("{UP}");
          break;
        case "downToolStripMenuItem":
          InsertKeystroke("{DOWN}");
          break;
        case "leftToolStripMenuItem":
          InsertKeystroke("{LEFT}");
          break;
        case "rightToolStripMenuItem":
          InsertKeystroke("{RIGHT}");
          break;

        case "f1ToolStripMenuItem":
          InsertKeystroke("{F1}");
          break;
        case "f2ToolStripMenuItem":
          InsertKeystroke("{F2}");
          break;
        case "f3ToolStripMenuItem":
          InsertKeystroke("{F3}");
          break;
        case "f4ToolStripMenuItem":
          InsertKeystroke("{F4}");
          break;
        case "f5ToolStripMenuItem":
          InsertKeystroke("{F5}");
          break;
        case "f6ToolStripMenuItem":
          InsertKeystroke("{F6}");
          break;
        case "f7ToolStripMenuItem":
          InsertKeystroke("{F7}");
          break;
        case "f8ToolStripMenuItem":
          InsertKeystroke("{F8}");
          break;
        case "f9ToolStripMenuItem":
          InsertKeystroke("{F9}");
          break;
        case "f10ToolStripMenuItem":
          InsertKeystroke("{F10}");
          break;
        case "f11ToolStripMenuItem":
          InsertKeystroke("{F11}");
          break;
        case "f12ToolStripMenuItem":
          InsertKeystroke("{F12}");
          break;
        case "f13ToolStripMenuItem":
          InsertKeystroke("{F13}");
          break;
        case "f14ToolStripMenuItem":
          InsertKeystroke("{F14}");
          break;
        case "f15ToolStripMenuItem":
          InsertKeystroke("{F15}");
          break;
        case "f16ToolStripMenuItem":
          InsertKeystroke("{F16}");
          break;

        case "addToolStripMenuItem":
          InsertKeystroke("{ADD}");
          break;
        case "subtractToolStripMenuItem":
          InsertKeystroke("{SUBTRACT}");
          break;
        case "multiplyToolStripMenuItem":
          InsertKeystroke("{MULTIPLY}");
          break;
        case "divideToolStripMenuItem":
          InsertKeystroke("{DIVIDE}");
          break;

        case "altToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierAlt);
          break;
        case "controlToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierControl);
          break;
        case "shiftToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierShift);
          break;
        case "windowsToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierWinKey);
          break;

        case "backspaceToolStripMenuItem":
          InsertKeystroke("{BACKSPACE}");
          break;
        case "breakToolStripMenuItem":
          InsertKeystroke("{BREAK}");
          break;
        case "capsLockToolStripMenuItem":
          InsertKeystroke("{CAPSLOCK}");
          break;
        case "delToolStripMenuItem":
          InsertKeystroke("{DEL}");
          break;

        case "endToolStripMenuItem":
          InsertKeystroke("{END}");
          break;
        case "enterToolStripMenuItem":
          InsertKeystroke("{ENTER}");
          break;
        case "escapeToolStripMenuItem":
          InsertKeystroke("{ESC}");
          break;
        case "helpToolStripMenuItem":
          InsertKeystroke("{HELP}");
          break;
        case "homeToolStripMenuItem":
          InsertKeystroke("{HOME}");
          break;
        case "insToolStripMenuItem":
          InsertKeystroke("{INS}");
          break;
        case "numLockToolStripMenuItem":
          InsertKeystroke("{NUMLOCK}");
          break;
        case "pageDownToolStripMenuItem":
          InsertKeystroke("{PGDN}");
          break;
        case "pageUpToolStripMenuItem":
          InsertKeystroke("{PGUP}");
          break;
        case "scrollLockToolStripMenuItem":
          InsertKeystroke("{SCROLLLOCK}");
          break;
        case "tabToolStripMenuItem":
          InsertKeystroke("{TAB}");
          break;
        case "windowsKeyToolStripMenuItem":
          InsertKeystroke("{WIN}");
          break;
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
  }
}