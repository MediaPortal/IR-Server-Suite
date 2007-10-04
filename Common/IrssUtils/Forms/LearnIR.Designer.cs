namespace IrssUtils.Forms
{

  partial class LearnIR
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
      this.buttonDone = new System.Windows.Forms.Button();
      this.labelLearned = new System.Windows.Forms.Label();
      this.buttonLearn = new System.Windows.Forms.Button();
      this.labelName = new System.Windows.Forms.Label();
      this.textBoxName = new System.Windows.Forms.TextBox();
      this.buttonTest = new System.Windows.Forms.Button();
      this.groupBoxStatus = new System.Windows.Forms.GroupBox();
      this.comboBoxPort = new System.Windows.Forms.ComboBox();
      this.labelPort = new System.Windows.Forms.Label();
      this.groupBoxTest = new System.Windows.Forms.GroupBox();
      this.groupBoxStatus.SuspendLayout();
      this.groupBoxTest.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonDone
      // 
      this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDone.Location = new System.Drawing.Point(216, 152);
      this.buttonDone.Name = "buttonDone";
      this.buttonDone.Size = new System.Drawing.Size(56, 24);
      this.buttonDone.TabIndex = 5;
      this.buttonDone.Text = "OK";
      this.buttonDone.UseVisualStyleBackColor = true;
      this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
      // 
      // labelLearned
      // 
      this.labelLearned.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelLearned.BackColor = System.Drawing.Color.WhiteSmoke;
      this.labelLearned.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.labelLearned.Location = new System.Drawing.Point(8, 16);
      this.labelLearned.Name = "labelLearned";
      this.labelLearned.Size = new System.Drawing.Size(248, 24);
      this.labelLearned.TabIndex = 0;
      this.labelLearned.Text = "Status";
      this.labelLearned.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // buttonLearn
      // 
      this.buttonLearn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonLearn.Location = new System.Drawing.Point(8, 152);
      this.buttonLearn.Name = "buttonLearn";
      this.buttonLearn.Size = new System.Drawing.Size(56, 24);
      this.buttonLearn.TabIndex = 4;
      this.buttonLearn.Text = "Learn";
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
      this.textBoxName.Size = new System.Drawing.Size(208, 20);
      this.textBoxName.TabIndex = 1;
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonTest.Enabled = false;
      this.buttonTest.Location = new System.Drawing.Point(200, 16);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(56, 24);
      this.buttonTest.TabIndex = 2;
      this.buttonTest.Text = "Test";
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // groupBoxStatus
      // 
      this.groupBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxStatus.Controls.Add(this.labelLearned);
      this.groupBoxStatus.Location = new System.Drawing.Point(8, 40);
      this.groupBoxStatus.Name = "groupBoxStatus";
      this.groupBoxStatus.Size = new System.Drawing.Size(264, 48);
      this.groupBoxStatus.TabIndex = 2;
      this.groupBoxStatus.TabStop = false;
      this.groupBoxStatus.Text = "Status";
      // 
      // comboBoxPort
      // 
      this.comboBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPort.FormattingEnabled = true;
      this.comboBoxPort.Location = new System.Drawing.Point(64, 16);
      this.comboBoxPort.Name = "comboBoxPort";
      this.comboBoxPort.Size = new System.Drawing.Size(112, 21);
      this.comboBoxPort.TabIndex = 1;
      // 
      // labelPort
      // 
      this.labelPort.Location = new System.Drawing.Point(8, 16);
      this.labelPort.Name = "labelPort";
      this.labelPort.Size = new System.Drawing.Size(56, 21);
      this.labelPort.TabIndex = 0;
      this.labelPort.Text = "Port:";
      this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // groupBoxTest
      // 
      this.groupBoxTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTest.Controls.Add(this.comboBoxPort);
      this.groupBoxTest.Controls.Add(this.buttonTest);
      this.groupBoxTest.Controls.Add(this.labelPort);
      this.groupBoxTest.Location = new System.Drawing.Point(8, 96);
      this.groupBoxTest.Name = "groupBoxTest";
      this.groupBoxTest.Size = new System.Drawing.Size(264, 48);
      this.groupBoxTest.TabIndex = 3;
      this.groupBoxTest.TabStop = false;
      this.groupBoxTest.Text = "Test";
      // 
      // LearnIR
      // 
      this.AcceptButton = this.buttonDone;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(280, 185);
      this.Controls.Add(this.groupBoxTest);
      this.Controls.Add(this.buttonDone);
      this.Controls.Add(this.groupBoxStatus);
      this.Controls.Add(this.buttonLearn);
      this.Controls.Add(this.labelName);
      this.Controls.Add(this.textBoxName);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(288, 212);
      this.Name = "LearnIR";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Learn IR Command";
      this.groupBoxStatus.ResumeLayout(false);
      this.groupBoxTest.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonDone;
    private System.Windows.Forms.Label labelLearned;
    private System.Windows.Forms.Button buttonLearn;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.GroupBox groupBoxStatus;
    private System.Windows.Forms.ComboBox comboBoxPort;
    private System.Windows.Forms.Label labelPort;
    private System.Windows.Forms.GroupBox groupBoxTest;
  }

}
