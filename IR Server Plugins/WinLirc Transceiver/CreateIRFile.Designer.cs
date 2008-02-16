namespace InputService.Plugin
{
  partial class CreateIRFile
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
      this.buttonCreate = new System.Windows.Forms.Button();
      this.groupBoxDetails = new System.Windows.Forms.GroupBox();
      this.labelPassword = new System.Windows.Forms.Label();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.labelRemote = new System.Windows.Forms.Label();
      this.textBoxRemote = new System.Windows.Forms.TextBox();
      this.labelButton = new System.Windows.Forms.Label();
      this.textBoxButton = new System.Windows.Forms.TextBox();
      this.labelRepeats = new System.Windows.Forms.Label();
      this.numericUpDownRepeats = new System.Windows.Forms.NumericUpDown();
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.groupBoxDetails.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeats)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonDone
      // 
      this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDone.Location = new System.Drawing.Point(208, 200);
      this.buttonDone.Name = "buttonDone";
      this.buttonDone.Size = new System.Drawing.Size(64, 24);
      this.buttonDone.TabIndex = 1;
      this.buttonDone.Text = "Done";
      this.buttonDone.UseVisualStyleBackColor = true;
      this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
      // 
      // buttonCreate
      // 
      this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCreate.Location = new System.Drawing.Point(176, 152);
      this.buttonCreate.Name = "buttonCreate";
      this.buttonCreate.Size = new System.Drawing.Size(80, 24);
      this.buttonCreate.TabIndex = 10;
      this.buttonCreate.Text = "Create file";
      this.buttonCreate.UseVisualStyleBackColor = true;
      this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
      // 
      // groupBoxDetails
      // 
      this.groupBoxDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxDetails.Controls.Add(this.numericUpDownRepeats);
      this.groupBoxDetails.Controls.Add(this.labelRepeats);
      this.groupBoxDetails.Controls.Add(this.labelButton);
      this.groupBoxDetails.Controls.Add(this.textBoxButton);
      this.groupBoxDetails.Controls.Add(this.labelRemote);
      this.groupBoxDetails.Controls.Add(this.textBoxRemote);
      this.groupBoxDetails.Controls.Add(this.labelPassword);
      this.groupBoxDetails.Controls.Add(this.textBoxPassword);
      this.groupBoxDetails.Controls.Add(this.buttonCreate);
      this.groupBoxDetails.Location = new System.Drawing.Point(8, 8);
      this.groupBoxDetails.Name = "groupBoxDetails";
      this.groupBoxDetails.Size = new System.Drawing.Size(264, 184);
      this.groupBoxDetails.TabIndex = 0;
      this.groupBoxDetails.TabStop = false;
      this.groupBoxDetails.Text = "Details";
      // 
      // labelPassword
      // 
      this.labelPassword.Location = new System.Drawing.Point(8, 24);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(96, 20);
      this.labelPassword.TabIndex = 2;
      this.labelPassword.Text = "Password:";
      this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPassword.Location = new System.Drawing.Point(104, 24);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new System.Drawing.Size(152, 20);
      this.textBoxPassword.TabIndex = 3;
      // 
      // labelRemote
      // 
      this.labelRemote.Location = new System.Drawing.Point(8, 56);
      this.labelRemote.Name = "labelRemote";
      this.labelRemote.Size = new System.Drawing.Size(96, 20);
      this.labelRemote.TabIndex = 4;
      this.labelRemote.Text = "Remote name:";
      this.labelRemote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxRemote
      // 
      this.textBoxRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxRemote.Location = new System.Drawing.Point(104, 56);
      this.textBoxRemote.Name = "textBoxRemote";
      this.textBoxRemote.Size = new System.Drawing.Size(152, 20);
      this.textBoxRemote.TabIndex = 5;
      // 
      // labelButton
      // 
      this.labelButton.Location = new System.Drawing.Point(8, 88);
      this.labelButton.Name = "labelButton";
      this.labelButton.Size = new System.Drawing.Size(96, 20);
      this.labelButton.TabIndex = 6;
      this.labelButton.Text = "Button name:";
      this.labelButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxButton
      // 
      this.textBoxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxButton.Location = new System.Drawing.Point(104, 88);
      this.textBoxButton.Name = "textBoxButton";
      this.textBoxButton.Size = new System.Drawing.Size(152, 20);
      this.textBoxButton.TabIndex = 7;
      // 
      // labelRepeats
      // 
      this.labelRepeats.Location = new System.Drawing.Point(8, 120);
      this.labelRepeats.Name = "labelRepeats";
      this.labelRepeats.Size = new System.Drawing.Size(96, 20);
      this.labelRepeats.TabIndex = 8;
      this.labelRepeats.Text = "Repeats:";
      this.labelRepeats.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // numericUpDownRepeats
      // 
      this.numericUpDownRepeats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownRepeats.Location = new System.Drawing.Point(104, 120);
      this.numericUpDownRepeats.Name = "numericUpDownRepeats";
      this.numericUpDownRepeats.Size = new System.Drawing.Size(152, 20);
      this.numericUpDownRepeats.TabIndex = 9;
      this.numericUpDownRepeats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericUpDownRepeats.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.Filter = "All Files|*.*";
      this.saveFileDialog.Title = "Create IR Command File";
      // 
      // CreateIRFile
      // 
      this.AcceptButton = this.buttonDone;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(280, 232);
      this.Controls.Add(this.groupBoxDetails);
      this.Controls.Add(this.buttonDone);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "CreateIRFile";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Create IR Command File";
      this.groupBoxDetails.ResumeLayout(false);
      this.groupBoxDetails.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeats)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonDone;
    private System.Windows.Forms.Button buttonCreate;
    private System.Windows.Forms.GroupBox groupBoxDetails;
    private System.Windows.Forms.Label labelRepeats;
    private System.Windows.Forms.Label labelButton;
    private System.Windows.Forms.TextBox textBoxButton;
    private System.Windows.Forms.Label labelRemote;
    private System.Windows.Forms.TextBox textBoxRemote;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.NumericUpDown numericUpDownRepeats;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
  }
}