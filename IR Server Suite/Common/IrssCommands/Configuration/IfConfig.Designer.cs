namespace IrssCommands
{
  partial class IfConfig
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
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.textBoxLabel2 = new System.Windows.Forms.TextBox();
      this.textBoxLabel1 = new System.Windows.Forms.TextBox();
      this.comboBoxComparer = new System.Windows.Forms.ComboBox();
      this.textBoxParam2 = new System.Windows.Forms.TextBox();
      this.textBoxParam1 = new System.Windows.Forms.TextBox();
      this.labelElseGoto = new System.Windows.Forms.Label();
      this.labelThenGoto = new System.Windows.Forms.Label();
      this.labelIf = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxLabel2
      // 
      this.textBoxLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxLabel2.Location = new System.Drawing.Point(67, 131);
      this.textBoxLabel2.Name = "textBoxLabel2";
      this.textBoxLabel2.Size = new System.Drawing.Size(100, 20);
      this.textBoxLabel2.TabIndex = 17;
      this.toolTips.SetToolTip(this.textBoxLabel2, "If this statement evaluates false then goto this label (optional)");
      // 
      // textBoxLabel1
      // 
      this.textBoxLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxLabel1.Location = new System.Drawing.Point(67, 99);
      this.textBoxLabel1.Name = "textBoxLabel1";
      this.textBoxLabel1.Size = new System.Drawing.Size(100, 20);
      this.textBoxLabel1.TabIndex = 15;
      this.toolTips.SetToolTip(this.textBoxLabel1, "If this statement evaluates true then goto this label");
      // 
      // comboBoxComparer
      // 
      this.comboBoxComparer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComparer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxComparer.FormattingEnabled = true;
      this.comboBoxComparer.Location = new System.Drawing.Point(3, 35);
      this.comboBoxComparer.Name = "comboBoxComparer";
      this.comboBoxComparer.Size = new System.Drawing.Size(164, 21);
      this.comboBoxComparer.TabIndex = 12;
      this.toolTips.SetToolTip(this.comboBoxComparer, "The method of comparing the two parameters");
      // 
      // textBoxParam2
      // 
      this.textBoxParam2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxParam2.Location = new System.Drawing.Point(3, 67);
      this.textBoxParam2.Name = "textBoxParam2";
      this.textBoxParam2.Size = new System.Drawing.Size(164, 20);
      this.textBoxParam2.TabIndex = 13;
      this.toolTips.SetToolTip(this.textBoxParam2, "Enter the second variable/value to compare here");
      // 
      // textBoxParam1
      // 
      this.textBoxParam1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxParam1.Location = new System.Drawing.Point(27, 3);
      this.textBoxParam1.Name = "textBoxParam1";
      this.textBoxParam1.Size = new System.Drawing.Size(140, 20);
      this.textBoxParam1.TabIndex = 11;
      this.toolTips.SetToolTip(this.textBoxParam1, "Enter the first variable/value to compare here");
      // 
      // labelElseGoto
      // 
      this.labelElseGoto.Location = new System.Drawing.Point(3, 131);
      this.labelElseGoto.Name = "labelElseGoto";
      this.labelElseGoto.Size = new System.Drawing.Size(64, 20);
      this.labelElseGoto.TabIndex = 16;
      this.labelElseGoto.Text = "else goto";
      this.labelElseGoto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelThenGoto
      // 
      this.labelThenGoto.Location = new System.Drawing.Point(3, 99);
      this.labelThenGoto.Name = "labelThenGoto";
      this.labelThenGoto.Size = new System.Drawing.Size(64, 20);
      this.labelThenGoto.TabIndex = 14;
      this.labelThenGoto.Text = "then goto";
      this.labelThenGoto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelIf
      // 
      this.labelIf.Location = new System.Drawing.Point(3, 3);
      this.labelIf.Name = "labelIf";
      this.labelIf.Size = new System.Drawing.Size(24, 20);
      this.labelIf.TabIndex = 10;
      this.labelIf.Text = "if";
      this.labelIf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // IfConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxLabel2);
      this.Controls.Add(this.labelElseGoto);
      this.Controls.Add(this.textBoxLabel1);
      this.Controls.Add(this.comboBoxComparer);
      this.Controls.Add(this.textBoxParam2);
      this.Controls.Add(this.textBoxParam1);
      this.Controls.Add(this.labelThenGoto);
      this.Controls.Add(this.labelIf);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "IfConfig";
      this.Size = new System.Drawing.Size(170, 159);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxLabel2;
    private System.Windows.Forms.TextBox textBoxLabel1;
    private System.Windows.Forms.ComboBox comboBoxComparer;
    private System.Windows.Forms.TextBox textBoxParam2;
    private System.Windows.Forms.TextBox textBoxParam1;
    private System.Windows.Forms.Label labelElseGoto;
    private System.Windows.Forms.Label labelThenGoto;
    private System.Windows.Forms.Label labelIf;





  }
}
