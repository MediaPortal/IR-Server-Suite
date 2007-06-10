namespace SkinEditor
{
  partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.panelRemote = new System.Windows.Forms.Panel();
      this.pictureBoxRemote = new System.Windows.Forms.PictureBox();
      this.listViewButtons = new System.Windows.Forms.ListView();
      this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderCode = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderShortcut = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderTop = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderLeft = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderHeight = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderWidth = new System.Windows.Forms.ColumnHeader();
      this.labelDivide = new System.Windows.Forms.Label();
      this.buttonLoadImage = new System.Windows.Forms.Button();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.iRServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.changeServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.timerFlash = new System.Windows.Forms.Timer(this.components);
      this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
      this.buttonAddButton = new System.Windows.Forms.Button();
      this.textBoxCode = new System.Windows.Forms.TextBox();
      this.buttonSetCode = new System.Windows.Forms.Button();
      this.buttonSetShortcut = new System.Windows.Forms.Button();
      this.comboBoxShortcut = new System.Windows.Forms.ComboBox();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.panelRemote.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRemote)).BeginInit();
      this.menuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelRemote
      // 
      this.panelRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelRemote.AutoScroll = true;
      this.panelRemote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.panelRemote.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelRemote.Controls.Add(this.pictureBoxRemote);
      this.panelRemote.Location = new System.Drawing.Point(8, 32);
      this.panelRemote.Name = "panelRemote";
      this.panelRemote.Size = new System.Drawing.Size(256, 448);
      this.panelRemote.TabIndex = 1;
      // 
      // pictureBoxRemote
      // 
      this.pictureBoxRemote.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxRemote.Name = "pictureBoxRemote";
      this.pictureBoxRemote.Size = new System.Drawing.Size(24, 24);
      this.pictureBoxRemote.TabIndex = 2;
      this.pictureBoxRemote.TabStop = false;
      this.pictureBoxRemote.Visible = false;
      // 
      // listViewButtons
      // 
      this.listViewButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewButtons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderCode,
            this.columnHeaderShortcut,
            this.columnHeaderTop,
            this.columnHeaderLeft,
            this.columnHeaderHeight,
            this.columnHeaderWidth});
      this.listViewButtons.FullRowSelect = true;
      this.listViewButtons.GridLines = true;
      this.listViewButtons.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewButtons.HideSelection = false;
      this.listViewButtons.LabelEdit = true;
      this.listViewButtons.LabelWrap = false;
      this.listViewButtons.Location = new System.Drawing.Point(280, 32);
      this.listViewButtons.MultiSelect = false;
      this.listViewButtons.Name = "listViewButtons";
      this.listViewButtons.ShowGroups = false;
      this.listViewButtons.Size = new System.Drawing.Size(504, 448);
      this.listViewButtons.TabIndex = 3;
      this.listViewButtons.UseCompatibleStateImageBehavior = false;
      this.listViewButtons.View = System.Windows.Forms.View.Details;
      this.listViewButtons.SelectedIndexChanged += new System.EventHandler(this.listViewButtons_SelectedIndexChanged);
      this.listViewButtons.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewButtons_KeyDown);
      // 
      // columnHeaderName
      // 
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 90;
      // 
      // columnHeaderCode
      // 
      this.columnHeaderCode.Text = "Code";
      this.columnHeaderCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderCode.Width = 80;
      // 
      // columnHeaderShortcut
      // 
      this.columnHeaderShortcut.Text = "Shortcut";
      this.columnHeaderShortcut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderShortcut.Width = 110;
      // 
      // columnHeaderTop
      // 
      this.columnHeaderTop.Text = "Top";
      this.columnHeaderTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderTop.Width = 50;
      // 
      // columnHeaderLeft
      // 
      this.columnHeaderLeft.Text = "Left";
      this.columnHeaderLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderLeft.Width = 50;
      // 
      // columnHeaderHeight
      // 
      this.columnHeaderHeight.Text = "Height";
      this.columnHeaderHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderHeight.Width = 50;
      // 
      // columnHeaderWidth
      // 
      this.columnHeaderWidth.Text = "Width";
      this.columnHeaderWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeaderWidth.Width = 50;
      // 
      // labelDivide
      // 
      this.labelDivide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDivide.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.labelDivide.Location = new System.Drawing.Point(272, 32);
      this.labelDivide.Name = "labelDivide";
      this.labelDivide.Size = new System.Drawing.Size(2, 481);
      this.labelDivide.TabIndex = 2;
      this.labelDivide.Text = "-";
      // 
      // buttonLoadImage
      // 
      this.buttonLoadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonLoadImage.Location = new System.Drawing.Point(8, 488);
      this.buttonLoadImage.Name = "buttonLoadImage";
      this.buttonLoadImage.Size = new System.Drawing.Size(72, 24);
      this.buttonLoadImage.TabIndex = 4;
      this.buttonLoadImage.Text = "Load Image";
      this.buttonLoadImage.UseVisualStyleBackColor = true;
      this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
      // 
      // openFileDialog
      // 
      this.openFileDialog.Filter = "All Files|*.*";
      this.openFileDialog.Title = "Load Image ...";
      // 
      // menuStrip
      // 
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.iRServerToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(792, 24);
      this.menuStrip.TabIndex = 0;
      this.menuStrip.Text = "menu";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.quitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.newToolStripMenuItem.Text = "New";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.openToolStripMenuItem.Text = "Open";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.saveToolStripMenuItem.Text = "Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.saveAsToolStripMenuItem.Text = "Save as ...";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.closeToolStripMenuItem.Text = "Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // quitToolStripMenuItem
      // 
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.quitToolStripMenuItem.Text = "Quit";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      // 
      // iRServerToolStripMenuItem
      // 
      this.iRServerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.changeServerToolStripMenuItem});
      this.iRServerToolStripMenuItem.Name = "iRServerToolStripMenuItem";
      this.iRServerToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
      this.iRServerToolStripMenuItem.Text = "IR Server";
      // 
      // connectToolStripMenuItem
      // 
      this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
      this.connectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.connectToolStripMenuItem.Text = "Connect";
      this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
      // 
      // disconnectToolStripMenuItem
      // 
      this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
      this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.disconnectToolStripMenuItem.Text = "Disconnect";
      this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
      // 
      // changeServerToolStripMenuItem
      // 
      this.changeServerToolStripMenuItem.Name = "changeServerToolStripMenuItem";
      this.changeServerToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
      this.changeServerToolStripMenuItem.Text = "Change Server";
      this.changeServerToolStripMenuItem.Click += new System.EventHandler(this.changeServerToolStripMenuItem_Click);
      // 
      // timerFlash
      // 
      this.timerFlash.Interval = 750;
      this.timerFlash.Tick += new System.EventHandler(this.timerFlash_Tick);
      // 
      // saveFileDialog
      // 
      this.saveFileDialog.Filter = "XML Skin Files|*.xml";
      this.saveFileDialog.Title = "Save Skin File As ...";
      // 
      // buttonAddButton
      // 
      this.buttonAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAddButton.Location = new System.Drawing.Point(280, 488);
      this.buttonAddButton.Name = "buttonAddButton";
      this.buttonAddButton.Size = new System.Drawing.Size(72, 24);
      this.buttonAddButton.TabIndex = 5;
      this.buttonAddButton.Text = "Add";
      this.buttonAddButton.UseVisualStyleBackColor = true;
      this.buttonAddButton.Click += new System.EventHandler(this.buttonAddButton_Click);
      // 
      // textBoxCode
      // 
      this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxCode.Location = new System.Drawing.Point(448, 490);
      this.textBoxCode.Name = "textBoxCode";
      this.textBoxCode.Size = new System.Drawing.Size(120, 20);
      this.textBoxCode.TabIndex = 7;
      // 
      // buttonSetCode
      // 
      this.buttonSetCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSetCode.Location = new System.Drawing.Point(368, 488);
      this.buttonSetCode.Name = "buttonSetCode";
      this.buttonSetCode.Size = new System.Drawing.Size(72, 24);
      this.buttonSetCode.TabIndex = 6;
      this.buttonSetCode.Text = "Set Code:";
      this.buttonSetCode.UseVisualStyleBackColor = true;
      this.buttonSetCode.Click += new System.EventHandler(this.buttonSetCode_Click);
      // 
      // buttonSetShortcut
      // 
      this.buttonSetShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSetShortcut.Location = new System.Drawing.Point(584, 488);
      this.buttonSetShortcut.Name = "buttonSetShortcut";
      this.buttonSetShortcut.Size = new System.Drawing.Size(80, 24);
      this.buttonSetShortcut.TabIndex = 8;
      this.buttonSetShortcut.Text = "Set Shortcut:";
      this.buttonSetShortcut.UseVisualStyleBackColor = true;
      this.buttonSetShortcut.Click += new System.EventHandler(this.buttonSetShortcut_Click);
      // 
      // comboBoxShortcut
      // 
      this.comboBoxShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxShortcut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxShortcut.FormattingEnabled = true;
      this.comboBoxShortcut.Location = new System.Drawing.Point(672, 490);
      this.comboBoxShortcut.Name = "comboBoxShortcut";
      this.comboBoxShortcut.Size = new System.Drawing.Size(112, 21);
      this.comboBoxShortcut.TabIndex = 9;
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "Help";
      this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(792, 522);
      this.Controls.Add(this.comboBoxShortcut);
      this.Controls.Add(this.buttonSetShortcut);
      this.Controls.Add(this.buttonSetCode);
      this.Controls.Add(this.textBoxCode);
      this.Controls.Add(this.buttonAddButton);
      this.Controls.Add(this.buttonLoadImage);
      this.Controls.Add(this.labelDivide);
      this.Controls.Add(this.listViewButtons);
      this.Controls.Add(this.panelRemote);
      this.Controls.Add(this.menuStrip);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip;
      this.MinimumSize = new System.Drawing.Size(640, 396);
      this.Name = "MainForm";
      this.Text = "Virtual Remote Skin Editor";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.panelRemote.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRemote)).EndInit();
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panelRemote;
    private System.Windows.Forms.PictureBox pictureBoxRemote;
    private System.Windows.Forms.ListView listViewButtons;
    private System.Windows.Forms.ColumnHeader columnHeaderName;
    private System.Windows.Forms.ColumnHeader columnHeaderCode;
    private System.Windows.Forms.ColumnHeader columnHeaderShortcut;
    private System.Windows.Forms.ColumnHeader columnHeaderTop;
    private System.Windows.Forms.ColumnHeader columnHeaderLeft;
    private System.Windows.Forms.ColumnHeader columnHeaderHeight;
    private System.Windows.Forms.ColumnHeader columnHeaderWidth;
    private System.Windows.Forms.Label labelDivide;
    private System.Windows.Forms.Button buttonLoadImage;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.Timer timerFlash;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.Button buttonAddButton;
    private System.Windows.Forms.TextBox textBoxCode;
    private System.Windows.Forms.Button buttonSetCode;
    private System.Windows.Forms.Button buttonSetShortcut;
    private System.Windows.Forms.ComboBox comboBoxShortcut;
    private System.Windows.Forms.ToolStripMenuItem iRServerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem changeServerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
  }

}
