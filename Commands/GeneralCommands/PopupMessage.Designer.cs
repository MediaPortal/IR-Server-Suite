namespace Commands.General
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
      this.components = new System.ComponentModel.Container();
      this.buttonOK = new System.Windows.Forms.Button();
      this.timerOK = new System.Windows.Forms.Timer(this.components);
      this.panel1 = new System.Windows.Forms.Panel();
      this.textBoxMessage = new System.Windows.Forms.TextBox();
      this.labelDiv = new System.Windows.Forms.Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(88, 8);
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
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 35);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(250, 40);
      this.panel1.TabIndex = 2;
      // 
      // textBoxMessage
      // 
      this.textBoxMessage.BackColor = System.Drawing.SystemColors.Control;
      this.textBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxMessage.Location = new System.Drawing.Point(0, 16);
      this.textBoxMessage.Multiline = true;
      this.textBoxMessage.Name = "textBoxMessage";
      this.textBoxMessage.Size = new System.Drawing.Size(250, 19);
      this.textBoxMessage.TabIndex = 3;
      this.textBoxMessage.Text = "Message";
      this.textBoxMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // labelDiv
      // 
      this.labelDiv.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelDiv.Location = new System.Drawing.Point(0, 0);
      this.labelDiv.Name = "labelDiv";
      this.labelDiv.Size = new System.Drawing.Size(250, 16);
      this.labelDiv.TabIndex = 4;
      this.labelDiv.Text = " ";
      this.labelDiv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // ShowPopupMessage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(250, 75);
      this.Controls.Add(this.textBoxMessage);
      this.Controls.Add(this.labelDiv);
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
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Timer timerOK;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TextBox textBoxMessage;
    private System.Windows.Forms.Label labelDiv;
  }
}