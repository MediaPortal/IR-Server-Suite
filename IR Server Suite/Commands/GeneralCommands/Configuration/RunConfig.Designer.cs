namespace IrssCommands.General
{
  partial class RunConfig
  {
    /// <summary> 
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary> 
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.buttonStartupFolder = new System.Windows.Forms.Button();
      this.buttonLocate = new System.Windows.Forms.Button();
      this.checkBoxForceFocus = new System.Windows.Forms.CheckBox();
      this.checkBoxNoWindow = new System.Windows.Forms.CheckBox();
      this.checkBoxShellExecute = new System.Windows.Forms.CheckBox();
      this.comboBoxWindowStyle = new System.Windows.Forms.ComboBox();
      this.labelWindowStyle = new System.Windows.Forms.Label();
      this.labelAppParams = new System.Windows.Forms.Label();
      this.textBoxApplicationParameters = new System.Windows.Forms.TextBox();
      this.labelStartupFolder = new System.Windows.Forms.Label();
      this.textBoxAppStartFolder = new System.Windows.Forms.TextBox();
      this.labelApplication = new System.Windows.Forms.Label();
      this.textBoxApp = new System.Windows.Forms.TextBox();
      this.buttonParamQuestion = new System.Windows.Forms.Button();
      this.checkBoxWaitForExit = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // buttonStartupFolder
      // 
      this.buttonStartupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonStartupFolder.Location = new System.Drawing.Point(319, 35);
      this.buttonStartupFolder.Name = "buttonStartupFolder";
      this.buttonStartupFolder.Size = new System.Drawing.Size(24, 20);
      this.buttonStartupFolder.TabIndex = 18;
      this.buttonStartupFolder.Text = "...";
      this.toolTip.SetToolTip(this.buttonStartupFolder, "Click here to locate the working folder for the application");
      this.buttonStartupFolder.UseVisualStyleBackColor = true;
      this.buttonStartupFolder.Click += new System.EventHandler(this.buttonStartupFolder_Click);
      // 
      // buttonLocate
      // 
      this.buttonLocate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonLocate.Location = new System.Drawing.Point(319, 3);
      this.buttonLocate.Name = "buttonLocate";
      this.buttonLocate.Size = new System.Drawing.Size(24, 20);
      this.buttonLocate.TabIndex = 15;
      this.buttonLocate.Text = "...";
      this.toolTip.SetToolTip(this.buttonLocate, "Click here to locate the application to run");
      this.buttonLocate.UseVisualStyleBackColor = true;
      this.buttonLocate.Click += new System.EventHandler(this.buttonLocate_Click);
      // 
      // checkBoxForceFocus
      // 
      this.checkBoxForceFocus.AutoSize = true;
      this.checkBoxForceFocus.Location = new System.Drawing.Point(227, 154);
      this.checkBoxForceFocus.Name = "checkBoxForceFocus";
      this.checkBoxForceFocus.Size = new System.Drawing.Size(121, 17);
      this.checkBoxForceFocus.TabIndex = 25;
      this.checkBoxForceFocus.Text = "Force window focus";
      this.checkBoxForceFocus.UseVisualStyleBackColor = true;
      // 
      // checkBoxNoWindow
      // 
      this.checkBoxNoWindow.Location = new System.Drawing.Point(227, 127);
      this.checkBoxNoWindow.Name = "checkBoxNoWindow";
      this.checkBoxNoWindow.Size = new System.Drawing.Size(104, 21);
      this.checkBoxNoWindow.TabIndex = 23;
      this.checkBoxNoWindow.Text = "No window";
      this.checkBoxNoWindow.UseVisualStyleBackColor = true;
      // 
      // checkBoxShellExecute
      // 
      this.checkBoxShellExecute.AutoSize = true;
      this.checkBoxShellExecute.Location = new System.Drawing.Point(62, 131);
      this.checkBoxShellExecute.Name = "checkBoxShellExecute";
      this.checkBoxShellExecute.Size = new System.Drawing.Size(141, 17);
      this.checkBoxShellExecute.TabIndex = 24;
      this.checkBoxShellExecute.Text = "Start using ShellExecute";
      this.checkBoxShellExecute.UseVisualStyleBackColor = true;
      // 
      // comboBoxWindowStyle
      // 
      this.comboBoxWindowStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxWindowStyle.FormattingEnabled = true;
      this.comboBoxWindowStyle.Location = new System.Drawing.Point(91, 99);
      this.comboBoxWindowStyle.MaxDropDownItems = 4;
      this.comboBoxWindowStyle.Name = "comboBoxWindowStyle";
      this.comboBoxWindowStyle.Size = new System.Drawing.Size(150, 21);
      this.comboBoxWindowStyle.TabIndex = 22;
      // 
      // labelWindowStyle
      // 
      this.labelWindowStyle.Location = new System.Drawing.Point(3, 99);
      this.labelWindowStyle.Name = "labelWindowStyle";
      this.labelWindowStyle.Size = new System.Drawing.Size(88, 20);
      this.labelWindowStyle.TabIndex = 21;
      this.labelWindowStyle.Text = "Window style:";
      this.labelWindowStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelAppParams
      // 
      this.labelAppParams.Location = new System.Drawing.Point(3, 67);
      this.labelAppParams.Name = "labelAppParams";
      this.labelAppParams.Size = new System.Drawing.Size(88, 20);
      this.labelAppParams.TabIndex = 19;
      this.labelAppParams.Text = "Parameters:";
      this.labelAppParams.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxApplicationParameters
      // 
      this.textBoxApplicationParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxApplicationParameters.Location = new System.Drawing.Point(91, 67);
      this.textBoxApplicationParameters.Name = "textBoxApplicationParameters";
      this.textBoxApplicationParameters.Size = new System.Drawing.Size(220, 20);
      this.textBoxApplicationParameters.TabIndex = 20;
      // 
      // labelStartupFolder
      // 
      this.labelStartupFolder.Location = new System.Drawing.Point(3, 35);
      this.labelStartupFolder.Name = "labelStartupFolder";
      this.labelStartupFolder.Size = new System.Drawing.Size(88, 20);
      this.labelStartupFolder.TabIndex = 16;
      this.labelStartupFolder.Text = "Start folder:";
      this.labelStartupFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxAppStartFolder
      // 
      this.textBoxAppStartFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAppStartFolder.Location = new System.Drawing.Point(91, 35);
      this.textBoxAppStartFolder.Name = "textBoxAppStartFolder";
      this.textBoxAppStartFolder.Size = new System.Drawing.Size(220, 20);
      this.textBoxAppStartFolder.TabIndex = 17;
      // 
      // labelApplication
      // 
      this.labelApplication.Location = new System.Drawing.Point(3, 3);
      this.labelApplication.Name = "labelApplication";
      this.labelApplication.Size = new System.Drawing.Size(88, 20);
      this.labelApplication.TabIndex = 13;
      this.labelApplication.Text = "Application:";
      this.labelApplication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxApp
      // 
      this.textBoxApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxApp.Location = new System.Drawing.Point(91, 3);
      this.textBoxApp.Name = "textBoxApp";
      this.textBoxApp.Size = new System.Drawing.Size(220, 20);
      this.textBoxApp.TabIndex = 14;
      // 
      // buttonParamQuestion
      // 
      this.buttonParamQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonParamQuestion.Location = new System.Drawing.Point(319, 67);
      this.buttonParamQuestion.Name = "buttonParamQuestion";
      this.buttonParamQuestion.Size = new System.Drawing.Size(24, 20);
      this.buttonParamQuestion.TabIndex = 26;
      this.buttonParamQuestion.Text = "?";
      this.buttonParamQuestion.UseVisualStyleBackColor = true;
      // 
      // checkBoxWaitForExit
      // 
      this.checkBoxWaitForExit.AutoSize = true;
      this.checkBoxWaitForExit.Location = new System.Drawing.Point(62, 154);
      this.checkBoxWaitForExit.Name = "checkBoxWaitForExit";
      this.checkBoxWaitForExit.Size = new System.Drawing.Size(121, 17);
      this.checkBoxWaitForExit.TabIndex = 25;
      this.checkBoxWaitForExit.Text = "Force window focus";
      this.checkBoxWaitForExit.UseVisualStyleBackColor = true;
      // 
      // RunConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonParamQuestion);
      this.Controls.Add(this.checkBoxWaitForExit);
      this.Controls.Add(this.checkBoxForceFocus);
      this.Controls.Add(this.checkBoxNoWindow);
      this.Controls.Add(this.checkBoxShellExecute);
      this.Controls.Add(this.comboBoxWindowStyle);
      this.Controls.Add(this.labelWindowStyle);
      this.Controls.Add(this.labelAppParams);
      this.Controls.Add(this.textBoxApplicationParameters);
      this.Controls.Add(this.labelStartupFolder);
      this.Controls.Add(this.buttonStartupFolder);
      this.Controls.Add(this.textBoxAppStartFolder);
      this.Controls.Add(this.labelApplication);
      this.Controls.Add(this.buttonLocate);
      this.Controls.Add(this.textBoxApp);
      this.MinimumSize = new System.Drawing.Size(347, 179);
      this.Name = "RunConfig";
      this.Size = new System.Drawing.Size(347, 179);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.CheckBox checkBoxForceFocus;
    private System.Windows.Forms.CheckBox checkBoxNoWindow;
    private System.Windows.Forms.CheckBox checkBoxShellExecute;
    private System.Windows.Forms.ComboBox comboBoxWindowStyle;
    private System.Windows.Forms.Label labelWindowStyle;
    private System.Windows.Forms.Label labelAppParams;
    private System.Windows.Forms.TextBox textBoxApplicationParameters;
    private System.Windows.Forms.Label labelStartupFolder;
    private System.Windows.Forms.Button buttonStartupFolder;
    private System.Windows.Forms.TextBox textBoxAppStartFolder;
    private System.Windows.Forms.Label labelApplication;
    private System.Windows.Forms.Button buttonLocate;
    private System.Windows.Forms.TextBox textBoxApp;
    private System.Windows.Forms.Button buttonParamQuestion;
    private System.Windows.Forms.CheckBox checkBoxWaitForExit;
  }
}
