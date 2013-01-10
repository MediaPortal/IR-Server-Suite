namespace IrssCommands
{

  partial class EditSwitch
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
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.toolTips = new System.Windows.Forms.ToolTip(this.components);
      this.textBoxSwitchVar = new System.Windows.Forms.TextBox();
      this.textBoxDefaultCase = new System.Windows.Forms.TextBox();
      this.labelVarPrefix = new System.Windows.Forms.Label();
      this.listViewCases = new System.Windows.Forms.ListView();
      this.columnHeaderCase = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderGoto = new System.Windows.Forms.ColumnHeader();
      this.groupBoxSwitchVariable = new System.Windows.Forms.GroupBox();
      this.groupBoxCases = new System.Windows.Forms.GroupBox();
      this.toolStripCases = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripButtonDeleteAll = new System.Windows.Forms.ToolStripButton();
      this.groupBoxDefaultCaseGoto = new System.Windows.Forms.GroupBox();
      this.groupBoxSwitchVariable.SuspendLayout();
      this.groupBoxCases.SuspendLayout();
      this.toolStripCases.SuspendLayout();
      this.groupBoxDefaultCaseGoto.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(152, 344);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(224, 344);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // textBoxSwitchVar
      // 
      this.textBoxSwitchVar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSwitchVar.Location = new System.Drawing.Point(40, 16);
      this.textBoxSwitchVar.Name = "textBoxSwitchVar";
      this.textBoxSwitchVar.Size = new System.Drawing.Size(232, 20);
      this.textBoxSwitchVar.TabIndex = 1;
      this.toolTips.SetToolTip(this.textBoxSwitchVar, "The value to check cases with");
      // 
      // textBoxDefaultCase
      // 
      this.textBoxDefaultCase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxDefaultCase.Location = new System.Drawing.Point(8, 16);
      this.textBoxDefaultCase.Name = "textBoxDefaultCase";
      this.textBoxDefaultCase.Size = new System.Drawing.Size(264, 20);
      this.textBoxDefaultCase.TabIndex = 0;
      this.toolTips.SetToolTip(this.textBoxDefaultCase, "If no other case is applicable then the default case is used");
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
      // listViewCases
      // 
      this.listViewCases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCase,
            this.columnHeaderGoto});
      this.listViewCases.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewCases.Location = new System.Drawing.Point(3, 16);
      this.listViewCases.Name = "listViewCases";
      this.listViewCases.Size = new System.Drawing.Size(274, 172);
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
      this.groupBoxSwitchVariable.Location = new System.Drawing.Point(8, 8);
      this.groupBoxSwitchVariable.Name = "groupBoxSwitchVariable";
      this.groupBoxSwitchVariable.Size = new System.Drawing.Size(280, 48);
      this.groupBoxSwitchVariable.TabIndex = 0;
      this.groupBoxSwitchVariable.TabStop = false;
      this.groupBoxSwitchVariable.Text = "Switch Variable";
      // 
      // groupBoxCases
      // 
      this.groupBoxCases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCases.Controls.Add(this.listViewCases);
      this.groupBoxCases.Controls.Add(this.toolStripCases);
      this.groupBoxCases.Location = new System.Drawing.Point(8, 64);
      this.groupBoxCases.Name = "groupBoxCases";
      this.groupBoxCases.Size = new System.Drawing.Size(280, 216);
      this.groupBoxCases.TabIndex = 1;
      this.groupBoxCases.TabStop = false;
      this.groupBoxCases.Text = "Cases";
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
      this.toolStripCases.Location = new System.Drawing.Point(3, 188);
      this.toolStripCases.Name = "toolStripCases";
      this.toolStripCases.Size = new System.Drawing.Size(274, 25);
      this.toolStripCases.TabIndex = 0;
      this.toolStripCases.Text = "Cases";
      // 
      // toolStripButtonAdd
      // 
      this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonAdd.Image = global::IrssCommands.Properties.Resources.Plus;
      this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAdd.Name = "toolStripButtonAdd";
      this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonAdd.Text = "Add";
      this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
      // 
      // toolStripButtonEdit
      // 
      this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonEdit.Image = global::IrssCommands.Properties.Resources.Edit;
      this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonEdit.Name = "toolStripButtonEdit";
      this.toolStripButtonEdit.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonEdit.Text = "Edit";
      this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
      // 
      // toolStripButtonDelete
      // 
      this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonDelete.Image = global::IrssCommands.Properties.Resources.Delete;
      this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDelete.Name = "toolStripButtonDelete";
      this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonDelete.Text = "Delete";
      this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
      // 
      // toolStripButtonDeleteAll
      // 
      this.toolStripButtonDeleteAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonDeleteAll.Image = global::IrssCommands.Properties.Resources.DeleteAll;
      this.toolStripButtonDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonDeleteAll.Name = "toolStripButtonDeleteAll";
      this.toolStripButtonDeleteAll.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonDeleteAll.Text = "Delete All";
      this.toolStripButtonDeleteAll.Click += new System.EventHandler(this.toolStripButtonDeleteAll_Click);
      // 
      // groupBoxDefaultCaseGoto
      // 
      this.groupBoxDefaultCaseGoto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxDefaultCaseGoto.Controls.Add(this.textBoxDefaultCase);
      this.groupBoxDefaultCaseGoto.Location = new System.Drawing.Point(8, 288);
      this.groupBoxDefaultCaseGoto.Name = "groupBoxDefaultCaseGoto";
      this.groupBoxDefaultCaseGoto.Size = new System.Drawing.Size(280, 48);
      this.groupBoxDefaultCaseGoto.TabIndex = 2;
      this.groupBoxDefaultCaseGoto.TabStop = false;
      this.groupBoxDefaultCaseGoto.Text = "Default Case Goto";
      // 
      // EditSwitch
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(296, 377);
      this.Controls.Add(this.groupBoxDefaultCaseGoto);
      this.Controls.Add(this.groupBoxCases);
      this.Controls.Add(this.groupBoxSwitchVariable);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(304, 404);
      this.Name = "EditSwitch";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Switch Statement";
      this.groupBoxSwitchVariable.ResumeLayout(false);
      this.groupBoxSwitchVariable.PerformLayout();
      this.groupBoxCases.ResumeLayout(false);
      this.groupBoxCases.PerformLayout();
      this.toolStripCases.ResumeLayout(false);
      this.toolStripCases.PerformLayout();
      this.groupBoxDefaultCaseGoto.ResumeLayout(false);
      this.groupBoxDefaultCaseGoto.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ToolTip toolTips;
    private System.Windows.Forms.TextBox textBoxSwitchVar;
    private System.Windows.Forms.TextBox textBoxDefaultCase;
    private System.Windows.Forms.Label labelVarPrefix;
    private System.Windows.Forms.ListView listViewCases;
    private System.Windows.Forms.GroupBox groupBoxSwitchVariable;
    private System.Windows.Forms.GroupBox groupBoxCases;
    private System.Windows.Forms.ColumnHeader columnHeaderCase;
    private System.Windows.Forms.ColumnHeader columnHeaderGoto;
    private System.Windows.Forms.GroupBox groupBoxDefaultCaseGoto;
    private System.Windows.Forms.ToolStrip toolStripCases;
    private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
    private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
    private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
    private System.Windows.Forms.ToolStripButton toolStripButtonDeleteAll;
  }

}
