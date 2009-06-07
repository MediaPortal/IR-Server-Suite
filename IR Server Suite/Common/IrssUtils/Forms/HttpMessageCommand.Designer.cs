namespace IrssUtils.Forms
{
  partial class HttpMessageCommand
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
      this.groupBoxMessageDetails = new System.Windows.Forms.GroupBox();
      this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
      this.labelTimeout = new System.Windows.Forms.Label();
      this.textBoxUsername = new System.Windows.Forms.TextBox();
      this.textBoxAddress = new System.Windows.Forms.TextBox();
      this.labelAddress = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.checkBoxUsernameAndPassword = new System.Windows.Forms.CheckBox();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelUsername = new System.Windows.Forms.Label();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxMessageDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBoxMessageDetails
      // 
      this.groupBoxMessageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMessageDetails.Controls.Add(this.labelUsername);
      this.groupBoxMessageDetails.Controls.Add(this.labelPassword);
      this.groupBoxMessageDetails.Controls.Add(this.checkBoxUsernameAndPassword);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxPassword);
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownTimeout);
      this.groupBoxMessageDetails.Controls.Add(this.labelTimeout);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxUsername);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxAddress);
      this.groupBoxMessageDetails.Controls.Add(this.labelAddress);
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(8, 8);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(288, 176);
      this.groupBoxMessageDetails.TabIndex = 0;
      this.groupBoxMessageDetails.TabStop = false;
      this.groupBoxMessageDetails.Text = "HTTP message details";
      // 
      // numericUpDownTimeout
      // 
      this.numericUpDownTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownTimeout.Location = new System.Drawing.Point(104, 56);
      this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            120000,
            0,
            0,
            0});
      this.numericUpDownTimeout.Name = "numericUpDownTimeout";
      this.numericUpDownTimeout.Size = new System.Drawing.Size(72, 20);
      this.numericUpDownTimeout.TabIndex = 3;
      this.numericUpDownTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownTimeout, "Web request timeout");
      this.numericUpDownTimeout.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
      // 
      // labelTimeout
      // 
      this.labelTimeout.Location = new System.Drawing.Point(8, 56);
      this.labelTimeout.Name = "labelTimeout";
      this.labelTimeout.Size = new System.Drawing.Size(96, 20);
      this.labelTimeout.TabIndex = 2;
      this.labelTimeout.Text = "Timeout:";
      this.labelTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxUsername.Location = new System.Drawing.Point(104, 112);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(176, 20);
      this.textBoxUsername.TabIndex = 6;
      this.toolTips.SetToolTip(this.textBoxUsername, "Username for HTTP request");
      // 
      // textBoxAddress
      // 
      this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAddress.Location = new System.Drawing.Point(104, 24);
      this.textBoxAddress.Name = "textBoxAddress";
      this.textBoxAddress.Size = new System.Drawing.Size(176, 20);
      this.textBoxAddress.TabIndex = 1;
      this.textBoxAddress.Text = "http://";
      this.toolTips.SetToolTip(this.textBoxAddress, "Web address to request");
      // 
      // labelAddress
      // 
      this.labelAddress.Location = new System.Drawing.Point(8, 24);
      this.labelAddress.Name = "labelAddress";
      this.labelAddress.Size = new System.Drawing.Size(96, 20);
      this.labelAddress.TabIndex = 0;
      this.labelAddress.Text = "Web address:";
      this.labelAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(160, 192);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(232, 192);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPassword.Location = new System.Drawing.Point(104, 144);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new System.Drawing.Size(176, 20);
      this.textBoxPassword.TabIndex = 8;
      this.toolTips.SetToolTip(this.textBoxPassword, "WARNING: This password is not securely stored.");
      // 
      // checkBoxUsernameAndPassword
      // 
      this.checkBoxUsernameAndPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxUsernameAndPassword.Location = new System.Drawing.Point(8, 88);
      this.checkBoxUsernameAndPassword.Name = "checkBoxUsernameAndPassword";
      this.checkBoxUsernameAndPassword.Size = new System.Drawing.Size(272, 20);
      this.checkBoxUsernameAndPassword.TabIndex = 4;
      this.checkBoxUsernameAndPassword.Text = "Use a username and password";
      this.toolTips.SetToolTip(this.checkBoxUsernameAndPassword, "Use a username and password for the HTTP request");
      this.checkBoxUsernameAndPassword.UseVisualStyleBackColor = true;
      // 
      // labelPassword
      // 
      this.labelPassword.Location = new System.Drawing.Point(8, 144);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(96, 20);
      this.labelPassword.TabIndex = 7;
      this.labelPassword.Text = "Password:";
      this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelUsername
      // 
      this.labelUsername.Location = new System.Drawing.Point(8, 112);
      this.labelUsername.Name = "labelUsername";
      this.labelUsername.Size = new System.Drawing.Size(96, 20);
      this.labelUsername.TabIndex = 5;
      this.labelUsername.Text = "Username:";
      this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // HttpMessageCommand
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(304, 233);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBoxMessageDetails);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "HttpMessageCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "HTTP Message Command";
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.Label labelAddress;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
    private System.Windows.Forms.Label labelTimeout;
    private System.Windows.Forms.TextBox textBoxUsername;
    private System.Windows.Forms.TextBox textBoxAddress;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.CheckBox checkBoxUsernameAndPassword;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.ToolTip toolTips;
  }
}