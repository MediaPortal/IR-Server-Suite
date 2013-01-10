namespace IrssCommands.General
{
  partial class CloseProgramConfig
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
      this.textBoxTarget = new System.Windows.Forms.TextBox();
      this.buttonLocate = new System.Windows.Forms.Button();
      this.groupBoxTarget = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBoxTargetType = new System.Windows.Forms.ComboBox();
      this.labelWindowStyle = new System.Windows.Forms.Label();
      this.groupBoxTarget.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBoxTarget
      // 
      this.textBoxTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTarget.Location = new System.Drawing.Point(103, 44);
      this.textBoxTarget.Name = "textBoxTarget";
      this.textBoxTarget.Size = new System.Drawing.Size(150, 20);
      this.textBoxTarget.TabIndex = 4;
      this.toolTip.SetToolTip(this.textBoxTarget, "Target");
      // 
      // buttonLocate
      // 
      this.buttonLocate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonLocate.Location = new System.Drawing.Point(259, 44);
      this.buttonLocate.Name = "buttonLocate";
      this.buttonLocate.Size = new System.Drawing.Size(24, 20);
      this.buttonLocate.TabIndex = 5;
      this.buttonLocate.Text = "...";
      this.toolTip.SetToolTip(this.buttonLocate, "Locate a target");
      this.buttonLocate.UseVisualStyleBackColor = true;
      this.buttonLocate.Click += new System.EventHandler(this.buttonLocate_Click);
      // 
      // groupBoxTarget
      // 
      this.groupBoxTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTarget.Controls.Add(this.label1);
      this.groupBoxTarget.Controls.Add(this.comboBoxTargetType);
      this.groupBoxTarget.Controls.Add(this.labelWindowStyle);
      this.groupBoxTarget.Controls.Add(this.textBoxTarget);
      this.groupBoxTarget.Controls.Add(this.buttonLocate);
      this.groupBoxTarget.Location = new System.Drawing.Point(3, 3);
      this.groupBoxTarget.Name = "groupBoxTarget";
      this.groupBoxTarget.Size = new System.Drawing.Size(289, 74);
      this.groupBoxTarget.TabIndex = 3;
      this.groupBoxTarget.TabStop = false;
      this.groupBoxTarget.Text = "Close program";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 47);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(91, 13);
      this.label1.TabIndex = 25;
      this.label1.Text = "Target parameter:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxTargetType
      // 
      this.comboBoxTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTargetType.FormattingEnabled = true;
      this.comboBoxTargetType.Location = new System.Drawing.Point(103, 16);
      this.comboBoxTargetType.MaxDropDownItems = 4;
      this.comboBoxTargetType.Name = "comboBoxTargetType";
      this.comboBoxTargetType.Size = new System.Drawing.Size(150, 21);
      this.comboBoxTargetType.TabIndex = 24;
      this.comboBoxTargetType.SelectedValueChanged += new System.EventHandler(this.comboBoxTargetType_SelectedValueChanged);
      // 
      // labelWindowStyle
      // 
      this.labelWindowStyle.AutoSize = true;
      this.labelWindowStyle.Location = new System.Drawing.Point(6, 19);
      this.labelWindowStyle.Name = "labelWindowStyle";
      this.labelWindowStyle.Size = new System.Drawing.Size(64, 13);
      this.labelWindowStyle.TabIndex = 23;
      this.labelWindowStyle.Text = "Target type:";
      this.labelWindowStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CloseProgramConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxTarget);
      this.Name = "CloseProgramConfig";
      this.Size = new System.Drawing.Size(295, 81);
      this.groupBoxTarget.ResumeLayout(false);
      this.groupBoxTarget.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxTarget;
    private System.Windows.Forms.TextBox textBoxTarget;
    private System.Windows.Forms.Button buttonLocate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBoxTargetType;
    private System.Windows.Forms.Label labelWindowStyle;
  }
}
