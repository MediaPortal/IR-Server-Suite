namespace MediaPortal.Plugins
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
      this.treeViewMenu = new System.Windows.Forms.TreeView();
      this.buttonChangeServer = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageMenuSetup = new System.Windows.Forms.TabPage();
      this.buttonSetCommand = new System.Windows.Forms.Button();
      this.groupBoxTreeCommands = new System.Windows.Forms.GroupBox();
      this.buttonEditTree = new System.Windows.Forms.Button();
      this.buttonAdd = new System.Windows.Forms.Button();
      this.buttonDeleteAll = new System.Windows.Forms.Button();
      this.buttonDelete = new System.Windows.Forms.Button();
      this.buttonBottom = new System.Windows.Forms.Button();
      this.buttonDown = new System.Windows.Forms.Button();
      this.buttonUp = new System.Windows.Forms.Button();
      this.buttonTop = new System.Windows.Forms.Button();
      this.comboBoxCommands = new System.Windows.Forms.ComboBox();
      this.buttonNewCommand = new System.Windows.Forms.Button();
      this.tabPageIR = new System.Windows.Forms.TabPage();
      this.listBoxIR = new System.Windows.Forms.ListBox();
      this.buttonNewIR = new System.Windows.Forms.Button();
      this.buttonEditIR = new System.Windows.Forms.Button();
      this.buttonDeleteIR = new System.Windows.Forms.Button();
      this.tabPageMacros = new System.Windows.Forms.TabPage();
      this.buttonTestMacro = new System.Windows.Forms.Button();
      this.buttonDeleteMacro = new System.Windows.Forms.Button();
      this.listBoxMacro = new System.Windows.Forms.ListBox();
      this.buttonEditMacro = new System.Windows.Forms.Button();
      this.buttonNewMacro = new System.Windows.Forms.Button();
      this.checkBoxLogVerbose = new System.Windows.Forms.CheckBox();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.buttonHelp = new System.Windows.Forms.Button();
      this.tabControl.SuspendLayout();
      this.tabPageMenuSetup.SuspendLayout();
      this.groupBoxTreeCommands.SuspendLayout();
      this.tabPageIR.SuspendLayout();
      this.tabPageMacros.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeViewMenu
      // 
      this.treeViewMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.treeViewMenu.FullRowSelect = true;
      this.treeViewMenu.HideSelection = false;
      this.treeViewMenu.LabelEdit = true;
      this.treeViewMenu.Location = new System.Drawing.Point(8, 8);
      this.treeViewMenu.Name = "treeViewMenu";
      this.treeViewMenu.Size = new System.Drawing.Size(432, 288);
      this.treeViewMenu.TabIndex = 0;
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
      this.toolTip.SetToolTip(this.buttonChangeServer, "Change the IR Server host");
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
      this.tabControl.Controls.Add(this.tabPageIR);
      this.tabControl.Controls.Add(this.tabPageMacros);
      this.tabControl.Location = new System.Drawing.Point(8, 8);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(504, 360);
      this.tabControl.TabIndex = 0;
      // 
      // tabPageMenuSetup
      // 
      this.tabPageMenuSetup.Controls.Add(this.buttonSetCommand);
      this.tabPageMenuSetup.Controls.Add(this.groupBoxTreeCommands);
      this.tabPageMenuSetup.Controls.Add(this.comboBoxCommands);
      this.tabPageMenuSetup.Controls.Add(this.buttonNewCommand);
      this.tabPageMenuSetup.Controls.Add(this.treeViewMenu);
      this.tabPageMenuSetup.Location = new System.Drawing.Point(4, 22);
      this.tabPageMenuSetup.Name = "tabPageMenuSetup";
      this.tabPageMenuSetup.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMenuSetup.Size = new System.Drawing.Size(496, 334);
      this.tabPageMenuSetup.TabIndex = 1;
      this.tabPageMenuSetup.Text = "Menu Setup";
      this.tabPageMenuSetup.UseVisualStyleBackColor = true;
      // 
      // buttonSetCommand
      // 
      this.buttonSetCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonSetCommand.Location = new System.Drawing.Point(128, 304);
      this.buttonSetCommand.Name = "buttonSetCommand";
      this.buttonSetCommand.Size = new System.Drawing.Size(48, 24);
      this.buttonSetCommand.TabIndex = 3;
      this.buttonSetCommand.Text = "Set:";
      this.buttonSetCommand.UseVisualStyleBackColor = true;
      this.buttonSetCommand.Click += new System.EventHandler(this.buttonSetCommand_Click);
      // 
      // groupBoxTreeCommands
      // 
      this.groupBoxTreeCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTreeCommands.Controls.Add(this.buttonEditTree);
      this.groupBoxTreeCommands.Controls.Add(this.buttonAdd);
      this.groupBoxTreeCommands.Controls.Add(this.buttonDeleteAll);
      this.groupBoxTreeCommands.Controls.Add(this.buttonDelete);
      this.groupBoxTreeCommands.Controls.Add(this.buttonBottom);
      this.groupBoxTreeCommands.Controls.Add(this.buttonDown);
      this.groupBoxTreeCommands.Controls.Add(this.buttonUp);
      this.groupBoxTreeCommands.Controls.Add(this.buttonTop);
      this.groupBoxTreeCommands.Location = new System.Drawing.Point(448, 0);
      this.groupBoxTreeCommands.Name = "groupBoxTreeCommands";
      this.groupBoxTreeCommands.Size = new System.Drawing.Size(40, 296);
      this.groupBoxTreeCommands.TabIndex = 1;
      this.groupBoxTreeCommands.TabStop = false;
      // 
      // buttonEditTree
      // 
      this.buttonEditTree.Image = global::MediaPortal.Plugins.Properties.Resources.Edit;
      this.buttonEditTree.Location = new System.Drawing.Point(8, 120);
      this.buttonEditTree.Name = "buttonEditTree";
      this.buttonEditTree.Size = new System.Drawing.Size(24, 24);
      this.buttonEditTree.TabIndex = 3;
      this.toolTip.SetToolTip(this.buttonEditTree, "Edit the selected item");
      this.buttonEditTree.UseVisualStyleBackColor = true;
      this.buttonEditTree.Click += new System.EventHandler(this.buttonEditTree_Click);
      // 
      // buttonAdd
      // 
      this.buttonAdd.Image = global::MediaPortal.Plugins.Properties.Resources.Plus;
      this.buttonAdd.Location = new System.Drawing.Point(8, 88);
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Size = new System.Drawing.Size(24, 24);
      this.buttonAdd.TabIndex = 2;
      this.toolTip.SetToolTip(this.buttonAdd, "Add a new collection");
      this.buttonAdd.UseVisualStyleBackColor = true;
      this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
      // 
      // buttonDeleteAll
      // 
      this.buttonDeleteAll.Image = global::MediaPortal.Plugins.Properties.Resources.DeleteAll;
      this.buttonDeleteAll.Location = new System.Drawing.Point(8, 184);
      this.buttonDeleteAll.Name = "buttonDeleteAll";
      this.buttonDeleteAll.Size = new System.Drawing.Size(24, 24);
      this.buttonDeleteAll.TabIndex = 5;
      this.toolTip.SetToolTip(this.buttonDeleteAll, "Delete all items");
      this.buttonDeleteAll.UseVisualStyleBackColor = true;
      this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
      // 
      // buttonDelete
      // 
      this.buttonDelete.Image = global::MediaPortal.Plugins.Properties.Resources.Delete;
      this.buttonDelete.Location = new System.Drawing.Point(8, 152);
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new System.Drawing.Size(24, 24);
      this.buttonDelete.TabIndex = 4;
      this.toolTip.SetToolTip(this.buttonDelete, "Delete selected item");
      this.buttonDelete.UseVisualStyleBackColor = true;
      this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
      // 
      // buttonBottom
      // 
      this.buttonBottom.Image = global::MediaPortal.Plugins.Properties.Resources.MoveBottom;
      this.buttonBottom.Location = new System.Drawing.Point(8, 256);
      this.buttonBottom.Name = "buttonBottom";
      this.buttonBottom.Size = new System.Drawing.Size(24, 24);
      this.buttonBottom.TabIndex = 7;
      this.toolTip.SetToolTip(this.buttonBottom, "Move selected item to the bottom");
      this.buttonBottom.UseVisualStyleBackColor = true;
      this.buttonBottom.Click += new System.EventHandler(this.buttonBottom_Click);
      // 
      // buttonDown
      // 
      this.buttonDown.Image = global::MediaPortal.Plugins.Properties.Resources.MoveDown;
      this.buttonDown.Location = new System.Drawing.Point(8, 224);
      this.buttonDown.Name = "buttonDown";
      this.buttonDown.Size = new System.Drawing.Size(24, 24);
      this.buttonDown.TabIndex = 6;
      this.toolTip.SetToolTip(this.buttonDown, "Move selected item down");
      this.buttonDown.UseVisualStyleBackColor = true;
      this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
      // 
      // buttonUp
      // 
      this.buttonUp.Image = global::MediaPortal.Plugins.Properties.Resources.MoveUp;
      this.buttonUp.Location = new System.Drawing.Point(8, 48);
      this.buttonUp.Name = "buttonUp";
      this.buttonUp.Size = new System.Drawing.Size(24, 24);
      this.buttonUp.TabIndex = 1;
      this.toolTip.SetToolTip(this.buttonUp, "Move selected item up");
      this.buttonUp.UseVisualStyleBackColor = true;
      this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
      // 
      // buttonTop
      // 
      this.buttonTop.Image = global::MediaPortal.Plugins.Properties.Resources.MoveTop;
      this.buttonTop.Location = new System.Drawing.Point(8, 16);
      this.buttonTop.Name = "buttonTop";
      this.buttonTop.Size = new System.Drawing.Size(24, 24);
      this.buttonTop.TabIndex = 0;
      this.toolTip.SetToolTip(this.buttonTop, "Move selected item to the top");
      this.buttonTop.UseVisualStyleBackColor = true;
      this.buttonTop.Click += new System.EventHandler(this.buttonTop_Click);
      // 
      // comboBoxCommands
      // 
      this.comboBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.comboBoxCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxCommands.FormattingEnabled = true;
      this.comboBoxCommands.Location = new System.Drawing.Point(184, 305);
      this.comboBoxCommands.Name = "comboBoxCommands";
      this.comboBoxCommands.Size = new System.Drawing.Size(256, 21);
      this.comboBoxCommands.TabIndex = 4;
      // 
      // buttonNewCommand
      // 
      this.buttonNewCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewCommand.Location = new System.Drawing.Point(8, 304);
      this.buttonNewCommand.Name = "buttonNewCommand";
      this.buttonNewCommand.Size = new System.Drawing.Size(96, 24);
      this.buttonNewCommand.TabIndex = 2;
      this.buttonNewCommand.Text = "New Command";
      this.buttonNewCommand.UseVisualStyleBackColor = true;
      this.buttonNewCommand.Click += new System.EventHandler(this.buttonNewCommand_Click);
      // 
      // tabPageIR
      // 
      this.tabPageIR.Controls.Add(this.listBoxIR);
      this.tabPageIR.Controls.Add(this.buttonNewIR);
      this.tabPageIR.Controls.Add(this.buttonEditIR);
      this.tabPageIR.Controls.Add(this.buttonDeleteIR);
      this.tabPageIR.Location = new System.Drawing.Point(4, 22);
      this.tabPageIR.Name = "tabPageIR";
      this.tabPageIR.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageIR.Size = new System.Drawing.Size(496, 334);
      this.tabPageIR.TabIndex = 2;
      this.tabPageIR.Text = "IR Commands";
      this.tabPageIR.UseVisualStyleBackColor = true;
      // 
      // listBoxIR
      // 
      this.listBoxIR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxIR.ColumnWidth = 260;
      this.listBoxIR.FormattingEnabled = true;
      this.listBoxIR.HorizontalScrollbar = true;
      this.listBoxIR.IntegralHeight = false;
      this.listBoxIR.Location = new System.Drawing.Point(8, 8);
      this.listBoxIR.MultiColumn = true;
      this.listBoxIR.Name = "listBoxIR";
      this.listBoxIR.Size = new System.Drawing.Size(480, 288);
      this.listBoxIR.TabIndex = 0;
      this.listBoxIR.DoubleClick += new System.EventHandler(this.listBoxIR_DoubleClick);
      // 
      // buttonNewIR
      // 
      this.buttonNewIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewIR.Location = new System.Drawing.Point(8, 304);
      this.buttonNewIR.Name = "buttonNewIR";
      this.buttonNewIR.Size = new System.Drawing.Size(56, 24);
      this.buttonNewIR.TabIndex = 1;
      this.buttonNewIR.Text = "New";
      this.buttonNewIR.UseVisualStyleBackColor = true;
      this.buttonNewIR.Click += new System.EventHandler(this.buttonNewIR_Click);
      // 
      // buttonEditIR
      // 
      this.buttonEditIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEditIR.Location = new System.Drawing.Point(72, 304);
      this.buttonEditIR.Name = "buttonEditIR";
      this.buttonEditIR.Size = new System.Drawing.Size(56, 24);
      this.buttonEditIR.TabIndex = 2;
      this.buttonEditIR.Text = "Edit";
      this.buttonEditIR.UseVisualStyleBackColor = true;
      this.buttonEditIR.Click += new System.EventHandler(this.buttonEditIR_Click);
      // 
      // buttonDeleteIR
      // 
      this.buttonDeleteIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDeleteIR.Location = new System.Drawing.Point(136, 304);
      this.buttonDeleteIR.Name = "buttonDeleteIR";
      this.buttonDeleteIR.Size = new System.Drawing.Size(56, 24);
      this.buttonDeleteIR.TabIndex = 3;
      this.buttonDeleteIR.Text = "Delete";
      this.buttonDeleteIR.UseVisualStyleBackColor = true;
      this.buttonDeleteIR.Click += new System.EventHandler(this.buttonDeleteIR_Click);
      // 
      // tabPageMacros
      // 
      this.tabPageMacros.Controls.Add(this.buttonTestMacro);
      this.tabPageMacros.Controls.Add(this.buttonDeleteMacro);
      this.tabPageMacros.Controls.Add(this.listBoxMacro);
      this.tabPageMacros.Controls.Add(this.buttonEditMacro);
      this.tabPageMacros.Controls.Add(this.buttonNewMacro);
      this.tabPageMacros.Location = new System.Drawing.Point(4, 22);
      this.tabPageMacros.Name = "tabPageMacros";
      this.tabPageMacros.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMacros.Size = new System.Drawing.Size(496, 334);
      this.tabPageMacros.TabIndex = 3;
      this.tabPageMacros.Text = "Macros";
      this.tabPageMacros.UseVisualStyleBackColor = true;
      // 
      // buttonTestMacro
      // 
      this.buttonTestMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTestMacro.Location = new System.Drawing.Point(208, 304);
      this.buttonTestMacro.Name = "buttonTestMacro";
      this.buttonTestMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonTestMacro.TabIndex = 4;
      this.buttonTestMacro.Text = "Test";
      this.buttonTestMacro.UseVisualStyleBackColor = true;
      this.buttonTestMacro.Click += new System.EventHandler(this.buttonTestMacro_Click);
      // 
      // buttonDeleteMacro
      // 
      this.buttonDeleteMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDeleteMacro.Location = new System.Drawing.Point(136, 304);
      this.buttonDeleteMacro.Name = "buttonDeleteMacro";
      this.buttonDeleteMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonDeleteMacro.TabIndex = 3;
      this.buttonDeleteMacro.Text = "Delete";
      this.buttonDeleteMacro.UseVisualStyleBackColor = true;
      this.buttonDeleteMacro.Click += new System.EventHandler(this.buttonDeleteMacro_Click);
      // 
      // listBoxMacro
      // 
      this.listBoxMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxMacro.ColumnWidth = 260;
      this.listBoxMacro.FormattingEnabled = true;
      this.listBoxMacro.HorizontalScrollbar = true;
      this.listBoxMacro.IntegralHeight = false;
      this.listBoxMacro.Location = new System.Drawing.Point(8, 8);
      this.listBoxMacro.MultiColumn = true;
      this.listBoxMacro.Name = "listBoxMacro";
      this.listBoxMacro.Size = new System.Drawing.Size(480, 288);
      this.listBoxMacro.TabIndex = 0;
      this.listBoxMacro.DoubleClick += new System.EventHandler(this.listBoxMacro_DoubleClick);
      // 
      // buttonEditMacro
      // 
      this.buttonEditMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEditMacro.Location = new System.Drawing.Point(72, 304);
      this.buttonEditMacro.Name = "buttonEditMacro";
      this.buttonEditMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonEditMacro.TabIndex = 2;
      this.buttonEditMacro.Text = "Edit";
      this.buttonEditMacro.UseVisualStyleBackColor = true;
      this.buttonEditMacro.Click += new System.EventHandler(this.buttonEditMacro_Click);
      // 
      // buttonNewMacro
      // 
      this.buttonNewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewMacro.Location = new System.Drawing.Point(8, 304);
      this.buttonNewMacro.Name = "buttonNewMacro";
      this.buttonNewMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonNewMacro.TabIndex = 1;
      this.buttonNewMacro.Text = "New";
      this.buttonNewMacro.UseVisualStyleBackColor = true;
      this.buttonNewMacro.Click += new System.EventHandler(this.buttonNewMacro_Click);
      // 
      // checkBoxLogVerbose
      // 
      this.checkBoxLogVerbose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.checkBoxLogVerbose.Location = new System.Drawing.Point(216, 376);
      this.checkBoxLogVerbose.Name = "checkBoxLogVerbose";
      this.checkBoxLogVerbose.Size = new System.Drawing.Size(120, 24);
      this.checkBoxLogVerbose.TabIndex = 3;
      this.checkBoxLogVerbose.Text = "&Extended logging";
      this.toolTip.SetToolTip(this.checkBoxLogVerbose, "Enable more detailed logging of plugin operations");
      this.checkBoxLogVerbose.UseVisualStyleBackColor = true;
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(112, 376);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 24);
      this.buttonHelp.TabIndex = 2;
      this.buttonHelp.Text = "&Help";
      this.toolTip.SetToolTip(this.buttonHelp, "Click here for help");
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // SetupForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(520, 408);
      this.Controls.Add(this.checkBoxLogVerbose);
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
      this.Load += new System.EventHandler(this.SetupForm_Load);
      this.tabControl.ResumeLayout(false);
      this.tabPageMenuSetup.ResumeLayout(false);
      this.groupBoxTreeCommands.ResumeLayout(false);
      this.tabPageIR.ResumeLayout(false);
      this.tabPageMacros.ResumeLayout(false);
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
    private System.Windows.Forms.ListBox listBoxIR;
    private System.Windows.Forms.Button buttonNewIR;
    private System.Windows.Forms.Button buttonEditIR;
    private System.Windows.Forms.Button buttonDeleteIR;
    private System.Windows.Forms.TabPage tabPageMacros;
    private System.Windows.Forms.Button buttonTestMacro;
    private System.Windows.Forms.Button buttonDeleteMacro;
    private System.Windows.Forms.ListBox listBoxMacro;
    private System.Windows.Forms.Button buttonEditMacro;
    private System.Windows.Forms.Button buttonNewMacro;
    private System.Windows.Forms.CheckBox checkBoxLogVerbose;
    private System.Windows.Forms.Button buttonNewCommand;
    private System.Windows.Forms.GroupBox groupBoxTreeCommands;
    private System.Windows.Forms.Button buttonBottom;
    private System.Windows.Forms.Button buttonDown;
    private System.Windows.Forms.Button buttonUp;
    private System.Windows.Forms.Button buttonTop;
    private System.Windows.Forms.ComboBox comboBoxCommands;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.Button buttonDeleteAll;
    private System.Windows.Forms.Button buttonDelete;
    private System.Windows.Forms.Button buttonSetCommand;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.Button buttonEditTree;
  }
}