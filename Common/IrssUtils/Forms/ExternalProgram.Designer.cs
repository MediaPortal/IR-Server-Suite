namespace IrssUtils.Forms
{
  partial class ExternalProgram
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
      this.textBoxProgram = new System.Windows.Forms.TextBox();
      this.labelProgram = new System.Windows.Forms.Label();
      this.buttonProgam = new System.Windows.Forms.Button();
      this.buttonStartup = new System.Windows.Forms.Button();
      this.labelStartup = new System.Windows.Forms.Label();
      this.textBoxStartup = new System.Windows.Forms.TextBox();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.labelParameters = new System.Windows.Forms.Label();
      this.textBoxParameters = new System.Windows.Forms.TextBox();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.buttonParamQuestion = new System.Windows.Forms.Button();
      this.checkBoxShellExecute = new System.Windows.Forms.CheckBox();
      this.buttonTest = new System.Windows.Forms.Button();
      this.checkBoxNoWindow = new System.Windows.Forms.CheckBox();
      this.checkBoxWaitForExit = new System.Windows.Forms.CheckBox();
      this.comboBoxWindowStyle = new System.Windows.Forms.ComboBox();
      this.labelWindowStyle = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // textBoxProgram
      // 
      this.textBoxProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxProgram.Location = new System.Drawing.Point(8, 24);
      this.textBoxProgram.Name = "textBoxProgram";
      this.textBoxProgram.Size = new System.Drawing.Size(288, 20);
      this.textBoxProgram.TabIndex = 1;
      // 
      // labelProgram
      // 
      this.labelProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelProgram.Location = new System.Drawing.Point(8, 8);
      this.labelProgram.Name = "labelProgram";
      this.labelProgram.Size = new System.Drawing.Size(288, 16);
      this.labelProgram.TabIndex = 0;
      this.labelProgram.Text = "Program:";
      this.labelProgram.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonProgam
      // 
      this.buttonProgam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonProgam.Location = new System.Drawing.Point(304, 24);
      this.buttonProgam.Name = "buttonProgam";
      this.buttonProgam.Size = new System.Drawing.Size(24, 20);
      this.buttonProgam.TabIndex = 2;
      this.buttonProgam.Text = "...";
      this.buttonProgam.UseVisualStyleBackColor = true;
      this.buttonProgam.Click += new System.EventHandler(this.buttonProgam_Click);
      // 
      // buttonStartup
      // 
      this.buttonStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonStartup.Location = new System.Drawing.Point(304, 72);
      this.buttonStartup.Name = "buttonStartup";
      this.buttonStartup.Size = new System.Drawing.Size(24, 20);
      this.buttonStartup.TabIndex = 5;
      this.buttonStartup.Text = "...";
      this.buttonStartup.UseVisualStyleBackColor = true;
      this.buttonStartup.Click += new System.EventHandler(this.buttonStartup_Click);
      // 
      // labelStartup
      // 
      this.labelStartup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelStartup.Location = new System.Drawing.Point(8, 56);
      this.labelStartup.Name = "labelStartup";
      this.labelStartup.Size = new System.Drawing.Size(288, 16);
      this.labelStartup.TabIndex = 3;
      this.labelStartup.Text = "Start in folder:";
      this.labelStartup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxStartup
      // 
      this.textBoxStartup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxStartup.Location = new System.Drawing.Point(8, 72);
      this.textBoxStartup.Name = "textBoxStartup";
      this.textBoxStartup.Size = new System.Drawing.Size(288, 20);
      this.textBoxStartup.TabIndex = 4;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(208, 216);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 15;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(272, 216);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 16;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelParameters
      // 
      this.labelParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelParameters.Location = new System.Drawing.Point(8, 104);
      this.labelParameters.Name = "labelParameters";
      this.labelParameters.Size = new System.Drawing.Size(288, 16);
      this.labelParameters.TabIndex = 6;
      this.labelParameters.Text = "Parameters:";
      this.labelParameters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxParameters
      // 
      this.textBoxParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxParameters.Location = new System.Drawing.Point(8, 120);
      this.textBoxParameters.Name = "textBoxParameters";
      this.textBoxParameters.Size = new System.Drawing.Size(288, 20);
      this.textBoxParameters.TabIndex = 7;
      // 
      // openFileDialog
      // 
      this.openFileDialog.Filter = "All files|*.*";
      this.openFileDialog.Title = "Select Program Executable";
      // 
      // folderBrowserDialog
      // 
      this.folderBrowserDialog.Description = "Select the startup folder for the program to run from";
      // 
      // buttonParamQuestion
      // 
      this.buttonParamQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonParamQuestion.Location = new System.Drawing.Point(304, 120);
      this.buttonParamQuestion.Name = "buttonParamQuestion";
      this.buttonParamQuestion.Size = new System.Drawing.Size(24, 20);
      this.buttonParamQuestion.TabIndex = 8;
      this.buttonParamQuestion.Text = "?";
      this.buttonParamQuestion.UseVisualStyleBackColor = true;
      this.buttonParamQuestion.Click += new System.EventHandler(this.buttonParamQuestion_Click);
      // 
      // checkBoxShellExecute
      // 
      this.checkBoxShellExecute.Location = new System.Drawing.Point(8, 184);
      this.checkBoxShellExecute.Name = "checkBoxShellExecute";
      this.checkBoxShellExecute.Size = new System.Drawing.Size(184, 21);
      this.checkBoxShellExecute.TabIndex = 12;
      this.checkBoxShellExecute.Text = "Start using ShellExecute";
      this.checkBoxShellExecute.UseVisualStyleBackColor = true;
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(8, 216);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(56, 24);
      this.buttonTest.TabIndex = 14;
      this.buttonTest.Text = "Test";
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // checkBoxNoWindow
      // 
      this.checkBoxNoWindow.Location = new System.Drawing.Point(208, 152);
      this.checkBoxNoWindow.Name = "checkBoxNoWindow";
      this.checkBoxNoWindow.Size = new System.Drawing.Size(104, 21);
      this.checkBoxNoWindow.TabIndex = 11;
      this.checkBoxNoWindow.Text = "No window";
      this.checkBoxNoWindow.UseVisualStyleBackColor = true;
      // 
      // checkBoxWaitForExit
      // 
      this.checkBoxWaitForExit.Location = new System.Drawing.Point(208, 184);
      this.checkBoxWaitForExit.Name = "checkBoxWaitForExit";
      this.checkBoxWaitForExit.Size = new System.Drawing.Size(104, 21);
      this.checkBoxWaitForExit.TabIndex = 13;
      this.checkBoxWaitForExit.Text = "Wait for exit";
      this.checkBoxWaitForExit.UseVisualStyleBackColor = true;
      // 
      // comboBoxWindowStyle
      // 
      this.comboBoxWindowStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxWindowStyle.FormattingEnabled = true;
      this.comboBoxWindowStyle.Location = new System.Drawing.Point(104, 152);
      this.comboBoxWindowStyle.MaxDropDownItems = 4;
      this.comboBoxWindowStyle.Name = "comboBoxWindowStyle";
      this.comboBoxWindowStyle.Size = new System.Drawing.Size(88, 21);
      this.comboBoxWindowStyle.TabIndex = 10;
      // 
      // labelWindowStyle
      // 
      this.labelWindowStyle.Location = new System.Drawing.Point(8, 152);
      this.labelWindowStyle.Name = "labelWindowStyle";
      this.labelWindowStyle.Size = new System.Drawing.Size(96, 21);
      this.labelWindowStyle.TabIndex = 9;
      this.labelWindowStyle.Text = "Window Style:";
      this.labelWindowStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // ExternalProgram
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(336, 248);
      this.Controls.Add(this.checkBoxNoWindow);
      this.Controls.Add(this.checkBoxWaitForExit);
      this.Controls.Add(this.comboBoxWindowStyle);
      this.Controls.Add(this.labelWindowStyle);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.checkBoxShellExecute);
      this.Controls.Add(this.buttonParamQuestion);
      this.Controls.Add(this.labelParameters);
      this.Controls.Add(this.textBoxParameters);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonStartup);
      this.Controls.Add(this.labelStartup);
      this.Controls.Add(this.textBoxStartup);
      this.Controls.Add(this.buttonProgam);
      this.Controls.Add(this.labelProgram);
      this.Controls.Add(this.textBoxProgram);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(344, 282);
      this.Name = "ExternalProgram";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "External Program Details";
      this.Load += new System.EventHandler(this.ExternalProgram_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxProgram;
    private System.Windows.Forms.Label labelProgram;
    private System.Windows.Forms.Button buttonProgam;
    private System.Windows.Forms.Button buttonStartup;
    private System.Windows.Forms.Label labelStartup;
    private System.Windows.Forms.TextBox textBoxStartup;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelParameters;
    private System.Windows.Forms.TextBox textBoxParameters;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.Button buttonParamQuestion;
    private System.Windows.Forms.CheckBox checkBoxShellExecute;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.CheckBox checkBoxNoWindow;
    private System.Windows.Forms.CheckBox checkBoxWaitForExit;
    private System.Windows.Forms.ComboBox comboBoxWindowStyle;
    private System.Windows.Forms.Label labelWindowStyle;
  }
}