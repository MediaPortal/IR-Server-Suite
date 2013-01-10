namespace IrssCommands.General
{
  partial class SendWOLConfig
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
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.labelPassword = new System.Windows.Forms.Label();
      this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxMac = new System.Windows.Forms.TextBox();
      this.labelMac = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
      this.SuspendLayout();
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.textBoxPassword.Location = new System.Drawing.Point(94, 55);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new System.Drawing.Size(176, 20);
      this.textBoxPassword.TabIndex = 14;
      this.textBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelPassword
      // 
      this.labelPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelPassword.AutoSize = true;
      this.labelPassword.Location = new System.Drawing.Point(3, 58);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(56, 13);
      this.labelPassword.TabIndex = 13;
      this.labelPassword.Text = "Password:";
      this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownPort
      // 
      this.numericUpDownPort.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownPort.Location = new System.Drawing.Point(94, 29);
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
      this.numericUpDownPort.Size = new System.Drawing.Size(176, 20);
      this.numericUpDownPort.TabIndex = 12;
      this.numericUpDownPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownPort.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
      // 
      // label1
      // 
      this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 31);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(29, 13);
      this.label1.TabIndex = 11;
      this.label1.Text = "Port:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxMac
      // 
      this.textBoxMac.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.textBoxMac.Location = new System.Drawing.Point(94, 3);
      this.textBoxMac.Name = "textBoxMac";
      this.textBoxMac.Size = new System.Drawing.Size(176, 20);
      this.textBoxMac.TabIndex = 10;
      this.textBoxMac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelMac
      // 
      this.labelMac.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelMac.AutoSize = true;
      this.labelMac.Location = new System.Drawing.Point(3, 6);
      this.labelMac.Name = "labelMac";
      this.labelMac.Size = new System.Drawing.Size(74, 13);
      this.labelMac.TabIndex = 9;
      this.labelMac.Text = "MAC Address:";
      this.labelMac.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // SendWOLConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxPassword);
      this.Controls.Add(this.labelPassword);
      this.Controls.Add(this.numericUpDownPort);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBoxMac);
      this.Controls.Add(this.labelMac);
      this.Name = "SendWOLConfig";
      this.Size = new System.Drawing.Size(273, 80);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.NumericUpDown numericUpDownPort;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxMac;
    private System.Windows.Forms.Label labelMac;
  }
}
