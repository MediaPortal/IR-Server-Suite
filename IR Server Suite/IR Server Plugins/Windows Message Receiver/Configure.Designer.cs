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
      this.labelMessageType = new System.Windows.Forms.Label();
      this.labelWParam = new System.Windows.Forms.Label();
      this.numericUpDownMessageType = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownWParam = new System.Windows.Forms.NumericUpDown();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.textBoxWindowTitle = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMessageType)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWParam)).BeginInit();
      this.SuspendLayout();
      // 
      // labelMessageType
      // 
      this.labelMessageType.Location = new System.Drawing.Point(8, 8);
      this.labelMessageType.Name = "labelMessageType";
      this.labelMessageType.Size = new System.Drawing.Size(136, 20);
      this.labelMessageType.TabIndex = 0;
      this.labelMessageType.Text = "Message type:";
      this.labelMessageType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelWParam
      // 
      this.labelWParam.Location = new System.Drawing.Point(8, 40);
      this.labelWParam.Name = "labelWParam";
      this.labelWParam.Size = new System.Drawing.Size(136, 20);
      this.labelWParam.TabIndex = 2;
      this.labelWParam.Text = "Match WParam:";
      this.labelWParam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownMessageType
      // 
      this.numericUpDownMessageType.Location = new System.Drawing.Point(152, 8);
      this.numericUpDownMessageType.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
      this.numericUpDownMessageType.Name = "numericUpDownMessageType";
      this.numericUpDownMessageType.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownMessageType.TabIndex = 1;
      this.numericUpDownMessageType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownMessageType.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownMessageType, "When the button is held this is the time between the first press and the first repeat");
      this.numericUpDownMessageType.Value = new decimal(new int[] {
            32768,
            0,
            0,
            0});
      // 
      // numericUpDownWParam
      // 
      this.numericUpDownWParam.Location = new System.Drawing.Point(152, 40);
      this.numericUpDownWParam.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
      this.numericUpDownWParam.Name = "numericUpDownWParam";
      this.numericUpDownWParam.Size = new System.Drawing.Size(80, 20);
      this.numericUpDownWParam.TabIndex = 3;
      this.numericUpDownWParam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownWParam.ThousandsSeparator = true;
      this.toolTips.SetToolTip(this.numericUpDownWParam, "When the button is held this is the time between repeats");
      this.numericUpDownWParam.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(96, 144);
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
      this.buttonCancel.Location = new System.Drawing.Point(168, 144);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxWindowTitle
      // 
      this.textBoxWindowTitle.AcceptsReturn = true;
      this.textBoxWindowTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxWindowTitle.Location = new System.Drawing.Point(8, 72);
      this.textBoxWindowTitle.Multiline = true;
      this.textBoxWindowTitle.Name = "textBoxWindowTitle";
      this.textBoxWindowTitle.ReadOnly = true;
      this.textBoxWindowTitle.Size = new System.Drawing.Size(224, 64);
      this.textBoxWindowTitle.TabIndex = 6;
      this.textBoxWindowTitle.Text = "Target window information";
      // 
      // Configure
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(240, 176);
      this.ControlBox = false;
      this.Controls.Add(this.textBoxWindowTitle);
      this.Controls.Add(this.labelMessageType);
      this.Controls.Add(this.numericUpDownWParam);
      this.Controls.Add(this.labelWParam);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.numericUpDownMessageType);
      this.Controls.Add(this.buttonOK);
      this.MinimumSize = new System.Drawing.Size(248, 210);
      this.Name = "Configure";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Windows Messages Configuration";
      this.Load += new System.EventHandler(this.Configure_Load);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMessageType)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWParam)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    
    private System.Windows.Forms.Label labelMessageType;
    private System.Windows.Forms.Label labelWParam;
    private System.Windows.Forms.NumericUpDown numericUpDownMessageType;
    private System.Windows.Forms.NumericUpDown numericUpDownWParam;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxWindowTitle;
  }
}