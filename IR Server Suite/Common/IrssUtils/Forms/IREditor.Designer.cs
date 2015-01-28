namespace IrssUtils.Forms
{

  partial class IREditor
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonLearn = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelInvalid = new System.Windows.Forms.Label();
            this.toolTipIr = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Enabled = false;
            this.buttonOk.Image = global::IrssUtils.Properties.Resources.ScrollLeft;
            this.buttonOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOk.Location = new System.Drawing.Point(157, 96);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(59, 24);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Insert";
            this.buttonOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipIr.SetToolTip(this.buttonOk, "Insert this command in the calling list and close editor (Enter)");
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonLearn
            // 
            this.buttonLearn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLearn.Image = global::IrssUtils.Properties.Resources.Edit;
            this.buttonLearn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonLearn.Location = new System.Drawing.Point(70, 96);
            this.buttonLearn.Name = "buttonLearn";
            this.buttonLearn.Size = new System.Drawing.Size(56, 24);
            this.buttonLearn.TabIndex = 3;
            this.buttonLearn.Text = "Learn";
            this.buttonLearn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipIr.SetToolTip(this.buttonLearn, "Learn the IR comand (F7)");
            this.buttonLearn.UseVisualStyleBackColor = true;
            this.buttonLearn.Click += new System.EventHandler(this.buttonLearn_Click);
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(8, 8);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(56, 20);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(64, 8);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(214, 20);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonTest.Image = global::IrssUtils.Properties.Resources.Run;
            this.buttonTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTest.Location = new System.Drawing.Point(8, 96);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(56, 24);
            this.buttonTest.TabIndex = 2;
            this.buttonTest.Text = "Test";
            this.buttonTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipIr.SetToolTip(this.buttonTest, "Run the IR comand (F5)");
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(64, 43);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(214, 21);
            this.comboBoxPort.TabIndex = 1;
            // 
            // labelPort
            // 
            this.labelPort.Location = new System.Drawing.Point(12, 44);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(52, 20);
            this.labelPort.TabIndex = 0;
            this.labelPort.Text = "Port:";
            this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStatus.Enabled = false;
            this.labelStatus.Location = new System.Drawing.Point(8, 68);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(270, 23);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "Nothing learnt yet";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(222, 96);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 24);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.toolTipIr.SetToolTip(this.buttonCancel, "Close this editor (Esc)");
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelInvalid
            // 
            this.labelInvalid.AutoSize = true;
            this.labelInvalid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelInvalid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelInvalid.Location = new System.Drawing.Point(29, 27);
            this.labelInvalid.Name = "labelInvalid";
            this.labelInvalid.Size = new System.Drawing.Size(240, 13);
            this.labelInvalid.TabIndex = 8;
            this.labelInvalid.Text = "the name can\'t contain characters: \\ / : * ? \" < > |";
            this.labelInvalid.Visible = false;
            // 
            // LearnIR
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(284, 129);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.comboBoxPort);
            this.Controls.Add(this.buttonLearn);
            this.Controls.Add(this.labelInvalid);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 167);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 167);
            this.Name = "LearnIR";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IR Command Editor";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.LearnIR_HelpButtonClicked);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.LearnIR_HelpRequested);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LearnIR_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Button buttonLearn;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.ComboBox comboBoxPort;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label labelInvalid;
    private System.Windows.Forms.ToolTip toolTipIr;
  }

}
