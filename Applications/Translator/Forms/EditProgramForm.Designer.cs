namespace Translator
{
  partial class EditProgramForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditProgramForm));
      this.textBoxApp = new System.Windows.Forms.TextBox();
      this.buttonLocate = new System.Windows.Forms.Button();
      this.labelApplication = new System.Windows.Forms.Label();
      this.labelAppDisplayName = new System.Windows.Forms.Label();
      this.textBoxDisplayName = new System.Windows.Forms.TextBox();
      this.labelStartupFolder = new System.Windows.Forms.Label();
      this.buttonStartupFolder = new System.Windows.Forms.Button();
      this.textBoxAppStartFolder = new System.Windows.Forms.TextBox();
      this.labelAppParams = new System.Windows.Forms.Label();
      this.textBoxApplicationParameters = new System.Windows.Forms.TextBox();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.checkBoxShellExecute = new System.Windows.Forms.CheckBox();
      this.comboBoxWindowStyle = new System.Windows.Forms.ComboBox();
      this.labelWindowStyle = new System.Windows.Forms.Label();
      this.checkBoxIgnoreSystemWide = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // textBoxApp
      // 
      this.textBoxApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxApp.Location = new System.Drawing.Point(8, 56);
      this.textBoxApp.Name = "textBoxApp";
      this.textBoxApp.Size = new System.Drawing.Size(336, 20);
      this.textBoxApp.TabIndex = 3;
      // 
      // buttonLocate
      // 
      this.buttonLocate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonLocate.Location = new System.Drawing.Point(352, 56);
      this.buttonLocate.Name = "buttonLocate";
      this.buttonLocate.Size = new System.Drawing.Size(24, 20);
      this.buttonLocate.TabIndex = 4;
      this.buttonLocate.Text = "...";
      this.buttonLocate.UseVisualStyleBackColor = true;
      this.buttonLocate.Click += new System.EventHandler(this.buttonLocate_Click);
      // 
      // labelApplication
      // 
      this.labelApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelApplication.Location = new System.Drawing.Point(8, 40);
      this.labelApplication.Name = "labelApplication";
      this.labelApplication.Size = new System.Drawing.Size(336, 16);
      this.labelApplication.TabIndex = 2;
      this.labelApplication.Text = "Application:";
      // 
      // labelAppDisplayName
      // 
      this.labelAppDisplayName.Location = new System.Drawing.Point(8, 8);
      this.labelAppDisplayName.Name = "labelAppDisplayName";
      this.labelAppDisplayName.Size = new System.Drawing.Size(144, 20);
      this.labelAppDisplayName.TabIndex = 0;
      this.labelAppDisplayName.Text = "Application display name:";
      this.labelAppDisplayName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxDisplayName
      // 
      this.textBoxDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxDisplayName.Location = new System.Drawing.Point(152, 8);
      this.textBoxDisplayName.Name = "textBoxDisplayName";
      this.textBoxDisplayName.Size = new System.Drawing.Size(224, 20);
      this.textBoxDisplayName.TabIndex = 1;
      // 
      // labelStartupFolder
      // 
      this.labelStartupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelStartupFolder.Location = new System.Drawing.Point(8, 88);
      this.labelStartupFolder.Name = "labelStartupFolder";
      this.labelStartupFolder.Size = new System.Drawing.Size(336, 16);
      this.labelStartupFolder.TabIndex = 5;
      this.labelStartupFolder.Text = "Application start folder:";
      // 
      // buttonStartupFolder
      // 
      this.buttonStartupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonStartupFolder.Location = new System.Drawing.Point(352, 104);
      this.buttonStartupFolder.Name = "buttonStartupFolder";
      this.buttonStartupFolder.Size = new System.Drawing.Size(24, 20);
      this.buttonStartupFolder.TabIndex = 7;
      this.buttonStartupFolder.Text = "...";
      this.buttonStartupFolder.UseVisualStyleBackColor = true;
      this.buttonStartupFolder.Click += new System.EventHandler(this.buttonStartupFolder_Click);
      // 
      // textBoxAppStartFolder
      // 
      this.textBoxAppStartFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAppStartFolder.Location = new System.Drawing.Point(8, 104);
      this.textBoxAppStartFolder.Name = "textBoxAppStartFolder";
      this.textBoxAppStartFolder.Size = new System.Drawing.Size(336, 20);
      this.textBoxAppStartFolder.TabIndex = 6;
      // 
      // labelAppParams
      // 
      this.labelAppParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelAppParams.Location = new System.Drawing.Point(8, 136);
      this.labelAppParams.Name = "labelAppParams";
      this.labelAppParams.Size = new System.Drawing.Size(368, 16);
      this.labelAppParams.TabIndex = 8;
      this.labelAppParams.Text = "Application parameters:";
      // 
      // textBoxApplicationParameters
      // 
      this.textBoxApplicationParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxApplicationParameters.Location = new System.Drawing.Point(8, 152);
      this.textBoxApplicationParameters.Name = "textBoxApplicationParameters";
      this.textBoxApplicationParameters.Size = new System.Drawing.Size(368, 20);
      this.textBoxApplicationParameters.TabIndex = 9;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(224, 248);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(72, 24);
      this.buttonOK.TabIndex = 15;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(304, 248);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(72, 24);
      this.buttonCancel.TabIndex = 16;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(8, 248);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(72, 24);
      this.buttonTest.TabIndex = 14;
      this.buttonTest.Text = "Test";
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // checkBoxShellExecute
      // 
      this.checkBoxShellExecute.Location = new System.Drawing.Point(8, 216);
      this.checkBoxShellExecute.Name = "checkBoxShellExecute";
      this.checkBoxShellExecute.Size = new System.Drawing.Size(176, 24);
      this.checkBoxShellExecute.TabIndex = 12;
      this.checkBoxShellExecute.Text = "Start using ShellExecute";
      this.checkBoxShellExecute.UseVisualStyleBackColor = true;
      // 
      // comboBoxWindowStyle
      // 
      this.comboBoxWindowStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxWindowStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxWindowStyle.FormattingEnabled = true;
      this.comboBoxWindowStyle.Location = new System.Drawing.Point(96, 184);
      this.comboBoxWindowStyle.MaxDropDownItems = 4;
      this.comboBoxWindowStyle.Name = "comboBoxWindowStyle";
      this.comboBoxWindowStyle.Size = new System.Drawing.Size(152, 21);
      this.comboBoxWindowStyle.TabIndex = 11;
      // 
      // labelWindowStyle
      // 
      this.labelWindowStyle.Location = new System.Drawing.Point(8, 184);
      this.labelWindowStyle.Name = "labelWindowStyle";
      this.labelWindowStyle.Size = new System.Drawing.Size(88, 21);
      this.labelWindowStyle.TabIndex = 10;
      this.labelWindowStyle.Text = "Window style:";
      this.labelWindowStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // checkBoxIgnoreSystemWide
      // 
      this.checkBoxIgnoreSystemWide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxIgnoreSystemWide.Location = new System.Drawing.Point(200, 216);
      this.checkBoxIgnoreSystemWide.Name = "checkBoxIgnoreSystemWide";
      this.checkBoxIgnoreSystemWide.Size = new System.Drawing.Size(176, 24);
      this.checkBoxIgnoreSystemWide.TabIndex = 13;
      this.checkBoxIgnoreSystemWide.Text = "Ignore System-Wide mappings";
      this.checkBoxIgnoreSystemWide.UseVisualStyleBackColor = true;
      // 
      // EditProgramForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 281);
      this.Controls.Add(this.checkBoxIgnoreSystemWide);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.checkBoxShellExecute);
      this.Controls.Add(this.comboBoxWindowStyle);
      this.Controls.Add(this.labelWindowStyle);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.labelAppParams);
      this.Controls.Add(this.textBoxApplicationParameters);
      this.Controls.Add(this.labelStartupFolder);
      this.Controls.Add(this.buttonStartupFolder);
      this.Controls.Add(this.textBoxAppStartFolder);
      this.Controls.Add(this.textBoxDisplayName);
      this.Controls.Add(this.labelAppDisplayName);
      this.Controls.Add(this.labelApplication);
      this.Controls.Add(this.buttonLocate);
      this.Controls.Add(this.textBoxApp);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(392, 308);
      this.Name = "EditProgramForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Application";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxApp;
    private System.Windows.Forms.Button buttonLocate;
    private System.Windows.Forms.Label labelApplication;
    private System.Windows.Forms.Label labelAppDisplayName;
    private System.Windows.Forms.TextBox textBoxDisplayName;
    private System.Windows.Forms.Label labelStartupFolder;
    private System.Windows.Forms.Button buttonStartupFolder;
    private System.Windows.Forms.TextBox textBoxAppStartFolder;
    private System.Windows.Forms.Label labelAppParams;
    private System.Windows.Forms.TextBox textBoxApplicationParameters;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.CheckBox checkBoxShellExecute;
    private System.Windows.Forms.ComboBox comboBoxWindowStyle;
    private System.Windows.Forms.Label labelWindowStyle;
    private System.Windows.Forms.CheckBox checkBoxIgnoreSystemWide;
  }
}