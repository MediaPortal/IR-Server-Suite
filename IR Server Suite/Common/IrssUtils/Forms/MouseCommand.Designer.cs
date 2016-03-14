namespace IrssUtils.Forms
{

  partial class MouseCommand
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelMouseMove = new System.Windows.Forms.Label();
            this.numericUpDownMouseMove = new System.Windows.Forms.NumericUpDown();
            this.checkBoxMouseMoveToPos = new System.Windows.Forms.CheckBox();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.labelX = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxMouseMoveLeft = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseMoveUp = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseMoveDown = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseMoveRight = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseClickLeft = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseDoubleRight = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseDoubleLeft = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseClickRight = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseClickMiddle = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseDoubleMiddle = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseScrollDown = new System.Windows.Forms.CheckBox();
            this.checkBoxMouseScrollUp = new System.Windows.Forms.CheckBox();
            this.groupBoxDoubleClick = new System.Windows.Forms.GroupBox();
            this.groupBoxPosition = new System.Windows.Forms.GroupBox();
            this.groupBoxMouseScroll = new System.Windows.Forms.GroupBox();
            this.groupBoxMouseClick = new System.Windows.Forms.GroupBox();
            this.groupBoxMouseMove = new System.Windows.Forms.GroupBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.labelMousePointer = new System.Windows.Forms.Label();
            this.labelMousePos = new System.Windows.Forms.Label();
            this.labelMouseKey = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            this.groupBoxDoubleClick.SuspendLayout();
            this.groupBoxPosition.SuspendLayout();
            this.groupBoxMouseScroll.SuspendLayout();
            this.groupBoxMouseClick.SuspendLayout();
            this.groupBoxMouseMove.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(371, 200);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 24);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(307, 200);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(56, 24);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelMouseMove
            // 
            this.labelMouseMove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelMouseMove.Location = new System.Drawing.Point(9, 134);
            this.labelMouseMove.Name = "labelMouseMove";
            this.labelMouseMove.Size = new System.Drawing.Size(58, 20);
            this.labelMouseMove.TabIndex = 4;
            this.labelMouseMove.Text = "Distance:";
            this.labelMouseMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownMouseMove
            // 
            this.numericUpDownMouseMove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownMouseMove.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownMouseMove.Location = new System.Drawing.Point(65, 134);
            this.numericUpDownMouseMove.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownMouseMove.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMouseMove.Name = "numericUpDownMouseMove";
            this.numericUpDownMouseMove.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownMouseMove.TabIndex = 4;
            this.numericUpDownMouseMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTips.SetToolTip(this.numericUpDownMouseMove, "The distance to move the mouse pointer");
            this.numericUpDownMouseMove.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // checkBoxMouseMoveToPos
            // 
            this.checkBoxMouseMoveToPos.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseMoveToPos.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseMoveToPos.Image = global::IrssUtils.Properties.Resources.MoveRight;
            this.checkBoxMouseMoveToPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxMouseMoveToPos.Location = new System.Drawing.Point(30, 19);
            this.checkBoxMouseMoveToPos.Name = "checkBoxMouseMoveToPos";
            this.checkBoxMouseMoveToPos.Size = new System.Drawing.Size(73, 24);
            this.checkBoxMouseMoveToPos.TabIndex = 0;
            this.checkBoxMouseMoveToPos.Text = "Move to";
            this.checkBoxMouseMoveToPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTips.SetToolTip(this.checkBoxMouseMoveToPos, "Move the mouse pointer to specific screen coordinates");
            this.checkBoxMouseMoveToPos.UseVisualStyleBackColor = true;
            this.checkBoxMouseMoveToPos.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownX.Location = new System.Drawing.Point(47, 46);
            this.numericUpDownX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownX.TabIndex = 1;
            this.numericUpDownX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTips.SetToolTip(this.numericUpDownX, "Distance from the left edge of the screen space");
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(30, 46);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(17, 20);
            this.labelX.TabIndex = 1;
            this.labelX.Text = "X: ";
            this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Y: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownY.Location = new System.Drawing.Point(47, 68);
            this.numericUpDownY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownY.TabIndex = 2;
            this.numericUpDownY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTips.SetToolTip(this.numericUpDownY, "Distance from the top of the screen space");
            // 
            // checkBoxMouseMoveLeft
            // 
            this.checkBoxMouseMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseMoveLeft.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseMoveLeft.Image = global::IrssUtils.Properties.Resources.MoveLeft;
            this.checkBoxMouseMoveLeft.Location = new System.Drawing.Point(25, 54);
            this.checkBoxMouseMoveLeft.Name = "checkBoxMouseMoveLeft";
            this.checkBoxMouseMoveLeft.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseMoveLeft.TabIndex = 1;
            this.toolTips.SetToolTip(this.checkBoxMouseMoveLeft, "Move the mouse left");
            this.checkBoxMouseMoveLeft.UseVisualStyleBackColor = true;
            this.checkBoxMouseMoveLeft.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseMoveUp
            // 
            this.checkBoxMouseMoveUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseMoveUp.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseMoveUp.Image = global::IrssUtils.Properties.Resources.MoveUp;
            this.checkBoxMouseMoveUp.Location = new System.Drawing.Point(57, 22);
            this.checkBoxMouseMoveUp.Name = "checkBoxMouseMoveUp";
            this.checkBoxMouseMoveUp.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseMoveUp.TabIndex = 0;
            this.toolTips.SetToolTip(this.checkBoxMouseMoveUp, "Move the mouse up the screen");
            this.checkBoxMouseMoveUp.UseVisualStyleBackColor = true;
            this.checkBoxMouseMoveUp.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseMoveDown
            // 
            this.checkBoxMouseMoveDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseMoveDown.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseMoveDown.Image = global::IrssUtils.Properties.Resources.MoveDown;
            this.checkBoxMouseMoveDown.Location = new System.Drawing.Point(57, 86);
            this.checkBoxMouseMoveDown.Name = "checkBoxMouseMoveDown";
            this.checkBoxMouseMoveDown.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseMoveDown.TabIndex = 3;
            this.toolTips.SetToolTip(this.checkBoxMouseMoveDown, "Move the mouse down the screen");
            this.checkBoxMouseMoveDown.UseVisualStyleBackColor = true;
            this.checkBoxMouseMoveDown.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseMoveRight
            // 
            this.checkBoxMouseMoveRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseMoveRight.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseMoveRight.Image = global::IrssUtils.Properties.Resources.MoveRight;
            this.checkBoxMouseMoveRight.Location = new System.Drawing.Point(89, 54);
            this.checkBoxMouseMoveRight.Name = "checkBoxMouseMoveRight";
            this.checkBoxMouseMoveRight.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseMoveRight.TabIndex = 2;
            this.toolTips.SetToolTip(this.checkBoxMouseMoveRight, "Move the mouse right");
            this.checkBoxMouseMoveRight.UseVisualStyleBackColor = true;
            this.checkBoxMouseMoveRight.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseClickLeft
            // 
            this.checkBoxMouseClickLeft.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseClickLeft.Image = global::IrssUtils.Properties.Resources.ClickLeft;
            this.checkBoxMouseClickLeft.Location = new System.Drawing.Point(9, 24);
            this.checkBoxMouseClickLeft.Name = "checkBoxMouseClickLeft";
            this.checkBoxMouseClickLeft.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseClickLeft.TabIndex = 0;
            this.toolTips.SetToolTip(this.checkBoxMouseClickLeft, "Click the left mouse button");
            this.checkBoxMouseClickLeft.UseVisualStyleBackColor = true;
            this.checkBoxMouseClickLeft.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseDoubleRight
            // 
            this.checkBoxMouseDoubleRight.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseDoubleRight.Image = global::IrssUtils.Properties.Resources.DoubleClickRight;
            this.checkBoxMouseDoubleRight.Location = new System.Drawing.Point(89, 34);
            this.checkBoxMouseDoubleRight.Name = "checkBoxMouseDoubleRight";
            this.checkBoxMouseDoubleRight.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseDoubleRight.TabIndex = 2;
            this.toolTips.SetToolTip(this.checkBoxMouseDoubleRight, "Double-Click the right mouse button");
            this.checkBoxMouseDoubleRight.UseVisualStyleBackColor = true;
            this.checkBoxMouseDoubleRight.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseDoubleLeft
            // 
            this.checkBoxMouseDoubleLeft.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseDoubleLeft.Image = global::IrssUtils.Properties.Resources.DoubleClickLeft;
            this.checkBoxMouseDoubleLeft.Location = new System.Drawing.Point(9, 34);
            this.checkBoxMouseDoubleLeft.Name = "checkBoxMouseDoubleLeft";
            this.checkBoxMouseDoubleLeft.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseDoubleLeft.TabIndex = 0;
            this.toolTips.SetToolTip(this.checkBoxMouseDoubleLeft, "Double-Click the left mouse button");
            this.checkBoxMouseDoubleLeft.UseVisualStyleBackColor = true;
            this.checkBoxMouseDoubleLeft.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseClickRight
            // 
            this.checkBoxMouseClickRight.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseClickRight.Image = global::IrssUtils.Properties.Resources.ClickRight;
            this.checkBoxMouseClickRight.Location = new System.Drawing.Point(89, 24);
            this.checkBoxMouseClickRight.Name = "checkBoxMouseClickRight";
            this.checkBoxMouseClickRight.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseClickRight.TabIndex = 2;
            this.toolTips.SetToolTip(this.checkBoxMouseClickRight, "Click the right mouse button");
            this.checkBoxMouseClickRight.UseVisualStyleBackColor = true;
            this.checkBoxMouseClickRight.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseClickMiddle
            // 
            this.checkBoxMouseClickMiddle.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseClickMiddle.Image = global::IrssUtils.Properties.Resources.ClickMiddle;
            this.checkBoxMouseClickMiddle.Location = new System.Drawing.Point(49, 24);
            this.checkBoxMouseClickMiddle.Name = "checkBoxMouseClickMiddle";
            this.checkBoxMouseClickMiddle.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseClickMiddle.TabIndex = 1;
            this.toolTips.SetToolTip(this.checkBoxMouseClickMiddle, "Click the middle mouse button");
            this.checkBoxMouseClickMiddle.UseVisualStyleBackColor = true;
            this.checkBoxMouseClickMiddle.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseDoubleMiddle
            // 
            this.checkBoxMouseDoubleMiddle.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseDoubleMiddle.Image = global::IrssUtils.Properties.Resources.DoubleClickMiddle;
            this.checkBoxMouseDoubleMiddle.Location = new System.Drawing.Point(49, 34);
            this.checkBoxMouseDoubleMiddle.Name = "checkBoxMouseDoubleMiddle";
            this.checkBoxMouseDoubleMiddle.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseDoubleMiddle.TabIndex = 1;
            this.toolTips.SetToolTip(this.checkBoxMouseDoubleMiddle, "Double-Click the middle mouse button");
            this.checkBoxMouseDoubleMiddle.UseVisualStyleBackColor = true;
            this.checkBoxMouseDoubleMiddle.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseScrollDown
            // 
            this.checkBoxMouseScrollDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseScrollDown.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseScrollDown.Image = global::IrssUtils.Properties.Resources.ScrollDown;
            this.checkBoxMouseScrollDown.Location = new System.Drawing.Point(71, 24);
            this.checkBoxMouseScrollDown.Name = "checkBoxMouseScrollDown";
            this.checkBoxMouseScrollDown.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseScrollDown.TabIndex = 1;
            this.toolTips.SetToolTip(this.checkBoxMouseScrollDown, "Simulate a mouse scroll wheel down command");
            this.checkBoxMouseScrollDown.UseVisualStyleBackColor = true;
            this.checkBoxMouseScrollDown.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // checkBoxMouseScrollUp
            // 
            this.checkBoxMouseScrollUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxMouseScrollUp.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMouseScrollUp.Image = global::IrssUtils.Properties.Resources.ScrollUp;
            this.checkBoxMouseScrollUp.Location = new System.Drawing.Point(30, 24);
            this.checkBoxMouseScrollUp.Name = "checkBoxMouseScrollUp";
            this.checkBoxMouseScrollUp.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMouseScrollUp.TabIndex = 0;
            this.toolTips.SetToolTip(this.checkBoxMouseScrollUp, "Simulate a mouse scroll whell up command");
            this.checkBoxMouseScrollUp.UseVisualStyleBackColor = true;
            this.checkBoxMouseScrollUp.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
            // 
            // groupBoxDoubleClick
            // 
            this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleLeft);
            this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleMiddle);
            this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleRight);
            this.groupBoxDoubleClick.Location = new System.Drawing.Point(162, 92);
            this.groupBoxDoubleClick.Name = "groupBoxDoubleClick";
            this.groupBoxDoubleClick.Size = new System.Drawing.Size(130, 101);
            this.groupBoxDoubleClick.TabIndex = 3;
            this.groupBoxDoubleClick.TabStop = false;
            this.groupBoxDoubleClick.Text = "Double Click";
            // 
            // groupBoxPosition
            // 
            this.groupBoxPosition.Controls.Add(this.checkBoxMouseMoveToPos);
            this.groupBoxPosition.Controls.Add(this.numericUpDownX);
            this.groupBoxPosition.Controls.Add(this.label1);
            this.groupBoxPosition.Controls.Add(this.labelX);
            this.groupBoxPosition.Controls.Add(this.numericUpDownY);
            this.groupBoxPosition.Location = new System.Drawing.Point(298, 92);
            this.groupBoxPosition.Name = "groupBoxPosition";
            this.groupBoxPosition.Size = new System.Drawing.Size(128, 101);
            this.groupBoxPosition.TabIndex = 4;
            this.groupBoxPosition.TabStop = false;
            this.groupBoxPosition.Text = "Position";
            // 
            // groupBoxMouseScroll
            // 
            this.groupBoxMouseScroll.Controls.Add(this.checkBoxMouseScrollDown);
            this.groupBoxMouseScroll.Controls.Add(this.checkBoxMouseScrollUp);
            this.groupBoxMouseScroll.Location = new System.Drawing.Point(298, 12);
            this.groupBoxMouseScroll.Name = "groupBoxMouseScroll";
            this.groupBoxMouseScroll.Size = new System.Drawing.Size(128, 74);
            this.groupBoxMouseScroll.TabIndex = 2;
            this.groupBoxMouseScroll.TabStop = false;
            this.groupBoxMouseScroll.Text = "Scroll";
            // 
            // groupBoxMouseClick
            // 
            this.groupBoxMouseClick.Controls.Add(this.checkBoxMouseClickLeft);
            this.groupBoxMouseClick.Controls.Add(this.checkBoxMouseClickRight);
            this.groupBoxMouseClick.Controls.Add(this.checkBoxMouseClickMiddle);
            this.groupBoxMouseClick.Location = new System.Drawing.Point(162, 12);
            this.groupBoxMouseClick.Name = "groupBoxMouseClick";
            this.groupBoxMouseClick.Size = new System.Drawing.Size(130, 74);
            this.groupBoxMouseClick.TabIndex = 1;
            this.groupBoxMouseClick.TabStop = false;
            this.groupBoxMouseClick.Text = "Click";
            // 
            // groupBoxMouseMove
            // 
            this.groupBoxMouseMove.Controls.Add(this.numericUpDownMouseMove);
            this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveLeft);
            this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveUp);
            this.groupBoxMouseMove.Controls.Add(this.labelMouseMove);
            this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveDown);
            this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveRight);
            this.groupBoxMouseMove.Location = new System.Drawing.Point(12, 12);
            this.groupBoxMouseMove.Name = "groupBoxMouseMove";
            this.groupBoxMouseMove.Size = new System.Drawing.Size(144, 181);
            this.groupBoxMouseMove.TabIndex = 0;
            this.groupBoxMouseMove.TabStop = false;
            this.groupBoxMouseMove.Text = "Move";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // labelMousePointer
            // 
            this.labelMousePointer.AutoSize = true;
            this.labelMousePointer.Location = new System.Drawing.Point(21, 206);
            this.labelMousePointer.Name = "labelMousePointer";
            this.labelMousePointer.Size = new System.Drawing.Size(82, 13);
            this.labelMousePointer.TabIndex = 7;
            this.labelMousePointer.Text = "Mouse Position:";
            // 
            // labelMousePos
            // 
            this.labelMousePos.Location = new System.Drawing.Point(109, 206);
            this.labelMousePos.Name = "labelMousePos";
            this.labelMousePos.Size = new System.Drawing.Size(94, 13);
            this.labelMousePos.TabIndex = 7;
            this.labelMousePos.Text = "-";
            // 
            // labelMouseKey
            // 
            this.labelMouseKey.AutoSize = true;
            this.labelMouseKey.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelMouseKey.Location = new System.Drawing.Point(209, 206);
            this.labelMouseKey.Name = "labelMouseKey";
            this.labelMouseKey.Size = new System.Drawing.Size(88, 13);
            this.labelMouseKey.TabIndex = 7;
            this.labelMouseKey.Text = "(press F8 to pick)";
            // 
            // MouseCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 233);
            this.Controls.Add(this.labelMousePos);
            this.Controls.Add(this.labelMouseKey);
            this.Controls.Add(this.labelMousePointer);
            this.Controls.Add(this.groupBoxPosition);
            this.Controls.Add(this.groupBoxDoubleClick);
            this.Controls.Add(this.groupBoxMouseScroll);
            this.Controls.Add(this.groupBoxMouseClick);
            this.Controls.Add(this.groupBoxMouseMove);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(451, 271);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(451, 271);
            this.Name = "MouseCommand";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mouse Command";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MouseCommand_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.MouseCommand_HelpRequested);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MouseCommand_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            this.groupBoxDoubleClick.ResumeLayout(false);
            this.groupBoxPosition.ResumeLayout(false);
            this.groupBoxMouseScroll.ResumeLayout(false);
            this.groupBoxMouseClick.ResumeLayout(false);
            this.groupBoxMouseMove.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.CheckBox checkBoxMouseScrollDown;
    private System.Windows.Forms.CheckBox checkBoxMouseScrollUp;
    private System.Windows.Forms.CheckBox checkBoxMouseClickRight;
    private System.Windows.Forms.CheckBox checkBoxMouseClickMiddle;
    private System.Windows.Forms.CheckBox checkBoxMouseClickLeft;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveLeft;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveDown;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveRight;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveUp;
    private System.Windows.Forms.Label labelMouseMove;
    private System.Windows.Forms.NumericUpDown numericUpDownMouseMove;
    private System.Windows.Forms.NumericUpDown numericUpDownX;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveToPos;
    private System.Windows.Forms.Label labelX;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown numericUpDownY;
    private System.Windows.Forms.CheckBox checkBoxMouseDoubleRight;
    private System.Windows.Forms.CheckBox checkBoxMouseDoubleMiddle;
    private System.Windows.Forms.CheckBox checkBoxMouseDoubleLeft;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.GroupBox groupBoxPosition;
    private System.Windows.Forms.GroupBox groupBoxDoubleClick;
    private System.Windows.Forms.GroupBox groupBoxMouseScroll;
    private System.Windows.Forms.GroupBox groupBoxMouseClick;
    private System.Windows.Forms.GroupBox groupBoxMouseMove;
    private System.Windows.Forms.Timer timer;
    private System.Windows.Forms.Label labelMousePointer;
    private System.Windows.Forms.Label labelMousePos;
    private System.Windows.Forms.Label labelMouseKey;
  }

}
