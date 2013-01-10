namespace IrssCommands.General
{
  partial class WindowsMessageConfig
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
      this.groupBoxMessageDetails = new System.Windows.Forms.GroupBox();
      this.labelMessage = new System.Windows.Forms.Label();
      this.numericUpDownMsg = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownLParam = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownWParam = new System.Windows.Forms.NumericUpDown();
      this.labelLParam = new System.Windows.Forms.Label();
      this.labelWParam = new System.Windows.Forms.Label();
      this.contextMenuStripWM = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.wMAPPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.wMUSERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.textBoxTarget = new System.Windows.Forms.TextBox();
      this.buttonLocate = new System.Windows.Forms.Button();
      this.labelWindowStyle = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBoxMessageTarget = new System.Windows.Forms.GroupBox();
      this.comboBoxTargetType = new System.Windows.Forms.ComboBox();
      this.groupBoxMessageDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMsg)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLParam)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWParam)).BeginInit();
      this.contextMenuStripWM.SuspendLayout();
      this.groupBoxMessageTarget.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxMessageDetails
      // 
      this.groupBoxMessageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMessageDetails.Controls.Add(this.labelMessage);
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownMsg);
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownLParam);
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownWParam);
      this.groupBoxMessageDetails.Controls.Add(this.labelLParam);
      this.groupBoxMessageDetails.Controls.Add(this.labelWParam);
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(3, 91);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(289, 102);
      this.groupBoxMessageDetails.TabIndex = 3;
      this.groupBoxMessageDetails.TabStop = false;
      this.groupBoxMessageDetails.Text = "Details";
      // 
      // labelMessage
      // 
      this.labelMessage.AutoSize = true;
      this.labelMessage.Location = new System.Drawing.Point(6, 26);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new System.Drawing.Size(53, 13);
      this.labelMessage.TabIndex = 0;
      this.labelMessage.Text = "Message:";
      this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownMsg
      // 
      this.numericUpDownMsg.Location = new System.Drawing.Point(103, 19);
      this.numericUpDownMsg.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.numericUpDownMsg.Name = "numericUpDownMsg";
      this.numericUpDownMsg.Size = new System.Drawing.Size(104, 20);
      this.numericUpDownMsg.TabIndex = 1;
      this.numericUpDownMsg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownMsg.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownMsg, "Message identifier");
      // 
      // numericUpDownLParam
      // 
      this.numericUpDownLParam.Location = new System.Drawing.Point(103, 71);
      this.numericUpDownLParam.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.numericUpDownLParam.Name = "numericUpDownLParam";
      this.numericUpDownLParam.Size = new System.Drawing.Size(104, 20);
      this.numericUpDownLParam.TabIndex = 5;
      this.numericUpDownLParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownLParam.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownLParam, "Long Parameter (or LParam)");
      // 
      // numericUpDownWParam
      // 
      this.numericUpDownWParam.Location = new System.Drawing.Point(103, 45);
      this.numericUpDownWParam.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.numericUpDownWParam.Name = "numericUpDownWParam";
      this.numericUpDownWParam.Size = new System.Drawing.Size(104, 20);
      this.numericUpDownWParam.TabIndex = 3;
      this.numericUpDownWParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownWParam.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownWParam, "Word Paramater (or WParam)");
      // 
      // labelLParam
      // 
      this.labelLParam.AutoSize = true;
      this.labelLParam.Location = new System.Drawing.Point(5, 78);
      this.labelLParam.Name = "labelLParam";
      this.labelLParam.Size = new System.Drawing.Size(67, 13);
      this.labelLParam.TabIndex = 4;
      this.labelLParam.Text = "Long Param:";
      this.labelLParam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelWParam
      // 
      this.labelWParam.AutoSize = true;
      this.labelWParam.Location = new System.Drawing.Point(6, 52);
      this.labelWParam.Name = "labelWParam";
      this.labelWParam.Size = new System.Drawing.Size(69, 13);
      this.labelWParam.TabIndex = 2;
      this.labelWParam.Text = "Word Param:";
      this.labelWParam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // contextMenuStripWM
      // 
      this.contextMenuStripWM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wMAPPToolStripMenuItem,
            this.wMUSERToolStripMenuItem});
      this.contextMenuStripWM.Name = "contextMenuStripWM";
      this.contextMenuStripWM.Size = new System.Drawing.Size(129, 48);
      // 
      // wMAPPToolStripMenuItem
      // 
      this.wMAPPToolStripMenuItem.Name = "wMAPPToolStripMenuItem";
      this.wMAPPToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.wMAPPToolStripMenuItem.Text = "WM_APP";
      this.wMAPPToolStripMenuItem.Click += new System.EventHandler(this.wMAPPToolStripMenuItem_Click);
      // 
      // wMUSERToolStripMenuItem
      // 
      this.wMUSERToolStripMenuItem.Name = "wMUSERToolStripMenuItem";
      this.wMUSERToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
      this.wMUSERToolStripMenuItem.Text = "WM_USER";
      this.wMUSERToolStripMenuItem.Click += new System.EventHandler(this.wMUSERToolStripMenuItem_Click);
      // 
      // textBoxTarget
      // 
      this.textBoxTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTarget.Location = new System.Drawing.Point(103, 48);
      this.textBoxTarget.Name = "textBoxTarget";
      this.textBoxTarget.Size = new System.Drawing.Size(153, 20);
      this.textBoxTarget.TabIndex = 33;
      this.toolTip.SetToolTip(this.textBoxTarget, "Target");
      // 
      // buttonLocate
      // 
      this.buttonLocate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonLocate.Location = new System.Drawing.Point(262, 48);
      this.buttonLocate.Name = "buttonLocate";
      this.buttonLocate.Size = new System.Drawing.Size(24, 20);
      this.buttonLocate.TabIndex = 34;
      this.buttonLocate.Text = "...";
      this.toolTip.SetToolTip(this.buttonLocate, "Locate a target");
      this.buttonLocate.UseVisualStyleBackColor = true;
      this.buttonLocate.Click += new System.EventHandler(this.buttonLocate_Click);
      // 
      // labelWindowStyle
      // 
      this.labelWindowStyle.AutoSize = true;
      this.labelWindowStyle.Location = new System.Drawing.Point(5, 21);
      this.labelWindowStyle.Name = "labelWindowStyle";
      this.labelWindowStyle.Size = new System.Drawing.Size(64, 13);
      this.labelWindowStyle.TabIndex = 31;
      this.labelWindowStyle.Text = "Target type:";
      this.labelWindowStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 51);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(91, 13);
      this.label1.TabIndex = 32;
      this.label1.Text = "Target parameter:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // groupBoxMessageTarget
      // 
      this.groupBoxMessageTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMessageTarget.Controls.Add(this.comboBoxTargetType);
      this.groupBoxMessageTarget.Controls.Add(this.textBoxTarget);
      this.groupBoxMessageTarget.Controls.Add(this.buttonLocate);
      this.groupBoxMessageTarget.Controls.Add(this.label1);
      this.groupBoxMessageTarget.Controls.Add(this.labelWindowStyle);
      this.groupBoxMessageTarget.Location = new System.Drawing.Point(3, 3);
      this.groupBoxMessageTarget.Name = "groupBoxMessageTarget";
      this.groupBoxMessageTarget.Size = new System.Drawing.Size(292, 80);
      this.groupBoxMessageTarget.TabIndex = 2;
      this.groupBoxMessageTarget.TabStop = false;
      this.groupBoxMessageTarget.Text = "Target";
      // 
      // comboBoxTargetType
      // 
      this.comboBoxTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTargetType.FormattingEnabled = true;
      this.comboBoxTargetType.Location = new System.Drawing.Point(103, 13);
      this.comboBoxTargetType.MaxDropDownItems = 4;
      this.comboBoxTargetType.Name = "comboBoxTargetType";
      this.comboBoxTargetType.Size = new System.Drawing.Size(150, 21);
      this.comboBoxTargetType.TabIndex = 35;
      this.comboBoxTargetType.SelectedValueChanged += new System.EventHandler(this.comboBoxTargetType_SelectedValueChanged);
      // 
      // WindowsMessageConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxMessageDetails);
      this.Controls.Add(this.groupBoxMessageTarget);
      this.Name = "WindowsMessageConfig";
      this.Size = new System.Drawing.Size(295, 198);
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMsg)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLParam)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWParam)).EndInit();
      this.contextMenuStripWM.ResumeLayout(false);
      this.groupBoxMessageTarget.ResumeLayout(false);
      this.groupBoxMessageTarget.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.Label labelMessage;
    private System.Windows.Forms.NumericUpDown numericUpDownMsg;
    private System.Windows.Forms.NumericUpDown numericUpDownLParam;
    private System.Windows.Forms.NumericUpDown numericUpDownWParam;
    private System.Windows.Forms.Label labelLParam;
    private System.Windows.Forms.Label labelWParam;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripWM;
    private System.Windows.Forms.ToolStripMenuItem wMAPPToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem wMUSERToolStripMenuItem;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.Label labelWindowStyle;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBoxMessageTarget;
    private System.Windows.Forms.ComboBox comboBoxTargetType;
    private System.Windows.Forms.TextBox textBoxTarget;
    private System.Windows.Forms.Button buttonLocate;
  }
}
