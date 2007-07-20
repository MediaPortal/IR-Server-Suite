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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxMouseScroll = new System.Windows.Forms.GroupBox();
      this.checkBoxMouseScrollDown = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseScrollUp = new System.Windows.Forms.CheckBox();
      this.groupBoxMouseClick = new System.Windows.Forms.GroupBox();
      this.checkBoxMouseClickRight = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseClickMiddle = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseClickLeft = new System.Windows.Forms.CheckBox();
      this.groupBoxMouseMove = new System.Windows.Forms.GroupBox();
      this.checkBoxMouseMoveLeft = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseMoveDown = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseMoveRight = new System.Windows.Forms.CheckBox();
      this.checkBoxMouseMoveUp = new System.Windows.Forms.CheckBox();
      this.labelMouseMove = new System.Windows.Forms.Label();
      this.numericUpDownMouseMove = new System.Windows.Forms.NumericUpDown();
      this.groupBoxMouseScroll.SuspendLayout();
      this.groupBoxMouseClick.SuspendLayout();
      this.groupBoxMouseMove.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(360, 192);
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
      this.buttonOK.Location = new System.Drawing.Point(288, 192);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // groupBoxMouseScroll
      // 
      this.groupBoxMouseScroll.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxMouseScroll.Controls.Add(this.checkBoxMouseScrollDown);
      this.groupBoxMouseScroll.Controls.Add(this.checkBoxMouseScrollUp);
      this.groupBoxMouseScroll.Location = new System.Drawing.Point(296, 8);
      this.groupBoxMouseScroll.Name = "groupBoxMouseScroll";
      this.groupBoxMouseScroll.Size = new System.Drawing.Size(128, 176);
      this.groupBoxMouseScroll.TabIndex = 6;
      this.groupBoxMouseScroll.TabStop = false;
      this.groupBoxMouseScroll.Text = "Scroll";
      // 
      // checkBoxMouseScrollDown
      // 
      this.checkBoxMouseScrollDown.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseScrollDown.Image = global::IrssUtils.Properties.Resources.ScrollDown;
      this.checkBoxMouseScrollDown.Location = new System.Drawing.Point(48, 96);
      this.checkBoxMouseScrollDown.Name = "checkBoxMouseScrollDown";
      this.checkBoxMouseScrollDown.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseScrollDown.TabIndex = 1;
      this.checkBoxMouseScrollDown.UseVisualStyleBackColor = true;
      // 
      // checkBoxMouseScrollUp
      // 
      this.checkBoxMouseScrollUp.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseScrollUp.Image = global::IrssUtils.Properties.Resources.ScrollUp;
      this.checkBoxMouseScrollUp.Location = new System.Drawing.Point(48, 40);
      this.checkBoxMouseScrollUp.Name = "checkBoxMouseScrollUp";
      this.checkBoxMouseScrollUp.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseScrollUp.TabIndex = 0;
      this.checkBoxMouseScrollUp.UseVisualStyleBackColor = true;
      // 
      // groupBoxMouseClick
      // 
      this.groupBoxMouseClick.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxMouseClick.Controls.Add(this.checkBoxMouseClickRight);
      this.groupBoxMouseClick.Controls.Add(this.checkBoxMouseClickMiddle);
      this.groupBoxMouseClick.Controls.Add(this.checkBoxMouseClickLeft);
      this.groupBoxMouseClick.Location = new System.Drawing.Point(160, 8);
      this.groupBoxMouseClick.Name = "groupBoxMouseClick";
      this.groupBoxMouseClick.Size = new System.Drawing.Size(128, 176);
      this.groupBoxMouseClick.TabIndex = 5;
      this.groupBoxMouseClick.TabStop = false;
      this.groupBoxMouseClick.Text = "Click";
      // 
      // checkBoxMouseClickRight
      // 
      this.checkBoxMouseClickRight.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseClickRight.Image = global::IrssUtils.Properties.Resources.ClickRight;
      this.checkBoxMouseClickRight.Location = new System.Drawing.Point(88, 64);
      this.checkBoxMouseClickRight.Name = "checkBoxMouseClickRight";
      this.checkBoxMouseClickRight.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseClickRight.TabIndex = 2;
      this.checkBoxMouseClickRight.UseVisualStyleBackColor = true;
      // 
      // checkBoxMouseClickMiddle
      // 
      this.checkBoxMouseClickMiddle.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseClickMiddle.Image = global::IrssUtils.Properties.Resources.ClickMiddle;
      this.checkBoxMouseClickMiddle.Location = new System.Drawing.Point(48, 64);
      this.checkBoxMouseClickMiddle.Name = "checkBoxMouseClickMiddle";
      this.checkBoxMouseClickMiddle.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseClickMiddle.TabIndex = 1;
      this.checkBoxMouseClickMiddle.UseVisualStyleBackColor = true;
      // 
      // checkBoxMouseClickLeft
      // 
      this.checkBoxMouseClickLeft.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseClickLeft.Image = global::IrssUtils.Properties.Resources.ClickLeft;
      this.checkBoxMouseClickLeft.Location = new System.Drawing.Point(8, 64);
      this.checkBoxMouseClickLeft.Name = "checkBoxMouseClickLeft";
      this.checkBoxMouseClickLeft.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseClickLeft.TabIndex = 0;
      this.checkBoxMouseClickLeft.UseVisualStyleBackColor = true;
      // 
      // groupBoxMouseMove
      // 
      this.groupBoxMouseMove.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveLeft);
      this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveDown);
      this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveRight);
      this.groupBoxMouseMove.Controls.Add(this.checkBoxMouseMoveUp);
      this.groupBoxMouseMove.Controls.Add(this.labelMouseMove);
      this.groupBoxMouseMove.Controls.Add(this.numericUpDownMouseMove);
      this.groupBoxMouseMove.Location = new System.Drawing.Point(8, 8);
      this.groupBoxMouseMove.Name = "groupBoxMouseMove";
      this.groupBoxMouseMove.Size = new System.Drawing.Size(144, 176);
      this.groupBoxMouseMove.TabIndex = 4;
      this.groupBoxMouseMove.TabStop = false;
      this.groupBoxMouseMove.Text = "Move";
      // 
      // checkBoxMouseMoveLeft
      // 
      this.checkBoxMouseMoveLeft.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveLeft.Image = global::IrssUtils.Properties.Resources.MoveLeft;
      this.checkBoxMouseMoveLeft.Location = new System.Drawing.Point(24, 64);
      this.checkBoxMouseMoveLeft.Name = "checkBoxMouseMoveLeft";
      this.checkBoxMouseMoveLeft.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveLeft.TabIndex = 1;
      this.checkBoxMouseMoveLeft.UseVisualStyleBackColor = true;
      // 
      // checkBoxMouseMoveDown
      // 
      this.checkBoxMouseMoveDown.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveDown.Image = global::IrssUtils.Properties.Resources.MoveDown;
      this.checkBoxMouseMoveDown.Location = new System.Drawing.Point(56, 96);
      this.checkBoxMouseMoveDown.Name = "checkBoxMouseMoveDown";
      this.checkBoxMouseMoveDown.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveDown.TabIndex = 3;
      this.checkBoxMouseMoveDown.UseVisualStyleBackColor = true;
      // 
      // checkBoxMouseMoveRight
      // 
      this.checkBoxMouseMoveRight.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveRight.Image = global::IrssUtils.Properties.Resources.MoveRight;
      this.checkBoxMouseMoveRight.Location = new System.Drawing.Point(88, 64);
      this.checkBoxMouseMoveRight.Name = "checkBoxMouseMoveRight";
      this.checkBoxMouseMoveRight.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveRight.TabIndex = 2;
      this.checkBoxMouseMoveRight.UseVisualStyleBackColor = true;
      // 
      // checkBoxMouseMoveUp
      // 
      this.checkBoxMouseMoveUp.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkBoxMouseMoveUp.Image = global::IrssUtils.Properties.Resources.MoveUp;
      this.checkBoxMouseMoveUp.Location = new System.Drawing.Point(56, 32);
      this.checkBoxMouseMoveUp.Name = "checkBoxMouseMoveUp";
      this.checkBoxMouseMoveUp.Size = new System.Drawing.Size(32, 32);
      this.checkBoxMouseMoveUp.TabIndex = 0;
      this.checkBoxMouseMoveUp.UseVisualStyleBackColor = true;
      // 
      // labelMouseMove
      // 
      this.labelMouseMove.Location = new System.Drawing.Point(8, 144);
      this.labelMouseMove.Name = "labelMouseMove";
      this.labelMouseMove.Size = new System.Drawing.Size(72, 20);
      this.labelMouseMove.TabIndex = 4;
      this.labelMouseMove.Text = "Distance:";
      this.labelMouseMove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownMouseMove
      // 
      this.numericUpDownMouseMove.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownMouseMove.Location = new System.Drawing.Point(80, 144);
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
      this.numericUpDownMouseMove.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
      // 
      // MouseCommand
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(432, 231);
      this.Controls.Add(this.groupBoxMouseScroll);
      this.Controls.Add(this.groupBoxMouseClick);
      this.Controls.Add(this.groupBoxMouseMove);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(440, 258);
      this.Name = "MouseCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Mouse Command";
      this.groupBoxMouseScroll.ResumeLayout(false);
      this.groupBoxMouseClick.ResumeLayout(false);
      this.groupBoxMouseMove.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMouseMove)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.GroupBox groupBoxMouseScroll;
    private System.Windows.Forms.CheckBox checkBoxMouseScrollDown;
    private System.Windows.Forms.CheckBox checkBoxMouseScrollUp;
    private System.Windows.Forms.GroupBox groupBoxMouseClick;
    private System.Windows.Forms.CheckBox checkBoxMouseClickRight;
    private System.Windows.Forms.CheckBox checkBoxMouseClickMiddle;
    private System.Windows.Forms.CheckBox checkBoxMouseClickLeft;
    private System.Windows.Forms.GroupBox groupBoxMouseMove;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveLeft;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveDown;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveRight;
    private System.Windows.Forms.CheckBox checkBoxMouseMoveUp;
    private System.Windows.Forms.Label labelMouseMove;
    private System.Windows.Forms.NumericUpDown numericUpDownMouseMove;
  }

}
