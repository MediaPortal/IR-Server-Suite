namespace VirtualRemote
{

  partial class FormMain
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.MainMenu mainMenu;

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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemSetup = new System.Windows.Forms.MenuItem();
      this.menuItemQuit = new System.Windows.Forms.MenuItem();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageNavigation = new System.Windows.Forms.TabPage();
      this.pictureBoxStart = new System.Windows.Forms.PictureBox();
      this.pictureBoxMute = new System.Windows.Forms.PictureBox();
      this.pictureBoxVolumeUp = new System.Windows.Forms.PictureBox();
      this.pictureBoxVolumeDown = new System.Windows.Forms.PictureBox();
      this.pictureBoxChannelDown = new System.Windows.Forms.PictureBox();
      this.pictureBoxChannelUp = new System.Windows.Forms.PictureBox();
      this.pictureBoxRight = new System.Windows.Forms.PictureBox();
      this.pictureBoxLeft = new System.Windows.Forms.PictureBox();
      this.pictureBoxDown = new System.Windows.Forms.PictureBox();
      this.pictureBoxOK = new System.Windows.Forms.PictureBox();
      this.pictureBoxUp = new System.Windows.Forms.PictureBox();
      this.pictureBoxBack = new System.Windows.Forms.PictureBox();
      this.tabPagePlayback = new System.Windows.Forms.TabPage();
      this.pictureBoxNextChapter = new System.Windows.Forms.PictureBox();
      this.pictureBoxPreviousChapter = new System.Windows.Forms.PictureBox();
      this.pictureBoxRewind = new System.Windows.Forms.PictureBox();
      this.pictureBoxFastForward = new System.Windows.Forms.PictureBox();
      this.pictureBoxPause = new System.Windows.Forms.PictureBox();
      this.pictureBoxRecord = new System.Windows.Forms.PictureBox();
      this.pictureBoxStop = new System.Windows.Forms.PictureBox();
      this.pictureBoxPlay = new System.Windows.Forms.PictureBox();
      this.tabPageNumbers = new System.Windows.Forms.TabPage();
      this.pictureBoxEnter = new System.Windows.Forms.PictureBox();
      this.pictureBoxClear = new System.Windows.Forms.PictureBox();
      this.pictureBoxHash = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber0 = new System.Windows.Forms.PictureBox();
      this.pictureBoxStar = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber9 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber8 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber7 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber6 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber5 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber4 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber3 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber2 = new System.Windows.Forms.PictureBox();
      this.pictureBoxNumber1 = new System.Windows.Forms.PictureBox();
      this.tabPageMisc = new System.Windows.Forms.TabPage();
      this.pictureBoxPictures = new System.Windows.Forms.PictureBox();
      this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
      this.pictureBoxMusic = new System.Windows.Forms.PictureBox();
      this.pictureBoxDVD = new System.Windows.Forms.PictureBox();
      this.pictureBoxGuide = new System.Windows.Forms.PictureBox();
      this.pictureBoxTV = new System.Windows.Forms.PictureBox();
      this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
      this.pictureBoxPower = new System.Windows.Forms.PictureBox();
      this.pictureBoxFullscreen = new System.Windows.Forms.PictureBox();
      this.pictureBoxAspectRatio = new System.Windows.Forms.PictureBox();
      this.pictureBoxTeletext = new System.Windows.Forms.PictureBox();
      this.pictureBoxBlue = new System.Windows.Forms.PictureBox();
      this.pictureBoxYellow = new System.Windows.Forms.PictureBox();
      this.pictureBoxGreen = new System.Windows.Forms.PictureBox();
      this.pictureBoxRed = new System.Windows.Forms.PictureBox();
      this.notification = new Microsoft.WindowsCE.Forms.Notification();
      this.tabControl.SuspendLayout();
      this.tabPageNavigation.SuspendLayout();
      this.tabPagePlayback.SuspendLayout();
      this.tabPageNumbers.SuspendLayout();
      this.tabPageMisc.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemSetup);
      this.mainMenu.MenuItems.Add(this.menuItemQuit);
      // 
      // menuItemSetup
      // 
      this.menuItemSetup.Text = "Setup";
      this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
      // 
      // menuItemQuit
      // 
      this.menuItemQuit.Text = "Quit";
      this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.tabPageNavigation);
      this.tabControl.Controls.Add(this.tabPagePlayback);
      this.tabControl.Controls.Add(this.tabPageNumbers);
      this.tabControl.Controls.Add(this.tabPageMisc);
      this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(240, 268);
      this.tabControl.TabIndex = 0;
      // 
      // tabPageNavigation
      // 
      this.tabPageNavigation.AutoScroll = true;
      this.tabPageNavigation.Controls.Add(this.pictureBoxStart);
      this.tabPageNavigation.Controls.Add(this.pictureBoxMute);
      this.tabPageNavigation.Controls.Add(this.pictureBoxVolumeUp);
      this.tabPageNavigation.Controls.Add(this.pictureBoxVolumeDown);
      this.tabPageNavigation.Controls.Add(this.pictureBoxChannelDown);
      this.tabPageNavigation.Controls.Add(this.pictureBoxChannelUp);
      this.tabPageNavigation.Controls.Add(this.pictureBoxRight);
      this.tabPageNavigation.Controls.Add(this.pictureBoxLeft);
      this.tabPageNavigation.Controls.Add(this.pictureBoxDown);
      this.tabPageNavigation.Controls.Add(this.pictureBoxOK);
      this.tabPageNavigation.Controls.Add(this.pictureBoxUp);
      this.tabPageNavigation.Controls.Add(this.pictureBoxBack);
      this.tabPageNavigation.Location = new System.Drawing.Point(0, 0);
      this.tabPageNavigation.Name = "tabPageNavigation";
      this.tabPageNavigation.Size = new System.Drawing.Size(240, 245);
      this.tabPageNavigation.Text = "Navigation ";
      // 
      // pictureBoxStart
      // 
      this.pictureBoxStart.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxStart.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxStart.Image")));
      this.pictureBoxStart.Location = new System.Drawing.Point(168, 8);
      this.pictureBoxStart.Name = "pictureBoxStart";
      this.pictureBoxStart.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxStart.Tag = "Start";
      this.pictureBoxStart.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxStart.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxMute
      // 
      this.pictureBoxMute.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxMute.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMute.Image")));
      this.pictureBoxMute.Location = new System.Drawing.Point(88, 192);
      this.pictureBoxMute.Name = "pictureBoxMute";
      this.pictureBoxMute.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxMute.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxMute.Tag = "Mute";
      this.pictureBoxMute.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxMute.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxVolumeUp
      // 
      this.pictureBoxVolumeUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxVolumeUp.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxVolumeUp.Image")));
      this.pictureBoxVolumeUp.Location = new System.Drawing.Point(8, 136);
      this.pictureBoxVolumeUp.Name = "pictureBoxVolumeUp";
      this.pictureBoxVolumeUp.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxVolumeUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxVolumeUp.Tag = "VolumeUp";
      this.pictureBoxVolumeUp.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxVolumeUp.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxVolumeDown
      // 
      this.pictureBoxVolumeDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxVolumeDown.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxVolumeDown.Image")));
      this.pictureBoxVolumeDown.Location = new System.Drawing.Point(8, 192);
      this.pictureBoxVolumeDown.Name = "pictureBoxVolumeDown";
      this.pictureBoxVolumeDown.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxVolumeDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxVolumeDown.Tag = "VolumeDown";
      this.pictureBoxVolumeDown.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxVolumeDown.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxChannelDown
      // 
      this.pictureBoxChannelDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxChannelDown.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxChannelDown.Image")));
      this.pictureBoxChannelDown.Location = new System.Drawing.Point(168, 192);
      this.pictureBoxChannelDown.Name = "pictureBoxChannelDown";
      this.pictureBoxChannelDown.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxChannelDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxChannelDown.Tag = "ChannelDown";
      this.pictureBoxChannelDown.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxChannelDown.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxChannelUp
      // 
      this.pictureBoxChannelUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxChannelUp.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxChannelUp.Image")));
      this.pictureBoxChannelUp.Location = new System.Drawing.Point(168, 136);
      this.pictureBoxChannelUp.Name = "pictureBoxChannelUp";
      this.pictureBoxChannelUp.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxChannelUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxChannelUp.Tag = "ChannelUp";
      this.pictureBoxChannelUp.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxChannelUp.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxRight
      // 
      this.pictureBoxRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxRight.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRight.Image")));
      this.pictureBoxRight.Location = new System.Drawing.Point(160, 72);
      this.pictureBoxRight.Name = "pictureBoxRight";
      this.pictureBoxRight.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxRight.Tag = "Right";
      this.pictureBoxRight.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxRight.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxLeft
      // 
      this.pictureBoxLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxLeft.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLeft.Image")));
      this.pictureBoxLeft.Location = new System.Drawing.Point(16, 72);
      this.pictureBoxLeft.Name = "pictureBoxLeft";
      this.pictureBoxLeft.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxLeft.Tag = "Left";
      this.pictureBoxLeft.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxLeft.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxDown
      // 
      this.pictureBoxDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxDown.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxDown.Image")));
      this.pictureBoxDown.Location = new System.Drawing.Point(88, 128);
      this.pictureBoxDown.Name = "pictureBoxDown";
      this.pictureBoxDown.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxDown.Tag = "Down";
      this.pictureBoxDown.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxDown.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxOK
      // 
      this.pictureBoxOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxOK.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxOK.Image")));
      this.pictureBoxOK.Location = new System.Drawing.Point(88, 72);
      this.pictureBoxOK.Name = "pictureBoxOK";
      this.pictureBoxOK.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxOK.Tag = "OK";
      this.pictureBoxOK.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxOK.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxUp
      // 
      this.pictureBoxUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxUp.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxUp.Image")));
      this.pictureBoxUp.Location = new System.Drawing.Point(88, 16);
      this.pictureBoxUp.Name = "pictureBoxUp";
      this.pictureBoxUp.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxUp.Tag = "Up";
      this.pictureBoxUp.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxUp.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxBack
      // 
      this.pictureBoxBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxBack.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxBack.Image")));
      this.pictureBoxBack.Location = new System.Drawing.Point(8, 8);
      this.pictureBoxBack.Name = "pictureBoxBack";
      this.pictureBoxBack.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxBack.Tag = "Back";
      this.pictureBoxBack.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxBack.Click += new System.EventHandler(this.button_Click);
      // 
      // tabPagePlayback
      // 
      this.tabPagePlayback.AutoScroll = true;
      this.tabPagePlayback.Controls.Add(this.pictureBoxNextChapter);
      this.tabPagePlayback.Controls.Add(this.pictureBoxPreviousChapter);
      this.tabPagePlayback.Controls.Add(this.pictureBoxRewind);
      this.tabPagePlayback.Controls.Add(this.pictureBoxFastForward);
      this.tabPagePlayback.Controls.Add(this.pictureBoxPause);
      this.tabPagePlayback.Controls.Add(this.pictureBoxRecord);
      this.tabPagePlayback.Controls.Add(this.pictureBoxStop);
      this.tabPagePlayback.Controls.Add(this.pictureBoxPlay);
      this.tabPagePlayback.Location = new System.Drawing.Point(0, 0);
      this.tabPagePlayback.Name = "tabPagePlayback";
      this.tabPagePlayback.Size = new System.Drawing.Size(240, 245);
      this.tabPagePlayback.Text = "Playback ";
      // 
      // pictureBoxNextChapter
      // 
      this.pictureBoxNextChapter.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNextChapter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNextChapter.Image")));
      this.pictureBoxNextChapter.Location = new System.Drawing.Point(136, 136);
      this.pictureBoxNextChapter.Name = "pictureBoxNextChapter";
      this.pictureBoxNextChapter.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxNextChapter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNextChapter.Tag = "NextChapter";
      this.pictureBoxNextChapter.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNextChapter.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxPreviousChapter
      // 
      this.pictureBoxPreviousChapter.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxPreviousChapter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPreviousChapter.Image")));
      this.pictureBoxPreviousChapter.Location = new System.Drawing.Point(40, 136);
      this.pictureBoxPreviousChapter.Name = "pictureBoxPreviousChapter";
      this.pictureBoxPreviousChapter.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxPreviousChapter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPreviousChapter.Tag = "PreviousChapter";
      this.pictureBoxPreviousChapter.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxPreviousChapter.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxRewind
      // 
      this.pictureBoxRewind.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxRewind.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRewind.Image")));
      this.pictureBoxRewind.Location = new System.Drawing.Point(8, 72);
      this.pictureBoxRewind.Name = "pictureBoxRewind";
      this.pictureBoxRewind.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxRewind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxRewind.Tag = "Rewind";
      this.pictureBoxRewind.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxRewind.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxFastForward
      // 
      this.pictureBoxFastForward.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxFastForward.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFastForward.Image")));
      this.pictureBoxFastForward.Location = new System.Drawing.Point(168, 72);
      this.pictureBoxFastForward.Name = "pictureBoxFastForward";
      this.pictureBoxFastForward.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxFastForward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxFastForward.Tag = "FastForward";
      this.pictureBoxFastForward.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxFastForward.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxPause
      // 
      this.pictureBoxPause.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxPause.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPause.Image")));
      this.pictureBoxPause.Location = new System.Drawing.Point(168, 8);
      this.pictureBoxPause.Name = "pictureBoxPause";
      this.pictureBoxPause.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPause.Tag = "Pause";
      this.pictureBoxPause.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxPause.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxRecord
      // 
      this.pictureBoxRecord.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxRecord.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRecord.Image")));
      this.pictureBoxRecord.Location = new System.Drawing.Point(8, 8);
      this.pictureBoxRecord.Name = "pictureBoxRecord";
      this.pictureBoxRecord.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxRecord.Tag = "Record";
      this.pictureBoxRecord.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxRecord.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxStop
      // 
      this.pictureBoxStop.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxStop.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxStop.Image")));
      this.pictureBoxStop.Location = new System.Drawing.Point(88, 8);
      this.pictureBoxStop.Name = "pictureBoxStop";
      this.pictureBoxStop.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxStop.Tag = "Stop";
      this.pictureBoxStop.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxStop.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxPlay
      // 
      this.pictureBoxPlay.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxPlay.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPlay.Image")));
      this.pictureBoxPlay.Location = new System.Drawing.Point(88, 72);
      this.pictureBoxPlay.Name = "pictureBoxPlay";
      this.pictureBoxPlay.Size = new System.Drawing.Size(64, 48);
      this.pictureBoxPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPlay.Tag = "Play";
      this.pictureBoxPlay.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxPlay.Click += new System.EventHandler(this.button_Click);
      // 
      // tabPageNumbers
      // 
      this.tabPageNumbers.AutoScroll = true;
      this.tabPageNumbers.Controls.Add(this.pictureBoxEnter);
      this.tabPageNumbers.Controls.Add(this.pictureBoxClear);
      this.tabPageNumbers.Controls.Add(this.pictureBoxHash);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber0);
      this.tabPageNumbers.Controls.Add(this.pictureBoxStar);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber9);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber8);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber7);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber6);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber5);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber4);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber3);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber2);
      this.tabPageNumbers.Controls.Add(this.pictureBoxNumber1);
      this.tabPageNumbers.Location = new System.Drawing.Point(0, 0);
      this.tabPageNumbers.Name = "tabPageNumbers";
      this.tabPageNumbers.Size = new System.Drawing.Size(240, 245);
      this.tabPageNumbers.Text = "Numbers ";
      // 
      // pictureBoxEnter
      // 
      this.pictureBoxEnter.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxEnter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxEnter.Image")));
      this.pictureBoxEnter.Location = new System.Drawing.Point(88, 200);
      this.pictureBoxEnter.Name = "pictureBoxEnter";
      this.pictureBoxEnter.Size = new System.Drawing.Size(144, 40);
      this.pictureBoxEnter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxEnter.Tag = "Enter";
      this.pictureBoxEnter.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxEnter.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxClear
      // 
      this.pictureBoxClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxClear.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxClear.Image")));
      this.pictureBoxClear.Location = new System.Drawing.Point(8, 200);
      this.pictureBoxClear.Name = "pictureBoxClear";
      this.pictureBoxClear.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxClear.Tag = "Clear";
      this.pictureBoxClear.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxClear.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxHash
      // 
      this.pictureBoxHash.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxHash.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxHash.Image")));
      this.pictureBoxHash.Location = new System.Drawing.Point(168, 152);
      this.pictureBoxHash.Name = "pictureBoxHash";
      this.pictureBoxHash.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxHash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxHash.Tag = "Hash";
      this.pictureBoxHash.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxHash.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber0
      // 
      this.pictureBoxNumber0.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber0.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber0.Image")));
      this.pictureBoxNumber0.Location = new System.Drawing.Point(88, 152);
      this.pictureBoxNumber0.Name = "pictureBoxNumber0";
      this.pictureBoxNumber0.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber0.Tag = "Number0";
      this.pictureBoxNumber0.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber0.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxStar
      // 
      this.pictureBoxStar.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxStar.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxStar.Image")));
      this.pictureBoxStar.Location = new System.Drawing.Point(8, 152);
      this.pictureBoxStar.Name = "pictureBoxStar";
      this.pictureBoxStar.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxStar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxStar.Tag = "Star";
      this.pictureBoxStar.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxStar.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber9
      // 
      this.pictureBoxNumber9.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber9.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber9.Image")));
      this.pictureBoxNumber9.Location = new System.Drawing.Point(168, 104);
      this.pictureBoxNumber9.Name = "pictureBoxNumber9";
      this.pictureBoxNumber9.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber9.Tag = "Number9";
      this.pictureBoxNumber9.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber9.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber8
      // 
      this.pictureBoxNumber8.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber8.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber8.Image")));
      this.pictureBoxNumber8.Location = new System.Drawing.Point(88, 104);
      this.pictureBoxNumber8.Name = "pictureBoxNumber8";
      this.pictureBoxNumber8.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber8.Tag = "Number8";
      this.pictureBoxNumber8.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber8.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber7
      // 
      this.pictureBoxNumber7.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber7.Image")));
      this.pictureBoxNumber7.Location = new System.Drawing.Point(8, 104);
      this.pictureBoxNumber7.Name = "pictureBoxNumber7";
      this.pictureBoxNumber7.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber7.Tag = "Number7";
      this.pictureBoxNumber7.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber7.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber6
      // 
      this.pictureBoxNumber6.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber6.Image")));
      this.pictureBoxNumber6.Location = new System.Drawing.Point(168, 56);
      this.pictureBoxNumber6.Name = "pictureBoxNumber6";
      this.pictureBoxNumber6.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber6.Tag = "Number6";
      this.pictureBoxNumber6.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber6.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber5
      // 
      this.pictureBoxNumber5.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber5.Image")));
      this.pictureBoxNumber5.Location = new System.Drawing.Point(88, 56);
      this.pictureBoxNumber5.Name = "pictureBoxNumber5";
      this.pictureBoxNumber5.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber5.Tag = "Number5";
      this.pictureBoxNumber5.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber5.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber4
      // 
      this.pictureBoxNumber4.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber4.Image")));
      this.pictureBoxNumber4.Location = new System.Drawing.Point(8, 56);
      this.pictureBoxNumber4.Name = "pictureBoxNumber4";
      this.pictureBoxNumber4.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber4.Tag = "Number4";
      this.pictureBoxNumber4.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber4.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber3
      // 
      this.pictureBoxNumber3.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber3.Image")));
      this.pictureBoxNumber3.Location = new System.Drawing.Point(168, 8);
      this.pictureBoxNumber3.Name = "pictureBoxNumber3";
      this.pictureBoxNumber3.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber3.Tag = "Number3";
      this.pictureBoxNumber3.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber3.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber2
      // 
      this.pictureBoxNumber2.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber2.Image")));
      this.pictureBoxNumber2.Location = new System.Drawing.Point(88, 8);
      this.pictureBoxNumber2.Name = "pictureBoxNumber2";
      this.pictureBoxNumber2.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber2.Tag = "Number2";
      this.pictureBoxNumber2.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber2.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxNumber1
      // 
      this.pictureBoxNumber1.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxNumber1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNumber1.Image")));
      this.pictureBoxNumber1.Location = new System.Drawing.Point(8, 8);
      this.pictureBoxNumber1.Name = "pictureBoxNumber1";
      this.pictureBoxNumber1.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxNumber1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxNumber1.Tag = "Number1";
      this.pictureBoxNumber1.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxNumber1.Click += new System.EventHandler(this.button_Click);
      // 
      // tabPageMisc
      // 
      this.tabPageMisc.AutoScroll = true;
      this.tabPageMisc.Controls.Add(this.pictureBoxPictures);
      this.tabPageMisc.Controls.Add(this.pictureBoxVideo);
      this.tabPageMisc.Controls.Add(this.pictureBoxMusic);
      this.tabPageMisc.Controls.Add(this.pictureBoxDVD);
      this.tabPageMisc.Controls.Add(this.pictureBoxGuide);
      this.tabPageMisc.Controls.Add(this.pictureBoxTV);
      this.tabPageMisc.Controls.Add(this.pictureBoxInfo);
      this.tabPageMisc.Controls.Add(this.pictureBoxPower);
      this.tabPageMisc.Controls.Add(this.pictureBoxFullscreen);
      this.tabPageMisc.Controls.Add(this.pictureBoxAspectRatio);
      this.tabPageMisc.Controls.Add(this.pictureBoxTeletext);
      this.tabPageMisc.Controls.Add(this.pictureBoxBlue);
      this.tabPageMisc.Controls.Add(this.pictureBoxYellow);
      this.tabPageMisc.Controls.Add(this.pictureBoxGreen);
      this.tabPageMisc.Controls.Add(this.pictureBoxRed);
      this.tabPageMisc.Location = new System.Drawing.Point(0, 0);
      this.tabPageMisc.Name = "tabPageMisc";
      this.tabPageMisc.Size = new System.Drawing.Size(240, 245);
      this.tabPageMisc.Text = "Misc ";
      // 
      // pictureBoxPictures
      // 
      this.pictureBoxPictures.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxPictures.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPictures.Image")));
      this.pictureBoxPictures.Location = new System.Drawing.Point(168, 104);
      this.pictureBoxPictures.Name = "pictureBoxPictures";
      this.pictureBoxPictures.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxPictures.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPictures.Tag = "Pictures";
      this.pictureBoxPictures.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxPictures.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxVideo
      // 
      this.pictureBoxVideo.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxVideo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxVideo.Image")));
      this.pictureBoxVideo.Location = new System.Drawing.Point(88, 104);
      this.pictureBoxVideo.Name = "pictureBoxVideo";
      this.pictureBoxVideo.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxVideo.Tag = "Video";
      this.pictureBoxVideo.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxVideo.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxMusic
      // 
      this.pictureBoxMusic.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxMusic.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMusic.Image")));
      this.pictureBoxMusic.Location = new System.Drawing.Point(8, 104);
      this.pictureBoxMusic.Name = "pictureBoxMusic";
      this.pictureBoxMusic.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxMusic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxMusic.Tag = "Music";
      this.pictureBoxMusic.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxMusic.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxDVD
      // 
      this.pictureBoxDVD.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxDVD.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxDVD.Image")));
      this.pictureBoxDVD.Location = new System.Drawing.Point(168, 56);
      this.pictureBoxDVD.Name = "pictureBoxDVD";
      this.pictureBoxDVD.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxDVD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxDVD.Tag = "DVD";
      this.pictureBoxDVD.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxDVD.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxGuide
      // 
      this.pictureBoxGuide.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxGuide.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxGuide.Image")));
      this.pictureBoxGuide.Location = new System.Drawing.Point(88, 56);
      this.pictureBoxGuide.Name = "pictureBoxGuide";
      this.pictureBoxGuide.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxGuide.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxGuide.Tag = "Guide";
      this.pictureBoxGuide.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxGuide.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxTV
      // 
      this.pictureBoxTV.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxTV.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTV.Image")));
      this.pictureBoxTV.Location = new System.Drawing.Point(8, 56);
      this.pictureBoxTV.Name = "pictureBoxTV";
      this.pictureBoxTV.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxTV.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxTV.Tag = "TV";
      this.pictureBoxTV.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxTV.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxInfo
      // 
      this.pictureBoxInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxInfo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxInfo.Image")));
      this.pictureBoxInfo.Location = new System.Drawing.Point(88, 8);
      this.pictureBoxInfo.Name = "pictureBoxInfo";
      this.pictureBoxInfo.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxInfo.Tag = "Info";
      this.pictureBoxInfo.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxInfo.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxPower
      // 
      this.pictureBoxPower.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxPower.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPower.Image")));
      this.pictureBoxPower.Location = new System.Drawing.Point(8, 8);
      this.pictureBoxPower.Name = "pictureBoxPower";
      this.pictureBoxPower.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxPower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPower.Tag = "Power";
      this.pictureBoxPower.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxPower.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxFullscreen
      // 
      this.pictureBoxFullscreen.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxFullscreen.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFullscreen.Image")));
      this.pictureBoxFullscreen.Location = new System.Drawing.Point(128, 152);
      this.pictureBoxFullscreen.Name = "pictureBoxFullscreen";
      this.pictureBoxFullscreen.Size = new System.Drawing.Size(104, 40);
      this.pictureBoxFullscreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxFullscreen.Tag = "Fullscreen";
      this.pictureBoxFullscreen.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxFullscreen.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxAspectRatio
      // 
      this.pictureBoxAspectRatio.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxAspectRatio.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxAspectRatio.Image")));
      this.pictureBoxAspectRatio.Location = new System.Drawing.Point(8, 152);
      this.pictureBoxAspectRatio.Name = "pictureBoxAspectRatio";
      this.pictureBoxAspectRatio.Size = new System.Drawing.Size(104, 40);
      this.pictureBoxAspectRatio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxAspectRatio.Tag = "AspectRatio";
      this.pictureBoxAspectRatio.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxAspectRatio.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxTeletext
      // 
      this.pictureBoxTeletext.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxTeletext.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTeletext.Image")));
      this.pictureBoxTeletext.Location = new System.Drawing.Point(168, 8);
      this.pictureBoxTeletext.Name = "pictureBoxTeletext";
      this.pictureBoxTeletext.Size = new System.Drawing.Size(64, 40);
      this.pictureBoxTeletext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxTeletext.Tag = "Teletext";
      this.pictureBoxTeletext.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxTeletext.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxBlue
      // 
      this.pictureBoxBlue.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxBlue.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxBlue.Image")));
      this.pictureBoxBlue.Location = new System.Drawing.Point(184, 208);
      this.pictureBoxBlue.Name = "pictureBoxBlue";
      this.pictureBoxBlue.Size = new System.Drawing.Size(48, 32);
      this.pictureBoxBlue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxBlue.Tag = "Blue";
      this.pictureBoxBlue.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxBlue.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxYellow
      // 
      this.pictureBoxYellow.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxYellow.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxYellow.Image")));
      this.pictureBoxYellow.Location = new System.Drawing.Point(128, 208);
      this.pictureBoxYellow.Name = "pictureBoxYellow";
      this.pictureBoxYellow.Size = new System.Drawing.Size(48, 32);
      this.pictureBoxYellow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxYellow.Tag = "Yellow";
      this.pictureBoxYellow.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxYellow.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxGreen
      // 
      this.pictureBoxGreen.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxGreen.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxGreen.Image")));
      this.pictureBoxGreen.Location = new System.Drawing.Point(64, 208);
      this.pictureBoxGreen.Name = "pictureBoxGreen";
      this.pictureBoxGreen.Size = new System.Drawing.Size(48, 32);
      this.pictureBoxGreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxGreen.Tag = "Green";
      this.pictureBoxGreen.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxGreen.Click += new System.EventHandler(this.button_Click);
      // 
      // pictureBoxRed
      // 
      this.pictureBoxRed.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.pictureBoxRed.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRed.Image")));
      this.pictureBoxRed.Location = new System.Drawing.Point(8, 208);
      this.pictureBoxRed.Name = "pictureBoxRed";
      this.pictureBoxRed.Size = new System.Drawing.Size(48, 32);
      this.pictureBoxRed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxRed.Tag = "Red";
      this.pictureBoxRed.DoubleClick += new System.EventHandler(this.button_Click);
      this.pictureBoxRed.Click += new System.EventHandler(this.button_Click);
      // 
      // notification
      // 
      this.notification.Caption = "Virtual Remote";
      this.notification.InitialDuration = 3;
      this.notification.Text = "";
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.tabControl);
      this.KeyPreview = true;
      this.Menu = this.mainMenu;
      this.Name = "FormMain";
      this.Text = "Virtual Remote";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
      this.Load += new System.EventHandler(this.FormMain_Load);
      this.tabControl.ResumeLayout(false);
      this.tabPageNavigation.ResumeLayout(false);
      this.tabPagePlayback.ResumeLayout(false);
      this.tabPageNumbers.ResumeLayout(false);
      this.tabPageMisc.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.MenuItem menuItemSetup;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageNavigation;
    private System.Windows.Forms.TabPage tabPagePlayback;
    private System.Windows.Forms.MenuItem menuItemQuit;
    private System.Windows.Forms.TabPage tabPageNumbers;
    private System.Windows.Forms.TabPage tabPageMisc;
    private Microsoft.WindowsCE.Forms.Notification notification;
    private System.Windows.Forms.PictureBox pictureBoxRecord;
    private System.Windows.Forms.PictureBox pictureBoxStop;
    private System.Windows.Forms.PictureBox pictureBoxPause;
    private System.Windows.Forms.PictureBox pictureBoxPlay;
    private System.Windows.Forms.PictureBox pictureBoxNextChapter;
    private System.Windows.Forms.PictureBox pictureBoxPreviousChapter;
    private System.Windows.Forms.PictureBox pictureBoxRewind;
    private System.Windows.Forms.PictureBox pictureBoxFastForward;
    private System.Windows.Forms.PictureBox pictureBoxRight;
    private System.Windows.Forms.PictureBox pictureBoxLeft;
    private System.Windows.Forms.PictureBox pictureBoxDown;
    private System.Windows.Forms.PictureBox pictureBoxOK;
    private System.Windows.Forms.PictureBox pictureBoxUp;
    private System.Windows.Forms.PictureBox pictureBoxBack;
    private System.Windows.Forms.PictureBox pictureBoxEnter;
    private System.Windows.Forms.PictureBox pictureBoxClear;
    private System.Windows.Forms.PictureBox pictureBoxHash;
    private System.Windows.Forms.PictureBox pictureBoxNumber0;
    private System.Windows.Forms.PictureBox pictureBoxStar;
    private System.Windows.Forms.PictureBox pictureBoxNumber9;
    private System.Windows.Forms.PictureBox pictureBoxNumber8;
    private System.Windows.Forms.PictureBox pictureBoxNumber7;
    private System.Windows.Forms.PictureBox pictureBoxNumber6;
    private System.Windows.Forms.PictureBox pictureBoxNumber5;
    private System.Windows.Forms.PictureBox pictureBoxNumber4;
    private System.Windows.Forms.PictureBox pictureBoxNumber3;
    private System.Windows.Forms.PictureBox pictureBoxNumber2;
    private System.Windows.Forms.PictureBox pictureBoxNumber1;
    private System.Windows.Forms.PictureBox pictureBoxBlue;
    private System.Windows.Forms.PictureBox pictureBoxYellow;
    private System.Windows.Forms.PictureBox pictureBoxGreen;
    private System.Windows.Forms.PictureBox pictureBoxRed;
    private System.Windows.Forms.PictureBox pictureBoxChannelDown;
    private System.Windows.Forms.PictureBox pictureBoxChannelUp;
    private System.Windows.Forms.PictureBox pictureBoxFullscreen;
    private System.Windows.Forms.PictureBox pictureBoxAspectRatio;
    private System.Windows.Forms.PictureBox pictureBoxTeletext;
    private System.Windows.Forms.PictureBox pictureBoxMute;
    private System.Windows.Forms.PictureBox pictureBoxVolumeUp;
    private System.Windows.Forms.PictureBox pictureBoxVolumeDown;
    private System.Windows.Forms.PictureBox pictureBoxStart;
    private System.Windows.Forms.PictureBox pictureBoxInfo;
    private System.Windows.Forms.PictureBox pictureBoxPower;
    private System.Windows.Forms.PictureBox pictureBoxPictures;
    private System.Windows.Forms.PictureBox pictureBoxVideo;
    private System.Windows.Forms.PictureBox pictureBoxMusic;
    private System.Windows.Forms.PictureBox pictureBoxDVD;
    private System.Windows.Forms.PictureBox pictureBoxGuide;
    private System.Windows.Forms.PictureBox pictureBoxTV;
  }

}
