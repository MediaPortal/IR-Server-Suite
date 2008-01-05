namespace LogViewer
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
      this.components = new System.ComponentModel.Container();
      this.listViewLines = new System.Windows.Forms.ListView();
      this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.removeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // listViewLines
      // 
      this.listViewLines.AllowDrop = true;
      this.listViewLines.ContextMenuStrip = this.contextMenuStrip;
      this.listViewLines.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listViewLines.FullRowSelect = true;
      this.listViewLines.GridLines = true;
      this.listViewLines.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewLines.HideSelection = false;
      this.listViewLines.Location = new System.Drawing.Point(0, 0);
      this.listViewLines.Name = "listViewLines";
      this.listViewLines.ShowGroups = false;
      this.listViewLines.Size = new System.Drawing.Size(584, 364);
      this.listViewLines.TabIndex = 0;
      this.listViewLines.UseCompatibleStateImageBehavior = false;
      this.listViewLines.View = System.Windows.Forms.View.Details;
      this.listViewLines.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewLines_DragDrop);
      // 
      // contextMenuStrip
      // 
      this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.removeFileToolStripMenuItem});
      this.contextMenuStrip.Name = "contextMenuStrip";
      this.contextMenuStrip.Size = new System.Drawing.Size(151, 70);
      // 
      // refreshToolStripMenuItem
      // 
      this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
      this.refreshToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.refreshToolStripMenuItem.Text = "&Refresh";
      // 
      // clearToolStripMenuItem
      // 
      this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
      this.clearToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.clearToolStripMenuItem.Text = "&Clear";
      // 
      // removeFileToolStripMenuItem
      // 
      this.removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
      this.removeFileToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
      this.removeFileToolStripMenuItem.Text = "Remove &File ...";
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(584, 364);
      this.Controls.Add(this.listViewLines);
      this.Name = "FormMain";
      this.Text = "Log Viewer";
      this.contextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listViewLines;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeFileToolStripMenuItem;
  }
}

