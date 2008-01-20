namespace Commands
{

  partial class EditMathsOperation
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
      this.textBoxInput1 = new System.Windows.Forms.TextBox();
      this.comboBoxOperation = new System.Windows.Forms.ComboBox();
      this.textBoxInput2 = new System.Windows.Forms.TextBox();
      this.labelOperation = new System.Windows.Forms.Label();
      this.labelInput1 = new System.Windows.Forms.Label();
      this.labelInput2 = new System.Windows.Forms.Label();
      this.labelVarPrefix = new System.Windows.Forms.Label();
      this.labelOutput = new System.Windows.Forms.Label();
      this.textBoxOutputVar = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(248, 136);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 9;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(320, 136);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 10;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxInput1
      // 
      this.textBoxInput1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxInput1.Location = new System.Drawing.Point(80, 40);
      this.textBoxInput1.Name = "textBoxInput1";
      this.textBoxInput1.Size = new System.Drawing.Size(304, 20);
      this.textBoxInput1.TabIndex = 3;
      this.toolTips.SetToolTip(this.textBoxInput1, "The first operation input");
      // 
      // comboBoxOperation
      // 
      this.comboBoxOperation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxOperation.FormattingEnabled = true;
      this.comboBoxOperation.Location = new System.Drawing.Point(80, 8);
      this.comboBoxOperation.Name = "comboBoxOperation";
      this.comboBoxOperation.Size = new System.Drawing.Size(304, 21);
      this.comboBoxOperation.TabIndex = 1;
      this.toolTips.SetToolTip(this.comboBoxOperation, "The string operation to perform");
      // 
      // textBoxInput2
      // 
      this.textBoxInput2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxInput2.Location = new System.Drawing.Point(80, 72);
      this.textBoxInput2.Name = "textBoxInput2";
      this.textBoxInput2.Size = new System.Drawing.Size(304, 20);
      this.textBoxInput2.TabIndex = 5;
      this.toolTips.SetToolTip(this.textBoxInput2, "The second operation input (sometimes optional)");
      // 
      // labelOperation
      // 
      this.labelOperation.Location = new System.Drawing.Point(8, 8);
      this.labelOperation.Name = "labelOperation";
      this.labelOperation.Size = new System.Drawing.Size(72, 21);
      this.labelOperation.TabIndex = 0;
      this.labelOperation.Text = "Operation:";
      this.labelOperation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelInput1
      // 
      this.labelInput1.Location = new System.Drawing.Point(8, 40);
      this.labelInput1.Name = "labelInput1";
      this.labelInput1.Size = new System.Drawing.Size(72, 21);
      this.labelInput1.TabIndex = 2;
      this.labelInput1.Text = "Input 1:";
      this.labelInput1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTips.SetToolTip(this.labelInput1, "The first input string");
      // 
      // labelInput2
      // 
      this.labelInput2.Location = new System.Drawing.Point(8, 72);
      this.labelInput2.Name = "labelInput2";
      this.labelInput2.Size = new System.Drawing.Size(72, 21);
      this.labelInput2.TabIndex = 4;
      this.labelInput2.Text = "Input 2:";
      this.labelInput2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTips.SetToolTip(this.labelInput2, "The second input string");
      // 
      // labelVarPrefix
      // 
      this.labelVarPrefix.Location = new System.Drawing.Point(80, 104);
      this.labelVarPrefix.Name = "labelVarPrefix";
      this.labelVarPrefix.Size = new System.Drawing.Size(32, 21);
      this.labelVarPrefix.TabIndex = 7;
      this.labelVarPrefix.Text = "var_";
      this.labelVarPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelOutput
      // 
      this.labelOutput.Location = new System.Drawing.Point(8, 104);
      this.labelOutput.Name = "labelOutput";
      this.labelOutput.Size = new System.Drawing.Size(72, 21);
      this.labelOutput.TabIndex = 6;
      this.labelOutput.Text = "Output:";
      this.labelOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxOutputVar
      // 
      this.textBoxOutputVar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOutputVar.Location = new System.Drawing.Point(112, 104);
      this.textBoxOutputVar.Name = "textBoxOutputVar";
      this.textBoxOutputVar.Size = new System.Drawing.Size(272, 20);
      this.textBoxOutputVar.TabIndex = 8;
      this.toolTips.SetToolTip(this.textBoxOutputVar, "The variable to place the output of the operation into");
      // 
      // EditStringOperation
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(392, 169);
      this.Controls.Add(this.labelVarPrefix);
      this.Controls.Add(this.labelOutput);
      this.Controls.Add(this.textBoxOutputVar);
      this.Controls.Add(this.labelInput2);
      this.Controls.Add(this.labelInput1);
      this.Controls.Add(this.labelOperation);
      this.Controls.Add(this.textBoxInput2);
      this.Controls.Add(this.comboBoxOperation);
      this.Controls.Add(this.textBoxInput1);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(400, 196);
      this.Name = "EditStringOperation";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "String Operation";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxInput1;
    private System.Windows.Forms.ComboBox comboBoxOperation;
    private System.Windows.Forms.TextBox textBoxInput2;
    private System.Windows.Forms.Label labelOperation;
    private System.Windows.Forms.Label labelInput1;
    private System.Windows.Forms.Label labelInput2;
    private System.Windows.Forms.Label labelVarPrefix;
    private System.Windows.Forms.Label labelOutput;
    private System.Windows.Forms.TextBox textBoxOutputVar;
  }

}
