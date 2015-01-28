namespace Translator
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxHideTrayIcon = new System.Windows.Forms.CheckBox();
            this.labelPriority = new System.Windows.Forms.Label();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.groupBoxTrayIcon = new System.Windows.Forms.GroupBox();
            this.groupBoxPriority = new System.Windows.Forms.GroupBox();
            this.groupBoxVerbosity = new System.Windows.Forms.GroupBox();
            this.labelVerbosity = new System.Windows.Forms.Label();
            this.comboBoxVerbosity = new System.Windows.Forms.ComboBox();
            this.checkBoxAutoRun = new System.Windows.Forms.CheckBox();
            this.groupBoxTrayIcon.SuspendLayout();
            this.groupBoxPriority.SuspendLayout();
            this.groupBoxVerbosity.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(272, 208);
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
            this.buttonOK.Location = new System.Drawing.Point(200, 208);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 24);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxHideTrayIcon
            // 
            this.checkBoxHideTrayIcon.AutoSize = true;
            this.checkBoxHideTrayIcon.Location = new System.Drawing.Point(11, 42);
            this.checkBoxHideTrayIcon.Name = "checkBoxHideTrayIcon";
            this.checkBoxHideTrayIcon.Size = new System.Drawing.Size(109, 17);
            this.checkBoxHideTrayIcon.TabIndex = 2;
            this.checkBoxHideTrayIcon.Text = "Hide the tray icon";
            this.toolTips.SetToolTip(this.checkBoxHideTrayIcon, "Hides the tray icon until Translator is launched again");
            this.checkBoxHideTrayIcon.UseVisualStyleBackColor = true;
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
            this.comboBoxPriority.Size = new System.Drawing.Size(216, 21);
            this.comboBoxPriority.TabIndex = 1;
            // 
            // groupBoxTrayIcon
            // 
            this.groupBoxTrayIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTrayIcon.Controls.Add(this.checkBoxAutoRun);
            this.groupBoxTrayIcon.Controls.Add(this.checkBoxHideTrayIcon);
            this.groupBoxTrayIcon.Location = new System.Drawing.Point(8, 12);
            this.groupBoxTrayIcon.Name = "groupBoxTrayIcon";
            this.groupBoxTrayIcon.Size = new System.Drawing.Size(328, 65);
            this.groupBoxTrayIcon.TabIndex = 0;
            this.groupBoxTrayIcon.TabStop = false;
            this.groupBoxTrayIcon.Text = "System";
            // 
            // groupBoxPriority
            // 
            this.groupBoxPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPriority.Controls.Add(this.labelPriority);
            this.groupBoxPriority.Controls.Add(this.comboBoxPriority);
            this.groupBoxPriority.Location = new System.Drawing.Point(8, 83);
            this.groupBoxPriority.Name = "groupBoxPriority";
            this.groupBoxPriority.Size = new System.Drawing.Size(328, 56);
            this.groupBoxPriority.TabIndex = 1;
            this.groupBoxPriority.TabStop = false;
            this.groupBoxPriority.Text = "Process Priority";
            // 
            // groupBoxVerbosity
            // 
            this.groupBoxVerbosity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxVerbosity.Controls.Add(this.labelVerbosity);
            this.groupBoxVerbosity.Controls.Add(this.comboBoxVerbosity);
            this.groupBoxVerbosity.Location = new System.Drawing.Point(8, 145);
            this.groupBoxVerbosity.Name = "groupBoxVerbosity";
            this.groupBoxVerbosity.Size = new System.Drawing.Size(328, 56);
            this.groupBoxVerbosity.TabIndex = 2;
            this.groupBoxVerbosity.TabStop = false;
            this.groupBoxVerbosity.Text = "Logging";
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
            this.comboBoxVerbosity.Size = new System.Drawing.Size(216, 21);
            this.comboBoxVerbosity.TabIndex = 1;
            // 
            // checkBoxAutoRun
            // 
            this.checkBoxAutoRun.AutoSize = true;
            this.checkBoxAutoRun.Location = new System.Drawing.Point(11, 19);
            this.checkBoxAutoRun.Name = "checkBoxAutoRun";
            this.checkBoxAutoRun.Size = new System.Drawing.Size(167, 17);
            this.checkBoxAutoRun.TabIndex = 1;
            this.checkBoxAutoRun.Text = "Start Translator with Windows";
            this.checkBoxAutoRun.UseVisualStyleBackColor = true;
            // 
            // Advanced
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(344, 240);
            this.Controls.Add(this.groupBoxTrayIcon);
            this.Controls.Add(this.groupBoxVerbosity);
            this.Controls.Add(this.groupBoxPriority);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(360, 278);
            this.Name = "Advanced";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Translator - Advanced Configuration";
            this.groupBoxTrayIcon.ResumeLayout(false);
            this.groupBoxTrayIcon.PerformLayout();
            this.groupBoxPriority.ResumeLayout(false);
            this.groupBoxVerbosity.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelPriority;
    private System.Windows.Forms.ComboBox comboBoxPriority;
    private System.Windows.Forms.GroupBox groupBoxTrayIcon;
    private System.Windows.Forms.CheckBox checkBoxHideTrayIcon;
    private System.Windows.Forms.GroupBox groupBoxPriority;
    private System.Windows.Forms.GroupBox groupBoxVerbosity;
    private System.Windows.Forms.Label labelVerbosity;
    private System.Windows.Forms.ComboBox comboBoxVerbosity;
    public System.Windows.Forms.CheckBox checkBoxAutoRun;
  }
}