namespace IrssCommands.MediaPortal
{
  partial class SendActionConfig
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
      this.textBoxFloat2 = new System.Windows.Forms.TextBox();
      this.textBoxFloat1 = new System.Windows.Forms.TextBox();
      this.labelFloat2 = new System.Windows.Forms.Label();
      this.labelFloat1 = new System.Windows.Forms.Label();
      this.labelActionType = new System.Windows.Forms.Label();
      this.comboBoxActionType = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // textBoxFloat2
      // 
      this.textBoxFloat2.Location = new System.Drawing.Point(84, 67);
      this.textBoxFloat2.Name = "textBoxFloat2";
      this.textBoxFloat2.Size = new System.Drawing.Size(230, 20);
      this.textBoxFloat2.TabIndex = 15;
      // 
      // textBoxFloat1
      // 
      this.textBoxFloat1.Location = new System.Drawing.Point(84, 35);
      this.textBoxFloat1.Name = "textBoxFloat1";
      this.textBoxFloat1.Size = new System.Drawing.Size(230, 20);
      this.textBoxFloat1.TabIndex = 14;
      // 
      // labelFloat2
      // 
      this.labelFloat2.Location = new System.Drawing.Point(4, 67);
      this.labelFloat2.Name = "labelFloat2";
      this.labelFloat2.Size = new System.Drawing.Size(80, 20);
      this.labelFloat2.TabIndex = 13;
      this.labelFloat2.Text = "float 2:";
      this.labelFloat2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelFloat1
      // 
      this.labelFloat1.Location = new System.Drawing.Point(4, 35);
      this.labelFloat1.Name = "labelFloat1";
      this.labelFloat1.Size = new System.Drawing.Size(80, 20);
      this.labelFloat1.TabIndex = 12;
      this.labelFloat1.Text = "float 1:";
      this.labelFloat1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelActionType
      // 
      this.labelActionType.Location = new System.Drawing.Point(4, 3);
      this.labelActionType.Name = "labelActionType";
      this.labelActionType.Size = new System.Drawing.Size(80, 21);
      this.labelActionType.TabIndex = 8;
      this.labelActionType.Text = "Action type:";
      this.labelActionType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // comboBoxActionType
      // 
      this.comboBoxActionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxActionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboBoxActionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxActionType.FormattingEnabled = true;
      this.comboBoxActionType.Location = new System.Drawing.Point(84, 3);
      this.comboBoxActionType.Name = "comboBoxActionType";
      this.comboBoxActionType.Size = new System.Drawing.Size(230, 21);
      this.comboBoxActionType.TabIndex = 9;
      // 
      // SendActionConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.textBoxFloat2);
      this.Controls.Add(this.textBoxFloat1);
      this.Controls.Add(this.labelFloat2);
      this.Controls.Add(this.labelFloat1);
      this.Controls.Add(this.labelActionType);
      this.Controls.Add(this.comboBoxActionType);
      this.Name = "SendActionConfig";
      this.Size = new System.Drawing.Size(317, 93);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxFloat2;
    private System.Windows.Forms.TextBox textBoxFloat1;
    private System.Windows.Forms.Label labelFloat2;
    private System.Windows.Forms.Label labelFloat1;
    private System.Windows.Forms.Label labelActionType;
    private System.Windows.Forms.ComboBox comboBoxActionType;

  }
}
