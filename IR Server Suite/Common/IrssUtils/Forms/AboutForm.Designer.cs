namespace IrssUtils.Forms
{
  partial class AboutForm
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
      this.forumLinkLabel = new System.Windows.Forms.LinkLabel();
      this.manualLinkLabel = new System.Windows.Forms.LinkLabel();
      this.productNameLabel = new System.Windows.Forms.Label();
      this.productVersionLabel = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.teamLinkLabel = new System.Windows.Forms.LinkLabel();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // forumLinkLabel
      // 
      this.forumLinkLabel.AutoSize = true;
      this.forumLinkLabel.Location = new System.Drawing.Point(18, 43);
      this.forumLinkLabel.Name = "forumLinkLabel";
      this.forumLinkLabel.Size = new System.Drawing.Size(64, 13);
      this.forumLinkLabel.TabIndex = 1;
      this.forumLinkLabel.TabStop = true;
      this.forumLinkLabel.Text = "IRSS Forum";
      this.forumLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.forumLinkLabel_LinkClicked);
      // 
      // manualLinkLabel
      // 
      this.manualLinkLabel.AutoSize = true;
      this.manualLinkLabel.Location = new System.Drawing.Point(18, 23);
      this.manualLinkLabel.Name = "manualLinkLabel";
      this.manualLinkLabel.Size = new System.Drawing.Size(75, 13);
      this.manualLinkLabel.TabIndex = 0;
      this.manualLinkLabel.TabStop = true;
      this.manualLinkLabel.Text = "Online Manual";
      this.manualLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.manualLinkLabel_LinkClicked);
      // 
      // productNameLabel
      // 
      this.productNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.productNameLabel.Location = new System.Drawing.Point(111, 57);
      this.productNameLabel.Name = "productNameLabel";
      this.productNameLabel.Size = new System.Drawing.Size(173, 13);
      this.productNameLabel.TabIndex = 2;
      this.productNameLabel.Text = "ProductName";
      this.productNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // productVersionLabel
      // 
      this.productVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.productVersionLabel.Location = new System.Drawing.Point(111, 75);
      this.productVersionLabel.Name = "productVersionLabel";
      this.productVersionLabel.Size = new System.Drawing.Size(173, 13);
      this.productVersionLabel.TabIndex = 4;
      this.productVersionLabel.Text = "ProductVersion";
      this.productVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(12, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(160, 26);
      this.label3.TabIndex = 0;
      this.label3.Text = "IR Server Suite";
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.teamLinkLabel);
      this.groupBox1.Controls.Add(this.forumLinkLabel);
      this.groupBox1.Controls.Add(this.manualLinkLabel);
      this.groupBox1.Location = new System.Drawing.Point(12, 115);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(272, 89);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Online Ressources";
      // 
      // teamLinkLabel
      // 
      this.teamLinkLabel.AutoSize = true;
      this.teamLinkLabel.Location = new System.Drawing.Point(18, 63);
      this.teamLinkLabel.Name = "teamLinkLabel";
      this.teamLinkLabel.Size = new System.Drawing.Size(93, 13);
      this.teamLinkLabel.TabIndex = 2;
      this.teamLinkLabel.TabStop = true;
      this.teamLinkLabel.Text = "Team MediaPortal";
      this.teamLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.teamLinkLabel_LinkClicked);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(14, 57);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(62, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Application:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(14, 75);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(45, 13);
      this.label5.TabIndex = 3;
      this.label5.Text = "Version:";
      // 
      // okButton
      // 
      this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.okButton.Location = new System.Drawing.Point(111, 212);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // AboutForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.okButton;
      this.ClientSize = new System.Drawing.Size(296, 247);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.productVersionLabel);
      this.Controls.Add(this.productNameLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AboutForm";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.LinkLabel forumLinkLabel;
    private System.Windows.Forms.LinkLabel manualLinkLabel;
    private System.Windows.Forms.Label productNameLabel;
    private System.Windows.Forms.Label productVersionLabel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.LinkLabel teamLinkLabel;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button okButton;
  }
}