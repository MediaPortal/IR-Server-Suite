namespace IrssUtils.Forms
{

  partial class KeysCommand
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
      this.keystrokeCommandPanel = new IrssUtils.Panels.KeystrokeCommandPanel();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(264, 128);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(192, 128);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // keystrokeCommandPanel
      // 
      this.keystrokeCommandPanel.CommandString = "";
      this.keystrokeCommandPanel.Location = new System.Drawing.Point(12, 12);
      this.keystrokeCommandPanel.Name = "keystrokeCommandPanel";
      this.keystrokeCommandPanel.Size = new System.Drawing.Size(312, 110);
      this.keystrokeCommandPanel.TabIndex = 4;
      // 
      // KeysCommand
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(336, 167);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.keystrokeCommandPanel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(344, 194);
      this.Name = "KeysCommand";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Keystroke Command";
      this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private IrssUtils.Panels.KeystrokeCommandPanel keystrokeCommandPanel;
  }

}
