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
      this.textBoxPluginFolder = new System.Windows.Forms.TextBox();
      this.buttonFind = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxPluginFolder = new System.Windows.Forms.GroupBox();
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.groupBoxPlugins = new System.Windows.Forms.GroupBox();
      this.listViewPlugins = new System.Windows.Forms.ListView();
      this.groupBoxPluginFolder.SuspendLayout();
      this.groupBoxPlugins.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonConfigureGirderPlugin
      // 
      this.buttonConfigureGirderPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonConfigureGirderPlugin.Location = new System.Drawing.Point(8, 240);
      this.buttonConfigureGirderPlugin.Name = "buttonConfigureGirderPlugin";
      this.buttonConfigureGirderPlugin.Size = new System.Drawing.Size(104, 24);
      this.buttonConfigureGirderPlugin.TabIndex = 2;
      this.buttonConfigureGirderPlugin.Text = "Create command";
      this.buttonConfigureGirderPlugin.UseVisualStyleBackColor = true;
      this.buttonConfigureGirderPlugin.Click += new System.EventHandler(this.buttonConfigureGirderPlugin_Click);
      // 
      // textBoxPluginFolder
      // 
      this.textBoxPluginFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPluginFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxPluginFolder.Location = new System.Drawing.Point(8, 24);
      this.textBoxPluginFolder.Name = "textBoxPluginFolder";
      this.textBoxPluginFolder.ReadOnly = true;
      this.textBoxPluginFolder.Size = new System.Drawing.Size(296, 20);
      this.textBoxPluginFolder.TabIndex = 0;
      // 
      // buttonFind
      // 
      this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonFind.Location = new System.Drawing.Point(312, 24);
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
      this.buttonOK.Location = new System.Drawing.Point(288, 240);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // groupBoxPluginFolder
      // 
      this.groupBoxPluginFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPluginFolder.Controls.Add(this.textBoxPluginFolder);
      this.groupBoxPluginFolder.Controls.Add(this.buttonFind);
      this.groupBoxPluginFolder.Location = new System.Drawing.Point(8, 8);
      this.groupBoxPluginFolder.Name = "groupBoxPluginFolder";
      this.groupBoxPluginFolder.Size = new System.Drawing.Size(344, 56);
      this.groupBoxPluginFolder.TabIndex = 4;
      this.groupBoxPluginFolder.TabStop = false;
      this.groupBoxPluginFolder.Text = "Girder plugin folder";
      // 
      // folderBrowserDialog
      // 
      this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.ProgramFiles;
      this.folderBrowserDialog.ShowNewFolderButton = false;
      // 
      // groupBoxPlugins
      // 
      this.groupBoxPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPlugins.Controls.Add(this.listViewPlugins);
      this.groupBoxPlugins.Location = new System.Drawing.Point(8, 72);
      this.groupBoxPlugins.Name = "groupBoxPlugins";
      this.groupBoxPlugins.Size = new System.Drawing.Size(344, 160);
      this.groupBoxPlugins.TabIndex = 5;
      this.groupBoxPlugins.TabStop = false;
      this.groupBoxPlugins.Text = "Girder plugins";
      // 
      // listViewPlugins
      // 
      this.listViewPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewPlugins.CheckBoxes = true;
      this.listViewPlugins.FullRowSelect = true;
      this.listViewPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewPlugins.HideSelection = false;
      this.listViewPlugins.Location = new System.Drawing.Point(8, 16);
      this.listViewPlugins.Name = "listViewPlugins";
      this.listViewPlugins.Size = new System.Drawing.Size(328, 136);
      this.listViewPlugins.TabIndex = 0;
      this.listViewPlugins.UseCompatibleStateImageBehavior = false;
      this.listViewPlugins.View = System.Windows.Forms.View.List;
      // 
      // Config
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(360, 271);
      this.Controls.Add(this.groupBoxPlugins);
      this.Controls.Add(this.groupBoxPluginFolder);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonConfigureGirderPlugin);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(368, 298);
      this.Name = "Config";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Girder Plugin Configuration";
      this.groupBoxPluginFolder.ResumeLayout(false);
      this.groupBoxPluginFolder.PerformLayout();
      this.groupBoxPlugins.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonConfigureGirderPlugin;
    private System.Windows.Forms.TextBox textBoxPluginFolder;
    private System.Windows.Forms.Button buttonFind;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.GroupBox groupBoxPluginFolder;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.GroupBox groupBoxPlugins;
    private System.Windows.Forms.ListView listViewPlugins;
  }
}