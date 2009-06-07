namespace IRServer
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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.treeViewExclusions = new System.Windows.Forms.TreeView();
      this.labelExplain = new System.Windows.Forms.Label();
      this.labelExpandAll = new System.Windows.Forms.Label();
      this.labelCollapseAll = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(240, 312);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(168, 312);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // treeViewExclusions
      // 
      this.treeViewExclusions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.treeViewExclusions.CheckBoxes = true;
      this.treeViewExclusions.Location = new System.Drawing.Point(8, 72);
      this.treeViewExclusions.Name = "treeViewExclusions";
      this.treeViewExclusions.ShowNodeToolTips = true;
      this.treeViewExclusions.Size = new System.Drawing.Size(296, 232);
      this.treeViewExclusions.TabIndex = 1;
      // 
      // labelExplain
      // 
      this.labelExplain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelExplain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.labelExplain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelExplain.Location = new System.Drawing.Point(8, 8);
      this.labelExplain.Name = "labelExplain";
      this.labelExplain.Size = new System.Drawing.Size(296, 56);
      this.labelExplain.TabIndex = 0;
      this.labelExplain.Text = "Put a check next to any device or individual remote you would like to exclude fro" +
          "m being processed by the Abstract Remote Model";
      this.labelExplain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelExpandAll
      // 
      this.labelExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelExpandAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelExpandAll.Location = new System.Drawing.Point(8, 304);
      this.labelExpandAll.Name = "labelExpandAll";
      this.labelExpandAll.Size = new System.Drawing.Size(16, 16);
      this.labelExpandAll.TabIndex = 2;
      this.labelExpandAll.Text = "+";
      this.labelExpandAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.labelExpandAll.Click += new System.EventHandler(this.labelExpandAll_Click);
      // 
      // labelCollapseAll
      // 
      this.labelCollapseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelCollapseAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelCollapseAll.Location = new System.Drawing.Point(24, 304);
      this.labelCollapseAll.Name = "labelCollapseAll";
      this.labelCollapseAll.Size = new System.Drawing.Size(16, 16);
      this.labelCollapseAll.TabIndex = 3;
      this.labelCollapseAll.Text = "-";
      this.labelCollapseAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.labelCollapseAll.Click += new System.EventHandler(this.labelCollapseAll_Click);
      // 
      // Exclusions
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(312, 346);
      this.Controls.Add(this.labelExplain);
      this.Controls.Add(this.treeViewExclusions);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.labelExpandAll);
      this.Controls.Add(this.labelCollapseAll);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(320, 380);
      this.Name = "Exclusions";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Abstrct Remote Model Exclusions";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.TreeView treeViewExclusions;
    private System.Windows.Forms.Label labelExplain;
    private System.Windows.Forms.Label labelExpandAll;
    private System.Windows.Forms.Label labelCollapseAll;

  }
}