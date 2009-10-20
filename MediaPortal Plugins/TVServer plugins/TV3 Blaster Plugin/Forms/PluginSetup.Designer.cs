namespace TvEngine.Forms
{
  partial class PluginSetup
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
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageSetup = new System.Windows.Forms.TabPage();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.buttonHostSetup = new System.Windows.Forms.Button();
      this.buttonSTB = new System.Windows.Forms.Button();
      this.tabPageIR = new System.Windows.Forms.TabPage();
      this.listViewIR = new System.Windows.Forms.ListView();
      this.buttonNewIR = new System.Windows.Forms.Button();
      this.buttonDeleteIR = new System.Windows.Forms.Button();
      this.buttonEditIR = new System.Windows.Forms.Button();
      this.tabPageMacros = new System.Windows.Forms.TabPage();
      this.listViewMacro = new System.Windows.Forms.ListView();
      this.buttonTestMacro = new System.Windows.Forms.Button();
      this.buttonDeleteMacro = new System.Windows.Forms.Button();
      this.buttonNewMacro = new System.Windows.Forms.Button();
      this.buttonEditMacro = new System.Windows.Forms.Button();
      this.tabControl.SuspendLayout();
      this.tabPageSetup.SuspendLayout();
      this.tabPageIR.SuspendLayout();
      this.tabPageMacros.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                      | System.Windows.Forms.AnchorStyles.Left)
                                                                     | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.tabPageSetup);
      this.tabControl.Controls.Add(this.tabPageMacros);
      this.tabControl.Controls.Add(this.tabPageIR);
      this.tabControl.Location = new System.Drawing.Point(8, 8);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(312, 248);
      this.tabControl.TabIndex = 0;
      // 
      // tabPageSetup
      // 
      this.tabPageSetup.Controls.Add(this.buttonHelp);
      this.tabPageSetup.Controls.Add(this.buttonHostSetup);
      this.tabPageSetup.Controls.Add(this.buttonSTB);
      this.tabPageSetup.Location = new System.Drawing.Point(4, 22);
      this.tabPageSetup.Name = "tabPageSetup";
      this.tabPageSetup.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageSetup.Size = new System.Drawing.Size(304, 222);
      this.tabPageSetup.TabIndex = 0;
      this.tabPageSetup.Text = "Plugin Setup";
      this.tabPageSetup.UseVisualStyleBackColor = true;
      // 
      // buttonHelp
      // 
      this.buttonHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.buttonHelp.Location = new System.Drawing.Point(128, 192);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(56, 24);
      this.buttonHelp.TabIndex = 2;
      this.buttonHelp.Text = "Help";
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // buttonHostSetup
      // 
      this.buttonHostSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHostSetup.Location = new System.Drawing.Point(8, 192);
      this.buttonHostSetup.Name = "buttonHostSetup";
      this.buttonHostSetup.Size = new System.Drawing.Size(96, 24);
      this.buttonHostSetup.TabIndex = 1;
      this.buttonHostSetup.Text = "Change Server";
      this.buttonHostSetup.UseVisualStyleBackColor = true;
      this.buttonHostSetup.Click += new System.EventHandler(this.buttonHostSetup_Click);
      // 
      // buttonSTB
      // 
      this.buttonSTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSTB.Location = new System.Drawing.Point(216, 192);
      this.buttonSTB.Name = "buttonSTB";
      this.buttonSTB.Size = new System.Drawing.Size(80, 24);
      this.buttonSTB.TabIndex = 3;
      this.buttonSTB.Text = "STB Setup";
      this.buttonSTB.UseVisualStyleBackColor = true;
      this.buttonSTB.Click += new System.EventHandler(this.buttonSTB_Click);
      // 
      // tabPageIR
      // 
      this.tabPageIR.Controls.Add(this.listViewIR);
      this.tabPageIR.Controls.Add(this.buttonNewIR);
      this.tabPageIR.Controls.Add(this.buttonDeleteIR);
      this.tabPageIR.Controls.Add(this.buttonEditIR);
      this.tabPageIR.Location = new System.Drawing.Point(4, 22);
      this.tabPageIR.Name = "tabPageIR";
      this.tabPageIR.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageIR.Size = new System.Drawing.Size(304, 222);
      this.tabPageIR.TabIndex = 1;
      this.tabPageIR.Text = "IR Commands";
      this.tabPageIR.UseVisualStyleBackColor = true;
      // 
      // listViewIR
      // 
      this.listViewIR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                      | System.Windows.Forms.AnchorStyles.Left)
                                                                     | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewIR.FullRowSelect = true;
      this.listViewIR.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewIR.HideSelection = false;
      this.listViewIR.LabelEdit = true;
      this.listViewIR.Location = new System.Drawing.Point(8, 8);
      this.listViewIR.MultiSelect = false;
      this.listViewIR.Name = "listViewIR";
      this.listViewIR.Size = new System.Drawing.Size(288, 176);
      this.listViewIR.TabIndex = 0;
      this.listViewIR.UseCompatibleStateImageBehavior = false;
      this.listViewIR.View = System.Windows.Forms.View.List;
      this.listViewIR.DoubleClick += new System.EventHandler(this.listViewIR_DoubleClick);
      this.listViewIR.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewIR_AfterLabelEdit);
      // 
      // buttonNewIR
      // 
      this.buttonNewIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewIR.Location = new System.Drawing.Point(8, 192);
      this.buttonNewIR.Name = "buttonNewIR";
      this.buttonNewIR.Size = new System.Drawing.Size(56, 24);
      this.buttonNewIR.TabIndex = 1;
      this.buttonNewIR.Text = "New";
      this.buttonNewIR.UseVisualStyleBackColor = true;
      this.buttonNewIR.Click += new System.EventHandler(this.buttonNewIR_Click);
      // 
      // buttonDeleteIR
      // 
      this.buttonDeleteIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDeleteIR.Location = new System.Drawing.Point(136, 192);
      this.buttonDeleteIR.Name = "buttonDeleteIR";
      this.buttonDeleteIR.Size = new System.Drawing.Size(56, 24);
      this.buttonDeleteIR.TabIndex = 3;
      this.buttonDeleteIR.Text = "Delete";
      this.buttonDeleteIR.UseVisualStyleBackColor = true;
      this.buttonDeleteIR.Click += new System.EventHandler(this.buttonDeleteIR_Click);
      // 
      // buttonEditIR
      // 
      this.buttonEditIR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEditIR.Location = new System.Drawing.Point(72, 192);
      this.buttonEditIR.Name = "buttonEditIR";
      this.buttonEditIR.Size = new System.Drawing.Size(56, 24);
      this.buttonEditIR.TabIndex = 2;
      this.buttonEditIR.Text = "Edit";
      this.buttonEditIR.UseVisualStyleBackColor = true;
      this.buttonEditIR.Click += new System.EventHandler(this.buttonEditIR_Click);
      // 
      // tabPageMacros
      // 
      this.tabPageMacros.Controls.Add(this.listViewMacro);
      this.tabPageMacros.Controls.Add(this.buttonTestMacro);
      this.tabPageMacros.Controls.Add(this.buttonDeleteMacro);
      this.tabPageMacros.Controls.Add(this.buttonNewMacro);
      this.tabPageMacros.Controls.Add(this.buttonEditMacro);
      this.tabPageMacros.Location = new System.Drawing.Point(4, 22);
      this.tabPageMacros.Name = "tabPageMacros";
      this.tabPageMacros.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageMacros.Size = new System.Drawing.Size(304, 222);
      this.tabPageMacros.TabIndex = 2;
      this.tabPageMacros.Text = "Macros";
      this.tabPageMacros.UseVisualStyleBackColor = true;
      // 
      // listViewMacro
      // 
      this.listViewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                         | System.Windows.Forms.AnchorStyles.Left)
                                                                        | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewMacro.FullRowSelect = true;
      this.listViewMacro.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewMacro.HideSelection = false;
      this.listViewMacro.LabelEdit = true;
      this.listViewMacro.Location = new System.Drawing.Point(8, 8);
      this.listViewMacro.MultiSelect = false;
      this.listViewMacro.Name = "listViewMacro";
      this.listViewMacro.Size = new System.Drawing.Size(288, 176);
      this.listViewMacro.TabIndex = 0;
      this.listViewMacro.UseCompatibleStateImageBehavior = false;
      this.listViewMacro.View = System.Windows.Forms.View.List;
      this.listViewMacro.DoubleClick += new System.EventHandler(this.listViewMacro_DoubleClick);
      this.listViewMacro.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMacro_AfterLabelEdit);
      // 
      // buttonTestMacro
      // 
      this.buttonTestMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTestMacro.Location = new System.Drawing.Point(208, 192);
      this.buttonTestMacro.Name = "buttonTestMacro";
      this.buttonTestMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonTestMacro.TabIndex = 4;
      this.buttonTestMacro.Text = "Test";
      this.buttonTestMacro.UseVisualStyleBackColor = true;
      this.buttonTestMacro.Click += new System.EventHandler(this.buttonTestMacro_Click);
      // 
      // buttonDeleteMacro
      // 
      this.buttonDeleteMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDeleteMacro.Location = new System.Drawing.Point(136, 192);
      this.buttonDeleteMacro.Name = "buttonDeleteMacro";
      this.buttonDeleteMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonDeleteMacro.TabIndex = 3;
      this.buttonDeleteMacro.Text = "Delete";
      this.buttonDeleteMacro.UseVisualStyleBackColor = true;
      this.buttonDeleteMacro.Click += new System.EventHandler(this.buttonDeleteMacro_Click);
      // 
      // buttonNewMacro
      // 
      this.buttonNewMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonNewMacro.Location = new System.Drawing.Point(8, 192);
      this.buttonNewMacro.Name = "buttonNewMacro";
      this.buttonNewMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonNewMacro.TabIndex = 1;
      this.buttonNewMacro.Text = "New";
      this.buttonNewMacro.UseVisualStyleBackColor = true;
      this.buttonNewMacro.Click += new System.EventHandler(this.buttonNewMacro_Click);
      // 
      // buttonEditMacro
      // 
      this.buttonEditMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonEditMacro.Location = new System.Drawing.Point(72, 192);
      this.buttonEditMacro.Name = "buttonEditMacro";
      this.buttonEditMacro.Size = new System.Drawing.Size(56, 24);
      this.buttonEditMacro.TabIndex = 2;
      this.buttonEditMacro.Text = "Edit";
      this.buttonEditMacro.UseVisualStyleBackColor = true;
      this.buttonEditMacro.Click += new System.EventHandler(this.buttonEditMacro_Click);
      // 
      // PluginSetup
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.tabControl);
      this.MinimumSize = new System.Drawing.Size(328, 266);
      this.Name = "PluginSetup";
      this.Size = new System.Drawing.Size(328, 266);
      this.tabControl.ResumeLayout(false);
      this.tabPageSetup.ResumeLayout(false);
      this.tabPageSetup.PerformLayout();
      this.tabPageIR.ResumeLayout(false);
      this.tabPageMacros.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageIR;
    private System.Windows.Forms.TabPage tabPageMacros;
    private System.Windows.Forms.Button buttonNewIR;
    private System.Windows.Forms.Button buttonDeleteIR;
    private System.Windows.Forms.Button buttonEditIR;
    private System.Windows.Forms.Button buttonTestMacro;
    private System.Windows.Forms.Button buttonDeleteMacro;
    private System.Windows.Forms.Button buttonNewMacro;
    private System.Windows.Forms.Button buttonEditMacro;
    private System.Windows.Forms.Button buttonSTB;
    private System.Windows.Forms.TabPage tabPageSetup;
    private System.Windows.Forms.Button buttonHostSetup;
    private System.Windows.Forms.Button buttonHelp;
    private System.Windows.Forms.ListView listViewIR;
    private System.Windows.Forms.ListView listViewMacro;


  }
}