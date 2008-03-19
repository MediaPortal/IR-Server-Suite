namespace InputService.Configuration
{
  partial class Exclusions
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Exclusions));
      this.listViewExclusions = new System.Windows.Forms.ListView();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.columnHeaderPlugin = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // listViewExclusions
      // 
      this.listViewExclusions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewExclusions.CheckBoxes = true;
      this.listViewExclusions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPlugin});
      this.listViewExclusions.FullRowSelect = true;
      this.listViewExclusions.GridLines = true;
      this.listViewExclusions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewExclusions.HideSelection = false;
      this.listViewExclusions.Location = new System.Drawing.Point(8, 8);
      this.listViewExclusions.MultiSelect = false;
      this.listViewExclusions.Name = "listViewExclusions";
      this.listViewExclusions.ShowGroups = false;
      this.listViewExclusions.Size = new System.Drawing.Size(304, 216);
      this.listViewExclusions.TabIndex = 0;
      this.listViewExclusions.UseCompatibleStateImageBehavior = false;
      this.listViewExclusions.View = System.Windows.Forms.View.Details;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(248, 232);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(176, 232);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // columnHeaderPlugin
      // 
      this.columnHeaderPlugin.Text = "Plugin";
      this.columnHeaderPlugin.Width = 284;
      // 
      // Exclusions
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(320, 266);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.listViewExclusions);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.Name = "Exclusions";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Abstrct Remote Model Exclusions";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listViewExclusions;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.ColumnHeader columnHeaderPlugin;

  }
}