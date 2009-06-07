namespace IrssUtils.Forms
{

  partial class MessageCommand
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
      this.numericUpDownLParam = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownWParam = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownMsg = new System.Windows.Forms.NumericUpDown();
      this.contextMenuStripWM = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.wMAPPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.wMUSERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.labelLParam = new System.Windows.Forms.Label();
      this.labelWParam = new System.Windows.Forms.Label();
      this.labelMessage = new System.Windows.Forms.Label();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxTarget = new System.Windows.Forms.GroupBox();
      this.radioButtonWindowTitle = new System.Windows.Forms.RadioButton();
      this.radioButtonClass = new System.Windows.Forms.RadioButton();
      this.radioButtonApplication = new System.Windows.Forms.RadioButton();
      this.radioButtonActiveWindow = new System.Windows.Forms.RadioButton();
      this.groupBoxDetails = new System.Windows.Forms.GroupBox();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLParam)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWParam)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMsg)).BeginInit();
      this.contextMenuStripWM.SuspendLayout();
      this.groupBoxTarget.SuspendLayout();
      this.groupBoxDetails.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonFindMsgTarget
      // 
      this.buttonFindMsgTarget.Location = new System.Drawing.Point(240, 80);
      this.buttonFindMsgTarget.Name = "buttonFindMsgTarget";
      this.buttonFindMsgTarget.Size = new System.Drawing.Size(24, 20);
      this.buttonFindMsgTarget.TabIndex = 5;
      this.buttonFindMsgTarget.Text = "...";
      this.toolTip.SetToolTip(this.buttonFindMsgTarget, "Locate a target for the message");
      this.buttonFindMsgTarget.UseVisualStyleBackColor = true;
      this.buttonFindMsgTarget.Click += new System.EventHandler(this.buttonFindMsgApp_Click);
      // 
      // textBoxMsgTarget
      // 
      this.textBoxMsgTarget.Location = new System.Drawing.Point(8, 80);
      this.textBoxMsgTarget.Name = "textBoxMsgTarget";
      this.textBoxMsgTarget.Size = new System.Drawing.Size(224, 20);
      this.textBoxMsgTarget.TabIndex = 4;
      this.toolTip.SetToolTip(this.textBoxMsgTarget, "Message target");
      // 
      // numericUpDownLParam
      // 
      this.numericUpDownLParam.Location = new System.Drawing.Point(144, 88);
      this.numericUpDownLParam.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.numericUpDownLParam.Name = "numericUpDownLParam";
      this.numericUpDownLParam.Size = new System.Drawing.Size(120, 20);
      this.numericUpDownLParam.TabIndex = 5;
      this.numericUpDownLParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownLParam.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownLParam, "Long Parameter (or LParam)");
      // 
      // numericUpDownWParam
      // 
      this.numericUpDownWParam.Location = new System.Drawing.Point(144, 56);
      this.numericUpDownWParam.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.numericUpDownWParam.Name = "numericUpDownWParam";
      this.numericUpDownWParam.Size = new System.Drawing.Size(120, 20);
      this.numericUpDownWParam.TabIndex = 3;
      this.numericUpDownWParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownWParam.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownWParam, "Word Paramater (or WParam)");
      // 
      // numericUpDownMsg
      // 
      this.numericUpDownMsg.ContextMenuStrip = this.contextMenuStripWM;
      this.numericUpDownMsg.Location = new System.Drawing.Point(144, 24);
      this.numericUpDownMsg.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.numericUpDownMsg.Name = "numericUpDownMsg";
      this.numericUpDownMsg.Size = new System.Drawing.Size(120, 20);
      this.numericUpDownMsg.TabIndex = 1;
      this.numericUpDownMsg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownMsg.ThousandsSeparator = true;
      this.toolTip.SetToolTip(this.numericUpDownMsg, "Message identifier");
      // 
      // contextMenuStripWM
      // 
      this.contextMenuStripWM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wMAPPToolStripMenuItem,
            this.wMUSERToolStripMenuItem});
      this.contextMenuStripWM.Name = "contextMenuStripWM";
      this.contextMenuStripWM.Size = new System.Drawing.Size(136, 48);
      // 
      // wMAPPToolStripMenuItem
      // 
      this.wMAPPToolStripMenuItem.Name = "wMAPPToolStripMenuItem";
      this.wMAPPToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
      this.wMAPPToolStripMenuItem.Text = "WM_APP";
      this.wMAPPToolStripMenuItem.Click += new System.EventHandler(this.wMAPPToolStripMenuItem_Click);
      // 
      // wMUSERToolStripMenuItem
      // 
      this.wMUSERToolStripMenuItem.Name = "wMUSERToolStripMenuItem";
      this.wMUSERToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
      this.wMUSERToolStripMenuItem.Text = "WM_USER";
      this.wMUSERToolStripMenuItem.Click += new System.EventHandler(this.wMUSERToolStripMenuItem_Click);
      // 
      // labelLParam
      // 
      this.labelLParam.Location = new System.Drawing.Point(8, 88);
      this.labelLParam.Name = "labelLParam";
      this.labelLParam.Size = new System.Drawing.Size(128, 20);
      this.labelLParam.TabIndex = 4;
      this.labelLParam.Text = "Long Param:";
      this.labelLParam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelWParam
      // 
      this.labelWParam.Location = new System.Drawing.Point(8, 56);
      this.labelWParam.Name = "labelWParam";
      this.labelWParam.Size = new System.Drawing.Size(128, 20);
      this.labelWParam.TabIndex = 2;
      this.labelWParam.Text = "Word Param:";
      this.labelWParam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelMessage
      // 
      this.labelMessage.Location = new System.Drawing.Point(8, 24);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new System.Drawing.Size(128, 20);
      this.labelMessage.TabIndex = 0;
      this.labelMessage.Text = "Message:";
      this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      this.toolTip.SetToolTip(this.radioButtonWindowTitle, "Send the message to the window with the specified title");
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
      this.toolTip.SetToolTip(this.radioButtonClass, "Send the message to the specified class");
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
      this.toolTip.SetToolTip(this.radioButtonApplication, "Send the message to the specified application");
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
      this.toolTip.SetToolTip(this.radioButtonActiveWindow, "Send the message to the active window");
      this.radioButtonActiveWindow.UseVisualStyleBackColor = true;
      this.radioButtonActiveWindow.CheckedChanged += new System.EventHandler(this.radioButtonActiveWindow_CheckedChanged);
      // 
      // groupBoxDetails
      // 
      this.groupBoxDetails.Controls.Add(this.labelMessage);
      this.groupBoxDetails.Controls.Add(this.labelWParam);
      this.groupBoxDetails.Controls.Add(this.labelLParam);
      this.groupBoxDetails.Controls.Add(this.numericUpDownMsg);
      this.groupBoxDetails.Controls.Add(this.numericUpDownWParam);
      this.groupBoxDetails.Controls.Add(this.numericUpDownLParam);
      this.groupBoxDetails.Location = new System.Drawing.Point(8, 128);
      this.groupBoxDetails.Name = "groupBoxDetails";
      this.groupBoxDetails.Size = new System.Drawing.Size(272, 120);
      this.groupBoxDetails.TabIndex = 1;
      this.groupBoxDetails.TabStop = false;
      this.groupBoxDetails.Text = "Message Details";
      // 
      // MessageCommand
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(288, 288);
      this.Controls.Add(this.groupBoxDetails);
      this.Controls.Add(this.groupBoxTarget);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(296, 322);
      this.Name = "MessageCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Window Message Command";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLParam)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWParam)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMsg)).EndInit();
      this.contextMenuStripWM.ResumeLayout(false);
      this.groupBoxTarget.ResumeLayout(false);
      this.groupBoxTarget.PerformLayout();
      this.groupBoxDetails.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonFindMsgTarget;
    private System.Windows.Forms.TextBox textBoxMsgTarget;
    private System.Windows.Forms.NumericUpDown numericUpDownLParam;
    private System.Windows.Forms.NumericUpDown numericUpDownWParam;
    private System.Windows.Forms.NumericUpDown numericUpDownMsg;
    private System.Windows.Forms.Label labelLParam;
    private System.Windows.Forms.Label labelWParam;
    private System.Windows.Forms.Label labelMessage;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripWM;
    private System.Windows.Forms.ToolStripMenuItem wMAPPToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem wMUSERToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBoxTarget;
    private System.Windows.Forms.GroupBox groupBoxDetails;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.RadioButton radioButtonActiveWindow;
    private System.Windows.Forms.RadioButton radioButtonWindowTitle;
    private System.Windows.Forms.RadioButton radioButtonClass;
    private System.Windows.Forms.RadioButton radioButtonApplication;
  }

}
