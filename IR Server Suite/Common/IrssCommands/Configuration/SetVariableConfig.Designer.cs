namespace IrssCommands
{
  partial class SetVariableConfig
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
      this.textBoxValue = new System.Windows.Forms.TextBox();
      this.textBoxVariable = new System.Windows.Forms.TextBox();
      this.labelValue = new System.Windows.Forms.Label();
      this.labelVariable = new System.Windows.Forms.Label();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.labelVarPrefix = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxValue
      // 
      this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxValue.Location = new System.Drawing.Point(67, 48);
      this.textBoxValue.Name = "textBoxValue";
      this.textBoxValue.Size = new System.Drawing.Size(92, 20);
      this.textBoxValue.TabIndex = 10;
      // 
      // textBoxVariable
      // 
      this.textBoxVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxVariable.Location = new System.Drawing.Point(99, 16);
      this.textBoxVariable.Name = "textBoxVariable";
      this.textBoxVariable.Size = new System.Drawing.Size(60, 20);
      this.textBoxVariable.TabIndex = 8;
      this.toolTips.SetToolTip(this.textBoxVariable, "Type your variable  name here (without the variable prefix)");
      // 
      // labelValue
      // 
      this.labelValue.Location = new System.Drawing.Point(3, 48);
      this.labelValue.Name = "labelValue";
      this.labelValue.Size = new System.Drawing.Size(64, 20);
      this.labelValue.TabIndex = 9;
      this.labelValue.Text = "Value:";
      this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVariable
      // 
      this.labelVariable.Location = new System.Drawing.Point(3, 16);
      this.labelVariable.Name = "labelVariable";
      this.labelVariable.Size = new System.Drawing.Size(64, 20);
      this.labelVariable.TabIndex = 7;
      this.labelVariable.Text = "Variable:";
      this.labelVariable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVarPrefix
      // 
      this.labelVarPrefix.Location = new System.Drawing.Point(67, 16);
      this.labelVarPrefix.Name = "labelVarPrefix";
      this.labelVarPrefix.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix.TabIndex = 13;
      this.labelVarPrefix.Text = "var_";
      this.labelVarPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // SetVariableConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxValue);
      this.Controls.Add(this.textBoxVariable);
      this.Controls.Add(this.labelValue);
      this.Controls.Add(this.labelVariable);
      this.Controls.Add(this.labelVarPrefix);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "SetVariableConfig";
      this.Size = new System.Drawing.Size(162, 84);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxValue;
    private System.Windows.Forms.TextBox textBoxVariable;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelValue;
    private System.Windows.Forms.Label labelVariable;
    private System.Windows.Forms.Label labelVarPrefix;

  }
}
