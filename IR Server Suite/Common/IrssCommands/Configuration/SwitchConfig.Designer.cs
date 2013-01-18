namespace IrssCommands
{
  partial class SwitchConfig
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
      this.groupBoxDefaultCaseGoto = new System.Windows.Forms.GroupBox();
      this.textBoxDefaultCase = new System.Windows.Forms.TextBox();
      this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
      this.toolStripCases = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonDeleteAll = new System.Windows.Forms.ToolStripButton();
      this.groupBoxCases = new System.Windows.Forms.GroupBox();
      this.listViewCases = new System.Windows.Forms.ListView();
      this.columnHeaderCase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeaderGoto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBoxSwitchVariable = new System.Windows.Forms.GroupBox();
      this.textBoxSwitchVar = new System.Windows.Forms.TextBox();
      this.labelVarPrefix = new System.Windows.Forms.Label();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.groupBoxDefaultCaseGoto.SuspendLayout();
      this.toolStripCases.SuspendLayout();
      this.groupBoxCases.SuspendLayout();
      this.groupBoxSwitchVariable.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxDefaultCaseGoto
      // 
      this.groupBoxDefaultCaseGoto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxDefaultCaseGoto.Controls.Add(this.textBoxDefaultCase);
      this.groupBoxDefaultCaseGoto.Location = new System.Drawing.Point(3, 207);
      this.groupBoxDefaultCaseGoto.Name = "groupBoxDefaultCaseGoto";
      this.groupBoxDefaultCaseGoto.Size = new System.Drawing.Size(269, 48);
      this.groupBoxDefaultCaseGoto.TabIndex = 7;
      this.groupBoxDefaultCaseGoto.TabStop = false;
      this.groupBoxDefaultCaseGoto.Text = "Default Case Goto";
      // 
      // textBoxDefaultCase
      // 
      this.textBoxDefaultCase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxDefaultCase.Location = new System.Drawing.Point(8, 16);
      this.textBoxDefaultCase.Name = "textBoxDefaultCase";
      this.textBoxDefaultCase.Size = new System.Drawing.Size(253, 20);
      this.textBoxDefaultCase.TabIndex = 0;
      this.toolTips.SetToolTip(this.textBoxDefaultCase, "If no other case is applicable then the default case is used");
      // 
      // toolStripButtonDelete
      // 
      this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDelete.Name = "toolStripButtonDelete";
      this.toolStripButtonDelete.Size = new System.Drawing.Size(44, 22);
      this.toolStripButtonDelete.Text = "Delete";
      // 
      // toolStripButtonEdit
      // 
      this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEdit.Name = "toolStripButtonEdit";
      this.toolStripButtonEdit.Size = new System.Drawing.Size(31, 22);
      this.toolStripButtonEdit.Text = "Edit";
      // 
      // toolStripButtonAdd
      // 
      this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAdd.Name = "toolStripButtonAdd";
      this.toolStripButtonAdd.Size = new System.Drawing.Size(33, 22);
      this.toolStripButtonAdd.Text = "Add";
      // 
      // toolStripCases
      // 
      this.toolStripCases.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.toolStripCases.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStripCases.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonDeleteAll});
      this.toolStripCases.Location = new System.Drawing.Point(3, 114);
      this.toolStripCases.Name = "toolStripCases";
      this.toolStripCases.Size = new System.Drawing.Size(263, 25);
      this.toolStripCases.TabIndex = 0;
      this.toolStripCases.Text = "Cases";
      // 
      // toolStripButtonDeleteAll
      // 
      this.toolStripButtonDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteAll.Name = "toolStripButtonDeleteAll";
      this.toolStripButtonDeleteAll.Size = new System.Drawing.Size(61, 22);
      this.toolStripButtonDeleteAll.Text = "Delete All";
      // 
      // groupBoxCases
      // 
      this.groupBoxCases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCases.Controls.Add(this.listViewCases);
      this.groupBoxCases.Controls.Add(this.toolStripCases);
      this.groupBoxCases.Location = new System.Drawing.Point(3, 59);
      this.groupBoxCases.Name = "groupBoxCases";
      this.groupBoxCases.Size = new System.Drawing.Size(269, 142);
      this.groupBoxCases.TabIndex = 6;
      this.groupBoxCases.TabStop = false;
      this.groupBoxCases.Text = "Cases";
      // 
      // listViewCases
      // 
      this.listViewCases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCase,
            this.columnHeaderGoto});
      this.listViewCases.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewCases.Location = new System.Drawing.Point(3, 16);
      this.listViewCases.Name = "listViewCases";
      this.listViewCases.Size = new System.Drawing.Size(263, 98);
      this.listViewCases.TabIndex = 1;
      this.listViewCases.UseCompatibleStateImageBehavior = false;
      this.listViewCases.View = System.Windows.Forms.View.Details;
      // 
      // columnHeaderCase
      // 
      this.columnHeaderCase.Text = "Case";
      this.columnHeaderCase.Width = 128;
      // 
      // columnHeaderGoto
      // 
      this.columnHeaderGoto.Text = "Goto";
      this.columnHeaderGoto.Width = 128;
      // 
      // groupBoxSwitchVariable
      // 
      this.groupBoxSwitchVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSwitchVariable.Controls.Add(this.textBoxSwitchVar);
      this.groupBoxSwitchVariable.Controls.Add(this.labelVarPrefix);
      this.groupBoxSwitchVariable.Location = new System.Drawing.Point(3, 3);
      this.groupBoxSwitchVariable.Name = "groupBoxSwitchVariable";
      this.groupBoxSwitchVariable.Size = new System.Drawing.Size(269, 48);
      this.groupBoxSwitchVariable.TabIndex = 5;
      this.groupBoxSwitchVariable.TabStop = false;
      this.groupBoxSwitchVariable.Text = "Switch Variable";
      // 
      // textBoxSwitchVar
      // 
      this.textBoxSwitchVar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSwitchVar.Location = new System.Drawing.Point(40, 16);
      this.textBoxSwitchVar.Name = "textBoxSwitchVar";
      this.textBoxSwitchVar.Size = new System.Drawing.Size(221, 20);
      this.textBoxSwitchVar.TabIndex = 1;
      this.toolTips.SetToolTip(this.textBoxSwitchVar, "The value to check cases with");
      // 
      // labelVarPrefix
      // 
      this.labelVarPrefix.Location = new System.Drawing.Point(8, 16);
      this.labelVarPrefix.Name = "labelVarPrefix";
      this.labelVarPrefix.Size = new System.Drawing.Size(32, 20);
      this.labelVarPrefix.TabIndex = 0;
      this.labelVarPrefix.Text = "var_";
      this.labelVarPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // SwitchConfig
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxDefaultCaseGoto);
      this.Controls.Add(this.groupBoxCases);
      this.Controls.Add(this.groupBoxSwitchVariable);
      this.MaximumSize = new System.Drawing.Size(544, 393);
      this.MinimumSize = new System.Drawing.Size(162, 84);
      this.Name = "SwitchConfig";
      this.Size = new System.Drawing.Size(275, 258);
      this.groupBoxDefaultCaseGoto.ResumeLayout(false);
      this.groupBoxDefaultCaseGoto.PerformLayout();
      this.toolStripCases.ResumeLayout(false);
      this.toolStripCases.PerformLayout();
      this.groupBoxCases.ResumeLayout(false);
      this.groupBoxCases.PerformLayout();
      this.groupBoxSwitchVariable.ResumeLayout(false);
      this.groupBoxSwitchVariable.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxDefaultCaseGoto;
    private System.Windows.Forms.TextBox textBoxDefaultCase;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
    private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
    private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
    private System.Windows.Forms.ToolStrip toolStripCases;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteAll;
    private System.Windows.Forms.GroupBox groupBoxCases;
    private System.Windows.Forms.ListView listViewCases;
    private System.Windows.Forms.ColumnHeader columnHeaderCase;
    private System.Windows.Forms.ColumnHeader columnHeaderGoto;
    private System.Windows.Forms.GroupBox groupBoxSwitchVariable;
    private System.Windows.Forms.TextBox textBoxSwitchVar;
    private System.Windows.Forms.Label labelVarPrefix;




  }
}
