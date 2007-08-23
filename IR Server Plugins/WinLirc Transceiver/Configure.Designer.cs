namespace WinLircTransceiver
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
      this.textBoxServerAddress = new System.Windows.Forms.TextBox();
      this.numericUpDownServerPort = new System.Windows.Forms.NumericUpDown();
      this.checkBoxStartServer = new System.Windows.Forms.CheckBox();
      this.textBoxServerPath = new System.Windows.Forms.TextBox();
      this.buttonLocate = new System.Windows.Forms.Button();
      this.numericUpDownButtonReleaseTime = new System.Windows.Forms.NumericUpDown();
      this.labelServerAddress = new System.Windows.Forms.Label();
      this.labelServerPort = new System.Windows.Forms.Label();
      this.groupBoxServerDetails = new System.Windows.Forms.GroupBox();
      this.labelButtonReleaseTime = new System.Windows.Forms.Label();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.buttonCreateIRFiles = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownServerPort)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonReleaseTime)).BeginInit();
      this.groupBoxServerDetails.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(104, 240);
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
      this.buttonCancel.Location = new System.Drawing.Point(176, 240);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxServerAddress
      // 
      this.textBoxServerAddress.Location = new System.Drawing.Point(128, 24);
      this.textBoxServerAddress.Name = "textBoxServerAddress";
      this.textBoxServerAddress.Size = new System.Drawing.Size(96, 20);
      this.textBoxServerAddress.TabIndex = 1;
      this.toolTips.SetToolTip(this.textBoxServerAddress, "IP Address for WinLirc server");
      // 
      // numericUpDownServerPort
      // 
      this.numericUpDownServerPort.Location = new System.Drawing.Point(128, 56);
      this.numericUpDownServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDownServerPort.Name = "numericUpDownServerPort";
      this.numericUpDownServerPort.Size = new System.Drawing.Size(96, 20);
      this.numericUpDownServerPort.TabIndex = 3;
      this.numericUpDownServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownServerPort, "TCP Port for WinLirc server");
      // 
      // checkBoxStartServer
      // 
      this.checkBoxStartServer.AutoSize = true;
      this.checkBoxStartServer.Location = new System.Drawing.Point(8, 96);
      this.checkBoxStartServer.Name = "checkBoxStartServer";
      this.checkBoxStartServer.Size = new System.Drawing.Size(119, 17);
      this.checkBoxStartServer.TabIndex = 4;
      this.checkBoxStartServer.Text = "Start WinLirc server";
      this.toolTips.SetToolTip(this.checkBoxStartServer, "Start the WinLirc server application?");
      this.checkBoxStartServer.UseVisualStyleBackColor = true;
      // 
      // textBoxServerPath
      // 
      this.textBoxServerPath.Location = new System.Drawing.Point(8, 120);
      this.textBoxServerPath.Name = "textBoxServerPath";
      this.textBoxServerPath.Size = new System.Drawing.Size(184, 20);
      this.textBoxServerPath.TabIndex = 5;
      this.toolTips.SetToolTip(this.textBoxServerPath, "Path to WinLirc server application");
      // 
      // buttonLocate
      // 
      this.buttonLocate.AutoEllipsis = true;
      this.buttonLocate.Location = new System.Drawing.Point(200, 120);
      this.buttonLocate.Name = "buttonLocate";
      this.buttonLocate.Size = new System.Drawing.Size(24, 20);
      this.buttonLocate.TabIndex = 6;
      this.buttonLocate.Text = "...";
      this.toolTips.SetToolTip(this.buttonLocate, "Locate WinLirc server application");
      this.buttonLocate.UseVisualStyleBackColor = true;
      this.buttonLocate.Click += new System.EventHandler(this.buttonLocate_Click);
      // 
      // numericUpDownButtonReleaseTime
      // 
      this.numericUpDownButtonReleaseTime.Location = new System.Drawing.Point(144, 176);
      this.numericUpDownButtonReleaseTime.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDownButtonReleaseTime.Name = "numericUpDownButtonReleaseTime";
      this.numericUpDownButtonReleaseTime.Size = new System.Drawing.Size(96, 20);
      this.numericUpDownButtonReleaseTime.TabIndex = 2;
      this.numericUpDownButtonReleaseTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownButtonReleaseTime, "Button release time");
      // 
      // labelServerAddress
      // 
      this.labelServerAddress.Location = new System.Drawing.Point(8, 24);
      this.labelServerAddress.Name = "labelServerAddress";
      this.labelServerAddress.Size = new System.Drawing.Size(112, 20);
      this.labelServerAddress.TabIndex = 0;
      this.labelServerAddress.Text = "Server address:";
      this.labelServerAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelServerPort
      // 
      this.labelServerPort.Location = new System.Drawing.Point(8, 56);
      this.labelServerPort.Name = "labelServerPort";
      this.labelServerPort.Size = new System.Drawing.Size(112, 20);
      this.labelServerPort.TabIndex = 2;
      this.labelServerPort.Text = "Server port:";
      this.labelServerPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // groupBoxServerDetails
      // 
      this.groupBoxServerDetails.Controls.Add(this.buttonLocate);
      this.groupBoxServerDetails.Controls.Add(this.textBoxServerPath);
      this.groupBoxServerDetails.Controls.Add(this.checkBoxStartServer);
      this.groupBoxServerDetails.Controls.Add(this.numericUpDownServerPort);
      this.groupBoxServerDetails.Controls.Add(this.labelServerAddress);
      this.groupBoxServerDetails.Controls.Add(this.labelServerPort);
      this.groupBoxServerDetails.Controls.Add(this.textBoxServerAddress);
      this.groupBoxServerDetails.Location = new System.Drawing.Point(8, 8);
      this.groupBoxServerDetails.Name = "groupBoxServerDetails";
      this.groupBoxServerDetails.Size = new System.Drawing.Size(232, 152);
      this.groupBoxServerDetails.TabIndex = 0;
      this.groupBoxServerDetails.TabStop = false;
      this.groupBoxServerDetails.Text = "WinLirc Server";
      // 
      // labelButtonReleaseTime
      // 
      this.labelButtonReleaseTime.Location = new System.Drawing.Point(8, 176);
      this.labelButtonReleaseTime.Name = "labelButtonReleaseTime";
      this.labelButtonReleaseTime.Size = new System.Drawing.Size(112, 20);
      this.labelButtonReleaseTime.TabIndex = 1;
      this.labelButtonReleaseTime.Text = "Button release time:";
      this.labelButtonReleaseTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // openFileDialog
      // 
      this.openFileDialog.FileName = "openFileDialog";
      this.openFileDialog.Filter = "All Files|*.*";
      this.openFileDialog.Title = "Locate WinLirc server application";
      // 
      // buttonCreateIRFiles
      // 
      this.buttonCreateIRFiles.Location = new System.Drawing.Point(8, 208);
      this.buttonCreateIRFiles.Name = "buttonCreateIRFiles";
      this.buttonCreateIRFiles.Size = new System.Drawing.Size(104, 24);
      this.buttonCreateIRFiles.TabIndex = 3;
      this.buttonCreateIRFiles.Text = "Create IR files";
      this.toolTips.SetToolTip(this.buttonCreateIRFiles, "Click here to make IR Command files for use with this plugin");
      this.buttonCreateIRFiles.UseVisualStyleBackColor = true;
      this.buttonCreateIRFiles.Click += new System.EventHandler(this.buttonCreateIRFiles_Click);
      // 
      // Configure
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(248, 272);
      this.Controls.Add(this.buttonCreateIRFiles);
      this.Controls.Add(this.numericUpDownButtonReleaseTime);
      this.Controls.Add(this.labelButtonReleaseTime);
      this.Controls.Add(this.groupBoxServerDetails);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 275);
      this.Name = "Configure";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "WinLirc Configuration";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownServerPort)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonReleaseTime)).EndInit();
      this.groupBoxServerDetails.ResumeLayout(false);
      this.groupBoxServerDetails.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelServerAddress;
    private System.Windows.Forms.TextBox textBoxServerAddress;
    private System.Windows.Forms.Label labelServerPort;
    private System.Windows.Forms.NumericUpDown numericUpDownServerPort;
    private System.Windows.Forms.GroupBox groupBoxServerDetails;
    private System.Windows.Forms.TextBox textBoxServerPath;
    private System.Windows.Forms.CheckBox checkBoxStartServer;
    private System.Windows.Forms.Button buttonLocate;
    private System.Windows.Forms.Label labelButtonReleaseTime;
    private System.Windows.Forms.NumericUpDown numericUpDownButtonReleaseTime;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.Button buttonCreateIRFiles;
  }
}