namespace IrssCommands.General
{
  partial class EjectConfig
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
      this.comboBoxDrive = new System.Windows.Forms.ComboBox();
      this.labelDrive = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // comboBoxDrive
      // 
      this.comboBoxDrive.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.comboBoxDrive.FormattingEnabled = true;
      this.comboBoxDrive.Location = new System.Drawing.Point(16, 33);
      this.comboBoxDrive.Name = "comboBoxDrive";
      this.comboBoxDrive.Size = new System.Drawing.Size(136, 21);
      this.comboBoxDrive.TabIndex = 3;
      // 
      // labelDrive
      // 
      this.labelDrive.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelDrive.Location = new System.Drawing.Point(16, 17);
      this.labelDrive.Name = "labelDrive";
      this.labelDrive.Size = new System.Drawing.Size(136, 16);
      this.labelDrive.TabIndex = 2;
      this.labelDrive.Text = "Select drive to eject:";
      // 
      // EjectCommandConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboBoxDrive);
      this.Controls.Add(this.labelDrive);
      this.Name = "EjectCommandConfig";
      this.Size = new System.Drawing.Size(169, 70);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ComboBox comboBoxDrive;
    private System.Windows.Forms.Label labelDrive;
  }
}
