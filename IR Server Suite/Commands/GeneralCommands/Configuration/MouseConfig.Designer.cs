namespace IrssCommands.General
{
  partial class MouseConfig
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
      this.checkBoxMouseMoveLeft = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseMoveUp = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseMoveDown = new System.Windows.Forms.CheckBox();
      this.numericUpDownMouseMove = new System.Windows.Forms.NumericUpDown();
      this.checkBoxMouseMoveRight = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseDoubleLeft = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseDoubleMiddle = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseDoubleRight = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseClickLeft = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseClickMiddle = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseClickRight = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseScrollDown = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseScrollUp = new System.Windows.Forms.CheckBox();
      this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
      this.checkBoxMouseMoveToPos = new System.Windows.Forms.CheckBox();
      this.groupBoxMovement = new System.Windows.Forms.GroupBox();
      this.labelMouseMove = new System.Windows.Forms.Label();
      this.groupBoxWheel = new System.Windows.Forms.GroupBox();
      this.groupBoxDoubleClick = new System.Windows.Forms.GroupBox();
      this.groupBoxClick = new System.Windows.Forms.GroupBox();
      this.groupBoxPosition = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.labelCurrentY = new System.Windows.Forms.Label();
      this.labelCurrentX = new System.Windows.Forms.Label();
      this.labelY = new System.Windows.Forms.Label();
      this.labelX = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
      this.groupBoxMovement.SuspendLayout();
      this.groupBoxWheel.SuspendLayout();
      this.groupBoxDoubleClick.SuspendLayout();
      this.groupBoxClick.SuspendLayout();
      this.groupBoxPosition.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBoxMouseMoveLeft
      // 
      this.checkBoxMouseMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseMoveLeft.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveLeft.Location = new System.Drawing.Point(24, 50);
      this.checkBoxMouseMoveLeft.Name = "checkBoxMouseMoveLeft";
      this.checkBoxMouseMoveLeft.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveLeft.TabIndex = 7;
      this.toolTip.SetToolTip(this.checkBoxMouseMoveLeft, "Move the mouse left");
      this.checkBoxMouseMoveLeft.UseVisualStyleBackColor = true;
      this.checkBoxMouseMoveLeft.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseMoveUp
      // 
      this.checkBoxMouseMoveUp.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseMoveUp.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveUp.Location = new System.Drawing.Point(56, 18);
      this.checkBoxMouseMoveUp.Name = "checkBoxMouseMoveUp";
      this.checkBoxMouseMoveUp.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveUp.TabIndex = 6;
      this.toolTip.SetToolTip(this.checkBoxMouseMoveUp, "Move the mouse up the screen");
      this.checkBoxMouseMoveUp.UseVisualStyleBackColor = true;
      this.checkBoxMouseMoveUp.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseMoveDown
      // 
      this.checkBoxMouseMoveDown.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseMoveDown.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveDown.Location = new System.Drawing.Point(56, 82);
      this.checkBoxMouseMoveDown.Name = "checkBoxMouseMoveDown";
      this.checkBoxMouseMoveDown.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveDown.TabIndex = 9;
      this.toolTip.SetToolTip(this.checkBoxMouseMoveDown, "Move the mouse down the screen");
      this.checkBoxMouseMoveDown.UseVisualStyleBackColor = true;
      this.checkBoxMouseMoveDown.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // numericUpDownMouseMove
      // 
      this.numericUpDownMouseMove.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownMouseMove.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownMouseMove.Location = new System.Drawing.Point(80, 121);
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
      this.numericUpDownMouseMove.TabIndex = 11;
      this.numericUpDownMouseMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTip.SetToolTip(this.numericUpDownMouseMove, "The distance to move the mouse pointer");
      this.numericUpDownMouseMove.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
      // 
      // checkBoxMouseMoveRight
      // 
      this.checkBoxMouseMoveRight.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseMoveRight.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveRight.Location = new System.Drawing.Point(88, 50);
      this.checkBoxMouseMoveRight.Name = "checkBoxMouseMoveRight";
      this.checkBoxMouseMoveRight.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveRight.TabIndex = 8;
      this.toolTip.SetToolTip(this.checkBoxMouseMoveRight, "Move the mouse right");
      this.checkBoxMouseMoveRight.UseVisualStyleBackColor = true;
      this.checkBoxMouseMoveRight.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseDoubleLeft
      // 
      this.checkBoxMouseDoubleLeft.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseDoubleLeft.Location = new System.Drawing.Point(16, 24);
      this.checkBoxMouseDoubleLeft.Name = "checkBoxMouseDoubleLeft";
      this.checkBoxMouseDoubleLeft.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseDoubleLeft.TabIndex = 0;
      this.toolTip.SetToolTip(this.checkBoxMouseDoubleLeft, "Double-Click the left mouse button");
      this.checkBoxMouseDoubleLeft.UseVisualStyleBackColor = true;
      this.checkBoxMouseDoubleLeft.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseDoubleMiddle
      // 
      this.checkBoxMouseDoubleMiddle.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseDoubleMiddle.Location = new System.Drawing.Point(56, 24);
      this.checkBoxMouseDoubleMiddle.Name = "checkBoxMouseDoubleMiddle";
      this.checkBoxMouseDoubleMiddle.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseDoubleMiddle.TabIndex = 1;
      this.toolTip.SetToolTip(this.checkBoxMouseDoubleMiddle, "Double-Click the middle mouse button");
      this.checkBoxMouseDoubleMiddle.UseVisualStyleBackColor = true;
      this.checkBoxMouseDoubleMiddle.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseDoubleRight
      // 
      this.checkBoxMouseDoubleRight.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseDoubleRight.Location = new System.Drawing.Point(96, 24);
      this.checkBoxMouseDoubleRight.Name = "checkBoxMouseDoubleRight";
      this.checkBoxMouseDoubleRight.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseDoubleRight.TabIndex = 2;
      this.toolTip.SetToolTip(this.checkBoxMouseDoubleRight, "Double-Click the right mouse button");
      this.checkBoxMouseDoubleRight.UseVisualStyleBackColor = true;
      this.checkBoxMouseDoubleRight.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseClickLeft
      // 
      this.checkBoxMouseClickLeft.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseClickLeft.Location = new System.Drawing.Point(16, 24);
      this.checkBoxMouseClickLeft.Name = "checkBoxMouseClickLeft";
      this.checkBoxMouseClickLeft.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseClickLeft.TabIndex = 0;
      this.toolTip.SetToolTip(this.checkBoxMouseClickLeft, "Click the left mouse button");
      this.checkBoxMouseClickLeft.UseVisualStyleBackColor = true;
      this.checkBoxMouseClickLeft.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseClickMiddle
      // 
      this.checkBoxMouseClickMiddle.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseClickMiddle.Location = new System.Drawing.Point(56, 24);
      this.checkBoxMouseClickMiddle.Name = "checkBoxMouseClickMiddle";
      this.checkBoxMouseClickMiddle.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseClickMiddle.TabIndex = 1;
      this.toolTip.SetToolTip(this.checkBoxMouseClickMiddle, "Click the middle mouse button");
      this.checkBoxMouseClickMiddle.UseVisualStyleBackColor = true;
      this.checkBoxMouseClickMiddle.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseClickRight
      // 
      this.checkBoxMouseClickRight.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseClickRight.Location = new System.Drawing.Point(96, 24);
      this.checkBoxMouseClickRight.Name = "checkBoxMouseClickRight";
      this.checkBoxMouseClickRight.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseClickRight.TabIndex = 2;
      this.toolTip.SetToolTip(this.checkBoxMouseClickRight, "Click the right mouse button");
      this.checkBoxMouseClickRight.UseVisualStyleBackColor = true;
      this.checkBoxMouseClickRight.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseScrollDown
      // 
      this.checkBoxMouseScrollDown.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseScrollDown.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseScrollDown.Location = new System.Drawing.Point(24, 87);
      this.checkBoxMouseScrollDown.Name = "checkBoxMouseScrollDown";
      this.checkBoxMouseScrollDown.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseScrollDown.TabIndex = 3;
      this.toolTip.SetToolTip(this.checkBoxMouseScrollDown, "Simulate a mouse scroll wheel down command");
      this.checkBoxMouseScrollDown.UseVisualStyleBackColor = true;
      this.checkBoxMouseScrollDown.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // checkBoxMouseScrollUp
      // 
      this.checkBoxMouseScrollUp.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseScrollUp.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseScrollUp.Location = new System.Drawing.Point(24, 31);
      this.checkBoxMouseScrollUp.Name = "checkBoxMouseScrollUp";
      this.checkBoxMouseScrollUp.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseScrollUp.TabIndex = 2;
      this.toolTip.SetToolTip(this.checkBoxMouseScrollUp, "Simulate a mouse scroll whell up command");
      this.checkBoxMouseScrollUp.UseVisualStyleBackColor = true;
      this.checkBoxMouseScrollUp.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      // 
      // numericUpDownX
      // 
      this.numericUpDownX.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownX.Location = new System.Drawing.Point(45, 20);
      this.numericUpDownX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownX.Name = "numericUpDownX";
      this.numericUpDownX.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownX.TabIndex = 13;
      this.numericUpDownX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTip.SetToolTip(this.numericUpDownX, "Distance from the left edge of the screen space");
      this.numericUpDownX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetXY);
      // 
      // numericUpDownY
      // 
      this.numericUpDownY.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.numericUpDownY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownY.Location = new System.Drawing.Point(45, 62);
      this.numericUpDownY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownY.Name = "numericUpDownY";
      this.numericUpDownY.Size = new System.Drawing.Size(56, 20);
      this.numericUpDownY.TabIndex = 15;
      this.numericUpDownY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTip.SetToolTip(this.numericUpDownY, "Distance from the top of the screen space");
      this.numericUpDownY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetXY);
      // 
      // checkBoxMouseMoveToPos
      // 
      this.checkBoxMouseMoveToPos.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.checkBoxMouseMoveToPos.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveToPos.Location = new System.Drawing.Point(13, 119);
      this.checkBoxMouseMoveToPos.Name = "checkBoxMouseMoveToPos";
      this.checkBoxMouseMoveToPos.Size = new System.Drawing.Size(96, 24);
      this.checkBoxMouseMoveToPos.TabIndex = 11;
      this.checkBoxMouseMoveToPos.Text = "Move to X, Y";
      this.checkBoxMouseMoveToPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTip.SetToolTip(this.checkBoxMouseMoveToPos, "Move the mouse pointer to specific screen coordinates");
      this.checkBoxMouseMoveToPos.UseVisualStyleBackColor = true;
      this.checkBoxMouseMoveToPos.CheckedChanged += new System.EventHandler(this.checkBoxMouse_CheckedChanged);
      this.checkBoxMouseMoveToPos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetXY);
      // 
      // groupBoxMovement
      // 
      this.groupBoxMovement.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxMovement.Controls.Add(this.checkBoxMouseMoveLeft);
      this.groupBoxMovement.Controls.Add(this.checkBoxMouseMoveUp);
      this.groupBoxMovement.Controls.Add(this.checkBoxMouseMoveDown);
      this.groupBoxMovement.Controls.Add(this.numericUpDownMouseMove);
      this.groupBoxMovement.Controls.Add(this.checkBoxMouseMoveRight);
      this.groupBoxMovement.Controls.Add(this.labelMouseMove);
      this.groupBoxMovement.Location = new System.Drawing.Point(3, 3);
      this.groupBoxMovement.Name = "groupBoxMovement";
      this.groupBoxMovement.Size = new System.Drawing.Size(144, 150);
      this.groupBoxMovement.TabIndex = 0;
      this.groupBoxMovement.TabStop = false;
      this.groupBoxMovement.Text = "Movement";
      // 
      // labelMouseMove
      // 
      this.labelMouseMove.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelMouseMove.Location = new System.Drawing.Point(8, 121);
      this.labelMouseMove.Name = "labelMouseMove";
      this.labelMouseMove.Size = new System.Drawing.Size(72, 20);
      this.labelMouseMove.TabIndex = 10;
      this.labelMouseMove.Text = "Distance:";
      this.labelMouseMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // groupBoxWheel
      // 
      this.groupBoxWheel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxWheel.Controls.Add(this.checkBoxMouseScrollDown);
      this.groupBoxWheel.Controls.Add(this.checkBoxMouseScrollUp);
      this.groupBoxWheel.Location = new System.Drawing.Point(303, 3);
      this.groupBoxWheel.Name = "groupBoxWheel";
      this.groupBoxWheel.Size = new System.Drawing.Size(81, 150);
      this.groupBoxWheel.TabIndex = 1;
      this.groupBoxWheel.TabStop = false;
      this.groupBoxWheel.Text = "Wheel";
      // 
      // groupBoxDoubleClick
      // 
      this.groupBoxDoubleClick.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleLeft);
      this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleMiddle);
      this.groupBoxDoubleClick.Controls.Add(this.checkBoxMouseDoubleRight);
      this.groupBoxDoubleClick.Location = new System.Drawing.Point(153, 81);
      this.groupBoxDoubleClick.Name = "groupBoxDoubleClick";
      this.groupBoxDoubleClick.Size = new System.Drawing.Size(144, 72);
      this.groupBoxDoubleClick.TabIndex = 3;
      this.groupBoxDoubleClick.TabStop = false;
      this.groupBoxDoubleClick.Text = "Double Click";
      // 
      // groupBoxClick
      // 
      this.groupBoxClick.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxClick.Controls.Add(this.checkBoxMouseClickLeft);
      this.groupBoxClick.Controls.Add(this.checkBoxMouseClickMiddle);
      this.groupBoxClick.Controls.Add(this.checkBoxMouseClickRight);
      this.groupBoxClick.Location = new System.Drawing.Point(153, 3);
      this.groupBoxClick.Name = "groupBoxClick";
      this.groupBoxClick.Size = new System.Drawing.Size(144, 72);
      this.groupBoxClick.TabIndex = 2;
      this.groupBoxClick.TabStop = false;
      this.groupBoxClick.Text = "Click";
      // 
      // groupBoxPosition
      // 
      this.groupBoxPosition.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxPosition.Controls.Add(this.label1);
      this.groupBoxPosition.Controls.Add(this.labelCurrentY);
      this.groupBoxPosition.Controls.Add(this.labelCurrentX);
      this.groupBoxPosition.Controls.Add(this.numericUpDownX);
      this.groupBoxPosition.Controls.Add(this.labelY);
      this.groupBoxPosition.Controls.Add(this.labelX);
      this.groupBoxPosition.Controls.Add(this.numericUpDownY);
      this.groupBoxPosition.Controls.Add(this.checkBoxMouseMoveToPos);
      this.groupBoxPosition.Location = new System.Drawing.Point(390, 3);
      this.groupBoxPosition.Name = "groupBoxPosition";
      this.groupBoxPosition.Size = new System.Drawing.Size(123, 150);
      this.groupBoxPosition.TabIndex = 4;
      this.groupBoxPosition.TabStop = false;
      this.groupBoxPosition.Text = "Position";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
      this.label1.Location = new System.Drawing.Point(6, 101);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(113, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Press F8 to set current";
      // 
      // labelCurrentY
      // 
      this.labelCurrentY.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelCurrentY.AutoSize = true;
      this.labelCurrentY.Enabled = false;
      this.labelCurrentY.Location = new System.Drawing.Point(50, 84);
      this.labelCurrentY.Name = "labelCurrentY";
      this.labelCurrentY.Size = new System.Drawing.Size(31, 13);
      this.labelCurrentY.TabIndex = 16;
      this.labelCurrentY.Text = "1234";
      this.labelCurrentY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelCurrentX
      // 
      this.labelCurrentX.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelCurrentX.AutoSize = true;
      this.labelCurrentX.Enabled = false;
      this.labelCurrentX.Location = new System.Drawing.Point(50, 42);
      this.labelCurrentX.Name = "labelCurrentX";
      this.labelCurrentX.Size = new System.Drawing.Size(31, 13);
      this.labelCurrentX.TabIndex = 16;
      this.labelCurrentX.Text = "1234";
      this.labelCurrentX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelY
      // 
      this.labelY.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelY.AutoSize = true;
      this.labelY.Location = new System.Drawing.Point(19, 63);
      this.labelY.Name = "labelY";
      this.labelY.Size = new System.Drawing.Size(20, 13);
      this.labelY.TabIndex = 14;
      this.labelY.Text = "Y: ";
      this.labelY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelX
      // 
      this.labelX.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelX.AutoSize = true;
      this.labelX.Location = new System.Drawing.Point(19, 22);
      this.labelX.Name = "labelX";
      this.labelX.Size = new System.Drawing.Size(20, 13);
      this.labelX.TabIndex = 12;
      this.labelX.Text = "X: ";
      this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // MouseConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxPosition);
      this.Controls.Add(this.groupBoxDoubleClick);
      this.Controls.Add(this.groupBoxClick);
      this.Controls.Add(this.groupBoxWheel);
      this.Controls.Add(this.groupBoxMovement);
      this.MinimumSize = new System.Drawing.Size(519, 160);
      this.Name = "MouseConfig";
      this.Size = new System.Drawing.Size(519, 160);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetXY);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
      this.groupBoxMovement.ResumeLayout(false);
      this.groupBoxWheel.ResumeLayout(false);
      this.groupBoxDoubleClick.ResumeLayout(false);
      this.groupBoxClick.ResumeLayout(false);
      this.groupBoxPosition.ResumeLayout(false);
      this.groupBoxPosition.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxMovement;
    private System.Windows.Forms.GroupBox groupBoxWheel;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveLeft;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveUp;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveDown;
    private System.Windows.Forms.NumericUpDown numericUpDownMouseMove;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveRight;
    private System.Windows.Forms.Label labelMouseMove;
    private System.Windows.Forms.GroupBox groupBoxDoubleClick;
    private System.Windows.Forms.CheckBox checkBoxMouseDoubleLeft;
    private System.Windows.Forms.CheckBox checkBoxMouseDoubleMiddle;
    private System.Windows.Forms.CheckBox checkBoxMouseDoubleRight;
    private System.Windows.Forms.GroupBox groupBoxClick;
    private System.Windows.Forms.CheckBox checkBoxMouseClickLeft;
    private System.Windows.Forms.CheckBox checkBoxMouseClickMiddle;
    private System.Windows.Forms.CheckBox checkBoxMouseClickRight;
    private System.Windows.Forms.CheckBox checkBoxMouseScrollDown;
    private System.Windows.Forms.CheckBox checkBoxMouseScrollUp;
    private System.Windows.Forms.GroupBox groupBoxPosition;
    private System.Windows.Forms.NumericUpDown numericUpDownX;
    private System.Windows.Forms.Label labelY;
    private System.Windows.Forms.Label labelX;
    private System.Windows.Forms.NumericUpDown numericUpDownY;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveToPos;
    private System.Windows.Forms.Label labelCurrentX;
    private System.Windows.Forms.Label labelCurrentY;
    private System.Windows.Forms.Label label1;
  }
}
