namespace InputService.Configuration
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Advanced));
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
      this.groupBoxMode.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxMode
      // 
      this.groupBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
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
      this.groupBoxMode.Text = "Input Service Mode";
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
      this.toolTips.SetToolTip(this.radioButtonRepeater, "All output commands from a host Input Service are repeated");
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
      this.toolTips.SetToolTip(this.radioButtonRelay, "All input is relayed to another Input Service instance");
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
      this.toolTips.SetToolTip(this.radioButtonServer, "Input Service operates as a device server (default)");
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
      this.buttonCancel.Location = new System.Drawing.Point(328, 136);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(256, 136);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // checkBoxAbstractRemoteMode
      // 
      this.checkBoxAbstractRemoteMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxAbstractRemoteMode.AutoSize = true;
      this.checkBoxAbstractRemoteMode.Location = new System.Drawing.Point(8, 136);
      this.checkBoxAbstractRemoteMode.Name = "checkBoxAbstractRemoteMode";
      this.checkBoxAbstractRemoteMode.Size = new System.Drawing.Size(159, 17);
      this.checkBoxAbstractRemoteMode.TabIndex = 1;
      this.checkBoxAbstractRemoteMode.Text = "Use Abstract Remote Model";
      this.toolTips.SetToolTip(this.checkBoxAbstractRemoteMode, "Enable automatic abstract remote model translation");
      this.checkBoxAbstractRemoteMode.UseVisualStyleBackColor = true;
      // 
      // Advanced
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(400, 169);
      this.Controls.Add(this.checkBoxAbstractRemoteMode);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBoxMode);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(406, 194);
      this.Name = "Advanced";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Input Service Configuration - Advanced";
      this.groupBoxMode.ResumeLayout(false);
      this.groupBoxMode.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

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
  }
}