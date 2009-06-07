namespace IrssUtils.Forms
{

  partial class WindowCommand
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
      this.buttonFindMsgTarget = new System.Windows.Forms.Button();
      this.textBoxMsgTarget = new System.Windows.Forms.TextBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxTarget = new System.Windows.Forms.GroupBox();
      this.radioButtonWindowTitle = new System.Windows.Forms.RadioButton();
      this.radioButtonClass = new System.Windows.Forms.RadioButton();
      this.radioButtonApplication = new System.Windows.Forms.RadioButton();
      this.radioButtonActiveWindow = new System.Windows.Forms.RadioButton();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxAction = new System.Windows.Forms.GroupBox();
      this.radioButtonRestore = new System.Windows.Forms.RadioButton();
      this.radioButtonMinimize = new System.Windows.Forms.RadioButton();
      this.radioButtonMaximize = new System.Windows.Forms.RadioButton();
      this.radioButtonHide = new System.Windows.Forms.RadioButton();
      this.radioButtonUnhide = new System.Windows.Forms.RadioButton();
      this.groupBoxTarget.SuspendLayout();
      this.groupBoxAction.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonFindMsgTarget
      // 
      this.buttonFindMsgTarget.Location = new System.Drawing.Point(240, 80);
      this.buttonFindMsgTarget.Name = "buttonFindMsgTarget";
      this.buttonFindMsgTarget.Size = new System.Drawing.Size(24, 20);
      this.buttonFindMsgTarget.TabIndex = 5;
      this.buttonFindMsgTarget.Text = "...";
      this.toolTip.SetToolTip(this.buttonFindMsgTarget, "Locate a target");
      this.buttonFindMsgTarget.UseVisualStyleBackColor = true;
      this.buttonFindMsgTarget.Click += new System.EventHandler(this.buttonFindMsgApp_Click);
      // 
      // textBoxMsgTarget
      // 
      this.textBoxMsgTarget.Location = new System.Drawing.Point(8, 80);
      this.textBoxMsgTarget.Name = "textBoxMsgTarget";
      this.textBoxMsgTarget.Size = new System.Drawing.Size(224, 20);
      this.textBoxMsgTarget.TabIndex = 4;
      this.toolTip.SetToolTip(this.textBoxMsgTarget, "Target");
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(216, 256);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(144, 256);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // groupBoxTarget
      // 
      this.groupBoxTarget.Controls.Add(this.radioButtonWindowTitle);
      this.groupBoxTarget.Controls.Add(this.radioButtonClass);
      this.groupBoxTarget.Controls.Add(this.radioButtonApplication);
      this.groupBoxTarget.Controls.Add(this.radioButtonActiveWindow);
      this.groupBoxTarget.Controls.Add(this.textBoxMsgTarget);
      this.groupBoxTarget.Controls.Add(this.buttonFindMsgTarget);
      this.groupBoxTarget.Location = new System.Drawing.Point(8, 128);
      this.groupBoxTarget.Name = "groupBoxTarget";
      this.groupBoxTarget.Size = new System.Drawing.Size(272, 112);
      this.groupBoxTarget.TabIndex = 1;
      this.groupBoxTarget.TabStop = false;
      this.groupBoxTarget.Text = "Target";
      // 
      // radioButtonWindowTitle
      // 
      this.radioButtonWindowTitle.Location = new System.Drawing.Point(152, 48);
      this.radioButtonWindowTitle.Name = "radioButtonWindowTitle";
      this.radioButtonWindowTitle.Size = new System.Drawing.Size(112, 16);
      this.radioButtonWindowTitle.TabIndex = 3;
      this.radioButtonWindowTitle.TabStop = true;
      this.radioButtonWindowTitle.Text = "Window title";
      this.toolTip.SetToolTip(this.radioButtonWindowTitle, "Target the specified window title");
      this.radioButtonWindowTitle.UseVisualStyleBackColor = true;
      this.radioButtonWindowTitle.CheckedChanged += new System.EventHandler(this.radioButtonWindowTitle_CheckedChanged);
      // 
      // radioButtonClass
      // 
      this.radioButtonClass.Location = new System.Drawing.Point(8, 48);
      this.radioButtonClass.Name = "radioButtonClass";
      this.radioButtonClass.Size = new System.Drawing.Size(112, 16);
      this.radioButtonClass.TabIndex = 2;
      this.radioButtonClass.TabStop = true;
      this.radioButtonClass.Text = "Class";
      this.toolTip.SetToolTip(this.radioButtonClass, "Target a specific class");
      this.radioButtonClass.UseVisualStyleBackColor = true;
      this.radioButtonClass.CheckedChanged += new System.EventHandler(this.radioButtonClass_CheckedChanged);
      // 
      // radioButtonApplication
      // 
      this.radioButtonApplication.Location = new System.Drawing.Point(152, 24);
      this.radioButtonApplication.Name = "radioButtonApplication";
      this.radioButtonApplication.Size = new System.Drawing.Size(112, 16);
      this.radioButtonApplication.TabIndex = 1;
      this.radioButtonApplication.TabStop = true;
      this.radioButtonApplication.Text = "Application";
      this.toolTip.SetToolTip(this.radioButtonApplication, "Target a specific application");
      this.radioButtonApplication.UseVisualStyleBackColor = true;
      this.radioButtonApplication.CheckedChanged += new System.EventHandler(this.radioButtonApplication_CheckedChanged);
      // 
      // radioButtonActiveWindow
      // 
      this.radioButtonActiveWindow.Location = new System.Drawing.Point(8, 24);
      this.radioButtonActiveWindow.Name = "radioButtonActiveWindow";
      this.radioButtonActiveWindow.Size = new System.Drawing.Size(112, 16);
      this.radioButtonActiveWindow.TabIndex = 0;
      this.radioButtonActiveWindow.Text = "Active window";
      this.toolTip.SetToolTip(this.radioButtonActiveWindow, "Target the active window");
      this.radioButtonActiveWindow.UseVisualStyleBackColor = true;
      this.radioButtonActiveWindow.CheckedChanged += new System.EventHandler(this.radioButtonActiveWindow_CheckedChanged);
      // 
      // groupBoxAction
      // 
      this.groupBoxAction.Controls.Add(this.radioButtonUnhide);
      this.groupBoxAction.Controls.Add(this.radioButtonHide);
      this.groupBoxAction.Controls.Add(this.radioButtonMaximize);
      this.groupBoxAction.Controls.Add(this.radioButtonMinimize);
      this.groupBoxAction.Controls.Add(this.radioButtonRestore);
      this.groupBoxAction.Location = new System.Drawing.Point(8, 8);
      this.groupBoxAction.Name = "groupBoxAction";
      this.groupBoxAction.Size = new System.Drawing.Size(272, 104);
      this.groupBoxAction.TabIndex = 0;
      this.groupBoxAction.TabStop = false;
      this.groupBoxAction.Text = "Action";
      // 
      // radioButtonRestore
      // 
      this.radioButtonRestore.Location = new System.Drawing.Point(8, 24);
      this.radioButtonRestore.Name = "radioButtonRestore";
      this.radioButtonRestore.Size = new System.Drawing.Size(112, 16);
      this.radioButtonRestore.TabIndex = 0;
      this.radioButtonRestore.TabStop = true;
      this.radioButtonRestore.Text = "Restore";
      this.toolTip.SetToolTip(this.radioButtonRestore, "Restore a window to the foreground");
      this.radioButtonRestore.UseVisualStyleBackColor = true;
      // 
      // radioButtonMinimize
      // 
      this.radioButtonMinimize.Location = new System.Drawing.Point(8, 48);
      this.radioButtonMinimize.Name = "radioButtonMinimize";
      this.radioButtonMinimize.Size = new System.Drawing.Size(112, 16);
      this.radioButtonMinimize.TabIndex = 1;
      this.radioButtonMinimize.TabStop = true;
      this.radioButtonMinimize.Text = "Minimize";
      this.toolTip.SetToolTip(this.radioButtonMinimize, "Minimize a window");
      this.radioButtonMinimize.UseVisualStyleBackColor = true;
      // 
      // radioButtonMaximize
      // 
      this.radioButtonMaximize.Location = new System.Drawing.Point(152, 48);
      this.radioButtonMaximize.Name = "radioButtonMaximize";
      this.radioButtonMaximize.Size = new System.Drawing.Size(112, 16);
      this.radioButtonMaximize.TabIndex = 2;
      this.radioButtonMaximize.TabStop = true;
      this.radioButtonMaximize.Text = "Maximize";
      this.toolTip.SetToolTip(this.radioButtonMaximize, "Maximize a window");
      this.radioButtonMaximize.UseVisualStyleBackColor = true;
      // 
      // radioButtonHide
      // 
      this.radioButtonHide.Location = new System.Drawing.Point(8, 72);
      this.radioButtonHide.Name = "radioButtonHide";
      this.radioButtonHide.Size = new System.Drawing.Size(112, 16);
      this.radioButtonHide.TabIndex = 3;
      this.radioButtonHide.TabStop = true;
      this.radioButtonHide.Text = "Hide";
      this.toolTip.SetToolTip(this.radioButtonHide, "Maximize a window");
      this.radioButtonHide.UseVisualStyleBackColor = true;
      // 
      // radioButtonUnhide
      // 
      this.radioButtonUnhide.Location = new System.Drawing.Point(152, 72);
      this.radioButtonUnhide.Name = "radioButtonUnhide";
      this.radioButtonUnhide.Size = new System.Drawing.Size(112, 16);
      this.radioButtonUnhide.TabIndex = 4;
      this.radioButtonUnhide.TabStop = true;
      this.radioButtonUnhide.Text = "Unhide";
      this.toolTip.SetToolTip(this.radioButtonUnhide, "Maximize a window");
      this.radioButtonUnhide.UseVisualStyleBackColor = true;
      // 
      // WindowCommand
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(288, 288);
      this.Controls.Add(this.groupBoxAction);
      this.Controls.Add(this.groupBoxTarget);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(296, 322);
      this.Name = "WindowCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Window State Command";
      this.groupBoxTarget.ResumeLayout(false);
      this.groupBoxTarget.PerformLayout();
      this.groupBoxAction.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonFindMsgTarget;
    private System.Windows.Forms.TextBox textBoxMsgTarget;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.GroupBox groupBoxTarget;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.RadioButton radioButtonActiveWindow;
    private System.Windows.Forms.RadioButton radioButtonWindowTitle;
    private System.Windows.Forms.RadioButton radioButtonClass;
    private System.Windows.Forms.RadioButton radioButtonApplication;
    private System.Windows.Forms.GroupBox groupBoxAction;
    private System.Windows.Forms.RadioButton radioButtonUnhide;
    private System.Windows.Forms.RadioButton radioButtonHide;
    private System.Windows.Forms.RadioButton radioButtonMaximize;
    private System.Windows.Forms.RadioButton radioButtonMinimize;
    private System.Windows.Forms.RadioButton radioButtonRestore;
  }

}
