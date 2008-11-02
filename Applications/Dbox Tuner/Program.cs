using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IrssUtils;

namespace DboxTuner
{

  #region Enumerations

  internal enum StbBoxType
  {
    Unknown,
    EnigmaV1,
    EnigmaV2,
    Neutrino,
  }

  #endregion Enumerations

  /// <summary>
  /// Based on MyDbox MediaPortal plugin by Mark Koenig (kroko).
  /// </summary>
  internal static class Program
  {
    #region Constants

    internal const string UrlPrefix = "http://";

    internal static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, "Dbox Tuner\\Dbox Tuner.xml");
    internal static readonly string DataFile = Path.Combine(Common.FolderAppData, "Dbox Tuner\\Data.xml");

    #endregion Constants

    #region Variables

    private static string _address;
    private static StbBoxType _boxType;
    private static string _password;
    private static int _timeout;

    private static DataTable _tvBouquets;
    private static string _url;
    private static string _userName;

    #endregion Variables

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command line parameters.</param>
    [STAThread]
    private static void Main(string[] args)
    {
      Console.WriteLine("Dbox Tuner");
      Console.WriteLine();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Append("Dbox Tuner.log");

      LoadSettings();

      if (args.Length == 0)
      {
        Console.WriteLine("Usage:");
        Console.WriteLine("       DboxTuner.exe Setup");
        Console.WriteLine("or");
        Console.WriteLine("       DboxTuner.exe Wake");
        Console.WriteLine("or");
        Console.WriteLine("       DboxTuner.exe Mute");
        Console.WriteLine("or");
        Console.WriteLine("       DboxTuner.exe SetSPTS");
        Console.WriteLine("or");
        Console.WriteLine("       DboxTuner.exe Message <Message Text>");
        Console.WriteLine("or");
        Console.WriteLine("       DboxTuner.exe Zap <Channel Number>");
        Console.WriteLine();
      }
      else
      {
        _url = UrlPrefix + _address;

        switch (args[0].ToUpperInvariant())
        {
          case "SETUP":
            SetupForm setup = new SetupForm();
            setup.Address = _address;
            setup.UserName = _userName;
            setup.Password = _password;
            setup.BoxType = _boxType;
            setup.Timeout = _timeout;

            if (setup.ShowDialog() == DialogResult.OK)
            {
              _address = setup.Address;
              _userName = setup.UserName;
              _password = setup.Password;
              _boxType = setup.BoxType;
              _timeout = setup.Timeout;

              SaveSettings();
              Info("Setup saved");
            }
            else
            {
              Info("Setup cancelled");
            }
            break;

          case "WAKE":
            Info("Command: Wake");
            WakeUp();
            break;

          case "MUTE":
            Info("Command: Mute");
            ToggleMute();
            break;

          case "MESSAGE":
            Info("Command: Message \"{0}\"", args[1]);
            ShowMessage(args[1]);
            break;

          case "SETSPTS":
            Info("Command: Set SPTS");
            SetSPTS();
            break;

          case "INFO":
            Info("Command: Get Info");
            string output = GetInfo();
            Info(output);
            break;

          case "ZAP":
            Info("Command: Zap, Channel: {0}", args[1]);

            try
            {
              Info("Raising process priority");
              Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            }
            catch
            {
              Info("Failed to elevate process priority");
            }

            _tvBouquets = new DataTable();
            _tvBouquets.ReadXml(DataFile);

            string expression = String.Format("Channel = '{0}'", args[1]); // Works on EnigmaV1, others unknown.

            /*
            string expression;
            switch (_boxType)
            {
              case StbBoxType.EnigmaV1: expression = String.Format("Channel = '{0}'", args[1]); break;
              case StbBoxType.EnigmaV2: expression = String.Format("Channel = '{0}'", args[1]); break;
              default: expression = String.Format("Channel = '{0}'", args[1]); break;
            }
            */

            DataRow[] rows = _tvBouquets.Select(expression);
            if (rows.Length == 1)
            {
              //string channelName = rows[0]["ChannelName"] as string;
              string channelID = rows[0]["ID"] as string;

              //Info("Zapping to channel {0} \"{1}\"", channelName, channelID);

              ZapTo(channelID);
              break;
            }
            else if (rows.Length > 0)
            {
              Info("Multiple channels match Channel No \"{0}\"", args[1]);
            }

            Info("Cannot find channel ({0}) to tune", args[1]);
            break;

          default:
            Info("Unknown Command \"{0}\"", args[0]);
            break;
        }
      }

      IrssLog.Close();
    }

    private static void Info(string format, params object[] args)
    {
      string message = String.Format(format, args);
      IrssLog.Info(message);
      Console.WriteLine(message);
    }

    private static void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _address = doc.DocumentElement.Attributes["Address"].Value;
        _userName = doc.DocumentElement.Attributes["UserName"].Value;
        _password = doc.DocumentElement.Attributes["Password"].Value;
        _boxType = (StbBoxType) Enum.Parse(typeof (StbBoxType), doc.DocumentElement.Attributes["BoxType"].Value, true);
        _timeout = int.Parse(doc.DocumentElement.Attributes["Timeout"].Value);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("Configuration file not found, using defaults");

        CreateDefaultSettings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        CreateDefaultSettings();
      }
    }

    private static void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("Address", _address);
          writer.WriteAttributeString("UserName", _userName);
          writer.WriteAttributeString("Password", _password);
          writer.WriteAttributeString("BoxType", Enum.GetName(typeof (StbBoxType), _boxType));
          writer.WriteAttributeString("Timeout", _timeout.ToString());

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

    private static void CreateDefaultSettings()
    {
      _address = "192.168.0.100";
      _userName = "root";
      _password = "dbox2";
      _boxType = StbBoxType.Unknown;
      _timeout = 4000;

      SaveSettings();
    }

    internal static string PostData(string url, string userName, string password, StbBoxType boxType, int timeout,
                                    string command)
    {
      try
      {
        Uri uri = new Uri(url + command);
        WebRequest request = WebRequest.Create(uri);
        request.Credentials = new NetworkCredential(userName, password);
        request.Timeout = timeout;

        // back to iso encoding sorry , should work anywhere in EU
        // which it doesn't, because dreambox use utf-8 encoding, making all UTF-8 extended characters a multibyte garble if we encode those to iso

        Encoding encode = Encoding.GetEncoding("iso-8859-1");
        if (boxType == StbBoxType.EnigmaV1 || boxType == StbBoxType.EnigmaV2)
          encode = Encoding.GetEncoding("utf-8");

        using (WebResponse response = request.GetResponse())
        using (Stream receiveStream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(receiveStream, encode))
          return reader.ReadToEnd();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        return String.Empty;
      }
    }

    internal static DataSet GetData(string url, string userName, string password, StbBoxType boxType, int timeout)
    {
      DataSet ds = new DataSet();

      string command;
      string temp;

      string sreturn = String.Empty;

      #region Get data from STB

      switch (boxType)
      {
        case StbBoxType.EnigmaV1:
          // get userbouquets (ref=4097:7:0:6:0:0:0:0:0:0:)
          sreturn = PostData(url, userName, password, boxType, timeout,
                             "/cgi-bin/getServices?ref=4097:7:0:6:0:0:0:0:0:0:");

          // get internal hdd recording
          if (
            !PostData(url, userName, password, boxType, timeout,
                      "/cgi-bin/getServices?ref=2:47:0:0:0:0:0:0:0:0:/var/media/movie/").Contains("E: "))
            sreturn += "2:47:0:0:0:0:0:0:0:0:/var/media/movie/;Recordings\n";

          // replace neutrino split character with ; 
          sreturn = sreturn.Replace(";", " ");
          sreturn = sreturn.Replace(" selected", ""); // removes enigma v1's selected tag in bouquets output

          // set the bouquet command for this boxtype
          command = "/cgi-bin/getServices?ref=";
          break;

        case StbBoxType.EnigmaV2:
          string serviceID = String.Empty;
          string serviceName = String.Empty;

          string returnedXml = PostData(url, userName, password, boxType, timeout,
                                        "/web/fetchchannels?ServiceListBrowse=1:7:1:0:0:0:0:0:0:0:(type == 1) FROM BOUQUET \"bouquets.tv\" ORDER BY bouquet");

          // xmlbased, return all userbouquets
          XmlDocument doc = new XmlDocument();
          doc.LoadXml(returnedXml);
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
          break;

        default:
          sreturn = PostData(url, userName, password, boxType, timeout, "/control/getbouquets");
          // set the bouquet command for this boxtype
          command = "/control/getbouquet?bouquet=";
          break;
      }

      #endregion Get data from STB

      #region Convert data to dataset

      DataTable table = new DataTable("BouquetsTV");
      DataRow row = null;

      string[] allBouquets = sreturn.Split('\n'); // split the list of userbouquets

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
          if (!String.IsNullOrEmpty(s))
          {
            string curCommand;

            // split the bouquet id from bouquet name
            // s = "0 ServiceID, 1 Name of the Bouquets"
            if (boxType == StbBoxType.EnigmaV2) //enigma2 splitchar is ";"
              temp = s.Split(';')[0];
            else //otherboxes splitchar is " "
              temp = s.Split(' ')[0];

            if (boxType == StbBoxType.Neutrino) //build neutrino command
              curCommand = command + temp + "&mode=TV";
            else //build enigma command
              curCommand = command + temp;


            //request list of channels contained in bouquetID "temp"
            sreturn = PostData(url, userName, password, boxType, timeout, curCommand);
            sreturn = sreturn.Replace(";selected", "");

            if (boxType == StbBoxType.EnigmaV2)
            {
              string serviceID = String.Empty;
              string serviceName = String.Empty;

              XmlDocument doc = new XmlDocument();
              doc.LoadXml(sreturn);
              XmlNodeList nodelist = doc.GetElementsByTagName("e2service");
              sreturn = String.Empty;
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
            string bucket = String.Empty;

            if (!String.IsNullOrEmpty(OneBouquet[0]))
            {
              foreach (string bouquets in OneBouquet)
              {
                if (!String.IsNullOrEmpty(bouquets))
                {
                  if ((bouquets.IndexOf(' ') > -1) || (bouquets.IndexOf(';') > -1))
                  {
                    row = table.NewRow();

                    int start = 0;
                    // modifying the enigma string so it's the same as neutrino
                    // that way I don't need to rewrite this textfilter
                    // using xml for enigma 1 is a very bad solution as the xml functions do not accept bouquet ids.
                    // which => one webrequest for each channel in every bouquet
                    if (boxType == StbBoxType.EnigmaV1 || boxType == StbBoxType.EnigmaV2)
                    {
                      string chan_id = bouquets.Split(';')[0]; // eg. 1:0:1:6D67:437:1:C00000:0:0:0:
                      string chan_name = bouquets.Split(';')[1]; // eg. DISCOVERY CHANNEL

                      // if chan_id is a TV service and chan_name is NOT <n/a> or empty
                      if (chan_id.StartsWith("1:0:1", StringComparison.Ordinal) ||
                          chan_id.StartsWith("1:0:0", StringComparison.Ordinal) && !String.IsNullOrEmpty(chan_name) &&
                          !chan_name.Equals("<n/a>", StringComparison.OrdinalIgnoreCase))
                        bucket = Convert.ToString(++loopcount) + " " + chan_id + " " + chan_name;
                    }
                    else if (boxType == StbBoxType.Neutrino)
                      bucket = bouquets;

                    string tmp_Ref;

                    if (boxType == StbBoxType.EnigmaV2) //enigma2 splitchar is ";"
                      tmp_Ref = s.Split(';')[0];
                    else //otherboxes splitchar is " "
                      tmp_Ref = s.Split(' ')[0];

                    start = tmp_Ref.Length + 1;
                    string tmp_Bouquet = s.Substring(start, s.Length - start);

                    string tmp_Channel = bucket.Split(' ')[0];
                    string tmp_ID = bucket.Split(' ')[1];

                    if (boxType == StbBoxType.EnigmaV1)
                      tmp_ID = tmp_ID.Replace("1:0:0:0:0:0:0:0:0:0:", _url + "/rootX");
                    //workaround for the inability to stream internal recordings from the enigma hdd

                    start = tmp_Channel.Length + tmp_ID.Length + 2;
                    string tmp_Name = bucket.Substring(start, bucket.Length - start);
                    tmp_Name = tmp_Name.Replace("\"", "'");

                    row["BouqNo"] = tmp_Ref;
                    row["BouqName"] = tmp_Bouquet;
                    row["Channel"] = tmp_Channel;
                    row["ID"] = tmp_ID;
                    row["Name"] = tmp_Name;

                    // test if enigma got a error on service list
                    table.Rows.Add(row);

                    if (tmp_ID.Equals("E:", StringComparison.OrdinalIgnoreCase) ||
                        tmp_Name.Equals("<n/a>", StringComparison.OrdinalIgnoreCase))
                    {
                      // kill the row or we get error
                      table.Rows.Remove(row);
                    }
                  }
                }
              }
            }
          }
        }
        ds.Tables.Add(table);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      #endregion Convert data to dataset

      return ds;
    }

    internal static StbBoxType DetectBoxType(string url, string userName, string password, int timeout)
    {
      string str1 =
        PostData(url, userName, password, StbBoxType.Unknown, timeout, "/control/getmode").ToUpperInvariant();
      if (str1.Contains("TV") || str1.Contains("RADIO") || str1.Contains("UNKNOWN"))
        return StbBoxType.Neutrino;

      string str2 = PostData(url, userName, password, StbBoxType.Unknown, timeout, "/cgi-bin/status").ToUpperInvariant();
      if (str2.Contains("ENIGMA"))
        return StbBoxType.EnigmaV1;

      string str3 = PostData(url, userName, password, StbBoxType.Unknown, timeout, "/web/stream.m3u").ToUpperInvariant();
      if (str3.Contains("#EXTM3U"))
        return StbBoxType.EnigmaV2;

      return StbBoxType.Unknown;
    }

    private static string ZapTo(string ID)
    {
      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          return PostData(_url, _userName, _password, _boxType, _timeout, "/cgi-bin/zapTo?path=" + ID);
        case StbBoxType.EnigmaV2:
          return PostData(_url, _userName, _password, _boxType, _timeout, "/web/zap?ZapTo=" + ID);
        default:
          return PostData(_url, _userName, _password, _boxType, _timeout, "/control/zapto?" + ID);
      }
    }

    private static void WakeUp()
    {
      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          PostData(_url, _userName, _password, _boxType, _timeout, "/cgi-bin/admin?command=wakeup");
          break;

        case StbBoxType.EnigmaV2: // donno if wakeup is correct command
          PostData(_url, _userName, _password, _boxType, _timeout, "/web/powerstate?newstate=wakeup");
          break;

        case StbBoxType.Neutrino: // off = wakeup
          PostData(_url, _userName, _password, _boxType, _timeout, "/control/standby?off");
          break;
      }
    }

    private static string SetSPTS()
    {
      //set playback to spts only required for neutrino, (i think)

      if (_boxType == StbBoxType.Neutrino) //send neutrino command
        return PostData(_url, _userName, _password, _boxType, _timeout, "/control/system?setAViAExtPlayBack=spts");
      else // return ok for enigma
        return "ok";
    }

    private static string ToggleMute()
    {
      string status;

      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          status = PostData(_url, _userName, _password, _boxType, _timeout, "/cgi-bin/audio?mute=0");
          break;

        case StbBoxType.EnigmaV2:
          status = PostData(_url, _userName, _password, _boxType, _timeout, "/web/vol?set=mute");
          break;

        default:
          status = PostData(_url, _userName, _password, _boxType, _timeout, "/control/volume?status");
          if (status.Equals("0", StringComparison.Ordinal))
            status = PostData(_url, _userName, _password, _boxType, _timeout, "/control/volume?mute");
          if (status.Equals("1", StringComparison.Ordinal))
            status = PostData(_url, _userName, _password, _boxType, _timeout, "/control/volume?unmute");
          break;
      }

      return status;
    }

    private static void ShowMessage(string message)
    {
      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          PostData(_url, _userName, _password, _boxType, _timeout,
                   "/cgi-bin/xmessage?timeout=10&caption=Message&body=" + message);
          break;

        case StbBoxType.EnigmaV2:
          PostData(_url, _userName, _password, _boxType, _timeout, "/web/message?type=1&timeout=10&text=" + message);
          break;

        default:
          PostData(_url, _userName, _password, _boxType, _timeout, "/control/message?popup=" + message);
          break;
      }
    }

    private static string GetInfo()
    {
      string info;

      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          info = PostData(_url, _userName, _password, _boxType, _timeout, "/xml/boxinfo");
          info = info.Replace("\n", " ");
          info = info.Replace("  ", "");
          break;

        case StbBoxType.EnigmaV2:
          info = PostData(_url, _userName, _password, _boxType, _timeout, "/web/about");
          info = info.Replace("\n", " ");
          break;

        default:
          info = PostData(_url, _userName, _password, _boxType, _timeout, "/control/info?version");
          info = info.Replace("\n", " ");
          break;
      }

      return info;
    }

    // Unused ...
    /*
    static string GetEpgXml(string ID)
    {
      string epgXml;

      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          epgXml = PostData(_url, _userName, _password, _boxType, _timeout, "/xml/serviceepg?ref=" + ID);
          break;

        case StbBoxType.EnigmaV2:
          epgXml = PostData(_url, _userName, _password, _boxType, _timeout, "/web/epgservice?ref=" + ID);
          break;

        default:
          epgXml = PostData(_url, _userName, _password, _boxType, _timeout, "/control/epg?xml=true&channelid=" + ID + "&details=true"); // &max=20
          break;
      }

      epgXml = epgXml.Replace("&", "&amp;");

      return epgXml;
    }
    static string GetID()
    {
      XmlDocument doc = new XmlDocument();
      XmlNode elem = doc.DocumentElement;

      string id;
      string xml;

      switch (_boxType)
      {
        case StbBoxType.EnigmaV1:
          xml = PostData(_url, _userName, _password, _boxType, _timeout, "/xml/streaminfo");
          doc.LoadXml(xml);
          elem = doc.SelectSingleNode("/streaminfo/service/reference");
          id = elem.InnerText;
          break;

        case StbBoxType.EnigmaV2:
          xml = PostData(_url, _userName, _password, _boxType, _timeout, "/web/subservices"); 
          doc.LoadXml(xml);
          elem = doc.SelectSingleNode("/e2servicelist/e2service/e2servicereference");
          id = elem.InnerText;
          break;

        default:
          id = PostData(_url, _userName, _password, _boxType, _timeout, "/control/zapto");
          id = id.Replace("\n", "");
          break;
      }

      return id;
    }
    */
  }
}