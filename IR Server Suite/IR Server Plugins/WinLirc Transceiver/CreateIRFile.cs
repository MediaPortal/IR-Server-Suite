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

namespace InputService.Plugin
{
  internal partial class CreateIRFile : Form
  {
    public CreateIRFile()
    {
      InitializeComponent();
    }

    private void buttonDone_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCreate_Click(object sender, EventArgs e)
    {
      try
      {
        if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
          return;

        string fileName = saveFileDialog.FileName;

        using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("IRCommand"); // <IRCommand>

          writer.WriteAttributeString("Password", textBoxPassword.Text);
          writer.WriteAttributeString("RemoteName", textBoxRemote.Text);
          writer.WriteAttributeString("ButtonName", textBoxButton.Text);
          writer.WriteAttributeString("Repeats", numericUpDownRepeats.Value.ToString());

          writer.WriteEndElement(); // </IRCommand>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.ToString(), "Error Creating IR File", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}