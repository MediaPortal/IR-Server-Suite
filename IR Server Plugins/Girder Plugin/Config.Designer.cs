namespace GirderPlugin
{
  partial class Config
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Config));
      this.buttonConfigureGirderPlugin = new System.Windows.Forms.Button();
      this.textBoxPluginFile = new System.Windows.Forms.TextBox();
      this.buttonFind = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.SuspendLayout();
      // 
      // buttonConfigureGirderPlugin
      // 
      this.buttonConfigureGirderPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonConfigureGirderPlugin.Location = new System.Drawing.Point(8, 40);
      this.buttonConfigureGirderPlugin.Name = "buttonConfigureGirderPlugin";
      this.buttonConfigureGirderPlugin.Size = new System.Drawing.Size(72, 24);
      this.buttonConfigureGirderPlugin.TabIndex = 2;
      this.buttonConfigureGirderPlugin.Text = "Configure";
      this.buttonConfigureGirderPlugin.UseVisualStyleBackColor = true;
      this.buttonConfigureGirderPlugin.Click += new System.EventHandler(this.buttonConfigureGirderPlugin_Click);
      // 
      // textBoxPluginFile
      // 
      this.textBoxPluginFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPluginFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxPluginFile.Location = new System.Drawing.Point(8, 8);
      this.textBoxPluginFile.Name = "textBoxPluginFile";
      this.textBoxPluginFile.ReadOnly = true;
      this.textBoxPluginFile.Size = new System.Drawing.Size(312, 20);
      this.textBoxPluginFile.TabIndex = 0;
      // 
      // buttonFind
      // 
      this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonFind.Location = new System.Drawing.Point(328, 8);
      this.buttonFind.Name = "buttonFind";
      this.buttonFind.Size = new System.Drawing.Size(24, 20);
      this.buttonFind.TabIndex = 1;
      this.buttonFind.Text = "...";
      this.buttonFind.UseVisualStyleBackColor = true;
      this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(288, 40);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // openFileDialog
      // 
      this.openFileDialog.Title = "Select Girder plugin to use";
      // 
      // Config
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(360, 72);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonFind);
      this.Controls.Add(this.textBoxPluginFile);
      this.Controls.Add(this.buttonConfigureGirderPlugin);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(368, 99);
      this.Name = "Config";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Girder Plugin Configuration";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonConfigureGirderPlugin;
    private System.Windows.Forms.TextBox textBoxPluginFile;
    private System.Windows.Forms.Button buttonFind;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
  }
}