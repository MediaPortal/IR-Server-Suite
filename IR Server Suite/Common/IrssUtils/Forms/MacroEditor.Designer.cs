namespace IrssUtils.Forms
{
  public partial class MacroEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MacroEditor));
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBoxCommandSequence = new System.Windows.Forms.GroupBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerMacro = new System.Windows.Forms.SplitContainer();
            this.panelActions = new System.Windows.Forms.Panel();
            this.labelHeaderActions = new System.Windows.Forms.Label();
            this.listBoxMacro = new System.Windows.Forms.ListBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuTest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripActions = new System.Windows.Forms.ToolStrip();
            this.buttonActionsTest = new System.Windows.Forms.ToolStripButton();
            this.buttonActionsEdit = new System.Windows.Forms.ToolStripButton();
            this.buttonActionsCopy = new System.Windows.Forms.ToolStripButton();
            this.buttonActionsRemove = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonAddCommand = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTipMacro = new System.Windows.Forms.ToolTip(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonShortcut = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.labelInvalid = new System.Windows.Forms.Label();
            this.groupBoxCommandSequence.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerMacro.Panel1.SuspendLayout();
            this.splitContainerMacro.Panel2.SuspendLayout();
            this.splitContainerMacro.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.toolStripActions.SuspendLayout();
            this.tableLayoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(8, 8);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(56, 20);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(64, 8);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(472, 20);
            this.textBoxName.TabIndex = 1;
            this.toolTipMacro.SetToolTip(this.textBoxName, "Set/change the name for this macro");
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // groupBoxCommandSequence
            // 
            this.groupBoxCommandSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCommandSequence.Controls.Add(this.splitContainerMain);
            this.groupBoxCommandSequence.Location = new System.Drawing.Point(8, 40);
            this.groupBoxCommandSequence.Name = "groupBoxCommandSequence";
            this.groupBoxCommandSequence.Size = new System.Drawing.Size(528, 329);
            this.groupBoxCommandSequence.TabIndex = 3;
            this.groupBoxCommandSequence.TabStop = false;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMain.Location = new System.Drawing.Point(3, 16);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerMacro);
            this.splitContainerMain.Panel1MinSize = 240;
            // 
            // splitContainerMain.Panel2
            // 
            this.toolTipMacro.SetToolTip(this.splitContainerMain.Panel2, "Command selector");
            this.splitContainerMain.Size = new System.Drawing.Size(522, 310);
            this.splitContainerMain.SplitterDistance = 271;
            this.splitContainerMain.TabIndex = 4;
            // 
            // splitContainerMacro
            // 
            this.splitContainerMacro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMacro.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMacro.IsSplitterFixed = true;
            this.splitContainerMacro.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMacro.MinimumSize = new System.Drawing.Size(100, 0);
            this.splitContainerMacro.Name = "splitContainerMacro";
            // 
            // splitContainerMacro.Panel1
            // 
            this.splitContainerMacro.Panel1.Controls.Add(this.panelActions);
            // 
            // splitContainerMacro.Panel2
            // 
            this.splitContainerMacro.Panel2.Controls.Add(this.tableLayoutButtons);
            this.splitContainerMacro.Size = new System.Drawing.Size(271, 310);
            this.splitContainerMacro.SplitterDistance = 245;
            this.splitContainerMacro.SplitterWidth = 1;
            this.splitContainerMacro.TabIndex = 0;
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.labelHeaderActions);
            this.panelActions.Controls.Add(this.listBoxMacro);
            this.panelActions.Controls.Add(this.toolStripActions);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActions.Location = new System.Drawing.Point(0, 0);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(245, 310);
            this.panelActions.TabIndex = 3;
            // 
            // labelHeaderActions
            // 
            this.labelHeaderActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHeaderActions.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelHeaderActions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeaderActions.Location = new System.Drawing.Point(0, 0);
            this.labelHeaderActions.Name = "labelHeaderActions";
            this.labelHeaderActions.Size = new System.Drawing.Size(245, 18);
            this.labelHeaderActions.TabIndex = 1;
            this.labelHeaderActions.Text = "Actions";
            this.labelHeaderActions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBoxMacro
            // 
            this.listBoxMacro.AllowDrop = true;
            this.listBoxMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMacro.ContextMenuStrip = this.contextMenuStrip;
            this.listBoxMacro.FormattingEnabled = true;
            this.listBoxMacro.IntegralHeight = false;
            this.listBoxMacro.Location = new System.Drawing.Point(1, 21);
            this.listBoxMacro.Name = "listBoxMacro";
            this.listBoxMacro.Size = new System.Drawing.Size(241, 261);
            this.listBoxMacro.TabIndex = 0;
            this.toolTipMacro.SetToolTip(this.listBoxMacro, "Sequence of actions in the macro");
            this.listBoxMacro.SelectedIndexChanged += new System.EventHandler(this.listBoxMacro_SelectedIndexChanged);
            this.listBoxMacro.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxMacro_DragDrop);
            this.listBoxMacro.DragOver += new System.Windows.Forms.DragEventHandler(this.listBoxMacro_DragOver);
            this.listBoxMacro.DoubleClick += new System.EventHandler(this.buttonActionsEdit_Click);
            this.listBoxMacro.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxMacro_MouseDown);
            this.listBoxMacro.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBoxMacro_MouseMove);
            this.listBoxMacro.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBoxMacro_MouseUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuTest,
            this.toolStripMenuEdit,
            this.toolStripMenuCopy,
            this.toolStripMenuRemove});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(145, 92);
            // 
            // toolStripMenuTest
            // 
            this.toolStripMenuTest.Enabled = false;
            this.toolStripMenuTest.Image = global::IrssUtils.Properties.Resources.Run;
            this.toolStripMenuTest.Name = "toolStripMenuTest";
            this.toolStripMenuTest.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.toolStripMenuTest.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuTest.Text = "&Test";
            this.toolStripMenuTest.Click += new System.EventHandler(this.toolStripMenuTest_Click);
            // 
            // toolStripMenuEdit
            // 
            this.toolStripMenuEdit.Enabled = false;
            this.toolStripMenuEdit.Image = global::IrssUtils.Properties.Resources.Edit;
            this.toolStripMenuEdit.Name = "toolStripMenuEdit";
            this.toolStripMenuEdit.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.toolStripMenuEdit.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuEdit.Text = "&Edit";
            this.toolStripMenuEdit.Click += new System.EventHandler(this.toolStripMenuEdit_Click);
            // 
            // toolStripMenuCopy
            // 
            this.toolStripMenuCopy.Enabled = false;
            this.toolStripMenuCopy.Image = global::IrssUtils.Properties.Resources.CopyDocument;
            this.toolStripMenuCopy.Name = "toolStripMenuCopy";
            this.toolStripMenuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuCopy.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuCopy.Text = "&Copy";
            this.toolStripMenuCopy.Click += new System.EventHandler(this.toolStripMenuCopy_Click);
            // 
            // toolStripMenuRemove
            // 
            this.toolStripMenuRemove.Enabled = false;
            this.toolStripMenuRemove.Image = global::IrssUtils.Properties.Resources.Delete;
            this.toolStripMenuRemove.Name = "toolStripMenuRemove";
            this.toolStripMenuRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuRemove.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuRemove.Text = "&Delete";
            this.toolStripMenuRemove.Click += new System.EventHandler(this.toolStripMenuRemove_Click);
            // 
            // toolStripActions
            // 
            this.toolStripActions.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripActions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonActionsTest,
            this.buttonActionsEdit,
            this.buttonActionsCopy,
            this.buttonActionsRemove});
            this.toolStripActions.Location = new System.Drawing.Point(0, 285);
            this.toolStripActions.Name = "toolStripActions";
            this.toolStripActions.Size = new System.Drawing.Size(245, 25);
            this.toolStripActions.TabIndex = 2;
            this.toolStripActions.Text = "toolStripActions";
            // 
            // buttonActionsTest
            // 
            this.buttonActionsTest.Enabled = false;
            this.buttonActionsTest.Image = global::IrssUtils.Properties.Resources.Run;
            this.buttonActionsTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonActionsTest.Name = "buttonActionsTest";
            this.buttonActionsTest.Size = new System.Drawing.Size(49, 22);
            this.buttonActionsTest.Text = "&Test";
            this.buttonActionsTest.ToolTipText = "Run the selected action (F6)";
            this.buttonActionsTest.Click += new System.EventHandler(this.buttonTestCommand_Click);
            // 
            // buttonActionsEdit
            // 
            this.buttonActionsEdit.Enabled = false;
            this.buttonActionsEdit.Image = global::IrssUtils.Properties.Resources.Edit;
            this.buttonActionsEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonActionsEdit.Name = "buttonActionsEdit";
            this.buttonActionsEdit.Size = new System.Drawing.Size(47, 22);
            this.buttonActionsEdit.Text = "&Edit";
            this.buttonActionsEdit.ToolTipText = "Edit the selected action (F7)";
            this.buttonActionsEdit.Click += new System.EventHandler(this.buttonActionsEdit_Click);
            // 
            // buttonActionsCopy
            // 
            this.buttonActionsCopy.Enabled = false;
            this.buttonActionsCopy.Image = global::IrssUtils.Properties.Resources.CopyDocument;
            this.buttonActionsCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonActionsCopy.Name = "buttonActionsCopy";
            this.buttonActionsCopy.Size = new System.Drawing.Size(55, 22);
            this.buttonActionsCopy.Text = "&Copy";
            this.buttonActionsCopy.ToolTipText = "Copy the selected action (Ctrl + C)";
            this.buttonActionsCopy.Click += new System.EventHandler(this.buttonCopyCommand_Click);
            // 
            // buttonActionsRemove
            // 
            this.buttonActionsRemove.Enabled = false;
            this.buttonActionsRemove.Image = global::IrssUtils.Properties.Resources.Delete;
            this.buttonActionsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonActionsRemove.Name = "buttonActionsRemove";
            this.buttonActionsRemove.Size = new System.Drawing.Size(60, 22);
            this.buttonActionsRemove.Text = "&Delete";
            this.buttonActionsRemove.ToolTipText = "Remove the selected action (Delete)";
            this.buttonActionsRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // tableLayoutButtons
            // 
            this.tableLayoutButtons.ColumnCount = 1;
            this.tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutButtons.Controls.Add(this.buttonMoveDown, 0, 3);
            this.tableLayoutButtons.Controls.Add(this.buttonAddCommand, 0, 2);
            this.tableLayoutButtons.Controls.Add(this.buttonMoveUp, 0, 1);
            this.tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutButtons.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutButtons.Name = "tableLayoutButtons";
            this.tableLayoutButtons.RowCount = 5;
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutButtons.Size = new System.Drawing.Size(25, 310);
            this.tableLayoutButtons.TabIndex = 0;
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonMoveDown.Enabled = false;
            this.buttonMoveDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonMoveDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.buttonMoveDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.buttonMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonMoveDown.Image")));
            this.buttonMoveDown.Location = new System.Drawing.Point(2, 196);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(2, 10, 0, 10);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(23, 25);
            this.buttonMoveDown.TabIndex = 3;
            this.buttonMoveDown.Text = " ";
            this.toolTipMacro.SetToolTip(this.buttonMoveDown, "move the command down (Alt + Down)");
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonAddCommand
            // 
            this.buttonAddCommand.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonAddCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAddCommand.Enabled = false;
            this.buttonAddCommand.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonAddCommand.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.buttonAddCommand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.buttonAddCommand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddCommand.Image = global::IrssUtils.Properties.Resources.ScrollLeft;
            this.buttonAddCommand.Location = new System.Drawing.Point(2, 142);
            this.buttonAddCommand.Margin = new System.Windows.Forms.Padding(2, 10, 0, 10);
            this.buttonAddCommand.Name = "buttonAddCommand";
            this.buttonAddCommand.Size = new System.Drawing.Size(23, 25);
            this.buttonAddCommand.TabIndex = 2;
            this.buttonAddCommand.Text = " ";
            this.toolTipMacro.SetToolTip(this.buttonAddCommand, "Insert the selected command (right pan) into the macro (Alt + Left)");
            this.buttonAddCommand.UseVisualStyleBackColor = true;
            this.buttonAddCommand.Click += new System.EventHandler(this.buttonAddCommand_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonMoveUp.Enabled = false;
            this.buttonMoveUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonMoveUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Highlight;
            this.buttonMoveUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.buttonMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonMoveUp.Image")));
            this.buttonMoveUp.Location = new System.Drawing.Point(2, 89);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(2, 10, 0, 10);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(23, 25);
            this.buttonMoveUp.TabIndex = 1;
            this.buttonMoveUp.Text = " ";
            this.toolTipMacro.SetToolTip(this.buttonMoveUp, "move the command up (Alt + Up)");
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(480, 375);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 24);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.toolTipMacro.SetToolTip(this.buttonCancel, "Do not insert this macro (Esc)");
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Image = global::IrssUtils.Properties.Resources.ScrollLeft;
            this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOK.Location = new System.Drawing.Point(413, 375);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(59, 24);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "Insert";
            this.buttonOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipMacro.SetToolTip(this.buttonOK, "Insert this macro in the calling list and close editor (Enter)");
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonShortcut
            // 
            this.buttonShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonShortcut.Image = global::IrssUtils.Properties.Resources.Shortcut1;
            this.buttonShortcut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonShortcut.Location = new System.Drawing.Point(351, 375);
            this.buttonShortcut.Name = "buttonShortcut";
            this.buttonShortcut.Size = new System.Drawing.Size(56, 24);
            this.buttonShortcut.TabIndex = 5;
            this.buttonShortcut.Text = "Icon";
            this.buttonShortcut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipMacro.SetToolTip(this.buttonShortcut, "Create a shortcut icon on the desktop for running this macro");
            this.buttonShortcut.UseVisualStyleBackColor = true;
            this.buttonShortcut.Click += new System.EventHandler(this.buttonShortcut_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonTest.Image = global::IrssUtils.Properties.Resources.Run;
            this.buttonTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTest.Location = new System.Drawing.Point(8, 375);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(56, 24);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "Run";
            this.buttonTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipMacro.SetToolTip(this.buttonTest, "Run the macro (F5)");
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // labelInvalid
            // 
            this.labelInvalid.AutoSize = true;
            this.labelInvalid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelInvalid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInvalid.Location = new System.Drawing.Point(67, 28);
            this.labelInvalid.Name = "labelInvalid";
            this.labelInvalid.Size = new System.Drawing.Size(357, 13);
            this.labelInvalid.TabIndex = 7;
            this.labelInvalid.Text = "a macro-name can\'t contain any of the following characters: \\ / : * ? \" < > |";
            this.labelInvalid.Visible = false;
            // 
            // MacroEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(544, 408);
            this.Controls.Add(this.labelInvalid);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonShortcut);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBoxCommandSequence);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 356);
            this.Name = "MacroEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Macro Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MacroEditor_FormClosed);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.MacroEditor_HelpRequested);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MacroEditor_KeyDown);
            this.groupBoxCommandSequence.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerMacro.Panel1.ResumeLayout(false);
            this.splitContainerMacro.Panel2.ResumeLayout(false);
            this.splitContainerMacro.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.toolStripActions.ResumeLayout(false);
            this.toolStripActions.PerformLayout();
            this.tableLayoutButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.GroupBox groupBoxCommandSequence;
    private System.Windows.Forms.Button buttonMoveDown;
    private System.Windows.Forms.Button buttonMoveUp;
    private System.Windows.Forms.ListBox listBoxMacro;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.SplitContainer splitContainerMacro;
    private System.Windows.Forms.TableLayoutPanel tableLayoutButtons;
    private System.Windows.Forms.Button buttonAddCommand;
    private System.Windows.Forms.ToolTip toolTipMacro;
    private System.Windows.Forms.Label labelInvalid;
    private System.Windows.Forms.ToolStrip toolStripActions;
    private System.Windows.Forms.Label labelHeaderActions;
    private System.Windows.Forms.ToolStripButton buttonActionsCopy;
    private System.Windows.Forms.ToolStripButton buttonActionsRemove;
    private System.Windows.Forms.ToolStripButton buttonActionsTest;
    private System.Windows.Forms.Panel panelActions;
    private System.Windows.Forms.ToolStripButton buttonActionsEdit;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuTest;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuEdit;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuCopy;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuRemove;
    private System.Windows.Forms.Button buttonShortcut;
  }
}