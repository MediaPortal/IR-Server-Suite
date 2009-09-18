namespace MacroScope
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
      this.statusStrip = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.stepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.resetDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.endDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.richTextBoxMacro = new System.Windows.Forms.RichTextBox();
      this.treeViewCommandList = new System.Windows.Forms.TreeView();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageCommands = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.listViewVariables = new System.Windows.Forms.ListView();
      this.columnHeaderVarName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderVarValue = new System.Windows.Forms.ColumnHeader();
      this.contextMenuStripVariables = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.statusStrip.SuspendLayout();
      this.menuStrip.SuspendLayout();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.tabPageCommands.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      // 
      // statusStrip
      // 
      this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
      this.statusStrip.Location = new System.Drawing.Point(0, 351);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new System.Drawing.Size(592, 22);
      this.statusStrip.TabIndex = 2;
      this.statusStrip.Text = "statusStrip";
      // 
      // toolStripStatusLabel
      // 
      this.toolStripStatusLabel.Name = "toolStripStatusLabel";
      this.toolStripStatusLabel.Size = new System.Drawing.Size(140, 17);
      this.toolStripStatusLabel.Text = "Welcome to MacroScope";
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(592, 24);
      this.menuStrip.TabIndex = 0;
      this.menuStrip.Text = "menuStrip";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
      this.newToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
      this.newToolStripMenuItem.Text = "&New";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this.openToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
      this.openToolStripMenuItem.Text = "&Open";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
      this.saveToolStripMenuItem.Text = "&Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                  | System.Windows.Forms.Keys.S)));
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
      this.saveAsToolStripMenuItem.Text = "Save &As ...";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
      this.closeToolStripMenuItem.Text = "&Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // debugToolStripMenuItem
      // 
      this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stepToolStripMenuItem,
            this.resetDebugToolStripMenuItem,
            this.endDebugToolStripMenuItem});
      this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
      this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
      this.debugToolStripMenuItem.Text = "Debug";
      // 
      // stepToolStripMenuItem
      // 
      this.stepToolStripMenuItem.Name = "stepToolStripMenuItem";
      this.stepToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.stepToolStripMenuItem.Text = "&Step Over";
      this.stepToolStripMenuItem.Click += new System.EventHandler(this.stepDebugToolStripMenuItem_Click);
      // 
      // resetDebugToolStripMenuItem
      // 
      this.resetDebugToolStripMenuItem.Name = "resetDebugToolStripMenuItem";
      this.resetDebugToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.resetDebugToolStripMenuItem.Text = "&Reset";
      this.resetDebugToolStripMenuItem.Click += new System.EventHandler(this.resetDebugToolStripMenuItem_Click);
      // 
      // endDebugToolStripMenuItem
      // 
      this.endDebugToolStripMenuItem.Name = "endDebugToolStripMenuItem";
      this.endDebugToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.endDebugToolStripMenuItem.Text = "&End";
      this.endDebugToolStripMenuItem.Click += new System.EventHandler(this.endDebugToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // contentsToolStripMenuItem
      // 
      this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
      this.contentsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.contentsToolStripMenuItem.Text = "&Contents";
      this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.aboutToolStripMenuItem.Text = "&About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // richTextBoxMacro
      // 
      this.richTextBoxMacro.AcceptsTab = true;
      this.richTextBoxMacro.AutoWordSelection = true;
      this.richTextBoxMacro.Dock = System.Windows.Forms.DockStyle.Fill;
      this.richTextBoxMacro.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBoxMacro.HideSelection = false;
      this.richTextBoxMacro.Location = new System.Drawing.Point(0, 0);
      this.richTextBoxMacro.Name = "richTextBoxMacro";
      this.richTextBoxMacro.ShowSelectionMargin = true;
      this.richTextBoxMacro.Size = new System.Drawing.Size(400, 327);
      this.richTextBoxMacro.TabIndex = 0;
      this.richTextBoxMacro.Text = "";
      this.richTextBoxMacro.WordWrap = false;
      // 
      // treeViewCommandList
      // 
      this.treeViewCommandList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewCommandList.Location = new System.Drawing.Point(3, 3);
      this.treeViewCommandList.Name = "treeViewCommandList";
      this.treeViewCommandList.Size = new System.Drawing.Size(174, 295);
      this.treeViewCommandList.TabIndex = 0;
      this.treeViewCommandList.DoubleClick += new System.EventHandler(this.treeViewCommandList_DoubleClick);
      // 
      // splitContainer
      // 
      this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer.Location = new System.Drawing.Point(0, 24);
      this.splitContainer.Name = "splitContainer";
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.Controls.Add(this.richTextBoxMacro);
      this.splitContainer.Panel1MinSize = 128;
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.tabControl);
      this.splitContainer.Panel2MinSize = 128;
      this.splitContainer.Size = new System.Drawing.Size(592, 327);
      this.splitContainer.SplitterDistance = 400;
      this.splitContainer.TabIndex = 1;
      // 
      // tabControl
      // 
      this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
      this.tabControl.Controls.Add(this.tabPageCommands);
      this.tabControl.Controls.Add(this.tabPage2);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(188, 327);
      this.tabControl.TabIndex = 1;
      // 
      // tabPageCommands
      // 
      this.tabPageCommands.Controls.Add(this.treeViewCommandList);
      this.tabPageCommands.Location = new System.Drawing.Point(4, 4);
      this.tabPageCommands.Name = "tabPageCommands";
      this.tabPageCommands.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageCommands.Size = new System.Drawing.Size(180, 301);
      this.tabPageCommands.TabIndex = 0;
      this.tabPageCommands.Text = "Commands";
      this.tabPageCommands.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.listViewVariables);
      this.tabPage2.Location = new System.Drawing.Point(4, 4);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(180, 301);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Variables";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // listViewVariables
      // 
      this.listViewVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderVarName,
            this.columnHeaderVarValue});
      this.listViewVariables.ContextMenuStrip = this.contextMenuStripVariables;
      this.listViewVariables.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewVariables.GridLines = true;
      this.listViewVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewVariables.Location = new System.Drawing.Point(3, 3);
      this.listViewVariables.Name = "listViewVariables";
      this.listViewVariables.Size = new System.Drawing.Size(174, 295);
      this.listViewVariables.TabIndex = 0;
      this.listViewVariables.UseCompatibleStateImageBehavior = false;
      this.listViewVariables.View = System.Windows.Forms.View.Details;
      // 
      // columnHeaderVarName
      // 
      this.columnHeaderVarName.Text = "Name";
      this.columnHeaderVarName.Width = 64;
      // 
      // columnHeaderVarValue
      // 
      this.columnHeaderVarValue.Text = "Value";
      this.columnHeaderVarValue.Width = 85;
      // 
      // contextMenuStripVariables
      // 
      this.contextMenuStripVariables.Name = "contextMenuStripVariables";
      this.contextMenuStripVariables.Size = new System.Drawing.Size(61, 4);
      // 
      // openFileDialog
      // 
      this.openFileDialog.DefaultExt = "Macro";
      this.openFileDialog.Filter = "Macro Files|*.Macro|All Files|*.*";
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.DefaultExt = "Macro";
      this.saveFileDialog.Filter = "Macro Files|*.Macro|All Files|*.*";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(248, 6);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(592, 373);
      this.Controls.Add(this.splitContainer);
      this.Controls.Add(this.statusStrip);
      this.Controls.Add(this.menuStrip);
      this.MainMenuStrip = this.menuStrip;
      this.Name = "MainForm";
      this.Text = "MacroScope";
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.ResumeLayout(false);
      this.tabControl.ResumeLayout(false);
      this.tabPageCommands.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.RichTextBox richTextBoxMacro;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem stepToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem resetDebugToolStripMenuItem;
    private System.Windows.Forms.TreeView treeViewCommandList;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageCommands;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.ListView listViewVariables;
    private System.Windows.Forms.ColumnHeader columnHeaderVarName;
    private System.Windows.Forms.ColumnHeader columnHeaderVarValue;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripVariables;
    private System.Windows.Forms.ToolStripMenuItem endDebugToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
  }
}

