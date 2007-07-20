using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{

  public partial class MouseCommand : Form
  {

    #region Properties

    public string CommandString
    {
      get
      {
        StringBuilder command = new StringBuilder();
        command.Append(Common.CmdPrefixMouse);

        if (checkBoxMouseClickLeft.Checked)         command.Append(Common.MouseClickLeft);
        else if (checkBoxMouseClickRight.Checked)   command.Append(Common.MouseClickRight);
        else if (checkBoxMouseClickMiddle.Checked)  command.Append(Common.MouseClickMiddle);
        else if (checkBoxMouseScrollUp.Checked)     command.Append(Common.MouseScrollUp);
        else if (checkBoxMouseScrollDown.Checked)   command.Append(Common.MouseScrollDown);
        else
        {
          if (checkBoxMouseMoveUp.Checked)          command.Append(Common.MouseMoveUp);
          else if (checkBoxMouseMoveDown.Checked)   command.Append(Common.MouseMoveDown);
          else if (checkBoxMouseMoveLeft.Checked)   command.Append(Common.MouseMoveLeft);
          else if (checkBoxMouseMoveRight.Checked)  command.Append(Common.MouseMoveRight);
          else
            return null;

          command.Append(numericUpDownMouseMove.Value.ToString());
        }

        return command.ToString();
      }
    }

    #endregion Properties

    #region Constructors

    public MouseCommand() : this(null) { }
    public MouseCommand(string command)
    {
      InitializeComponent();

      if (String.IsNullOrEmpty(command))
        return;

      switch (command)
      {
        case Common.MouseClickLeft:   checkBoxMouseClickLeft.Checked = true;    break;
        case Common.MouseClickMiddle: checkBoxMouseClickMiddle.Checked = true;  break;
        case Common.MouseClickRight:  checkBoxMouseClickRight.Checked = true;   break;
        case Common.MouseScrollDown:  checkBoxMouseScrollDown.Checked = true;   break;
        case Common.MouseScrollUp:    checkBoxMouseScrollUp.Checked = true;     break;

        default:
          if (command.StartsWith(Common.MouseMoveDown))       checkBoxMouseMoveDown.Checked = true;
          else if (command.StartsWith(Common.MouseMoveLeft))  checkBoxMouseMoveLeft.Checked = true;
          else if (command.StartsWith(Common.MouseMoveRight)) checkBoxMouseMoveRight.Checked = true;
          else if (command.StartsWith(Common.MouseMoveUp))    checkBoxMouseMoveUp.Checked = true;

          numericUpDownMouseMove.Value = Decimal.Parse(command.Substring(command.IndexOf(" ")));
          break;
      }
  }

    #endregion

    #region Buttons

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

    #endregion Buttons

  }

}
