namespace SageSetup
{
  partial class FormMain
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
      this.buttonSet = new System.Windows.Forms.Button();
      this.groupBoxIRBlast = new System.Windows.Forms.GroupBox();
      this.radioButtonConsole = new System.Windows.Forms.RadioButton();
      this.radioButtonWindowless = new System.Windows.Forms.RadioButton();
      this.groupBoxSagePlugin = new System.Windows.Forms.GroupBox();
      this.radioButtonExeTuner = new System.Windows.Forms.RadioButton();
      this.radioButtonExeMultiTuner = new System.Windows.Forms.RadioButton();
      this.groupBoxIRServer = new System.Windows.Forms.GroupBox();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.groupBoxChannelFormat = new System.Windows.Forms.GroupBox();
      this.checkBoxPad = new System.Windows.Forms.CheckBox();
      this.numericUpDownPad = new System.Windows.Forms.NumericUpDown();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxIRBlast.SuspendLayout();
      this.groupBoxSagePlugin.SuspendLayout();
      this.groupBoxIRServer.SuspendLayout();
      this.groupBoxChannelFormat.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPad)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonSet
      // 
      this.buttonSet.Location = new System.Drawing.Point(184, 312);
      this.buttonSet.Name = "buttonSet";
      this.buttonSet.Size = new System.Drawing.Size(56, 24);
      this.buttonSet.TabIndex = 4;
      this.buttonSet.Text = "Set!";
      this.toolTips.SetToolTip(this.buttonSet, "Put the setttings in place");
      this.buttonSet.UseVisualStyleBackColor = true;
      this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
      // 
      // groupBoxIRBlast
      // 
      this.groupBoxIRBlast.Controls.Add(this.radioButtonWindowless);
      this.groupBoxIRBlast.Controls.Add(this.radioButtonConsole);
      this.groupBoxIRBlast.Location = new System.Drawing.Point(8, 96);
      this.groupBoxIRBlast.Name = "groupBoxIRBlast";
      this.groupBoxIRBlast.Size = new System.Drawing.Size(232, 80);
      this.groupBoxIRBlast.TabIndex = 1;
      this.groupBoxIRBlast.TabStop = false;
      this.groupBoxIRBlast.Text = "IR Blast";
      // 
      // radioButtonConsole
      // 
      this.radioButtonConsole.AutoSize = true;
      this.radioButtonConsole.Checked = true;
      this.radioButtonConsole.Location = new System.Drawing.Point(8, 24);
      this.radioButtonConsole.Name = "radioButtonConsole";
      this.radioButtonConsole.Size = new System.Drawing.Size(121, 17);
      this.radioButtonConsole.TabIndex = 0;
      this.radioButtonConsole.TabStop = true;
      this.radioButtonConsole.Text = "Use console version";
      this.toolTips.SetToolTip(this.radioButtonConsole, "Use the console version of IR Blast");
      this.radioButtonConsole.UseVisualStyleBackColor = true;
      // 
      // radioButtonWindowless
      // 
      this.radioButtonWindowless.AutoSize = true;
      this.radioButtonWindowless.Location = new System.Drawing.Point(8, 48);
      this.radioButtonWindowless.Name = "radioButtonWindowless";
      this.radioButtonWindowless.Size = new System.Drawing.Size(138, 17);
      this.radioButtonWindowless.TabIndex = 1;
      this.radioButtonWindowless.Text = "Use windowless version";
      this.toolTips.SetToolTip(this.radioButtonWindowless, "Use the No Window version of IR Blast");
      this.radioButtonWindowless.UseVisualStyleBackColor = true;
      // 
      // groupBoxSagePlugin
      // 
      this.groupBoxSagePlugin.Controls.Add(this.radioButtonExeMultiTuner);
      this.groupBoxSagePlugin.Controls.Add(this.radioButtonExeTuner);
      this.groupBoxSagePlugin.Location = new System.Drawing.Point(8, 8);
      this.groupBoxSagePlugin.Name = "groupBoxSagePlugin";
      this.groupBoxSagePlugin.Size = new System.Drawing.Size(232, 80);
      this.groupBoxSagePlugin.TabIndex = 0;
      this.groupBoxSagePlugin.TabStop = false;
      this.groupBoxSagePlugin.Text = "Sage Plugin";
      // 
      // radioButtonExeTuner
      // 
      this.radioButtonExeTuner.AutoSize = true;
      this.radioButtonExeTuner.Checked = true;
      this.radioButtonExeTuner.Location = new System.Drawing.Point(8, 24);
      this.radioButtonExeTuner.Name = "radioButtonExeTuner";
      this.radioButtonExeTuner.Size = new System.Drawing.Size(96, 17);
      this.radioButtonExeTuner.TabIndex = 0;
      this.radioButtonExeTuner.TabStop = true;
      this.radioButtonExeTuner.Text = "Use EXETuner";
      this.toolTips.SetToolTip(this.radioButtonExeTuner, "Use the Sage EXE Tuner plugin");
      this.radioButtonExeTuner.UseVisualStyleBackColor = true;
      // 
      // radioButtonExeMultiTuner
      // 
      this.radioButtonExeMultiTuner.AutoSize = true;
      this.radioButtonExeMultiTuner.Location = new System.Drawing.Point(8, 48);
      this.radioButtonExeMultiTuner.Name = "radioButtonExeMultiTuner";
      this.radioButtonExeMultiTuner.Size = new System.Drawing.Size(118, 17);
      this.radioButtonExeMultiTuner.TabIndex = 1;
      this.radioButtonExeMultiTuner.Text = "Use EXEMultiTuner";
      this.toolTips.SetToolTip(this.radioButtonExeMultiTuner, "Use the Sage EXE Multi Tuner plugin");
      this.radioButtonExeMultiTuner.UseVisualStyleBackColor = true;
      // 
      // groupBoxIRServer
      // 
      this.groupBoxIRServer.Controls.Add(this.comboBoxComputer);
      this.groupBoxIRServer.Location = new System.Drawing.Point(8, 184);
      this.groupBoxIRServer.Name = "groupBoxIRServer";
      this.groupBoxIRServer.Size = new System.Drawing.Size(232, 56);
      this.groupBoxIRServer.TabIndex = 2;
      this.groupBoxIRServer.TabStop = false;
      this.groupBoxIRServer.Text = "IR Server";
      // 
      // comboBoxComputer
      // 
      this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxComputer.FormattingEnabled = true;
      this.comboBoxComputer.Location = new System.Drawing.Point(8, 24);
      this.comboBoxComputer.Name = "comboBoxComputer";
      this.comboBoxComputer.Size = new System.Drawing.Size(216, 21);
      this.comboBoxComputer.TabIndex = 0;
      this.toolTips.SetToolTip(this.comboBoxComputer, "Host computer for IR Server");
      // 
      // groupBoxChannelFormat
      // 
      this.groupBoxChannelFormat.Controls.Add(this.numericUpDownPad);
      this.groupBoxChannelFormat.Controls.Add(this.checkBoxPad);
      this.groupBoxChannelFormat.Location = new System.Drawing.Point(8, 248);
      this.groupBoxChannelFormat.Name = "groupBoxChannelFormat";
      this.groupBoxChannelFormat.Size = new System.Drawing.Size(232, 56);
      this.groupBoxChannelFormat.TabIndex = 3;
      this.groupBoxChannelFormat.TabStop = false;
      this.groupBoxChannelFormat.Text = "Channel Format";
      // 
      // checkBoxPad
      // 
      this.checkBoxPad.Location = new System.Drawing.Point(8, 24);
      this.checkBoxPad.Name = "checkBoxPad";
      this.checkBoxPad.Size = new System.Drawing.Size(144, 20);
      this.checkBoxPad.TabIndex = 0;
      this.checkBoxPad.Text = "Pad channel number";
      this.toolTips.SetToolTip(this.checkBoxPad, "Do you want to pad channel numbers?");
      this.checkBoxPad.UseVisualStyleBackColor = true;
      // 
      // numericUpDownPad
      // 
      this.numericUpDownPad.Location = new System.Drawing.Point(152, 24);
      this.numericUpDownPad.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
      this.numericUpDownPad.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this.numericUpDownPad.Name = "numericUpDownPad";
      this.numericUpDownPad.Size = new System.Drawing.Size(72, 20);
      this.numericUpDownPad.TabIndex = 1;
      this.numericUpDownPad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTips.SetToolTip(this.numericUpDownPad, "Pad out channel numbers to this many digits");
      this.numericUpDownPad.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(248, 344);
      this.Controls.Add(this.groupBoxChannelFormat);
      this.Controls.Add(this.groupBoxIRServer);
      this.Controls.Add(this.groupBoxSagePlugin);
      this.Controls.Add(this.groupBoxIRBlast);
      this.Controls.Add(this.buttonSet);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormMain";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "IR Server - Sage Setup";
      this.Load += new System.EventHandler(this.FormMain_Load);
      this.groupBoxIRBlast.ResumeLayout(false);
      this.groupBoxIRBlast.PerformLayout();
      this.groupBoxSagePlugin.ResumeLayout(false);
      this.groupBoxSagePlugin.PerformLayout();
      this.groupBoxIRServer.ResumeLayout(false);
      this.groupBoxChannelFormat.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPad)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonSet;
    private System.Windows.Forms.GroupBox groupBoxIRBlast;
    private System.Windows.Forms.RadioButton radioButtonWindowless;
    private System.Windows.Forms.RadioButton radioButtonConsole;
    private System.Windows.Forms.GroupBox groupBoxSagePlugin;
    private System.Windows.Forms.RadioButton radioButtonExeMultiTuner;
    private System.Windows.Forms.RadioButton radioButtonExeTuner;
    private System.Windows.Forms.GroupBox groupBoxIRServer;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.GroupBox groupBoxChannelFormat;
    private System.Windows.Forms.CheckBox checkBoxPad;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.NumericUpDown numericUpDownPad;
  }
}

