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
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonMapButtons = new System.Windows.Forms.Button();
      this.checkBoxRequiresFocus = new System.Windows.Forms.CheckBox();
      this.buttonNewMacro = new System.Windows.Forms.Button();
      this.buttonEditIR = new System.Windows.Forms.Button();
      this.buttonDeleteIR = new System.Windows.Forms.Button();
      this.buttonNewIR = new System.Windows.Forms.Button();
      this.buttonTestMacro = new System.Windows.Forms.Button();
      this.buttonDeleteMacro = new System.Windows.Forms.Button();
      this.buttonEditMacro = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.checkBoxLogVerbose = new System.Windows.Forms.CheckBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.checkBoxMultiMapping = new System.Windows.Forms.CheckBox();
      this.checkBoxEventMapper = new System.Windows.Forms.CheckBox();
      this.buttonSetCommand = new System.Windows.Forms.Button();
      this.buttonClearEventParams = new System.Windows.Forms.Button();
      this.buttonAddEvent = new System.Windows.Forms.Button();
      this.buttonDown = new System.Windows.Forms.Button();
      this.buttonUp = new System.Windows.Forms.Button();
      this.buttonEdit = new System.Windows.Forms.Button();
      this.buttonRemove = new System.Windows.Forms.Button();
      this.buttonNew = new System.Windows.Forms.Button();
      this.comboBoxMultiButton = new System.Windows.Forms.ComboBox();
      this.comboBoxCommands = new System.Windows.Forms.ComboBox();
      this.textBoxParamValue = new System.Windows.Forms.TextBox();
      this.comboBoxParameter = new System.Windows.Forms.ComboBox();
      this.comboBoxEvents = new System.Windows.Forms.ComboBox();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.checkBoxMouseMode = new System.Windows.Forms.CheckBox();
      this.comboBoxMouseModeButton = new System.Windows.Forms.ComboBox();
      this.numericUpDownMouseStep = new System.Windows.Forms.NumericUpDown();
      this.checkBoxMouseAcceleration = new System.Windows.Forms.CheckBox();
      this.buttonClearAll = new System.Windows.Forms.Button();
      this.buttonLoadPreset = new System.Windows.Forms.Button();
      this.buttonChangeServer = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageRemotes = new System.Windows.Forms.TabPage();
      this.comboBoxRemotePresets = new System.Windows.Forms.ComboBox();
      this.groupBoxStatus = new System.Windows.Forms.GroupBox();
      this.labelStatus = new System.Windows.Forms.Label();
      this.treeViewRemotes = new System.Windows.Forms.TreeView();
      this.tabPageIR = new System.Windows.Forms.TabPage();
      this.listViewIR = new System.Windows.Forms.ListView();
      this.tabPageMacros = new System.Windows.Forms.TabPage();
      this.listViewMacro = new System.Windows.Forms.ListView();
      this.tabPageMultiMapping = new System.Windows.Forms.TabPage();
      this.listBoxMappings = new System.Windows.Forms.ListBox();
      this.labelButton = new System.Windows.Forms.Label();
      this.tabPageEventMapper = new System.Windows.Forms.TabPage();
      this.labelCommand = new System.Windows.Forms.Label();
      this.labelEvent = new System.Windows.Forms.Label();
      this.labelValue = new System.Windows.Forms.Label();
      this.labelParameter = new System.Windows.Forms.Label();
      this.listViewEventMap = new System.Windows.Forms.ListView();
      this.columnHeaderEvent = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderCommand = new System.Windows.Forms.ColumnHeader();
      this.tabPageMouseMode = new System.Windows.Forms.TabPage();
      this.groupBoxMouseModeOptions = new System.Windows.Forms.GroupBox();
      this.labelMouseModeToggle = new System.Windows.Forms.Label();
      this.labelMouseStep = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseStep)).BeginInit();
      this.tabControl.SuspendLayout();
      this.tabPageRemotes.SuspendLayout();
      this.groupBoxStatus.SuspendLayout();
      this.tabPageIR.SuspendLayout();
      this.tabPageMacros.SuspendLayout();
      this.tabPageMultiMapping.SuspendLayout();
      this.tabPageEventMapper.SuspendLayout();
      this.tabPageMouseMode.SuspendLayout();
      this.groupBoxMouseModeOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(464, 344);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 5;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonMapButtons
      // 
      this.buttonMapButtons.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.buttonMapButtons.Location = new System.Drawing.Point(344, 272);
      this.buttonMapButtons.Name = "buttonMapButtons";
      this.buttonMapButtons.Size = new System.Drawing.Size(88, 24);
      this.buttonMapButtons.TabIndex = 4;
      this.buttonMapButtons.Text = "Map Buttons";
      this.toolTips.SetToolTip(this.buttonMapButtons, "Modify mappings for the MCE remote");
      this.buttonMapButtons.UseVisualStyleBackColor = true;
      this.buttonMapButtons.Click += new System.EventHandler(this.buttonMapButtons_Click);
      // 
      // checkBoxRequiresFocus
      // 
      this.checkBoxRequiresFocus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.checkBoxRequiresFocus.Location = new System.Drawing.Point(328, 344);
      this.checkBoxRequiresFocus.Name = "checkBoxRequiresFocus";
      this.checkBoxRequiresFocus.Size = new System.Drawing.Size(104, 24);
      this.checkBoxRequiresFocus.TabIndex = 4;
      this.checkBoxRequiresFocus.Text = "&Require focus";
      this.toolTips.SetToolTip(this.checkBoxRequiresFocus, "Require that MediaPortal be in focus for remote button presses to have an effect");
      this.checkBoxRequiresFocus.UseVisualStyleBackColor = true;
      // 
      // buttonNewMacro
      // 
      this.buttonNewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewMacro.Location = new System.Drawing.Point(8, 272);
      this.buttonNewMacro.Name = "buttonNewMacro";
      this.buttonNewMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonNewMacro.TabIndex = 1;
      this.buttonNewMacro.Text = "New";
      this.toolTips.SetToolTip(this.buttonNewMacro, "Create a new Macro");
      this.buttonNewMacro.UseVisualStyleBackColor = true;
      this.buttonNewMacro.Click += new System.EventHandler(this.buttonNewMacro_Click);
      // 
      // buttonEditIR
      // 
      this.buttonEditIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEditIR.Location = new System.Drawing.Point(72, 272);
      this.buttonEditIR.Name = "buttonEditIR";
      this.buttonEditIR.Size = new System.Drawing.Size(56, 24);
      this.buttonEditIR.TabIndex = 2;
      this.buttonEditIR.Text = "Edit";
      this.toolTips.SetToolTip(this.buttonEditIR, "Re-Learn an existing IR command");
      this.buttonEditIR.UseVisualStyleBackColor = true;
      this.buttonEditIR.Click += new System.EventHandler(this.buttonEditIR_Click);
      // 
      // buttonDeleteIR
      // 
      this.buttonDeleteIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDeleteIR.Location = new System.Drawing.Point(136, 272);
      this.buttonDeleteIR.Name = "buttonDeleteIR";
      this.buttonDeleteIR.Size = new System.Drawing.Size(56, 24);
      this.buttonDeleteIR.TabIndex = 3;
      this.buttonDeleteIR.Text = "Delete";
      this.toolTips.SetToolTip(this.buttonDeleteIR, "Delete an IR command file");
      this.buttonDeleteIR.UseVisualStyleBackColor = true;
      this.buttonDeleteIR.Click += new System.EventHandler(this.buttonDeleteIR_Click);
      // 
      // buttonNewIR
      // 
      this.buttonNewIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewIR.Location = new System.Drawing.Point(8, 272);
      this.buttonNewIR.Name = "buttonNewIR";
      this.buttonNewIR.Size = new System.Drawing.Size(56, 24);
      this.buttonNewIR.TabIndex = 1;
      this.buttonNewIR.Text = "New";
      this.toolTips.SetToolTip(this.buttonNewIR, "Learn a new IR command");
      this.buttonNewIR.UseVisualStyleBackColor = true;
      this.buttonNewIR.Click += new System.EventHandler(this.buttonNewIR_Click);
      // 
      // buttonTestMacro
      // 
      this.buttonTestMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTestMacro.Location = new System.Drawing.Point(208, 272);
      this.buttonTestMacro.Name = "buttonTestMacro";
      this.buttonTestMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonTestMacro.TabIndex = 4;
      this.buttonTestMacro.Text = "Test";
      this.toolTips.SetToolTip(this.buttonTestMacro, "Test a Macro");
      this.buttonTestMacro.UseVisualStyleBackColor = true;
      this.buttonTestMacro.Click += new System.EventHandler(this.buttonTestMacro_Click);
      // 
      // buttonDeleteMacro
      // 
      this.buttonDeleteMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDeleteMacro.Location = new System.Drawing.Point(136, 272);
      this.buttonDeleteMacro.Name = "buttonDeleteMacro";
      this.buttonDeleteMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonDeleteMacro.TabIndex = 3;
      this.buttonDeleteMacro.Text = "Delete";
      this.toolTips.SetToolTip(this.buttonDeleteMacro, "Delete a Macro file");
      this.buttonDeleteMacro.UseVisualStyleBackColor = true;
      this.buttonDeleteMacro.Click += new System.EventHandler(this.buttonDeleteMacro_Click);
      // 
      // buttonEditMacro
      // 
      this.buttonEditMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEditMacro.Location = new System.Drawing.Point(72, 272);
      this.buttonEditMacro.Name = "buttonEditMacro";
      this.buttonEditMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonEditMacro.TabIndex = 2;
      this.buttonEditMacro.Text = "Edit";
      this.toolTips.SetToolTip(this.buttonEditMacro, "Edit an existing Macro");
      this.buttonEditMacro.UseVisualStyleBackColor = true;
      this.buttonEditMacro.Click += new System.EventHandler(this.buttonEditMacro_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(528, 344);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // checkBoxLogVerbose
      // 
      this.checkBoxLogVerbose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.checkBoxLogVerbose.Location = new System.Drawing.Point(192, 344);
      this.checkBoxLogVerbose.Name = "checkBoxLogVerbose";
      this.checkBoxLogVerbose.Size = new System.Drawing.Size(120, 24);
      this.checkBoxLogVerbose.TabIndex = 3;
      this.checkBoxLogVerbose.Text = "&Extended logging";
      this.toolTips.SetToolTip(this.checkBoxLogVerbose, "Enable more detailed logging of plugin operations");
      this.checkBoxLogVerbose.UseVisualStyleBackColor = true;
      // 
      // checkBoxMultiMapping
      // 
      this.checkBoxMultiMapping.Location = new System.Drawing.Point(8, 8);
      this.checkBoxMultiMapping.Name = "checkBoxMultiMapping";
      this.checkBoxMultiMapping.Size = new System.Drawing.Size(72, 24);
      this.checkBoxMultiMapping.TabIndex = 0;
      this.checkBoxMultiMapping.Text = "Enable";
      this.checkBoxMultiMapping.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTips.SetToolTip(this.checkBoxMultiMapping, "Enable the multi-mapping feature, which lets you cycle through remote button mapp" +
              "ings");
      this.checkBoxMultiMapping.UseVisualStyleBackColor = true;
      // 
      // checkBoxEventMapper
      // 
      this.checkBoxEventMapper.Location = new System.Drawing.Point(8, 8);
      this.checkBoxEventMapper.Name = "checkBoxEventMapper";
      this.checkBoxEventMapper.Size = new System.Drawing.Size(72, 24);
      this.checkBoxEventMapper.TabIndex = 0;
      this.checkBoxEventMapper.Text = "Enable";
      this.checkBoxEventMapper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTips.SetToolTip(this.checkBoxEventMapper, "Enable the event mapper, which lets you map plugin functions to MediaPortal inter" +
              "nal events");
      this.checkBoxEventMapper.UseVisualStyleBackColor = true;
      // 
      // buttonSetCommand
      // 
      this.buttonSetCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSetCommand.Location = new System.Drawing.Point(504, 272);
      this.buttonSetCommand.Name = "buttonSetCommand";
      this.buttonSetCommand.Size = new System.Drawing.Size(56, 24);
      this.buttonSetCommand.TabIndex = 12;
      this.buttonSetCommand.Text = "Set";
      this.toolTips.SetToolTip(this.buttonSetCommand, "Set this command to to the currently selected event");
      this.buttonSetCommand.UseVisualStyleBackColor = true;
      this.buttonSetCommand.Click += new System.EventHandler(this.buttonSetCommand_Click);
      // 
      // buttonClearEventParams
      // 
      this.buttonClearEventParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClearEventParams.Location = new System.Drawing.Point(504, 232);
      this.buttonClearEventParams.Name = "buttonClearEventParams";
      this.buttonClearEventParams.Size = new System.Drawing.Size(56, 24);
      this.buttonClearEventParams.TabIndex = 9;
      this.buttonClearEventParams.Text = "Clear";
      this.toolTips.SetToolTip(this.buttonClearEventParams, "Clear the parameter values");
      this.buttonClearEventParams.UseVisualStyleBackColor = true;
      this.buttonClearEventParams.Click += new System.EventHandler(this.buttonClearEventParams_Click);
      // 
      // buttonAddEvent
      // 
      this.buttonAddEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAddEvent.Location = new System.Drawing.Point(504, 168);
      this.buttonAddEvent.Name = "buttonAddEvent";
      this.buttonAddEvent.Size = new System.Drawing.Size(56, 24);
      this.buttonAddEvent.TabIndex = 4;
      this.buttonAddEvent.Text = "Add";
      this.toolTips.SetToolTip(this.buttonAddEvent, "Add this event to the list");
      this.buttonAddEvent.UseVisualStyleBackColor = true;
      this.buttonAddEvent.Click += new System.EventHandler(this.buttonAddEvent_Click);
      // 
      // buttonDown
      // 
      this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDown.Location = new System.Drawing.Point(208, 272);
      this.buttonDown.Name = "buttonDown";
      this.buttonDown.Size = new System.Drawing.Size(56, 24);
      this.buttonDown.TabIndex = 7;
      this.buttonDown.Text = "Down";
      this.toolTips.SetToolTip(this.buttonDown, "Move the currently selected mapping down the order");
      this.buttonDown.UseVisualStyleBackColor = true;
      this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
      // 
      // buttonUp
      // 
      this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonUp.Location = new System.Drawing.Point(144, 272);
      this.buttonUp.Name = "buttonUp";
      this.buttonUp.Size = new System.Drawing.Size(56, 24);
      this.buttonUp.TabIndex = 6;
      this.buttonUp.Text = "Up";
      this.toolTips.SetToolTip(this.buttonUp, "Move the currently selected mapping up the order");
      this.buttonUp.UseVisualStyleBackColor = true;
      this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
      // 
      // buttonEdit
      // 
      this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEdit.Location = new System.Drawing.Point(280, 272);
      this.buttonEdit.Name = "buttonEdit";
      this.buttonEdit.Size = new System.Drawing.Size(56, 24);
      this.buttonEdit.TabIndex = 8;
      this.buttonEdit.Text = "Edit";
      this.toolTips.SetToolTip(this.buttonEdit, "Edit the currently selected mapping");
      this.buttonEdit.UseVisualStyleBackColor = true;
      this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
      // 
      // buttonRemove
      // 
      this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonRemove.Location = new System.Drawing.Point(72, 272);
      this.buttonRemove.Name = "buttonRemove";
      this.buttonRemove.Size = new System.Drawing.Size(56, 24);
      this.buttonRemove.TabIndex = 5;
      this.buttonRemove.Text = "Remove";
      this.toolTips.SetToolTip(this.buttonRemove, "Remove the currently selected mapping");
      this.buttonRemove.UseVisualStyleBackColor = true;
      this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
      // 
      // buttonNew
      // 
      this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNew.Location = new System.Drawing.Point(8, 272);
      this.buttonNew.Name = "buttonNew";
      this.buttonNew.Size = new System.Drawing.Size(56, 24);
      this.buttonNew.TabIndex = 4;
      this.buttonNew.Text = "New";
      this.toolTips.SetToolTip(this.buttonNew, "Create a new mapping");
      this.buttonNew.UseVisualStyleBackColor = true;
      this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
      // 
      // comboBoxMultiButton
      // 
      this.comboBoxMultiButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxMultiButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxMultiButton.FormattingEnabled = true;
      this.comboBoxMultiButton.Location = new System.Drawing.Point(392, 8);
      this.comboBoxMultiButton.Name = "comboBoxMultiButton";
      this.comboBoxMultiButton.Size = new System.Drawing.Size(168, 21);
      this.comboBoxMultiButton.TabIndex = 2;
      this.toolTips.SetToolTip(this.comboBoxMultiButton, "Select the button that will be used to cycle through the mappings");
      // 
      // comboBoxCommands
      // 
      this.comboBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxCommands.FormattingEnabled = true;
      this.comboBoxCommands.Location = new System.Drawing.Point(96, 272);
      this.comboBoxCommands.Name = "comboBoxCommands";
      this.comboBoxCommands.Size = new System.Drawing.Size(400, 21);
      this.comboBoxCommands.TabIndex = 11;
      this.toolTips.SetToolTip(this.comboBoxCommands, "Select a command to execute");
      // 
      // textBoxParamValue
      // 
      this.textBoxParamValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxParamValue.Location = new System.Drawing.Point(96, 232);
      this.textBoxParamValue.Name = "textBoxParamValue";
      this.textBoxParamValue.Size = new System.Drawing.Size(400, 20);
      this.textBoxParamValue.TabIndex = 8;
      this.toolTips.SetToolTip(this.textBoxParamValue, "Parameter value to match");
      // 
      // comboBoxParameter
      // 
      this.comboBoxParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxParameter.FormattingEnabled = true;
      this.comboBoxParameter.Location = new System.Drawing.Point(96, 200);
      this.comboBoxParameter.Name = "comboBoxParameter";
      this.comboBoxParameter.Size = new System.Drawing.Size(400, 21);
      this.comboBoxParameter.TabIndex = 6;
      this.toolTips.SetToolTip(this.comboBoxParameter, "Match a parameter");
      // 
      // comboBoxEvents
      // 
      this.comboBoxEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEvents.FormattingEnabled = true;
      this.comboBoxEvents.Location = new System.Drawing.Point(96, 168);
      this.comboBoxEvents.Name = "comboBoxEvents";
      this.comboBoxEvents.Size = new System.Drawing.Size(400, 21);
      this.comboBoxEvents.TabIndex = 3;
      this.toolTips.SetToolTip(this.comboBoxEvents, "Event to map");
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(112, 344);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 24);
      this.buttonHelp.TabIndex = 2;
      this.buttonHelp.Text = "&Help";
      this.toolTips.SetToolTip(this.buttonHelp, "Click here for help");
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // checkBoxMouseMode
      // 
      this.checkBoxMouseMode.Location = new System.Drawing.Point(8, 8);
      this.checkBoxMouseMode.Name = "checkBoxMouseMode";
      this.checkBoxMouseMode.Size = new System.Drawing.Size(72, 24);
      this.checkBoxMouseMode.TabIndex = 0;
      this.checkBoxMouseMode.Text = "Enable";
      this.checkBoxMouseMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTips.SetToolTip(this.checkBoxMouseMode, "Enable the mouse mode feature, which lets you control the on-screen mouse with yo" +
              "ur remote");
      this.checkBoxMouseMode.UseVisualStyleBackColor = true;
      // 
      // comboBoxMouseModeButton
      // 
      this.comboBoxMouseModeButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxMouseModeButton.FormattingEnabled = true;
      this.comboBoxMouseModeButton.Location = new System.Drawing.Point(160, 24);
      this.comboBoxMouseModeButton.Name = "comboBoxMouseModeButton";
      this.comboBoxMouseModeButton.Size = new System.Drawing.Size(168, 21);
      this.comboBoxMouseModeButton.TabIndex = 1;
      this.toolTips.SetToolTip(this.comboBoxMouseModeButton, "Select the button that will be used to toggle mouse mode on and off");
      // 
      // numericUpDownMouseStep
      // 
      this.numericUpDownMouseStep.Location = new System.Drawing.Point(160, 64);
      this.numericUpDownMouseStep.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDownMouseStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownMouseStep.Name = "numericUpDownMouseStep";
      this.numericUpDownMouseStep.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownMouseStep.TabIndex = 3;
      this.numericUpDownMouseStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownMouseStep.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownMouseStep, "Set the distance to move the mouse cursor for each remote button press");
      this.numericUpDownMouseStep.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      // 
      // checkBoxMouseAcceleration
      // 
      this.checkBoxMouseAcceleration.Location = new System.Drawing.Point(8, 104);
      this.checkBoxMouseAcceleration.Name = "checkBoxMouseAcceleration";
      this.checkBoxMouseAcceleration.Size = new System.Drawing.Size(152, 24);
      this.checkBoxMouseAcceleration.TabIndex = 4;
      this.checkBoxMouseAcceleration.Text = "Use mouse acceleration";
      this.toolTips.SetToolTip(this.checkBoxMouseAcceleration, "Enable mouse acceleration, which makes the mouse move in larger steps when holdin" +
              "g the remote button down");
      this.checkBoxMouseAcceleration.UseVisualStyleBackColor = true;
      // 
      // buttonClearAll
      // 
      this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClearAll.Location = new System.Drawing.Point(496, 272);
      this.buttonClearAll.Name = "buttonClearAll";
      this.buttonClearAll.Size = new System.Drawing.Size(64, 24);
      this.buttonClearAll.TabIndex = 5;
      this.buttonClearAll.Text = "Clear All";
      this.toolTips.SetToolTip(this.buttonClearAll, "Clear all mappings");
      this.buttonClearAll.UseVisualStyleBackColor = true;
      this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
      // 
      // buttonLoadPreset
      // 
      this.buttonLoadPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonLoadPreset.Location = new System.Drawing.Point(8, 272);
      this.buttonLoadPreset.Name = "buttonLoadPreset";
      this.buttonLoadPreset.Size = new System.Drawing.Size(88, 24);
      this.buttonLoadPreset.TabIndex = 2;
      this.buttonLoadPreset.Text = "Add Remote:";
      this.toolTips.SetToolTip(this.buttonLoadPreset, "Clear currently selected MCE Button to Different Remote mapping");
      this.buttonLoadPreset.UseVisualStyleBackColor = true;
      this.buttonLoadPreset.Click += new System.EventHandler(this.buttonLoadPreset_Click);
      // 
      // buttonChangeServer
      // 
      this.buttonChangeServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonChangeServer.Location = new System.Drawing.Point(8, 344);
      this.buttonChangeServer.Name = "buttonChangeServer";
      this.buttonChangeServer.Size = new System.Drawing.Size(96, 24);
      this.buttonChangeServer.TabIndex = 1;
      this.buttonChangeServer.Text = "Change &Server";
      this.toolTips.SetToolTip(this.buttonChangeServer, "Change the IR Server host");
      this.buttonChangeServer.UseVisualStyleBackColor = true;
      this.buttonChangeServer.Click += new System.EventHandler(this.buttonChangeServer_Click);
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.tabPageRemotes);
      this.tabControl.Controls.Add(this.tabPageIR);
      this.tabControl.Controls.Add(this.tabPageMacros);
      this.tabControl.Controls.Add(this.tabPageMultiMapping);
      this.tabControl.Controls.Add(this.tabPageEventMapper);
      this.tabControl.Controls.Add(this.tabPageMouseMode);
      this.tabControl.Location = new System.Drawing.Point(8, 8);
      this.tabControl.Multiline = true;
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(576, 328);
      this.tabControl.TabIndex = 0;
      // 
      // tabPageRemotes
      // 
      this.tabPageRemotes.Controls.Add(this.comboBoxRemotePresets);
      this.tabPageRemotes.Controls.Add(this.buttonLoadPreset);
      this.tabPageRemotes.Controls.Add(this.buttonClearAll);
      this.tabPageRemotes.Controls.Add(this.groupBoxStatus);
      this.tabPageRemotes.Controls.Add(this.treeViewRemotes);
      this.tabPageRemotes.Controls.Add(this.buttonMapButtons);
      this.tabPageRemotes.Location = new System.Drawing.Point(4, 22);
      this.tabPageRemotes.Name = "tabPageRemotes";
      this.tabPageRemotes.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageRemotes.Size = new System.Drawing.Size(568, 302);
      this.tabPageRemotes.TabIndex = 1;
      this.tabPageRemotes.Text = "Remote Controls";
      this.tabPageRemotes.UseVisualStyleBackColor = true;
      // 
      // comboBoxRemotePresets
      // 
      this.comboBoxRemotePresets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.comboBoxRemotePresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxRemotePresets.FormattingEnabled = true;
      this.comboBoxRemotePresets.Location = new System.Drawing.Point(104, 272);
      this.comboBoxRemotePresets.Name = "comboBoxRemotePresets";
      this.comboBoxRemotePresets.Size = new System.Drawing.Size(176, 21);
      this.comboBoxRemotePresets.TabIndex = 3;
      // 
      // groupBoxStatus
      // 
      this.groupBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxStatus.Controls.Add(this.labelStatus);
      this.groupBoxStatus.Location = new System.Drawing.Point(8, 208);
      this.groupBoxStatus.Name = "groupBoxStatus";
      this.groupBoxStatus.Size = new System.Drawing.Size(552, 56);
      this.groupBoxStatus.TabIndex = 1;
      this.groupBoxStatus.TabStop = false;
      this.groupBoxStatus.Text = "Status";
      // 
      // labelStatus
      // 
      this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelStatus.BackColor = System.Drawing.Color.WhiteSmoke;
      this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.labelStatus.Location = new System.Drawing.Point(8, 16);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new System.Drawing.Size(536, 32);
      this.labelStatus.TabIndex = 0;
      this.labelStatus.Text = "Ready";
      this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // treeViewRemotes
      // 
      this.treeViewRemotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.treeViewRemotes.HideSelection = false;
      this.treeViewRemotes.Location = new System.Drawing.Point(8, 8);
      this.treeViewRemotes.Name = "treeViewRemotes";
      this.treeViewRemotes.Size = new System.Drawing.Size(552, 192);
      this.treeViewRemotes.TabIndex = 0;
      this.treeViewRemotes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewRemotes_KeyDown);
      // 
      // tabPageIR
      // 
      this.tabPageIR.Controls.Add(this.listViewIR);
      this.tabPageIR.Controls.Add(this.buttonNewIR);
      this.tabPageIR.Controls.Add(this.buttonEditIR);
      this.tabPageIR.Controls.Add(this.buttonDeleteIR);
      this.tabPageIR.Location = new System.Drawing.Point(4, 22);
      this.tabPageIR.Name = "tabPageIR";
      this.tabPageIR.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageIR.Size = new System.Drawing.Size(568, 302);
      this.tabPageIR.TabIndex = 2;
      this.tabPageIR.Text = "IR Commands";
      this.tabPageIR.UseVisualStyleBackColor = true;
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
      this.listViewIR.Size = new System.Drawing.Size(552, 256);
      this.listViewIR.TabIndex = 0;
      this.listViewIR.UseCompatibleStateImageBehavior = false;
      this.listViewIR.View = System.Windows.Forms.View.List;
      this.listViewIR.DoubleClick += new System.EventHandler(this.listViewIR_DoubleClick);
      this.listViewIR.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewIR_AfterLabelEdit);
      // 
      // tabPageMacros
      // 
      this.tabPageMacros.Controls.Add(this.listViewMacro);
      this.tabPageMacros.Controls.Add(this.buttonTestMacro);
      this.tabPageMacros.Controls.Add(this.buttonDeleteMacro);
      this.tabPageMacros.Controls.Add(this.buttonEditMacro);
      this.tabPageMacros.Controls.Add(this.buttonNewMacro);
      this.tabPageMacros.Location = new System.Drawing.Point(4, 22);
      this.tabPageMacros.Name = "tabPageMacros";
      this.tabPageMacros.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMacros.Size = new System.Drawing.Size(568, 302);
      this.tabPageMacros.TabIndex = 3;
      this.tabPageMacros.Text = "Macros";
      this.tabPageMacros.UseVisualStyleBackColor = true;
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
      this.listViewMacro.Size = new System.Drawing.Size(552, 256);
      this.listViewMacro.TabIndex = 0;
      this.listViewMacro.UseCompatibleStateImageBehavior = false;
      this.listViewMacro.View = System.Windows.Forms.View.List;
      this.listViewMacro.DoubleClick += new System.EventHandler(this.listViewMacro_DoubleClick);
      this.listViewMacro.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMacro_AfterLabelEdit);
      // 
      // tabPageMultiMapping
      // 
      this.tabPageMultiMapping.Controls.Add(this.checkBoxMultiMapping);
      this.tabPageMultiMapping.Controls.Add(this.buttonDown);
      this.tabPageMultiMapping.Controls.Add(this.buttonUp);
      this.tabPageMultiMapping.Controls.Add(this.listBoxMappings);
      this.tabPageMultiMapping.Controls.Add(this.buttonEdit);
      this.tabPageMultiMapping.Controls.Add(this.buttonRemove);
      this.tabPageMultiMapping.Controls.Add(this.buttonNew);
      this.tabPageMultiMapping.Controls.Add(this.comboBoxMultiButton);
      this.tabPageMultiMapping.Controls.Add(this.labelButton);
      this.tabPageMultiMapping.Location = new System.Drawing.Point(4, 22);
      this.tabPageMultiMapping.Name = "tabPageMultiMapping";
      this.tabPageMultiMapping.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMultiMapping.Size = new System.Drawing.Size(568, 302);
      this.tabPageMultiMapping.TabIndex = 4;
      this.tabPageMultiMapping.Text = "Multi-Mapping";
      this.tabPageMultiMapping.UseVisualStyleBackColor = true;
      // 
      // listBoxMappings
      // 
      this.listBoxMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxMappings.FormattingEnabled = true;
      this.listBoxMappings.HorizontalScrollbar = true;
      this.listBoxMappings.IntegralHeight = false;
      this.listBoxMappings.Location = new System.Drawing.Point(8, 40);
      this.listBoxMappings.Name = "listBoxMappings";
      this.listBoxMappings.Size = new System.Drawing.Size(552, 224);
      this.listBoxMappings.TabIndex = 3;
      this.listBoxMappings.DoubleClick += new System.EventHandler(this.listBoxMappings_DoubleClick);
      // 
      // labelButton
      // 
      this.labelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelButton.Location = new System.Drawing.Point(232, 8);
      this.labelButton.Name = "labelButton";
      this.labelButton.Size = new System.Drawing.Size(152, 21);
      this.labelButton.TabIndex = 1;
      this.labelButton.Text = "Mapping change button:";
      this.labelButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tabPageEventMapper
      // 
      this.tabPageEventMapper.Controls.Add(this.labelCommand);
      this.tabPageEventMapper.Controls.Add(this.buttonSetCommand);
      this.tabPageEventMapper.Controls.Add(this.comboBoxCommands);
      this.tabPageEventMapper.Controls.Add(this.labelEvent);
      this.tabPageEventMapper.Controls.Add(this.labelValue);
      this.tabPageEventMapper.Controls.Add(this.labelParameter);
      this.tabPageEventMapper.Controls.Add(this.textBoxParamValue);
      this.tabPageEventMapper.Controls.Add(this.comboBoxParameter);
      this.tabPageEventMapper.Controls.Add(this.buttonClearEventParams);
      this.tabPageEventMapper.Controls.Add(this.comboBoxEvents);
      this.tabPageEventMapper.Controls.Add(this.buttonAddEvent);
      this.tabPageEventMapper.Controls.Add(this.checkBoxEventMapper);
      this.tabPageEventMapper.Controls.Add(this.listViewEventMap);
      this.tabPageEventMapper.Location = new System.Drawing.Point(4, 22);
      this.tabPageEventMapper.Name = "tabPageEventMapper";
      this.tabPageEventMapper.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageEventMapper.Size = new System.Drawing.Size(568, 302);
      this.tabPageEventMapper.TabIndex = 5;
      this.tabPageEventMapper.Text = "Event Mapper";
      this.tabPageEventMapper.UseVisualStyleBackColor = true;
      // 
      // labelCommand
      // 
      this.labelCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelCommand.Location = new System.Drawing.Point(8, 272);
      this.labelCommand.Name = "labelCommand";
      this.labelCommand.Size = new System.Drawing.Size(80, 20);
      this.labelCommand.TabIndex = 10;
      this.labelCommand.Text = "Command:";
      this.labelCommand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelEvent
      // 
      this.labelEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelEvent.Location = new System.Drawing.Point(8, 168);
      this.labelEvent.Name = "labelEvent";
      this.labelEvent.Size = new System.Drawing.Size(80, 21);
      this.labelEvent.TabIndex = 2;
      this.labelEvent.Text = "Event:";
      this.labelEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelValue
      // 
      this.labelValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelValue.Location = new System.Drawing.Point(8, 232);
      this.labelValue.Name = "labelValue";
      this.labelValue.Size = new System.Drawing.Size(80, 20);
      this.labelValue.TabIndex = 7;
      this.labelValue.Text = "Value:";
      this.labelValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelParameter
      // 
      this.labelParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelParameter.Location = new System.Drawing.Point(8, 200);
      this.labelParameter.Name = "labelParameter";
      this.labelParameter.Size = new System.Drawing.Size(80, 21);
      this.labelParameter.TabIndex = 5;
      this.labelParameter.Text = "Parameter:";
      this.labelParameter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // listViewEventMap
      // 
      this.listViewEventMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewEventMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderEvent,
            this.columnHeaderCommand});
      this.listViewEventMap.FullRowSelect = true;
      this.listViewEventMap.GridLines = true;
      this.listViewEventMap.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewEventMap.HideSelection = false;
      this.listViewEventMap.Location = new System.Drawing.Point(8, 40);
      this.listViewEventMap.Name = "listViewEventMap";
      this.listViewEventMap.Size = new System.Drawing.Size(552, 120);
      this.listViewEventMap.TabIndex = 1;
      this.listViewEventMap.UseCompatibleStateImageBehavior = false;
      this.listViewEventMap.View = System.Windows.Forms.View.Details;
      this.listViewEventMap.DoubleClick += new System.EventHandler(this.listViewEventMap_DoubleClick);
      this.listViewEventMap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewEventMap_KeyDown);
      // 
      // columnHeaderEvent
      // 
      this.columnHeaderEvent.Text = "Event";
      this.columnHeaderEvent.Width = 290;
      // 
      // columnHeaderCommand
      // 
      this.columnHeaderCommand.Text = "Command";
      this.columnHeaderCommand.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.columnHeaderCommand.Width = 240;
      // 
      // tabPageMouseMode
      // 
      this.tabPageMouseMode.Controls.Add(this.groupBoxMouseModeOptions);
      this.tabPageMouseMode.Controls.Add(this.checkBoxMouseMode);
      this.tabPageMouseMode.Location = new System.Drawing.Point(4, 22);
      this.tabPageMouseMode.Name = "tabPageMouseMode";
      this.tabPageMouseMode.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMouseMode.Size = new System.Drawing.Size(568, 302);
      this.tabPageMouseMode.TabIndex = 6;
      this.tabPageMouseMode.Text = "Mouse Mode";
      this.tabPageMouseMode.UseVisualStyleBackColor = true;
      // 
      // groupBoxMouseModeOptions
      // 
      this.groupBoxMouseModeOptions.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxMouseModeOptions.Controls.Add(this.labelMouseModeToggle);
      this.groupBoxMouseModeOptions.Controls.Add(this.checkBoxMouseAcceleration);
      this.groupBoxMouseModeOptions.Controls.Add(this.comboBoxMouseModeButton);
      this.groupBoxMouseModeOptions.Controls.Add(this.labelMouseStep);
      this.groupBoxMouseModeOptions.Controls.Add(this.numericUpDownMouseStep);
      this.groupBoxMouseModeOptions.Location = new System.Drawing.Point(112, 80);
      this.groupBoxMouseModeOptions.Name = "groupBoxMouseModeOptions";
      this.groupBoxMouseModeOptions.Size = new System.Drawing.Size(336, 136);
      this.groupBoxMouseModeOptions.TabIndex = 1;
      this.groupBoxMouseModeOptions.TabStop = false;
      this.groupBoxMouseModeOptions.Text = "Options";
      // 
      // labelMouseModeToggle
      // 
      this.labelMouseModeToggle.Location = new System.Drawing.Point(8, 24);
      this.labelMouseModeToggle.Name = "labelMouseModeToggle";
      this.labelMouseModeToggle.Size = new System.Drawing.Size(152, 21);
      this.labelMouseModeToggle.TabIndex = 0;
      this.labelMouseModeToggle.Text = "Mouse mode toggle button:";
      this.labelMouseModeToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelMouseStep
      // 
      this.labelMouseStep.Location = new System.Drawing.Point(8, 64);
      this.labelMouseStep.Name = "labelMouseStep";
      this.labelMouseStep.Size = new System.Drawing.Size(152, 20);
      this.labelMouseStep.TabIndex = 2;
      this.labelMouseStep.Text = "Mouse step distance:";
      this.labelMouseStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // SetupForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(592, 373);
      this.Controls.Add(this.checkBoxRequiresFocus);
      this.Controls.Add(this.checkBoxLogVerbose);
      this.Controls.Add(this.buttonChangeServer);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(600, 400);
      this.Name = "SetupForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "MediaPortal Control Plugin";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupForm_FormClosing);
      this.Load += new System.EventHandler(this.SetupForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseStep)).EndInit();
      this.tabControl.ResumeLayout(false);
      this.tabPageRemotes.ResumeLayout(false);
      this.groupBoxStatus.ResumeLayout(false);
      this.tabPageIR.ResumeLayout(false);
      this.tabPageMacros.ResumeLayout(false);
      this.tabPageMultiMapping.ResumeLayout(false);
      this.tabPageEventMapper.ResumeLayout(false);
      this.tabPageEventMapper.PerformLayout();
      this.tabPageMouseMode.ResumeLayout(false);
      this.groupBoxMouseModeOptions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonMapButtons;
    private System.Windows.Forms.CheckBox checkBoxRequiresFocus;
    private System.Windows.Forms.Button buttonNewMacro;
    private System.Windows.Forms.Button buttonEditIR;
    private System.Windows.Forms.Button buttonDeleteIR;
    private System.Windows.Forms.Button buttonNewIR;
    private System.Windows.Forms.Button buttonTestMacro;
    private System.Windows.Forms.Button buttonDeleteMacro;
    private System.Windows.Forms.Button buttonEditMacro;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.CheckBox checkBoxLogVerbose;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageIR;
    private System.Windows.Forms.TabPage tabPageMacros;
    private System.Windows.Forms.TabPage tabPageMultiMapping;
    private System.Windows.Forms.Button buttonDown;
    private System.Windows.Forms.Button buttonUp;
    private System.Windows.Forms.ListBox listBoxMappings;
    private System.Windows.Forms.Button buttonEdit;
    private System.Windows.Forms.Button buttonRemove;
    private System.Windows.Forms.Button buttonNew;
    private System.Windows.Forms.ComboBox comboBoxMultiButton;
    private System.Windows.Forms.Label labelButton;
    private System.Windows.Forms.CheckBox checkBoxMultiMapping;
    private System.Windows.Forms.TabPage tabPageEventMapper;
    private System.Windows.Forms.ListView listViewEventMap;
    private System.Windows.Forms.ColumnHeader columnHeaderEvent;
    private System.Windows.Forms.ColumnHeader columnHeaderCommand;
    private System.Windows.Forms.CheckBox checkBoxEventMapper;
    private System.Windows.Forms.Button buttonSetCommand;
    private System.Windows.Forms.ComboBox comboBoxCommands;
    private System.Windows.Forms.Label labelCommand;
    private System.Windows.Forms.Label labelEvent;
    private System.Windows.Forms.Label labelValue;
    private System.Windows.Forms.Label labelParameter;
    private System.Windows.Forms.TextBox textBoxParamValue;
    private System.Windows.Forms.ComboBox comboBoxParameter;
    private System.Windows.Forms.Button buttonClearEventParams;
    private System.Windows.Forms.ComboBox comboBoxEvents;
    private System.Windows.Forms.Button buttonAddEvent;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.TabPage tabPageMouseMode;
    private System.Windows.Forms.CheckBox checkBoxMouseMode;
    private System.Windows.Forms.ComboBox comboBoxMouseModeButton;
    private System.Windows.Forms.Label labelMouseModeToggle;
    private System.Windows.Forms.Label labelMouseStep;
    private System.Windows.Forms.NumericUpDown numericUpDownMouseStep;
    private System.Windows.Forms.CheckBox checkBoxMouseAcceleration;
    private System.Windows.Forms.Button buttonChangeServer;
    private System.Windows.Forms.TabPage tabPageRemotes;
    private System.Windows.Forms.TreeView treeViewRemotes;
    private System.Windows.Forms.GroupBox groupBoxStatus;
    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.Button buttonClearAll;
    private System.Windows.Forms.ComboBox comboBoxRemotePresets;
    private System.Windows.Forms.Button buttonLoadPreset;
    private System.Windows.Forms.GroupBox groupBoxMouseModeOptions;
    private System.Windows.Forms.ListView listViewIR;
    private System.Windows.Forms.ListView listViewMacro;
  }
}