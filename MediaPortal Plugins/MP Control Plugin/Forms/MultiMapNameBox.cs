using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugins
{

  partial class MultiMapNameBox : Form
  {


    public string MapName
    {
      get { return textBoxName.Text; }
    }


    public MultiMapNameBox()
    {
      InitializeComponent();
    }


    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void textBoxName_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        e.SuppressKeyPress = true;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }


  }

}
