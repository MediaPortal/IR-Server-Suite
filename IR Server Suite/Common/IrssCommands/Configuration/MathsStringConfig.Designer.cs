namespace IrssCommands
{
  partial class MathsStringConfig
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
      this.textBoxOutputVar = new System.Windows.Forms.TextBox();
      this.labelInput2 = new System.Windows.Forms.Label();
      this.labelInput1 = new System.Windows.Forms.Label();
      this.textBoxInput2 = new System.Windows.Forms.TextBox();
      this.textBoxInput1 = new System.Windows.Forms.TextBox();
      this.labelVarPrefix = new System.Windows.Forms.Label();
      this.labelOutput = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxOutputVar
      // 
      this.textBoxOutputVar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOutputVar.Location = new System.Drawing.Point(107, 67);
      this.textBoxOutputVar.Name = "textBoxOutputVar";
      this.textBoxOutputVar.Size = new System.Drawing.Size(96, 20);
      this.textBoxOutputVar.TabIndex = 15;
      this.toolTips.SetToolTip(this.textBoxOutputVar, "The variable to place the output of the operation into");
      // 
      // labelInput2
      // 
      this.labelInput2.Location = new System.Drawing.Point(3, 35);
      this.labelInput2.Name = "labelInput2";
      this.labelInput2.Size = new System.Drawing.Size(72, 21);
      this.labelInput2.TabIndex = 11;
      this.labelInput2.Text = "Input 2:";
      this.labelInput2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTips.SetToolTip(this.labelInput2, "The second input string");
      // 
      // labelInput1
      // 
      this.labelInput1.Location = new System.Drawing.Point(3, 3);
      this.labelInput1.Name = "labelInput1";
      this.labelInput1.Size = new System.Drawing.Size(72, 21);
      this.labelInput1.TabIndex = 9;
      this.labelInput1.Text = "Input 1:";
      this.labelInput1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTips.SetToolTip(this.labelInput1, "The first input string");
      // 
      // textBoxInput2
      // 
      this.textBoxInput2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxInput2.Location = new System.Drawing.Point(75, 35);
      this.textBoxInput2.Name = "textBoxInput2";
      this.textBoxInput2.Size = new System.Drawing.Size(128, 20);
      this.textBoxInput2.TabIndex = 12;
      this.toolTips.SetToolTip(this.textBoxInput2, "The second operation input (sometimes optional)");
      // 
      // textBoxInput1
      // 
      this.textBoxInput1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxInput1.Location = new System.Drawing.Point(75, 3);
      this.textBoxInput1.Name = "textBoxInput1";
      this.textBoxInput1.Size = new System.Drawing.Size(128, 20);
      this.textBoxInput1.TabIndex = 10;
      this.toolTips.SetToolTip(this.textBoxInput1, "The first operation input");
      // 
      // labelVarPrefix
      // 
      this.labelVarPrefix.Location = new System.Drawing.Point(75, 67);
      this.labelVarPrefix.Name = "labelVarPrefix";
      this.labelVarPrefix.Size = new System.Drawing.Size(32, 21);
      this.labelVarPrefix.TabIndex = 14;
      this.labelVarPrefix.Text = "var_";
      this.labelVarPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelOutput
      // 
      this.labelOutput.Location = new System.Drawing.Point(3, 67);
      this.labelOutput.Name = "labelOutput";
      this.labelOutput.Size = new System.Drawing.Size(72, 21);
      this.labelOutput.TabIndex = 13;
      this.labelOutput.Text = "Output:";
      this.labelOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // MathsStringConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.labelVarPrefix);
      this.Controls.Add(this.labelOutput);
      this.Controls.Add(this.textBoxOutputVar);
      this.Controls.Add(this.labelInput2);
      this.Controls.Add(this.labelInput1);
      this.Controls.Add(this.textBoxInput2);
      this.Controls.Add(this.textBoxInput1);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "MathsStringConfig";
      this.Size = new System.Drawing.Size(206, 94);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxOutputVar;
    private System.Windows.Forms.Label labelInput2;
    private System.Windows.Forms.Label labelInput1;
    private System.Windows.Forms.TextBox textBoxInput2;
    private System.Windows.Forms.TextBox textBoxInput1;
    private System.Windows.Forms.Label labelVarPrefix;
    private System.Windows.Forms.Label labelOutput;




  }
}
