namespace IrssUtils.Forms
{
  partial class PopupMessage
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
      this.groupBoxMessageDetails = new System.Windows.Forms.GroupBox();
      this.labelSeconds = new System.Windows.Forms.Label();
      this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
      this.labelTimeout = new System.Windows.Forms.Label();
      this.textBoxText = new System.Windows.Forms.TextBox();
      this.labelText = new System.Windows.Forms.Label();
      this.textBoxHeading = new System.Windows.Forms.TextBox();
      this.labelHeading = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBoxMessageDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBoxMessageDetails
      // 
      this.groupBoxMessageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMessageDetails.Controls.Add(this.labelSeconds);
      this.groupBoxMessageDetails.Controls.Add(this.numericUpDownTimeout);
      this.groupBoxMessageDetails.Controls.Add(this.labelTimeout);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxText);
      this.groupBoxMessageDetails.Controls.Add(this.labelText);
      this.groupBoxMessageDetails.Controls.Add(this.textBoxHeading);
      this.groupBoxMessageDetails.Controls.Add(this.labelHeading);
      this.groupBoxMessageDetails.Location = new System.Drawing.Point(8, 8);
      this.groupBoxMessageDetails.Name = "groupBoxMessageDetails";
      this.groupBoxMessageDetails.Size = new System.Drawing.Size(272, 103);
      this.groupBoxMessageDetails.TabIndex = 0;
      this.groupBoxMessageDetails.TabStop = false;
      this.groupBoxMessageDetails.Text = "Message details";
      // 
      // labelSeconds
      // 
      this.labelSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelSeconds.Location = new System.Drawing.Point(144, 72);
      this.labelSeconds.Name = "labelSeconds";
      this.labelSeconds.Size = new System.Drawing.Size(120, 20);
      this.labelSeconds.TabIndex = 6;
      this.labelSeconds.Text = "seconds";
      this.labelSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownTimeout
      // 
      this.numericUpDownTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownTimeout.Location = new System.Drawing.Point(72, 72);
      this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
      this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownTimeout.Name = "numericUpDownTimeout";
      this.numericUpDownTimeout.Size = new System.Drawing.Size(64, 20);
      this.numericUpDownTimeout.TabIndex = 5;
      this.numericUpDownTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // labelTimeout
      // 
      this.labelTimeout.Location = new System.Drawing.Point(8, 72);
      this.labelTimeout.Name = "labelTimeout";
      this.labelTimeout.Size = new System.Drawing.Size(64, 20);
      this.labelTimeout.TabIndex = 4;
      this.labelTimeout.Text = "Timeout:";
      this.labelTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxText
      // 
      this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxText.Location = new System.Drawing.Point(72, 48);
      this.textBoxText.Name = "textBoxText";
      this.textBoxText.Size = new System.Drawing.Size(192, 20);
      this.textBoxText.TabIndex = 3;
      // 
      // labelText
      // 
      this.labelText.Location = new System.Drawing.Point(8, 48);
      this.labelText.Name = "labelText";
      this.labelText.Size = new System.Drawing.Size(64, 20);
      this.labelText.TabIndex = 2;
      this.labelText.Text = "Text:";
      this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxHeading
      // 
      this.textBoxHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxHeading.Location = new System.Drawing.Point(72, 24);
      this.textBoxHeading.Name = "textBoxHeading";
      this.textBoxHeading.Size = new System.Drawing.Size(192, 20);
      this.textBoxHeading.TabIndex = 1;
      // 
      // labelHeading
      // 
      this.labelHeading.Location = new System.Drawing.Point(8, 24);
      this.labelHeading.Name = "labelHeading";
      this.labelHeading.Size = new System.Drawing.Size(64, 20);
      this.labelHeading.TabIndex = 0;
      this.labelHeading.Text = "Heading:";
      this.labelHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(144, 120);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(216, 120);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // PopupMessage
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(288, 152);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.groupBoxMessageDetails);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(296, 186);
      this.Name = "PopupMessage";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Popup Message Command";
      this.groupBoxMessageDetails.ResumeLayout(false);
      this.groupBoxMessageDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxMessageDetails;
    private System.Windows.Forms.Label labelHeading;
    private System.Windows.Forms.Label labelSeconds;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
    private System.Windows.Forms.Label labelTimeout;
    private System.Windows.Forms.TextBox textBoxText;
    private System.Windows.Forms.Label labelText;
    private System.Windows.Forms.TextBox textBoxHeading;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
  }
}