namespace MPUtils.Forms
{
  partial class MPMessage
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
      this.comboBoxMessageType = new System.Windows.Forms.ComboBox();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.labelMessageType = new System.Windows.Forms.Label();
      this.labelWindowId = new System.Windows.Forms.Label();
      this.labelSenderId = new System.Windows.Forms.Label();
      this.textBoxWindowId = new System.Windows.Forms.TextBox();
      this.textBoxSenderId = new System.Windows.Forms.TextBox();
      this.textBoxParam1 = new System.Windows.Forms.TextBox();
      this.textBoxControlId = new System.Windows.Forms.TextBox();
      this.labelParam1 = new System.Windows.Forms.Label();
      this.labelControlId = new System.Windows.Forms.Label();
      this.textBoxParam2 = new System.Windows.Forms.TextBox();
      this.labelParam2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // comboBoxMessageType
      // 
      this.comboBoxMessageType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxMessageType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxMessageType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxMessageType.FormattingEnabled = true;
      this.comboBoxMessageType.Location = new System.Drawing.Point(96, 8);
      this.comboBoxMessageType.Name = "comboBoxMessageType";
      this.comboBoxMessageType.Size = new System.Drawing.Size(160, 21);
      this.comboBoxMessageType.TabIndex = 1;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(120, 200);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 12;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(192, 200);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 13;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelMessageType
      // 
      this.labelMessageType.Location = new System.Drawing.Point(8, 8);
      this.labelMessageType.Name = "labelMessageType";
      this.labelMessageType.Size = new System.Drawing.Size(88, 21);
      this.labelMessageType.TabIndex = 0;
      this.labelMessageType.Text = "Message type:";
      this.labelMessageType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelWindowId
      // 
      this.labelWindowId.Location = new System.Drawing.Point(8, 40);
      this.labelWindowId.Name = "labelWindowId";
      this.labelWindowId.Size = new System.Drawing.Size(88, 20);
      this.labelWindowId.TabIndex = 2;
      this.labelWindowId.Text = "Window ID:";
      this.labelWindowId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelSenderId
      // 
      this.labelSenderId.Location = new System.Drawing.Point(8, 72);
      this.labelSenderId.Name = "labelSenderId";
      this.labelSenderId.Size = new System.Drawing.Size(88, 20);
      this.labelSenderId.TabIndex = 4;
      this.labelSenderId.Text = "Sender ID:";
      this.labelSenderId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxWindowId
      // 
      this.textBoxWindowId.Location = new System.Drawing.Point(96, 40);
      this.textBoxWindowId.Name = "textBoxWindowId";
      this.textBoxWindowId.Size = new System.Drawing.Size(160, 20);
      this.textBoxWindowId.TabIndex = 3;
      // 
      // textBoxSenderId
      // 
      this.textBoxSenderId.Location = new System.Drawing.Point(96, 72);
      this.textBoxSenderId.Name = "textBoxSenderId";
      this.textBoxSenderId.Size = new System.Drawing.Size(160, 20);
      this.textBoxSenderId.TabIndex = 5;
      // 
      // textBoxParam1
      // 
      this.textBoxParam1.Location = new System.Drawing.Point(96, 136);
      this.textBoxParam1.Name = "textBoxParam1";
      this.textBoxParam1.Size = new System.Drawing.Size(160, 20);
      this.textBoxParam1.TabIndex = 9;
      // 
      // textBoxControlId
      // 
      this.textBoxControlId.Location = new System.Drawing.Point(96, 104);
      this.textBoxControlId.Name = "textBoxControlId";
      this.textBoxControlId.Size = new System.Drawing.Size(160, 20);
      this.textBoxControlId.TabIndex = 7;
      // 
      // labelParam1
      // 
      this.labelParam1.Location = new System.Drawing.Point(8, 136);
      this.labelParam1.Name = "labelParam1";
      this.labelParam1.Size = new System.Drawing.Size(88, 20);
      this.labelParam1.TabIndex = 8;
      this.labelParam1.Text = "Param 1:";
      this.labelParam1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelControlId
      // 
      this.labelControlId.Location = new System.Drawing.Point(8, 104);
      this.labelControlId.Name = "labelControlId";
      this.labelControlId.Size = new System.Drawing.Size(88, 20);
      this.labelControlId.TabIndex = 6;
      this.labelControlId.Text = "Control ID:";
      this.labelControlId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxParam2
      // 
      this.textBoxParam2.Location = new System.Drawing.Point(96, 168);
      this.textBoxParam2.Name = "textBoxParam2";
      this.textBoxParam2.Size = new System.Drawing.Size(160, 20);
      this.textBoxParam2.TabIndex = 11;
      // 
      // labelParam2
      // 
      this.labelParam2.Location = new System.Drawing.Point(8, 168);
      this.labelParam2.Name = "labelParam2";
      this.labelParam2.Size = new System.Drawing.Size(88, 20);
      this.labelParam2.TabIndex = 10;
      this.labelParam2.Text = "Param 2:";
      this.labelParam2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // MPMessage
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(264, 233);
      this.Controls.Add(this.textBoxParam2);
      this.Controls.Add(this.labelParam2);
      this.Controls.Add(this.textBoxParam1);
      this.Controls.Add(this.textBoxControlId);
      this.Controls.Add(this.labelParam1);
      this.Controls.Add(this.labelControlId);
      this.Controls.Add(this.textBoxSenderId);
      this.Controls.Add(this.textBoxWindowId);
      this.Controls.Add(this.labelSenderId);
      this.Controls.Add(this.labelWindowId);
      this.Controls.Add(this.labelMessageType);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.comboBoxMessageType);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(272, 260);
      this.Name = "MPMessage";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Send MediaPortal Message";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxMessageType;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelMessageType;
    private System.Windows.Forms.Label labelWindowId;
    private System.Windows.Forms.Label labelSenderId;
    private System.Windows.Forms.TextBox textBoxWindowId;
    private System.Windows.Forms.TextBox textBoxSenderId;
    private System.Windows.Forms.TextBox textBoxParam1;
    private System.Windows.Forms.TextBox textBoxControlId;
    private System.Windows.Forms.Label labelParam1;
    private System.Windows.Forms.Label labelControlId;
    private System.Windows.Forms.TextBox textBoxParam2;
    private System.Windows.Forms.Label labelParam2;
  }
}