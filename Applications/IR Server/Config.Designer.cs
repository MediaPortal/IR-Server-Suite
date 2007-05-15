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
      this.buttonConfigureTransceiver = new System.Windows.Forms.Button();
      this.textBoxPlugin = new System.Windows.Forms.TextBox();
      this.checkBoxRunAtBoot = new System.Windows.Forms.CheckBox();
      this.radioButtonServer = new System.Windows.Forms.RadioButton();
      this.radioButtonRelay = new System.Windows.Forms.RadioButton();
      this.radioButtonRepeater = new System.Windows.Forms.RadioButton();
      this.listViewTransceiver = new System.Windows.Forms.ListView();
      this.columnHeaderTransceiver = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderCanReceive = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderCanTransmit = new System.Windows.Forms.ColumnHeader();
      this.groupBoxTransceiver = new System.Windows.Forms.GroupBox();
      this.groupBoxMode = new System.Windows.Forms.GroupBox();
      this.labelComputer = new System.Windows.Forms.Label();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.labelCurrentPlugin = new System.Windows.Forms.Label();
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
      // buttonConfigureTransceiver
      // 
      this.buttonConfigureTransceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonConfigureTransceiver.Enabled = false;
      this.buttonConfigureTransceiver.Location = new System.Drawing.Point(344, 224);
      this.buttonConfigureTransceiver.Name = "buttonConfigureTransceiver";
      this.buttonConfigureTransceiver.Size = new System.Drawing.Size(72, 24);
      this.buttonConfigureTransceiver.TabIndex = 2;
      this.buttonConfigureTransceiver.Text = "Configure";
      this.toolTips.SetToolTip(this.buttonConfigureTransceiver, "Configure the remote control transceiver");
      this.buttonConfigureTransceiver.UseVisualStyleBackColor = true;
      this.buttonConfigureTransceiver.Click += new System.EventHandler(this.buttonConfigureTransceiver_Click);
      // 
      // textBoxPlugin
      // 
      this.textBoxPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPlugin.BackColor = System.Drawing.SystemColors.ControlLight;
      this.textBoxPlugin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxPlugin.Location = new System.Drawing.Point(104, 224);
      this.textBoxPlugin.Name = "textBoxPlugin";
      this.textBoxPlugin.ReadOnly = true;
      this.textBoxPlugin.Size = new System.Drawing.Size(232, 24);
      this.textBoxPlugin.TabIndex = 1;
      this.textBoxPlugin.Text = "None set";
      this.textBoxPlugin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.textBoxPlugin, "Currently selected Remote Transceiver");
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
      // listViewTransceiver
      // 
      this.listViewTransceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewTransceiver.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTransceiver,
            this.columnHeaderCanReceive,
            this.columnHeaderCanTransmit});
      this.listViewTransceiver.FullRowSelect = true;
      this.listViewTransceiver.GridLines = true;
      this.listViewTransceiver.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewTransceiver.HideSelection = false;
      this.listViewTransceiver.Location = new System.Drawing.Point(8, 24);
      this.listViewTransceiver.MultiSelect = false;
      this.listViewTransceiver.Name = "listViewTransceiver";
      this.listViewTransceiver.ShowGroups = false;
      this.listViewTransceiver.ShowItemToolTips = true;
      this.listViewTransceiver.Size = new System.Drawing.Size(408, 192);
      this.listViewTransceiver.TabIndex = 0;
      this.listViewTransceiver.UseCompatibleStateImageBehavior = false;
      this.listViewTransceiver.View = System.Windows.Forms.View.Details;
      this.listViewTransceiver.DoubleClick += new System.EventHandler(this.listViewTransceiver_DoubleClick);
      // 
      // columnHeaderTransceiver
      // 
      this.columnHeaderTransceiver.Text = "Name";
      this.columnHeaderTransceiver.Width = 240;
      // 
      // columnHeaderCanReceive
      // 
      this.columnHeaderCanReceive.Text = "Receiver";
      this.columnHeaderCanReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderCanReceive.Width = 70;
      // 
      // columnHeaderCanTransmit
      // 
      this.columnHeaderCanTransmit.Text = "Blaster";
      this.columnHeaderCanTransmit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderCanTransmit.Width = 70;
      // 
      // groupBoxTransceiver
      // 
      this.groupBoxTransceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTransceiver.Controls.Add(this.labelCurrentPlugin);
      this.groupBoxTransceiver.Controls.Add(this.textBoxPlugin);
      this.groupBoxTransceiver.Controls.Add(this.listViewTransceiver);
      this.groupBoxTransceiver.Controls.Add(this.buttonConfigureTransceiver);
      this.groupBoxTransceiver.Location = new System.Drawing.Point(8, 8);
      this.groupBoxTransceiver.Name = "groupBoxTransceiver";
      this.groupBoxTransceiver.Size = new System.Drawing.Size(424, 256);
      this.groupBoxTransceiver.TabIndex = 0;
      this.groupBoxTransceiver.TabStop = false;
      this.groupBoxTransceiver.Text = "Device plugin";
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
      this.groupBoxMode.Text = "Server mode";
      // 
      // labelComputer
      // 
      this.labelComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelComputer.Location = new System.Drawing.Point(160, 24);
      this.labelComputer.Name = "labelComputer";
      this.labelComputer.Size = new System.Drawing.Size(256, 64);
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
      this.comboBoxComputer.Size = new System.Drawing.Size(256, 21);
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
      // labelCurrentPlugin
      // 
      this.labelCurrentPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelCurrentPlugin.Location = new System.Drawing.Point(8, 224);
      this.labelCurrentPlugin.Name = "labelCurrentPlugin";
      this.labelCurrentPlugin.Size = new System.Drawing.Size(96, 24);
      this.labelCurrentPlugin.TabIndex = 3;
      this.labelCurrentPlugin.Text = "Current plugin:";
      this.labelCurrentPlugin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      this.Load += new System.EventHandler(this.Config_Load);
      this.groupBoxTransceiver.ResumeLayout(false);
      this.groupBoxTransceiver.PerformLayout();
      this.groupBoxMode.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.GroupBox groupBoxTransceiver;
    private System.Windows.Forms.Button buttonConfigureTransceiver;
    private System.Windows.Forms.ListView listViewTransceiver;
    private System.Windows.Forms.ColumnHeader columnHeaderTransceiver;
    private System.Windows.Forms.ColumnHeader columnHeaderCanReceive;
    private System.Windows.Forms.ColumnHeader columnHeaderCanTransmit;
    private System.Windows.Forms.TextBox textBoxPlugin;
    private System.Windows.Forms.GroupBox groupBoxMode;
    private System.Windows.Forms.CheckBox checkBoxRunAtBoot;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.RadioButton radioButtonRelay;
    private System.Windows.Forms.RadioButton radioButtonServer;
    private System.Windows.Forms.RadioButton radioButtonRepeater;
    private System.Windows.Forms.Label labelComputer;
    private System.Windows.Forms.Label labelCurrentPlugin;
  }
}