namespace TvEngine
{
  partial class MacroEditor
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
      this.labelName = new System.Windows.Forms.Label();
      this.textBoxName = new System.Windows.Forms.TextBox();
      this.groupBoxCommandSequence = new System.Windows.Forms.GroupBox();
      this.buttonRemove = new System.Windows.Forms.Button();
      this.buttonMoveDown = new System.Windows.Forms.Button();
      this.buttonMoveUp = new System.Windows.Forms.Button();
      this.listBoxMacro = new System.Windows.Forms.ListBox();
      this.comboBoxCommands = new System.Windows.Forms.ComboBox();
      this.buttonAddCommand = new System.Windows.Forms.Button();
      this.groupBoxCommands = new System.Windows.Forms.GroupBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxCommandSequence.SuspendLayout();
      this.groupBoxCommands.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelName
      // 
      this.labelName.Location = new System.Drawing.Point(8, 8);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(48, 20);
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
      this.textBoxName.Size = new System.Drawing.Size(240, 20);
      this.textBoxName.TabIndex = 1;
      // 
      // groupBoxCommandSequence
      // 
      this.groupBoxCommandSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCommandSequence.Controls.Add(this.buttonRemove);
      this.groupBoxCommandSequence.Controls.Add(this.buttonMoveDown);
      this.groupBoxCommandSequence.Controls.Add(this.buttonMoveUp);
      this.groupBoxCommandSequence.Controls.Add(this.listBoxMacro);
      this.groupBoxCommandSequence.Location = new System.Drawing.Point(8, 40);
      this.groupBoxCommandSequence.Name = "groupBoxCommandSequence";
      this.groupBoxCommandSequence.Size = new System.Drawing.Size(296, 192);
      this.groupBoxCommandSequence.TabIndex = 2;
      this.groupBoxCommandSequence.TabStop = false;
      this.groupBoxCommandSequence.Text = "Macro";
      // 
      // buttonRemove
      // 
      this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRemove.Location = new System.Drawing.Point(232, 167);
      this.buttonRemove.Name = "buttonRemove";
      this.buttonRemove.Size = new System.Drawing.Size(56, 24);
      this.buttonRemove.TabIndex = 3;
      this.buttonRemove.Text = "Remove";
      this.buttonRemove.UseVisualStyleBackColor = true;
      this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
      // 
      // buttonMoveDown
      // 
      this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonMoveDown.Location = new System.Drawing.Point(64, 167);
      this.buttonMoveDown.Name = "buttonMoveDown";
      this.buttonMoveDown.Size = new System.Drawing.Size(48, 24);
      this.buttonMoveDown.TabIndex = 2;
      this.buttonMoveDown.Text = "Down";
      this.buttonMoveDown.UseVisualStyleBackColor = true;
      this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
      // 
      // buttonMoveUp
      // 
      this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonMoveUp.Location = new System.Drawing.Point(8, 167);
      this.buttonMoveUp.Name = "buttonMoveUp";
      this.buttonMoveUp.Size = new System.Drawing.Size(48, 24);
      this.buttonMoveUp.TabIndex = 1;
      this.buttonMoveUp.Text = "Up";
      this.buttonMoveUp.UseVisualStyleBackColor = true;
      this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
      // 
      // listBoxMacro
      // 
      this.listBoxMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxMacro.FormattingEnabled = true;
      this.listBoxMacro.IntegralHeight = false;
      this.listBoxMacro.Location = new System.Drawing.Point(8, 16);
      this.listBoxMacro.Name = "listBoxMacro";
      this.listBoxMacro.Size = new System.Drawing.Size(280, 143);
      this.listBoxMacro.TabIndex = 0;
      this.listBoxMacro.DoubleClick += new System.EventHandler(this.listBoxCommandSequence_DoubleClick);
      // 
      // comboBoxCommands
      // 
      this.comboBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxCommands.FormattingEnabled = true;
      this.comboBoxCommands.Location = new System.Drawing.Point(8, 16);
      this.comboBoxCommands.Name = "comboBoxCommands";
      this.comboBoxCommands.Size = new System.Drawing.Size(232, 21);
      this.comboBoxCommands.TabIndex = 0;
      // 
      // buttonAddCommand
      // 
      this.buttonAddCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAddCommand.Location = new System.Drawing.Point(248, 16);
      this.buttonAddCommand.Name = "buttonAddCommand";
      this.buttonAddCommand.Size = new System.Drawing.Size(40, 21);
      this.buttonAddCommand.TabIndex = 1;
      this.buttonAddCommand.Text = "Add";
      this.buttonAddCommand.UseVisualStyleBackColor = true;
      this.buttonAddCommand.Click += new System.EventHandler(this.buttonAddCommand_Click);
      // 
      // groupBoxCommands
      // 
      this.groupBoxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCommands.Controls.Add(this.buttonAddCommand);
      this.groupBoxCommands.Controls.Add(this.comboBoxCommands);
      this.groupBoxCommands.Location = new System.Drawing.Point(8, 240);
      this.groupBoxCommands.Name = "groupBoxCommands";
      this.groupBoxCommands.Size = new System.Drawing.Size(296, 48);
      this.groupBoxCommands.TabIndex = 3;
      this.groupBoxCommands.TabStop = false;
      this.groupBoxCommands.Text = "Commands";
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(256, 296);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(48, 24);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(8, 296);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(48, 24);
      this.buttonTest.TabIndex = 4;
      this.buttonTest.Text = "Test";
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(200, 296);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(48, 24);
      this.buttonOK.TabIndex = 5;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // MacroEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(312, 329);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.groupBoxCommands);
      this.Controls.Add(this.groupBoxCommandSequence);
      this.Controls.Add(this.labelName);
      this.Controls.Add(this.textBoxName);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(320, 356);
      this.Name = "MacroEditor";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Macro Editor";
      this.Load += new System.EventHandler(this.MacroEditor_Load);
      this.groupBoxCommandSequence.ResumeLayout(false);
      this.groupBoxCommands.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.GroupBox groupBoxCommandSequence;
    private System.Windows.Forms.Button buttonRemove;
    private System.Windows.Forms.Button buttonMoveDown;
    private System.Windows.Forms.Button buttonMoveUp;
    private System.Windows.Forms.ListBox listBoxMacro;
    private System.Windows.Forms.ComboBox comboBoxCommands;
    private System.Windows.Forms.Button buttonAddCommand;
    private System.Windows.Forms.GroupBox groupBoxCommands;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button buttonOK;
  }
}