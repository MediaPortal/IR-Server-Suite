namespace IrssCommands.Forms
{
  partial class MacroPanel
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
      this.listViewMacro = new System.Windows.Forms.ListView();
      this.macrosContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addMacroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editMacroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteMacroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.testMacroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.createShortcutForMacroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.macrosToolStrip = new System.Windows.Forms.ToolStrip();
      this.newMacroToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.editMacroToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.deleteMacroToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.createShortcutForMacroToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.testMacroToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.macrosContextMenuStrip.SuspendLayout();
      this.macrosToolStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // listViewMacro
      // 
      this.listViewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewMacro.ContextMenuStrip = this.macrosContextMenuStrip;
      this.listViewMacro.FullRowSelect = true;
      this.listViewMacro.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewMacro.HideSelection = false;
      this.listViewMacro.LabelEdit = true;
      this.listViewMacro.Location = new System.Drawing.Point(3, 3);
      this.listViewMacro.MultiSelect = false;
      this.listViewMacro.Name = "listViewMacro";
      this.listViewMacro.Size = new System.Drawing.Size(384, 166);
      this.listViewMacro.TabIndex = 10;
      this.listViewMacro.UseCompatibleStateImageBehavior = false;
      this.listViewMacro.View = System.Windows.Forms.View.List;
      this.listViewMacro.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMacro_AfterLabelEdit);
      this.listViewMacro.SelectedIndexChanged += new System.EventHandler(this.listViewMacro_SelectedIndexChanged);
      this.listViewMacro.DoubleClick += new System.EventHandler(this.EditMacro);
      // 
      // macrosContextMenuStrip
      // 
      this.macrosContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMacroToolStripMenuItem,
            this.editMacroToolStripMenuItem,
            this.deleteMacroToolStripMenuItem,
            this.toolStripSeparator4,
            this.testMacroToolStripMenuItem,
            this.toolStripSeparator2,
            this.createShortcutForMacroToolStripMenuItem});
      this.macrosContextMenuStrip.Name = "macrosContextMenuStrip";
      this.macrosContextMenuStrip.Size = new System.Drawing.Size(159, 126);
      // 
      // addMacroToolStripMenuItem
      // 
      this.addMacroToolStripMenuItem.Name = "addMacroToolStripMenuItem";
      this.addMacroToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.addMacroToolStripMenuItem.Text = "Add new Macro";
      this.addMacroToolStripMenuItem.Click += new System.EventHandler(this.NewMacro);
      // 
      // editMacroToolStripMenuItem
      // 
      this.editMacroToolStripMenuItem.Name = "editMacroToolStripMenuItem";
      this.editMacroToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.editMacroToolStripMenuItem.Text = "Edit Macro";
      this.editMacroToolStripMenuItem.Click += new System.EventHandler(this.EditMacro);
      // 
      // deleteMacroToolStripMenuItem
      // 
      this.deleteMacroToolStripMenuItem.Name = "deleteMacroToolStripMenuItem";
      this.deleteMacroToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.deleteMacroToolStripMenuItem.Text = "Delete Macro";
      this.deleteMacroToolStripMenuItem.Click += new System.EventHandler(this.DeleteMacro);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(155, 6);
      // 
      // testMacroToolStripMenuItem
      // 
      this.testMacroToolStripMenuItem.Name = "testMacroToolStripMenuItem";
      this.testMacroToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.testMacroToolStripMenuItem.Text = "Test Macro";
      this.testMacroToolStripMenuItem.Click += new System.EventHandler(this.TestMacro);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(155, 6);
      // 
      // createShortcutForMacroToolStripMenuItem
      // 
      this.createShortcutForMacroToolStripMenuItem.Name = "createShortcutForMacroToolStripMenuItem";
      this.createShortcutForMacroToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
      this.createShortcutForMacroToolStripMenuItem.Text = "Create Shortcut";
      this.createShortcutForMacroToolStripMenuItem.Click += new System.EventHandler(this.CreateShortcutForMacro);
      // 
      // macrosToolStrip
      // 
      this.macrosToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.macrosToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.macrosToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMacroToolStripButton,
            this.editMacroToolStripButton,
            this.deleteMacroToolStripButton,
            this.createShortcutForMacroToolStripButton,
            this.testMacroToolStripButton});
      this.macrosToolStrip.Location = new System.Drawing.Point(0, 172);
      this.macrosToolStrip.Name = "macrosToolStrip";
      this.macrosToolStrip.Size = new System.Drawing.Size(390, 25);
      this.macrosToolStrip.TabIndex = 15;
      this.macrosToolStrip.Text = "Macros";
      // 
      // newMacroToolStripButton
      // 
      this.newMacroToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newMacroToolStripButton.Name = "newMacroToolStripButton";
      this.newMacroToolStripButton.Size = new System.Drawing.Size(35, 22);
      this.newMacroToolStripButton.Text = "New";
      this.newMacroToolStripButton.ToolTipText = "Create a new macro";
      this.newMacroToolStripButton.Click += new System.EventHandler(this.NewMacro);
      // 
      // editMacroToolStripButton
      // 
      this.editMacroToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.editMacroToolStripButton.Name = "editMacroToolStripButton";
      this.editMacroToolStripButton.Size = new System.Drawing.Size(31, 22);
      this.editMacroToolStripButton.Text = "Edit";
      this.editMacroToolStripButton.ToolTipText = "Edit the selected macro";
      this.editMacroToolStripButton.Click += new System.EventHandler(this.EditMacro);
      // 
      // deleteMacroToolStripButton
      // 
      this.deleteMacroToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.deleteMacroToolStripButton.Name = "deleteMacroToolStripButton";
      this.deleteMacroToolStripButton.Size = new System.Drawing.Size(44, 22);
      this.deleteMacroToolStripButton.Text = "Delete";
      this.deleteMacroToolStripButton.ToolTipText = "Delete the selected macro";
      this.deleteMacroToolStripButton.Click += new System.EventHandler(this.DeleteMacro);
      // 
      // createShortcutForMacroToolStripButton
      // 
      this.createShortcutForMacroToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.createShortcutForMacroToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.createShortcutForMacroToolStripButton.Name = "createShortcutForMacroToolStripButton";
      this.createShortcutForMacroToolStripButton.Size = new System.Drawing.Size(92, 22);
      this.createShortcutForMacroToolStripButton.Text = "Create shortcut";
      this.createShortcutForMacroToolStripButton.ToolTipText = "Create a shortcut to run the selected macro";
      this.createShortcutForMacroToolStripButton.Click += new System.EventHandler(this.CreateShortcutForMacro);
      // 
      // testMacroToolStripButton
      // 
      this.testMacroToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.testMacroToolStripButton.Name = "testMacroToolStripButton";
      this.testMacroToolStripButton.Size = new System.Drawing.Size(33, 22);
      this.testMacroToolStripButton.Text = "Test";
      this.testMacroToolStripButton.ToolTipText = "Test the selected macro";
      this.testMacroToolStripButton.Click += new System.EventHandler(this.TestMacro);
      // 
      // MacroPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.macrosToolStrip);
      this.Controls.Add(this.listViewMacro);
      this.Name = "MacroPanel";
      this.Size = new System.Drawing.Size(390, 197);
      this.Load += new System.EventHandler(this.MacroPanel_Load);
      this.macrosContextMenuStrip.ResumeLayout(false);
      this.macrosToolStrip.ResumeLayout(false);
      this.macrosToolStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView listViewMacro;
    private System.Windows.Forms.ToolStrip macrosToolStrip;
    private System.Windows.Forms.ToolStripButton newMacroToolStripButton;
    private System.Windows.Forms.ToolStripButton editMacroToolStripButton;
    private System.Windows.Forms.ToolStripButton deleteMacroToolStripButton;
    private System.Windows.Forms.ToolStripButton createShortcutForMacroToolStripButton;
    private System.Windows.Forms.ToolStripButton testMacroToolStripButton;
    private System.Windows.Forms.ContextMenuStrip macrosContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem addMacroToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editMacroToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteMacroToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem testMacroToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem createShortcutForMacroToolStripMenuItem;
  }
}
