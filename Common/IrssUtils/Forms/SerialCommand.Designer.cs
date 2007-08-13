namespace IrssUtils.Forms
{
  partial class SerialCommand
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
      this.labelCommand = new System.Windows.Forms.Label();
      this.textBoxCommand = new System.Windows.Forms.TextBox();
      this.buttonParamQuestion = new System.Windows.Forms.Button();
      this.groupBoxPortSetup = new System.Windows.Forms.GroupBox();
      this.comboBoxPort = new System.Windows.Forms.ComboBox();
      this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
      this.labelStopBits = new System.Windows.Forms.Label();
      this.numericUpDownDataBits = new System.Windows.Forms.NumericUpDown();
      this.labelDataBits = new System.Windows.Forms.Label();
      this.comboBoxParity = new System.Windows.Forms.ComboBox();
      this.labelParity = new System.Windows.Forms.Label();
      this.numericUpDownBaudRate = new System.Windows.Forms.NumericUpDown();
      this.labelBaudRate = new System.Windows.Forms.Label();
      this.labelComPort = new System.Windows.Forms.Label();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.checkBoxWaitForResponse = new System.Windows.Forms.CheckBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxPortSetup.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataBits)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBaudRate)).BeginInit();
      this.SuspendLayout();
      // 
      // labelCommand
      // 
      this.labelCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelCommand.Location = new System.Drawing.Point(8, 8);
      this.labelCommand.Name = "labelCommand";
      this.labelCommand.Size = new System.Drawing.Size(328, 16);
      this.labelCommand.TabIndex = 0;
      this.labelCommand.Text = "Command:";
      this.labelCommand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxCommand
      // 
      this.textBoxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxCommand.Location = new System.Drawing.Point(8, 24);
      this.textBoxCommand.Name = "textBoxCommand";
      this.textBoxCommand.Size = new System.Drawing.Size(328, 20);
      this.textBoxCommand.TabIndex = 1;
      this.toolTips.SetToolTip(this.textBoxCommand, "Enter the serial command to transmit here");
      // 
      // buttonParamQuestion
      // 
      this.buttonParamQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonParamQuestion.Location = new System.Drawing.Point(344, 24);
      this.buttonParamQuestion.Name = "buttonParamQuestion";
      this.buttonParamQuestion.Size = new System.Drawing.Size(32, 20);
      this.buttonParamQuestion.TabIndex = 2;
      this.buttonParamQuestion.Text = "?";
      this.toolTips.SetToolTip(this.buttonParamQuestion, "Click here to see available parameter substitutions");
      this.buttonParamQuestion.UseVisualStyleBackColor = true;
      this.buttonParamQuestion.Click += new System.EventHandler(this.buttonParamQuestion_Click);
      // 
      // groupBoxPortSetup
      // 
      this.groupBoxPortSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPortSetup.Controls.Add(this.checkBoxWaitForResponse);
      this.groupBoxPortSetup.Controls.Add(this.comboBoxPort);
      this.groupBoxPortSetup.Controls.Add(this.comboBoxStopBits);
      this.groupBoxPortSetup.Controls.Add(this.labelStopBits);
      this.groupBoxPortSetup.Controls.Add(this.numericUpDownDataBits);
      this.groupBoxPortSetup.Controls.Add(this.labelDataBits);
      this.groupBoxPortSetup.Controls.Add(this.comboBoxParity);
      this.groupBoxPortSetup.Controls.Add(this.labelParity);
      this.groupBoxPortSetup.Controls.Add(this.numericUpDownBaudRate);
      this.groupBoxPortSetup.Controls.Add(this.labelBaudRate);
      this.groupBoxPortSetup.Controls.Add(this.labelComPort);
      this.groupBoxPortSetup.Location = new System.Drawing.Point(8, 56);
      this.groupBoxPortSetup.Name = "groupBoxPortSetup";
      this.groupBoxPortSetup.Size = new System.Drawing.Size(368, 120);
      this.groupBoxPortSetup.TabIndex = 3;
      this.groupBoxPortSetup.TabStop = false;
      this.groupBoxPortSetup.Text = "Port Setup";
      // 
      // comboBoxPort
      // 
      this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPort.FormattingEnabled = true;
      this.comboBoxPort.Location = new System.Drawing.Point(80, 24);
      this.comboBoxPort.Name = "comboBoxPort";
      this.comboBoxPort.Size = new System.Drawing.Size(96, 21);
      this.comboBoxPort.TabIndex = 1;
      this.toolTips.SetToolTip(this.comboBoxPort, "Select a port to send the command to");
      // 
      // comboBoxStopBits
      // 
      this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxStopBits.FormattingEnabled = true;
      this.comboBoxStopBits.Location = new System.Drawing.Point(80, 88);
      this.comboBoxStopBits.Name = "comboBoxStopBits";
      this.comboBoxStopBits.Size = new System.Drawing.Size(96, 21);
      this.comboBoxStopBits.TabIndex = 9;
      this.toolTips.SetToolTip(this.comboBoxStopBits, "Select number of stop bits");
      // 
      // labelStopBits
      // 
      this.labelStopBits.Location = new System.Drawing.Point(8, 88);
      this.labelStopBits.Name = "labelStopBits";
      this.labelStopBits.Size = new System.Drawing.Size(72, 21);
      this.labelStopBits.TabIndex = 8;
      this.labelStopBits.Text = "Stop bits:";
      this.labelStopBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownDataBits
      // 
      this.numericUpDownDataBits.Location = new System.Drawing.Point(264, 56);
      this.numericUpDownDataBits.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
      this.numericUpDownDataBits.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this.numericUpDownDataBits.Name = "numericUpDownDataBits";
      this.numericUpDownDataBits.Size = new System.Drawing.Size(96, 20);
      this.numericUpDownDataBits.TabIndex = 7;
      this.numericUpDownDataBits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownDataBits, "Select number of data bits");
      this.numericUpDownDataBits.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
      // 
      // labelDataBits
      // 
      this.labelDataBits.Location = new System.Drawing.Point(192, 56);
      this.labelDataBits.Name = "labelDataBits";
      this.labelDataBits.Size = new System.Drawing.Size(72, 20);
      this.labelDataBits.TabIndex = 6;
      this.labelDataBits.Text = "Data bits:";
      this.labelDataBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxParity
      // 
      this.comboBoxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxParity.FormattingEnabled = true;
      this.comboBoxParity.Location = new System.Drawing.Point(80, 56);
      this.comboBoxParity.Name = "comboBoxParity";
      this.comboBoxParity.Size = new System.Drawing.Size(96, 21);
      this.comboBoxParity.TabIndex = 5;
      this.toolTips.SetToolTip(this.comboBoxParity, "Select data parity");
      // 
      // labelParity
      // 
      this.labelParity.Location = new System.Drawing.Point(8, 56);
      this.labelParity.Name = "labelParity";
      this.labelParity.Size = new System.Drawing.Size(72, 21);
      this.labelParity.TabIndex = 4;
      this.labelParity.Text = "Parity:";
      this.labelParity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownBaudRate
      // 
      this.numericUpDownBaudRate.Location = new System.Drawing.Point(264, 24);
      this.numericUpDownBaudRate.Maximum = new decimal(new int[] {
            460800,
            0,
            0,
            0});
      this.numericUpDownBaudRate.Minimum = new decimal(new int[] {
            110,
            0,
            0,
            0});
      this.numericUpDownBaudRate.Name = "numericUpDownBaudRate";
      this.numericUpDownBaudRate.Size = new System.Drawing.Size(96, 20);
      this.numericUpDownBaudRate.TabIndex = 3;
      this.numericUpDownBaudRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownBaudRate, "Port baud rate");
      this.numericUpDownBaudRate.Value = new decimal(new int[] {
            9600,
            0,
            0,
            0});
      // 
      // labelBaudRate
      // 
      this.labelBaudRate.Location = new System.Drawing.Point(192, 24);
      this.labelBaudRate.Name = "labelBaudRate";
      this.labelBaudRate.Size = new System.Drawing.Size(72, 20);
      this.labelBaudRate.TabIndex = 2;
      this.labelBaudRate.Text = "Baud rate:";
      this.labelBaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelComPort
      // 
      this.labelComPort.Location = new System.Drawing.Point(8, 24);
      this.labelComPort.Name = "labelComPort";
      this.labelComPort.Size = new System.Drawing.Size(72, 20);
      this.labelComPort.TabIndex = 0;
      this.labelComPort.Text = "Com port:";
      this.labelComPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(312, 184);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(240, 184);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 5;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(8, 184);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(64, 24);
      this.buttonTest.TabIndex = 4;
      this.buttonTest.Text = "&Test";
      this.toolTips.SetToolTip(this.buttonTest, "Test this serial command");
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // checkBoxWaitForResponse
      // 
      this.checkBoxWaitForResponse.Location = new System.Drawing.Point(192, 88);
      this.checkBoxWaitForResponse.Name = "checkBoxWaitForResponse";
      this.checkBoxWaitForResponse.Size = new System.Drawing.Size(168, 21);
      this.checkBoxWaitForResponse.TabIndex = 10;
      this.checkBoxWaitForResponse.Text = "Wait for response";
      this.toolTips.SetToolTip(this.checkBoxWaitForResponse, "Wait up to 5 seconds for a response after the command has been sent");
      this.checkBoxWaitForResponse.UseVisualStyleBackColor = true;
      // 
      // SerialCommand
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(384, 216);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.groupBoxPortSetup);
      this.Controls.Add(this.buttonParamQuestion);
      this.Controls.Add(this.textBoxCommand);
      this.Controls.Add(this.labelCommand);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(392, 250);
      this.Name = "SerialCommand";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Serial Command";
      this.groupBoxPortSetup.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataBits)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBaudRate)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelCommand;
    private System.Windows.Forms.TextBox textBoxCommand;
    private System.Windows.Forms.Button buttonParamQuestion;
    private System.Windows.Forms.GroupBox groupBoxPortSetup;
    private System.Windows.Forms.Label labelComPort;
    private System.Windows.Forms.NumericUpDown numericUpDownBaudRate;
    private System.Windows.Forms.Label labelBaudRate;
    private System.Windows.Forms.ComboBox comboBoxParity;
    private System.Windows.Forms.Label labelParity;
    private System.Windows.Forms.ComboBox comboBoxStopBits;
    private System.Windows.Forms.Label labelStopBits;
    private System.Windows.Forms.NumericUpDown numericUpDownDataBits;
    private System.Windows.Forms.Label labelDataBits;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.ComboBox comboBoxPort;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.CheckBox checkBoxWaitForResponse;
  }
}