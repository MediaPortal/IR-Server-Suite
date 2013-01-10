namespace IrssCommands
{

  partial class EditIf
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
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.textBoxParam1 = new System.Windows.Forms.TextBox();
      this.textBoxParam2 = new System.Windows.Forms.TextBox();
      this.comboBoxComparer = new System.Windows.Forms.ComboBox();
      this.textBoxLabel1 = new System.Windows.Forms.TextBox();
      this.textBoxLabel2 = new System.Windows.Forms.TextBox();
      this.labelIf = new System.Windows.Forms.Label();
      this.labelThenGoto = new System.Windows.Forms.Label();
      this.labelElseGoto = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(56, 176);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 8;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(128, 176);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 9;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxParam1
      // 
      this.textBoxParam1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxParam1.Location = new System.Drawing.Point(32, 8);
      this.textBoxParam1.Name = "textBoxParam1";
      this.textBoxParam1.Size = new System.Drawing.Size(160, 20);
      this.textBoxParam1.TabIndex = 1;
      this.toolTips.SetToolTip(this.textBoxParam1, "Enter the first variable/value to compare here");
      // 
      // textBoxParam2
      // 
      this.textBoxParam2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxParam2.Location = new System.Drawing.Point(8, 72);
      this.textBoxParam2.Name = "textBoxParam2";
      this.textBoxParam2.Size = new System.Drawing.Size(184, 20);
      this.textBoxParam2.TabIndex = 3;
      this.toolTips.SetToolTip(this.textBoxParam2, "Enter the second variable/value to compare here");
      // 
      // comboBoxComparer
      // 
      this.comboBoxComparer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComparer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxComparer.FormattingEnabled = true;
      this.comboBoxComparer.Location = new System.Drawing.Point(8, 40);
      this.comboBoxComparer.Name = "comboBoxComparer";
      this.comboBoxComparer.Size = new System.Drawing.Size(184, 21);
      this.comboBoxComparer.TabIndex = 2;
      this.toolTips.SetToolTip(this.comboBoxComparer, "The method of comparing the two parameters");
      // 
      // textBoxLabel1
      // 
      this.textBoxLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxLabel1.Location = new System.Drawing.Point(72, 104);
      this.textBoxLabel1.Name = "textBoxLabel1";
      this.textBoxLabel1.Size = new System.Drawing.Size(120, 20);
      this.textBoxLabel1.TabIndex = 5;
      this.toolTips.SetToolTip(this.textBoxLabel1, "If this statement evaluates true then goto this label");
      // 
      // textBoxLabel2
      // 
      this.textBoxLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxLabel2.Location = new System.Drawing.Point(72, 136);
      this.textBoxLabel2.Name = "textBoxLabel2";
      this.textBoxLabel2.Size = new System.Drawing.Size(120, 20);
      this.textBoxLabel2.TabIndex = 7;
      this.toolTips.SetToolTip(this.textBoxLabel2, "If this statement evaluates false then goto this label (optional)");
      // 
      // labelIf
      // 
      this.labelIf.Location = new System.Drawing.Point(8, 8);
      this.labelIf.Name = "labelIf";
      this.labelIf.Size = new System.Drawing.Size(24, 20);
      this.labelIf.TabIndex = 0;
      this.labelIf.Text = "if";
      this.labelIf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelThenGoto
      // 
      this.labelThenGoto.Location = new System.Drawing.Point(8, 104);
      this.labelThenGoto.Name = "labelThenGoto";
      this.labelThenGoto.Size = new System.Drawing.Size(64, 20);
      this.labelThenGoto.TabIndex = 4;
      this.labelThenGoto.Text = "then goto";
      this.labelThenGoto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelElseGoto
      // 
      this.labelElseGoto.Location = new System.Drawing.Point(8, 136);
      this.labelElseGoto.Name = "labelElseGoto";
      this.labelElseGoto.Size = new System.Drawing.Size(64, 20);
      this.labelElseGoto.TabIndex = 6;
      this.labelElseGoto.Text = "else goto";
      this.labelElseGoto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // EditIf
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(200, 209);
      this.Controls.Add(this.textBoxLabel2);
      this.Controls.Add(this.labelElseGoto);
      this.Controls.Add(this.textBoxLabel1);
      this.Controls.Add(this.comboBoxComparer);
      this.Controls.Add(this.textBoxParam2);
      this.Controls.Add(this.textBoxParam1);
      this.Controls.Add(this.labelThenGoto);
      this.Controls.Add(this.labelIf);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(208, 236);
      this.Name = "EditIf";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "If Statement";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelIf;
    private System.Windows.Forms.Label labelThenGoto;
    private System.Windows.Forms.TextBox textBoxParam1;
    private System.Windows.Forms.TextBox textBoxParam2;
    private System.Windows.Forms.ComboBox comboBoxComparer;
    private System.Windows.Forms.TextBox textBoxLabel1;
    private System.Windows.Forms.Label labelElseGoto;
    private System.Windows.Forms.TextBox textBoxLabel2;
  }

}
