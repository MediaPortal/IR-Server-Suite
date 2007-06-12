namespace IRServer
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
      this.radioButtonServer = new System.Windows.Forms.RadioButton();
      this.radioButtonRelay = new System.Windows.Forms.RadioButton();
      this.radioButtonRepeater = new System.Windows.Forms.RadioButton();
      this.groupBoxTransceiver = new System.Windows.Forms.GroupBox();
      this.gridPlugins = new SourceGrid.Grid();
      this.groupBoxMode = new System.Windows.Forms.GroupBox();
      this.labelComputer = new System.Windows.Forms.Label();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.groupBoxTransceiver.SuspendLayout();
      this.groupBoxMode.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(296, 408);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(368, 408);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // checkBoxRunAtBoot
      // 
      this.checkBoxRunAtBoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxRunAtBoot.Location = new System.Drawing.Point(16, 408);
      this.checkBoxRunAtBoot.Name = "checkBoxRunAtBoot";
      this.checkBoxRunAtBoot.Size = new System.Drawing.Size(176, 24);
      this.checkBoxRunAtBoot.TabIndex = 2;
      this.checkBoxRunAtBoot.Text = "&Start IR Server with Windows";
      this.toolTips.SetToolTip(this.checkBoxRunAtBoot, "Run IR Server when windows boots up?");
      this.checkBoxRunAtBoot.UseVisualStyleBackColor = true;
      // 
      // radioButtonServer
      // 
      this.radioButtonServer.Location = new System.Drawing.Point(16, 24);
      this.radioButtonServer.Name = "radioButtonServer";
      this.radioButtonServer.Size = new System.Drawing.Size(128, 24);
      this.radioButtonServer.TabIndex = 0;
      this.radioButtonServer.TabStop = true;
      this.radioButtonServer.Text = "Server mode";
      this.toolTips.SetToolTip(this.radioButtonServer, "Server mode (default)");
      this.radioButtonServer.UseVisualStyleBackColor = true;
      this.radioButtonServer.CheckedChanged += new System.EventHandler(this.radioButtonServer_CheckedChanged);
      // 
      // radioButtonRelay
      // 
      this.radioButtonRelay.Location = new System.Drawing.Point(16, 56);
      this.radioButtonRelay.Name = "radioButtonRelay";
      this.radioButtonRelay.Size = new System.Drawing.Size(128, 24);
      this.radioButtonRelay.TabIndex = 1;
      this.radioButtonRelay.TabStop = true;
      this.radioButtonRelay.Text = "Button relay mode";
      this.toolTips.SetToolTip(this.radioButtonRelay, "Relays button presses to another IR Server");
      this.radioButtonRelay.UseVisualStyleBackColor = true;
      this.radioButtonRelay.CheckedChanged += new System.EventHandler(this.radioButtonRelay_CheckedChanged);
      // 
      // radioButtonRepeater
      // 
      this.radioButtonRepeater.Location = new System.Drawing.Point(16, 88);
      this.radioButtonRepeater.Name = "radioButtonRepeater";
      this.radioButtonRepeater.Size = new System.Drawing.Size(128, 24);
      this.radioButtonRepeater.TabIndex = 2;
      this.radioButtonRepeater.TabStop = true;
      this.radioButtonRepeater.Text = "IR repeater mode";
      this.toolTips.SetToolTip(this.radioButtonRepeater, "Acts as a repeater for another IR Server\'s IR blasting");
      this.radioButtonRepeater.UseVisualStyleBackColor = true;
      this.radioButtonRepeater.CheckedChanged += new System.EventHandler(this.radioButtonRepeater_CheckedChanged);
      // 
      // groupBoxTransceiver
      // 
      this.groupBoxTransceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTransceiver.Controls.Add(this.gridPlugins);
      this.groupBoxTransceiver.Location = new System.Drawing.Point(8, 8);
      this.groupBoxTransceiver.Name = "groupBoxTransceiver";
      this.groupBoxTransceiver.Size = new System.Drawing.Size(424, 256);
      this.groupBoxTransceiver.TabIndex = 0;
      this.groupBoxTransceiver.TabStop = false;
      this.groupBoxTransceiver.Text = "Device plugin";
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
      this.gridPlugins.Size = new System.Drawing.Size(392, 216);
      this.gridPlugins.TabIndex = 0;
      this.gridPlugins.TabStop = true;
      this.gridPlugins.ToolTipText = "";
      // 
      // groupBoxMode
      // 
      this.groupBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMode.Controls.Add(this.labelComputer);
      this.groupBoxMode.Controls.Add(this.radioButtonRepeater);
      this.groupBoxMode.Controls.Add(this.radioButtonRelay);
      this.groupBoxMode.Controls.Add(this.radioButtonServer);
      this.groupBoxMode.Controls.Add(this.comboBoxComputer);
      this.groupBoxMode.Location = new System.Drawing.Point(8, 272);
      this.groupBoxMode.Name = "groupBoxMode";
      this.groupBoxMode.Size = new System.Drawing.Size(424, 120);
      this.groupBoxMode.TabIndex = 1;
      this.groupBoxMode.TabStop = false;
      this.groupBoxMode.Text = "Mode";
      // 
      // labelComputer
      // 
      this.labelComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelComputer.Location = new System.Drawing.Point(160, 56);
      this.labelComputer.Name = "labelComputer";
      this.labelComputer.Size = new System.Drawing.Size(248, 32);
      this.labelComputer.TabIndex = 3;
      this.labelComputer.Text = "Button Relay / IR Repeater mode host computer:";
      this.labelComputer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // comboBoxComputer
      // 
      this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxComputer.Enabled = false;
      this.comboBoxComputer.FormattingEnabled = true;
      this.comboBoxComputer.Location = new System.Drawing.Point(160, 88);
      this.comboBoxComputer.Name = "comboBoxComputer";
      this.comboBoxComputer.Size = new System.Drawing.Size(248, 21);
      this.comboBoxComputer.TabIndex = 4;
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.buttonHelp.Location = new System.Drawing.Point(208, 408);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(64, 24);
      this.buttonHelp.TabIndex = 3;
      this.buttonHelp.Text = "Help";
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // Config
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(440, 440);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.checkBoxRunAtBoot);
      this.Controls.Add(this.groupBoxMode);
      this.Controls.Add(this.groupBoxTransceiver);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(448, 474);
      this.Name = "Config";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "IR Server - Configuration";
      this.groupBoxTransceiver.ResumeLayout(false);
      this.groupBoxMode.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.GroupBox groupBoxTransceiver;
    private System.Windows.Forms.GroupBox groupBoxMode;
    private System.Windows.Forms.CheckBox checkBoxRunAtBoot;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.RadioButton radioButtonRelay;
    private System.Windows.Forms.RadioButton radioButtonServer;
    private System.Windows.Forms.RadioButton radioButtonRepeater;
    private System.Windows.Forms.Label labelComputer;
    private SourceGrid.Grid gridPlugins;
  }
}