namespace TrayLauncher
{
  partial class GetKeyCodeForm
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
      this.labelStatus = new System.Windows.Forms.Label();
      this.timer = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // labelStatus
      // 
      this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelStatus.Location = new System.Drawing.Point(0, 0);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new System.Drawing.Size(288, 57);
      this.labelStatus.TabIndex = 0;
      this.labelStatus.Text = "Status";
      this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // timer
      // 
      this.timer.Interval = 5000;
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      // 
      // GetKeyCodeForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(288, 57);
      this.ControlBox = false;
      this.Controls.Add(this.labelStatus);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GetKeyCodeForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "New Remote Button";
      this.Load += new System.EventHandler(this.GetKeyCodeForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.Timer timer;
  }
}