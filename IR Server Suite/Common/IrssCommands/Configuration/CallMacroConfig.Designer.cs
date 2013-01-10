namespace IrssCommands
{
  partial class CallMacroConfig
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
      this.listView = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // listView
      // 
      this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listView.HideSelection = false;
      this.listView.Location = new System.Drawing.Point(3, 3);
      this.listView.MultiSelect = false;
      this.listView.Name = "listView";
      this.listView.Size = new System.Drawing.Size(156, 78);
      this.listView.TabIndex = 0;
      this.listView.UseCompatibleStateImageBehavior = false;
      this.listView.View = System.Windows.Forms.View.List;
      // 
      // NoConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.listView);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "NoConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listView;

  }
}
