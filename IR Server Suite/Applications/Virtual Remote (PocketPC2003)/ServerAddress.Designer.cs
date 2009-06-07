namespace VirtualRemote
{
  partial class ServerAddress
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
      this.buttonOK = new System.Windows.Forms.Button();
      this.textBoxComputer = new System.Windows.Forms.TextBox();
      this.labelText = new System.Windows.Forms.Label();
      this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(168, 64);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // textBoxComputer
      // 
      this.textBoxComputer.Location = new System.Drawing.Point(8, 32);
      this.textBoxComputer.Name = "textBoxComputer";
      this.textBoxComputer.Size = new System.Drawing.Size(224, 21);
      this.textBoxComputer.TabIndex = 2;
      // 
      // labelText
      // 
      this.labelText.Location = new System.Drawing.Point(8, 8);
      this.labelText.Name = "labelText";
      this.labelText.Size = new System.Drawing.Size(224, 20);
      this.labelText.Text = "Select IR Server host computer";
      // 
      // ServerAddress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(240, 100);
      this.ControlBox = false;
      this.Controls.Add(this.labelText);
      this.Controls.Add(this.textBoxComputer);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ServerAddress";
      this.Text = "Select Server";
      this.TopMost = true;
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TextBox textBoxComputer;
    private System.Windows.Forms.Label labelText;
    private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
  }
}