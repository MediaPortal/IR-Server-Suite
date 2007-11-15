namespace Translator
{
  partial class MenuForm
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
      this.listViewMenu = new System.Windows.Forms.ListView();
      this.columnHeader = new System.Windows.Forms.ColumnHeader();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.labelHeader = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // listViewMenu
      // 
      this.listViewMenu.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.listViewMenu.Alignment = System.Windows.Forms.ListViewAlignment.Left;
      this.listViewMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.listViewMenu.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
      this.listViewMenu.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewMenu.FullRowSelect = true;
      this.listViewMenu.GridLines = true;
      this.listViewMenu.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewMenu.HotTracking = true;
      this.listViewMenu.HoverSelection = true;
      this.listViewMenu.Location = new System.Drawing.Point(0, 20);
      this.listViewMenu.MultiSelect = false;
      this.listViewMenu.Name = "listViewMenu";
      this.listViewMenu.ShowGroups = false;
      this.listViewMenu.ShowItemToolTips = true;
      this.listViewMenu.Size = new System.Drawing.Size(256, 108);
      this.listViewMenu.TabIndex = 0;
      this.listViewMenu.TileSize = new System.Drawing.Size(128, 96);
      this.listViewMenu.UseCompatibleStateImageBehavior = false;
      this.listViewMenu.ItemActivate += new System.EventHandler(this.listViewMenu_ItemActivate);
      this.listViewMenu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewMenu_KeyPress);
      // 
      // columnHeader
      // 
      this.columnHeader.Width = 232;
      // 
      // labelHeader
      // 
      this.labelHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
      this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.Location = new System.Drawing.Point(0, 0);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(256, 20);
      this.labelHeader.TabIndex = 1;
      this.labelHeader.Text = "Translator OSD";
      this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // MenuForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(256, 128);
      this.ControlBox = false;
      this.Controls.Add(this.listViewMenu);
      this.Controls.Add(this.labelHeader);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(512, 512);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(256, 128);
      this.Name = "MenuForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MenuForm";
      this.TopMost = true;
      this.Deactivate += new System.EventHandler(this.MenuForm_Deactivate);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MenuForm_FormClosed);
      this.Shown += new System.EventHandler(this.MenuForm_Shown);
      this.Load += new System.EventHandler(this.MenuForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listViewMenu;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.Label labelHeader;
    private System.Windows.Forms.ColumnHeader columnHeader;
  }
}