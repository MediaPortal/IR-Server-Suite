namespace MediaCenterBlaster
{
  partial class StbSetup
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
      this.groupBoxOptions = new System.Windows.Forms.GroupBox();
      this.checkBoxUsePreChange = new System.Windows.Forms.CheckBox();
      this.numericUpDownRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.labelWait2 = new System.Windows.Forms.Label();
      this.numericUpDownPauseTime = new System.Windows.Forms.NumericUpDown();
      this.comboBoxChDigits = new System.Windows.Forms.ComboBox();
      this.labelChannelNumbers = new System.Windows.Forms.Label();
      this.numericUpDownRepeat = new System.Windows.Forms.NumericUpDown();
      this.labelRepeatCount = new System.Windows.Forms.Label();
      this.checkBoxDoubleSelect = new System.Windows.Forms.CheckBox();
      this.labelMS = new System.Windows.Forms.Label();
      this.checkBoxSendSelect = new System.Windows.Forms.CheckBox();
      this.groupBoxCommands = new System.Windows.Forms.GroupBox();
      this.buttonSet = new System.Windows.Forms.Button();
      this.comboBoxCommands = new System.Windows.Forms.ComboBox();
      this.listViewExternalCommands = new System.Windows.Forms.ListView();
      this.columnHeaderExternal = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderCommand = new System.Windows.Forms.ColumnHeader();
      this.groupBoxOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPauseTime)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeat)).BeginInit();
      this.groupBoxCommands.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxOptions
      // 
      this.groupBoxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxOptions.Controls.Add(this.checkBoxUsePreChange);
      this.groupBoxOptions.Controls.Add(this.numericUpDownRepeatDelay);
      this.groupBoxOptions.Controls.Add(this.labelWait2);
      this.groupBoxOptions.Controls.Add(this.numericUpDownPauseTime);
      this.groupBoxOptions.Controls.Add(this.comboBoxChDigits);
      this.groupBoxOptions.Controls.Add(this.labelChannelNumbers);
      this.groupBoxOptions.Controls.Add(this.numericUpDownRepeat);
      this.groupBoxOptions.Controls.Add(this.labelRepeatCount);
      this.groupBoxOptions.Controls.Add(this.checkBoxDoubleSelect);
      this.groupBoxOptions.Controls.Add(this.labelMS);
      this.groupBoxOptions.Controls.Add(this.checkBoxSendSelect);
      this.groupBoxOptions.Location = new System.Drawing.Point(296, 8);
      this.groupBoxOptions.Name = "groupBoxOptions";
      this.groupBoxOptions.Size = new System.Drawing.Size(200, 256);
      this.groupBoxOptions.TabIndex = 1;
      this.groupBoxOptions.TabStop = false;
      this.groupBoxOptions.Text = "Options";
      // 
      // checkBoxUsePreChange
      // 
      this.checkBoxUsePreChange.Location = new System.Drawing.Point(8, 64);
      this.checkBoxUsePreChange.Name = "checkBoxUsePreChange";
      this.checkBoxUsePreChange.Size = new System.Drawing.Size(184, 24);
      this.checkBoxUsePreChange.TabIndex = 2;
      this.checkBoxUsePreChange.Text = "Use pre-change command";
      this.checkBoxUsePreChange.UseVisualStyleBackColor = true;
      // 
      // numericUpDownRepeatDelay
      // 
      this.numericUpDownRepeatDelay.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.numericUpDownRepeatDelay.Location = new System.Drawing.Point(104, 216);
      this.numericUpDownRepeatDelay.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
      this.numericUpDownRepeatDelay.Name = "numericUpDownRepeatDelay";
      this.numericUpDownRepeatDelay.Size = new System.Drawing.Size(88, 20);
      this.numericUpDownRepeatDelay.TabIndex = 14;
      this.numericUpDownRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownRepeatDelay.ThousandsSeparator = true;
      this.numericUpDownRepeatDelay.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      // 
      // labelWait2
      // 
      this.labelWait2.Location = new System.Drawing.Point(8, 216);
      this.labelWait2.Name = "labelWait2";
      this.labelWait2.Size = new System.Drawing.Size(96, 20);
      this.labelWait2.TabIndex = 13;
      this.labelWait2.Text = "Repeat delay:";
      this.labelWait2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownPauseTime
      // 
      this.numericUpDownPauseTime.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownPauseTime.Location = new System.Drawing.Point(8, 32);
      this.numericUpDownPauseTime.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
      this.numericUpDownPauseTime.Name = "numericUpDownPauseTime";
      this.numericUpDownPauseTime.Size = new System.Drawing.Size(48, 20);
      this.numericUpDownPauseTime.TabIndex = 0;
      this.numericUpDownPauseTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownPauseTime.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
      // 
      // comboBoxChDigits
      // 
      this.comboBoxChDigits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxChDigits.FormattingEnabled = true;
      this.comboBoxChDigits.Items.AddRange(new object[] {
            "Simple",
            "2 Digits",
            "3 Digits",
            "4 Digits"});
      this.comboBoxChDigits.Location = new System.Drawing.Point(104, 152);
      this.comboBoxChDigits.Name = "comboBoxChDigits";
      this.comboBoxChDigits.Size = new System.Drawing.Size(88, 21);
      this.comboBoxChDigits.TabIndex = 6;
      // 
      // labelChannelNumbers
      // 
      this.labelChannelNumbers.Location = new System.Drawing.Point(8, 152);
      this.labelChannelNumbers.Name = "labelChannelNumbers";
      this.labelChannelNumbers.Size = new System.Drawing.Size(96, 21);
      this.labelChannelNumbers.TabIndex = 5;
      this.labelChannelNumbers.Text = "Channel digits:";
      this.labelChannelNumbers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownRepeat
      // 
      this.numericUpDownRepeat.Location = new System.Drawing.Point(104, 184);
      this.numericUpDownRepeat.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.numericUpDownRepeat.Name = "numericUpDownRepeat";
      this.numericUpDownRepeat.Size = new System.Drawing.Size(88, 20);
      this.numericUpDownRepeat.TabIndex = 12;
      this.numericUpDownRepeat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelRepeatCount
      // 
      this.labelRepeatCount.Location = new System.Drawing.Point(8, 184);
      this.labelRepeatCount.Name = "labelRepeatCount";
      this.labelRepeatCount.Size = new System.Drawing.Size(96, 20);
      this.labelRepeatCount.TabIndex = 11;
      this.labelRepeatCount.Text = "Repeat count:";
      this.labelRepeatCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // checkBoxDoubleSelect
      // 
      this.checkBoxDoubleSelect.Location = new System.Drawing.Point(24, 120);
      this.checkBoxDoubleSelect.Name = "checkBoxDoubleSelect";
      this.checkBoxDoubleSelect.Size = new System.Drawing.Size(168, 24);
      this.checkBoxDoubleSelect.TabIndex = 4;
      this.checkBoxDoubleSelect.Text = "Send select twice";
      this.checkBoxDoubleSelect.UseVisualStyleBackColor = true;
      // 
      // labelMS
      // 
      this.labelMS.Location = new System.Drawing.Point(56, 32);
      this.labelMS.Name = "labelMS";
      this.labelMS.Size = new System.Drawing.Size(136, 20);
      this.labelMS.TabIndex = 1;
      this.labelMS.Text = " ms wait between digits";
      this.labelMS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // checkBoxSendSelect
      // 
      this.checkBoxSendSelect.Location = new System.Drawing.Point(8, 96);
      this.checkBoxSendSelect.Name = "checkBoxSendSelect";
      this.checkBoxSendSelect.Size = new System.Drawing.Size(184, 24);
      this.checkBoxSendSelect.TabIndex = 3;
      this.checkBoxSendSelect.Text = "Send select command";
      this.checkBoxSendSelect.UseVisualStyleBackColor = true;
      this.checkBoxSendSelect.CheckedChanged += new System.EventHandler(this.checkBoxSendSelect_CheckedChanged);
      // 
      // groupBoxCommands
      // 
      this.groupBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCommands.Controls.Add(this.buttonSet);
      this.groupBoxCommands.Controls.Add(this.comboBoxCommands);
      this.groupBoxCommands.Controls.Add(this.listViewExternalCommands);
      this.groupBoxCommands.Location = new System.Drawing.Point(8, 8);
      this.groupBoxCommands.Name = "groupBoxCommands";
      this.groupBoxCommands.Size = new System.Drawing.Size(280, 296);
      this.groupBoxCommands.TabIndex = 0;
      this.groupBoxCommands.TabStop = false;
      this.groupBoxCommands.Text = "Commands";
      // 
      // buttonSet
      // 
      this.buttonSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSet.Location = new System.Drawing.Point(232, 264);
      this.buttonSet.Name = "buttonSet";
      this.buttonSet.Size = new System.Drawing.Size(40, 21);
      this.buttonSet.TabIndex = 2;
      this.buttonSet.Text = "Set";
      this.buttonSet.UseVisualStyleBackColor = true;
      this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
      // 
      // comboBoxCommands
      // 
      this.comboBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxCommands.FormattingEnabled = true;
      this.comboBoxCommands.Location = new System.Drawing.Point(8, 264);
      this.comboBoxCommands.Name = "comboBoxCommands";
      this.comboBoxCommands.Size = new System.Drawing.Size(216, 21);
      this.comboBoxCommands.TabIndex = 1;
      // 
      // listViewExternalCommands
      // 
      this.listViewExternalCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewExternalCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderExternal,
            this.columnHeaderCommand});
      this.listViewExternalCommands.FullRowSelect = true;
      this.listViewExternalCommands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewExternalCommands.HideSelection = false;
      this.listViewExternalCommands.Location = new System.Drawing.Point(8, 16);
      this.listViewExternalCommands.Name = "listViewExternalCommands";
      this.listViewExternalCommands.Size = new System.Drawing.Size(264, 240);
      this.listViewExternalCommands.TabIndex = 0;
      this.listViewExternalCommands.UseCompatibleStateImageBehavior = false;
      this.listViewExternalCommands.View = System.Windows.Forms.View.Details;
      this.listViewExternalCommands.DoubleClick += new System.EventHandler(this.listViewExternalCommands_DoubleClick);
      this.listViewExternalCommands.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewExternalCommands_KeyDown);
      // 
      // columnHeaderExternal
      // 
      this.columnHeaderExternal.Text = "External";
      this.columnHeaderExternal.Width = 70;
      // 
      // columnHeaderCommand
      // 
      this.columnHeaderCommand.Text = "Command";
      this.columnHeaderCommand.Width = 170;
      // 
      // StbSetup
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.groupBoxOptions);
      this.Controls.Add(this.groupBoxCommands);
      this.MinimumSize = new System.Drawing.Size(504, 310);
      this.Name = "StbSetup";
      this.Size = new System.Drawing.Size(504, 310);
      this.groupBoxOptions.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPauseTime)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeat)).EndInit();
      this.groupBoxCommands.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxOptions;
    private System.Windows.Forms.CheckBox checkBoxUsePreChange;
    private System.Windows.Forms.NumericUpDown numericUpDownRepeatDelay;
    private System.Windows.Forms.Label labelWait2;
    private System.Windows.Forms.NumericUpDown numericUpDownPauseTime;
    private System.Windows.Forms.ComboBox comboBoxChDigits;
    private System.Windows.Forms.Label labelChannelNumbers;
    private System.Windows.Forms.NumericUpDown numericUpDownRepeat;
    private System.Windows.Forms.Label labelRepeatCount;
    private System.Windows.Forms.CheckBox checkBoxDoubleSelect;
    private System.Windows.Forms.Label labelMS;
    private System.Windows.Forms.CheckBox checkBoxSendSelect;
    private System.Windows.Forms.GroupBox groupBoxCommands;
    private System.Windows.Forms.Button buttonSet;
    private System.Windows.Forms.ComboBox comboBoxCommands;
    private System.Windows.Forms.ListView listViewExternalCommands;
    private System.Windows.Forms.ColumnHeader columnHeaderExternal;
    private System.Windows.Forms.ColumnHeader columnHeaderCommand;
  }
}
