using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;

namespace DboxTuner
{

  /// <summary>
  /// Based on MyDbox MediaPortal plugin by Mark Koenig (kroko).
  /// </summary>
  static class Program
  {

    #region Constants

    internal static readonly string ConfigurationFile = Path.Combine(Common.FolderAppData, "Dbox Tuner\\Dbox Tuner.xml");
    internal static readonly string DataFile          = Path.Combine(Common.FolderAppData, "Dbox Tuner\\Data.xml");

    #endregion Constants

    #region Variables

    static string _address;
    static string _userName;
    static string _password;
    static string _boxType;
    static DataTable _tvBouquets;

    #endregion Variables

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command line parameters.</param>
    [STAThread]
    static void Main(string[] args)
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
        DboxFunctions func = new DboxFunctions(_address, _userName, _password, _boxType);

        switch (args[0].ToUpperInvariant())
        {
          case "SETUP":
            SetupForm setup = new SetupForm();
            setup.Address  = _address;
            setup.UserName = _userName;
            setup.Password = _password;
            setup.BoxType  = _boxType;

            if (setup.ShowDialog() == DialogResult.OK)
            {
              _address  = setup.Address;
              _userName = setup.UserName;
              _password = setup.Password;
              _boxType  = setup.BoxType;

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
            func.WakeUp();
            break;

          case "MUTE":
            Info("Command: Mute");
            func.ToggleMute();
            break;

          case "MESSAGE":
            Info("Command: Message \"{0}\"", args[1]);
            func.ShowMessage(args[1]);
            break;

          case "SETSPTS":
            Info("Command: Set SPTS");
            func.SetSPTS();
            break;

          case "INFO":
            Info("Command: Get Info");
            string output = func.GetInfo();
            Info(output);
            break;

          case "ZAP":
            Info("Command: Zap, Channel: {0}", args[1]);

            _tvBouquets = new DataTable();
            _tvBouquets.ReadXml(DataFile);

            string expression = String.Format("ChanNo = '{0}'", args[1]);

            DataRow[] rows = _tvBouquets.Select(expression);
            if (rows.Length == 1)
            {
              string channelID = rows[0]["ID"] as string;
              func.ZapTo(channelID);
              break;
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

    static void Info(string format, params object[] args)
    {
      string message = String.Format(format, args);
      IrssLog.Info(message);
      Console.WriteLine(message);
    }
    
    static void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _address  = doc.DocumentElement.Attributes["Address"].Value;
        _userName = doc.DocumentElement.Attributes["UserName"].Value;
        _password = doc.DocumentElement.Attributes["Password"].Value;
        _boxType  = doc.DocumentElement.Attributes["BoxType"].Value;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _address  = "192.168.0.100";
        _userName = "root";
        _password = "dbox2";
        _boxType  = String.Empty;
      }
    }
    static void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("Address", _address);
          writer.WriteAttributeString("UserName", _userName);
          writer.WriteAttributeString("Password", _password);
          writer.WriteAttributeString("BoxType", _boxType);

          writer.WriteEndElement(); // </settings>
          writer.WriteEndDocument();
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }

  }

}
