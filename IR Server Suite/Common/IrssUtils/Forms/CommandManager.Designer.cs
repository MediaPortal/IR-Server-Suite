namespace IrssUtils.Forms
{
    partial class CommandManager
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeViewCommandList = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.labelHeaderCommand = new System.Windows.Forms.Label();
            this.toolStripCommand = new System.Windows.Forms.ToolStrip();
            this.ButtonAddCommand = new System.Windows.Forms.ToolStripDropDownButton();
            this.learnIRCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.macroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCommandEdit = new System.Windows.Forms.ToolStripButton();
            this.buttonCommandCopy = new System.Windows.Forms.ToolStripButton();
            this.buttonCommandRemove = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip.SuspendLayout();
            this.toolStripCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewCommandList
            // 
            this.treeViewCommandList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewCommandList.ContextMenuStrip = this.contextMenuStrip;
            this.treeViewCommandList.HideSelection = false;
            this.treeViewCommandList.Location = new System.Drawing.Point(0, 21);
            this.treeViewCommandList.Name = "treeViewCommandList";
            this.treeViewCommandList.Size = new System.Drawing.Size(241, 251);
            this.treeViewCommandList.TabIndex = 1;
            this.treeViewCommandList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCommandList_AfterSelect);
            this.treeViewCommandList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewCommandList_NodeMouseClick);
            this.treeViewCommandList.DoubleClick += new System.EventHandler(this.buttonCommandEdit_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuAdd,
            this.toolStripMenuEdit,
            this.toolStripMenuCopy,
            this.toolStripMenuRemove});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(145, 92);
            // 
            // toolStripMenuAdd
            // 
            this.toolStripMenuAdd.Image = global::IrssUtils.Properties.Resources.Plus;
            this.toolStripMenuAdd.Name = "toolStripMenuAdd";
            this.toolStripMenuAdd.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuAdd.Text = "&New";
            this.toolStripMenuAdd.Click += new System.EventHandler(this.toolStripMenuAdd_Click);
            // 
            // toolStripMenuEdit
            // 
            this.toolStripMenuEdit.Image = global::IrssUtils.Properties.Resources.Edit;
            this.toolStripMenuEdit.Name = "toolStripMenuEdit";
            this.toolStripMenuEdit.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.toolStripMenuEdit.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuEdit.Text = "&Edit";
            this.toolStripMenuEdit.Click += new System.EventHandler(this.toolStripMenuEdit_Click);
            // 
            // toolStripMenuCopy
            // 
            this.toolStripMenuCopy.Image = global::IrssUtils.Properties.Resources.CopyDocument;
            this.toolStripMenuCopy.Name = "toolStripMenuCopy";
            this.toolStripMenuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuCopy.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuCopy.Text = "&Copy";
            this.toolStripMenuCopy.Click += new System.EventHandler(this.toolStripMenuCopy_Click);
            // 
            // toolStripMenuRemove
            // 
            this.toolStripMenuRemove.Image = global::IrssUtils.Properties.Resources.Delete;
            this.toolStripMenuRemove.Name = "toolStripMenuRemove";
            this.toolStripMenuRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuRemove.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuRemove.Text = "&Delete";
            this.toolStripMenuRemove.Click += new System.EventHandler(this.toolStripMenuRemove_Click);
            // 
            // labelHeaderCommand
            // 
            this.labelHeaderCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHeaderCommand.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelHeaderCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeaderCommand.Location = new System.Drawing.Point(0, 0);
            this.labelHeaderCommand.Name = "labelHeaderCommand";
            this.labelHeaderCommand.Size = new System.Drawing.Size(241, 18);
            this.labelHeaderCommand.TabIndex = 0;
            this.labelHeaderCommand.Text = "Command Manager";
            this.labelHeaderCommand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripCommand
            // 
            this.toolStripCommand.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripCommand.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripCommand.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonAddCommand,
            this.buttonCommandEdit,
            this.buttonCommandCopy,
            this.buttonCommandRemove});
            this.toolStripCommand.Location = new System.Drawing.Point(0, 275);
            this.toolStripCommand.Name = "toolStripCommand";
            this.toolStripCommand.Size = new System.Drawing.Size(241, 25);
            this.toolStripCommand.TabIndex = 3;
            this.toolStripCommand.Text = "toolStripActions";
            // 
            // ButtonAddCommand
            // 
            this.ButtonAddCommand.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.learnIRCommandToolStripMenuItem,
            this.macroToolStripMenuItem});
            this.ButtonAddCommand.Image = global::IrssUtils.Properties.Resources.Plus;
            this.ButtonAddCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAddCommand.Name = "ButtonAddCommand";
            this.ButtonAddCommand.Size = new System.Drawing.Size(60, 22);
            this.ButtonAddCommand.Text = "&New";
            this.ButtonAddCommand.ToolTipText = "Add a new command (F8)";
            // 
            // learnIRCommandToolStripMenuItem
            // 
            this.learnIRCommandToolStripMenuItem.Name = "learnIRCommandToolStripMenuItem";
            this.learnIRCommandToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.learnIRCommandToolStripMenuItem.Text = "&Blast command...";
            this.learnIRCommandToolStripMenuItem.Click += new System.EventHandler(this.learnIRCommandToolStripMenuItem_Click);
            // 
            // macroToolStripMenuItem
            // 
            this.macroToolStripMenuItem.Name = "macroToolStripMenuItem";
            this.macroToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.macroToolStripMenuItem.Text = "&Macro...";
            this.macroToolStripMenuItem.Click += new System.EventHandler(this.macroToolStripMenuItem_Click);
            // 
            // buttonCommandEdit
            // 
            this.buttonCommandEdit.Enabled = false;
            this.buttonCommandEdit.Image = global::IrssUtils.Properties.Resources.Edit;
            this.buttonCommandEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCommandEdit.Name = "buttonCommandEdit";
            this.buttonCommandEdit.Size = new System.Drawing.Size(47, 22);
            this.buttonCommandEdit.Text = "&Edit";
            this.buttonCommandEdit.ToolTipText = "Edit the selected command (F7)";
            this.buttonCommandEdit.Click += new System.EventHandler(this.buttonCommandEdit_Click);
            // 
            // buttonCommandCopy
            // 
            this.buttonCommandCopy.Enabled = false;
            this.buttonCommandCopy.Image = global::IrssUtils.Properties.Resources.CopyDocument;
            this.buttonCommandCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCommandCopy.Name = "buttonCommandCopy";
            this.buttonCommandCopy.Size = new System.Drawing.Size(55, 22);
            this.buttonCommandCopy.Text = "&Copy";
            this.buttonCommandCopy.ToolTipText = "Copy the selected command (Ctrl + C)";
            this.buttonCommandCopy.Click += new System.EventHandler(this.buttonCommandCopy_Click);
            // 
            // buttonCommandRemove
            // 
            this.buttonCommandRemove.Enabled = false;
            this.buttonCommandRemove.Image = global::IrssUtils.Properties.Resources.Delete;
            this.buttonCommandRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCommandRemove.Name = "buttonCommandRemove";
            this.buttonCommandRemove.Size = new System.Drawing.Size(60, 22);
            this.buttonCommandRemove.Text = "&Delete";
            this.buttonCommandRemove.ToolTipText = "Remove the selected command (Delete)";
            this.buttonCommandRemove.Click += new System.EventHandler(this.buttonCommandRemove_Click);
            // 
            // CommandManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripCommand);
            this.Controls.Add(this.labelHeaderCommand);
            this.Controls.Add(this.treeViewCommandList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CommandManager";
            this.Size = new System.Drawing.Size(241, 300);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.CommandManager_HelpRequested);
            this.contextMenuStrip.ResumeLayout(false);
            this.toolStripCommand.ResumeLayout(false);
            this.toolStripCommand.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TreeView treeViewCommandList;
        private System.Windows.Forms.Label labelHeaderCommand;
        private System.Windows.Forms.ToolStrip toolStripCommand;
        private System.Windows.Forms.ToolStripButton buttonCommandCopy;
        private System.Windows.Forms.ToolStripButton buttonCommandRemove;
        private System.Windows.Forms.ToolStripButton buttonCommandEdit;
        private System.Windows.Forms.ToolStripDropDownButton ButtonAddCommand;
        private System.Windows.Forms.ToolStripMenuItem learnIRCommandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem macroToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuRemove;
    }
}
