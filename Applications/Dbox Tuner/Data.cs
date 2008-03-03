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
using System.Data;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace DboxTuner
{

  public class Data
  {

    string _url;
    string _userName;
    string _password;
    
    public static string _boxtype = String.Empty;
    
    private string _Command = "/control/";

    public Data(string url, string username, string password, string boxtype)
    {
      _url = url;
      _userName = username;
      _password = password;
      _boxtype = boxtype;
    }


    public DataSet UserTVBouquets
    {
      get
      {
        DboxFunctions dboxfunc = new DboxFunctions(_url, _userName, _password, _boxtype);
        //dboxfunc.ErrorLog("data.cs box = " + _boxtype);
        string command = "";
        string temp = "";
        string sreturn = "";
        Request request = new Request(_url, _userName, _password);

        switch (_boxtype)
        {
          case "Enigma v1":
            // get userbouquets (ref=4097:7:0:6:0:0:0:0:0:0:)
            sreturn = request.PostData("/cgi-bin/getServices?ref=4097:7:0:6:0:0:0:0:0:0:");

            // get internal hdd recording
            if (!request.PostData("/cgi-bin/getServices?ref=2:47:0:0:0:0:0:0:0:0:/var/media/movie/").Contains("E: "))
              sreturn += "2:47:0:0:0:0:0:0:0:0:/var/media/movie/;Recordings\n";

            // replace neutrino split character with ; 
            sreturn = sreturn.Replace(";", " ");
            sreturn = sreturn.Replace(" selected", ""); // removes enigma v1's selected tag in bouquets output

            //dboxfunc.ErrorLog("returned bouquets: " + sreturn);
            // set the bouquet command for this boxtype
            command = "/cgi-bin/getServices?ref=";
            break;

          case "Enigma v2":
            string serviceID = "";
            string serviceName = "";
            // xmlbased, return all userbouquets
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request.PostData("/web/fetchchannels?ServiceListBrowse=1:7:1:0:0:0:0:0:0:0:(type == 1) FROM BOUQUET \"bouquets.tv\" ORDER BY bouquet"));
            XmlNodeList nodelist = doc.GetElementsByTagName("e2service");
            
            foreach (XmlNode docnode in nodelist)
            {
              foreach (XmlNode datanode in docnode)
              {
                switch (datanode.Name)
                {
                  case "e2servicereference":
                    serviceID = datanode.InnerText;
                    break;

                  case "e2servicename":
                    serviceName = datanode.InnerText.Replace(" (TV)", "");
                    break;
                }
              }
              sreturn += serviceID + ";" + serviceName + "\n"; // make a list of all the userbouquets
            }
            
            command = "/web/fetchchannels?ServiceListBrowse=";
            //dboxfunc.ErrorLog("returned bouquets: " + sreturn);
            break;

          default:
            sreturn = request.PostData(_Command + "getbouquets");
            // set the bouquet command for this boxtype
            command = (_Command + "getbouquet?bouquet=");
            break;
        }

        string[] allBouquets = sreturn.Split('\n'); // split the list of userbouquets

        // convert to dataset
        DataSet ds = new DataSet();
        DataTable table = new DataTable("BouquetsTV");
        DataRow row = null;

        try
        {
          table.Columns.Add("BouqNo", Type.GetType("System.String"));
          table.Columns.Add("BouqName", Type.GetType("System.String"));
          table.Columns.Add("Channel", Type.GetType("System.String"));
          table.Columns.Add("ID", Type.GetType("System.String"));
          table.Columns.Add("Name", Type.GetType("System.String"));

          int loopcount = 0;
          foreach (string s in allBouquets)
          {
            if (s != "")
            {
              ////dboxfunc.ErrorLog("on top of foreach allbouquet");
              // split the bouquet id from bouquet name
              // s = "0 ServiceID, 1 Name of the Bouquets"
              if (_boxtype == "Enigma v2")
                temp = s.Split(';')[0];                //enigma2 splitchar is ";"
              else
                temp = s.Split(' ')[0];                                        //otherboxes splitchar is " "
              
              //dboxfunc.ErrorLog("splitted string to temp: " + temp);
              
              if (_boxtype == "Neutrino")
                _Command = command + temp + "&mode=TV";  //build neutrino command
              else
                _Command = command + temp;                                      //build enigma command
              
              //dboxfunc.ErrorLog("sending command: " + _Command);
              
              sreturn = request.PostData(_Command);                                //request list of channels contained in bouquetID "temp"
              sreturn = sreturn.Replace(";selected", "");
              //dboxfunc.ErrorLog("sent command and returned: " + sreturn);
              
              if (_boxtype == "Enigma v2")
              {
                string serviceID = "";
                string serviceName = "";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sreturn);
                XmlNodeList nodelist = doc.GetElementsByTagName("e2service");
                sreturn = "";
                foreach (XmlNode docnode in nodelist)
                {
                  foreach (XmlNode datanode in docnode)
                  {
                    switch (datanode.Name)
                    {
                      case "e2servicereference":
                        serviceID = datanode.InnerText;
                        break;
                      case "e2servicename":
                        serviceName = datanode.InnerText;
                        break;
                    }
                  }
                  sreturn += serviceID + ";" + serviceName + "\n"; // make a list of all the channels
                }
              }

              string[] OneBouquet = sreturn.Split('\n');
              string bucket = "";
              ////dboxfunc.ErrorLog("starting onebouquet again");
              if (OneBouquet[0] != "")
              {

                foreach (string bouquets in OneBouquet)
                {

                  //dboxfunc.ErrorLog("on top of onebouquets foreach");
                  if (bouquets != "")
                  {
                    //dboxfunc.ErrorLog("string is: " + bouquets);
                    if ((bouquets.IndexOf(' ') > -1) || (bouquets.IndexOf(';') > -1))
                    {
                      row = table.NewRow();
                      //dboxfunc.ErrorLog("created new row");
                      int start = 0;
                      // modifying the enigma string so it's the same as neutrino
                      // that way I don't need to rewrite this textfilter
                      // using xml for enigma 1 is a very bad solution as the xml functions do not accept bouquet ids.
                      // which => one webrequest for each channel in every bouquet
                      if (_boxtype == "Enigma v1" || _boxtype == "Enigma v2")
                      {
                        string chan_id = bouquets.Split(';')[0]; // f.ex.: 1:0:1:6D67:437:1:C00000:0:0:0:
                        string chan_name = bouquets.Split(';')[1]; //f.eks DISCOVERY CHANNEL
                        if (chan_id.StartsWith("1:0:1") || chan_id.StartsWith("1:0:0") && chan_name != "<n/a>" && chan_name != "") // if chan_id is a TV service and chan_name is NOT <n/a> or emty
                          bucket = Convert.ToString(++loopcount) + " " + chan_id + " " + chan_name;

                      }
                      //dboxfunc.ErrorLog("starting string functions");
                      //dboxfunc.ErrorLog("split tmp_ref: " + s + " number of chars: " + s.Length);

                      if (_boxtype == "Neutrino")
                        bucket = bouquets;
                      
                      String tmp_Ref;
                      
                      if (_boxtype == "Enigma v2")
                        tmp_Ref = s.Split(';')[0];                //enigma2 splitchar is ";"
                      else
                        tmp_Ref = s.Split(' ')[0];                                        //otherboxes splitchar is " "
                      
                      start = tmp_Ref.Length + 1;
                      String tmp_Bouquet = s.Substring(start, s.Length - start);
                      //dboxfunc.ErrorLog("split tmp_bouq, bucket is: " + bucket);

                      String tmp_Channel = bucket.Split(' ')[0];
                      //dboxfunc.ErrorLog("split channel");
                      String tmp_ID = bucket.Split(' ')[1];
                      
                      if (_boxtype == "Enigma v1")
                        tmp_ID = tmp_ID.Replace("1:0:0:0:0:0:0:0:0:0:", _url + "/rootX");  //workaround for the inability to stream internal recordings from the enigma hdd

                      //dboxfunc.ErrorLog("split ID");
                      start = tmp_Channel.Length + tmp_ID.Length + 2;
                      String tmp_Name = bucket.Substring(start, bucket.Length - start);
                      //dboxfunc.ErrorLog("split name");
                      tmp_Name = tmp_Name.Replace("\"", "'");
                      //dboxfunc.ErrorLog("ended string functions");
                      
                      row["BouqNo"] = tmp_Ref;
                      row["BouqName"] = tmp_Bouquet;
                      row["Channel"] = tmp_Channel;
                      row["ID"] = tmp_ID;
                      row["Name"] = @tmp_Name;
                      
                      //dboxfunc.ErrorLog("ended row functions");
                      //dboxfunc.ErrorLog("added: " + tmp_Ref + "channel: " + tmp_Channel + "id: " + tmp_ID + "name: " + tmp_Name);
                      // test if enigma got a error on service list
                      //dboxfunc.ErrorLog("trying to add row" + tmp_Channel);
                      table.Rows.Add(row);
                      //dboxfunc.ErrorLog("Added row" + tmp_Channel);

                      if (tmp_ID == "E:" || tmp_Name == "<n/a>")
                      {
                        // kill the row or we get error
                        //dboxfunc.ErrorLog("trying to remove row" + tmp_Channel);
                        table.Rows.Remove(row);
                        //dboxfunc.ErrorLog("removed row" + tmp_Channel);
                      }
                    }
                  }
                }
              }
            }


          }
          ds.Tables.Add(table);
        }
        catch
        {

        }
        //dboxfunc.ErrorLog("returning bouquets dataset");
        return ds;
      }
    }

  }

}
