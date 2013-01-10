namespace IrssCommands.General
{
  partial class HttpMessageConfig
  {
    /// <summary> 
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary> 
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.checkBoxUsernameAndPassword = new System.Windows.Forms.CheckBox();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
      this.textBoxAddress = new System.Windows.Forms.TextBox();
      this.textBoxUsername = new System.Windows.Forms.TextBox();
      this.groupBoxMessageDetails = new System.Windows.Forms.GroupBox();
      this.labelUsername = new System.Windows.Forms.Label();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelTimeout = new System.Windows.Forms.Label();
      this.labelAddress = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
      this.groupBoxMessageDetails.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBoxUsernameAndPassword
      // 
      this.checkBoxUsernameAndPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxUsernameAndPassword.Location = new System.Drawing.Point(8, 88);
      this.checkBoxUsernameAndPassword.Name = "checkBoxUsernameAndPassword";
      this.checkBoxUsernameAndPassword.Size = new System.Drawing.Size(297, 20);
      this.checkBoxUsernameAndPassword.TabIndex = 4;
      this.checkBoxUsernameAndPassword.Text = "Use a username and password";
      this.toolTip.SetToolTip(this.checkBoxUsernameAndPassword, "Use a username and password for the HTTP request");
      this.checkBoxUsernameAndPassword.UseVisualStyleBackColor = true;
      this.checkBoxUsernameAndPassword.CheckedChanged += new System.EventHandler(this.checkBoxUsernameAndPassword_CheckedChanged);
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPassword.Location = new System.Drawing.Point(104, 144);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new System.Drawing.Size(201, 20);
      this.textBoxPassword.TabIndex = 8;
      this.toolTip.SetToolTip(this.textBoxPassword, "WARNING: This password is not securely stored.");
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
      this.numericUpDownTimeout.Size = new System.Drawing.Size(109, 20);
      this.numericUpDownTimeout.TabIndex = 3;
      this.numericUpDownTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTip.SetToolTip(this.numericUpDownTimeout, "Web request timeout");
      this.numericUpDownTimeout.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
      // 
      // textBoxAddress
      // 
      this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAddress.Location = new System.Drawing.Point(104, 24);
      this.textBoxAddress.Name = "textBoxAddress";
      this.textBoxAddress.Size = new System.Drawing.Size(201, 20);
      this.textBoxAddress.TabIndex = 1;
      this.textBoxAddress.Text = "http://";
      this.toolTip.SetToolTip(this.textBoxAddress, "Web address to request");
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxUsername.Location = new System.Drawing.Point(104, 112);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(201, 20);
      this.textBoxUsername.TabIndex = 6;
      this.toolTip.SetToolTip(this.textBoxUsername, "Username for HTTP request");
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
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(3, 3);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(313, 173);
      this.groupBoxMessageDetails.TabIndex = 1;
      this.groupBoxMessageDetails.TabStop = false;
      this.groupBoxMessageDetails.Text = "HTTP message details";
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
      // labelPassword
      // 
      this.labelPassword.Location = new System.Drawing.Point(8, 144);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(96, 20);
      this.labelPassword.TabIndex = 7;
      this.labelPassword.Text = "Password:";
      this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      // labelAddress
      // 
      this.labelAddress.Location = new System.Drawing.Point(8, 24);
      this.labelAddress.Name = "labelAddress";
      this.labelAddress.Size = new System.Drawing.Size(96, 20);
      this.labelAddress.TabIndex = 0;
      this.labelAddress.Text = "Web address:";
      this.labelAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // HttpMessageConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxMessageDetails);
      this.Name = "HttpMessageConfig";
      this.Size = new System.Drawing.Size(319, 179);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.CheckBox checkBoxUsernameAndPassword;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
    private System.Windows.Forms.Label labelTimeout;
    private System.Windows.Forms.TextBox textBoxAddress;
    private System.Windows.Forms.Label labelAddress;
    private System.Windows.Forms.TextBox textBoxUsername;
  }
}
