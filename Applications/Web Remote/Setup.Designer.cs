namespace WebRemote
{
  partial class Setup
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBoxServerHost = new System.Windows.Forms.GroupBox();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.groupBoxWebServer = new System.Windows.Forms.GroupBox();
      this.numericUpDownWebPort = new System.Windows.Forms.NumericUpDown();
      this.labelPort = new System.Windows.Forms.Label();
      this.labelSkin = new System.Windows.Forms.Label();
      this.comboBoxSkin = new System.Windows.Forms.ComboBox();
      this.groupBoxServerHost.SuspendLayout();
      this.groupBoxWebServer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebPort)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(152, 168);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(216, 168);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // groupBoxServerHost
      // 
      this.groupBoxServerHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxServerHost.Controls.Add(this.comboBoxComputer);
      this.groupBoxServerHost.Location = new System.Drawing.Point(8, 8);
      this.groupBoxServerHost.Name = "groupBoxServerHost";
      this.groupBoxServerHost.Size = new System.Drawing.Size(264, 56);
      this.groupBoxServerHost.TabIndex = 0;
      this.groupBoxServerHost.TabStop = false;
      this.groupBoxServerHost.Text = "IR Server host";
      // 
      // comboBoxComputer
      // 
      this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxComputer.FormattingEnabled = true;
      this.comboBoxComputer.Location = new System.Drawing.Point(8, 24);
      this.comboBoxComputer.Name = "comboBoxComputer";
      this.comboBoxComputer.Size = new System.Drawing.Size(248, 21);
      this.comboBoxComputer.TabIndex = 0;
      // 
      // groupBoxWebServer
      // 
      this.groupBoxWebServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxWebServer.Controls.Add(this.numericUpDownWebPort);
      this.groupBoxWebServer.Controls.Add(this.labelPort);
      this.groupBoxWebServer.Controls.Add(this.labelSkin);
      this.groupBoxWebServer.Controls.Add(this.comboBoxSkin);
      this.groupBoxWebServer.Location = new System.Drawing.Point(8, 72);
      this.groupBoxWebServer.Name = "groupBoxWebServer";
      this.groupBoxWebServer.Size = new System.Drawing.Size(264, 88);
      this.groupBoxWebServer.TabIndex = 1;
      this.groupBoxWebServer.TabStop = false;
      this.groupBoxWebServer.Text = "Web Server";
      // 
      // numericUpDownWebPort
      // 
      this.numericUpDownWebPort.Location = new System.Drawing.Point(88, 24);
      this.numericUpDownWebPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDownWebPort.Name = "numericUpDownWebPort";
      this.numericUpDownWebPort.Size = new System.Drawing.Size(72, 20);
      this.numericUpDownWebPort.TabIndex = 1;
      this.numericUpDownWebPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelPort
      // 
      this.labelPort.Location = new System.Drawing.Point(8, 24);
      this.labelPort.Name = "labelPort";
      this.labelPort.Size = new System.Drawing.Size(80, 20);
      this.labelPort.TabIndex = 0;
      this.labelPort.Text = "Server Port:";
      this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelSkin
      // 
      this.labelSkin.Location = new System.Drawing.Point(8, 53);
      this.labelSkin.Name = "labelSkin";
      this.labelSkin.Size = new System.Drawing.Size(80, 24);
      this.labelSkin.TabIndex = 2;
      this.labelSkin.Text = "Skin:";
      this.labelSkin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxSkin
      // 
      this.comboBoxSkin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxSkin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxSkin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxSkin.FormattingEnabled = true;
      this.comboBoxSkin.Location = new System.Drawing.Point(88, 56);
      this.comboBoxSkin.Name = "comboBoxSkin";
      this.comboBoxSkin.Size = new System.Drawing.Size(168, 21);
      this.comboBoxSkin.TabIndex = 3;
      // 
      // Setup
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(280, 207);
      this.Controls.Add(this.groupBoxWebServer);
      this.Controls.Add(this.groupBoxServerHost);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(288, 234);
      this.Name = "Setup";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Web Remote Setup";
      this.groupBoxServerHost.ResumeLayout(false);
      this.groupBoxWebServer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebPort)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.GroupBox groupBoxServerHost;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.GroupBox groupBoxWebServer;
    private System.Windows.Forms.NumericUpDown numericUpDownWebPort;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.Label labelSkin;
    private System.Windows.Forms.ComboBox comboBoxSkin;
  }
}