namespace IrssCommands
{
  sealed partial class CommandConfigForm
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
      this.panel = new System.Windows.Forms.Panel();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.buttonTest = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(278, 170);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(208, 170);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // panel
      // 
      this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel.Location = new System.Drawing.Point(12, 12);
      this.panel.MinimumSize = new System.Drawing.Size(230, 72);
      this.panel.Name = "panel";
      this.panel.Size = new System.Drawing.Size(330, 152);
      this.panel.TabIndex = 4;
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(74, 170);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(58, 24);
      this.buttonHelp.TabIndex = 7;
      this.buttonHelp.Text = "&Help";
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(12, 170);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(56, 24);
      this.buttonTest.TabIndex = 6;
      this.buttonTest.Text = "&Test";
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // CommandConfigForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(354, 206);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.panel);
      this.Controls.Add(this.buttonTest);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(370, 244);
      this.Name = "CommandConfigForm";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "CommandConfigForm";
      this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.CommandConfigForm_HelpRequested);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Panel panel;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.Button buttonTest;
  }

}
