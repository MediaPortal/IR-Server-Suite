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
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.textBoxPronto = new System.Windows.Forms.TextBox();
      this.labelCarrier = new System.Windows.Forms.Label();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.buttonSetCarrier = new System.Windows.Forms.Button();
      this.checkBoxStoreBinary = new System.Windows.Forms.CheckBox();
      this.buttonAttemptDecode = new System.Windows.Forms.Button();
      this.textBoxCarrier = new System.Windows.Forms.TextBox();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.menuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(497, 24);
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
      this.textBoxPronto.Size = new System.Drawing.Size(481, 136);
      this.textBoxPronto.TabIndex = 1;
      // 
      // labelCarrier
      // 
      this.labelCarrier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelCarrier.Location = new System.Drawing.Point(8, 176);
      this.labelCarrier.Name = "labelCarrier";
      this.labelCarrier.Size = new System.Drawing.Size(56, 20);
      this.labelCarrier.TabIndex = 2;
      this.labelCarrier.Text = "Carrier:";
      this.labelCarrier.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonSetCarrier
      // 
      this.buttonSetCarrier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonSetCarrier.Location = new System.Drawing.Point(136, 176);
      this.buttonSetCarrier.Name = "buttonSetCarrier";
      this.buttonSetCarrier.Size = new System.Drawing.Size(32, 20);
      this.buttonSetCarrier.TabIndex = 4;
      this.buttonSetCarrier.Text = "Set";
      this.toolTips.SetToolTip(this.buttonSetCarrier, "Change the carrier frequency");
      this.buttonSetCarrier.UseVisualStyleBackColor = true;
      this.buttonSetCarrier.Click += new System.EventHandler(this.buttonSetCarrier_Click);
      // 
      // checkBoxStoreBinary
      // 
      this.checkBoxStoreBinary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxStoreBinary.Location = new System.Drawing.Point(328, 176);
      this.checkBoxStoreBinary.Name = "checkBoxStoreBinary";
      this.checkBoxStoreBinary.Size = new System.Drawing.Size(160, 24);
      this.checkBoxStoreBinary.TabIndex = 6;
      this.checkBoxStoreBinary.Text = "Store mceir.dll compatible";
      this.toolTips.SetToolTip(this.checkBoxStoreBinary, "Store this IR Code in an MceIr.dll compatible form");
      this.checkBoxStoreBinary.UseVisualStyleBackColor = true;
      // 
      // buttonAttemptDecode
      // 
      this.buttonAttemptDecode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonAttemptDecode.Location = new System.Drawing.Point(192, 176);
      this.buttonAttemptDecode.Name = "buttonAttemptDecode";
      this.buttonAttemptDecode.Size = new System.Drawing.Size(112, 24);
      this.buttonAttemptDecode.TabIndex = 5;
      this.buttonAttemptDecode.Text = "Attempt decode";
      this.toolTips.SetToolTip(this.buttonAttemptDecode, "Try to decode the IR signal into a recognised format");
      this.buttonAttemptDecode.UseVisualStyleBackColor = true;
      this.buttonAttemptDecode.Click += new System.EventHandler(this.buttonAttemptDecode_Click);
      // 
      // textBoxCarrier
      // 
      this.textBoxCarrier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.textBoxCarrier.Location = new System.Drawing.Point(64, 176);
      this.textBoxCarrier.Name = "textBoxCarrier";
      this.textBoxCarrier.Size = new System.Drawing.Size(64, 20);
      this.textBoxCarrier.TabIndex = 3;
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
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(497, 209);
      this.Controls.Add(this.buttonAttemptDecode);
      this.Controls.Add(this.checkBoxStoreBinary);
      this.Controls.Add(this.buttonSetCarrier);
      this.Controls.Add(this.textBoxCarrier);
      this.Controls.Add(this.labelCarrier);
      this.Controls.Add(this.textBoxPronto);
      this.Controls.Add(this.menuStrip);
      this.MainMenuStrip = this.menuStrip;
      this.MinimumSize = new System.Drawing.Size(505, 236);
      this.Name = "FormMain";
      this.Text = "IR File Tool";
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
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
    private System.Windows.Forms.Label labelCarrier;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxCarrier;
    private System.Windows.Forms.Button buttonSetCarrier;
    private System.Windows.Forms.CheckBox checkBoxStoreBinary;
    private System.Windows.Forms.Button buttonAttemptDecode;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
  }
}

