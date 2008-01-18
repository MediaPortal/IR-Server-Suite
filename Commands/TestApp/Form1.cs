using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace Commands
{

  public partial class Form1 : Form
  {

    public Form1()
    {
      InitializeComponent();

      VariableList variables = new VariableList();


      EditMacro edit = new EditMacro("C:\\", variables, new IrssUtils.BlastIrDelegate(BlastIr), new string[] { "Default" }, new string[] { "General Commands", "MediaPortal Commands" });
      edit.ShowDialog(this);

    }


    void BlastIr(string fileName, string port)
    {

    }

  }

}
