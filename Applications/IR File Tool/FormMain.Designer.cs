namespace IrFileTool
{
  partial class FormMain
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.changeServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.textBoxPronto = new System.Windows.Forms.TextBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.buttonSetCarrier = new System.Windows.Forms.Button();
      this.checkBoxStoreBinary = new System.Windows.Forms.CheckBox();
      this.buttonAttemptDecode = new System.Windows.Forms.Button();
      this.buttonBlast = new System.Windows.Forms.Button();
      this.buttonLearn = new System.Windows.Forms.Button();
      this.textBoxCarrier = new System.Windows.Forms.TextBox();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.groupBoxCarrier = new System.Windows.Forms.GroupBox();
      this.groupBoxTestBlast = new System.Windows.Forms.GroupBox();
      this.comboBoxPort = new System.Windows.Forms.ComboBox();
      this.menuStrip.SuspendLayout();
      this.groupBoxCarrier.SuspendLayout();
      this.groupBoxTestBlast.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serverToolStripMenuItem});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(504, 24);
      this.menuStrip.TabIndex = 0;
      this.menuStrip.Text = "menuStrip";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.newToolStripMenuItem.Text = "&New";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.openToolStripMenuItem.Text = "&Open ...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.saveToolStripMenuItem.Text = "&Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveasToolStripMenuItem
      // 
      this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
      this.saveasToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.saveasToolStripMenuItem.Text = "Save &as ...";
      this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
      // 
      // quitToolStripMenuItem
      // 
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.quitToolStripMenuItem.Text = "&Quit";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      // 
      // serverToolStripMenuItem
      // 
      this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.changeServerToolStripMenuItem});
      this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
      this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
      this.serverToolStripMenuItem.Text = "&Server";
      // 
      // connectToolStripMenuItem
      // 
      this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
      this.connectToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this.connectToolStripMenuItem.Text = "&Connect";
      this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
      // 
      // disconnectToolStripMenuItem
      // 
      this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
      this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this.disconnectToolStripMenuItem.Text = "&Disconnect";
      this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
      // 
      // changeServerToolStripMenuItem
      // 
      this.changeServerToolStripMenuItem.Name = "changeServerToolStripMenuItem";
      this.changeServerToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this.changeServerToolStripMenuItem.Text = "Change &server";
      this.changeServerToolStripMenuItem.Click += new System.EventHandler(this.changeServerToolStripMenuItem_Click);
      // 
      // textBoxPronto
      // 
      this.textBoxPronto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPronto.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxPronto.Location = new System.Drawing.Point(8, 32);
      this.textBoxPronto.Multiline = true;
      this.textBoxPronto.Name = "textBoxPronto";
      this.textBoxPronto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxPronto.Size = new System.Drawing.Size(312, 264);
      this.textBoxPronto.TabIndex = 1;
      // 
      // buttonSetCarrier
      // 
      this.buttonSetCarrier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSetCarrier.Location = new System.Drawing.Point(112, 24);
      this.buttonSetCarrier.Name = "buttonSetCarrier";
      this.buttonSetCarrier.Size = new System.Drawing.Size(48, 24);
      this.buttonSetCarrier.TabIndex = 1;
      this.buttonSetCarrier.Text = "&Set";
      this.toolTips.SetToolTip(this.buttonSetCarrier, "Change the carrier frequency");
      this.buttonSetCarrier.UseVisualStyleBackColor = true;
      this.buttonSetCarrier.Click += new System.EventHandler(this.buttonSetCarrier_Click);
      // 
      // checkBoxStoreBinary
      // 
      this.checkBoxStoreBinary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxStoreBinary.Location = new System.Drawing.Point(336, 272);
      this.checkBoxStoreBinary.Name = "checkBoxStoreBinary";
      this.checkBoxStoreBinary.Size = new System.Drawing.Size(160, 24);
      this.checkBoxStoreBinary.TabIndex = 6;
      this.checkBoxStoreBinary.Text = "Store &mceir.dll compatible";
      this.toolTips.SetToolTip(this.checkBoxStoreBinary, "Store this IR Code in an MceIr.dll compatible form");
      this.checkBoxStoreBinary.UseVisualStyleBackColor = true;
      // 
      // buttonAttemptDecode
      // 
      this.buttonAttemptDecode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAttemptDecode.Location = new System.Drawing.Point(360, 208);
      this.buttonAttemptDecode.Name = "buttonAttemptDecode";
      this.buttonAttemptDecode.Size = new System.Drawing.Size(112, 24);
      this.buttonAttemptDecode.TabIndex = 5;
      this.buttonAttemptDecode.Text = "&Attempt Decode";
      this.toolTips.SetToolTip(this.buttonAttemptDecode, "Try to decode the IR signal into a recognised format");
      this.buttonAttemptDecode.UseVisualStyleBackColor = true;
      this.buttonAttemptDecode.Click += new System.EventHandler(this.buttonAttemptDecode_Click);
      // 
      // buttonBlast
      // 
      this.buttonBlast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonBlast.Location = new System.Drawing.Point(112, 24);
      this.buttonBlast.Name = "buttonBlast";
      this.buttonBlast.Size = new System.Drawing.Size(51, 24);
      this.buttonBlast.TabIndex = 1;
      this.buttonBlast.Text = "&Blast";
      this.toolTips.SetToolTip(this.buttonBlast, "Blast the current IR command for testing");
      this.buttonBlast.UseVisualStyleBackColor = true;
      this.buttonBlast.Click += new System.EventHandler(this.buttonBlast_Click);
      // 
      // buttonLearn
      // 
      this.buttonLearn.Enabled = false;
      this.buttonLearn.Location = new System.Drawing.Point(360, 168);
      this.buttonLearn.Name = "buttonLearn";
      this.buttonLearn.Size = new System.Drawing.Size(112, 24);
      this.buttonLearn.TabIndex = 4;
      this.buttonLearn.Text = "&Learn IR";
      this.toolTips.SetToolTip(this.buttonLearn, "Learn a new IR command");
      this.buttonLearn.UseVisualStyleBackColor = true;
      this.buttonLearn.Click += new System.EventHandler(this.buttonLearn_Click);
      // 
      // textBoxCarrier
      // 
      this.textBoxCarrier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxCarrier.Location = new System.Drawing.Point(8, 24);
      this.textBoxCarrier.Name = "textBoxCarrier";
      this.textBoxCarrier.Size = new System.Drawing.Size(96, 20);
      this.textBoxCarrier.TabIndex = 0;
      // 
      // openFileDialog
      // 
      this.openFileDialog.DefaultExt = "IR";
      this.openFileDialog.Filter = "IR Files|*.IR";
      this.openFileDialog.Title = "Open an IR file ...";
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.DefaultExt = "IR";
      this.saveFileDialog.Filter = "IR Files|*.IR";
      this.saveFileDialog.Title = "Save an IR file ...";
      // 
      // groupBoxCarrier
      // 
      this.groupBoxCarrier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCarrier.Controls.Add(this.textBoxCarrier);
      this.groupBoxCarrier.Controls.Add(this.buttonSetCarrier);
      this.groupBoxCarrier.Location = new System.Drawing.Point(328, 32);
      this.groupBoxCarrier.Name = "groupBoxCarrier";
      this.groupBoxCarrier.Size = new System.Drawing.Size(168, 56);
      this.groupBoxCarrier.TabIndex = 2;
      this.groupBoxCarrier.TabStop = false;
      this.groupBoxCarrier.Text = "Carrier frequency";
      // 
      // groupBoxTestBlast
      // 
      this.groupBoxTestBlast.Controls.Add(this.buttonBlast);
      this.groupBoxTestBlast.Controls.Add(this.comboBoxPort);
      this.groupBoxTestBlast.Enabled = false;
      this.groupBoxTestBlast.Location = new System.Drawing.Point(328, 96);
      this.groupBoxTestBlast.Name = "groupBoxTestBlast";
      this.groupBoxTestBlast.Size = new System.Drawing.Size(168, 56);
      this.groupBoxTestBlast.TabIndex = 3;
      this.groupBoxTestBlast.TabStop = false;
      this.groupBoxTestBlast.Text = "Test blast";
      // 
      // comboBoxPort
      // 
      this.comboBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxPort.FormattingEnabled = true;
      this.comboBoxPort.Location = new System.Drawing.Point(8, 24);
      this.comboBoxPort.Name = "comboBoxPort";
      this.comboBoxPort.Size = new System.Drawing.Size(96, 21);
      this.comboBoxPort.TabIndex = 0;
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(504, 305);
      this.Controls.Add(this.buttonLearn);
      this.Controls.Add(this.groupBoxTestBlast);
      this.Controls.Add(this.groupBoxCarrier);
      this.Controls.Add(this.buttonAttemptDecode);
      this.Controls.Add(this.checkBoxStoreBinary);
      this.Controls.Add(this.textBoxPronto);
      this.Controls.Add(this.menuStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip;
      this.MinimumSize = new System.Drawing.Size(512, 332);
      this.Name = "FormMain";
      this.Text = "IR File Tool";
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.groupBoxCarrier.ResumeLayout(false);
      this.groupBoxCarrier.PerformLayout();
      this.groupBoxTestBlast.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.TextBox textBoxPronto;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxCarrier;
    private System.Windows.Forms.Button buttonSetCarrier;
    private System.Windows.Forms.CheckBox checkBoxStoreBinary;
    private System.Windows.Forms.Button buttonAttemptDecode;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem changeServerToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBoxCarrier;
    private System.Windows.Forms.GroupBox groupBoxTestBlast;
    private System.Windows.Forms.Button buttonBlast;
    private System.Windows.Forms.ComboBox comboBoxPort;
    private System.Windows.Forms.Button buttonLearn;
  }
}

