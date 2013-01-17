namespace MediaPortal.Plugins.IRSS.MPBlastZonePlugin.Forms
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
      this.treeViewMenu = new System.Windows.Forms.TreeView();
      this.buttonChangeServer = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageMenuSetup = new System.Windows.Forms.TabPage();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.groupBoxMenu = new System.Windows.Forms.GroupBox();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonTop = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonUp = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonAddCategory = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonAddCommand = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteAll = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonDown = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonBottom = new System.Windows.Forms.ToolStripButton();
      this.treeViewCommandList = new System.Windows.Forms.TreeView();
      this.tabPageMacros = new System.Windows.Forms.TabPage();
      this.tabPageIR = new System.Windows.Forms.TabPage();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.buttonHelp = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.tabControl.SuspendLayout();
      this.tabPageMenuSetup.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBoxMenu.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.RightToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeViewMenu
      // 
      this.treeViewMenu.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewMenu.FullRowSelect = true;
      this.treeViewMenu.HideSelection = false;
      this.treeViewMenu.LabelEdit = true;
      this.treeViewMenu.Location = new System.Drawing.Point(0, 0);
      this.treeViewMenu.Name = "treeViewMenu";
      this.treeViewMenu.Size = new System.Drawing.Size(138, 309);
      this.treeViewMenu.TabIndex = 0;
      this.treeViewMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMenu_AfterSelect);
      this.treeViewMenu.DoubleClick += new System.EventHandler(this.treeViewMenu_DoubleClick);
      // 
      // buttonChangeServer
      // 
      this.buttonChangeServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonChangeServer.Location = new System.Drawing.Point(8, 376);
      this.buttonChangeServer.Name = "buttonChangeServer";
      this.buttonChangeServer.Size = new System.Drawing.Size(96, 24);
      this.buttonChangeServer.TabIndex = 1;
      this.buttonChangeServer.Text = "Change &Server";
      this.toolTips.SetToolTip(this.buttonChangeServer, "Change the IR Server host");
      this.buttonChangeServer.UseVisualStyleBackColor = true;
      this.buttonChangeServer.Click += new System.EventHandler(this.buttonChangeServer_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(456, 376);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(392, 376);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.tabPageMenuSetup);
      this.tabControl.Controls.Add(this.tabPageMacros);
      this.tabControl.Controls.Add(this.tabPageIR);
      this.tabControl.Location = new System.Drawing.Point(8, 8);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(504, 360);
      this.tabControl.TabIndex = 0;
      // 
      // tabPageMenuSetup
      // 
      this.tabPageMenuSetup.Controls.Add(this.splitContainer1);
      this.tabPageMenuSetup.Location = new System.Drawing.Point(4, 22);
      this.tabPageMenuSetup.Name = "tabPageMenuSetup";
      this.tabPageMenuSetup.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMenuSetup.Size = new System.Drawing.Size(496, 334);
      this.tabPageMenuSetup.TabIndex = 1;
      this.tabPageMenuSetup.Text = "Menu Setup";
      this.tabPageMenuSetup.UseVisualStyleBackColor = true;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(3, 3);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.groupBoxMenu);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
      this.splitContainer1.Size = new System.Drawing.Size(490, 328);
      this.splitContainer1.SplitterDistance = 235;
      this.splitContainer1.TabIndex = 6;
      // 
      // groupBoxMenu
      // 
      this.groupBoxMenu.Controls.Add(this.toolStripContainer1);
      this.groupBoxMenu.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxMenu.Location = new System.Drawing.Point(0, 0);
      this.groupBoxMenu.Name = "groupBoxMenu";
      this.groupBoxMenu.Size = new System.Drawing.Size(235, 328);
      this.groupBoxMenu.TabIndex = 0;
      this.groupBoxMenu.TabStop = false;
      this.groupBoxMenu.Text = "Menu";
      // 
      // toolStripContainer1
      // 
      this.toolStripContainer1.BottomToolStripPanelVisible = false;
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(this.treeViewMenu);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(138, 309);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point(3, 16);
      this.toolStripContainer1.Name = "toolStripContainer1";
      // 
      // toolStripContainer1.RightToolStripPanel
      // 
      this.toolStripContainer1.RightToolStripPanel.Controls.Add(this.toolStrip1);
      this.toolStripContainer1.Size = new System.Drawing.Size(229, 309);
      this.toolStripContainer1.TabIndex = 5;
      this.toolStripContainer1.Text = "toolStripContainer1";
      this.toolStripContainer1.TopToolStripPanelVisible = false;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTop,
            this.toolStripButtonUp,
            this.toolStripSeparator1,
            this.toolStripButtonAddCategory,
            this.toolStripButtonAddCommand,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonDeleteAll,
            this.toolStripSeparator2,
            this.toolStripButtonDown,
            this.toolStripButtonBottom});
      this.toolStrip1.Location = new System.Drawing.Point(0, 3);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(91, 221);
      this.toolStrip1.TabIndex = 0;
      // 
      // toolStripButtonTop
      // 
      this.toolStripButtonTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonTop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTop.Image")));
      this.toolStripButtonTop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonTop.Name = "toolStripButtonTop";
      this.toolStripButtonTop.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonTop.Text = "Top";
      this.toolStripButtonTop.Click += new System.EventHandler(this.toolStripButtonTop_Click);
      // 
      // toolStripButtonUp
      // 
      this.toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUp.Image")));
      this.toolStripButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonUp.Name = "toolStripButtonUp";
      this.toolStripButtonUp.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonUp.Text = "Up";
      this.toolStripButtonUp.Click += new System.EventHandler(this.toolStripButtonUp_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(89, 6);
      // 
      // toolStripButtonAddCategory
      // 
      this.toolStripButtonAddCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonAddCategory.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddCategory.Image")));
      this.toolStripButtonAddCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAddCategory.Name = "toolStripButtonAddCategory";
      this.toolStripButtonAddCategory.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonAddCategory.Text = "AddCategory";
      this.toolStripButtonAddCategory.Click += new System.EventHandler(this.toolStripButtonAddCategory_Click);
      // 
      // toolStripButtonAddCommand
      // 
      this.toolStripButtonAddCommand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonAddCommand.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddCommand.Image")));
      this.toolStripButtonAddCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAddCommand.Name = "toolStripButtonAddCommand";
      this.toolStripButtonAddCommand.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonAddCommand.Text = "AddCommand";
      this.toolStripButtonAddCommand.Click += new System.EventHandler(this.toolStripButtonAddCommand_Click);
      // 
      // toolStripButtonEdit
      // 
      this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdit.Image")));
      this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEdit.Name = "toolStripButtonEdit";
      this.toolStripButtonEdit.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonEdit.Text = "Edit";
      this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
      // 
      // toolStripButtonDelete
      // 
      this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
      this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDelete.Name = "toolStripButtonDelete";
      this.toolStripButtonDelete.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonDelete.Text = "Delete";
      this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
      // 
      // toolStripButtonDeleteAll
      // 
      this.toolStripButtonDeleteAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonDeleteAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteAll.Image")));
      this.toolStripButtonDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteAll.Name = "toolStripButtonDeleteAll";
      this.toolStripButtonDeleteAll.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonDeleteAll.Text = "DeleteAll";
      this.toolStripButtonDeleteAll.Click += new System.EventHandler(this.toolStripButtonDeleteAll_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(89, 6);
      // 
      // toolStripButtonDown
      // 
      this.toolStripButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDown.Image")));
      this.toolStripButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDown.Name = "toolStripButtonDown";
      this.toolStripButtonDown.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonDown.Text = "Down";
      this.toolStripButtonDown.Click += new System.EventHandler(this.toolStripButtonDown_Click);
      // 
      // toolStripButtonBottom
      // 
      this.toolStripButtonBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripButtonBottom.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBottom.Image")));
      this.toolStripButtonBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonBottom.Name = "toolStripButtonBottom";
      this.toolStripButtonBottom.Size = new System.Drawing.Size(89, 19);
      this.toolStripButtonBottom.Text = "Bottom";
      this.toolStripButtonBottom.Click += new System.EventHandler(this.toolStripButtonBottom_Click);
      // 
      // treeViewCommandList
      // 
      this.treeViewCommandList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewCommandList.FullRowSelect = true;
      this.treeViewCommandList.Location = new System.Drawing.Point(3, 16);
      this.treeViewCommandList.Name = "treeViewCommandList";
      this.treeViewCommandList.Size = new System.Drawing.Size(245, 309);
      this.treeViewCommandList.TabIndex = 14;
      this.treeViewCommandList.DoubleClick += new System.EventHandler(this.treeViewCommandList_DoubleClick);
      // 
      // tabPageMacros
      // 
      this.tabPageMacros.Location = new System.Drawing.Point(4, 22);
      this.tabPageMacros.Name = "tabPageMacros";
      this.tabPageMacros.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMacros.Size = new System.Drawing.Size(496, 334);
      this.tabPageMacros.TabIndex = 3;
      this.tabPageMacros.Text = "Macros";
      this.tabPageMacros.UseVisualStyleBackColor = true;
      // 
      // tabPageIR
      // 
      this.tabPageIR.Location = new System.Drawing.Point(4, 22);
      this.tabPageIR.Name = "tabPageIR";
      this.tabPageIR.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageIR.Size = new System.Drawing.Size(496, 334);
      this.tabPageIR.TabIndex = 2;
      this.tabPageIR.Text = "IR Commands";
      this.tabPageIR.UseVisualStyleBackColor = true;
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(112, 376);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 24);
      this.buttonHelp.TabIndex = 2;
      this.buttonHelp.Text = "&Help";
      this.toolTips.SetToolTip(this.buttonHelp, "Click here for help");
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.treeViewCommandList);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(251, 328);
      this.groupBox1.TabIndex = 15;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "DoubleClick to assign a new Command";
      // 
      // SetupForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(520, 415);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.buttonChangeServer);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimumSize = new System.Drawing.Size(528, 442);
      this.Name = "SetupForm";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "MediaPortal Blast Zone Plugin";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
      this.Load += new System.EventHandler(this.SetupForm_Load);
      this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.SetupForm_HelpRequested);
      this.tabControl.ResumeLayout(false);
      this.tabPageMenuSetup.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.groupBoxMenu.ResumeLayout(false);
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.RightToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.RightToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView treeViewMenu;
    private System.Windows.Forms.Button buttonChangeServer;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageMenuSetup;
    private System.Windows.Forms.TabPage tabPageIR;
    private System.Windows.Forms.TabPage tabPageMacros;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButtonTop;
    private System.Windows.Forms.ToolStripButton toolStripButtonUp;
    private System.Windows.Forms.ToolStripButton toolStripButtonAddCategory;
    private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteAll;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripButtonDown;
    private System.Windows.Forms.ToolStripButton toolStripButtonBottom;
    private System.Windows.Forms.GroupBox groupBoxMenu;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TreeView treeViewCommandList;
    private System.Windows.Forms.ToolStripButton toolStripButtonAddCommand;
    private System.Windows.Forms.GroupBox groupBox1;
  }
}