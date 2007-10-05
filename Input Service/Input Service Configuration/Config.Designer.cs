namespace Configuration
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
      this.checkBoxRunAtBoot = new System.Windows.Forms.CheckBox();
      this.buttonAdvanced = new System.Windows.Forms.Button();
      this.buttonDetect = new System.Windows.Forms.Button();
      this.groupBoxTransceiver = new System.Windows.Forms.GroupBox();
      this.gridPlugins = new SourceGrid.Grid();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.groupBoxTransceiver.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(352, 304);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 5;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(424, 304);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // checkBoxRunAtBoot
      // 
      this.checkBoxRunAtBoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxRunAtBoot.AutoSize = true;
      this.checkBoxRunAtBoot.Location = new System.Drawing.Point(8, 272);
      this.checkBoxRunAtBoot.Name = "checkBoxRunAtBoot";
      this.checkBoxRunAtBoot.Size = new System.Drawing.Size(165, 17);
      this.checkBoxRunAtBoot.TabIndex = 1;
      this.checkBoxRunAtBoot.Text = "&Start IR Server with Windows";
      this.toolTips.SetToolTip(this.checkBoxRunAtBoot, "Run IR Server when windows boots up?");
      this.checkBoxRunAtBoot.UseVisualStyleBackColor = true;
      // 
      // buttonAdvanced
      // 
      this.buttonAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonAdvanced.Location = new System.Drawing.Point(80, 304);
      this.buttonAdvanced.Name = "buttonAdvanced";
      this.buttonAdvanced.Size = new System.Drawing.Size(64, 24);
      this.buttonAdvanced.TabIndex = 3;
      this.buttonAdvanced.Text = "Advanced";
      this.toolTips.SetToolTip(this.buttonAdvanced, "Click here for advanced options");
      this.buttonAdvanced.UseVisualStyleBackColor = true;
      this.buttonAdvanced.Click += new System.EventHandler(this.buttonAdvanced_Click);
      // 
      // buttonDetect
      // 
      this.buttonDetect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.buttonDetect.Location = new System.Drawing.Point(216, 304);
      this.buttonDetect.Name = "buttonDetect";
      this.buttonDetect.Size = new System.Drawing.Size(80, 24);
      this.buttonDetect.TabIndex = 4;
      this.buttonDetect.Text = "Auto-Detect";
      this.toolTips.SetToolTip(this.buttonDetect, "Click here to automatically detect attached devices");
      this.buttonDetect.UseVisualStyleBackColor = true;
      this.buttonDetect.Click += new System.EventHandler(this.buttonDetect_Click);
      // 
      // groupBoxTransceiver
      // 
      this.groupBoxTransceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTransceiver.Controls.Add(this.gridPlugins);
      this.groupBoxTransceiver.Location = new System.Drawing.Point(8, 8);
      this.groupBoxTransceiver.Name = "groupBoxTransceiver";
      this.groupBoxTransceiver.Size = new System.Drawing.Size(480, 256);
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
      this.gridPlugins.Location = new System.Drawing.Point(16, 24);
      this.gridPlugins.Name = "gridPlugins";
      this.gridPlugins.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
      this.gridPlugins.SelectionMode = SourceGrid.GridSelectionMode.Row;
      this.gridPlugins.Size = new System.Drawing.Size(448, 216);
      this.gridPlugins.TabIndex = 0;
      this.gridPlugins.TabStop = true;
      this.gridPlugins.ToolTipText = "";
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(8, 304);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(64, 24);
      this.buttonHelp.TabIndex = 2;
      this.buttonHelp.Text = "Help";
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // Config
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(496, 343);
      this.Controls.Add(this.buttonDetect);
      this.Controls.Add(this.buttonAdvanced);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.checkBoxRunAtBoot);
      this.Controls.Add(this.groupBoxTransceiver);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(504, 370);
      this.Name = "Config";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "IR Server - Configuration";
      this.groupBoxTransceiver.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.GroupBox groupBoxTransceiver;
    private System.Windows.Forms.CheckBox checkBoxRunAtBoot;
    private System.Windows.Forms.Button buttonHelp;
    private SourceGrid.Grid gridPlugins;
    private System.Windows.Forms.Button buttonAdvanced;
    private System.Windows.Forms.Button buttonDetect;
  }
}