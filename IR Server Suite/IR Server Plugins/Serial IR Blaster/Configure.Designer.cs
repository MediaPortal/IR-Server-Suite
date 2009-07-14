namespace IRServer.Plugin
{
  partial class Configure
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
      this.labelPort = new System.Windows.Forms.Label();
      this.comboBoxPort = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(80, 48);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(152, 48);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelPort
      // 
      this.labelPort.Location = new System.Drawing.Point(6, 14);
      this.labelPort.Name = "labelPort";
      this.labelPort.Size = new System.Drawing.Size(106, 21);
      this.labelPort.TabIndex = 0;
      this.labelPort.Text = "Serial port:";
      this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxPort
      // 
      this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPort.FormattingEnabled = true;
      this.comboBoxPort.Location = new System.Drawing.Point(112, 14);
      this.comboBoxPort.Name = "comboBoxPort";
      this.comboBoxPort.Size = new System.Drawing.Size(102, 21);
      this.comboBoxPort.TabIndex = 1;
      this.toolTips.SetToolTip(this.comboBoxPort, "Select the serial port that the Serial IR Blaster is attached to");
      // 
      // Configure
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(226, 81);
      this.Controls.Add(this.comboBoxPort);
      this.Controls.Add(this.labelPort);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(232, 106);
      this.Name = "Configure";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Serial IR Blaster Configuration";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.ComboBox comboBoxPort;
  }
}