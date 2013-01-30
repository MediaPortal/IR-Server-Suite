namespace TestApp
{
  partial class Form1
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

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.buttonControl = new System.Windows.Forms.Button();
      this.buttonBlastZone = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonControl
      // 
      this.buttonControl.Location = new System.Drawing.Point(64, 70);
      this.buttonControl.Name = "buttonControl";
      this.buttonControl.Size = new System.Drawing.Size(163, 23);
      this.buttonControl.TabIndex = 0;
      this.buttonControl.Text = "MPControlPlugin";
      this.buttonControl.UseVisualStyleBackColor = true;
      this.buttonControl.Click += new System.EventHandler(this.buttonControl_Click);
      // 
      // buttonBlastZone
      // 
      this.buttonBlastZone.Location = new System.Drawing.Point(64, 99);
      this.buttonBlastZone.Name = "buttonBlastZone";
      this.buttonBlastZone.Size = new System.Drawing.Size(163, 23);
      this.buttonBlastZone.TabIndex = 1;
      this.buttonBlastZone.Text = "MPBlastZonePlugin";
      this.buttonBlastZone.UseVisualStyleBackColor = true;
      this.buttonBlastZone.Click += new System.EventHandler(this.buttonBlastZone_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.buttonBlastZone);
      this.Controls.Add(this.buttonControl);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonControl;
    private System.Windows.Forms.Button buttonBlastZone;
  }
}

