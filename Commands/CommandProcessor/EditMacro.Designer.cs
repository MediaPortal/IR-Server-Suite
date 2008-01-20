namespace Commands
{

  partial class EditMacro
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
      this.textBoxName = new System.Windows.Forms.TextBox();
      this.groupBoxCommandSequence = new System.Windows.Forms.GroupBox();
      this.listViewMacro = new System.Windows.Forms.ListView();
      this.columnHeader = new System.Windows.Forms.ColumnHeader();
      this.groupBoxCommands = new System.Windows.Forms.GroupBox();
      this.treeViewCommandList = new System.Windows.Forms.TreeView();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxMacroName = new System.Windows.Forms.GroupBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.toolStripCommandSequence = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonTop = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonUp = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteAll = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDown = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonBottom = new System.Windows.Forms.ToolStripButton();
      this.groupBoxCommandSequence.SuspendLayout();
      this.groupBoxCommands.SuspendLayout();
      this.groupBoxMacroName.SuspendLayout();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.toolStripCommandSequence.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBoxName
      // 
      this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxName.Location = new System.Drawing.Point(8, 16);
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new System.Drawing.Size(488, 22);
      this.textBoxName.TabIndex = 0;
      this.toolTips.SetToolTip(this.textBoxName, "Provide a name for this macro (must be a valid windows file name)");
      // 
      // groupBoxCommandSequence
      // 
      this.groupBoxCommandSequence.Controls.Add(this.listViewMacro);
      this.groupBoxCommandSequence.Controls.Add(this.toolStripCommandSequence);
      this.groupBoxCommandSequence.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxCommandSequence.Location = new System.Drawing.Point(0, 0);
      this.groupBoxCommandSequence.Name = "groupBoxCommandSequence";
      this.groupBoxCommandSequence.Size = new System.Drawing.Size(318, 232);
      this.groupBoxCommandSequence.TabIndex = 0;
      this.groupBoxCommandSequence.TabStop = false;
      this.groupBoxCommandSequence.Text = "Command Sequence";
      // 
      // listViewMacro
      // 
      this.listViewMacro.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
      this.listViewMacro.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewMacro.FullRowSelect = true;
      this.listViewMacro.GridLines = true;
      this.listViewMacro.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewMacro.HideSelection = false;
      this.listViewMacro.Location = new System.Drawing.Point(3, 16);
      this.listViewMacro.MultiSelect = false;
      this.listViewMacro.Name = "listViewMacro";
      this.listViewMacro.ShowGroups = false;
      this.listViewMacro.Size = new System.Drawing.Size(280, 213);
      this.listViewMacro.TabIndex = 0;
      this.listViewMacro.UseCompatibleStateImageBehavior = false;
      this.listViewMacro.View = System.Windows.Forms.View.Details;
      this.listViewMacro.DoubleClick += new System.EventHandler(this.listViewMacro_DoubleClick);
      // 
      // columnHeader
      // 
      this.columnHeader.Text = "Command Sequence";
      this.columnHeader.Width = 268;
      // 
      // groupBoxCommands
      // 
      this.groupBoxCommands.Controls.Add(this.treeViewCommandList);
      this.groupBoxCommands.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxCommands.Location = new System.Drawing.Point(0, 0);
      this.groupBoxCommands.Name = "groupBoxCommands";
      this.groupBoxCommands.Size = new System.Drawing.Size(180, 232);
      this.groupBoxCommands.TabIndex = 0;
      this.groupBoxCommands.TabStop = false;
      this.groupBoxCommands.Text = "Commands";
      // 
      // treeViewCommandList
      // 
      this.treeViewCommandList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewCommandList.Location = new System.Drawing.Point(3, 16);
      this.treeViewCommandList.Name = "treeViewCommandList";
      this.treeViewCommandList.Size = new System.Drawing.Size(174, 213);
      this.treeViewCommandList.TabIndex = 0;
      this.treeViewCommandList.DoubleClick += new System.EventHandler(this.treeViewCommandList_DoubleClick);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(456, 304);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(8, 304);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(56, 24);
      this.buttonTest.TabIndex = 2;
      this.buttonTest.Text = "Test";
      this.toolTips.SetToolTip(this.buttonTest, "Test this macro");
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(392, 304);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // groupBoxMacroName
      // 
      this.groupBoxMacroName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMacroName.Controls.Add(this.textBoxName);
      this.groupBoxMacroName.Location = new System.Drawing.Point(8, 8);
      this.groupBoxMacroName.Name = "groupBoxMacroName";
      this.groupBoxMacroName.Size = new System.Drawing.Size(504, 48);
      this.groupBoxMacroName.TabIndex = 0;
      this.groupBoxMacroName.TabStop = false;
      this.groupBoxMacroName.Text = "Macro Name";
      // 
      // splitContainer
      // 
      this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer.Location = new System.Drawing.Point(8, 64);
      this.splitContainer.Name = "splitContainer";
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.Controls.Add(this.groupBoxCommands);
      this.splitContainer.Panel1MinSize = 128;
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.groupBoxCommandSequence);
      this.splitContainer.Panel2MinSize = 128;
      this.splitContainer.Size = new System.Drawing.Size(504, 232);
      this.splitContainer.SplitterDistance = 180;
      this.splitContainer.SplitterWidth = 6;
      this.splitContainer.TabIndex = 1;
      // 
      // toolStripCommandSequence
      // 
      this.toolStripCommandSequence.Dock = System.Windows.Forms.DockStyle.Right;
      this.toolStripCommandSequence.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStripCommandSequence.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTop,
            this.toolStripButtonUp,
            this.toolStripSeparator1,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonDeleteAll,
            this.toolStripSeparator2,
            this.toolStripButtonDown,
            this.toolStripButtonBottom});
      this.toolStripCommandSequence.Location = new System.Drawing.Point(283, 16);
      this.toolStripCommandSequence.Name = "toolStripCommandSequence";
      this.toolStripCommandSequence.Size = new System.Drawing.Size(32, 213);
      this.toolStripCommandSequence.TabIndex = 1;
      this.toolStripCommandSequence.Text = "Command Sequence";
      // 
      // toolStripButtonTop
      // 
      this.toolStripButtonTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonTop.Image = global::Commands.Properties.Resources.MoveTop;
      this.toolStripButtonTop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonTop.Name = "toolStripButtonTop";
      this.toolStripButtonTop.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonTop.Text = "Move to Top";
      this.toolStripButtonTop.Click += new System.EventHandler(this.toolStripButtonTop_Click);
      // 
      // toolStripButtonUp
      // 
      this.toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonUp.Image = global::Commands.Properties.Resources.MoveUp;
      this.toolStripButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonUp.Name = "toolStripButtonUp";
      this.toolStripButtonUp.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonUp.Text = "Move Up";
      this.toolStripButtonUp.Click += new System.EventHandler(this.toolStripButtonUp_Click);
      // 
      // toolStripButtonEdit
      // 
      this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonEdit.Image = global::Commands.Properties.Resources.Edit;
      this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEdit.Name = "toolStripButtonEdit";
      this.toolStripButtonEdit.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonEdit.Text = "Edit";
      this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
      // 
      // toolStripButtonDelete
      // 
      this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonDelete.Image = global::Commands.Properties.Resources.Delete;
      this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDelete.Name = "toolStripButtonDelete";
      this.toolStripButtonDelete.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonDelete.Text = "Delete";
      this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
      // 
      // toolStripButtonDeleteAll
      // 
      this.toolStripButtonDeleteAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonDeleteAll.Image = global::Commands.Properties.Resources.DeleteAll;
      this.toolStripButtonDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteAll.Name = "toolStripButtonDeleteAll";
      this.toolStripButtonDeleteAll.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonDeleteAll.Text = "Delete All";
      this.toolStripButtonDeleteAll.Click += new System.EventHandler(this.toolStripButtonDeleteAll_Click);
      // 
      // toolStripButtonDown
      // 
      this.toolStripButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonDown.Image = global::Commands.Properties.Resources.MoveDown;
      this.toolStripButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDown.Name = "toolStripButtonDown";
      this.toolStripButtonDown.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonDown.Text = "Move Down";
      this.toolStripButtonDown.Click += new System.EventHandler(this.toolStripButtonDown_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(29, 6);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(29, 6);
      // 
      // toolStripButtonBottom
      // 
      this.toolStripButtonBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonBottom.Image = global::Commands.Properties.Resources.MoveBottom;
      this.toolStripButtonBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonBottom.Name = "toolStripButtonBottom";
      this.toolStripButtonBottom.Size = new System.Drawing.Size(29, 20);
      this.toolStripButtonBottom.Text = "Move to Bottom";
      this.toolStripButtonBottom.Click += new System.EventHandler(this.toolStripButtonBottom_Click);
      // 
      // EditMacro
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(520, 337);
      this.Controls.Add(this.groupBoxMacroName);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.splitContainer);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(528, 364);
      this.Name = "EditMacro";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Macro Editor";
      this.groupBoxCommandSequence.ResumeLayout(false);
      this.groupBoxCommandSequence.PerformLayout();
      this.groupBoxCommands.ResumeLayout(false);
      this.groupBoxMacroName.ResumeLayout(false);
      this.groupBoxMacroName.PerformLayout();
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.ResumeLayout(false);
      this.toolStripCommandSequence.ResumeLayout(false);
      this.toolStripCommandSequence.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.GroupBox groupBoxCommandSequence;
    private System.Windows.Forms.GroupBox groupBoxCommands;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.GroupBox groupBoxMacroName;
    private System.Windows.Forms.TreeView treeViewCommandList;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.ListView listViewMacro;
    private System.Windows.Forms.ColumnHeader columnHeader;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.ToolStrip toolStripCommandSequence;
    private System.Windows.Forms.ToolStripButton toolStripButtonTop;
    private System.Windows.Forms.ToolStripButton toolStripButtonUp;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
    private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteAll;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripButtonDown;
    private System.Windows.Forms.ToolStripButton toolStripButtonBottom;
  }

}
