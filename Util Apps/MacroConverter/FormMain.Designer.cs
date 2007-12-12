namespace MacroConverter
{
  partial class FormMain
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
      this.buttonGO = new System.Windows.Forms.Button();
      this.listViewStatus = new System.Windows.Forms.ListView();
      this.columnHeaderText = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // buttonGO
      // 
      this.buttonGO.Dock = System.Windows.Forms.DockStyle.Top;
      this.buttonGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonGO.Location = new System.Drawing.Point(0, 0);
      this.buttonGO.Name = "buttonGO";
      this.buttonGO.Size = new System.Drawing.Size(506, 32);
      this.buttonGO.TabIndex = 0;
      this.buttonGO.Text = "GO !";
      this.buttonGO.UseVisualStyleBackColor = true;
      this.buttonGO.Click += new System.EventHandler(this.buttonGO_Click);
      // 
      // listViewStatus
      // 
      this.listViewStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderText});
      this.listViewStatus.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewStatus.Location = new System.Drawing.Point(0, 32);
      this.listViewStatus.Name = "listViewStatus";
      this.listViewStatus.Size = new System.Drawing.Size(506, 455);
      this.listViewStatus.TabIndex = 1;
      this.listViewStatus.UseCompatibleStateImageBehavior = false;
      this.listViewStatus.View = System.Windows.Forms.View.Details;
      // 
      // columnHeaderText
      // 
      this.columnHeaderText.Text = "Status";
      this.columnHeaderText.Width = 485;
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(506, 487);
      this.Controls.Add(this.listViewStatus);
      this.Controls.Add(this.buttonGO);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormMain";
      this.ShowIcon = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Macro Converter";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonGO;
    private System.Windows.Forms.ListView listViewStatus;
    private System.Windows.Forms.ColumnHeader columnHeaderText;
  }
}

