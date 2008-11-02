using System;
using System.Windows.Forms;

namespace MediaPortal.Plugins
{
  internal partial class MultiMapNameBox : Form
  {
    public MultiMapNameBox()
    {
      InitializeComponent();
    }

    public MultiMapNameBox(string mapName)
    {
      textBoxName.Text = mapName;
    }

    public string MapName
    {
      get { return textBoxName.Text; }
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

    private void textBoxName_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        e.SuppressKeyPress = true;
        DialogResult = DialogResult.OK;
        Close();
      }
    }
  }
}