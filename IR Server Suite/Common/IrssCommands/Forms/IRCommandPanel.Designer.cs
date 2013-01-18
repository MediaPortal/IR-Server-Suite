namespace IrssCommands.Forms
{
  partial class IRCommandPanel
  {
    /// <summary> 
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary> 
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.listViewIR = new System.Windows.Forms.ListView();
      this.irCommandsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addIRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editIRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteIRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.irCommandsToolStrip = new System.Windows.Forms.ToolStrip();
      this.newIRToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.editIRToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.deleteIRToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.irCommandsContextMenuStrip.SuspendLayout();
      this.irCommandsToolStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // listViewIR
      // 
      this.listViewIR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewIR.ContextMenuStrip = this.irCommandsContextMenuStrip;
      this.listViewIR.FullRowSelect = true;
      this.listViewIR.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewIR.HideSelection = false;
      this.listViewIR.LabelEdit = true;
      this.listViewIR.Location = new System.Drawing.Point(3, 3);
      this.listViewIR.MultiSelect = false;
      this.listViewIR.Name = "listViewIR";
      this.listViewIR.ShowGroups = false;
      this.listViewIR.Size = new System.Drawing.Size(330, 145);
      this.listViewIR.TabIndex = 1;
      this.listViewIR.UseCompatibleStateImageBehavior = false;
      this.listViewIR.View = System.Windows.Forms.View.List;
      this.listViewIR.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewIR_AfterLabelEdit);
      this.listViewIR.SelectedIndexChanged += new System.EventHandler(this.listViewIR_SelectedIndexChanged);
      // 
      // irCommandsContextMenuStrip
      // 
      this.irCommandsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addIRToolStripMenuItem,
            this.editIRToolStripMenuItem,
            this.deleteIRToolStripMenuItem});
      this.irCommandsContextMenuStrip.Name = "contextMenuStrip1";
      this.irCommandsContextMenuStrip.Size = new System.Drawing.Size(195, 70);
      // 
      // addIRToolStripMenuItem
      // 
      this.addIRToolStripMenuItem.Name = "addIRToolStripMenuItem";
      this.addIRToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
      this.addIRToolStripMenuItem.Text = "Add new IR Command";
      this.addIRToolStripMenuItem.Click += new System.EventHandler(this.NewIRCommand);
      // 
      // editIRToolStripMenuItem
      // 
      this.editIRToolStripMenuItem.Name = "editIRToolStripMenuItem";
      this.editIRToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
      this.editIRToolStripMenuItem.Text = "Edit IR Command";
      this.editIRToolStripMenuItem.Click += new System.EventHandler(this.EditIRCommand);
      // 
      // deleteIRToolStripMenuItem
      // 
      this.deleteIRToolStripMenuItem.Name = "deleteIRToolStripMenuItem";
      this.deleteIRToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
      this.deleteIRToolStripMenuItem.Text = "Delete IR Command";
      this.deleteIRToolStripMenuItem.Click += new System.EventHandler(this.DeleteIRCommand);
      // 
      // irCommandsToolStrip
      // 
      this.irCommandsToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.irCommandsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.irCommandsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newIRToolStripButton,
            this.editIRToolStripButton,
            this.deleteIRToolStripButton});
      this.irCommandsToolStrip.Location = new System.Drawing.Point(0, 151);
      this.irCommandsToolStrip.Name = "irCommandsToolStrip";
      this.irCommandsToolStrip.Size = new System.Drawing.Size(336, 25);
      this.irCommandsToolStrip.TabIndex = 2;
      this.irCommandsToolStrip.Text = "IR Commands";
      // 
      // newIRToolStripButton
      // 
      this.newIRToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newIRToolStripButton.Name = "newIRToolStripButton";
      this.newIRToolStripButton.Size = new System.Drawing.Size(35, 22);
      this.newIRToolStripButton.Text = "New";
      this.newIRToolStripButton.ToolTipText = "Create a new IR Command";
      this.newIRToolStripButton.Click += new System.EventHandler(this.NewIRCommand);
      // 
      // editIRToolStripButton
      // 
      this.editIRToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.editIRToolStripButton.Name = "editIRToolStripButton";
      this.editIRToolStripButton.Size = new System.Drawing.Size(31, 22);
      this.editIRToolStripButton.Text = "Edit";
      this.editIRToolStripButton.ToolTipText = "Edit the selected IR Command";
      this.editIRToolStripButton.Click += new System.EventHandler(this.EditIRCommand);
      // 
      // deleteIRToolStripButton
      // 
      this.deleteIRToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.deleteIRToolStripButton.Name = "deleteIRToolStripButton";
      this.deleteIRToolStripButton.Size = new System.Drawing.Size(44, 22);
      this.deleteIRToolStripButton.Text = "Delete";
      this.deleteIRToolStripButton.ToolTipText = "Delete the selected IR Command";
      this.deleteIRToolStripButton.Click += new System.EventHandler(this.DeleteIRCommand);
      // 
      // IRCommandPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.irCommandsToolStrip);
      this.Controls.Add(this.listViewIR);
      this.Name = "IRCommandPanel";
      this.Size = new System.Drawing.Size(336, 176);
      this.irCommandsContextMenuStrip.ResumeLayout(false);
      this.irCommandsToolStrip.ResumeLayout(false);
      this.irCommandsToolStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView listViewIR;
    private System.Windows.Forms.ToolStrip irCommandsToolStrip;
    private System.Windows.Forms.ToolStripButton newIRToolStripButton;
    private System.Windows.Forms.ToolStripButton editIRToolStripButton;
    private System.Windows.Forms.ToolStripButton deleteIRToolStripButton;
    private System.Windows.Forms.ContextMenuStrip irCommandsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem addIRToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editIRToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteIRToolStripMenuItem;

  }
}
