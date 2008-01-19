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
      this.buttonRemove = new System.Windows.Forms.Button();
      this.buttonMoveDown = new System.Windows.Forms.Button();
      this.buttonMoveUp = new System.Windows.Forms.Button();
      this.groupBoxCommands = new System.Windows.Forms.GroupBox();
      this.treeViewCommandList = new System.Windows.Forms.TreeView();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxMacroName = new System.Windows.Forms.GroupBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.groupBoxCommandSequence.SuspendLayout();
      this.groupBoxCommands.SuspendLayout();
      this.groupBoxMacroName.SuspendLayout();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBoxName
      // 
      this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxName.Location = new System.Drawing.Point(8, 16);
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new System.Drawing.Size(294, 22);
      this.textBoxName.TabIndex = 0;
      this.toolTips.SetToolTip(this.textBoxName, "Provide a name for this macro (must be a valid windows file name)");
      // 
      // groupBoxCommandSequence
      // 
      this.groupBoxCommandSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCommandSequence.Controls.Add(this.listViewMacro);
      this.groupBoxCommandSequence.Controls.Add(this.buttonRemove);
      this.groupBoxCommandSequence.Controls.Add(this.buttonMoveDown);
      this.groupBoxCommandSequence.Controls.Add(this.buttonMoveUp);
      this.groupBoxCommandSequence.Location = new System.Drawing.Point(0, 56);
      this.groupBoxCommandSequence.Name = "groupBoxCommandSequence";
      this.groupBoxCommandSequence.Size = new System.Drawing.Size(310, 232);
      this.groupBoxCommandSequence.TabIndex = 1;
      this.groupBoxCommandSequence.TabStop = false;
      this.groupBoxCommandSequence.Text = "Macro";
      // 
      // listViewMacro
      // 
      this.listViewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewMacro.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
      this.listViewMacro.FullRowSelect = true;
      this.listViewMacro.GridLines = true;
      this.listViewMacro.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewMacro.HideSelection = false;
      this.listViewMacro.Location = new System.Drawing.Point(8, 16);
      this.listViewMacro.Name = "listViewMacro";
      this.listViewMacro.ShowGroups = false;
      this.listViewMacro.Size = new System.Drawing.Size(294, 176);
      this.listViewMacro.TabIndex = 0;
      this.listViewMacro.UseCompatibleStateImageBehavior = false;
      this.listViewMacro.View = System.Windows.Forms.View.Details;
      this.listViewMacro.DoubleClick += new System.EventHandler(this.listViewMacro_DoubleClick);
      // 
      // columnHeader
      // 
      this.columnHeader.Text = "Commands";
      this.columnHeader.Width = 276;
      // 
      // buttonRemove
      // 
      this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRemove.Location = new System.Drawing.Point(246, 200);
      this.buttonRemove.Name = "buttonRemove";
      this.buttonRemove.Size = new System.Drawing.Size(56, 24);
      this.buttonRemove.TabIndex = 3;
      this.buttonRemove.Text = "Remove";
      this.toolTips.SetToolTip(this.buttonRemove, "Remove the selected command(s)");
      this.buttonRemove.UseVisualStyleBackColor = true;
      this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
      // 
      // buttonMoveDown
      // 
      this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonMoveDown.Location = new System.Drawing.Point(64, 200);
      this.buttonMoveDown.Name = "buttonMoveDown";
      this.buttonMoveDown.Size = new System.Drawing.Size(48, 24);
      this.buttonMoveDown.TabIndex = 2;
      this.buttonMoveDown.Text = "Down";
      this.toolTips.SetToolTip(this.buttonMoveDown, "Move the selected command(s) down one position");
      this.buttonMoveDown.UseVisualStyleBackColor = true;
      this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
      // 
      // buttonMoveUp
      // 
      this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonMoveUp.Location = new System.Drawing.Point(8, 200);
      this.buttonMoveUp.Name = "buttonMoveUp";
      this.buttonMoveUp.Size = new System.Drawing.Size(48, 24);
      this.buttonMoveUp.TabIndex = 1;
      this.buttonMoveUp.Text = "Up";
      this.toolTips.SetToolTip(this.buttonMoveUp, "Move the selected command(s) up one position");
      this.buttonMoveUp.UseVisualStyleBackColor = true;
      this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
      // 
      // groupBoxCommands
      // 
      this.groupBoxCommands.Controls.Add(this.treeViewCommandList);
      this.groupBoxCommands.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxCommands.Location = new System.Drawing.Point(0, 0);
      this.groupBoxCommands.Name = "groupBoxCommands";
      this.groupBoxCommands.Size = new System.Drawing.Size(156, 288);
      this.groupBoxCommands.TabIndex = 0;
      this.groupBoxCommands.TabStop = false;
      this.groupBoxCommands.Text = "Commands";
      // 
      // treeViewCommandList
      // 
      this.treeViewCommandList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.treeViewCommandList.Location = new System.Drawing.Point(8, 16);
      this.treeViewCommandList.Name = "treeViewCommandList";
      this.treeViewCommandList.Size = new System.Drawing.Size(140, 264);
      this.treeViewCommandList.TabIndex = 0;
      this.treeViewCommandList.DoubleClick += new System.EventHandler(this.treeViewCommandList_DoubleClick);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(424, 304);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 3;
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
      this.buttonTest.TabIndex = 1;
      this.buttonTest.Text = "Test";
      this.toolTips.SetToolTip(this.buttonTest, "Test this macro");
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(360, 304);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // groupBoxMacroName
      // 
      this.groupBoxMacroName.Controls.Add(this.textBoxName);
      this.groupBoxMacroName.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBoxMacroName.Location = new System.Drawing.Point(0, 0);
      this.groupBoxMacroName.Name = "groupBoxMacroName";
      this.groupBoxMacroName.Size = new System.Drawing.Size(310, 48);
      this.groupBoxMacroName.TabIndex = 0;
      this.groupBoxMacroName.TabStop = false;
      this.groupBoxMacroName.Text = "Macro Name";
      // 
      // splitContainer
      // 
      this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer.Location = new System.Drawing.Point(8, 8);
      this.splitContainer.Name = "splitContainer";
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.Controls.Add(this.groupBoxCommandSequence);
      this.splitContainer.Panel1.Controls.Add(this.groupBoxMacroName);
      this.splitContainer.Panel1MinSize = 256;
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.groupBoxCommands);
      this.splitContainer.Panel2MinSize = 128;
      this.splitContainer.Size = new System.Drawing.Size(472, 288);
      this.splitContainer.SplitterDistance = 310;
      this.splitContainer.SplitterWidth = 6;
      this.splitContainer.TabIndex = 0;
      // 
      // EditMacro
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(488, 337);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.splitContainer);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(496, 364);
      this.Name = "EditMacro";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Macro Editor";
      this.groupBoxCommandSequence.ResumeLayout(false);
      this.groupBoxCommands.ResumeLayout(false);
      this.groupBoxMacroName.ResumeLayout(false);
      this.groupBoxMacroName.PerformLayout();
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.GroupBox groupBoxCommandSequence;
    private System.Windows.Forms.Button buttonRemove;
    private System.Windows.Forms.Button buttonMoveDown;
    private System.Windows.Forms.Button buttonMoveUp;
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
  }

}
