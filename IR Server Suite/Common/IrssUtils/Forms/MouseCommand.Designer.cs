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
      this.tabControlMouse = new System.Windows.Forms.TabControl();
      this.tabPageMovement = new System.Windows.Forms.TabPage();
      this.tabPageButtons = new System.Windows.Forms.TabPage();
      this.tabPageScrollWheel = new System.Windows.Forms.TabPage();
      this.tabPagePosition = new System.Windows.Forms.TabPage();
      this.groupBoxPosition = new System.Windows.Forms.GroupBox();
      this.groupBoxClick = new System.Windows.Forms.GroupBox();
      this.groupBoxDoubleClick = new System.Windows.Forms.GroupBox();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
      this.tabControlMouse.SuspendLayout();
      this.tabPageMovement.SuspendLayout();
      this.tabPageButtons.SuspendLayout();
      this.tabPageScrollWheel.SuspendLayout();
      this.tabPagePosition.SuspendLayout();
      this.groupBoxPosition.SuspendLayout();
      this.groupBoxClick.SuspendLayout();
      this.groupBoxDoubleClick.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(168, 208);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(104, 208);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // labelMouseMove
      // 
      this.labelMouseMove.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelMouseMove.Location = new System.Drawing.Point(40, 128);
      this.labelMouseMove.Name = "labelMouseMove";
      this.labelMouseMove.Size = new System.Drawing.Size(72, 20);
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
      this.numericUpDownMouseMove.Location = new System.Drawing.Point(112, 128);
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
      this.numericUpDownMouseMove.TabIndex = 5;
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
      this.checkBoxMouseMoveToPos.Location = new System.Drawing.Point(56, 120);
      this.checkBoxMouseMoveToPos.Name = "checkBoxMouseMoveToPos";
      this.checkBoxMouseMoveToPos.Size = new System.Drawing.Size(96, 24);
      this.checkBoxMouseMoveToPos.TabIndex = 0;
      this.checkBoxMouseMoveToPos.Text = "Move to X, Y";
      this.checkBoxMouseMoveToPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
      this.numericUpDownX.Location = new System.Drawing.Point(32, 24);
      this.numericUpDownX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownX.Name = "numericUpDownX";
      this.numericUpDownX.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownX.TabIndex = 2;
      this.numericUpDownX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownX, "Distance from the left edge of the screen space");
      // 
      // labelX
      // 
      this.labelX.Location = new System.Drawing.Point(8, 24);
      this.labelX.Name = "labelX";
      this.labelX.Size = new System.Drawing.Size(24, 20);
      this.labelX.TabIndex = 1;
      this.labelX.Text = "X: ";
      this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(8, 56);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(24, 20);
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
      this.numericUpDownY.Location = new System.Drawing.Point(32, 56);
      this.numericUpDownY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownY.Name = "numericUpDownY";
      this.numericUpDownY.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownY.TabIndex = 4;
      this.numericUpDownY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownY, "Distance from the top of the screen space");
      // 
      // checkBoxMouseMoveLeft
      // 
      this.checkBoxMouseMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseMoveLeft.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveLeft.Image = global::IrssUtils.Properties.Resources.MoveLeft;
      this.checkBoxMouseMoveLeft.Location = new System.Drawing.Point(56, 48);
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
      this.checkBoxMouseMoveUp.Location = new System.Drawing.Point(88, 16);
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
      this.checkBoxMouseMoveDown.Location = new System.Drawing.Point(88, 80);
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
      this.checkBoxMouseMoveRight.Location = new System.Drawing.Point(120, 48);
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
      this.checkBoxMouseClickLeft.Location = new System.Drawing.Point(16, 24);
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
      this.checkBoxMouseDoubleRight.Location = new System.Drawing.Point(96, 24);
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
      this.checkBoxMouseDoubleLeft.Location = new System.Drawing.Point(16, 24);
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
      this.checkBoxMouseClickRight.Location = new System.Drawing.Point(96, 24);
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
      this.checkBoxMouseClickMiddle.Location = new System.Drawing.Point(56, 24);
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
      this.checkBoxMouseDoubleMiddle.Location = new System.Drawing.Point(56, 24);
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
      this.checkBoxMouseScrollDown.Location = new System.Drawing.Point(88, 96);
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
      this.checkBoxMouseScrollUp.Location = new System.Drawing.Point(88, 40);
      this.checkBoxMouseScrollUp.Name = "checkBoxMouseScrollUp";
      this.checkBoxMouseScrollUp.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseScrollUp.TabIndex = 0;
      this.toolTips.SetToolTip(this.checkBoxMouseScrollUp, "Simulate a mouse scroll whell up command");
      this.checkBoxMouseScrollUp.UseVisualStyleBackColor = true;
      this.checkBoxMouseScrollUp.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // tabControlMouse
      // 
      this.tabControlMouse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControlMouse.Controls.Add(this.tabPageMovement);
      this.tabControlMouse.Controls.Add(this.tabPageButtons);
      this.tabControlMouse.Controls.Add(this.tabPageScrollWheel);
      this.tabControlMouse.Controls.Add(this.tabPagePosition);
      this.tabControlMouse.Location = new System.Drawing.Point(8, 8);
      this.tabControlMouse.Name = "tabControlMouse";
      this.tabControlMouse.SelectedIndex = 0;
      this.tabControlMouse.Size = new System.Drawing.Size(216, 192);
      this.tabControlMouse.TabIndex = 0;
      // 
      // tabPageMovement
      // 
      this.tabPageMovement.Controls.Add(this.checkBoxMouseMoveLeft);
      this.tabPageMovement.Controls.Add(this.checkBoxMouseMoveUp);
      this.tabPageMovement.Controls.Add(this.checkBoxMouseMoveDown);
      this.tabPageMovement.Controls.Add(this.numericUpDownMouseMove);
      this.tabPageMovement.Controls.Add(this.checkBoxMouseMoveRight);
      this.tabPageMovement.Controls.Add(this.labelMouseMove);
      this.tabPageMovement.Location = new System.Drawing.Point(4, 22);
      this.tabPageMovement.Name = "tabPageMovement";
      this.tabPageMovement.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMovement.Size = new System.Drawing.Size(208, 166);
      this.tabPageMovement.TabIndex = 0;
      this.tabPageMovement.Text = "Movement";
      this.tabPageMovement.UseVisualStyleBackColor = true;
      // 
      // tabPageButtons
      // 
      this.tabPageButtons.Controls.Add(this.groupBoxDoubleClick);
      this.tabPageButtons.Controls.Add(this.groupBoxClick);
      this.tabPageButtons.Location = new System.Drawing.Point(4, 22);
      this.tabPageButtons.Name = "tabPageButtons";
      this.tabPageButtons.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageButtons.Size = new System.Drawing.Size(208, 166);
      this.tabPageButtons.TabIndex = 1;
      this.tabPageButtons.Text = "Buttons";
      this.tabPageButtons.UseVisualStyleBackColor = true;
      // 
      // tabPageScrollWheel
      // 
      this.tabPageScrollWheel.Controls.Add(this.checkBoxMouseScrollDown);
      this.tabPageScrollWheel.Controls.Add(this.checkBoxMouseScrollUp);
      this.tabPageScrollWheel.Location = new System.Drawing.Point(4, 22);
      this.tabPageScrollWheel.Name = "tabPageScrollWheel";
      this.tabPageScrollWheel.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageScrollWheel.Size = new System.Drawing.Size(208, 166);
      this.tabPageScrollWheel.TabIndex = 2;
      this.tabPageScrollWheel.Text = "Wheel";
      this.tabPageScrollWheel.UseVisualStyleBackColor = true;
      // 
      // tabPagePosition
      // 
      this.tabPagePosition.Controls.Add(this.groupBoxPosition);
      this.tabPagePosition.Controls.Add(this.checkBoxMouseMoveToPos);
      this.tabPagePosition.Location = new System.Drawing.Point(4, 22);
      this.tabPagePosition.Name = "tabPagePosition";
      this.tabPagePosition.Padding = new System.Windows.Forms.Padding(3);
      this.tabPagePosition.Size = new System.Drawing.Size(208, 166);
      this.tabPagePosition.TabIndex = 3;
      this.tabPagePosition.Text = "Position";
      this.tabPagePosition.UseVisualStyleBackColor = true;
      // 
      // groupBoxPosition
      // 
      this.groupBoxPosition.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxPosition.Controls.Add(this.numericUpDownX);
      this.groupBoxPosition.Controls.Add(this.label1);
      this.groupBoxPosition.Controls.Add(this.labelX);
      this.groupBoxPosition.Controls.Add(this.numericUpDownY);
      this.groupBoxPosition.Location = new System.Drawing.Point(56, 24);
      this.groupBoxPosition.Name = "groupBoxPosition";
      this.groupBoxPosition.Size = new System.Drawing.Size(96, 88);
      this.groupBoxPosition.TabIndex = 5;
      this.groupBoxPosition.TabStop = false;
      this.groupBoxPosition.Text = "Position";
      // 
      // groupBoxClick
      // 
      this.groupBoxClick.Controls.Add(this.checkBoxMouseClickLeft);
      this.groupBoxClick.Controls.Add(this.checkBoxMouseClickMiddle);
      this.groupBoxClick.Controls.Add(this.checkBoxMouseClickRight);
      this.groupBoxClick.Location = new System.Drawing.Point(32, 8);
      this.groupBoxClick.Name = "groupBoxClick";
      this.groupBoxClick.Size = new System.Drawing.Size(144, 72);
      this.groupBoxClick.TabIndex = 0;
      this.groupBoxClick.TabStop = false;
      this.groupBoxClick.Text = "Click";
      // 
      // groupBoxDoubleClick
      // 
      this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleLeft);
      this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleMiddle);
      this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleRight);
      this.groupBoxDoubleClick.Location = new System.Drawing.Point(32, 88);
      this.groupBoxDoubleClick.Name = "groupBoxDoubleClick";
      this.groupBoxDoubleClick.Size = new System.Drawing.Size(144, 72);
      this.groupBoxDoubleClick.TabIndex = 1;
      this.groupBoxDoubleClick.TabStop = false;
      this.groupBoxDoubleClick.Text = "Double Click";
      // 
      // MouseCommand
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(232, 241);
      this.Controls.Add(this.tabControlMouse);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(240, 268);
      this.Name = "MouseCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Mouse Command";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
      this.tabControlMouse.ResumeLayout(false);
      this.tabPageMovement.ResumeLayout(false);
      this.tabPageButtons.ResumeLayout(false);
      this.tabPageScrollWheel.ResumeLayout(false);
      this.tabPagePosition.ResumeLayout(false);
      this.groupBoxPosition.ResumeLayout(false);
      this.groupBoxClick.ResumeLayout(false);
      this.groupBoxDoubleClick.ResumeLayout(false);
      this.ResumeLayout(false);

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
    private System.Windows.Forms.TabControl tabControlMouse;
    private System.Windows.Forms.TabPage tabPageMovement;
    private System.Windows.Forms.TabPage tabPageButtons;
    private System.Windows.Forms.TabPage tabPageScrollWheel;
    private System.Windows.Forms.TabPage tabPagePosition;
    private System.Windows.Forms.GroupBox groupBoxPosition;
    private System.Windows.Forms.GroupBox groupBoxDoubleClick;
    private System.Windows.Forms.GroupBox groupBoxClick;
  }

}
