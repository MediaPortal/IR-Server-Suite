namespace MPUtils.Forms
{
  partial class MPAction
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
      this.comboBoxActionType = new System.Windows.Forms.ComboBox();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.labelActionType = new System.Windows.Forms.Label();
      this.labelFloat1 = new System.Windows.Forms.Label();
      this.labelFloat2 = new System.Windows.Forms.Label();
      this.textBoxFloat1 = new System.Windows.Forms.TextBox();
      this.textBoxFloat2 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // comboBoxActionType
      // 
      this.comboBoxActionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxActionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxActionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxActionType.FormattingEnabled = true;
      this.comboBoxActionType.Location = new System.Drawing.Point(88, 8);
      this.comboBoxActionType.Name = "comboBoxActionType";
      this.comboBoxActionType.Size = new System.Drawing.Size(168, 21);
      this.comboBoxActionType.TabIndex = 1;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(120, 104);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(192, 104);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelActionType
      // 
      this.labelActionType.Location = new System.Drawing.Point(8, 8);
      this.labelActionType.Name = "labelActionType";
      this.labelActionType.Size = new System.Drawing.Size(80, 21);
      this.labelActionType.TabIndex = 0;
      this.labelActionType.Text = "Action type:";
      this.labelActionType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelFloat1
      // 
      this.labelFloat1.Location = new System.Drawing.Point(8, 40);
      this.labelFloat1.Name = "labelFloat1";
      this.labelFloat1.Size = new System.Drawing.Size(80, 20);
      this.labelFloat1.TabIndex = 4;
      this.labelFloat1.Text = "float 1:";
      this.labelFloat1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelFloat2
      // 
      this.labelFloat2.Location = new System.Drawing.Point(8, 72);
      this.labelFloat2.Name = "labelFloat2";
      this.labelFloat2.Size = new System.Drawing.Size(80, 20);
      this.labelFloat2.TabIndex = 5;
      this.labelFloat2.Text = "float 2:";
      this.labelFloat2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxFloat1
      // 
      this.textBoxFloat1.Location = new System.Drawing.Point(88, 40);
      this.textBoxFloat1.Name = "textBoxFloat1";
      this.textBoxFloat1.Size = new System.Drawing.Size(168, 20);
      this.textBoxFloat1.TabIndex = 6;
      // 
      // textBoxFloat2
      // 
      this.textBoxFloat2.Location = new System.Drawing.Point(88, 72);
      this.textBoxFloat2.Name = "textBoxFloat2";
      this.textBoxFloat2.Size = new System.Drawing.Size(168, 20);
      this.textBoxFloat2.TabIndex = 7;
      // 
      // MPAction
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(264, 137);
      this.Controls.Add(this.textBoxFloat2);
      this.Controls.Add(this.textBoxFloat1);
      this.Controls.Add(this.labelFloat2);
      this.Controls.Add(this.labelFloat1);
      this.Controls.Add(this.labelActionType);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.comboBoxActionType);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(272, 164);
      this.Name = "MPAction";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Send MediaPortal Action";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxActionType;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelActionType;
    private System.Windows.Forms.Label labelFloat1;
    private System.Windows.Forms.Label labelFloat2;
    private System.Windows.Forms.TextBox textBoxFloat1;
    private System.Windows.Forms.TextBox textBoxFloat2;
  }
}