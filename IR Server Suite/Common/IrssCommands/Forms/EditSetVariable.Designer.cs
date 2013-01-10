namespace IrssCommands
{
  partial class EditSetVariable
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.labelVariable = new System.Windows.Forms.Label();
      this.labelValue = new System.Windows.Forms.Label();
      this.textBoxVariable = new System.Windows.Forms.TextBox();
      this.textBoxValue = new System.Windows.Forms.TextBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.labelVarPrefix = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(112, 72);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(184, 72);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelVariable
      // 
      this.labelVariable.Location = new System.Drawing.Point(8, 8);
      this.labelVariable.Name = "labelVariable";
      this.labelVariable.Size = new System.Drawing.Size(64, 20);
      this.labelVariable.TabIndex = 0;
      this.labelVariable.Text = "Variable:";
      this.labelVariable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelValue
      // 
      this.labelValue.Location = new System.Drawing.Point(8, 40);
      this.labelValue.Name = "labelValue";
      this.labelValue.Size = new System.Drawing.Size(64, 20);
      this.labelValue.TabIndex = 2;
      this.labelValue.Text = "Value:";
      this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxVariable
      // 
      this.textBoxVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxVariable.Location = new System.Drawing.Point(104, 8);
      this.textBoxVariable.Name = "textBoxVariable";
      this.textBoxVariable.Size = new System.Drawing.Size(144, 20);
      this.textBoxVariable.TabIndex = 1;
      this.toolTips.SetToolTip(this.textBoxVariable, "Type your variable  name here (without the variable prefix)");
      // 
      // textBoxValue
      // 
      this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxValue.Location = new System.Drawing.Point(72, 40);
      this.textBoxValue.Name = "textBoxValue";
      this.textBoxValue.Size = new System.Drawing.Size(176, 20);
      this.textBoxValue.TabIndex = 3;
      // 
      // labelVarPrefix
      // 
      this.labelVarPrefix.Location = new System.Drawing.Point(72, 8);
      this.labelVarPrefix.Name = "labelVarPrefix";
      this.labelVarPrefix.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix.TabIndex = 6;
      this.labelVarPrefix.Text = "var_";
      this.labelVarPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // EditSetVariable
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(256, 105);
      this.Controls.Add(this.labelVarPrefix);
      this.Controls.Add(this.textBoxValue);
      this.Controls.Add(this.textBoxVariable);
      this.Controls.Add(this.labelValue);
      this.Controls.Add(this.labelVariable);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(264, 132);
      this.Name = "EditSetVariable";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Set Variable Command";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelVariable;
    private System.Windows.Forms.Label labelValue;
    private System.Windows.Forms.TextBox textBoxVariable;
    private System.Windows.Forms.TextBox textBoxValue;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelVarPrefix;
  }
}