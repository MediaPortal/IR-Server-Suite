namespace MicrosoftMceTransceiver
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
      this.groupBoxHidIr = new System.Windows.Forms.GroupBox();
      this.radioButtonEnabled = new System.Windows.Forms.RadioButton();
      this.radioButtonDisabled = new System.Windows.Forms.RadioButton();
      this.buttonOK = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxHidIr.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxHidIr
      // 
      this.groupBoxHidIr.Controls.Add(this.radioButtonDisabled);
      this.groupBoxHidIr.Controls.Add(this.radioButtonEnabled);
      this.groupBoxHidIr.Location = new System.Drawing.Point(8, 8);
      this.groupBoxHidIr.Name = "groupBoxHidIr";
      this.groupBoxHidIr.Size = new System.Drawing.Size(168, 96);
      this.groupBoxHidIr.TabIndex = 0;
      this.groupBoxHidIr.TabStop = false;
      this.groupBoxHidIr.Text = "Microsoft HidIr Service";
      // 
      // radioButtonEnabled
      // 
      this.radioButtonEnabled.Location = new System.Drawing.Point(16, 24);
      this.radioButtonEnabled.Name = "radioButtonEnabled";
      this.radioButtonEnabled.Size = new System.Drawing.Size(80, 24);
      this.radioButtonEnabled.TabIndex = 0;
      this.radioButtonEnabled.TabStop = true;
      this.radioButtonEnabled.Text = "Enabled";
      this.toolTips.SetToolTip(this.radioButtonEnabled, "With the HidIr Service enabled the MCE Keyboard and some MCE Remote buttons will " +
              "be handled by the operating system");
      this.radioButtonEnabled.UseVisualStyleBackColor = true;
      // 
      // radioButtonDisabled
      // 
      this.radioButtonDisabled.Location = new System.Drawing.Point(16, 56);
      this.radioButtonDisabled.Name = "radioButtonDisabled";
      this.radioButtonDisabled.Size = new System.Drawing.Size(80, 24);
      this.radioButtonDisabled.TabIndex = 1;
      this.radioButtonDisabled.TabStop = true;
      this.radioButtonDisabled.Text = "Disabled";
      this.toolTips.SetToolTip(this.radioButtonDisabled, "With the HidIr Service disabled the MCE Keyboard and MCE Remote will NOT be handl" +
              "ed by the operating system");
      this.radioButtonDisabled.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(112, 112);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // Advanced
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(184, 144);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBoxHidIr);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Advanced";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Advanced Setup";
      this.groupBoxHidIr.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxHidIr;
    private System.Windows.Forms.RadioButton radioButtonDisabled;
    private System.Windows.Forms.RadioButton radioButtonEnabled;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Button buttonOK;
  }
}