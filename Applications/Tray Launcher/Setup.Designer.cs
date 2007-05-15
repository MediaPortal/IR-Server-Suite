namespace TrayLauncher
{
  partial class Setup
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setup));
      this.checkBoxAuto = new System.Windows.Forms.CheckBox();
      this.groupBoxApp = new System.Windows.Forms.GroupBox();
      this.textBoxApplication = new System.Windows.Forms.TextBox();
      this.buttonFind = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.checkBoxLaunchOnLoad = new System.Windows.Forms.CheckBox();
      this.groupBoxOptions = new System.Windows.Forms.GroupBox();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.buttonRemoteButton = new System.Windows.Forms.Button();
      this.groupBoxServerHost = new System.Windows.Forms.GroupBox();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.groupBoxApp.SuspendLayout();
      this.groupBoxOptions.SuspendLayout();
      this.groupBoxServerHost.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBoxAuto
      // 
      this.checkBoxAuto.Location = new System.Drawing.Point(8, 16);
      this.checkBoxAuto.Name = "checkBoxAuto";
      this.checkBoxAuto.Size = new System.Drawing.Size(120, 24);
      this.checkBoxAuto.TabIndex = 0;
      this.checkBoxAuto.Text = "Run at boot time";
      this.toolTip.SetToolTip(this.checkBoxAuto, "Run Tray Launcher when windows boots up?");
      this.checkBoxAuto.UseVisualStyleBackColor = true;
      // 
      // groupBoxApp
      // 
      this.groupBoxApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxApp.Controls.Add(this.textBoxApplication);
      this.groupBoxApp.Controls.Add(this.buttonFind);
      this.groupBoxApp.Location = new System.Drawing.Point(8, 72);
      this.groupBoxApp.Name = "groupBoxApp";
      this.groupBoxApp.Size = new System.Drawing.Size(264, 56);
      this.groupBoxApp.TabIndex = 1;
      this.groupBoxApp.TabStop = false;
      this.groupBoxApp.Text = "Program to launch";
      // 
      // textBoxApplication
      // 
      this.textBoxApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxApplication.Location = new System.Drawing.Point(8, 24);
      this.textBoxApplication.Name = "textBoxApplication";
      this.textBoxApplication.Size = new System.Drawing.Size(216, 20);
      this.textBoxApplication.TabIndex = 0;
      this.toolTip.SetToolTip(this.textBoxApplication, "Enter the full path and file name of the program to launch");
      // 
      // buttonFind
      // 
      this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonFind.Location = new System.Drawing.Point(232, 24);
      this.buttonFind.Name = "buttonFind";
      this.buttonFind.Size = new System.Drawing.Size(24, 20);
      this.buttonFind.TabIndex = 1;
      this.buttonFind.Text = "...";
      this.toolTip.SetToolTip(this.buttonFind, "Click here to select the program to launch");
      this.buttonFind.UseVisualStyleBackColor = true;
      this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(152, 192);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(216, 192);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // checkBoxLaunchOnLoad
      // 
      this.checkBoxLaunchOnLoad.Location = new System.Drawing.Point(136, 16);
      this.checkBoxLaunchOnLoad.Name = "checkBoxLaunchOnLoad";
      this.checkBoxLaunchOnLoad.Size = new System.Drawing.Size(120, 24);
      this.checkBoxLaunchOnLoad.TabIndex = 1;
      this.checkBoxLaunchOnLoad.Text = "Launch on load";
      this.toolTip.SetToolTip(this.checkBoxLaunchOnLoad, "Launch the program whenever Tray Launcher loads?");
      this.checkBoxLaunchOnLoad.UseVisualStyleBackColor = true;
      // 
      // groupBoxOptions
      // 
      this.groupBoxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxOptions.Controls.Add(this.checkBoxAuto);
      this.groupBoxOptions.Controls.Add(this.checkBoxLaunchOnLoad);
      this.groupBoxOptions.Location = new System.Drawing.Point(8, 136);
      this.groupBoxOptions.Name = "groupBoxOptions";
      this.groupBoxOptions.Size = new System.Drawing.Size(264, 48);
      this.groupBoxOptions.TabIndex = 2;
      this.groupBoxOptions.TabStop = false;
      this.groupBoxOptions.Text = "Options";
      // 
      // buttonRemoteButton
      // 
      this.buttonRemoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonRemoteButton.Location = new System.Drawing.Point(8, 192);
      this.buttonRemoteButton.Name = "buttonRemoteButton";
      this.buttonRemoteButton.Size = new System.Drawing.Size(96, 24);
      this.buttonRemoteButton.TabIndex = 3;
      this.buttonRemoteButton.Text = "Remote Button";
      this.toolTip.SetToolTip(this.buttonRemoteButton, "Set the remote button used to launch");
      this.buttonRemoteButton.UseVisualStyleBackColor = true;
      this.buttonRemoteButton.Click += new System.EventHandler(this.buttonRemoteButton_Click);
      // 
      // groupBoxServerHost
      // 
      this.groupBoxServerHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxServerHost.Controls.Add(this.comboBoxComputer);
      this.groupBoxServerHost.Location = new System.Drawing.Point(8, 8);
      this.groupBoxServerHost.Name = "groupBoxServerHost";
      this.groupBoxServerHost.Size = new System.Drawing.Size(264, 56);
      this.groupBoxServerHost.TabIndex = 0;
      this.groupBoxServerHost.TabStop = false;
      this.groupBoxServerHost.Text = "IR Server host";
      // 
      // comboBoxComputer
      // 
      this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxComputer.FormattingEnabled = true;
      this.comboBoxComputer.Location = new System.Drawing.Point(8, 24);
      this.comboBoxComputer.Name = "comboBoxComputer";
      this.comboBoxComputer.Size = new System.Drawing.Size(248, 21);
      this.comboBoxComputer.TabIndex = 1;
      // 
      // Setup
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(280, 224);
      this.Controls.Add(this.buttonRemoteButton);
      this.Controls.Add(this.groupBoxServerHost);
      this.Controls.Add(this.groupBoxOptions);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.groupBoxApp);
      this.Controls.Add(this.buttonOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(288, 258);
      this.Name = "Setup";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Tray Launcher - Setup";
      this.groupBoxApp.ResumeLayout(false);
      this.groupBoxApp.PerformLayout();
      this.groupBoxOptions.ResumeLayout(false);
      this.groupBoxServerHost.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox checkBoxAuto;
    private System.Windows.Forms.GroupBox groupBoxApp;
    private System.Windows.Forms.TextBox textBoxApplication;
    private System.Windows.Forms.Button buttonFind;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.CheckBox checkBoxLaunchOnLoad;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxOptions;
    private System.Windows.Forms.GroupBox groupBoxServerHost;
    private System.Windows.Forms.Button buttonRemoteButton;
    private System.Windows.Forms.ComboBox comboBoxComputer;
  }
}