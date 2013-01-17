namespace MediaCenterBlaster
{
  partial class SetupForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonExtChannels = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.buttonChangeServer = new System.Windows.Forms.Button();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageOptions = new System.Windows.Forms.TabPage();
      this.checkBoxAutoRun = new System.Windows.Forms.CheckBox();
      this.tabPageMacros = new System.Windows.Forms.TabPage();
      this.tabPageIR = new System.Windows.Forms.TabPage();
      this.tabControl.SuspendLayout();
      this.tabPageOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(264, 256);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(328, 256);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonExtChannels
      // 
      this.buttonExtChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonExtChannels.Location = new System.Drawing.Point(280, 184);
      this.buttonExtChannels.Name = "buttonExtChannels";
      this.buttonExtChannels.Size = new System.Drawing.Size(80, 24);
      this.buttonExtChannels.TabIndex = 3;
      this.buttonExtChannels.Text = "STB Setup";
      this.toolTips.SetToolTip(this.buttonExtChannels, "Setup external channel changing");
      this.buttonExtChannels.UseVisualStyleBackColor = true;
      this.buttonExtChannels.Click += new System.EventHandler(this.buttonExtChannels_Click);
      // 
      // buttonChangeServer
      // 
      this.buttonChangeServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonChangeServer.Location = new System.Drawing.Point(8, 184);
      this.buttonChangeServer.Name = "buttonChangeServer";
      this.buttonChangeServer.Size = new System.Drawing.Size(96, 24);
      this.buttonChangeServer.TabIndex = 2;
      this.buttonChangeServer.Text = "Change Server";
      this.toolTips.SetToolTip(this.buttonChangeServer, "Change the IR Server host");
      this.buttonChangeServer.UseVisualStyleBackColor = true;
      this.buttonChangeServer.Click += new System.EventHandler(this.buttonChangeServer_Click);
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(8, 256);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 24);
      this.buttonHelp.TabIndex = 1;
      this.buttonHelp.Text = "&Help";
      this.toolTips.SetToolTip(this.buttonHelp, "Click here for help");
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.tabPageOptions);
      this.tabControl.Controls.Add(this.tabPageMacros);
      this.tabControl.Controls.Add(this.tabPageIR);
      this.tabControl.Location = new System.Drawing.Point(8, 8);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(376, 240);
      this.tabControl.TabIndex = 0;
      // 
      // tabPageOptions
      // 
      this.tabPageOptions.Controls.Add(this.checkBoxAutoRun);
      this.tabPageOptions.Controls.Add(this.buttonChangeServer);
      this.tabPageOptions.Controls.Add(this.buttonExtChannels);
      this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
      this.tabPageOptions.Name = "tabPageOptions";
      this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageOptions.Size = new System.Drawing.Size(368, 214);
      this.tabPageOptions.TabIndex = 0;
      this.tabPageOptions.Text = "General Setup";
      this.tabPageOptions.UseVisualStyleBackColor = true;
      // 
      // checkBoxAutoRun
      // 
      this.checkBoxAutoRun.AutoSize = true;
      this.checkBoxAutoRun.Location = new System.Drawing.Point(8, 48);
      this.checkBoxAutoRun.Name = "checkBoxAutoRun";
      this.checkBoxAutoRun.Size = new System.Drawing.Size(218, 17);
      this.checkBoxAutoRun.TabIndex = 1;
      this.checkBoxAutoRun.Text = "Start Media Center Blaster with Windows";
      this.checkBoxAutoRun.UseVisualStyleBackColor = true;
      this.checkBoxAutoRun.CheckedChanged += new System.EventHandler(this.checkBoxAutoRun_CheckedChanged);
      // 
      // tabPageMacros
      // 
      this.tabPageMacros.Location = new System.Drawing.Point(4, 22);
      this.tabPageMacros.Name = "tabPageMacros";
      this.tabPageMacros.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMacros.Size = new System.Drawing.Size(368, 214);
      this.tabPageMacros.TabIndex = 2;
      this.tabPageMacros.Text = "Macros";
      this.tabPageMacros.UseVisualStyleBackColor = true;
      // 
      // tabPageIR
      // 
      this.tabPageIR.Location = new System.Drawing.Point(4, 22);
      this.tabPageIR.Name = "tabPageIR";
      this.tabPageIR.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageIR.Size = new System.Drawing.Size(368, 214);
      this.tabPageIR.TabIndex = 1;
      this.tabPageIR.Text = "IR Commands";
      this.tabPageIR.UseVisualStyleBackColor = true;
      // 
      // SetupForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(392, 289);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(400, 323);
      this.Name = "SetupForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Media Center Blaster - Setup";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
      this.Load += new System.EventHandler(this.SetupForm_Load);
      this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.SetupForm_HelpRequested);
      this.tabControl.ResumeLayout(false);
      this.tabPageOptions.ResumeLayout(false);
      this.tabPageOptions.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonExtChannels;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageIR;
    private System.Windows.Forms.TabPage tabPageMacros;
    private System.Windows.Forms.Button buttonChangeServer;
    private System.Windows.Forms.TabPage tabPageOptions;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.CheckBox checkBoxAutoRun;
  }
}