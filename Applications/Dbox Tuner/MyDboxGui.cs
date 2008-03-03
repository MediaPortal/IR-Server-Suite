#region Copyright (C) 2005-2007 Team MediaPortal

/* 
 *	Copyright (C) 2005-2007 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Dialogs;
using MediaPortal.Playlists;
using MediaPortal.Localisation;
using System.Xml;
using System.IO;
using System.Data;
using System.Drawing;
using MediaPortal.Util;
using AXVLC;
using System.Net;

namespace DboxTuner
{
  public class MyDboxGui : GUIWindow
  {
    #region SkinControlAttributes
    [SkinControlAttribute(2)]
    protected GUIButtonControl btnTVGuide = null;
    [SkinControlAttribute(3)]
    protected GUIToggleButtonControl btnRecord = null;
    [SkinControlAttribute(4)]
    protected GUIButtonControl btnSnap = null;
    [SkinControlAttribute(6)]
    protected GUIButtonControl btnBouquet = null;
    [SkinControlAttribute(7)]
    protected GUIButtonControl btnChannel = null;
    [SkinControlAttribute(8)]
    protected GUIToggleButtonControl btnTVOnOff = null;
    [SkinControlAttribute(9)]
    protected GUIToggleButtonControl btnShiftOnOff = null;
    [SkinControlAttribute(11)]
    protected GUIButtonControl btnRecordings = null;
    [SkinControlAttribute(13)]
    protected GUIButtonControl btnWhatsThis = null;
    [SkinControlAttribute(24)]
    protected GUIImage imgRecord;
    [SkinControlAttribute(99)]
    protected GUIVideoControl videoWindow = null;
    #endregion

    #region Private Variables

    private static string server;
    private static string username;
    private static string password;
    public static string boxtype;

    private static string AutoOn;
    private static bool BoxConn = false;

    private static DateTime startRecord;

    DataRow row = null;
    DataTable _TV_Bouquets = new DataTable("BouquetsTV");

    private static int ActBouquetNo;
    private static string ActBouquet;
    public static string ActChannel;

    private static VirtualDirectory recDirectory = new VirtualDirectory();
    private static string RecDir = String.Empty;
    private static string SnapDir = String.Empty;

    private string LogPathName = Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config) + @"\log\MyDbox.log";

    private static OnActionHandler ah;

    public static VideoLanControl vlcControl = null;
    // osd for channel
    public static MyOSD osd;
    public int t_OSD = 0;
    // select direct the channel
    public static MySelect sel;
    public int t_Sel = 0;
    public string SelStr = "....";
    // info screen
    public static MyOSD_Info info;
    public int t_Info = 0;
    // info screen
    public static MyOSD_Chan chan;
    public int t_Chan = 0;
    public ArrayList SelChan;
    public int MaxBouq;
    // global functions dbox (faster)
    static DboxFunctions func;

    public AxAXVLC.AxVLCPlugin Control
    {
      get
      {
        return vlcControl.Player;
      }
    }

    string _currentGroup = String.Empty;
    string _currentChannel = String.Empty;

    bool _started;
    bool _paused;
    bool _isFullScreen;
    bool _wasFullscreen; // true if we are in fullscreen and ACTION_PREVIOUS_MENU is sent to action handler


    int _cnt_Ply = 0;
    bool _Ply_VLC = false;

    int _positionX = 10, _positionY = 10, _videoWidth = 100, _videoHeight = 100;
    int _PreviousWindowID = 0;

    // ticker for updates
    private System.Windows.Forms.Timer _Update = new System.Windows.Forms.Timer();
    private System.Windows.Forms.Timer _OSDTimer = new System.Windows.Forms.Timer();

    //thread for EPG updates
    System.Threading.Thread EPG_Thread;

    static cGlobal cGlobal;

    #endregion

    #region Private Enumerations
    enum Controls
    {
      BouquetButton = 6,
      ChannelButton = 7,
      TVButton = 3,
      TVOnOff = 8,
      RecordingsButton = 11,
      RadioButton = 12,
      DirectButton = 13,
      List = 50
    }
    #endregion

    public override int GetID
    {
      get
      {
        return 75642203;
      }
      set
      {
        base.GetID = value;
      }
    }

    public override bool Init()
    {
      bool result = Load(GUIGraphicsContext.Skin + @"\mydboxmain.xml");
      Mydbox.GUILocalizeStrings.Load(GUI.Library.GUILocalizeStrings.CurrentLanguage());

      LoadSettings();

      Data dboxdata = new Data(server, username, password, boxtype);
      dboxdata.loadEPGdata(); //load epg data from file

      if (File.Exists(LogPathName)) File.Delete(LogPathName);
      ErrorLog("Init plugin");

      SelChan = new ArrayList();

      // get bouquets
      ParseXML();
      ErrorLog("XML parsed " + _TV_Bouquets.Rows.Count);

      recDirectory.AddExtension(".dat");
      recDirectory.ShowFilesWithoutExtension = false;

      Share share2 = new Share();
      share2.Name = "Recordings";
      share2.Path = RecDir;
      recDirectory.IsRootShare(RecDir);

      if (ah == null) ah = new OnActionHandler(OnAction2);

      #region get some data from box
      try
      {
        // get actual ID
        func = new DboxFunctions(server, username, password, boxtype);
        func.wakeup();
        cGlobal.currentID = func.getID();
        if (cGlobal.currentID != "")
        {
          ErrorLog("Actual ID " + cGlobal.currentID);
          func.setSPTS();
          ErrorLog("SetSPTS has been set");
          ErrorLog("Version=> " + func.getInfo());
          func.showMessage("Mediaportal%20connected");
          // looks box is working
          BoxConn = true;
        }
        else
        {
          ErrorLog("Receiver is not reachable ?");
        }
      }

      catch
      {
        ErrorLog("Receiver is not reachable ?");
      }

      // get group/channel
      ActBouquetNo = 1;
      for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
      {
        if (_TV_Bouquets.Rows[i]["ID"].ToString() == cGlobal.currentID)
        {
          ActBouquetNo = Convert.ToInt16(_TV_Bouquets.Rows[i]["BouqNo"].ToString());

          ActBouquet = _TV_Bouquets.Rows[i]["BouqName"].ToString();
          ActChannel = _TV_Bouquets.Rows[i]["Name"].ToString();

          ErrorLog("Actual group " + ActBouquet + " actual channel " + ActChannel);
        }
      }

      #endregion

      return result;
    }

    #region Actions

    public void OnAction2(Action action)
    {
      Debug.WriteLine("Action2:" + action.wID.ToString());

      if (GUIWindowManager.ActiveWindowEx == (int)GUIWindow.Window.WINDOW_DIALOG_MENU) return;
      //bool actionTaken = true;

      //Debug.WriteLine(action.wID.ToString());

      object key;
      switch (action.wID)
      {
        case Action.ActionType.ACTION_EXIT:
          { // prepare for exit
            OnPageDestroy(0);
            break;
          }
        case Action.ActionType.ACTION_PREVIOUS_MENU:
          {
            if (_isFullScreen)
            {
              if (t_Chan > 0) t_Chan = 0; // channel osd is up, kill it
              else if (t_OSD > 0) t_OSD = 0; // epg is up, kill it
              else if (t_Info > 0)
              {
                t_Info = 0;
                t_OSD = 2; // info is up, go back to epg
              }
              else                            // no osd is up, exit fullscreen
              {
                _isFullScreen = false;
                _wasFullscreen = true;
                SetWindows();
              }
            }
            break;
          }
        case Action.ActionType.ACTION_MOVE_LEFT:
          if ((_isFullScreen == true) && (cGlobal.VLC_Live == true))
          {
            if (t_Chan > 0)
            { // only if already visible
              ActBouquetNo--;
            }

            if (ActBouquetNo == 0)
              ActBouquetNo = MaxBouq;

            t_Chan = 10;
            t_OSD = 0;
            chan.DisplayBouq(ActBouquetNo.ToString(), "0");
          }
          break;
        case Action.ActionType.ACTION_MOVE_RIGHT:
          if ((_isFullScreen == true) && (cGlobal.VLC_Live == true))
          {
            if (t_Chan > 0)
            { // only if already visible
              ActBouquetNo++;
            }
            if (ActBouquetNo > MaxBouq)
              ActBouquetNo = 1;

            t_Chan = 10;
            t_OSD = 0;
            chan.DisplayBouq(ActBouquetNo.ToString(), "-1");
          }
          break;
        case Action.ActionType.ACTION_MOVE_DOWN:
        case Action.ActionType.ACTION_MOVE_UP:
          if ((_isFullScreen == true) && (cGlobal.VLC_Live == true))
          {
            t_Chan = 10;
            t_OSD = 0;
          }
          break;
        case Action.ActionType.ACTION_VOLUME_MUTE:
          DboxFunctions func = new DboxFunctions(server, username, password, boxtype);
          func.toggleMute();
          break;
        case Action.ActionType.ACTION_FORWARD:
        case Action.ActionType.ACTION_MUSIC_FORWARD:
          key = vlcControl.Player.getVariable("key-faster");
          vlcControl.Player.setVariable("key-pressed", key);
          break;
        case Action.ActionType.ACTION_MUSIC_REWIND:
        case Action.ActionType.ACTION_REWIND:
          key = vlcControl.Player.getVariable("key-slower");
          vlcControl.Player.setVariable("key-pressed", key);
          break;
        case Action.ActionType.ACTION_PREV_CHANNEL:
          OnPreviousChannel();
          PlayCurrentChannel();
          break;
        case Action.ActionType.ACTION_PAGE_DOWN:
          OnPreviousChannel();
          PlayCurrentChannel();
          break;
        case Action.ActionType.ACTION_NEXT_CHANNEL:
          OnNextChannel();
          PlayCurrentChannel();
          break;
        case Action.ActionType.ACTION_PAGE_UP:
          OnNextChannel();
          PlayCurrentChannel();
          break;
        case Action.ActionType.ACTION_PLAY:
        case Action.ActionType.ACTION_MUSIC_PLAY:
          PlayCurrentChannel();
          break;
        case Action.ActionType.ACTION_STOP:
          StopPlaying();
          btnTVOnOff.Selected = false;
          break;
        case Action.ActionType.ACTION_PAUSE:
          if (_started)
          {
            if (!vlcControl.Player.Playing)
            {
              _paused = false;
              vlcControl.Player.play();
            }
            else
            {
              _paused = true;
              vlcControl.Player.pause();
            }
          }
          break;
        case (Action.ActionType.ACTION_RECORD):
          if (btnRecord.Selected)
          {
            btnRecord.Selected = false;
            imgRecord.Visible = false;
          }
          else
          {
            btnRecord.Selected = true;
            imgRecord.Visible = true;
            OnRecord();
          }
          break;
        case (Action.ActionType.ACTION_SHOW_FULLSCREEN):
          SetWindows();
          break;
        case (Action.ActionType.ACTION_SHOW_GUI):
          if (_isFullScreen == false)
          {
            //vlcControl.Player.fullscreen(); 
            _isFullScreen = true;
            SetWindows();
          }
          else
          {
            if (chan.Visible == false)
            {
              //vlcControl.Player.fullscreen(); 
              _isFullScreen = false;
              SetWindows();
            }
            else
            {

            }
          }
          break;
        case Action.ActionType.ACTION_SELECT_ITEM:
          string t = chan.GetChannel(ActBouquet);
          if ((t != "") && (_isFullScreen == true))
          {
            if (t_Chan > 0)
            {
              t_Chan = 0;
              ChangeChannel(t);
              PlayCurrentChannel();
            }
          }
          break;
        case (Action.ActionType.ACTION_CONTEXT_MENU):
          if (t_OSD > 0)
          {
            if (cGlobal.VLC_Live == true)
            {
              t_OSD = 0;
              t_Info = 10;
            }
            else
              t_OSD = 0;
          }
          else
          {
            t_OSD = 10;
            t_Info = 0;
            t_Chan = 0;
          }
          break;
        case Action.ActionType.ACTION_KEY_PRESSED:
          if (cGlobal.VLC_Live == true)
          {
            switch (action.m_key.KeyChar)
            {
              case '1':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "1";
                SelStr = callSelect(SelStr);
                break;
              case '2':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "2";
                SelStr = callSelect(SelStr);
                break;
              case '3':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "3";
                SelStr = callSelect(SelStr);
                break;
              case '4':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "4";
                SelStr = callSelect(SelStr);
                break;
              case '5':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "5";
                SelStr = callSelect(SelStr);
                break;
              case '6':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "6";
                SelStr = callSelect(SelStr);
                break;
              case '7':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "7";
                SelStr = callSelect(SelStr);
                break;
              case '8':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "8";
                SelStr = callSelect(SelStr);
                break;
              case '9':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "9";
                SelStr = callSelect(SelStr);
                break;
              case '0':
                t_Sel = 2;
                t_Info = 0;
                SelStr = SelStr + "0";
                SelStr = callSelect(SelStr);
                break;
              case '#':
                if ((SelStr != "....") & (SelStr != "") & (SelStr.Length > 0) & (t_Sel > 0))
                {
                  int x = Convert.ToInt16(SelStr) - 1;
                  try
                  {
                    t_Sel = 0;
                    ChangeChannel(_TV_Bouquets.Rows[x]["ID"].ToString());
                    PlayCurrentChannel();
                  }
                  catch
                  {
                  }
                }
                break;
            }
          }
          break;
        case (Action.ActionType.ACTION_ASPECT_RATIO):
          if ((SelStr != "....") & (SelStr != "") & (SelStr.Length > 0) & (t_Sel > 0))
          {
            int x = Convert.ToInt16(SelStr) - 1;
            try
            {
              t_Sel = 0;
              ChangeChannel(_TV_Bouquets.Rows[x]["ID"].ToString());
              PlayCurrentChannel();
            }
            catch
            {
            }
          }
          break;

        default:
          //actionTaken = false;
          break;
      }
    }

    private string callSelect(string selNo)
    {
      selNo = selNo.Replace(".", "");
      if (selNo.Length > 4)
      {
        selNo = selNo.Substring(4, selNo.Length - 4);
      }

      string ch = "";
      int x = Convert.ToInt32(selNo);
      if (x > 0)
      {
        x = x - 1;
        // valid no ??
        if (x <= _TV_Bouquets.Rows.Count)
        {
          ch = _TV_Bouquets.Rows[x]["Name"].ToString();
        }
        else
        {
          ch = "";
        }
      }
      sel.SetSelction(selNo, ch);

      return selNo;
    }

    public override void OnAction(Action action)
    {
      if (_isFullScreen || _wasFullscreen)
      { // disable "right click"/previous menu when in fullscreen
        if (action.wID == Action.ActionType.ACTION_PREVIOUS_MENU)
        {
          action = new Action(Action.ActionType.ACTION_SHOW_GUI, 0, 0);
          _wasFullscreen = false;
        }
        base.OnAction(action);
      }
      else
      { // if windowed still the same
        base.OnAction(action);
      }
    }

    #endregion

    #region overrides

    public override bool OnMessage(GUIMessage message)
    {
      return base.OnMessage(message);
    }

    protected override void OnPageLoad()
    {
      base.OnPageLoad();

      ErrorLog("Load plugin");

      // set actions
      //GUIGraphicsContext.OnNewAction -= ah;
      GUIGraphicsContext.OnNewAction += ah;

      // disable exclusive mode
      Log.Info("DBox2 plugin: Disabling DX9 exclusive mode");
      GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SWITCH_FULL_WINDOWED, 0, 0, 0, 0, 0, null);
      GUIWindowManager.SendMessage(msg);

      // init record fields
      GUIPropertyManager.SetProperty("#Dbox.Record.channel", " ");
      GUIPropertyManager.SetProperty("#Dbox.Record.title", " ");
      GUIPropertyManager.SetProperty("#Dbox.Record.start", " ");
      GUIPropertyManager.SetProperty("#Dbox.Record.stop", " ");


      #region set timer
      // start update ticker
      _Update.Interval = 2000;
      _Update.Tick += new EventHandler(_Update_Tick);
      _Update.Start();
      // start OSD ticker
      _OSDTimer.Interval = 1000;
      _OSDTimer.Tick += new EventHandler(_OSDTimer_Tick);
      _OSDTimer.Start();
      #endregion

      //set video window position
      if (videoWindow != null)
      {
        GUIGraphicsContext.VideoWindow = new Rectangle(videoWindow.XPosition, videoWindow.YPosition, videoWindow.Width, videoWindow.Height);
      }

      // TV is off
      btnTVOnOff.Selected = false;

      #region init vlc
      // close last instance
      if (vlcControl != null)
        vlcControl = null;

      // SaveDir exits ?
      if (!Directory.Exists(RecDir)) Directory.CreateDirectory(RecDir);
      if (!Directory.Exists(SnapDir)) Directory.CreateDirectory(SnapDir);

      // new instance
      vlcControl = new VideoLanControl();
      vlcControl = VideoLanControl.Instance;
      // control is now visible
      vlcControl.Visible = false;
      vlcControl.Enabled = false;
      // add control to GUI 
      GUIGraphicsContext.form.Controls.Add(vlcControl);
      // set focus
      GUIGraphicsContext.form.Focus();
      #endregion

      #region init osd
      // prepare osd
      if (osd == null)
      {
        osd = new MyOSD();
        osd.Visible = false;
        GUIGraphicsContext.form.Controls.Add(osd);
        GUIGraphicsContext.form.Focus();
      }
      // prepare select
      if (sel == null)
      {
        sel = new MySelect();
        sel.Visible = false;
        GUIGraphicsContext.form.Controls.Add(sel);
        GUIGraphicsContext.form.Focus();
      }
      if (info == null)
      {
        info = new MyOSD_Info();
        info.Visible = false;
        GUIGraphicsContext.form.Controls.Add(info);
        GUIGraphicsContext.form.Focus();
      }
      if (chan == null)
      {
        chan = new MyOSD_Chan(_TV_Bouquets);
        chan.Visible = false;
        GUIGraphicsContext.form.Controls.Add(chan);
        GUIGraphicsContext.form.Focus();
      }
      #endregion

      // init channel display
      chan.DisplayBouq(ActBouquetNo.ToString(), "1");

      // set windows
      SetWindows();

      //Disable program & timeshift for now
      GUIControl.DisableControl(GetID, 2);
      GUIControl.DisableControl(GetID, 9);
      //focus on/off
      GUIControl.FocusControl(GetID, 8);

      // set localized labels for static controls
      GUIPropertyManager.SetProperty("#header_label", Mydbox.GUILocalizeStrings.Get(0)); // MyDreamDboxTV
      GUIControl.SetControlLabel(GetID, btnTVGuide.GetID, Mydbox.GUILocalizeStrings.Get(600)); // Program
      GUIControl.SetControlLabel(GetID, btnRecord.GetID, Mydbox.GUILocalizeStrings.Get(601)); // Record
      GUIControl.SetControlLabel(GetID, btnSnap.GetID, Mydbox.GUILocalizeStrings.Get(602)); // Snapshot
      GUIControl.SetControlLabel(GetID, btnBouquet.GetID, Mydbox.GUILocalizeStrings.Get(603)); // Group
      GUIControl.SetControlLabel(GetID, btnChannel.GetID, Mydbox.GUILocalizeStrings.Get(604)); // Channel
      GUIControl.SetControlLabel(GetID, btnTVOnOff.GetID, Mydbox.GUILocalizeStrings.Get(605)); //On/Off
      GUIControl.SetControlLabel(GetID, btnShiftOnOff.GetID, Mydbox.GUILocalizeStrings.Get(606)); // Shift
      GUIControl.SetControlLabel(GetID, btnRecordings.GetID, Mydbox.GUILocalizeStrings.Get(607)); // Recordings
      GUIControl.SetControlLabel(GetID, btnWhatsThis.GetID, Mydbox.GUILocalizeStrings.Get(608)); // Whats ..

      if (AutoOn == "yes")
      {
        _isFullScreen = true;
        SetWindows();
        btnTVOnOff.Selected = true;
        AutoOn = "no";
        PlayCurrentChannel();
      }

      EPG_Thread = new System.Threading.Thread(new System.Threading.ThreadStart(EPG_Refresh));
      EPG_Thread.Start();
      System.Threading.Thread.Sleep(0);

    }

    protected override void OnPageDestroy(int new_windowId)
    {
      _Update.Stop();
      _Update.Tick -= new EventHandler(_Update_Tick);

      _OSDTimer.Stop();
      _OSDTimer.Tick -= new EventHandler(_Update_Tick);

      EPG_Thread.Interrupt();
      EPG_Thread.Abort();

      Data dboxdata = new Data(server, username, password, boxtype);
      dboxdata.saveEPGdata();

      if (_started)
        StopPlaying();

      ErrorLog("Unload plugin");

      if (new_windowId != (int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO) GUIGraphicsContext.OnNewAction -= ah;
      base.OnPageDestroy(35); //(new_windowId);            
    }

    protected override void OnClicked(int controlId, GUIControl control, MediaPortal.GUI.Library.Action.ActionType actionType)
    {
      if (!_isFullScreen)
      {
        if (control == btnRecord)
        {
          OnRecord();
        }
        if (control == btnSnap)
        {
          object key = vlcControl.Player.getVariable("key-snapshot");
          vlcControl.Player.setVariable("key-pressed", key);
        }
        if (control == btnBouquet)
          showBouquet();
        if (control == btnChannel)
          showChannel();
        if (control == btnTVOnOff)
          switchOnOff();
        if (control == btnWhatsThis)
          OnBtnWhatsThis();
        if (control == btnShiftOnOff)
        {
        }
        if (control == btnRecordings)
        {
          t_OSD = 0;
          osd.Visible = false;
          t_Sel = 0;
          sel.Visible = false;

          if (_started)
            StopVLC();

          // Play recording
          cGlobal.VLC_Live = false;
          //display tv on
          btnTVOnOff.Selected = false;

          //activate Recordings Screen
          GUIWindowManager.ActivateWindow(75642204);

        }
      }
      base.OnClicked(controlId, control, actionType);
    }

    protected override void OnShowContextMenu()
    {
      base.OnShowContextMenu();
    }

    #endregion overrides

    private void showBouquet()
    {
      //show list with bouquets
      GUIDialogMenuBottomRight menu = (GUIDialogMenuBottomRight)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU_BOTTOM_RIGHT);
      menu.Reset();
      menu.SetHeading(GUILocalizeStrings.Get(971) + ": ");

      int act = 0;
      String actRef = "";

      for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
      {
        actRef = _TV_Bouquets.Rows[i]["BouqNo"].ToString();
        if (act != Convert.ToInt16(actRef))
        {
          menu.Add(_TV_Bouquets.Rows[i]["BouqName"].ToString());
          act = Convert.ToInt16(actRef);
        }
      }
      menu.DoModal(GetID);
      string bouquetName = menu.SelectedLabelText;
      if (bouquetName != "")
      {
        for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
        {
          if (_TV_Bouquets.Rows[i]["BouqName"].ToString() == bouquetName)
          {
            ActBouquetNo = Convert.ToInt16(_TV_Bouquets.Rows[i]["BouqNo"].ToString());
            ActBouquet = _TV_Bouquets.Rows[i]["BouqName"].ToString();
            break;
          }
        }
      }

    }

    private void showChannel()
    {
      //show list with bouquets
      GUIDialogMenuBottomRight menu = (GUIDialogMenuBottomRight)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU_BOTTOM_RIGHT);
      menu.Reset();
      menu.SetHeading(GUILocalizeStrings.Get(972) + ": ");

      String actRef = "";

      for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
      {
        actRef = _TV_Bouquets.Rows[i]["BouqNo"].ToString();

        if (ActBouquetNo == Convert.ToInt16(actRef))
        {
          ActChannel = _TV_Bouquets.Rows[i]["Name"].ToString();
          string s = _TV_Bouquets.Rows[i]["ChanNo"].ToString() + " ";
          s = s + _TV_Bouquets.Rows[i]["Name"].ToString();

          menu.Add(s);
        }
      }
      menu.DoModal(GetID);

      int start = 0;
      int stop = menu.SelectedLabelText.IndexOf(" ", 0);

      if (stop > 0)
      {
        string channelNr = menu.SelectedLabelText.Substring(start, stop);

        if (channelNr != "")
        {
          for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
          {
            if (_TV_Bouquets.Rows[i]["ChanNo"].ToString() == channelNr)
            {
              ChangeChannel(_TV_Bouquets.Rows[i]["ID"].ToString());

              if (btnTVOnOff.Selected)
                PlayCurrentChannel();
              break;
            }
          }
        }
      }

    }

    private void switchOnOff()
    {
      //check if TV is turned on or off
      if (btnTVOnOff.Selected)
      {
        if (_started)
          StopPlaying();

        cGlobal.VLC_Live = true;
        PlayCurrentChannel();
      }
      else
      {
        if (_started)
          StopPlaying();
      }
    }

    private void OnBtnWhatsThis()
    {
      bool was = vlcControl.Visible;

      DisableOSD();
      vlcControl.Visible = false;

      GUIDialogText dlg = (GUIDialogText)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_TEXT);
      dlg.SetHeading(Mydbox.GUILocalizeStrings.Get(608)); // help dialog heading
      dlg.SetText(Mydbox.GUILocalizeStrings.Get(999)); // help dialog text
      dlg.DoModal(GetID);

      vlcControl.Visible = was;
    }

    // not needed and so disabled
    private void selectChannel()
    {
      if (!cGlobal.VLC_Live)
        return;

      StopPlaying();

      VirtualKeyboard menu = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
      menu.Reset();
      menu.DoModal(GetID);

      string channelNumber = menu.Text;
      if (channelNumber != "")
      {
        for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
        {
          if (_TV_Bouquets.Rows[i]["ChanNo"].ToString() == channelNumber)
          {
            ChangeChannel(_TV_Bouquets.Rows[i]["ID"].ToString());
            break;
          }
        }
      }
      if (btnTVOnOff.Selected)
        PlayCurrentChannel();
    }

    public void LoadSettings()
    {
      using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings("MediaPortal.xml"))
      {
        server = xmlreader.GetValue("mydbox", "IP");
        username = xmlreader.GetValue("mydbox", "UserName");
        password = xmlreader.GetValue("mydbox", "Password");
        boxtype = xmlreader.GetValue("mydbox", "Boxtype");

        RecDir = xmlreader.GetValue("mydbox", "Record");
        SnapDir = xmlreader.GetValue("mydbox", "Snapshot");

        AutoOn = xmlreader.GetValue("mydbox", "AutoOn");
        ErrorLog("loaded settings - box = " + boxtype);
      }
    }

    public void ChangeChannel(string ID)
    {
      if (_started)
        StopPlaying();

      System.Threading.Thread.Sleep(100);

      ErrorLog("Zap to " + ID);
      // zapto ID
      func = new DboxFunctions(server, username, password, boxtype);
      func.Zapto(ID);

      cGlobal.currentID = ID;

      System.Threading.Thread.Sleep(1000);
      ErrorLog("Zap and waited " + ID);

      // get group/channel
      ActBouquetNo = 1;
      for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
      {
        if (_TV_Bouquets.Rows[i]["ID"].ToString() == cGlobal.currentID)
        {
          ActBouquetNo = Convert.ToInt16(_TV_Bouquets.Rows[i]["BouqNo"].ToString());

          ActBouquet = _TV_Bouquets.Rows[i]["BouqName"].ToString();
          ActChannel = _TV_Bouquets.Rows[i]["Name"].ToString();

          ErrorLog("Actual group " + ActBouquet + " actual channel " + ActChannel);
        }
      }

    }

    public void PlayCurrentChannel()
    {
      if (!btnTVOnOff.Selected)
        return;
      string url;
      // get LiveURL
      func = new DboxFunctions(server, username, password, boxtype);

      if (cGlobal.currentID.Contains("rootX"))
      { // workaround for e1 internal hdd movieplaying
        url = cGlobal.currentID;
        cGlobal.VLC_Live = false; // kill the live flag, don't want epg and such
      }
      else
      { // normally this is the code we use
        url = func.getLiveUrl();
        cGlobal.VLC_Live = true;
      }
      ErrorLog("Get live url = " + url);



      if (url == null)
      {
        ErrorLog("ERROR: NOTHING TO PLAY !");
        GUIDialogOK dlg = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
        dlg.SetHeading("Error nothing to play");
        dlg.SetLine(1, "Channel is not playable !");
        dlg.SetLine(2, String.Empty);
        dlg.SetLine(3, String.Empty);
        dlg.DoModal(GUIWindowManager.ActiveWindow);
      }

      if (url != null)
      {

        //GUIGraphicsContext.IsFullScreenVideo = false;

        cGlobal.VLC_File = url;
        cGlobal.VLC_StartPlay = true;
        ErrorLog("SetStartPlay " + url);
      }
    }

    public void OnPreviousChannel()
    {
      if ((cGlobal.currentID != "") & (btnTVOnOff.Selected == true))
      {
        for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
        {
          if (_TV_Bouquets.Rows[i]["ID"].ToString() == cGlobal.currentID)
          {
            if (i > 0)
            {
              i--;
              ChangeChannel(_TV_Bouquets.Rows[i]["ID"].ToString());
              break;
            }
          }
        }
      }
    }

    public void OnNextChannel()
    {
      if ((cGlobal.currentID != "") && (btnTVOnOff.Selected == true))
      {
        for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
        {
          if (_TV_Bouquets.Rows[i]["ID"].ToString() == cGlobal.currentID)
          {
            if (i < _TV_Bouquets.Rows.Count - 1)
            {
              i++;
              ChangeChannel(_TV_Bouquets.Rows[i]["ID"].ToString());
              break;
            }
          }
        }
      }
    }

    public void OnRecord()
    {
      if ((btnTVOnOff.Selected == true) & (cGlobal.VLC_Live == true))
      {
        startRecord = DateTime.Now;

        object key = vlcControl.Player.getVariable("key-snapshot");
        if (btnRecord.Selected)
        {
          vlcControl.Player.setVariable("key-pressed", key);
          imgRecord.Visible = true;
        }
        else
        {
          imgRecord.Visible = false;
        }
        key = vlcControl.Player.getVariable("key-record");
        vlcControl.Player.setVariable("key-pressed", key);
      }
      else
      {
        btnRecord.Selected = false;
      }
    }

    public void StopPlaying()
    {
      sel.Visible = false;
      t_Sel = 0;
      osd.Visible = false;
      t_OSD = 0;
      info.Visible = false;
      t_Info = 0;

      if (_started)
      {
        StopVLC();
      }
    }

    public void StopVLC()
    {

      if (vlcControl != null)
      {
        vlcControl.Visible = false;

        if (vlcControl.Player.Playing)
        {
          vlcControl.Player.stop();
          ErrorLog("Stop playing");
          while (vlcControl.Player.Playing)
          { }
        }
      }

      // stop recording
      btnRecord.Selected = false;
      imgRecord.Visible = false;

      //Log.Info("MyDBOX:: Playback stopped: {0}, {1}", _currentFile, vlcControl.Player.Length);
      //FullScreen = false;
      _paused = false;
      _started = false;

      System.Threading.Thread.Sleep(1000);
      ErrorLog("Stop playing finished");
    }

    public void PlayVLC()
    {
      // do not try to play if empty, crash MP
      if (cGlobal.VLC_File == "")
      {
        btnTVOnOff.Selected = false;
        return;
      }

      if (cGlobal.VLC_Live == false)
      { // play recordings in fullscreen
        _isFullScreen = true;

        SetWindows();
      }

      ErrorLog("Clear playlist");
      // clear playlist
      vlcControl.Player.playlistClear();

      string[] option = new string[]{
                         ":http-caching=500" ,
                         //":mms-caching500=" ,
                         //":realrtsp-caching=500" ,
                         //":tcp-caching=500",
                         ":udp-caching=500",
                         //":smb-caching=500" ,
                         ":snapshot-path=" + SnapDir,
                         ":record-path=" + RecDir,
                         ":timeshift-dir=" + RecDir,
                         ":access-filter=record",
                         //":no-overlay",
                         ":vout-filter=deinterlace",
                         ":deinterlace-mode=Bob"
                         };
      // play file
      // _currentFile
      ErrorLog("Start playing " + cGlobal.VLC_File);
      vlcControl.Player.addTarget(cGlobal.VLC_File, option, AXVLC.VLCPlaylistMode.VLCPlayListAppendAndGo, 0);
      while (!vlcControl.Player.Playing)
      { }

      vlcControl.Visible = true;
      vlcControl.Update();

      System.Threading.Thread.Sleep(1000);

      ErrorLog("Playing " + cGlobal.VLC_File);

      //show OSD
      t_OSD = 10;
      //display tv on
      btnTVOnOff.Selected = true;

      //Log.Info("MyDBOX: Playback started: {0}, {1}", _currentFile);
      //_started = true;
      _paused = false;
    }

    private void SetWindows()
    {
      try
      {
        Point location = new Point(0, 0);
        Size size = new Size(0, 0);

        if (_isFullScreen)
        {
          _positionX = GUIGraphicsContext.OverScanLeft;
          _positionY = GUIGraphicsContext.OverScanTop;
          _videoHeight = GUIGraphicsContext.OverScanHeight;
          _videoWidth = GUIGraphicsContext.OverScanWidth;

          if (GUIWindowManager.ActiveWindow > 0)
            _PreviousWindowID = GUIWindowManager.ActiveWindow;

          // do checks
          location = new Point(_positionX, _positionY);
          size = new Size(_videoWidth, _videoHeight);
          if (vlcControl.Location != location)
          {
            vlcControl.Location = location;
            vlcControl.Player.Location = location;
            //GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO);
          }
          if (vlcControl.Size != size)
          {
            vlcControl.ClientSize = size;
            vlcControl.Player.ClientSize = size;
            //GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO);

          }


        }
        else
        {
          // normal view

          _positionX = GUIGraphicsContext.VideoWindow.Left;
          _positionY = GUIGraphicsContext.VideoWindow.Top;
          _videoWidth = GUIGraphicsContext.VideoWindow.Width;
          _videoHeight = GUIGraphicsContext.VideoWindow.Height;

          // do checks
          location = new Point(_positionX, _positionY);
          size = new Size(_videoWidth, _videoHeight);
          if (vlcControl.Location != location)
          {
            vlcControl.Location = location;
            vlcControl.Player.Location = new Point(0, 0);
            //GUIWindowManager.ActivateWindow((int)6801);
          }
          if (vlcControl.Size != size)
          {
            vlcControl.ClientSize = size;
            vlcControl.Player.ClientSize = size;
            //GUIWindowManager.ActivateWindow((int)6801);

          }

          chan.Visible = false;
          t_Chan = 0;

        }

        // OSD

        int facH = (vlcControl.ClientSize.Height / 5);
        int facW = (vlcControl.ClientSize.Width / 20);

        _positionX = vlcControl.Location.X;
        _positionY = vlcControl.Location.Y + (vlcControl.ClientSize.Height - facH - facW);

        // do checks
        location = new Point(_positionX + (facW / 2), _positionY);
        size = new Size(vlcControl.ClientSize.Width - facW, facH);

        osd.Location = location;
        osd.Size = size;

        // Select

        facH = (vlcControl.ClientSize.Height / 6);
        facW = (vlcControl.ClientSize.Width / 3);

        _positionX = vlcControl.Location.X + (vlcControl.ClientSize.Width / 2);
        _positionY = vlcControl.Location.Y + (vlcControl.ClientSize.Height / 2);

        // do checks
        location = new Point(_positionX - (facW / 2), _positionY - (facH / 2));
        size = new Size(facW, facH);

        sel.Location = location;
        sel.Size = size;

        // Info

        facH = (vlcControl.ClientSize.Height / 10);
        facW = (vlcControl.ClientSize.Width / 10);

        _positionX = vlcControl.Location.X + (facW);
        _positionY = vlcControl.Location.Y + (facH);

        // do checks
        location = new Point(_positionX, _positionY);
        size = new Size(facW * 8, facH * 8);

        info.Location = location;
        info.Size = size;

        // Chan

        facH = (vlcControl.ClientSize.Height / 10);
        facW = (vlcControl.ClientSize.Width / 10);

        _positionX = vlcControl.Location.X + (facW);
        _positionY = vlcControl.Location.Y + (facH);

        // do checks
        location = new Point(_positionX, _positionY);
        size = new Size(facW * 8, facH * 8);

        chan.Location = location;
        chan.Size = size;

      }
      catch { }

    }

    public void ErrorLog(string sErrMsg)
    {

      string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

      StreamWriter sw = new StreamWriter(LogPathName, true);
      sw.WriteLine(sLogFormat + sErrMsg);
      sw.Flush();
      sw.Close();
    }

    public void ParseXML()
    {
      string path = Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config);
      Data dboxdata = new Data(server, username, password, boxtype);
      StreamReader sr = new StreamReader(path + @"\dbox.xml", true);

      string Line = "";
      string Bouq = "";
      string Chan = "";

      int x = 0;
      int y = 0;

      int refNo = 0;

      string ChName = "";
      string ChId = "";

      _TV_Bouquets = null;
      _TV_Bouquets = new DataTable("BouquetsTV");

      _TV_Bouquets.Clear();
      _TV_Bouquets.Columns.Add("BouqNo", Type.GetType("System.String"));
      _TV_Bouquets.Columns.Add("BouqName", Type.GetType("System.String"));
      _TV_Bouquets.Columns.Add("ChanNo", Type.GetType("System.String"));
      _TV_Bouquets.Columns.Add("ID", Type.GetType("System.String"));
      _TV_Bouquets.Columns.Add("Name", Type.GetType("System.String"));

      int cntCh = 0;

      try
      {
        while (!sr.EndOfStream)
        {
          Line = sr.ReadLine();

          // Section = bouquet
          x = Line.IndexOf("section name");
          if (x > 0)
          {
            refNo++;
            cntCh = 0;
            x = Line.IndexOf("=");
            x = x + 2;
            Bouq = Line.Substring(x, Line.Length - x - 2);

            Bouq = Bouq.Replace("&amp;", "&");
            Log.Debug(Bouq);
          }
          // /Section = bouquet
          x = Line.IndexOf("/section");
          if (x > 0)
          {
            SelChan.Add(cntCh);
          }
          // Channel
          x = Line.IndexOf("entry name");
          if (x > 0)
          {
            cntCh++;
            x = Line.IndexOf("=");
            y = Line.IndexOf(">");
            x = x + 2;
            Chan = Line.Substring(x, y - x - 1);

            Chan = Chan.Replace("&amp;", "&");
            //Log.Debug(Chan);

            x = Line.IndexOf(">");
            y = Line.IndexOf("#", x + 1);
            ChName = Line.Substring((x + 1), (y - x - 1));
            ChName = ChName.Replace("&amp;", "&");
            //Log.Debug(ChNo);

            x = Line.IndexOf("<", x + 1);
            ChId = Line.Substring((y + 1), (x - y - 1));
            //Log.Debug(ChNo);

            row = _TV_Bouquets.NewRow();

            row["BouqNo"] = refNo.ToString();
            row["BouqName"] = Bouq;
            row["ChanNo"] = Chan;
            row["ID"] = ChId;
            row["Name"] = ChName;
            try // try to refresh epg data for current channel
            { // would take too long > 950 channels...
              // dboxdata.refreshEPG(ChId);
            }
            catch
            {
            }
            if (MaxBouq < refNo)
              MaxBouq = refNo;

            _TV_Bouquets.Rows.Add(row);
          }
        }
      }
      catch
      {
        //Line = Line;
      }
      sr.Close();
    }

    private void DisableOSD()
    {
      t_Chan = 0;
      t_Info = 0;
      t_OSD = 0;
      t_Sel = 0;

      chan.Visible = false;
      info.Visible = false;
      osd.Visible = false;
      sel.Visible = false;

    }

    void _Update_Tick(object sender, EventArgs e)
    {
      if (cGlobal.VLC_StartPlay) //(_Ply_VLC)
      {
        _cnt_Ply++; // delay playback
        if (_cnt_Ply > 1) // not neede for enigma ?
        {
          _cnt_Ply = 0;
          cGlobal.VLC_StartPlay = false;
          PlayVLC();
        }
      }

      if (t_OSD > 0)
      {
        osd.UpdateOSD(vlcControl.Player.Position);
      }

      if (t_Info > 0)
      {
        info.UpdateOSDInfo(vlcControl.Player.Position);
      }

      if (vlcControl != null)
      {
        double x = vlcControl.Player.Position;

        if (_started)
        {
          if (cGlobal.VLC_Live == false)
          {
            if (x == -20)
            {
              StopVLC();
              btnTVOnOff.Selected = false;
              DisableOSD();
              _isFullScreen = false;
            }

          }
        }
        else
        {
          if (x >= 0)
            _started = true;
        }
      }

      if (btnRecord.Selected == true)
      {
        if (startRecord != null)
        {
          foreach (GUIListItem item in recDirectory.GetDirectory(RecDir))
          {
            if (!item.IsFolder)
            {
              if (item.FileInfo.CreationTime >= startRecord)
              {
                // init record fields
                GUIPropertyManager.SetProperty("#Dbox.Record.channel", ActChannel);
                GUIPropertyManager.SetProperty("#Dbox.Record.title", item.FileInfo.Name);
                GUIPropertyManager.SetProperty("#Dbox.Record.start", item.FileInfo.CreationTime.ToLongTimeString());
                GUIPropertyManager.SetProperty("#Dbox.Record.stop", " ");
              }
            }
          }
        }
      }
      else
      {
        // erase record fields
        GUIPropertyManager.SetProperty("#Dbox.Record.channel", " ");
        GUIPropertyManager.SetProperty("#Dbox.Record.title", " ");
        GUIPropertyManager.SetProperty("#Dbox.Record.start", " ");
        GUIPropertyManager.SetProperty("#Dbox.Record.stop", " ");
      }


    }

    void _OSDTimer_Tick(object sender, EventArgs e)
    {
      //Debug.WriteLine(t_OSD + " ODS  - " + t_Chan + " Chan");

      if (cGlobal.VLC_StartPlay)
        btnTVOnOff.Selected = true;

      if (t_OSD > 0)
      {
        osd.Visible = true;
        osd.BringToFront();
        GUIGraphicsContext.form.Focus();
        t_OSD--;
      }
      else
      {
        osd.Visible = false;
        osd.SendToBack();
        Focus();
      }

      if (t_Info > 0)
      {
        info.Visible = true;
        info.BringToFront();
        t_Info--;
      }
      else
      {
        info.Visible = false;
        info.SendToBack();
        Focus();
      }

      if (t_Sel > 0)
      {
        sel.Visible = true;
        sel.BringToFront();

        t_Sel--;
      }
      else
      {
        sel.Visible = false;
        SelStr = "....";
        sel.SendToBack();
        Focus();
      }
      if (t_Chan > 0)
      {
        // high prio
        t_Info = 0;
        t_Sel = 0;
        t_OSD = 0;

        chan.Visible = true;
        chan.BringToFront();
        chan.Focus();
        t_Chan--;
      }
      else
      {
        chan.Visible = false;
        chan.SendToBack();
        Focus();
      }

      if ((chan.Visible == false) & (sel.Visible == false) & (osd.Visible == false) & (info.Visible == false))
      {
        vlcControl.BringToFront();
      }

    }

    void EPG_Refresh()
    {
      Data dboxdata = new Data(server, username, password, boxtype);
      try
      {
        while (0 != 1)
        {
          for (int i = 0; i < _TV_Bouquets.Rows.Count; i++)
          {
            if (_TV_Bouquets.Rows[i]["BouqNo"].ToString() == ActBouquetNo.ToString())
            {
              System.Threading.Thread.Sleep(2000);
              //dboxdata.refreshEPG(_TV_Bouquets.Rows[i]["ID"].ToString());
              //Debug.WriteLine("Refresh ID " + _TV_Bouquets.Rows[i]["ID"].ToString());
            }
          }
        }
      }
      catch
      {
        // if aborted by main thread
      }
    }

  }
}