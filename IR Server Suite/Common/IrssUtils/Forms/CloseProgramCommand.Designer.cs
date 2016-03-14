namespace IrssUtils.Forms
{

  partial class CloseProgramCommand
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
            this.buttonFindTarget = new System.Windows.Forms.Button();
            this.textBoxTarget = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBoxTarget = new System.Windows.Forms.GroupBox();
            this.radioButtonWindowTitle = new System.Windows.Forms.RadioButton();
            this.radioButtonClass = new System.Windows.Forms.RadioButton();
            this.radioButtonApplication = new System.Windows.Forms.RadioButton();
            this.radioButtonActiveWindow = new System.Windows.Forms.RadioButton();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonFindTarget
            // 
            this.buttonFindTarget.Location = new System.Drawing.Point(240, 80);
            this.buttonFindTarget.Name = "buttonFindTarget";
            this.buttonFindTarget.Size = new System.Drawing.Size(24, 20);
            this.buttonFindTarget.TabIndex = 5;
            this.buttonFindTarget.Text = "...";
            this.toolTips.SetToolTip(this.buttonFindTarget, "Locate a target to close");
            this.buttonFindTarget.UseVisualStyleBackColor = true;
            this.buttonFindTarget.Click += new System.EventHandler(this.buttonFindApp_Click);
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.Location = new System.Drawing.Point(8, 80);
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.Size = new System.Drawing.Size(224, 20);
            this.textBoxTarget.TabIndex = 4;
            this.toolTips.SetToolTip(this.textBoxTarget, "Program to close");
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(216, 128);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 24);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(144, 128);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(64, 24);
            this.buttonOK.TabIndex = 1;
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
            this.groupBoxTarget.Controls.Add(this.textBoxTarget);
            this.groupBoxTarget.Controls.Add(this.buttonFindTarget);
            this.groupBoxTarget.Location = new System.Drawing.Point(8, 8);
            this.groupBoxTarget.Name = "groupBoxTarget";
            this.groupBoxTarget.Size = new System.Drawing.Size(272, 112);
            this.groupBoxTarget.TabIndex = 0;
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
            this.toolTips.SetToolTip(this.radioButtonWindowTitle, "Close the window with the specified title");
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
            this.toolTips.SetToolTip(this.radioButtonClass, "Close the window with the specified class");
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
            this.toolTips.SetToolTip(this.radioButtonApplication, "Close the specified application");
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
            this.toolTips.SetToolTip(this.radioButtonActiveWindow, "Close the active window");
            this.radioButtonActiveWindow.UseVisualStyleBackColor = true;
            this.radioButtonActiveWindow.CheckedChanged += new System.EventHandler(this.radioButtonActiveWindow_CheckedChanged);
            // 
            // CloseProgramCommand
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(288, 161);
            this.Controls.Add(this.groupBoxTarget);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(296, 188);
            this.Name = "CloseProgramCommand";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Close Program Command";
            this.groupBoxTarget.ResumeLayout(false);
            this.groupBoxTarget.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonFindTarget;
    private System.Windows.Forms.TextBox textBoxTarget;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.GroupBox groupBoxTarget;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.RadioButton radioButtonActiveWindow;
    private System.Windows.Forms.RadioButton radioButtonWindowTitle;
    private System.Windows.Forms.RadioButton radioButtonClass;
    private System.Windows.Forms.RadioButton radioButtonApplication;
  }

}
