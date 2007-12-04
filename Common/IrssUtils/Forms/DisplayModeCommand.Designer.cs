namespace IrssUtils.Forms
{

  partial class DisplayModeCommand
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
      this.textBoxWidth = new System.Windows.Forms.TextBox();
      this.labelWidth = new System.Windows.Forms.Label();
      this.labelHeight = new System.Windows.Forms.Label();
      this.textBoxHeight = new System.Windows.Forms.TextBox();
      this.labelX = new System.Windows.Forms.Label();
      this.textBoxRefresh = new System.Windows.Forms.TextBox();
      this.textBoxBpp = new System.Windows.Forms.TextBox();
      this.checkBoxBpp = new System.Windows.Forms.CheckBox();
      this.checkBoxRefresh = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(120, 112);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 10;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(48, 112);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 9;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // textBoxWidth
      // 
      this.textBoxWidth.Location = new System.Drawing.Point(8, 24);
      this.textBoxWidth.Name = "textBoxWidth";
      this.textBoxWidth.Size = new System.Drawing.Size(80, 20);
      this.textBoxWidth.TabIndex = 1;
      this.textBoxWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelWidth
      // 
      this.labelWidth.Location = new System.Drawing.Point(8, 8);
      this.labelWidth.Name = "labelWidth";
      this.labelWidth.Size = new System.Drawing.Size(80, 16);
      this.labelWidth.TabIndex = 0;
      this.labelWidth.Text = "Width:";
      // 
      // labelHeight
      // 
      this.labelHeight.Location = new System.Drawing.Point(104, 8);
      this.labelHeight.Name = "labelHeight";
      this.labelHeight.Size = new System.Drawing.Size(80, 16);
      this.labelHeight.TabIndex = 3;
      this.labelHeight.Text = "Height:";
      // 
      // textBoxHeight
      // 
      this.textBoxHeight.Location = new System.Drawing.Point(104, 24);
      this.textBoxHeight.Name = "textBoxHeight";
      this.textBoxHeight.Size = new System.Drawing.Size(80, 20);
      this.textBoxHeight.TabIndex = 4;
      this.textBoxHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelX
      // 
      this.labelX.Location = new System.Drawing.Point(88, 24);
      this.labelX.Name = "labelX";
      this.labelX.Size = new System.Drawing.Size(16, 24);
      this.labelX.TabIndex = 2;
      this.labelX.Text = "x";
      this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // textBoxRefresh
      // 
      this.textBoxRefresh.Location = new System.Drawing.Point(104, 80);
      this.textBoxRefresh.Name = "textBoxRefresh";
      this.textBoxRefresh.Size = new System.Drawing.Size(80, 20);
      this.textBoxRefresh.TabIndex = 8;
      this.textBoxRefresh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // textBoxBpp
      // 
      this.textBoxBpp.Location = new System.Drawing.Point(8, 80);
      this.textBoxBpp.Name = "textBoxBpp";
      this.textBoxBpp.Size = new System.Drawing.Size(80, 20);
      this.textBoxBpp.TabIndex = 6;
      this.textBoxBpp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // checkBoxBpp
      // 
      this.checkBoxBpp.Location = new System.Drawing.Point(8, 64);
      this.checkBoxBpp.Name = "checkBoxBpp";
      this.checkBoxBpp.Size = new System.Drawing.Size(80, 16);
      this.checkBoxBpp.TabIndex = 5;
      this.checkBoxBpp.Text = "Bit depth";
      this.checkBoxBpp.UseVisualStyleBackColor = true;
      // 
      // checkBoxRefresh
      // 
      this.checkBoxRefresh.Location = new System.Drawing.Point(104, 64);
      this.checkBoxRefresh.Name = "checkBoxRefresh";
      this.checkBoxRefresh.Size = new System.Drawing.Size(80, 16);
      this.checkBoxRefresh.TabIndex = 7;
      this.checkBoxRefresh.Text = "Refresh Hz";
      this.checkBoxRefresh.UseVisualStyleBackColor = true;
      // 
      // DisplayModeCommand
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(192, 145);
      this.Controls.Add(this.checkBoxRefresh);
      this.Controls.Add(this.checkBoxBpp);
      this.Controls.Add(this.textBoxRefresh);
      this.Controls.Add(this.textBoxBpp);
      this.Controls.Add(this.labelX);
      this.Controls.Add(this.labelHeight);
      this.Controls.Add(this.textBoxHeight);
      this.Controls.Add(this.labelWidth);
      this.Controls.Add(this.textBoxWidth);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(160, 116);
      this.Name = "DisplayModeCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Display Mode Command";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TextBox textBoxWidth;
    private System.Windows.Forms.Label labelWidth;
    private System.Windows.Forms.Label labelHeight;
    private System.Windows.Forms.TextBox textBoxHeight;
    private System.Windows.Forms.Label labelX;
    private System.Windows.Forms.TextBox textBoxRefresh;
    private System.Windows.Forms.TextBox textBoxBpp;
    private System.Windows.Forms.CheckBox checkBoxBpp;
    private System.Windows.Forms.CheckBox checkBoxRefresh;
  }

}
