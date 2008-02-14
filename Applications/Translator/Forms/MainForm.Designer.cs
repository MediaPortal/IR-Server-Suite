namespace Translator
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
      this.listViewPrograms = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.contextMenuStripPrograms = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.imageListPrograms = new System.Windows.Forms.ImageList(this.components);
      this.listViewButtons = new System.Windows.Forms.ListView();
      this.columnHeaderButton = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderDescription = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderCommand = new System.Windows.Forms.ColumnHeader();
      this.contextMenuStripButtonMapping = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.newButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearButtonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.remapButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.copyButtonsFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.buttonOK = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPagePrograms = new System.Windows.Forms.TabPage();
      this.panelPrograms = new System.Windows.Forms.Panel();
      this.panelProgramsButtons = new System.Windows.Forms.Panel();
      this.labelProgramsDelete = new System.Windows.Forms.Label();
      this.labelProgramsEdit = new System.Windows.Forms.Label();
      this.labelProgramsAdd = new System.Windows.Forms.Label();
      this.toolStripButtonMappings = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonNewMapping = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEditMapping = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteMapping = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteAllMappings = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonRemapMapping = new System.Windows.Forms.ToolStripButton();
      this.tabPageEvents = new System.Windows.Forms.TabPage();
      this.buttonSetCommand = new System.Windows.Forms.Button();
      this.buttonAddEvent = new System.Windows.Forms.Button();
      this.labelCommand = new System.Windows.Forms.Label();
      this.comboBoxCommands = new System.Windows.Forms.ComboBox();
      this.labelEvent = new System.Windows.Forms.Label();
      this.comboBoxEvents = new System.Windows.Forms.ComboBox();
      this.listViewEventMap = new System.Windows.Forms.ListView();
      this.columnHeaderEvent = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderEventCommand = new System.Windows.Forms.ColumnHeader();
      this.contextMenuStripEvents = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.removeEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabPageMacro = new System.Windows.Forms.TabPage();
      this.toolStripMacros = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonNewMacro = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEditMacro = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteMacro = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonCreateShortcutForMacro = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonTestMacro = new System.Windows.Forms.ToolStripButton();
      this.listViewMacro = new System.Windows.Forms.ListView();
      this.tabPageIRCommands = new System.Windows.Forms.TabPage();
      this.toolStripIRCommands = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonNewIR = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEditIR = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteIR = new System.Windows.Forms.ToolStripButton();
      this.listViewIR = new System.Windows.Forms.ListView();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.checkBoxAutoRun = new System.Windows.Forms.CheckBox();
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.translatorHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.contextMenuStripButtonMapping.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.tabPagePrograms.SuspendLayout();
      this.panelPrograms.SuspendLayout();
      this.panelProgramsButtons.SuspendLayout();
      this.toolStripButtonMappings.SuspendLayout();
      this.tabPageEvents.SuspendLayout();
      this.contextMenuStripEvents.SuspendLayout();
      this.tabPageMacro.SuspendLayout();
      this.toolStripMacros.SuspendLayout();
      this.tabPageIRCommands.SuspendLayout();
      this.toolStripIRCommands.SuspendLayout();
      this.menuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // listViewPrograms
      // 
      this.listViewPrograms.Alignment = System.Windows.Forms.ListViewAlignment.Left;
      this.listViewPrograms.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.listViewPrograms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.listViewPrograms.ContextMenuStrip = this.contextMenuStripPrograms;
      this.listViewPrograms.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewPrograms.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewPrograms.HideSelection = false;
      this.listViewPrograms.LargeImageList = this.imageListPrograms;
      this.listViewPrograms.Location = new System.Drawing.Point(0, 0);
      this.listViewPrograms.MultiSelect = false;
      this.listViewPrograms.Name = "listViewPrograms";
      this.listViewPrograms.ShowGroups = false;
      this.listViewPrograms.ShowItemToolTips = true;
      this.listViewPrograms.Size = new System.Drawing.Size(470, 84);
      this.listViewPrograms.TabIndex = 0;
      this.listViewPrograms.TileSize = new System.Drawing.Size(128, 48);
      this.toolTip.SetToolTip(this.listViewPrograms, "Choose a Program to modify mappings");
      this.listViewPrograms.UseCompatibleStateImageBehavior = false;
      this.listViewPrograms.DoubleClick += new System.EventHandler(this.listViewPrograms_DoubleClick);
      this.listViewPrograms.SelectedIndexChanged += new System.EventHandler(this.listViewPrograms_SelectedIndexChanged);
      // 
      // contextMenuStripPrograms
      // 
      this.contextMenuStripPrograms.Name = "contextMenuStripPrograms";
      this.contextMenuStripPrograms.Size = new System.Drawing.Size(61, 4);
      this.contextMenuStripPrograms.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripPrograms_Opening);
      // 
      // imageListPrograms
      // 
      this.imageListPrograms.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.imageListPrograms.ImageSize = new System.Drawing.Size(32, 32);
      this.imageListPrograms.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // listViewButtons
      // 
      this.listViewButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewButtons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderButton,
            this.columnHeaderDescription,
            this.columnHeaderCommand});
      this.listViewButtons.ContextMenuStrip = this.contextMenuStripButtonMapping;
      this.listViewButtons.FullRowSelect = true;
      this.listViewButtons.GridLines = true;
      this.listViewButtons.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewButtons.HideSelection = false;
      this.listViewButtons.Location = new System.Drawing.Point(8, 104);
      this.listViewButtons.MultiSelect = false;
      this.listViewButtons.Name = "listViewButtons";
      this.listViewButtons.ShowGroups = false;
      this.listViewButtons.Size = new System.Drawing.Size(504, 232);
      this.listViewButtons.TabIndex = 1;
      this.listViewButtons.UseCompatibleStateImageBehavior = false;
      this.listViewButtons.View = System.Windows.Forms.View.Details;
      this.listViewButtons.DoubleClick += new System.EventHandler(this.listViewButtons_DoubleClick);
      this.listViewButtons.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewButtons_KeyDown);
      // 
      // columnHeaderButton
      // 
      this.columnHeaderButton.Text = "Code";
      this.columnHeaderButton.Width = 100;
      // 
      // columnHeaderDescription
      // 
      this.columnHeaderDescription.Text = "Description";
      this.columnHeaderDescription.Width = 180;
      // 
      // columnHeaderCommand
      // 
      this.columnHeaderCommand.Text = "Command";
      this.columnHeaderCommand.Width = 200;
      // 
      // contextMenuStripButtonMapping
      // 
      this.contextMenuStripButtonMapping.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButtonToolStripMenuItem,
            this.editButtonToolStripMenuItem,
            this.deleteButtonToolStripMenuItem,
            this.clearButtonsToolStripMenuItem,
            this.remapButtonToolStripMenuItem,
            this.toolStripSeparator3,
            this.copyButtonsFromToolStripMenuItem});
      this.contextMenuStripButtonMapping.Name = "contextMenuStripButtonMapping";
      this.contextMenuStripButtonMapping.Size = new System.Drawing.Size(151, 142);
      this.contextMenuStripButtonMapping.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripButtonMapping_Opening);
      // 
      // newButtonToolStripMenuItem
      // 
      this.newButtonToolStripMenuItem.Image = global::Translator.Properties.Resources.Plus;
      this.newButtonToolStripMenuItem.Name = "newButtonToolStripMenuItem";
      this.newButtonToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.newButtonToolStripMenuItem.Text = "&New";
      this.newButtonToolStripMenuItem.Click += new System.EventHandler(this.newButtonToolStripMenuItem_Click);
      // 
      // editButtonToolStripMenuItem
      // 
      this.editButtonToolStripMenuItem.Image = global::Translator.Properties.Resources.Edit;
      this.editButtonToolStripMenuItem.Name = "editButtonToolStripMenuItem";
      this.editButtonToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.editButtonToolStripMenuItem.Text = "&Edit";
      this.editButtonToolStripMenuItem.Click += new System.EventHandler(this.editButtonToolStripMenuItem_Click);
      // 
      // deleteButtonToolStripMenuItem
      // 
      this.deleteButtonToolStripMenuItem.Image = global::Translator.Properties.Resources.Delete;
      this.deleteButtonToolStripMenuItem.Name = "deleteButtonToolStripMenuItem";
      this.deleteButtonToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.deleteButtonToolStripMenuItem.Text = "&Delete";
      this.deleteButtonToolStripMenuItem.Click += new System.EventHandler(this.deleteButtonToolStripMenuItem_Click);
      // 
      // clearButtonsToolStripMenuItem
      // 
      this.clearButtonsToolStripMenuItem.Image = global::Translator.Properties.Resources.DeleteAll;
      this.clearButtonsToolStripMenuItem.Name = "clearButtonsToolStripMenuItem";
      this.clearButtonsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.clearButtonsToolStripMenuItem.Text = "&Clear";
      this.clearButtonsToolStripMenuItem.Click += new System.EventHandler(this.clearButtonsToolStripMenuItem_Click);
      // 
      // remapButtonToolStripMenuItem
      // 
      this.remapButtonToolStripMenuItem.Image = global::Translator.Properties.Resources.Remap;
      this.remapButtonToolStripMenuItem.Name = "remapButtonToolStripMenuItem";
      this.remapButtonToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.remapButtonToolStripMenuItem.Text = "&Remap";
      this.remapButtonToolStripMenuItem.Click += new System.EventHandler(this.remapButtonToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
      // 
      // copyButtonsFromToolStripMenuItem
      // 
      this.copyButtonsFromToolStripMenuItem.Image = global::Translator.Properties.Resources.MoveRight;
      this.copyButtonsFromToolStripMenuItem.Name = "copyButtonsFromToolStripMenuItem";
      this.copyButtonsFromToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.copyButtonsFromToolStripMenuItem.Text = "Copy &from ...";
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOK.Location = new System.Drawing.Point(472, 440);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.tabPagePrograms);
      this.tabControl.Controls.Add(this.tabPageEvents);
      this.tabControl.Controls.Add(this.tabPageMacro);
      this.tabControl.Controls.Add(this.tabPageIRCommands);
      this.tabControl.Location = new System.Drawing.Point(8, 32);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(528, 400);
      this.tabControl.TabIndex = 1;
      this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
      // 
      // tabPagePrograms
      // 
      this.tabPagePrograms.Controls.Add(this.panelPrograms);
      this.tabPagePrograms.Controls.Add(this.listViewButtons);
      this.tabPagePrograms.Controls.Add(this.toolStripButtonMappings);
      this.tabPagePrograms.Location = new System.Drawing.Point(4, 22);
      this.tabPagePrograms.Name = "tabPagePrograms";
      this.tabPagePrograms.Padding = new System.Windows.Forms.Padding(3);
      this.tabPagePrograms.Size = new System.Drawing.Size(520, 374);
      this.tabPagePrograms.TabIndex = 0;
      this.tabPagePrograms.Text = "Programs";
      this.tabPagePrograms.UseVisualStyleBackColor = true;
      // 
      // panelPrograms
      // 
      this.panelPrograms.BackColor = System.Drawing.SystemColors.Window;
      this.panelPrograms.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelPrograms.Controls.Add(this.listViewPrograms);
      this.panelPrograms.Controls.Add(this.panelProgramsButtons);
      this.panelPrograms.Location = new System.Drawing.Point(8, 8);
      this.panelPrograms.Name = "panelPrograms";
      this.panelPrograms.Size = new System.Drawing.Size(504, 88);
      this.panelPrograms.TabIndex = 3;
      // 
      // panelProgramsButtons
      // 
      this.panelProgramsButtons.Controls.Add(this.labelProgramsDelete);
      this.panelProgramsButtons.Controls.Add(this.labelProgramsEdit);
      this.panelProgramsButtons.Controls.Add(this.labelProgramsAdd);
      this.panelProgramsButtons.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelProgramsButtons.Location = new System.Drawing.Point(470, 0);
      this.panelProgramsButtons.Name = "panelProgramsButtons";
      this.panelProgramsButtons.Size = new System.Drawing.Size(30, 84);
      this.panelProgramsButtons.TabIndex = 1;
      // 
      // labelProgramsDelete
      // 
      this.labelProgramsDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.labelProgramsDelete.Cursor = System.Windows.Forms.Cursors.Hand;
      this.labelProgramsDelete.Image = global::Translator.Properties.Resources.Delete;
      this.labelProgramsDelete.Location = new System.Drawing.Point(1, 52);
      this.labelProgramsDelete.Name = "labelProgramsDelete";
      this.labelProgramsDelete.Size = new System.Drawing.Size(24, 24);
      this.labelProgramsDelete.TabIndex = 2;
      this.labelProgramsDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTip.SetToolTip(this.labelProgramsDelete, "Remove program");
      // 
      // labelProgramsEdit
      // 
      this.labelProgramsEdit.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.labelProgramsEdit.Cursor = System.Windows.Forms.Cursors.Hand;
      this.labelProgramsEdit.Image = global::Translator.Properties.Resources.Edit;
      this.labelProgramsEdit.Location = new System.Drawing.Point(1, 27);
      this.labelProgramsEdit.Name = "labelProgramsEdit";
      this.labelProgramsEdit.Size = new System.Drawing.Size(24, 24);
      this.labelProgramsEdit.TabIndex = 1;
      this.labelProgramsEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTip.SetToolTip(this.labelProgramsEdit, "Edit program");
      // 
      // labelProgramsAdd
      // 
      this.labelProgramsAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.labelProgramsAdd.Cursor = System.Windows.Forms.Cursors.Hand;
      this.labelProgramsAdd.Image = global::Translator.Properties.Resources.Plus;
      this.labelProgramsAdd.Location = new System.Drawing.Point(1, 2);
      this.labelProgramsAdd.Name = "labelProgramsAdd";
      this.labelProgramsAdd.Size = new System.Drawing.Size(24, 24);
      this.labelProgramsAdd.TabIndex = 0;
      this.labelProgramsAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTip.SetToolTip(this.labelProgramsAdd, "Add program");
      // 
      // toolStripButtonMappings
      // 
      this.toolStripButtonMappings.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.toolStripButtonMappings.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStripButtonMappings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewMapping,
            this.toolStripButtonEditMapping,
            this.toolStripButtonDeleteMapping,
            this.toolStripButtonDeleteAllMappings,
            this.toolStripButtonRemapMapping});
      this.toolStripButtonMappings.Location = new System.Drawing.Point(3, 346);
      this.toolStripButtonMappings.Name = "toolStripButtonMappings";
      this.toolStripButtonMappings.Size = new System.Drawing.Size(514, 25);
      this.toolStripButtonMappings.TabIndex = 2;
      this.toolStripButtonMappings.Text = "Button Mappings";
      // 
      // toolStripButtonNewMapping
      // 
      this.toolStripButtonNewMapping.Image = global::Translator.Properties.Resources.Plus;
      this.toolStripButtonNewMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonNewMapping.Name = "toolStripButtonNewMapping";
      this.toolStripButtonNewMapping.Size = new System.Drawing.Size(48, 22);
      this.toolStripButtonNewMapping.Text = "New";
      this.toolStripButtonNewMapping.ToolTipText = "Create a new button mapping";
      this.toolStripButtonNewMapping.Click += new System.EventHandler(this.toolStripButtonNewMapping_Click);
      // 
      // toolStripButtonEditMapping
      // 
      this.toolStripButtonEditMapping.Image = global::Translator.Properties.Resources.Edit;
      this.toolStripButtonEditMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEditMapping.Name = "toolStripButtonEditMapping";
      this.toolStripButtonEditMapping.Size = new System.Drawing.Size(45, 22);
      this.toolStripButtonEditMapping.Text = "Edit";
      this.toolStripButtonEditMapping.ToolTipText = "Edit the selected button mapping";
      this.toolStripButtonEditMapping.Click += new System.EventHandler(this.toolStripButtonEditMapping_Click);
      // 
      // toolStripButtonDeleteMapping
      // 
      this.toolStripButtonDeleteMapping.Image = global::Translator.Properties.Resources.Delete;
      this.toolStripButtonDeleteMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteMapping.Name = "toolStripButtonDeleteMapping";
      this.toolStripButtonDeleteMapping.Size = new System.Drawing.Size(58, 22);
      this.toolStripButtonDeleteMapping.Text = "Delete";
      this.toolStripButtonDeleteMapping.ToolTipText = "Delete the selected button mapping";
      this.toolStripButtonDeleteMapping.Click += new System.EventHandler(this.toolStripButtonDeleteMapping_Click);
      // 
      // toolStripButtonDeleteAllMappings
      // 
      this.toolStripButtonDeleteAllMappings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripButtonDeleteAllMappings.Image = global::Translator.Properties.Resources.DeleteAll;
      this.toolStripButtonDeleteAllMappings.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteAllMappings.Name = "toolStripButtonDeleteAllMappings";
      this.toolStripButtonDeleteAllMappings.Size = new System.Drawing.Size(72, 22);
      this.toolStripButtonDeleteAllMappings.Text = "Delete All";
      this.toolStripButtonDeleteAllMappings.ToolTipText = "Delete all the button mappings";
      this.toolStripButtonDeleteAllMappings.Click += new System.EventHandler(this.toolStripButtonDeleteAllMappings_Click);
      // 
      // toolStripButtonRemapMapping
      // 
      this.toolStripButtonRemapMapping.Image = global::Translator.Properties.Resources.Remap;
      this.toolStripButtonRemapMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonRemapMapping.Name = "toolStripButtonRemapMapping";
      this.toolStripButtonRemapMapping.Size = new System.Drawing.Size(60, 22);
      this.toolStripButtonRemapMapping.Text = "Remap";
      this.toolStripButtonRemapMapping.ToolTipText = "Link a new code to the selected mapping";
      this.toolStripButtonRemapMapping.Click += new System.EventHandler(this.toolStripButtonRemapMapping_Click);
      // 
      // tabPageEvents
      // 
      this.tabPageEvents.Controls.Add(this.buttonSetCommand);
      this.tabPageEvents.Controls.Add(this.buttonAddEvent);
      this.tabPageEvents.Controls.Add(this.labelCommand);
      this.tabPageEvents.Controls.Add(this.comboBoxCommands);
      this.tabPageEvents.Controls.Add(this.labelEvent);
      this.tabPageEvents.Controls.Add(this.comboBoxEvents);
      this.tabPageEvents.Controls.Add(this.listViewEventMap);
      this.tabPageEvents.Location = new System.Drawing.Point(4, 22);
      this.tabPageEvents.Name = "tabPageEvents";
      this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageEvents.Size = new System.Drawing.Size(520, 374);
      this.tabPageEvents.TabIndex = 1;
      this.tabPageEvents.Text = "Events";
      this.tabPageEvents.UseVisualStyleBackColor = true;
      // 
      // buttonSetCommand
      // 
      this.buttonSetCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSetCommand.Location = new System.Drawing.Point(456, 344);
      this.buttonSetCommand.Name = "buttonSetCommand";
      this.buttonSetCommand.Size = new System.Drawing.Size(56, 24);
      this.buttonSetCommand.TabIndex = 6;
      this.buttonSetCommand.Text = "Set";
      this.buttonSetCommand.UseVisualStyleBackColor = true;
      this.buttonSetCommand.Click += new System.EventHandler(this.buttonSetCommand_Click);
      // 
      // buttonAddEvent
      // 
      this.buttonAddEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAddEvent.Location = new System.Drawing.Point(456, 312);
      this.buttonAddEvent.Name = "buttonAddEvent";
      this.buttonAddEvent.Size = new System.Drawing.Size(56, 24);
      this.buttonAddEvent.TabIndex = 3;
      this.buttonAddEvent.Text = "Add";
      this.buttonAddEvent.UseVisualStyleBackColor = true;
      this.buttonAddEvent.Click += new System.EventHandler(this.buttonAddEvent_Click);
      // 
      // labelCommand
      // 
      this.labelCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelCommand.Location = new System.Drawing.Point(8, 344);
      this.labelCommand.Name = "labelCommand";
      this.labelCommand.Size = new System.Drawing.Size(80, 20);
      this.labelCommand.TabIndex = 4;
      this.labelCommand.Text = "Command:";
      this.labelCommand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxCommands
      // 
      this.comboBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxCommands.FormattingEnabled = true;
      this.comboBoxCommands.Location = new System.Drawing.Point(88, 344);
      this.comboBoxCommands.Name = "comboBoxCommands";
      this.comboBoxCommands.Size = new System.Drawing.Size(352, 21);
      this.comboBoxCommands.TabIndex = 5;
      // 
      // labelEvent
      // 
      this.labelEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelEvent.Location = new System.Drawing.Point(8, 312);
      this.labelEvent.Name = "labelEvent";
      this.labelEvent.Size = new System.Drawing.Size(80, 21);
      this.labelEvent.TabIndex = 1;
      this.labelEvent.Text = "New event:";
      this.labelEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxEvents
      // 
      this.comboBoxEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEvents.FormattingEnabled = true;
      this.comboBoxEvents.Location = new System.Drawing.Point(88, 312);
      this.comboBoxEvents.Name = "comboBoxEvents";
      this.comboBoxEvents.Size = new System.Drawing.Size(352, 21);
      this.comboBoxEvents.TabIndex = 2;
      // 
      // listViewEventMap
      // 
      this.listViewEventMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewEventMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderEvent,
            this.columnHeaderEventCommand});
      this.listViewEventMap.ContextMenuStrip = this.contextMenuStripEvents;
      this.listViewEventMap.FullRowSelect = true;
      this.listViewEventMap.GridLines = true;
      this.listViewEventMap.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewEventMap.HideSelection = false;
      this.listViewEventMap.Location = new System.Drawing.Point(8, 8);
      this.listViewEventMap.Name = "listViewEventMap";
      this.listViewEventMap.ShowGroups = false;
      this.listViewEventMap.Size = new System.Drawing.Size(504, 296);
      this.listViewEventMap.TabIndex = 0;
      this.listViewEventMap.UseCompatibleStateImageBehavior = false;
      this.listViewEventMap.View = System.Windows.Forms.View.Details;
      this.listViewEventMap.DoubleClick += new System.EventHandler(this.listViewEventMap_DoubleClick);
      this.listViewEventMap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewEventMap_KeyDown);
      // 
      // columnHeaderEvent
      // 
      this.columnHeaderEvent.Text = "Event";
      this.columnHeaderEvent.Width = 200;
      // 
      // columnHeaderEventCommand
      // 
      this.columnHeaderEventCommand.Text = "Command";
      this.columnHeaderEventCommand.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.columnHeaderEventCommand.Width = 280;
      // 
      // contextMenuStripEvents
      // 
      this.contextMenuStripEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeEventToolStripMenuItem});
      this.contextMenuStripEvents.Name = "contextMenuStripEvents";
      this.contextMenuStripEvents.Size = new System.Drawing.Size(125, 26);
      // 
      // removeEventToolStripMenuItem
      // 
      this.removeEventToolStripMenuItem.Image = global::Translator.Properties.Resources.Delete;
      this.removeEventToolStripMenuItem.Name = "removeEventToolStripMenuItem";
      this.removeEventToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
      this.removeEventToolStripMenuItem.Text = "&Remove";
      this.removeEventToolStripMenuItem.Click += new System.EventHandler(this.removeEventToolStripMenuItem_Click);
      // 
      // tabPageMacro
      // 
      this.tabPageMacro.Controls.Add(this.toolStripMacros);
      this.tabPageMacro.Controls.Add(this.listViewMacro);
      this.tabPageMacro.Location = new System.Drawing.Point(4, 22);
      this.tabPageMacro.Name = "tabPageMacro";
      this.tabPageMacro.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMacro.Size = new System.Drawing.Size(520, 374);
      this.tabPageMacro.TabIndex = 2;
      this.tabPageMacro.Text = "Macros";
      this.tabPageMacro.UseVisualStyleBackColor = true;
      // 
      // toolStripMacros
      // 
      this.toolStripMacros.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.toolStripMacros.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStripMacros.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewMacro,
            this.toolStripButtonEditMacro,
            this.toolStripButtonDeleteMacro,
            this.toolStripButtonCreateShortcutForMacro,
            this.toolStripButtonTestMacro});
      this.toolStripMacros.Location = new System.Drawing.Point(3, 346);
      this.toolStripMacros.Name = "toolStripMacros";
      this.toolStripMacros.Size = new System.Drawing.Size(514, 25);
      this.toolStripMacros.TabIndex = 1;
      this.toolStripMacros.Text = "Macros";
      // 
      // toolStripButtonNewMacro
      // 
      this.toolStripButtonNewMacro.Image = global::Translator.Properties.Resources.Plus;
      this.toolStripButtonNewMacro.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonNewMacro.Name = "toolStripButtonNewMacro";
      this.toolStripButtonNewMacro.Size = new System.Drawing.Size(48, 22);
      this.toolStripButtonNewMacro.Text = "New";
      this.toolStripButtonNewMacro.ToolTipText = "Create a new macro";
      this.toolStripButtonNewMacro.Click += new System.EventHandler(this.toolStripButtonNewMacro_Click);
      // 
      // toolStripButtonEditMacro
      // 
      this.toolStripButtonEditMacro.Image = global::Translator.Properties.Resources.Edit;
      this.toolStripButtonEditMacro.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEditMacro.Name = "toolStripButtonEditMacro";
      this.toolStripButtonEditMacro.Size = new System.Drawing.Size(45, 22);
      this.toolStripButtonEditMacro.Text = "Edit";
      this.toolStripButtonEditMacro.ToolTipText = "Edit the selected macro";
      this.toolStripButtonEditMacro.Click += new System.EventHandler(this.toolStripButtonEditMacro_Click);
      // 
      // toolStripButtonDeleteMacro
      // 
      this.toolStripButtonDeleteMacro.Image = global::Translator.Properties.Resources.Delete;
      this.toolStripButtonDeleteMacro.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteMacro.Name = "toolStripButtonDeleteMacro";
      this.toolStripButtonDeleteMacro.Size = new System.Drawing.Size(58, 22);
      this.toolStripButtonDeleteMacro.Text = "Delete";
      this.toolStripButtonDeleteMacro.ToolTipText = "Delete the selected macro";
      this.toolStripButtonDeleteMacro.Click += new System.EventHandler(this.toolStripButtonDeleteMacro_Click);
      // 
      // toolStripButtonCreateShortcutForMacro
      // 
      this.toolStripButtonCreateShortcutForMacro.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripButtonCreateShortcutForMacro.Image = global::Translator.Properties.Resources.Shortcut;
      this.toolStripButtonCreateShortcutForMacro.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonCreateShortcutForMacro.Name = "toolStripButtonCreateShortcutForMacro";
      this.toolStripButtonCreateShortcutForMacro.Size = new System.Drawing.Size(103, 22);
      this.toolStripButtonCreateShortcutForMacro.Text = "Create shortcut";
      this.toolStripButtonCreateShortcutForMacro.ToolTipText = "Create a shortcut to run the selected macro";
      this.toolStripButtonCreateShortcutForMacro.Click += new System.EventHandler(this.toolStripButtonCreateShortcutForMacro_Click);
      // 
      // toolStripButtonTestMacro
      // 
      this.toolStripButtonTestMacro.Image = global::Translator.Properties.Resources.MoveRight;
      this.toolStripButtonTestMacro.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonTestMacro.Name = "toolStripButtonTestMacro";
      this.toolStripButtonTestMacro.Size = new System.Drawing.Size(48, 22);
      this.toolStripButtonTestMacro.Text = "Test";
      this.toolStripButtonTestMacro.ToolTipText = "Test the selected macro";
      this.toolStripButtonTestMacro.Click += new System.EventHandler(this.toolStripButtonTestMacro_Click);
      // 
      // listViewMacro
      // 
      this.listViewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewMacro.FullRowSelect = true;
      this.listViewMacro.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewMacro.HideSelection = false;
      this.listViewMacro.LabelEdit = true;
      this.listViewMacro.Location = new System.Drawing.Point(8, 8);
      this.listViewMacro.MultiSelect = false;
      this.listViewMacro.Name = "listViewMacro";
      this.listViewMacro.ShowGroups = false;
      this.listViewMacro.Size = new System.Drawing.Size(504, 328);
      this.listViewMacro.TabIndex = 0;
      this.listViewMacro.UseCompatibleStateImageBehavior = false;
      this.listViewMacro.View = System.Windows.Forms.View.List;
      this.listViewMacro.DoubleClick += new System.EventHandler(this.listViewMacro_DoubleClick);
      this.listViewMacro.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMacro_AfterLabelEdit);
      // 
      // tabPageIRCommands
      // 
      this.tabPageIRCommands.Controls.Add(this.toolStripIRCommands);
      this.tabPageIRCommands.Controls.Add(this.listViewIR);
      this.tabPageIRCommands.Location = new System.Drawing.Point(4, 22);
      this.tabPageIRCommands.Name = "tabPageIRCommands";
      this.tabPageIRCommands.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageIRCommands.Size = new System.Drawing.Size(520, 374);
      this.tabPageIRCommands.TabIndex = 3;
      this.tabPageIRCommands.Text = "IR Commands";
      this.tabPageIRCommands.UseVisualStyleBackColor = true;
      // 
      // toolStripIRCommands
      // 
      this.toolStripIRCommands.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.toolStripIRCommands.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStripIRCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewIR,
            this.toolStripButtonEditIR,
            this.toolStripButtonDeleteIR});
      this.toolStripIRCommands.Location = new System.Drawing.Point(3, 346);
      this.toolStripIRCommands.Name = "toolStripIRCommands";
      this.toolStripIRCommands.Size = new System.Drawing.Size(514, 25);
      this.toolStripIRCommands.TabIndex = 1;
      this.toolStripIRCommands.Text = "IR Commands";
      // 
      // toolStripButtonNewIR
      // 
      this.toolStripButtonNewIR.Image = global::Translator.Properties.Resources.Plus;
      this.toolStripButtonNewIR.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonNewIR.Name = "toolStripButtonNewIR";
      this.toolStripButtonNewIR.Size = new System.Drawing.Size(48, 22);
      this.toolStripButtonNewIR.Text = "New";
      this.toolStripButtonNewIR.ToolTipText = "Create a new IR Command";
      this.toolStripButtonNewIR.Click += new System.EventHandler(this.toolStripButtonNewIR_Click);
      // 
      // toolStripButtonEditIR
      // 
      this.toolStripButtonEditIR.Image = global::Translator.Properties.Resources.Edit;
      this.toolStripButtonEditIR.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEditIR.Name = "toolStripButtonEditIR";
      this.toolStripButtonEditIR.Size = new System.Drawing.Size(45, 22);
      this.toolStripButtonEditIR.Text = "Edit";
      this.toolStripButtonEditIR.ToolTipText = "Edit the selected IR Command";
      this.toolStripButtonEditIR.Click += new System.EventHandler(this.toolStripButtonEditIR_Click);
      // 
      // toolStripButtonDeleteIR
      // 
      this.toolStripButtonDeleteIR.Image = global::Translator.Properties.Resources.Delete;
      this.toolStripButtonDeleteIR.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteIR.Name = "toolStripButtonDeleteIR";
      this.toolStripButtonDeleteIR.Size = new System.Drawing.Size(58, 22);
      this.toolStripButtonDeleteIR.Text = "Delete";
      this.toolStripButtonDeleteIR.ToolTipText = "Delete the selected IR Command";
      this.toolStripButtonDeleteIR.Click += new System.EventHandler(this.toolStripButtonDeleteIR_Click);
      // 
      // listViewIR
      // 
      this.listViewIR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewIR.FullRowSelect = true;
      this.listViewIR.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewIR.HideSelection = false;
      this.listViewIR.LabelEdit = true;
      this.listViewIR.Location = new System.Drawing.Point(8, 8);
      this.listViewIR.MultiSelect = false;
      this.listViewIR.Name = "listViewIR";
      this.listViewIR.ShowGroups = false;
      this.listViewIR.Size = new System.Drawing.Size(504, 328);
      this.listViewIR.TabIndex = 0;
      this.listViewIR.UseCompatibleStateImageBehavior = false;
      this.listViewIR.View = System.Windows.Forms.View.List;
      this.listViewIR.DoubleClick += new System.EventHandler(this.listViewIR_DoubleClick);
      this.listViewIR.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewIR_AfterLabelEdit);
      // 
      // checkBoxAutoRun
      // 
      this.checkBoxAutoRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxAutoRun.AutoSize = true;
      this.checkBoxAutoRun.Location = new System.Drawing.Point(16, 440);
      this.checkBoxAutoRun.Name = "checkBoxAutoRun";
      this.checkBoxAutoRun.Size = new System.Drawing.Size(167, 17);
      this.checkBoxAutoRun.TabIndex = 2;
      this.checkBoxAutoRun.Text = "&Start Translator with Windows";
      this.toolTip.SetToolTip(this.checkBoxAutoRun, "Set this to make Translator automatically start when you turn the computer on");
      this.checkBoxAutoRun.UseVisualStyleBackColor = true;
      this.checkBoxAutoRun.CheckedChanged += new System.EventHandler(this.checkBoxAutoRun_CheckedChanged);
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(544, 24);
      this.menuStrip.TabIndex = 0;
      this.menuStrip.Text = "menuStrip";
      // 
      // configurationToolStripMenuItem
      // 
      this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator1,
            this.serverToolStripMenuItem,
            this.toolStripSeparator2,
            this.quitToolStripMenuItem});
      this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
      this.configurationToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.configurationToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.newToolStripMenuItem.Text = "&New";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.openToolStripMenuItem.Text = "&Open ...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // importToolStripMenuItem
      // 
      this.importToolStripMenuItem.Name = "importToolStripMenuItem";
      this.importToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.importToolStripMenuItem.Text = "&Import ...";
      this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
      // 
      // exportToolStripMenuItem
      // 
      this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
      this.exportToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.exportToolStripMenuItem.Text = "&Export ...";
      this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
      // 
      // serverToolStripMenuItem
      // 
      this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
      this.serverToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.serverToolStripMenuItem.Text = "&Server ...";
      this.serverToolStripMenuItem.Click += new System.EventHandler(this.serverToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(129, 6);
      // 
      // quitToolStripMenuItem
      // 
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.quitToolStripMenuItem.Text = "&Quit";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.translatorHelpToolStripMenuItem,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // translatorHelpToolStripMenuItem
      // 
      this.translatorHelpToolStripMenuItem.Name = "translatorHelpToolStripMenuItem";
      this.translatorHelpToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.translatorHelpToolStripMenuItem.Text = "&Contents";
      this.translatorHelpToolStripMenuItem.Click += new System.EventHandler(this.translatorHelpToolStripMenuItem_Click);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.aboutToolStripMenuItem.Text = "&About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // openFileDialog
      // 
      this.openFileDialog.Filter = "XML Files|*.xml";
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.Filter = "XML Files|*.xml";
      this.saveFileDialog.Title = "Export settings ...";
      // 
      // MainForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(544, 472);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.checkBoxAutoRun);
      this.Controls.Add(this.menuStrip);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(472, 448);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Translator";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.contextMenuStripButtonMapping.ResumeLayout(false);
      this.tabControl.ResumeLayout(false);
      this.tabPagePrograms.ResumeLayout(false);
      this.tabPagePrograms.PerformLayout();
      this.panelPrograms.ResumeLayout(false);
      this.panelProgramsButtons.ResumeLayout(false);
      this.toolStripButtonMappings.ResumeLayout(false);
      this.toolStripButtonMappings.PerformLayout();
      this.tabPageEvents.ResumeLayout(false);
      this.contextMenuStripEvents.ResumeLayout(false);
      this.tabPageMacro.ResumeLayout(false);
      this.tabPageMacro.PerformLayout();
      this.toolStripMacros.ResumeLayout(false);
      this.toolStripMacros.PerformLayout();
      this.tabPageIRCommands.ResumeLayout(false);
      this.tabPageIRCommands.PerformLayout();
      this.toolStripIRCommands.ResumeLayout(false);
      this.toolStripIRCommands.PerformLayout();
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView listViewButtons;
    private System.Windows.Forms.ColumnHeader columnHeaderButton;
    private System.Windows.Forms.ColumnHeader columnHeaderCommand;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPagePrograms;
    private System.Windows.Forms.TabPage tabPageIRCommands;
    private System.Windows.Forms.TabPage tabPageMacro;
    private System.Windows.Forms.ColumnHeader columnHeaderDescription;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.CheckBox checkBoxAutoRun;
    private System.Windows.Forms.TabPage tabPageEvents;
    private System.Windows.Forms.Label labelEvent;
    private System.Windows.Forms.ComboBox comboBoxEvents;
    private System.Windows.Forms.ListView listViewEventMap;
    private System.Windows.Forms.ColumnHeader columnHeaderEvent;
    private System.Windows.Forms.ColumnHeader columnHeaderEventCommand;
    private System.Windows.Forms.Button buttonSetCommand;
    private System.Windows.Forms.Button buttonAddEvent;
    private System.Windows.Forms.Label labelCommand;
    private System.Windows.Forms.ComboBox comboBoxCommands;
    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem translatorHelpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.ListView listViewIR;
    private System.Windows.Forms.ListView listViewMacro;
    private System.Windows.Forms.ListView listViewPrograms;
    private System.Windows.Forms.ImageList imageListPrograms;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripPrograms;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripEvents;
    private System.Windows.Forms.ToolStripMenuItem removeEventToolStripMenuItem;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripButtonMapping;
    private System.Windows.Forms.ToolStripMenuItem newButtonToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editButtonToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteButtonToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearButtonsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem copyButtonsFromToolStripMenuItem;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ToolStrip toolStripButtonMappings;
    private System.Windows.Forms.ToolStripButton toolStripButtonNewMapping;
    private System.Windows.Forms.ToolStripButton toolStripButtonEditMapping;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteMapping;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteAllMappings;
    private System.Windows.Forms.ToolStrip toolStripMacros;
    private System.Windows.Forms.ToolStripButton toolStripButtonNewMacro;
    private System.Windows.Forms.ToolStripButton toolStripButtonEditMacro;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteMacro;
    private System.Windows.Forms.ToolStripButton toolStripButtonTestMacro;
    private System.Windows.Forms.ToolStripButton toolStripButtonCreateShortcutForMacro;
    private System.Windows.Forms.ToolStrip toolStripIRCommands;
    private System.Windows.Forms.ToolStripButton toolStripButtonNewIR;
    private System.Windows.Forms.ToolStripButton toolStripButtonEditIR;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteIR;
    private System.Windows.Forms.ToolStripButton toolStripButtonRemapMapping;
    private System.Windows.Forms.Panel panelPrograms;
    private System.Windows.Forms.Panel panelProgramsButtons;
    private System.Windows.Forms.Label labelProgramsDelete;
    private System.Windows.Forms.Label labelProgramsEdit;
    private System.Windows.Forms.Label labelProgramsAdd;
    private System.Windows.Forms.ToolStripMenuItem remapButtonToolStripMenuItem;
  }
}

