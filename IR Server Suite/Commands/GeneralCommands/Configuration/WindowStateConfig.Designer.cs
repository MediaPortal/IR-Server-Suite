namespace IrssCommands.General
{
  partial class WindowStateConfig
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
      this.groupBoxTarget = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBoxTargetType = new System.Windows.Forms.ComboBox();
      this.labelWindowStyle = new System.Windows.Forms.Label();
      this.textBoxTarget = new System.Windows.Forms.TextBox();
      this.buttonLocate = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxAction = new System.Windows.Forms.ComboBox();
      this.groupBoxTarget.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxTarget
      // 
      this.groupBoxTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTarget.Controls.Add(this.label1);
      this.groupBoxTarget.Controls.Add(this.comboBoxAction);
      this.groupBoxTarget.Controls.Add(this.comboBoxTargetType);
      this.groupBoxTarget.Controls.Add(this.label2);
      this.groupBoxTarget.Controls.Add(this.labelWindowStyle);
      this.groupBoxTarget.Controls.Add(this.textBoxTarget);
      this.groupBoxTarget.Controls.Add(this.buttonLocate);
      this.groupBoxTarget.Location = new System.Drawing.Point(3, 3);
      this.groupBoxTarget.Name = "groupBoxTarget";
      this.groupBoxTarget.Size = new System.Drawing.Size(302, 101);
      this.groupBoxTarget.TabIndex = 3;
      this.groupBoxTarget.TabStop = false;
      this.groupBoxTarget.Text = "Change window state";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 77);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(91, 13);
      this.label1.TabIndex = 30;
      this.label1.Text = "Target parameter:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxTargetType
      // 
      this.comboBoxTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTargetType.FormattingEnabled = true;
      this.comboBoxTargetType.Location = new System.Drawing.Point(104, 46);
      this.comboBoxTargetType.MaxDropDownItems = 4;
      this.comboBoxTargetType.Name = "comboBoxTargetType";
      this.comboBoxTargetType.Size = new System.Drawing.Size(150, 21);
      this.comboBoxTargetType.TabIndex = 29;
      this.comboBoxTargetType.SelectedValueChanged += new System.EventHandler(this.comboBoxTargetType_SelectedValueChanged);
      // 
      // labelWindowStyle
      // 
      this.labelWindowStyle.AutoSize = true;
      this.labelWindowStyle.Location = new System.Drawing.Point(7, 49);
      this.labelWindowStyle.Name = "labelWindowStyle";
      this.labelWindowStyle.Size = new System.Drawing.Size(64, 13);
      this.labelWindowStyle.TabIndex = 28;
      this.labelWindowStyle.Text = "Target type:";
      this.labelWindowStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxTarget
      // 
      this.textBoxTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTarget.Location = new System.Drawing.Point(104, 74);
      this.textBoxTarget.Name = "textBoxTarget";
      this.textBoxTarget.Size = new System.Drawing.Size(162, 20);
      this.textBoxTarget.TabIndex = 26;
      this.toolTip.SetToolTip(this.textBoxTarget, "Target");
      // 
      // buttonLocate
      // 
      this.buttonLocate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonLocate.Location = new System.Drawing.Point(272, 74);
      this.buttonLocate.Name = "buttonLocate";
      this.buttonLocate.Size = new System.Drawing.Size(24, 20);
      this.buttonLocate.TabIndex = 27;
      this.buttonLocate.Text = "...";
      this.toolTip.SetToolTip(this.buttonLocate, "Locate a target");
      this.buttonLocate.UseVisualStyleBackColor = true;
      this.buttonLocate.Click += new System.EventHandler(this.buttonLocate_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 22);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 28;
      this.label2.Text = "Action";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxAction
      // 
      this.comboBoxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxAction.FormattingEnabled = true;
      this.comboBoxAction.Location = new System.Drawing.Point(104, 19);
      this.comboBoxAction.MaxDropDownItems = 4;
      this.comboBoxAction.Name = "comboBoxAction";
      this.comboBoxAction.Size = new System.Drawing.Size(150, 21);
      this.comboBoxAction.TabIndex = 29;
      // 
      // WindowStateCommandConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxTarget);
      this.Name = "WindowStateCommandConfig";
      this.Size = new System.Drawing.Size(308, 107);
      this.groupBoxTarget.ResumeLayout(false);
      this.groupBoxTarget.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxTarget;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBoxAction;
    private System.Windows.Forms.ComboBox comboBoxTargetType;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label labelWindowStyle;
    private System.Windows.Forms.TextBox textBoxTarget;
    private System.Windows.Forms.Button buttonLocate;
  }
}
