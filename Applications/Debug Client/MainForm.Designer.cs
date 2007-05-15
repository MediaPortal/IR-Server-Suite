namespace DebugClient
{
  partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.buttonBlast = new System.Windows.Forms.Button();
      this.buttonLearnIR = new System.Windows.Forms.Button();
      this.labelServerAddress = new System.Windows.Forms.Label();
      this.buttonConnect = new System.Windows.Forms.Button();
      this.buttonDisconnect = new System.Windows.Forms.Button();
      this.buttonShutdownServer = new System.Windows.Forms.Button();
      this.listBoxStatus = new System.Windows.Forms.ListBox();
      this.buttonCrash = new System.Windows.Forms.Button();
      this.buttonListConnected = new System.Windows.Forms.Button();
      this.buttonPing = new System.Windows.Forms.Button();
      this.groupBoxGenerateMessage = new System.Windows.Forms.GroupBox();
      this.textBoxCustom = new System.Windows.Forms.TextBox();
      this.buttonSendCustom = new System.Windows.Forms.Button();
      this.groupBoxStatus = new System.Windows.Forms.GroupBox();
      this.groupBoxRemoteButton = new System.Windows.Forms.GroupBox();
      this.labelCustomButton = new System.Windows.Forms.Label();
      this.numericUpDownButton = new System.Windows.Forms.NumericUpDown();
      this.comboBoxRemoteButtons = new System.Windows.Forms.ComboBox();
      this.buttonSendRemoteButton = new System.Windows.Forms.Button();
      this.groupBoxSetup = new System.Windows.Forms.GroupBox();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.groupBoxCommands = new System.Windows.Forms.GroupBox();
      this.comboBoxSpeed = new System.Windows.Forms.ComboBox();
      this.comboBoxPort = new System.Windows.Forms.ComboBox();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.buttonHelp = new System.Windows.Forms.Button();
      this.groupBoxGenerateMessage.SuspendLayout();
      this.groupBoxStatus.SuspendLayout();
      this.groupBoxRemoteButton.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButton)).BeginInit();
      this.groupBoxSetup.SuspendLayout();
      this.groupBoxCommands.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonBlast
      // 
      this.buttonBlast.Location = new System.Drawing.Point(8, 48);
      this.buttonBlast.Name = "buttonBlast";
      this.buttonBlast.Size = new System.Drawing.Size(64, 24);
      this.buttonBlast.TabIndex = 2;
      this.buttonBlast.Text = "Blast IR";
      this.toolTip.SetToolTip(this.buttonBlast, "Blast learned IR code");
      this.buttonBlast.UseVisualStyleBackColor = true;
      this.buttonBlast.Click += new System.EventHandler(this.buttonBlast_Click);
      // 
      // buttonLearnIR
      // 
      this.buttonLearnIR.Location = new System.Drawing.Point(8, 16);
      this.buttonLearnIR.Name = "buttonLearnIR";
      this.buttonLearnIR.Size = new System.Drawing.Size(64, 24);
      this.buttonLearnIR.TabIndex = 0;
      this.buttonLearnIR.Text = "Learn IR";
      this.toolTip.SetToolTip(this.buttonLearnIR, "Learn an IR code");
      this.buttonLearnIR.UseVisualStyleBackColor = true;
      this.buttonLearnIR.Click += new System.EventHandler(this.buttonLearnIR_Click);
      // 
      // labelServerAddress
      // 
      this.labelServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelServerAddress.Location = new System.Drawing.Point(8, 16);
      this.labelServerAddress.Name = "labelServerAddress";
      this.labelServerAddress.Size = new System.Drawing.Size(240, 16);
      this.labelServerAddress.TabIndex = 0;
      this.labelServerAddress.Text = "IR Server host computer:";
      // 
      // buttonConnect
      // 
      this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonConnect.Location = new System.Drawing.Point(264, 32);
      this.buttonConnect.Name = "buttonConnect";
      this.buttonConnect.Size = new System.Drawing.Size(80, 24);
      this.buttonConnect.TabIndex = 2;
      this.buttonConnect.Text = "Connect";
      this.toolTip.SetToolTip(this.buttonConnect, "Connect to server");
      this.buttonConnect.UseVisualStyleBackColor = true;
      this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
      // 
      // buttonDisconnect
      // 
      this.buttonDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDisconnect.Location = new System.Drawing.Point(352, 32);
      this.buttonDisconnect.Name = "buttonDisconnect";
      this.buttonDisconnect.Size = new System.Drawing.Size(80, 24);
      this.buttonDisconnect.TabIndex = 3;
      this.buttonDisconnect.Text = "Disconnect";
      this.toolTip.SetToolTip(this.buttonDisconnect, "Disconnect from server");
      this.buttonDisconnect.UseVisualStyleBackColor = true;
      this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
      // 
      // buttonShutdownServer
      // 
      this.buttonShutdownServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonShutdownServer.Location = new System.Drawing.Point(296, 16);
      this.buttonShutdownServer.Name = "buttonShutdownServer";
      this.buttonShutdownServer.Size = new System.Drawing.Size(64, 24);
      this.buttonShutdownServer.TabIndex = 5;
      this.buttonShutdownServer.Text = "Shutdown";
      this.toolTip.SetToolTip(this.buttonShutdownServer, "Shutdown server");
      this.buttonShutdownServer.UseVisualStyleBackColor = true;
      this.buttonShutdownServer.Click += new System.EventHandler(this.buttonShutdownServer_Click);
      // 
      // listBoxStatus
      // 
      this.listBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxStatus.FormattingEnabled = true;
      this.listBoxStatus.HorizontalScrollbar = true;
      this.listBoxStatus.IntegralHeight = false;
      this.listBoxStatus.Location = new System.Drawing.Point(8, 16);
      this.listBoxStatus.Name = "listBoxStatus";
      this.listBoxStatus.ScrollAlwaysVisible = true;
      this.listBoxStatus.Size = new System.Drawing.Size(424, 192);
      this.listBoxStatus.TabIndex = 0;
      this.toolTip.SetToolTip(this.listBoxStatus, "Status messages");
      // 
      // buttonCrash
      // 
      this.buttonCrash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCrash.Location = new System.Drawing.Point(368, 16);
      this.buttonCrash.Name = "buttonCrash";
      this.buttonCrash.Size = new System.Drawing.Size(64, 24);
      this.buttonCrash.TabIndex = 6;
      this.buttonCrash.Text = "Crash";
      this.toolTip.SetToolTip(this.buttonCrash, "Crash this program");
      this.buttonCrash.UseVisualStyleBackColor = true;
      this.buttonCrash.Click += new System.EventHandler(this.buttonCrash_Click);
      // 
      // buttonListConnected
      // 
      this.buttonListConnected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonListConnected.Location = new System.Drawing.Point(368, 48);
      this.buttonListConnected.Name = "buttonListConnected";
      this.buttonListConnected.Size = new System.Drawing.Size(64, 24);
      this.buttonListConnected.TabIndex = 8;
      this.buttonListConnected.Text = "# clients";
      this.toolTip.SetToolTip(this.buttonListConnected, "Get client count from server");
      this.buttonListConnected.UseVisualStyleBackColor = true;
      this.buttonListConnected.Click += new System.EventHandler(this.buttonListConnected_Click);
      // 
      // buttonPing
      // 
      this.buttonPing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonPing.Location = new System.Drawing.Point(296, 48);
      this.buttonPing.Name = "buttonPing";
      this.buttonPing.Size = new System.Drawing.Size(64, 24);
      this.buttonPing.TabIndex = 7;
      this.buttonPing.Text = "Ping";
      this.toolTip.SetToolTip(this.buttonPing, "Ping the server");
      this.buttonPing.UseVisualStyleBackColor = true;
      this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
      // 
      // groupBoxGenerateMessage
      // 
      this.groupBoxGenerateMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxGenerateMessage.Controls.Add(this.textBoxCustom);
      this.groupBoxGenerateMessage.Controls.Add(this.buttonSendCustom);
      this.groupBoxGenerateMessage.Location = new System.Drawing.Point(8, 224);
      this.groupBoxGenerateMessage.Name = "groupBoxGenerateMessage";
      this.groupBoxGenerateMessage.Size = new System.Drawing.Size(440, 80);
      this.groupBoxGenerateMessage.TabIndex = 3;
      this.groupBoxGenerateMessage.TabStop = false;
      this.groupBoxGenerateMessage.Text = "Generate message";
      // 
      // textBoxCustom
      // 
      this.textBoxCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxCustom.Location = new System.Drawing.Point(8, 16);
      this.textBoxCustom.Multiline = true;
      this.textBoxCustom.Name = "textBoxCustom";
      this.textBoxCustom.Size = new System.Drawing.Size(352, 56);
      this.textBoxCustom.TabIndex = 0;
      this.toolTip.SetToolTip(this.textBoxCustom, "Create a custom message to send to the server");
      // 
      // buttonSendCustom
      // 
      this.buttonSendCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSendCustom.Location = new System.Drawing.Point(376, 48);
      this.buttonSendCustom.Name = "buttonSendCustom";
      this.buttonSendCustom.Size = new System.Drawing.Size(56, 24);
      this.buttonSendCustom.TabIndex = 1;
      this.buttonSendCustom.Text = "Send";
      this.toolTip.SetToolTip(this.buttonSendCustom, "Send custom message to server");
      this.buttonSendCustom.UseVisualStyleBackColor = true;
      this.buttonSendCustom.Click += new System.EventHandler(this.buttonSendCustom_Click);
      // 
      // groupBoxStatus
      // 
      this.groupBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxStatus.Controls.Add(this.listBoxStatus);
      this.groupBoxStatus.Location = new System.Drawing.Point(8, 312);
      this.groupBoxStatus.Name = "groupBoxStatus";
      this.groupBoxStatus.Size = new System.Drawing.Size(440, 216);
      this.groupBoxStatus.TabIndex = 4;
      this.groupBoxStatus.TabStop = false;
      this.groupBoxStatus.Text = "Status";
      // 
      // groupBoxRemoteButton
      // 
      this.groupBoxRemoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxRemoteButton.Controls.Add(this.labelCustomButton);
      this.groupBoxRemoteButton.Controls.Add(this.numericUpDownButton);
      this.groupBoxRemoteButton.Controls.Add(this.comboBoxRemoteButtons);
      this.groupBoxRemoteButton.Controls.Add(this.buttonSendRemoteButton);
      this.groupBoxRemoteButton.Location = new System.Drawing.Point(8, 168);
      this.groupBoxRemoteButton.Name = "groupBoxRemoteButton";
      this.groupBoxRemoteButton.Size = new System.Drawing.Size(440, 48);
      this.groupBoxRemoteButton.TabIndex = 2;
      this.groupBoxRemoteButton.TabStop = false;
      this.groupBoxRemoteButton.Text = "Remote button";
      // 
      // labelCustomButton
      // 
      this.labelCustomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelCustomButton.Location = new System.Drawing.Point(136, 16);
      this.labelCustomButton.Name = "labelCustomButton";
      this.labelCustomButton.Size = new System.Drawing.Size(144, 20);
      this.labelCustomButton.TabIndex = 1;
      this.labelCustomButton.Text = "Custom button key code:";
      this.labelCustomButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownButton
      // 
      this.numericUpDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownButton.Location = new System.Drawing.Point(288, 16);
      this.numericUpDownButton.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
      this.numericUpDownButton.Name = "numericUpDownButton";
      this.numericUpDownButton.Size = new System.Drawing.Size(72, 20);
      this.numericUpDownButton.TabIndex = 2;
      this.numericUpDownButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownButton.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownButton, "Specify a custom button code to forward to the server");
      // 
      // comboBoxRemoteButtons
      // 
      this.comboBoxRemoteButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxRemoteButtons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxRemoteButtons.FormattingEnabled = true;
      this.comboBoxRemoteButtons.Location = new System.Drawing.Point(8, 16);
      this.comboBoxRemoteButtons.Name = "comboBoxRemoteButtons";
      this.comboBoxRemoteButtons.Size = new System.Drawing.Size(120, 21);
      this.comboBoxRemoteButtons.TabIndex = 0;
      this.toolTip.SetToolTip(this.comboBoxRemoteButtons, "Choose a remote control button to forward to the server");
      this.comboBoxRemoteButtons.SelectedIndexChanged += new System.EventHandler(this.comboBoxRemoteButtons_SelectedIndexChanged);
      // 
      // buttonSendRemoteButton
      // 
      this.buttonSendRemoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSendRemoteButton.Location = new System.Drawing.Point(376, 16);
      this.buttonSendRemoteButton.Name = "buttonSendRemoteButton";
      this.buttonSendRemoteButton.Size = new System.Drawing.Size(56, 24);
      this.buttonSendRemoteButton.TabIndex = 3;
      this.buttonSendRemoteButton.Text = "Send";
      this.buttonSendRemoteButton.UseVisualStyleBackColor = true;
      this.buttonSendRemoteButton.Click += new System.EventHandler(this.buttonSendRemoteButton_Click);
      // 
      // groupBoxSetup
      // 
      this.groupBoxSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSetup.Controls.Add(this.comboBoxComputer);
      this.groupBoxSetup.Controls.Add(this.labelServerAddress);
      this.groupBoxSetup.Controls.Add(this.buttonConnect);
      this.groupBoxSetup.Controls.Add(this.buttonDisconnect);
      this.groupBoxSetup.Location = new System.Drawing.Point(8, 8);
      this.groupBoxSetup.Name = "groupBoxSetup";
      this.groupBoxSetup.Size = new System.Drawing.Size(440, 64);
      this.groupBoxSetup.TabIndex = 0;
      this.groupBoxSetup.TabStop = false;
      this.groupBoxSetup.Text = "Setup";
      // 
      // comboBoxComputer
      // 
      this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxComputer.FormattingEnabled = true;
      this.comboBoxComputer.Location = new System.Drawing.Point(8, 32);
      this.comboBoxComputer.Name = "comboBoxComputer";
      this.comboBoxComputer.Size = new System.Drawing.Size(240, 21);
      this.comboBoxComputer.TabIndex = 5;
      // 
      // groupBoxCommands
      // 
      this.groupBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCommands.Controls.Add(this.comboBoxSpeed);
      this.groupBoxCommands.Controls.Add(this.comboBoxPort);
      this.groupBoxCommands.Controls.Add(this.buttonCrash);
      this.groupBoxCommands.Controls.Add(this.buttonBlast);
      this.groupBoxCommands.Controls.Add(this.buttonLearnIR);
      this.groupBoxCommands.Controls.Add(this.buttonShutdownServer);
      this.groupBoxCommands.Controls.Add(this.buttonListConnected);
      this.groupBoxCommands.Controls.Add(this.buttonPing);
      this.groupBoxCommands.Location = new System.Drawing.Point(8, 80);
      this.groupBoxCommands.Name = "groupBoxCommands";
      this.groupBoxCommands.Size = new System.Drawing.Size(440, 80);
      this.groupBoxCommands.TabIndex = 1;
      this.groupBoxCommands.TabStop = false;
      this.groupBoxCommands.Text = "Commands";
      // 
      // comboBoxSpeed
      // 
      this.comboBoxSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxSpeed.FormattingEnabled = true;
      this.comboBoxSpeed.Location = new System.Drawing.Point(168, 50);
      this.comboBoxSpeed.Name = "comboBoxSpeed";
      this.comboBoxSpeed.Size = new System.Drawing.Size(80, 21);
      this.comboBoxSpeed.TabIndex = 4;
      this.toolTip.SetToolTip(this.comboBoxSpeed, "Speed to blast IR at");
      // 
      // comboBoxPort
      // 
      this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPort.FormattingEnabled = true;
      this.comboBoxPort.Location = new System.Drawing.Point(80, 50);
      this.comboBoxPort.Name = "comboBoxPort";
      this.comboBoxPort.Size = new System.Drawing.Size(80, 21);
      this.comboBoxPort.TabIndex = 3;
      this.toolTip.SetToolTip(this.comboBoxPort, "Port for blasting IR");
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(8, 536);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 24);
      this.buttonHelp.TabIndex = 5;
      this.buttonHelp.Text = "Help";
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(456, 566);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.groupBoxCommands);
      this.Controls.Add(this.groupBoxSetup);
      this.Controls.Add(this.groupBoxRemoteButton);
      this.Controls.Add(this.groupBoxStatus);
      this.Controls.Add(this.groupBoxGenerateMessage);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(464, 600);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Debug Client";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.groupBoxGenerateMessage.ResumeLayout(false);
      this.groupBoxGenerateMessage.PerformLayout();
      this.groupBoxStatus.ResumeLayout(false);
      this.groupBoxRemoteButton.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButton)).EndInit();
      this.groupBoxSetup.ResumeLayout(false);
      this.groupBoxCommands.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonBlast;
    private System.Windows.Forms.Button buttonLearnIR;
    private System.Windows.Forms.Label labelServerAddress;
    private System.Windows.Forms.Button buttonConnect;
    private System.Windows.Forms.Button buttonDisconnect;
    private System.Windows.Forms.Button buttonShutdownServer;
    private System.Windows.Forms.ListBox listBoxStatus;
    private System.Windows.Forms.Button buttonCrash;
    private System.Windows.Forms.Button buttonListConnected;
    private System.Windows.Forms.Button buttonPing;
    private System.Windows.Forms.GroupBox groupBoxGenerateMessage;
    private System.Windows.Forms.TextBox textBoxCustom;
    private System.Windows.Forms.Button buttonSendCustom;
    private System.Windows.Forms.GroupBox groupBoxStatus;
    private System.Windows.Forms.GroupBox groupBoxRemoteButton;
    private System.Windows.Forms.NumericUpDown numericUpDownButton;
    private System.Windows.Forms.ComboBox comboBoxRemoteButtons;
    private System.Windows.Forms.Button buttonSendRemoteButton;
    private System.Windows.Forms.Label labelCustomButton;
    private System.Windows.Forms.GroupBox groupBoxSetup;
    private System.Windows.Forms.GroupBox groupBoxCommands;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ComboBox comboBoxPort;
    private System.Windows.Forms.ComboBox comboBoxSpeed;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.Button buttonHelp;
  }
}

