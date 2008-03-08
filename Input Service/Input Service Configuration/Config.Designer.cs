namespace InputService.Configuration
{
  partial class Config
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Config));
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxTransceiver = new System.Windows.Forms.GroupBox();
      this.gridPlugins = new SourceGrid.Grid();
      this.toolStrip = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonDetect = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonAdvancedSettings = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
      this.groupBoxTransceiver.SuspendLayout();
      this.toolStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(360, 272);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(432, 272);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // groupBoxTransceiver
      // 
      this.groupBoxTransceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTransceiver.Controls.Add(this.gridPlugins);
      this.groupBoxTransceiver.Location = new System.Drawing.Point(8, 32);
      this.groupBoxTransceiver.Name = "groupBoxTransceiver";
      this.groupBoxTransceiver.Size = new System.Drawing.Size(488, 232);
      this.groupBoxTransceiver.TabIndex = 0;
      this.groupBoxTransceiver.TabStop = false;
      this.groupBoxTransceiver.Text = "Device plugins";
      // 
      // gridPlugins
      // 
      this.gridPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.gridPlugins.BackColor = System.Drawing.SystemColors.Window;
      this.gridPlugins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.gridPlugins.Location = new System.Drawing.Point(8, 16);
      this.gridPlugins.Name = "gridPlugins";
      this.gridPlugins.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
      this.gridPlugins.SelectionMode = SourceGrid.GridSelectionMode.Row;
      this.gridPlugins.Size = new System.Drawing.Size(472, 208);
      this.gridPlugins.TabIndex = 0;
      this.gridPlugins.TabStop = true;
      this.gridPlugins.ToolTipText = "";
      // 
      // toolStrip
      // 
      this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDetect,
            this.toolStripButtonAdvancedSettings,
            this.toolStripButtonHelp});
      this.toolStrip.Location = new System.Drawing.Point(0, 0);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.Size = new System.Drawing.Size(504, 25);
      this.toolStrip.TabIndex = 3;
      // 
      // toolStripButtonDetect
      // 
      this.toolStripButtonDetect.Image = global::InputService.Configuration.Properties.Resources.Detect;
      this.toolStripButtonDetect.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDetect.Name = "toolStripButtonDetect";
      this.toolStripButtonDetect.Size = new System.Drawing.Size(59, 22);
      this.toolStripButtonDetect.Text = "Detect";
      this.toolStripButtonDetect.ToolTipText = "Detect attached devices";
      this.toolStripButtonDetect.Click += new System.EventHandler(this.toolStripButtonDetect_Click);
      // 
      // toolStripButtonAdvancedSettings
      // 
      this.toolStripButtonAdvancedSettings.Image = global::InputService.Configuration.Properties.Resources.Advanced;
      this.toolStripButtonAdvancedSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAdvancedSettings.Name = "toolStripButtonAdvancedSettings";
      this.toolStripButtonAdvancedSettings.Size = new System.Drawing.Size(75, 22);
      this.toolStripButtonAdvancedSettings.Text = "Advanced";
      this.toolStripButtonAdvancedSettings.ToolTipText = "Advanced settings";
      this.toolStripButtonAdvancedSettings.Click += new System.EventHandler(this.toolStripButtonAdvancedSettings_Click);
      // 
      // toolStripButtonHelp
      // 
      this.toolStripButtonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripButtonHelp.Image = global::InputService.Configuration.Properties.Resources.Help;
      this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonHelp.Name = "toolStripButtonHelp";
      this.toolStripButtonHelp.Size = new System.Drawing.Size(48, 22);
      this.toolStripButtonHelp.Text = "Help";
      this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
      // 
      // Config
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(504, 305);
      this.Controls.Add(this.toolStrip);
      this.Controls.Add(this.groupBoxTransceiver);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(512, 332);
      this.Name = "Config";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Input Service Configuration";
      this.groupBoxTransceiver.ResumeLayout(false);
      this.toolStrip.ResumeLayout(false);
      this.toolStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.GroupBox groupBoxTransceiver;
    private SourceGrid.Grid gridPlugins;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton toolStripButtonDetect;
    private System.Windows.Forms.ToolStripButton toolStripButtonAdvancedSettings;
    private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
  }
}