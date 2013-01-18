namespace IrssCommands
{
  partial class SwapVariablesConfig
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
      this.textBoxVariable2 = new System.Windows.Forms.TextBox();
      this.textBoxVariable1 = new System.Windows.Forms.TextBox();
      this.labelVariable2 = new System.Windows.Forms.Label();
      this.labelVariable1 = new System.Windows.Forms.Label();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.labelVarPrefix2 = new System.Windows.Forms.Label();
      this.labelVarPrefix1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxVariable2
      // 
      this.textBoxVariable2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxVariable2.Location = new System.Drawing.Point(99, 48);
      this.textBoxVariable2.Name = "textBoxVariable2";
      this.textBoxVariable2.Size = new System.Drawing.Size(60, 20);
      this.textBoxVariable2.TabIndex = 13;
      this.toolTips.SetToolTip(this.textBoxVariable2, "The second variable to swap with the first");
      // 
      // textBoxVariable1
      // 
      this.textBoxVariable1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxVariable1.Location = new System.Drawing.Point(99, 16);
      this.textBoxVariable1.Name = "textBoxVariable1";
      this.textBoxVariable1.Size = new System.Drawing.Size(60, 20);
      this.textBoxVariable1.TabIndex = 10;
      this.toolTips.SetToolTip(this.textBoxVariable1, "The first variable to swap with the second");
      // 
      // labelVariable2
      // 
      this.labelVariable2.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelVariable2.Location = new System.Drawing.Point(3, 48);
      this.labelVariable2.Name = "labelVariable2";
      this.labelVariable2.Size = new System.Drawing.Size(64, 20);
      this.labelVariable2.TabIndex = 11;
      this.labelVariable2.Text = "Variable 2:";
      this.labelVariable2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVariable1
      // 
      this.labelVariable1.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelVariable1.Location = new System.Drawing.Point(3, 16);
      this.labelVariable1.Name = "labelVariable1";
      this.labelVariable1.Size = new System.Drawing.Size(64, 20);
      this.labelVariable1.TabIndex = 8;
      this.labelVariable1.Text = "Variable 1:";
      this.labelVariable1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVarPrefix2
      // 
      this.labelVarPrefix2.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelVarPrefix2.Location = new System.Drawing.Point(67, 48);
      this.labelVarPrefix2.Name = "labelVarPrefix2";
      this.labelVarPrefix2.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix2.TabIndex = 12;
      this.labelVarPrefix2.Text = "var_";
      this.labelVarPrefix2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelVarPrefix1
      // 
      this.labelVarPrefix1.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelVarPrefix1.Location = new System.Drawing.Point(67, 16);
      this.labelVarPrefix1.Name = "labelVarPrefix1";
      this.labelVarPrefix1.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix1.TabIndex = 9;
      this.labelVarPrefix1.Text = "var_";
      this.labelVarPrefix1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // SetVariableConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxVariable2);
      this.Controls.Add(this.textBoxVariable1);
      this.Controls.Add(this.labelVariable2);
      this.Controls.Add(this.labelVariable1);
      this.Controls.Add(this.labelVarPrefix2);
      this.Controls.Add(this.labelVarPrefix1);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "SetVariableConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.Load += new System.EventHandler(this.SetVariableConfig_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxVariable2;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxVariable1;
    private System.Windows.Forms.Label labelVariable2;
    private System.Windows.Forms.Label labelVariable1;
    private System.Windows.Forms.Label labelVarPrefix2;
    private System.Windows.Forms.Label labelVarPrefix1;


  }
}
