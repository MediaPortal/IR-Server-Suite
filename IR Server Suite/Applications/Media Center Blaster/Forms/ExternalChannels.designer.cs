namespace MediaCenterBlaster
{
  partial class ExternalChannels
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
      this.buttonOK = new System.Windows.Forms.Button();
      this.groupBoxQuickSetup = new System.Windows.Forms.GroupBox();
      this.buttonQuickSet = new System.Windows.Forms.Button();
      this.comboBoxQuickSetup = new System.Windows.Forms.ComboBox();
      this.groupBoxTest = new System.Windows.Forms.GroupBox();
      this.labelCh = new System.Windows.Forms.Label();
      this.buttonTest = new System.Windows.Forms.Button();
      this.numericUpDownTest = new System.Windows.Forms.NumericUpDown();
      this.tabControlTVCards = new System.Windows.Forms.TabControl();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBoxQuickSetup.SuspendLayout();
      this.groupBoxTest.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTest)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(400, 408);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // groupBoxQuickSetup
      // 
      this.groupBoxQuickSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxQuickSetup.Controls.Add(this.buttonQuickSet);
      this.groupBoxQuickSetup.Controls.Add(this.comboBoxQuickSetup);
      this.groupBoxQuickSetup.Location = new System.Drawing.Point(8, 352);
      this.groupBoxQuickSetup.Name = "groupBoxQuickSetup";
      this.groupBoxQuickSetup.Size = new System.Drawing.Size(288, 48);
      this.groupBoxQuickSetup.TabIndex = 1;
      this.groupBoxQuickSetup.TabStop = false;
      this.groupBoxQuickSetup.Text = "Quick Setup";
      // 
      // buttonQuickSet
      // 
      this.buttonQuickSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonQuickSet.Location = new System.Drawing.Point(232, 16);
      this.buttonQuickSet.Name = "buttonQuickSet";
      this.buttonQuickSet.Size = new System.Drawing.Size(48, 21);
      this.buttonQuickSet.TabIndex = 1;
      this.buttonQuickSet.Text = "Set";
      this.buttonQuickSet.UseVisualStyleBackColor = true;
      this.buttonQuickSet.Click += new System.EventHandler(this.buttonQuickSet_Click);
      // 
      // comboBoxQuickSetup
      // 
      this.comboBoxQuickSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxQuickSetup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxQuickSetup.FormattingEnabled = true;
      this.comboBoxQuickSetup.Location = new System.Drawing.Point(8, 16);
      this.comboBoxQuickSetup.Name = "comboBoxQuickSetup";
      this.comboBoxQuickSetup.Size = new System.Drawing.Size(216, 21);
      this.comboBoxQuickSetup.TabIndex = 0;
      // 
      // groupBoxTest
      // 
      this.groupBoxTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxTest.Controls.Add(this.labelCh);
      this.groupBoxTest.Controls.Add(this.buttonTest);
      this.groupBoxTest.Controls.Add(this.numericUpDownTest);
      this.groupBoxTest.Location = new System.Drawing.Point(304, 352);
      this.groupBoxTest.Name = "groupBoxTest";
      this.groupBoxTest.Size = new System.Drawing.Size(216, 48);
      this.groupBoxTest.TabIndex = 2;
      this.groupBoxTest.TabStop = false;
      this.groupBoxTest.Text = "Test";
      // 
      // labelCh
      // 
      this.labelCh.Location = new System.Drawing.Point(8, 16);
      this.labelCh.Name = "labelCh";
      this.labelCh.Size = new System.Drawing.Size(64, 20);
      this.labelCh.TabIndex = 0;
      this.labelCh.Text = "Channel:";
      this.labelCh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonTest
      // 
      this.buttonTest.Location = new System.Drawing.Point(152, 16);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(56, 20);
      this.buttonTest.TabIndex = 2;
      this.buttonTest.Text = "Test";
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // numericUpDownTest
      // 
      this.numericUpDownTest.Location = new System.Drawing.Point(72, 16);
      this.numericUpDownTest.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
      this.numericUpDownTest.Name = "numericUpDownTest";
      this.numericUpDownTest.Size = new System.Drawing.Size(72, 20);
      this.numericUpDownTest.TabIndex = 1;
      this.numericUpDownTest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownTest.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // tabControlTVCards
      // 
      this.tabControlTVCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControlTVCards.Location = new System.Drawing.Point(8, 8);
      this.tabControlTVCards.Name = "tabControlTVCards";
      this.tabControlTVCards.SelectedIndex = 0;
      this.tabControlTVCards.Size = new System.Drawing.Size(512, 336);
      this.tabControlTVCards.TabIndex = 0;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(464, 408);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // ExternalChannels
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(528, 439);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.tabControlTVCards);
      this.Controls.Add(this.groupBoxTest);
      this.Controls.Add(this.groupBoxQuickSetup);
      this.Controls.Add(this.buttonOK);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(536, 466);
      this.Name = "ExternalChannels";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "External Channel Changing";
      this.Load += new System.EventHandler(this.ExternalChannels_Load);
      this.groupBoxQuickSetup.ResumeLayout(false);
      this.groupBoxTest.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTest)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.GroupBox groupBoxQuickSetup;
    private System.Windows.Forms.ComboBox comboBoxQuickSetup;
    private System.Windows.Forms.GroupBox groupBoxTest;
    private System.Windows.Forms.NumericUpDown numericUpDownTest;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button buttonQuickSet;
    private System.Windows.Forms.Label labelCh;
    private System.Windows.Forms.TabControl tabControlTVCards;
    private System.Windows.Forms.Button buttonCancel;

  }
}