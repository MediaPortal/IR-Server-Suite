namespace Abstractor
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
      this.groupBoxSetup = new System.Windows.Forms.GroupBox();
      this.comboBoxComputer = new System.Windows.Forms.ComboBox();
      this.labelServerAddress = new System.Windows.Forms.Label();
      this.buttonConnect = new System.Windows.Forms.Button();
      this.buttonDisconnect = new System.Windows.Forms.Button();
      this.groupBoxStatus = new System.Windows.Forms.GroupBox();
      this.listBoxStatus = new System.Windows.Forms.ListBox();
      this.groupBoxMapAbstract = new System.Windows.Forms.GroupBox();
      this.buttonClear = new System.Windows.Forms.Button();
      this.buttonLoad = new System.Windows.Forms.Button();
      this.buttonSave = new System.Windows.Forms.Button();
      this.labelDevice = new System.Windows.Forms.Label();
      this.labelRemote = new System.Windows.Forms.Label();
      this.comboBoxDevice = new System.Windows.Forms.ComboBox();
      this.textBoxRemoteName = new System.Windows.Forms.TextBox();
      this.listViewButtonMap = new System.Windows.Forms.ListView();
      this.columnHeaderAbstractButton = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderKeyCode = new System.Windows.Forms.ColumnHeader();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.groupBoxSetup.SuspendLayout();
      this.groupBoxStatus.SuspendLayout();
      this.groupBoxMapAbstract.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxSetup
      // 
      this.groupBoxSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSetup.Controls.Add(this.comboBoxComputer);
      this.groupBoxSetup.Controls.Add(this.labelServerAddress);
      this.groupBoxSetup.Controls.Add(this.buttonConnect);
      this.groupBoxSetup.Controls.Add(this.buttonDisconnect);
      this.groupBoxSetup.Location = new System.Drawing.Point(8, 8);
      this.groupBoxSetup.Name = "groupBoxSetup";
      this.groupBoxSetup.Size = new System.Drawing.Size(440, 64);
      this.groupBoxSetup.TabIndex = 4;
      this.groupBoxSetup.TabStop = false;
      this.groupBoxSetup.Text = "Setup";
      // 
      // comboBoxComputer
      // 
      this.comboBoxComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.comboBoxComputer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboBoxComputer.FormattingEnabled = true;
      this.comboBoxComputer.Location = new System.Drawing.Point(8, 32);
      this.comboBoxComputer.Name = "comboBoxComputer";
      this.comboBoxComputer.Size = new System.Drawing.Size(240, 21);
      this.comboBoxComputer.TabIndex = 1;
      // 
      // labelServerAddress
      // 
      this.labelServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelServerAddress.Location = new System.Drawing.Point(8, 16);
      this.labelServerAddress.Name = "labelServerAddress";
      this.labelServerAddress.Size = new System.Drawing.Size(240, 16);
      this.labelServerAddress.TabIndex = 0;
      this.labelServerAddress.Text = "IR Server host computer:";
      // 
      // buttonConnect
      // 
      this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonConnect.Location = new System.Drawing.Point(264, 32);
      this.buttonConnect.Name = "buttonConnect";
      this.buttonConnect.Size = new System.Drawing.Size(80, 24);
      this.buttonConnect.TabIndex = 2;
      this.buttonConnect.Text = "Connect";
      this.buttonConnect.UseVisualStyleBackColor = true;
      this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
      // 
      // buttonDisconnect
      // 
      this.buttonDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDisconnect.Location = new System.Drawing.Point(352, 32);
      this.buttonDisconnect.Name = "buttonDisconnect";
      this.buttonDisconnect.Size = new System.Drawing.Size(80, 24);
      this.buttonDisconnect.TabIndex = 3;
      this.buttonDisconnect.Text = "Disconnect";
      this.buttonDisconnect.UseVisualStyleBackColor = true;
      this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
      // 
      // groupBoxStatus
      // 
      this.groupBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxStatus.Controls.Add(this.listBoxStatus);
      this.groupBoxStatus.Location = new System.Drawing.Point(8, 344);
      this.groupBoxStatus.Name = "groupBoxStatus";
      this.groupBoxStatus.Size = new System.Drawing.Size(440, 216);
      this.groupBoxStatus.TabIndex = 5;
      this.groupBoxStatus.TabStop = false;
      this.groupBoxStatus.Text = "Status";
      // 
      // listBoxStatus
      // 
      this.listBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxStatus.FormattingEnabled = true;
      this.listBoxStatus.HorizontalScrollbar = true;
      this.listBoxStatus.IntegralHeight = false;
      this.listBoxStatus.Location = new System.Drawing.Point(8, 16);
      this.listBoxStatus.Name = "listBoxStatus";
      this.listBoxStatus.ScrollAlwaysVisible = true;
      this.listBoxStatus.Size = new System.Drawing.Size(424, 185);
      this.listBoxStatus.TabIndex = 0;
      // 
      // groupBoxMapAbstract
      // 
      this.groupBoxMapAbstract.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMapAbstract.Controls.Add(this.checkBox1);
      this.groupBoxMapAbstract.Controls.Add(this.buttonClear);
      this.groupBoxMapAbstract.Controls.Add(this.buttonLoad);
      this.groupBoxMapAbstract.Controls.Add(this.buttonSave);
      this.groupBoxMapAbstract.Controls.Add(this.labelDevice);
      this.groupBoxMapAbstract.Controls.Add(this.labelRemote);
      this.groupBoxMapAbstract.Controls.Add(this.comboBoxDevice);
      this.groupBoxMapAbstract.Controls.Add(this.textBoxRemoteName);
      this.groupBoxMapAbstract.Controls.Add(this.listViewButtonMap);
      this.groupBoxMapAbstract.Location = new System.Drawing.Point(8, 80);
      this.groupBoxMapAbstract.Name = "groupBoxMapAbstract";
      this.groupBoxMapAbstract.Size = new System.Drawing.Size(440, 256);
      this.groupBoxMapAbstract.TabIndex = 6;
      this.groupBoxMapAbstract.TabStop = false;
      this.groupBoxMapAbstract.Text = "Abstract Remote Map";
      // 
      // buttonClear
      // 
      this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonClear.Location = new System.Drawing.Point(8, 224);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new System.Drawing.Size(80, 24);
      this.buttonClear.TabIndex = 7;
      this.buttonClear.Text = "Clear";
      this.buttonClear.UseVisualStyleBackColor = true;
      this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
      // 
      // buttonLoad
      // 
      this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonLoad.Location = new System.Drawing.Point(264, 224);
      this.buttonLoad.Name = "buttonLoad";
      this.buttonLoad.Size = new System.Drawing.Size(80, 24);
      this.buttonLoad.TabIndex = 5;
      this.buttonLoad.Text = "Load";
      this.buttonLoad.UseVisualStyleBackColor = true;
      this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
      // 
      // buttonSave
      // 
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(352, 224);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(80, 24);
      this.buttonSave.TabIndex = 6;
      this.buttonSave.Text = "Save";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      // 
      // labelDevice
      // 
      this.labelDevice.Location = new System.Drawing.Point(232, 24);
      this.labelDevice.Name = "labelDevice";
      this.labelDevice.Size = new System.Drawing.Size(72, 20);
      this.labelDevice.TabIndex = 4;
      this.labelDevice.Text = "Receiver:";
      // 
      // labelRemote
      // 
      this.labelRemote.Location = new System.Drawing.Point(8, 24);
      this.labelRemote.Name = "labelRemote";
      this.labelRemote.Size = new System.Drawing.Size(96, 20);
      this.labelRemote.TabIndex = 3;
      this.labelRemote.Text = "Remote name:";
      // 
      // comboBoxDevice
      // 
      this.comboBoxDevice.FormattingEnabled = true;
      this.comboBoxDevice.Location = new System.Drawing.Point(304, 24);
      this.comboBoxDevice.Name = "comboBoxDevice";
      this.comboBoxDevice.Size = new System.Drawing.Size(128, 21);
      this.comboBoxDevice.TabIndex = 2;
      // 
      // textBoxRemoteName
      // 
      this.textBoxRemoteName.Location = new System.Drawing.Point(104, 24);
      this.textBoxRemoteName.Name = "textBoxRemoteName";
      this.textBoxRemoteName.Size = new System.Drawing.Size(104, 20);
      this.textBoxRemoteName.TabIndex = 1;
      // 
      // listViewButtonMap
      // 
      this.listViewButtonMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewButtonMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAbstractButton,
            this.columnHeaderKeyCode});
      this.listViewButtonMap.FullRowSelect = true;
      this.listViewButtonMap.GridLines = true;
      this.listViewButtonMap.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewButtonMap.HideSelection = false;
      this.listViewButtonMap.Location = new System.Drawing.Point(8, 48);
      this.listViewButtonMap.MultiSelect = false;
      this.listViewButtonMap.Name = "listViewButtonMap";
      this.listViewButtonMap.Size = new System.Drawing.Size(424, 168);
      this.listViewButtonMap.TabIndex = 0;
      this.listViewButtonMap.UseCompatibleStateImageBehavior = false;
      this.listViewButtonMap.View = System.Windows.Forms.View.Details;
      this.listViewButtonMap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewButtonMap_KeyDown);
      // 
      // columnHeaderAbstractButton
      // 
      this.columnHeaderAbstractButton.Text = "AbstractButton";
      this.columnHeaderAbstractButton.Width = 116;
      // 
      // columnHeaderKeyCode
      // 
      this.columnHeaderKeyCode.Text = "KeyCode";
      this.columnHeaderKeyCode.Width = 288;
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(136, 224);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(85, 17);
      this.checkBox1.TabIndex = 8;
      this.checkBox1.Text = "Toggle Keys";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(456, 569);
      this.Controls.Add(this.groupBoxMapAbstract);
      this.Controls.Add(this.groupBoxSetup);
      this.Controls.Add(this.groupBoxStatus);
      this.Name = "MainForm";
      this.Text = "Abstractor";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.groupBoxSetup.ResumeLayout(false);
      this.groupBoxStatus.ResumeLayout(false);
      this.groupBoxMapAbstract.ResumeLayout(false);
      this.groupBoxMapAbstract.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxSetup;
    private System.Windows.Forms.ComboBox comboBoxComputer;
    private System.Windows.Forms.Label labelServerAddress;
    private System.Windows.Forms.Button buttonConnect;
    private System.Windows.Forms.Button buttonDisconnect;
    private System.Windows.Forms.GroupBox groupBoxStatus;
    private System.Windows.Forms.ListBox listBoxStatus;
    private System.Windows.Forms.GroupBox groupBoxMapAbstract;
    private System.Windows.Forms.Button buttonClear;
    private System.Windows.Forms.Button buttonLoad;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.Label labelDevice;
    private System.Windows.Forms.Label labelRemote;
    private System.Windows.Forms.ComboBox comboBoxDevice;
    private System.Windows.Forms.TextBox textBoxRemoteName;
    private System.Windows.Forms.ListView listViewButtonMap;
    private System.Windows.Forms.ColumnHeader columnHeaderAbstractButton;
    private System.Windows.Forms.ColumnHeader columnHeaderKeyCode;
    private System.Windows.Forms.CheckBox checkBox1;
  }
}

