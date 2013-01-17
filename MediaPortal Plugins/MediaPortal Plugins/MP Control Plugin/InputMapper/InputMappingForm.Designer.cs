using System.Windows.Forms;
using MediaPortal.UserInterface.Controls;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin.InputMapper
{
  partial class InputMappingForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputMappingForm));
      this.treeMapping = new System.Windows.Forms.TreeView();
      this.buttonDefault = new System.Windows.Forms.Button();
      this.buttonApply = new System.Windows.Forms.Button();
      this.buttonOk = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.headerLabel = new MediaPortal.UserInterface.Controls.MPGradientLabel();
      this.groupBoxSound = new System.Windows.Forms.GroupBox();
      this.comboBoxSound = new System.Windows.Forms.ComboBox();
      this.groupBoxCondition = new System.Windows.Forms.GroupBox();
      this.conditionPanel = new System.Windows.Forms.Panel();
      this.conditionComboBox = new System.Windows.Forms.ComboBox();
      this.groupBoxLayer = new System.Windows.Forms.GroupBox();
      this.comboBoxLayer = new System.Windows.Forms.ComboBox();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.groupBoxMapping = new System.Windows.Forms.GroupBox();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.expandToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.collapseToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.upToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.downToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.groupBoxCommand = new System.Windows.Forms.GroupBox();
      this.treeViewCommandList = new System.Windows.Forms.TreeView();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.groupBoxSound.SuspendLayout();
      this.groupBoxCondition.SuspendLayout();
      this.groupBoxLayer.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.RightToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.groupBoxMapping.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.groupBoxCommand.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeMapping
      // 
      this.treeMapping.AllowDrop = true;
      this.treeMapping.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeMapping.FullRowSelect = true;
      this.treeMapping.HideSelection = false;
      this.treeMapping.Location = new System.Drawing.Point(3, 16);
      this.treeMapping.Name = "treeMapping";
      this.treeMapping.Size = new System.Drawing.Size(383, 467);
      this.treeMapping.TabIndex = 0;
      this.treeMapping.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMapping_AfterSelect);
      this.treeMapping.DoubleClick += new System.EventHandler(this.treeMapping_DoubleClick);
      // 
      // buttonDefault
      // 
      this.buttonDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDefault.Location = new System.Drawing.Point(439, 563);
      this.buttonDefault.Name = "buttonDefault";
      this.buttonDefault.Size = new System.Drawing.Size(75, 23);
      this.buttonDefault.TabIndex = 11;
      this.buttonDefault.Text = "Reset";
      this.buttonDefault.UseVisualStyleBackColor = true;
      this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
      // 
      // buttonApply
      // 
      this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonApply.Location = new System.Drawing.Point(518, 563);
      this.buttonApply.Name = "buttonApply";
      this.buttonApply.Size = new System.Drawing.Size(75, 23);
      this.buttonApply.TabIndex = 12;
      this.buttonApply.Text = "Apply";
      this.buttonApply.UseVisualStyleBackColor = true;
      this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
      // 
      // buttonOk
      // 
      this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOk.Location = new System.Drawing.Point(597, 563);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new System.Drawing.Size(75, 23);
      this.buttonOk.TabIndex = 13;
      this.buttonOk.Text = "OK";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(676, 563);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 14;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // headerLabel
      // 
      this.headerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.headerLabel.Caption = "";
      this.headerLabel.FirstColor = System.Drawing.SystemColors.InactiveCaption;
      this.headerLabel.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.headerLabel.LastColor = System.Drawing.Color.WhiteSmoke;
      this.headerLabel.Location = new System.Drawing.Point(16, 16);
      this.headerLabel.Name = "headerLabel";
      this.headerLabel.PaddingLeft = 2;
      this.headerLabel.Size = new System.Drawing.Size(729, 24);
      this.headerLabel.TabIndex = 15;
      this.headerLabel.TextColor = System.Drawing.Color.WhiteSmoke;
      this.headerLabel.TextFont = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      // 
      // groupBoxSound
      // 
      this.groupBoxSound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSound.Controls.Add(this.comboBoxSound);
      this.groupBoxSound.Enabled = false;
      this.groupBoxSound.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBoxSound.Location = new System.Drawing.Point(0, 246);
      this.groupBoxSound.Name = "groupBoxSound";
      this.groupBoxSound.Size = new System.Drawing.Size(281, 48);
      this.groupBoxSound.TabIndex = 8;
      this.groupBoxSound.TabStop = false;
      this.groupBoxSound.Text = "Sound";
      // 
      // comboBoxSound
      // 
      this.comboBoxSound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxSound.ForeColor = System.Drawing.Color.DarkRed;
      this.comboBoxSound.Location = new System.Drawing.Point(6, 19);
      this.comboBoxSound.Name = "comboBoxSound";
      this.comboBoxSound.Size = new System.Drawing.Size(269, 21);
      this.comboBoxSound.TabIndex = 12;
      this.comboBoxSound.SelectionChangeCommitted += new System.EventHandler(this.SetSound);
      // 
      // groupBoxCondition
      // 
      this.groupBoxCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCondition.Controls.Add(this.conditionPanel);
      this.groupBoxCondition.Controls.Add(this.conditionComboBox);
      this.groupBoxCondition.Enabled = false;
      this.groupBoxCondition.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBoxCondition.Location = new System.Drawing.Point(0, 55);
      this.groupBoxCondition.Name = "groupBoxCondition";
      this.groupBoxCondition.Size = new System.Drawing.Size(281, 159);
      this.groupBoxCondition.TabIndex = 7;
      this.groupBoxCondition.TabStop = false;
      this.groupBoxCondition.Text = "Condition";
      // 
      // conditionPanel
      // 
      this.conditionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.conditionPanel.Location = new System.Drawing.Point(6, 46);
      this.conditionPanel.Name = "conditionPanel";
      this.conditionPanel.Size = new System.Drawing.Size(269, 107);
      this.conditionPanel.TabIndex = 7;
      // 
      // conditionComboBox
      // 
      this.conditionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.conditionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.conditionComboBox.FormattingEnabled = true;
      this.conditionComboBox.Location = new System.Drawing.Point(6, 19);
      this.conditionComboBox.Name = "conditionComboBox";
      this.conditionComboBox.Size = new System.Drawing.Size(269, 21);
      this.conditionComboBox.TabIndex = 6;
      this.conditionComboBox.SelectedIndexChanged += new System.EventHandler(this.conditionComboBox_SelectedIndexChanged);
      // 
      // groupBoxLayer
      // 
      this.groupBoxLayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxLayer.Controls.Add(this.comboBoxLayer);
      this.groupBoxLayer.Enabled = false;
      this.groupBoxLayer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBoxLayer.Location = new System.Drawing.Point(0, 0);
      this.groupBoxLayer.Name = "groupBoxLayer";
      this.groupBoxLayer.Size = new System.Drawing.Size(281, 49);
      this.groupBoxLayer.TabIndex = 6;
      this.groupBoxLayer.TabStop = false;
      this.groupBoxLayer.Text = "Layer";
      // 
      // comboBoxLayer
      // 
      this.comboBoxLayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxLayer.ForeColor = System.Drawing.Color.DimGray;
      this.comboBoxLayer.Location = new System.Drawing.Point(6, 19);
      this.comboBoxLayer.Name = "comboBoxLayer";
      this.comboBoxLayer.Size = new System.Drawing.Size(269, 21);
      this.comboBoxLayer.TabIndex = 1;
      this.comboBoxLayer.SelectionChangeCommitted += new System.EventHandler(this.SetLayer);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.Location = new System.Drawing.Point(16, 46);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
      this.splitContainer1.Size = new System.Drawing.Size(729, 511);
      this.splitContainer1.SplitterDistance = 444;
      this.splitContainer1.TabIndex = 16;
      // 
      // toolStripContainer1
      // 
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBoxMapping);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(389, 486);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      // 
      // toolStripContainer1.RightToolStripPanel
      // 
      this.toolStripContainer1.RightToolStripPanel.Controls.Add(this.toolStrip1);
      this.toolStripContainer1.Size = new System.Drawing.Size(444, 511);
      this.toolStripContainer1.TabIndex = 17;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // groupBoxMapping
      // 
      this.groupBoxMapping.Controls.Add(this.treeMapping);
      this.groupBoxMapping.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxMapping.Location = new System.Drawing.Point(0, 0);
      this.groupBoxMapping.Name = "groupBoxMapping";
      this.groupBoxMapping.Size = new System.Drawing.Size(389, 486);
      this.groupBoxMapping.TabIndex = 1;
      this.groupBoxMapping.TabStop = false;
      this.groupBoxMapping.Text = "Mapping (DoubleClick on command nodes to edit)";
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandToolStripButton,
            this.collapseToolStripButton,
            this.toolStripSeparator1,
            this.upToolStripButton,
            this.downToolStripButton,
            this.toolStripSeparator2,
            this.newToolStripButton,
            this.removeToolStripButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 3);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
      this.toolStrip1.Size = new System.Drawing.Size(55, 155);
      this.toolStrip1.TabIndex = 6;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // expandToolStripButton
      // 
      this.expandToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.expandToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("expandToolStripButton.Image")));
      this.expandToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.expandToolStripButton.Name = "expandToolStripButton";
      this.expandToolStripButton.Size = new System.Drawing.Size(53, 19);
      this.expandToolStripButton.Text = "+";
      this.expandToolStripButton.ToolTipText = "Expand all";
      this.expandToolStripButton.Click += new System.EventHandler(this.Expand);
      // 
      // collapseToolStripButton
      // 
      this.collapseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.collapseToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("collapseToolStripButton.Image")));
      this.collapseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.collapseToolStripButton.Name = "collapseToolStripButton";
      this.collapseToolStripButton.Size = new System.Drawing.Size(53, 19);
      this.collapseToolStripButton.Text = "-";
      this.collapseToolStripButton.ToolTipText = "Collapse all";
      this.collapseToolStripButton.Click += new System.EventHandler(this.Collapse);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(53, 6);
      // 
      // upToolStripButton
      // 
      this.upToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.upToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("upToolStripButton.Image")));
      this.upToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.upToolStripButton.Name = "upToolStripButton";
      this.upToolStripButton.Size = new System.Drawing.Size(53, 19);
      this.upToolStripButton.Text = "Up";
      this.upToolStripButton.Click += new System.EventHandler(this.buttonUp_Click);
      // 
      // downToolStripButton
      // 
      this.downToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.downToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("downToolStripButton.Image")));
      this.downToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.downToolStripButton.Name = "downToolStripButton";
      this.downToolStripButton.Size = new System.Drawing.Size(53, 19);
      this.downToolStripButton.Text = "Down";
      this.downToolStripButton.Click += new System.EventHandler(this.buttonDown_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(53, 6);
      // 
      // newToolStripButton
      // 
      this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
      this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newToolStripButton.Name = "newToolStripButton";
      this.newToolStripButton.Size = new System.Drawing.Size(53, 19);
      this.newToolStripButton.Text = "New";
      this.newToolStripButton.Click += new System.EventHandler(this.buttonNew_Click);
      // 
      // removeToolStripButton
      // 
      this.removeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
      this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.removeToolStripButton.Name = "removeToolStripButton";
      this.removeToolStripButton.Size = new System.Drawing.Size(53, 19);
      this.removeToolStripButton.Text = "Remove";
      this.removeToolStripButton.Click += new System.EventHandler(this.buttonRemove_Click);
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.groupBoxCondition);
      this.splitContainer2.Panel1.Controls.Add(this.groupBoxLayer);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.groupBoxSound);
      this.splitContainer2.Panel2.Controls.Add(this.groupBoxCommand);
      this.splitContainer2.Size = new System.Drawing.Size(281, 511);
      this.splitContainer2.SplitterDistance = 213;
      this.splitContainer2.TabIndex = 10;
      // 
      // groupBoxCommand
      // 
      this.groupBoxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCommand.Controls.Add(this.treeViewCommandList);
      this.groupBoxCommand.Location = new System.Drawing.Point(0, 3);
      this.groupBoxCommand.Name = "groupBoxCommand";
      this.groupBoxCommand.Size = new System.Drawing.Size(281, 237);
      this.groupBoxCommand.TabIndex = 9;
      this.groupBoxCommand.TabStop = false;
      this.groupBoxCommand.Text = "Command (DoubleClick to assign a new command)";
      // 
      // treeViewCommandList
      // 
      this.treeViewCommandList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeViewCommandList.FullRowSelect = true;
      this.treeViewCommandList.Location = new System.Drawing.Point(3, 16);
      this.treeViewCommandList.Name = "treeViewCommandList";
      this.treeViewCommandList.Size = new System.Drawing.Size(275, 218);
      this.treeViewCommandList.TabIndex = 14;
      this.treeViewCommandList.DoubleClick += new System.EventHandler(this.treeViewCommandList_DoubleClick);
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHelp.Location = new System.Drawing.Point(12, 563);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 23);
      this.buttonHelp.TabIndex = 17;
      this.buttonHelp.Text = "&Help";
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // InputMappingForm
      // 
      this.AcceptButton = this.buttonOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScroll = true;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(761, 596);
      this.Controls.Add(this.buttonHelp);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.buttonDefault);
      this.Controls.Add(this.buttonApply);
      this.Controls.Add(this.buttonOk);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.headerLabel);
      this.MinimumSize = new System.Drawing.Size(598, 509);
      this.Name = "InputMappingForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MediaPortal - Setup";
      this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.InputMappingForm_HelpRequested);
      this.groupBoxSound.ResumeLayout(false);
      this.groupBoxCondition.ResumeLayout(false);
      this.groupBoxLayer.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.RightToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.RightToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.groupBoxMapping.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.ResumeLayout(false);
      this.groupBoxCommand.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private Button buttonApply;
    private Button buttonCancel;
    private Button buttonDefault;
    private Button buttonOk;
    private ComboBox comboBoxLayer;
    private GroupBox groupBoxSound;
    private GroupBox groupBoxCondition;
    private GroupBox groupBoxLayer;
    private MPGradientLabel headerLabel;
    private TreeView treeMapping;
    private SplitContainer splitContainer1;
    private ToolStrip toolStrip1;
    private ToolStripContainer toolStripContainer1;
    private ToolStripButton expandToolStripButton;
    private ToolStripButton collapseToolStripButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton upToolStripButton;
    private ToolStripButton downToolStripButton;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripButton newToolStripButton;
    private ToolStripButton removeToolStripButton;
    private GroupBox groupBoxCommand;
    private TreeView treeViewCommandList;
    private ComboBox conditionComboBox;
    private Panel conditionPanel;
    private ComboBox comboBoxSound;
    private SplitContainer splitContainer2;
    private GroupBox groupBoxMapping;
    private Button buttonHelp;
  }
}