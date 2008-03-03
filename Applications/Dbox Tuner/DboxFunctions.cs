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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace DboxTuner
{

  class DboxFunctions
  {

    #region Variables

    string _url;
    string _userName;
    string _password;
    string _command = "/control/";
    string _boxType;

    #endregion Variables

    #region Constructor

    public DboxFunctions(string url, string username, string password, string boxtype)
    {
      _url      = "http://" + url;
      _userName = username;
      _password = password;
      _boxType  = boxtype;
    }

    #endregion Constructor

    public string GetID()
    {
      //ErrorLog("entered getid with " +_Url + " "+_UserName+" "+_Password+" "+_Boxtype);
      Request request = new Request(_url, _userName, _password);
      XmlDocument doc = new XmlDocument();
      XmlNode elem = doc.DocumentElement;

      string s = "";
      //get actual channel (ID)

      switch (_boxType)
      {
        case "Enigma v1":
          doc.LoadXml(request.PostData("/xml/streaminfo"));
          elem = doc.SelectSingleNode("/streaminfo/service/reference");
          s = elem.InnerText;
          break;

        case "Enigma v2":
          doc.LoadXml(request.PostData("/web/subservices"));
          elem = doc.SelectSingleNode("/e2servicelist/e2service/e2servicereference");
          s = elem.InnerText;
          break;

        default:
          s = request.PostData(_command + "zapto");
          s = s.Replace("\n", "");
          //ErrorLog("get channel "+s+" for "+_Boxtype);
          break;
      }

      return s;
    }
    public string ZapTo(string ID)
    {
      Request request = new Request(_url, _userName, _password);
      string s = "";
      //zap to channel (ID)
      switch (_boxType)
      {
        case "Enigma v1":
          s = request.PostData("/cgi-bin/zapTo?path=" + ID);
          return s;
        case "Enigma v2":
          s = request.PostData("/web/zap?ZapTo=" + ID);
          return s;
        default:
          s = request.PostData(_command + "zapto?" + ID);
          return s;
      }
    }
    public void WakeUp()
    {
      Request request = new Request(_url, _userName, _password);
      switch (_boxType)
      {
        case "Enigma v1":
          request.PostData("/cgi-bin/admin?command=wakeup");
          break;
        case "Enigma v2": // donno if wakeup is correct command
          request.PostData("/web/powerstate?newstate=wakeup");
          break;
        case "Neutrino": // off = wakeup
          request.PostData("/control/standby?off");
          break;
      }

    }
    public string SetSPTS()
    {
      Request request = new Request(_url, _userName, _password);
      string s = "";
      //set playback to spts only required for neutrino, (i think)
      // return ok for enigma
      if (_boxType != "Neutrino")
        s = "ok";
      else //send neutrino command
        s = request.PostData(_command + "system?setAViAExtPlayBack=spts");
      return s;
    }
    public string ToggleMute()
    {
      Request request = new Request(_url, _userName, _password);
      string s = "";
      // get status
      switch (_boxType)
      {
        case "Enigma v1":
          s = request.PostData("/cgi-bin/audio?mute=0");
          return s;
        case "Enigma v2":
          s = request.PostData("/web/vol?set=mute");
          return s;
        default:
          s = request.PostData(_command + "volume?status");
          if (s == "0")
            s = request.PostData(_command + "volume?mute");
          if (s == "1")
            s = request.PostData(_command + "volume?unmute");

          return s;
      }

    }
    public string GetEpgXml(string ID)
    {
      Request request = new Request(_url, _userName, _password);
      string s = "";
      // get epg from box (xml formatted)
      switch (_boxType)
      {
        case "Enigma v1":
          s = request.PostData("/xml/serviceepg?ref=" + ID);
          break;
        case "Enigma v2":
          s = request.PostData("/web/epgservice?ref=" + ID);
          break;
        default:
          s = request.PostData(_command + "epg?xml=true&channelid=" + ID + "&details=true"); // &max=20
          break;
      }
      s = s.Replace("&", "&amp;");
      return s;

    }
    public void ShowMessage(string message)
    {
      Request request = new Request(_url, _userName, _password);

      switch (_boxType)
      {
        case "Enigma v1":
          request.PostData("/cgi-bin/xmessage?timeout=10&caption=Message&body=" + message);
          break;

        case "Enigma v2":
          request.PostData("/web/message?type=1&timeout=10&text=" + message);
          break;

        default:
          request.PostData(_command + "message?popup=" + message);
          break;
      }
    }
    public string GetInfo()
    {
      Request request = new Request(_url, _userName, _password);
      string s = String.Empty;

      switch (_boxType)
      {
        case "Enigma v1":
          s = request.PostData("/xml/boxinfo");
          s = s.Replace("\n", " ");
          s = s.Replace("  ", "");
          break;

        case "Enigma v2":
          s = request.PostData("/web/about");
          s = s.Replace("\n", " ");
          break;

        default:
          s = request.PostData(_command + "info?version");
          s = s.Replace("\n", " ");
          break;
      }

      return s;
    }

  }

}
