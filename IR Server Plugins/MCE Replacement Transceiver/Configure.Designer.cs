namespace MceReplacementTransceiver
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
      this.labelButtonRepeatDelay = new System.Windows.Forms.Label();
      this.labelButtonHeldDelay = new System.Windows.Forms.Label();
      this.numericUpDownButtonRepeatDelay = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownButtonHeldDelay = new System.Windows.Forms.NumericUpDown();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBoxTimes = new System.Windows.Forms.GroupBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.comboBoxBlasterType = new System.Windows.Forms.ComboBox();
      this.labelBlasterType = new System.Windows.Forms.Label();
      this.groupBoxBlaster = new System.Windows.Forms.GroupBox();
      this.groupBoxLearnTimeout = new System.Windows.Forms.GroupBox();
      this.labelLearnIRTimeout = new System.Windows.Forms.Label();
      this.numericUpDownLearnTimeout = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).BeginInit();
      this.groupBoxTimes.SuspendLayout();
      this.groupBoxBlaster.SuspendLayout();
      this.groupBoxLearnTimeout.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearnTimeout)).BeginInit();
      this.SuspendLayout();
      // 
      // labelButtonRepeatDelay
      // 
      this.labelButtonRepeatDelay.Location = new System.Drawing.Point(8, 24);
      this.labelButtonRepeatDelay.Name = "labelButtonRepeatDelay";
      this.labelButtonRepeatDelay.Size = new System.Drawing.Size(120, 20);
      this.labelButtonRepeatDelay.TabIndex = 0;
      this.labelButtonRepeatDelay.Text = "Button repeat delay:";
      this.labelButtonRepeatDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelButtonHeldDelay
      // 
      this.labelButtonHeldDelay.Location = new System.Drawing.Point(8, 56);
      this.labelButtonHeldDelay.Name = "labelButtonHeldDelay";
      this.labelButtonHeldDelay.Size = new System.Drawing.Size(120, 20);
      this.labelButtonHeldDelay.TabIndex = 2;
      this.labelButtonHeldDelay.Text = "Button held delay:";
      this.labelButtonHeldDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownButtonRepeatDelay
      // 
      this.numericUpDownButtonRepeatDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Location = new System.Drawing.Point(136, 24);
      this.numericUpDownButtonRepeatDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonRepeatDelay.Name = "numericUpDownButtonRepeatDelay";
      this.numericUpDownButtonRepeatDelay.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownButtonRepeatDelay.TabIndex = 1;
      this.numericUpDownButtonRepeatDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownButtonRepeatDelay.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownButtonRepeatDelay, "When the button is held this is the time between the first press and the first re" +
              "peat");
      this.numericUpDownButtonRepeatDelay.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // numericUpDownButtonHeldDelay
      // 
      this.numericUpDownButtonHeldDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonHeldDelay.Location = new System.Drawing.Point(136, 56);
      this.numericUpDownButtonHeldDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericUpDownButtonHeldDelay.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
      this.numericUpDownButtonHeldDelay.Name = "numericUpDownButtonHeldDelay";
      this.numericUpDownButtonHeldDelay.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownButtonHeldDelay.TabIndex = 3;
      this.numericUpDownButtonHeldDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownButtonHeldDelay.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownButtonHeldDelay, "When the button is held this is the time between repeats");
      this.numericUpDownButtonHeldDelay.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(96, 232);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(168, 232);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // groupBoxTimes
      // 
      this.groupBoxTimes.Controls.Add(this.labelButtonRepeatDelay);
      this.groupBoxTimes.Controls.Add(this.numericUpDownButtonHeldDelay);
      this.groupBoxTimes.Controls.Add(this.labelButtonHeldDelay);
      this.groupBoxTimes.Controls.Add(this.numericUpDownButtonRepeatDelay);
      this.groupBoxTimes.Location = new System.Drawing.Point(8, 72);
      this.groupBoxTimes.Name = "groupBoxTimes";
      this.groupBoxTimes.Size = new System.Drawing.Size(224, 88);
      this.groupBoxTimes.TabIndex = 1;
      this.groupBoxTimes.TabStop = false;
      this.groupBoxTimes.Text = "Remote button timing (in milliseconds)";
      // 
      // comboBoxBlasterType
      // 
      this.comboBoxBlasterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxBlasterType.FormattingEnabled = true;
      this.comboBoxBlasterType.Location = new System.Drawing.Point(128, 24);
      this.comboBoxBlasterType.Name = "comboBoxBlasterType";
      this.comboBoxBlasterType.Size = new System.Drawing.Size(88, 21);
      this.comboBoxBlasterType.TabIndex = 1;
      this.toolTips.SetToolTip(this.comboBoxBlasterType, "Choose between Microsoft or SMK manufactured MCE IR transceivers");
      // 
      // labelBlasterType
      // 
      this.labelBlasterType.Location = new System.Drawing.Point(8, 24);
      this.labelBlasterType.Name = "labelBlasterType";
      this.labelBlasterType.Size = new System.Drawing.Size(120, 21);
      this.labelBlasterType.TabIndex = 0;
      this.labelBlasterType.Text = "Blaster manufacturer:";
      this.labelBlasterType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // groupBoxBlaster
      // 
      this.groupBoxBlaster.Controls.Add(this.labelBlasterType);
      this.groupBoxBlaster.Controls.Add(this.comboBoxBlasterType);
      this.groupBoxBlaster.Location = new System.Drawing.Point(8, 8);
      this.groupBoxBlaster.Name = "groupBoxBlaster";
      this.groupBoxBlaster.Size = new System.Drawing.Size(224, 56);
      this.groupBoxBlaster.TabIndex = 0;
      this.groupBoxBlaster.TabStop = false;
      this.groupBoxBlaster.Text = "Blaster setup";
      // 
      // groupBoxLearnTimeout
      // 
      this.groupBoxLearnTimeout.Controls.Add(this.labelLearnIRTimeout);
      this.groupBoxLearnTimeout.Controls.Add(this.numericUpDownLearnTimeout);
      this.groupBoxLearnTimeout.Location = new System.Drawing.Point(8, 168);
      this.groupBoxLearnTimeout.Name = "groupBoxLearnTimeout";
      this.groupBoxLearnTimeout.Size = new System.Drawing.Size(224, 56);
      this.groupBoxLearnTimeout.TabIndex = 4;
      this.groupBoxLearnTimeout.TabStop = false;
      this.groupBoxLearnTimeout.Text = "Learn IR timeout (in milliseconds)";
      // 
      // labelLearnIRTimeout
      // 
      this.labelLearnIRTimeout.Location = new System.Drawing.Point(8, 24);
      this.labelLearnIRTimeout.Name = "labelLearnIRTimeout";
      this.labelLearnIRTimeout.Size = new System.Drawing.Size(120, 20);
      this.labelLearnIRTimeout.TabIndex = 2;
      this.labelLearnIRTimeout.Text = "Learn IR timeout:";
      this.labelLearnIRTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownLearnTimeout
      // 
      this.numericUpDownLearnTimeout.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Location = new System.Drawing.Point(136, 24);
      this.numericUpDownLearnTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.numericUpDownLearnTimeout.Name = "numericUpDownLearnTimeout";
      this.numericUpDownLearnTimeout.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownLearnTimeout.TabIndex = 3;
      this.numericUpDownLearnTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownLearnTimeout.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownLearnTimeout, "When teaching IR commands this is how long before the process times out");
      this.numericUpDownLearnTimeout.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
      // 
      // Configure
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(240, 263);
      this.ControlBox = false;
      this.Controls.Add(this.groupBoxLearnTimeout);
      this.Controls.Add(this.groupBoxBlaster);
      this.Controls.Add(this.groupBoxTimes);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "Configure";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "MCE Replacement Configuration";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonRepeatDelay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownButtonHeldDelay)).EndInit();
      this.groupBoxTimes.ResumeLayout(false);
      this.groupBoxBlaster.ResumeLayout(false);
      this.groupBoxLearnTimeout.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLearnTimeout)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label labelButtonRepeatDelay;
    private System.Windows.Forms.Label labelButtonHeldDelay;
    private System.Windows.Forms.NumericUpDown numericUpDownButtonRepeatDelay;
    private System.Windows.Forms.NumericUpDown numericUpDownButtonHeldDelay;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.GroupBox groupBoxTimes;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.ComboBox comboBoxBlasterType;
    private System.Windows.Forms.Label labelBlasterType;
    private System.Windows.Forms.GroupBox groupBoxBlaster;
    private System.Windows.Forms.GroupBox groupBoxLearnTimeout;
    private System.Windows.Forms.Label labelLearnIRTimeout;
    private System.Windows.Forms.NumericUpDown numericUpDownLearnTimeout;
  }
}