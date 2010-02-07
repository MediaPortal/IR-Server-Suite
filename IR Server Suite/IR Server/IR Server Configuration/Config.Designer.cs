namespace IRServer.Configuration
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
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxTransceiver = new System.Windows.Forms.GroupBox();
      this.gridPlugins = new SourceGrid.Grid();
      this.toolStrip = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonDetect = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonAdvancedSettings = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.toolStripButtonService = new System.Windows.Forms.ToolStripButton();
      this.toolStripServiceButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
      this.toolStripButtonApplication = new System.Windows.Forms.ToolStripButton();
      this.groupBoxTransceiver.SuspendLayout();
      this.toolStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(427, 400);
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
      this.buttonCancel.Location = new System.Drawing.Point(499, 400);
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
      this.groupBoxTransceiver.Size = new System.Drawing.Size(555, 360);
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
      this.gridPlugins.Size = new System.Drawing.Size(539, 336);
      this.gridPlugins.TabIndex = 0;
      this.gridPlugins.TabStop = true;
      this.gridPlugins.ToolTipText = "";
      // 
      // toolStrip
      // 
      this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDetect,
            this.toolStripButtonAdvancedSettings,
            this.toolStripButtonHelp,
            this.toolStripSeparator,
            this.toolStripLabel1,
            this.toolStripButtonService,
            this.toolStripServiceButton,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.toolStripButtonApplication});
      this.toolStrip.Location = new System.Drawing.Point(0, 0);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.Size = new System.Drawing.Size(571, 25);
      this.toolStrip.TabIndex = 3;
      // 
      // toolStripButtonDetect
      // 
      this.toolStripButtonDetect.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDetect.Name = "toolStripButtonDetect";
      this.toolStripButtonDetect.Size = new System.Drawing.Size(45, 22);
      this.toolStripButtonDetect.Text = "Detect";
      this.toolStripButtonDetect.ToolTipText = "Detect attached devices";
      this.toolStripButtonDetect.Click += new System.EventHandler(this.toolStripButtonDetect_Click);
      // 
      // toolStripButtonAdvancedSettings
      // 
      this.toolStripButtonAdvancedSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAdvancedSettings.Name = "toolStripButtonAdvancedSettings";
      this.toolStripButtonAdvancedSettings.Size = new System.Drawing.Size(64, 22);
      this.toolStripButtonAdvancedSettings.Text = "Advanced";
      this.toolStripButtonAdvancedSettings.ToolTipText = "Advanced settings";
      this.toolStripButtonAdvancedSettings.Click += new System.EventHandler(this.toolStripButtonAdvancedSettings_Click);
      // 
      // toolStripButtonHelp
      // 
      this.toolStripButtonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonHelp.Name = "toolStripButtonHelp";
      this.toolStripButtonHelp.Size = new System.Drawing.Size(36, 22);
      this.toolStripButtonHelp.Text = "Help";
      this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
      // 
      // toolStripSeparator
      // 
      this.toolStripSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.toolStripSeparator.Name = "toolStripSeparator";
      this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(47, 22);
      this.toolStripLabel1.Text = "Service:";
      // 
      // toolStripButtonService
      // 
      this.toolStripButtonService.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonService.Enabled = false;
      this.toolStripButtonService.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonService.Name = "toolStripButtonService";
      this.toolStripButtonService.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonService.Text = "Stop";
      this.toolStripButtonService.ToolTipText = "Stop the IR Server";
      this.toolStripButtonService.Click += new System.EventHandler(this.toolStripButtonService_Click);
      // 
      // toolStripServiceButton
      // 
      this.toolStripServiceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripServiceButton.Name = "toolStripServiceButton";
      this.toolStripServiceButton.Size = new System.Drawing.Size(23, 22);
      this.toolStripServiceButton.Click += new System.EventHandler(this.toolStripServiceButton_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripLabel2
      // 
      this.toolStripLabel2.Name = "toolStripLabel2";
      this.toolStripLabel2.Size = new System.Drawing.Size(71, 22);
      this.toolStripLabel2.Text = "Application:";
      // 
      // toolStripButtonApplication
      // 
      this.toolStripButtonApplication.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonApplication.Enabled = false;
      this.toolStripButtonApplication.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonApplication.Name = "toolStripButtonApplication";
      this.toolStripButtonApplication.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonApplication.Text = "Stop";
      this.toolStripButtonApplication.Click += new System.EventHandler(this.toolStripButtonApplication_Click);
      // 
      // Config
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(571, 432);
      this.Controls.Add(this.toolStrip);
      this.Controls.Add(this.groupBoxTransceiver);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimumSize = new System.Drawing.Size(480, 298);
      this.Name = "Config";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "IR Server Configuration";
      this.Load += new System.EventHandler(this.Config_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Config_FormClosing);
      this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Config_HelpRequested);
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
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
    private System.Windows.Forms.ToolStripButton toolStripButtonService;
    private System.Windows.Forms.ToolStripButton toolStripServiceButton;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    private System.Windows.Forms.ToolStripButton toolStripButtonApplication;
  }
}