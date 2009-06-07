namespace VirtualRemote
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
      this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.toolStripComboBoxSkin = new System.Windows.Forms.ToolStripComboBox();
      this.changeServerHostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
      this.labelDisabled = new System.Windows.Forms.Label();
      this.contextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuStrip
      // 
      this.contextMenuStrip.DropShadowEnabled = false;
      this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxSkin,
            this.changeServerHostToolStripMenuItem,
            this.toolStripMenuItemHelp,
            this.toolStripMenuItemQuit});
      this.contextMenuStrip.Name = "contextMenuStrip";
      this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.contextMenuStrip.ShowImageMargin = false;
      this.contextMenuStrip.Size = new System.Drawing.Size(156, 95);
      // 
      // toolStripComboBoxSkin
      // 
      this.toolStripComboBoxSkin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.toolStripComboBoxSkin.Name = "toolStripComboBoxSkin";
      this.toolStripComboBoxSkin.Size = new System.Drawing.Size(120, 21);
      this.toolStripComboBoxSkin.ToolTipText = "Select a custom skin";
      this.toolStripComboBoxSkin.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxSkin_SelectedIndexChanged);
      // 
      // changeServerHostToolStripMenuItem
      // 
      this.changeServerHostToolStripMenuItem.Name = "changeServerHostToolStripMenuItem";
      this.changeServerHostToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
      this.changeServerHostToolStripMenuItem.Text = "Change server host";
      this.changeServerHostToolStripMenuItem.ToolTipText = "Change the address of the server host";
      this.changeServerHostToolStripMenuItem.Click += new System.EventHandler(this.changeServerHostToolStripMenuItem_Click);
      // 
      // toolStripMenuItemHelp
      // 
      this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
      this.toolStripMenuItemHelp.Size = new System.Drawing.Size(155, 22);
      this.toolStripMenuItemHelp.Text = "Help";
      this.toolStripMenuItemHelp.Click += new System.EventHandler(this.toolStripMenuItemHelp_Click);
      // 
      // toolStripMenuItemQuit
      // 
      this.toolStripMenuItemQuit.Name = "toolStripMenuItemQuit";
      this.toolStripMenuItemQuit.Size = new System.Drawing.Size(155, 22);
      this.toolStripMenuItemQuit.Text = "Quit";
      this.toolStripMenuItemQuit.Click += new System.EventHandler(this.toolStripMenuItemQuit_Click);
      // 
      // labelDisabled
      // 
      this.labelDisabled.BackColor = System.Drawing.Color.Transparent;
      this.labelDisabled.Cursor = System.Windows.Forms.Cursors.Arrow;
      this.labelDisabled.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelDisabled.ForeColor = System.Drawing.Color.Red;
      this.labelDisabled.Location = new System.Drawing.Point(0, 0);
      this.labelDisabled.Name = "labelDisabled";
      this.labelDisabled.Size = new System.Drawing.Size(250, 231);
      this.labelDisabled.TabIndex = 1;
      this.labelDisabled.Text = "Connecting to IR Server ...";
      this.labelDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.labelDisabled.Visible = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(250, 231);
      this.ContextMenuStrip = this.contextMenuStrip;
      this.Controls.Add(this.labelDisabled);
      this.Cursor = System.Windows.Forms.Cursors.Hand;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Virtual Remote";
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.contextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSkin;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemQuit;
    private System.Windows.Forms.ToolStripMenuItem changeServerHostToolStripMenuItem;
    private System.Windows.Forms.Label labelDisabled;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
  }
}

