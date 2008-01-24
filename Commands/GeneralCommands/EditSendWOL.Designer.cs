namespace Commands.General
{

  partial class EditSendWOL
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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.labelMac = new System.Windows.Forms.Label();
      this.textBoxMac = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
      this.labelPassword = new System.Windows.Forms.Label();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(176, 104);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(104, 104);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // labelMac
      // 
      this.labelMac.Location = new System.Drawing.Point(8, 8);
      this.labelMac.Name = "labelMac";
      this.labelMac.Size = new System.Drawing.Size(88, 20);
      this.labelMac.TabIndex = 0;
      this.labelMac.Text = "MAC Address:";
      this.labelMac.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxMac
      // 
      this.textBoxMac.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxMac.Location = new System.Drawing.Point(96, 8);
      this.textBoxMac.Name = "textBoxMac";
      this.textBoxMac.Size = new System.Drawing.Size(144, 20);
      this.textBoxMac.TabIndex = 4;
      this.textBoxMac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.textBoxMac, "MAC Address of machine to wake");
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(8, 40);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(88, 20);
      this.label1.TabIndex = 5;
      this.label1.Text = "Port:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownPort
      // 
      this.numericUpDownPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownPort.Location = new System.Drawing.Point(96, 40);
      this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownPort.Name = "numericUpDownPort";
      this.numericUpDownPort.Size = new System.Drawing.Size(144, 20);
      this.numericUpDownPort.TabIndex = 6;
      this.numericUpDownPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownPort, "WakeOnLan port (default: 9)");
      this.numericUpDownPort.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
      // 
      // labelPassword
      // 
      this.labelPassword.Location = new System.Drawing.Point(8, 72);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(88, 20);
      this.labelPassword.TabIndex = 7;
      this.labelPassword.Text = "Password:";
      this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPassword.Location = new System.Drawing.Point(96, 72);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new System.Drawing.Size(144, 20);
      this.textBoxPassword.TabIndex = 8;
      this.textBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.textBoxPassword, "Optional password (either 4 or 6 charactors)");
      // 
      // EditSendWOL
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(248, 137);
      this.Controls.Add(this.textBoxPassword);
      this.Controls.Add(this.labelPassword);
      this.Controls.Add(this.numericUpDownPort);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBoxMac);
      this.Controls.Add(this.labelMac);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 164);
      this.Name = "EditSendWOL";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Wake On Lan Command";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Label labelMac;
    private System.Windows.Forms.TextBox textBoxMac;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown numericUpDownPort;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.ToolTip toolTips;
  }

}
