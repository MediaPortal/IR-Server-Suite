namespace IRServer.Configuration
{
  partial class Advanced
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
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.labelComputer = new System.Windows.Forms.Label();
            this.radioButtonRepeater = new System.Windows.Forms.RadioButton();
            this.radioButtonRelay = new System.Windows.Forms.RadioButton();
            this.radioButtonServer = new System.Windows.Forms.RadioButton();
            this.comboBoxComputer = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxAbstractRemoteMode = new System.Windows.Forms.CheckBox();
            this.buttonExclusions = new System.Windows.Forms.Button();
            this.groupBoxAbstractRemoteModel = new System.Windows.Forms.GroupBox();
            this.groupBoxPriority = new System.Windows.Forms.GroupBox();
            this.labelPriority = new System.Windows.Forms.Label();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.groupBoxLogging = new System.Windows.Forms.GroupBox();
            this.labelVerbosity = new System.Windows.Forms.Label();
            this.comboBoxVerbosity = new System.Windows.Forms.ComboBox();
            this.groupBoxMode.SuspendLayout();
            this.groupBoxAbstractRemoteModel.SuspendLayout();
            this.groupBoxPriority.SuspendLayout();
            this.groupBoxLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMode.Controls.Add(this.labelComputer);
            this.groupBoxMode.Controls.Add(this.radioButtonRepeater);
            this.groupBoxMode.Controls.Add(this.radioButtonRelay);
            this.groupBoxMode.Controls.Add(this.radioButtonServer);
            this.groupBoxMode.Controls.Add(this.comboBoxComputer);
            this.groupBoxMode.Location = new System.Drawing.Point(8, 8);
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.Size = new System.Drawing.Size(384, 120);
            this.groupBoxMode.TabIndex = 0;
            this.groupBoxMode.TabStop = false;
            this.groupBoxMode.Text = "IR Server Mode";
            // 
            // labelComputer
            // 
            this.labelComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelComputer.Location = new System.Drawing.Point(168, 56);
            this.labelComputer.Name = "labelComputer";
            this.labelComputer.Size = new System.Drawing.Size(208, 32);
            this.labelComputer.TabIndex = 3;
            this.labelComputer.Text = "Input Relay / Repeater host computer:";
            this.labelComputer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // radioButtonRepeater
            // 
            this.radioButtonRepeater.AutoSize = true;
            this.radioButtonRepeater.Location = new System.Drawing.Point(16, 88);
            this.radioButtonRepeater.Name = "radioButtonRepeater";
            this.radioButtonRepeater.Size = new System.Drawing.Size(98, 17);
            this.radioButtonRepeater.TabIndex = 2;
            this.radioButtonRepeater.TabStop = true;
            this.radioButtonRepeater.Text = "Repeater mode";
            this.toolTips.SetToolTip(this.radioButtonRepeater, "All output commands from a host IR Server are repeated");
            this.radioButtonRepeater.UseVisualStyleBackColor = true;
            this.radioButtonRepeater.CheckedChanged += new System.EventHandler(this.radioButtonRepeater_CheckedChanged);
            // 
            // radioButtonRelay
            // 
            this.radioButtonRelay.AutoSize = true;
            this.radioButtonRelay.Location = new System.Drawing.Point(16, 56);
            this.radioButtonRelay.Name = "radioButtonRelay";
            this.radioButtonRelay.Size = new System.Drawing.Size(103, 17);
            this.radioButtonRelay.TabIndex = 1;
            this.radioButtonRelay.TabStop = true;
            this.radioButtonRelay.Text = "Input relay mode";
            this.toolTips.SetToolTip(this.radioButtonRelay, "All input is relayed to another IR Server instance");
            this.radioButtonRelay.UseVisualStyleBackColor = true;
            this.radioButtonRelay.CheckedChanged += new System.EventHandler(this.radioButtonRelay_CheckedChanged);
            // 
            // radioButtonServer
            // 
            this.radioButtonServer.AutoSize = true;
            this.radioButtonServer.Location = new System.Drawing.Point(16, 24);
            this.radioButtonServer.Name = "radioButtonServer";
            this.radioButtonServer.Size = new System.Drawing.Size(126, 17);
            this.radioButtonServer.TabIndex = 0;
            this.radioButtonServer.TabStop = true;
            this.radioButtonServer.Text = "Server mode (default)";
            this.toolTips.SetToolTip(this.radioButtonServer, "IR Server operates as a device server (default)");
            this.radioButtonServer.UseVisualStyleBackColor = true;
            this.radioButtonServer.CheckedChanged += new System.EventHandler(this.radioButtonServer_CheckedChanged);
            // 
            // comboBoxComputer
            // 
            this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxComputer.Enabled = false;
            this.comboBoxComputer.FormattingEnabled = true;
            this.comboBoxComputer.Location = new System.Drawing.Point(168, 88);
            this.comboBoxComputer.Name = "comboBoxComputer";
            this.comboBoxComputer.Size = new System.Drawing.Size(208, 21);
            this.comboBoxComputer.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(328, 331);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 24);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(256, 331);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 24);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxAbstractRemoteMode
            // 
            this.checkBoxAbstractRemoteMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAbstractRemoteMode.Location = new System.Drawing.Point(16, 24);
            this.checkBoxAbstractRemoteMode.Name = "checkBoxAbstractRemoteMode";
            this.checkBoxAbstractRemoteMode.Size = new System.Drawing.Size(264, 24);
            this.checkBoxAbstractRemoteMode.TabIndex = 0;
            this.checkBoxAbstractRemoteMode.Text = "Use the Abstract Remote Model";
            this.toolTips.SetToolTip(this.checkBoxAbstractRemoteMode, "Enable automatic abstract remote model translation");
            this.checkBoxAbstractRemoteMode.UseVisualStyleBackColor = true;
            this.checkBoxAbstractRemoteMode.CheckedChanged += new System.EventHandler(this.checkBoxAbstractRemoteMode_CheckedChanged);
            // 
            // buttonExclusions
            // 
            this.buttonExclusions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExclusions.Location = new System.Drawing.Point(296, 24);
            this.buttonExclusions.Name = "buttonExclusions";
            this.buttonExclusions.Size = new System.Drawing.Size(80, 24);
            this.buttonExclusions.TabIndex = 1;
            this.buttonExclusions.Text = "Exclusions";
            this.toolTips.SetToolTip(this.buttonExclusions, "Configure Abstract Remote Model exclusions");
            this.buttonExclusions.UseVisualStyleBackColor = true;
            this.buttonExclusions.Visible = false;
            this.buttonExclusions.Click += new System.EventHandler(this.buttonExclusions_Click);
            // 
            // groupBoxAbstractRemoteModel
            // 
            this.groupBoxAbstractRemoteModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAbstractRemoteModel.Controls.Add(this.buttonExclusions);
            this.groupBoxAbstractRemoteModel.Controls.Add(this.checkBoxAbstractRemoteMode);
            this.groupBoxAbstractRemoteModel.Location = new System.Drawing.Point(8, 136);
            this.groupBoxAbstractRemoteModel.Name = "groupBoxAbstractRemoteModel";
            this.groupBoxAbstractRemoteModel.Size = new System.Drawing.Size(384, 56);
            this.groupBoxAbstractRemoteModel.TabIndex = 1;
            this.groupBoxAbstractRemoteModel.TabStop = false;
            this.groupBoxAbstractRemoteModel.Text = "Abstract Remote Model";
            // 
            // groupBoxPriority
            // 
            this.groupBoxPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPriority.Controls.Add(this.labelPriority);
            this.groupBoxPriority.Controls.Add(this.comboBoxPriority);
            this.groupBoxPriority.Location = new System.Drawing.Point(8, 200);
            this.groupBoxPriority.Name = "groupBoxPriority";
            this.groupBoxPriority.Size = new System.Drawing.Size(384, 56);
            this.groupBoxPriority.TabIndex = 2;
            this.groupBoxPriority.TabStop = false;
            this.groupBoxPriority.Text = "Process Priority";
            // 
            // labelPriority
            // 
            this.labelPriority.Location = new System.Drawing.Point(8, 21);
            this.labelPriority.Name = "labelPriority";
            this.labelPriority.Size = new System.Drawing.Size(88, 24);
            this.labelPriority.TabIndex = 0;
            this.labelPriority.Text = "Set Priority:";
            this.labelPriority.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxPriority
            // 
            this.comboBoxPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPriority.FormattingEnabled = true;
            this.comboBoxPriority.Location = new System.Drawing.Point(104, 24);
            this.comboBoxPriority.Name = "comboBoxPriority";
            this.comboBoxPriority.Size = new System.Drawing.Size(272, 21);
            this.comboBoxPriority.TabIndex = 1;
            // 
            // groupBoxLogging
            // 
            this.groupBoxLogging.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLogging.Controls.Add(this.labelVerbosity);
            this.groupBoxLogging.Controls.Add(this.comboBoxVerbosity);
            this.groupBoxLogging.Location = new System.Drawing.Point(8, 262);
            this.groupBoxLogging.Name = "groupBoxLogging";
            this.groupBoxLogging.Size = new System.Drawing.Size(384, 56);
            this.groupBoxLogging.TabIndex = 2;
            this.groupBoxLogging.TabStop = false;
            this.groupBoxLogging.Text = "Logging";
            // 
            // labelVerbosity
            // 
            this.labelVerbosity.Location = new System.Drawing.Point(8, 21);
            this.labelVerbosity.Name = "labelVerbosity";
            this.labelVerbosity.Size = new System.Drawing.Size(88, 24);
            this.labelVerbosity.TabIndex = 0;
            this.labelVerbosity.Text = "Verbosity Level:";
            this.labelVerbosity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxVerbosity
            // 
            this.comboBoxVerbosity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVerbosity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVerbosity.FormattingEnabled = true;
            this.comboBoxVerbosity.Location = new System.Drawing.Point(104, 24);
            this.comboBoxVerbosity.Name = "comboBoxVerbosity";
            this.comboBoxVerbosity.Size = new System.Drawing.Size(272, 21);
            this.comboBoxVerbosity.TabIndex = 1;
            // 
            // Advanced
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(400, 367);
            this.Controls.Add(this.groupBoxLogging);
            this.Controls.Add(this.groupBoxPriority);
            this.Controls.Add(this.groupBoxAbstractRemoteModel);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 336);
            this.Name = "Advanced";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IR Server Configuration - Advanced";
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.groupBoxAbstractRemoteModel.ResumeLayout(false);
            this.groupBoxPriority.ResumeLayout(false);
            this.groupBoxLogging.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxMode;
    private System.Windows.Forms.Label labelComputer;
    private System.Windows.Forms.RadioButton radioButtonRepeater;
    private System.Windows.Forms.RadioButton radioButtonRelay;
    private System.Windows.Forms.RadioButton radioButtonServer;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.CheckBox checkBoxAbstractRemoteMode;
    private System.Windows.Forms.Button buttonExclusions;
    private System.Windows.Forms.GroupBox groupBoxAbstractRemoteModel;
    private System.Windows.Forms.GroupBox groupBoxPriority;
    private System.Windows.Forms.ComboBox comboBoxPriority;
    private System.Windows.Forms.Label labelPriority;
    private System.Windows.Forms.GroupBox groupBoxLogging;
    private System.Windows.Forms.Label labelVerbosity;
    private System.Windows.Forms.ComboBox comboBoxVerbosity;
  }
}