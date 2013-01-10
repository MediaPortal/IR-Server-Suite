namespace Commands
{
  partial class EditSwapVariables
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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.labelVariable1 = new System.Windows.Forms.Label();
      this.labelVariable2 = new System.Windows.Forms.Label();
      this.textBoxVariable1 = new System.Windows.Forms.TextBox();
      this.textBoxVariable2 = new System.Windows.Forms.TextBox();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.labelVarPrefix1 = new System.Windows.Forms.Label();
      this.labelVarPrefix2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(112, 72);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 6;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(184, 72);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 7;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelVariable1
      // 
      this.labelVariable1.Location = new System.Drawing.Point(8, 8);
      this.labelVariable1.Name = "labelVariable1";
      this.labelVariable1.Size = new System.Drawing.Size(64, 20);
      this.labelVariable1.TabIndex = 0;
      this.labelVariable1.Text = "Variable 1:";
      this.labelVariable1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVariable2
      // 
      this.labelVariable2.Location = new System.Drawing.Point(8, 40);
      this.labelVariable2.Name = "labelVariable2";
      this.labelVariable2.Size = new System.Drawing.Size(64, 20);
      this.labelVariable2.TabIndex = 3;
      this.labelVariable2.Text = "Variable 2:";
      this.labelVariable2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxVariable1
      // 
      this.textBoxVariable1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxVariable1.Location = new System.Drawing.Point(104, 8);
      this.textBoxVariable1.Name = "textBoxVariable1";
      this.textBoxVariable1.Size = new System.Drawing.Size(144, 20);
      this.textBoxVariable1.TabIndex = 2;
      this.toolTips.SetToolTip(this.textBoxVariable1, "The first variable to swap with the second");
      // 
      // textBoxVariable2
      // 
      this.textBoxVariable2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxVariable2.Location = new System.Drawing.Point(104, 40);
      this.textBoxVariable2.Name = "textBoxVariable2";
      this.textBoxVariable2.Size = new System.Drawing.Size(144, 20);
      this.textBoxVariable2.TabIndex = 5;
      this.toolTips.SetToolTip(this.textBoxVariable2, "The second variable to swap with the first");
      // 
      // labelVarPrefix1
      // 
      this.labelVarPrefix1.Location = new System.Drawing.Point(72, 8);
      this.labelVarPrefix1.Name = "labelVarPrefix1";
      this.labelVarPrefix1.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix1.TabIndex = 1;
      this.labelVarPrefix1.Text = "var_";
      this.labelVarPrefix1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelVarPrefix2
      // 
      this.labelVarPrefix2.Location = new System.Drawing.Point(72, 40);
      this.labelVarPrefix2.Name = "labelVarPrefix2";
      this.labelVarPrefix2.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix2.TabIndex = 4;
      this.labelVarPrefix2.Text = "var_";
      this.labelVarPrefix2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // EditSwapVariables
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(256, 105);
      this.Controls.Add(this.labelVarPrefix2);
      this.Controls.Add(this.labelVarPrefix1);
      this.Controls.Add(this.textBoxVariable2);
      this.Controls.Add(this.textBoxVariable1);
      this.Controls.Add(this.labelVariable2);
      this.Controls.Add(this.labelVariable1);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(264, 132);
      this.Name = "EditSwapVariables";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Swap Variables Command";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelVariable1;
    private System.Windows.Forms.Label labelVariable2;
    private System.Windows.Forms.TextBox textBoxVariable1;
    private System.Windows.Forms.TextBox textBoxVariable2;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelVarPrefix1;
    private System.Windows.Forms.Label labelVarPrefix2;
  }
}