namespace IrssUtils.Forms
{
  partial class ShowPopupMessage
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
      this.buttonOK = new System.Windows.Forms.Button();
      this.timerOK = new System.Windows.Forms.Timer(this.components);
      this.labelMessage = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(178, 0);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(72, 24);
      this.buttonOK.TabIndex = 0;
      this.buttonOK.Text = "OK (100)";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // timerOK
      // 
      this.timerOK.Interval = 1000;
      this.timerOK.Tick += new System.EventHandler(this.timerOK_Tick);
      // 
      // labelMessage
      // 
      this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelMessage.Location = new System.Drawing.Point(0, 0);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new System.Drawing.Size(250, 51);
      this.labelMessage.TabIndex = 1;
      this.labelMessage.Text = "Message";
      this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 51);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(250, 24);
      this.panel1.TabIndex = 2;
      // 
      // ShowPopupMessage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(250, 75);
      this.Controls.Add(this.labelMessage);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 100);
      this.Name = "ShowPopupMessage";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Header";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowPopupMessage_FormClosing);
      this.Load += new System.EventHandler(this.ShowPopupMessage_Load);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Timer timerOK;
    private System.Windows.Forms.Label labelMessage;
    private System.Windows.Forms.Panel panel1;
  }
}