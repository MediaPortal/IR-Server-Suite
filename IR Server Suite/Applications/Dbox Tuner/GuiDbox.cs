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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Dialogs;
using MediaPortal.Configuration;
using MediaPortal.Playlists;
using System.Xml;
using System.IO;
using System.Data;
using MediaPortal.Util;

namespace MediaPortal.GUI.Mydbox
{
    public class DboxGui : GUIWindow
    {
        public const int WindowID = 9900;

        #region Constructor
        public DboxGui()
        {
            GetID = (int)MyDboxGui.WindowID;
        }
        #endregion

        #region SkinControlAttributes
        [SkinControlAttribute(50)]
        protected GUIFacadeControl facadeview = null;
        [SkinControlAttribute(2)]
        protected GUIButtonControl btnFavoriet = null;
        [SkinControlAttribute(3)]
        protected GUIButtonControl btnRadio = null;
        [SkinControlAttribute(4)]
        protected GUIButtonControl btnRecordings = null;
        #endregion

        #region Variables
        //private DBox.Core _Dreambox = null;
        //PlayListPlayer playlistPlayer;

        // bools
        //private bool _ShowChannels = false;

        private static string server;
        private static string username;
        private static string password;

        private string _Url = "";
        private string _UserName = "";
        private string _Password = "";

        private static DataTable _TV_Bouquets = null;
        //private static DataTable _Radio_Bouquets = null;

        #endregion

        #region Overrides
        public override bool Init()
        {
            //playlistPlayer = PlayListPlayer.SingletonPlayer;
            LoadSettings();

            return Load(GUIGraphicsContext.Skin + @"\mydbox.xml");
        }

        #endregion

        #region Private Methods
        void LoadSettings()
          {
            string path = Path.Combine(Config.GetFolder(Config.Dir.Config), "MediaPortal.xml");
            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(path))
            //using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
            {
                server = xmlreader.GetValue("mydbox", "IP");
                username = xmlreader.GetValue("mydbox", "UserName");
                password = xmlreader.GetValue("mydbox", "Password");
            }

            ShowBoutiques(); 

          }
     
        void ShowBoutiques()
        {
            _Url = "http://" + server ;
            _UserName = "";
            _Password = "";

            //_ShowChannels = false;
            facadeview.Clear();

            //get bouquets
            Data _DBox = new Data(_Url, _UserName, _Password);
            _TV_Bouquets = _DBox.UserTVBouquets.Tables[0];

            foreach (DataRow row in _TV_Bouquets.Rows)
            {
                GUIListItem li = new GUIListItem();
                li.Label = row["Name"].ToString();
                facadeview.Add(li);
            }
        }
        void ShowChannels(GUIListItem guiListItem)
        {
            _Url = "http://192.168.0.100/";
            _UserName = "";
            _Password = "";

            //_ShowChannels = true;
            facadeview.Clear();
            Data _DBox = new Data(_Url, _UserName, _Password);
            string reference = GetChannelReference(guiListItem);
            //DataTable dt = _DBox.Channels(reference).Tables[0];
            //foreach (DataRow row in dt.Rows)
            {
             //   GUIListItem li = new GUIListItem();
             //   li.Label = row["Name"].ToString();
             //   facadeview.Add(li);
            }
            try
            {
                //string currentChannelName = _Dbox.CurrentChannel.Name;
                //GUIPropertyManager.SetProperty("#Play.Current.File", currentChannelName);
                //GUIPropertyManager.SetProperty("#Play.Current.Title", currentChannelName);
            }
            catch (Exception) { }
        }
        string GetChannelReference(GUIListItem guiListItem)
        {
            string reference = "";
            Data _DBox = new Data(_Url, _UserName, _Password);
            DataTable dt = _DBox.UserTVBouquets.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                if (guiListItem.Label == row["Name"].ToString())
                {
                    reference = row["Ref"].ToString();
                    break;
                }
            }
            return reference;
        }
        #endregion


    }




}