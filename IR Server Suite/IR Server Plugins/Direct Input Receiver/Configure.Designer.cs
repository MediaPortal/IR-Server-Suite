namespace IRServer.Plugin
{
  partial class Configure
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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.buttonConfigure = new System.Windows.Forms.Button();
      this.listViewDevices = new System.Windows.Forms.ListView();
      this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderDevice = new System.Windows.Forms.ColumnHeader();
      this.radioButtonNone = new System.Windows.Forms.RadioButton();
      this.groupBoxMouseMovement = new System.Windows.Forms.GroupBox();
      this.radioButtonAxesXY = new System.Windows.Forms.RadioButton();
      this.radioButtonRotationXY = new System.Windows.Forms.RadioButton();
      this.groupBoxMouseButtons = new System.Windows.Forms.GroupBox();
      this.labelLeft = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxLeft = new System.Windows.Forms.ComboBox();
      this.comboBoxMiddle = new System.Windows.Forms.ComboBox();
      this.comboBoxRight = new System.Windows.Forms.ComboBox();
      this.groupBoxMouseMovement.SuspendLayout();
      this.groupBoxMouseButtons.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(168, 264);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(240, 264);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonConfigure
      // 
      this.buttonConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonConfigure.Location = new System.Drawing.Point(8, 264);
      this.buttonConfigure.Name = "buttonConfigure";
      this.buttonConfigure.Size = new System.Drawing.Size(64, 24);
      this.buttonConfigure.TabIndex = 6;
      this.buttonConfigure.Text = "Configure";
      this.toolTips.SetToolTip(this.buttonConfigure, "Configure the selected device");
      this.buttonConfigure.UseVisualStyleBackColor = true;
      this.buttonConfigure.Click += new System.EventHandler(this.buttonConfigure_Click);
      // 
      // listViewDevices
      // 
      this.listViewDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderDevice});
      this.listViewDevices.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewDevices.HideSelection = false;
      this.listViewDevices.Location = new System.Drawing.Point(8, 8);
      this.listViewDevices.MultiSelect = false;
      this.listViewDevices.Name = "listViewDevices";
      this.listViewDevices.ShowGroups = false;
      this.listViewDevices.Size = new System.Drawing.Size(296, 104);
      this.listViewDevices.TabIndex = 7;
      this.toolTips.SetToolTip(this.listViewDevices, "Select the device to use");
      this.listViewDevices.UseCompatibleStateImageBehavior = false;
      this.listViewDevices.View = System.Windows.Forms.View.Details;
      this.listViewDevices.SelectedIndexChanged += new System.EventHandler(this.listViewDevices_SelectedIndexChanged);
      // 
      // columnHeaderName
      // 
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 108;
      // 
      // columnHeaderDevice
      // 
      this.columnHeaderDevice.Text = "Device";
      this.columnHeaderDevice.Width = 124;
      // 
      // radioButtonNone
      // 
      this.radioButtonNone.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.radioButtonNone.AutoSize = true;
      this.radioButtonNone.Checked = true;
      this.radioButtonNone.Location = new System.Drawing.Point(8, 24);
      this.radioButtonNone.Name = "radioButtonNone";
      this.radioButtonNone.Size = new System.Drawing.Size(51, 17);
      this.radioButtonNone.TabIndex = 8;
      this.radioButtonNone.TabStop = true;
      this.radioButtonNone.Text = "None";
      this.toolTips.SetToolTip(this.radioButtonNone, "Don\'t use the controller to move the mouse");
      this.radioButtonNone.UseVisualStyleBackColor = true;
      // 
      // groupBoxMouseMovement
      // 
      this.groupBoxMouseMovement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMouseMovement.Controls.Add(this.radioButtonRotationXY);
      this.groupBoxMouseMovement.Controls.Add(this.radioButtonAxesXY);
      this.groupBoxMouseMovement.Controls.Add(this.radioButtonNone);
      this.groupBoxMouseMovement.Enabled = false;
      this.groupBoxMouseMovement.Location = new System.Drawing.Point(8, 120);
      this.groupBoxMouseMovement.Name = "groupBoxMouseMovement";
      this.groupBoxMouseMovement.Size = new System.Drawing.Size(296, 56);
      this.groupBoxMouseMovement.TabIndex = 9;
      this.groupBoxMouseMovement.TabStop = false;
      this.groupBoxMouseMovement.Text = "Mouse movement";
      // 
      // radioButtonAxesXY
      // 
      this.radioButtonAxesXY.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.radioButtonAxesXY.AutoSize = true;
      this.radioButtonAxesXY.Location = new System.Drawing.Point(72, 24);
      this.radioButtonAxesXY.Name = "radioButtonAxesXY";
      this.radioButtonAxesXY.Size = new System.Drawing.Size(91, 17);
      this.radioButtonAxesXY.TabIndex = 9;
      this.radioButtonAxesXY.Text = "Use X/Y axes";
      this.toolTips.SetToolTip(this.radioButtonAxesXY, "Use the X and Y axes to move the mouse");
      this.radioButtonAxesXY.UseVisualStyleBackColor = true;
      // 
      // radioButtonRotationXY
      // 
      this.radioButtonRotationXY.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.radioButtonRotationXY.AutoSize = true;
      this.radioButtonRotationXY.Location = new System.Drawing.Point(176, 24);
      this.radioButtonRotationXY.Name = "radioButtonRotationXY";
      this.radioButtonRotationXY.Size = new System.Drawing.Size(109, 17);
      this.radioButtonRotationXY.TabIndex = 10;
      this.radioButtonRotationXY.Text = "Use Rotation X/Y";
      this.toolTips.SetToolTip(this.radioButtonRotationXY, "Use rotation X and Y to move the mouse");
      this.radioButtonRotationXY.UseVisualStyleBackColor = true;
      // 
      // groupBoxMouseButtons
      // 
      this.groupBoxMouseButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMouseButtons.Controls.Add(this.comboBoxRight);
      this.groupBoxMouseButtons.Controls.Add(this.comboBoxMiddle);
      this.groupBoxMouseButtons.Controls.Add(this.comboBoxLeft);
      this.groupBoxMouseButtons.Controls.Add(this.label2);
      this.groupBoxMouseButtons.Controls.Add(this.label1);
      this.groupBoxMouseButtons.Controls.Add(this.labelLeft);
      this.groupBoxMouseButtons.Enabled = false;
      this.groupBoxMouseButtons.Location = new System.Drawing.Point(8, 184);
      this.groupBoxMouseButtons.Name = "groupBoxMouseButtons";
      this.groupBoxMouseButtons.Size = new System.Drawing.Size(296, 72);
      this.groupBoxMouseButtons.TabIndex = 14;
      this.groupBoxMouseButtons.TabStop = false;
      this.groupBoxMouseButtons.Text = "Mouse buttons";
      this.groupBoxMouseButtons.Enter += new System.EventHandler(this.groupBoxMouseButtons_Enter);
      // 
      // labelLeft
      // 
      this.labelLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.labelLeft.AutoSize = true;
      this.labelLeft.Location = new System.Drawing.Point(8, 24);
      this.labelLeft.Name = "labelLeft";
      this.labelLeft.Size = new System.Drawing.Size(28, 13);
      this.labelLeft.TabIndex = 14;
      this.labelLeft.Text = "Left:";
      // 
      // label1
      // 
      this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(112, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(41, 13);
      this.label1.TabIndex = 16;
      this.label1.Text = "Middle:";
      // 
      // label2
      // 
      this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(216, 24);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(35, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Right:";
      // 
      // comboBoxLeft
      // 
      this.comboBoxLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.comboBoxLeft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxLeft.FormattingEnabled = true;
      this.comboBoxLeft.Location = new System.Drawing.Point(8, 40);
      this.comboBoxLeft.Name = "comboBoxLeft";
      this.comboBoxLeft.Size = new System.Drawing.Size(72, 21);
      this.comboBoxLeft.TabIndex = 19;
      // 
      // comboBoxMiddle
      // 
      this.comboBoxMiddle.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.comboBoxMiddle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxMiddle.FormattingEnabled = true;
      this.comboBoxMiddle.Location = new System.Drawing.Point(112, 40);
      this.comboBoxMiddle.Name = "comboBoxMiddle";
      this.comboBoxMiddle.Size = new System.Drawing.Size(72, 21);
      this.comboBoxMiddle.TabIndex = 20;
      // 
      // comboBoxRight
      // 
      this.comboBoxRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.comboBoxRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxRight.FormattingEnabled = true;
      this.comboBoxRight.Location = new System.Drawing.Point(216, 40);
      this.comboBoxRight.Name = "comboBoxRight";
      this.comboBoxRight.Size = new System.Drawing.Size(72, 21);
      this.comboBoxRight.TabIndex = 21;
      // 
      // Configure
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(312, 297);
      this.Controls.Add(this.groupBoxMouseButtons);
      this.Controls.Add(this.groupBoxMouseMovement);
      this.Controls.Add(this.listViewDevices);
      this.Controls.Add(this.buttonConfigure);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(320, 324);
      this.Name = "Configure";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Direct Input Configuration";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Configure_FormClosed);
      this.groupBoxMouseMovement.ResumeLayout(false);
      this.groupBoxMouseMovement.PerformLayout();
      this.groupBoxMouseButtons.ResumeLayout(false);
      this.groupBoxMouseButtons.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Button buttonConfigure;
    private System.Windows.Forms.ListView listViewDevices;
    private System.Windows.Forms.ColumnHeader columnHeaderName;
    private System.Windows.Forms.ColumnHeader columnHeaderDevice;
    private System.Windows.Forms.RadioButton radioButtonNone;
    private System.Windows.Forms.GroupBox groupBoxMouseMovement;
    private System.Windows.Forms.RadioButton radioButtonAxesXY;
    private System.Windows.Forms.RadioButton radioButtonRotationXY;
    private System.Windows.Forms.GroupBox groupBoxMouseButtons;
    private System.Windows.Forms.ComboBox comboBoxRight;
    private System.Windows.Forms.ComboBox comboBoxMiddle;
    private System.Windows.Forms.ComboBox comboBoxLeft;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelLeft;
  }
}