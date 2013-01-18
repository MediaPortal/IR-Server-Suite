namespace Translator.Forms
{
  partial class ButtonMappingForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonMappingForm));
      this.groupBoxButton = new System.Windows.Forms.GroupBox();
      this.labelKeyCode = new System.Windows.Forms.Label();
      this.labelButtonDesc = new System.Windows.Forms.Label();
      this.textBoxKeyCode = new System.Windows.Forms.TextBox();
      this.textBoxButtonDesc = new System.Windows.Forms.TextBox();
      this.groupBoxSet = new System.Windows.Forms.GroupBox();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.treeViewCommandList = new System.Windows.Forms.TreeView();
      this.buttonTest = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxButton.SuspendLayout();
      this.groupBoxSet.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxButton
      // 
      this.groupBoxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxButton.Controls.Add(this.labelKeyCode);
      this.groupBoxButton.Controls.Add(this.labelButtonDesc);
      this.groupBoxButton.Controls.Add(this.textBoxKeyCode);
      this.groupBoxButton.Controls.Add(this.textBoxButtonDesc);
      this.groupBoxButton.Location = new System.Drawing.Point(8, 8);
      this.groupBoxButton.Name = "groupBoxButton";
      this.groupBoxButton.Size = new System.Drawing.Size(530, 80);
      this.groupBoxButton.TabIndex = 0;
      this.groupBoxButton.TabStop = false;
      this.groupBoxButton.Text = "Button";
      // 
      // labelKeyCode
      // 
      this.labelKeyCode.Location = new System.Drawing.Point(8, 16);
      this.labelKeyCode.Name = "labelKeyCode";
      this.labelKeyCode.Size = new System.Drawing.Size(80, 22);
      this.labelKeyCode.TabIndex = 0;
      this.labelKeyCode.Text = "Code:";
      this.labelKeyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelButtonDesc
      // 
      this.labelButtonDesc.Location = new System.Drawing.Point(8, 48);
      this.labelButtonDesc.Name = "labelButtonDesc";
      this.labelButtonDesc.Size = new System.Drawing.Size(80, 21);
      this.labelButtonDesc.TabIndex = 2;
      this.labelButtonDesc.Text = "Description:";
      this.labelButtonDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxKeyCode
      // 
      this.textBoxKeyCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxKeyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxKeyCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxKeyCode.Location = new System.Drawing.Point(88, 16);
      this.textBoxKeyCode.Name = "textBoxKeyCode";
      this.textBoxKeyCode.ReadOnly = true;
      this.textBoxKeyCode.Size = new System.Drawing.Size(434, 22);
      this.textBoxKeyCode.TabIndex = 1;
      this.textBoxKeyCode.TabStop = false;
      this.textBoxKeyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.toolTip.SetToolTip(this.textBoxKeyCode, "This button\'s unique IR code");
      // 
      // textBoxButtonDesc
      // 
      this.textBoxButtonDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxButtonDesc.Location = new System.Drawing.Point(88, 48);
      this.textBoxButtonDesc.Name = "textBoxButtonDesc";
      this.textBoxButtonDesc.Size = new System.Drawing.Size(434, 20);
      this.textBoxButtonDesc.TabIndex = 3;
      this.toolTip.SetToolTip(this.textBoxButtonDesc, "Provide a description of this button here");
      this.textBoxButtonDesc.TextChanged += new System.EventHandler(this.textBoxButtonDesc_TextChanged);
      // 
      // groupBoxSet
      // 
      this.groupBoxSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSet.Controls.Add(this.splitContainer1);
      this.groupBoxSet.Location = new System.Drawing.Point(8, 94);
      this.groupBoxSet.Name = "groupBoxSet";
      this.groupBoxSet.Size = new System.Drawing.Size(530, 352);
      this.groupBoxSet.TabIndex = 1;
      this.groupBoxSet.TabStop = false;
      this.groupBoxSet.Text = "Command";
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(3, 16);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.treeViewCommandList);
      this.splitContainer1.Size = new System.Drawing.Size(524, 333);
      this.splitContainer1.SplitterDistance = 115;
      this.splitContainer1.TabIndex = 6;
      // 
      // treeViewCommandList
      // 
      this.treeViewCommandList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewCommandList.FullRowSelect = true;
      this.treeViewCommandList.Location = new System.Drawing.Point(0, 0);
      this.treeViewCommandList.Name = "treeViewCommandList";
      this.treeViewCommandList.Size = new System.Drawing.Size(524, 115);
      this.treeViewCommandList.TabIndex = 1;
      this.treeViewCommandList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCommandList_AfterSelect);
      // 
      // buttonTest
      // 
      this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTest.Location = new System.Drawing.Point(8, 453);
      this.buttonTest.Name = "buttonTest";
      this.buttonTest.Size = new System.Drawing.Size(48, 22);
      this.buttonTest.TabIndex = 3;
      this.buttonTest.Text = "Test";
      this.toolTip.SetToolTip(this.buttonTest, "Click here to test the currently set command");
      this.buttonTest.UseVisualStyleBackColor = true;
      this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(482, 452);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(56, 24);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(418, 452);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // ButtonMappingForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(546, 485);
      this.Controls.Add(this.groupBoxButton);
      this.Controls.Add(this.groupBoxSet);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonTest);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(562, 523);
      this.Name = "ButtonMappingForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Button Mapping";
      this.Load += new System.EventHandler(this.ButtonMappingForm_Load);
      this.groupBoxButton.ResumeLayout(false);
      this.groupBoxButton.PerformLayout();
      this.groupBoxSet.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxButton;
    private System.Windows.Forms.Label labelKeyCode;
    private System.Windows.Forms.Label labelButtonDesc;
    private System.Windows.Forms.TextBox textBoxKeyCode;
    private System.Windows.Forms.TextBox textBoxButtonDesc;
    private System.Windows.Forms.GroupBox groupBoxSet;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TreeView treeViewCommandList;

  }
}