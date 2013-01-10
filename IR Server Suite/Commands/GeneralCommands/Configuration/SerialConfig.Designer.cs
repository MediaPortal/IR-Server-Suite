namespace IrssCommands.General
{
  partial class SerialConfig
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
      this.checkBoxWaitForResponse = new System.Windows.Forms.CheckBox();
      this.comboBoxPort = new System.Windows.Forms.ComboBox();
      this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
      this.numericUpDownDataBits = new System.Windows.Forms.NumericUpDown();
      this.comboBoxParity = new System.Windows.Forms.ComboBox();
      this.numericUpDownBaudRate = new System.Windows.Forms.NumericUpDown();
      this.buttonParamQuestion = new System.Windows.Forms.Button();
      this.textBoxCommand = new System.Windows.Forms.TextBox();
      this.groupBoxPortSetup = new System.Windows.Forms.GroupBox();
      this.labelStopBits = new System.Windows.Forms.Label();
      this.labelDataBits = new System.Windows.Forms.Label();
      this.labelParity = new System.Windows.Forms.Label();
      this.labelBaudRate = new System.Windows.Forms.Label();
      this.labelComPort = new System.Windows.Forms.Label();
      this.labelCommand = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataBits)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBaudRate)).BeginInit();
      this.groupBoxPortSetup.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBoxWaitForResponse
      // 
      this.checkBoxWaitForResponse.Location = new System.Drawing.Point(192, 88);
      this.checkBoxWaitForResponse.Name = "checkBoxWaitForResponse";
      this.checkBoxWaitForResponse.Size = new System.Drawing.Size(168, 21);
      this.checkBoxWaitForResponse.TabIndex = 10;
      this.checkBoxWaitForResponse.Text = "Wait for response";
      this.toolTip.SetToolTip(this.checkBoxWaitForResponse, "Wait up to 5 seconds for a response after the command has been sent");
      this.checkBoxWaitForResponse.UseVisualStyleBackColor = true;
      // 
      // comboBoxPort
      // 
      this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPort.FormattingEnabled = true;
      this.comboBoxPort.Location = new System.Drawing.Point(80, 24);
      this.comboBoxPort.Name = "comboBoxPort";
      this.comboBoxPort.Size = new System.Drawing.Size(96, 21);
      this.comboBoxPort.TabIndex = 1;
      this.toolTip.SetToolTip(this.comboBoxPort, "Select a port to send the command to");
      // 
      // comboBoxStopBits
      // 
      this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxStopBits.FormattingEnabled = true;
      this.comboBoxStopBits.Location = new System.Drawing.Point(80, 88);
      this.comboBoxStopBits.Name = "comboBoxStopBits";
      this.comboBoxStopBits.Size = new System.Drawing.Size(96, 21);
      this.comboBoxStopBits.TabIndex = 9;
      this.toolTip.SetToolTip(this.comboBoxStopBits, "Select number of stop bits");
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
      this.toolTip.SetToolTip(this.numericUpDownDataBits, "Select number of data bits");
      this.numericUpDownDataBits.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
      // 
      // comboBoxParity
      // 
      this.comboBoxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxParity.FormattingEnabled = true;
      this.comboBoxParity.Location = new System.Drawing.Point(80, 56);
      this.comboBoxParity.Name = "comboBoxParity";
      this.comboBoxParity.Size = new System.Drawing.Size(96, 21);
      this.comboBoxParity.TabIndex = 5;
      this.toolTip.SetToolTip(this.comboBoxParity, "Select data parity");
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
      this.toolTip.SetToolTip(this.numericUpDownBaudRate, "Port baud rate");
      this.numericUpDownBaudRate.Value = new decimal(new int[] {
            9600,
            0,
            0,
            0});
      // 
      // buttonParamQuestion
      // 
      this.buttonParamQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonParamQuestion.Location = new System.Drawing.Point(343, 15);
      this.buttonParamQuestion.Name = "buttonParamQuestion";
      this.buttonParamQuestion.Size = new System.Drawing.Size(32, 20);
      this.buttonParamQuestion.TabIndex = 6;
      this.buttonParamQuestion.Text = "?";
      this.toolTip.SetToolTip(this.buttonParamQuestion, "Click here to see available parameter substitutions");
      this.buttonParamQuestion.UseVisualStyleBackColor = true;
      this.buttonParamQuestion.Click += new System.EventHandler(this.buttonParamQuestion_Click);
      // 
      // textBoxCommand
      // 
      this.textBoxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxCommand.Location = new System.Drawing.Point(3, 16);
      this.textBoxCommand.Name = "textBoxCommand";
      this.textBoxCommand.Size = new System.Drawing.Size(334, 20);
      this.textBoxCommand.TabIndex = 5;
      this.toolTip.SetToolTip(this.textBoxCommand, "Enter the serial command to transmit here");
      // 
      // groupBoxPortSetup
      // 
      this.groupBoxPortSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
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
      this.groupBoxPortSetup.Location = new System.Drawing.Point(3, 48);
      this.groupBoxPortSetup.Name = "groupBoxPortSetup";
      this.groupBoxPortSetup.Size = new System.Drawing.Size(372, 118);
      this.groupBoxPortSetup.TabIndex = 7;
      this.groupBoxPortSetup.TabStop = false;
      this.groupBoxPortSetup.Text = "Port Setup";
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
      // labelDataBits
      // 
      this.labelDataBits.Location = new System.Drawing.Point(192, 56);
      this.labelDataBits.Name = "labelDataBits";
      this.labelDataBits.Size = new System.Drawing.Size(72, 20);
      this.labelDataBits.TabIndex = 6;
      this.labelDataBits.Text = "Data bits:";
      this.labelDataBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
      // labelCommand
      // 
      this.labelCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelCommand.Location = new System.Drawing.Point(3, 0);
      this.labelCommand.Name = "labelCommand";
      this.labelCommand.Size = new System.Drawing.Size(159, 16);
      this.labelCommand.TabIndex = 4;
      this.labelCommand.Text = "Command:";
      this.labelCommand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // SerialConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxPortSetup);
      this.Controls.Add(this.buttonParamQuestion);
      this.Controls.Add(this.textBoxCommand);
      this.Controls.Add(this.labelCommand);
      this.Name = "SerialConfig";
      this.Size = new System.Drawing.Size(378, 169);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDataBits)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBaudRate)).EndInit();
      this.groupBoxPortSetup.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.GroupBox groupBoxPortSetup;
    private System.Windows.Forms.CheckBox checkBoxWaitForResponse;
    private System.Windows.Forms.ComboBox comboBoxPort;
    private System.Windows.Forms.ComboBox comboBoxStopBits;
    private System.Windows.Forms.Label labelStopBits;
    private System.Windows.Forms.NumericUpDown numericUpDownDataBits;
    private System.Windows.Forms.Label labelDataBits;
    private System.Windows.Forms.ComboBox comboBoxParity;
    private System.Windows.Forms.Label labelParity;
    private System.Windows.Forms.NumericUpDown numericUpDownBaudRate;
    private System.Windows.Forms.Label labelBaudRate;
    private System.Windows.Forms.Label labelComPort;
    private System.Windows.Forms.Button buttonParamQuestion;
    private System.Windows.Forms.TextBox textBoxCommand;
    private System.Windows.Forms.Label labelCommand;
  }
}
