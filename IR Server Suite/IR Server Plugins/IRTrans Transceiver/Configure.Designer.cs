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
      this.labelServerAddress = new System.Windows.Forms.Label();
      this.groupBoxServer = new System.Windows.Forms.GroupBox();
      this.labelServerPort = new System.Windows.Forms.Label();
      this.labelRemoteModel = new System.Windows.Forms.Label();
      this.textBoxServerAddress = new System.Windows.Forms.TextBox();
      this.textBoxRemoteModel = new System.Windows.Forms.TextBox();
      this.numericUpDownServerPort = new System.Windows.Forms.NumericUpDown();
      this.groupBoxServer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownServerPort)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(136, 136);
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
      this.buttonCancel.Location = new System.Drawing.Point(208, 136);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelServerAddress
      // 
      this.labelServerAddress.Location = new System.Drawing.Point(8, 24);
      this.labelServerAddress.Name = "labelServerAddress";
      this.labelServerAddress.Size = new System.Drawing.Size(104, 20);
      this.labelServerAddress.TabIndex = 0;
      this.labelServerAddress.Text = "Server address:";
      this.labelServerAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // groupBoxServer
      // 
      this.groupBoxServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxServer.Controls.Add(this.numericUpDownServerPort);
      this.groupBoxServer.Controls.Add(this.textBoxRemoteModel);
      this.groupBoxServer.Controls.Add(this.textBoxServerAddress);
      this.groupBoxServer.Controls.Add(this.labelRemoteModel);
      this.groupBoxServer.Controls.Add(this.labelServerPort);
      this.groupBoxServer.Controls.Add(this.labelServerAddress);
      this.groupBoxServer.Location = new System.Drawing.Point(8, 8);
      this.groupBoxServer.Name = "groupBoxServer";
      this.groupBoxServer.Size = new System.Drawing.Size(264, 120);
      this.groupBoxServer.TabIndex = 0;
      this.groupBoxServer.TabStop = false;
      this.groupBoxServer.Text = "IRTrans server setup";
      // 
      // labelServerPort
      // 
      this.labelServerPort.Location = new System.Drawing.Point(8, 56);
      this.labelServerPort.Name = "labelServerPort";
      this.labelServerPort.Size = new System.Drawing.Size(104, 20);
      this.labelServerPort.TabIndex = 2;
      this.labelServerPort.Text = "Server port:";
      this.labelServerPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelRemoteModel
      // 
      this.labelRemoteModel.Location = new System.Drawing.Point(8, 88);
      this.labelRemoteModel.Name = "labelRemoteModel";
      this.labelRemoteModel.Size = new System.Drawing.Size(104, 20);
      this.labelRemoteModel.TabIndex = 4;
      this.labelRemoteModel.Text = "Remote model:";
      this.labelRemoteModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxServerAddress
      // 
      this.textBoxServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxServerAddress.Location = new System.Drawing.Point(112, 24);
      this.textBoxServerAddress.Name = "textBoxServerAddress";
      this.textBoxServerAddress.Size = new System.Drawing.Size(144, 20);
      this.textBoxServerAddress.TabIndex = 1;
      // 
      // textBoxRemoteModel
      // 
      this.textBoxRemoteModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxRemoteModel.Location = new System.Drawing.Point(112, 88);
      this.textBoxRemoteModel.Name = "textBoxRemoteModel";
      this.textBoxRemoteModel.Size = new System.Drawing.Size(144, 20);
      this.textBoxRemoteModel.TabIndex = 5;
      // 
      // numericUpDownServerPort
      // 
      this.numericUpDownServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownServerPort.Location = new System.Drawing.Point(112, 56);
      this.numericUpDownServerPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
      this.numericUpDownServerPort.Name = "numericUpDownServerPort";
      this.numericUpDownServerPort.Size = new System.Drawing.Size(144, 20);
      this.numericUpDownServerPort.TabIndex = 3;
      this.numericUpDownServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // Configure
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(280, 168);
      this.ControlBox = false;
      this.Controls.Add(this.groupBoxServer);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MinimumSize = new System.Drawing.Size(286, 200);
      this.Name = "Configure";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "IRTrans Configuration";
      this.groupBoxServer.ResumeLayout(false);
      this.groupBoxServer.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownServerPort)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelServerAddress;
    private System.Windows.Forms.GroupBox groupBoxServer;
    private System.Windows.Forms.Label labelServerPort;
    private System.Windows.Forms.TextBox textBoxRemoteModel;
    private System.Windows.Forms.TextBox textBoxServerAddress;
    private System.Windows.Forms.Label labelRemoteModel;
    private System.Windows.Forms.NumericUpDown numericUpDownServerPort;
  }
}