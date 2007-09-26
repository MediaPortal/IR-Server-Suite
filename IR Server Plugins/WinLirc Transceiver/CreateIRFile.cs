using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WinLircTransceiver
{

  partial class CreateIRFile : Form
  {

    public CreateIRFile()
    {
      InitializeComponent();
    }

    private void buttonDone_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCreate_Click(object sender, EventArgs e)
    {
      try
      {
        if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
          return;

        string fileName = saveFileDialog.FileName;

        using (XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
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
