using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;

namespace MacroConverter
{

  public partial class FormMain : Form
  {

    public const string XmlTagMacro = "MACRO";
    public const string XmlTagBlast = "BLAST";
    public const string XmlTagPause = "PAUSE";
    public const string XmlTagRun = "RUN";
    public const string XmlTagSerial = "SERIAL";
    public const string XmlTagKeys = "KEYS";
    public const string XmlTagWindowMsg = "WINDOW_MESSAGE";
    public const string XmlTagTcpMsg = "TCP_MESSAGE";
    public const string XmlTagGoto = "GOTO";
    public const string XmlTagPopup = "POPUP";
    public const string XmlTagMultiMap = "MULTI_MAPPING";
    public const string XmlTagMouseMode = "MOUSE_MODE";
    public const string XmlTagInputLayer = "INPUT_LAYER";
    public const string XmlTagWindowState = "WINDOW_STATE";
    public const string XmlTagFocus = "GET_FOCUS";
    public const string XmlTagExit = "EXIT";

    public const string XmlTagStandby = "STANDBY";
    public const string XmlTagHibernate = "HIBERNATE";
    public const string XmlTagReboot = "REBOOT";
    public const string XmlTagShutdown = "SHUTDOWN";
    public const string XmlTagLogOff = "LOG_OFF";

    public const string XmlTagMouse = "MOUSE";
    public const string XmlTagEject = "EJECT";
    public const string XmlTagSound = "SOUND";
    public const string XmlTagBeep = "BEEP";
    public const string XmlTagDisplay = "DISPLAY";

    public const string XmlTagTranslator = "TRANSLATOR";
    public const string XmlTagVirtualKB = "VIRTUAL_KEYBOARD";
    public const string XmlTagSmsKB = "SMS_KEYBOARD";



    public FormMain()
    {
      InitializeComponent();
    }

    private void buttonGO_Click(object sender, EventArgs e)
    {
      listViewStatus.Items.Add("Translator Macros ...");
      ProcessFolder(Common.FolderAppData + "Translator\\Macro\\");

      listViewStatus.Items.Add("MP Blast Zone Plugin Macros ...");
      ProcessFolder(Common.FolderAppData + "MP Blast Zone Plugin\\Macro\\");

      listViewStatus.Items.Add("MP Control Plugin Macros ...");
      ProcessFolder(Common.FolderAppData + "MP Control Plugin\\Macro\\");

      listViewStatus.Items.Add("TV2 Blaster Plugin Macros ...");
      ProcessFolder(Common.FolderAppData + "TV2 Blaster Plugin\\Macro\\");

      listViewStatus.Items.Add("TV3 Blaster Plugin Macros ...");
      ProcessFolder(Common.FolderAppData + "TV3 Blaster Plugin\\Macro\\");

      listViewStatus.Items.Add("MCE Replacement Plugin Macros ...");
      ProcessFolder(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\MediaPortal MCE Replacement Plugin\\Macro\\");

      listViewStatus.Items.Add("Done.");
    }

    void ProcessFolder(string folder)
    {
      try
      {
        string[] macros = Directory.GetFiles(folder, '*' + Common.FileExtensionMacro);
        ProcessMacros(macros);
      }
      catch (Exception ex)
      {
        listViewStatus.Items.Add(ex.Message);
      }
    }

    void ProcessMacros(string[] macros)
    {
      string[] macroContents;

      foreach (string macro in macros)
      {
        try
        {
          listViewStatus.Items.Add(Path.GetFileName(macro));

          macroContents = LoadMacro(macro);
          SaveMacro(macro, macroContents);
        }
        catch (Exception ex)
        {
          listViewStatus.Items.Add(ex.Message);
        }
      }
    }


    string[] LoadMacro(string fileName)
    {
      List<string> macroContents = new List<string>();

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");

      string commandProperty;
      foreach (XmlNode item in commandSequence)
      {
        commandProperty = item.Attributes["cmdproperty"].Value;

        switch (item.Attributes["command"].Value)
        {
          case XmlTagMacro:
            macroContents.Add(Common.CmdPrefixMacro + commandProperty);
            break;

          case XmlTagBlast:
            macroContents.Add(Common.CmdPrefixBlast + commandProperty);
            break;

          case XmlTagPause:
            macroContents.Add(Common.CmdPrefixPause + commandProperty);
            break;

          case XmlTagRun:
            macroContents.Add(Common.CmdPrefixRun + commandProperty);
            break;

          case XmlTagSerial:
            macroContents.Add(Common.CmdPrefixSerial + commandProperty);
            break;

          case XmlTagWindowMsg:
            macroContents.Add(Common.CmdPrefixWindowMsg + commandProperty);
            break;

          case XmlTagTcpMsg:
            macroContents.Add(Common.CmdPrefixTcpMsg + commandProperty);
            break;

          case XmlTagKeys:
            macroContents.Add(Common.CmdPrefixKeys + commandProperty);
            break;

          case XmlTagMouse:
            macroContents.Add(Common.CmdPrefixMouse + commandProperty);
            break;

          case XmlTagEject:
            macroContents.Add(Common.CmdPrefixEject + commandProperty);
            break;

          case XmlTagGoto:
            macroContents.Add(Common.CmdPrefixGoto + commandProperty);
            break;

          case XmlTagPopup:
            macroContents.Add(Common.CmdPrefixPopup + commandProperty);
            break;

          case XmlTagMultiMap:
            macroContents.Add(Common.CmdPrefixMultiMap + commandProperty);
            break;

          case XmlTagMouseMode:
            macroContents.Add(Common.CmdPrefixMouseMode + commandProperty);
            break;

          case XmlTagInputLayer:
            macroContents.Add(Common.CmdPrefixInputLayer);
            break;

          case XmlTagFocus:
            macroContents.Add(Common.CmdPrefixFocus);
            break;

          case XmlTagExit:
            macroContents.Add(Common.CmdPrefixExit);
            break;

          case XmlTagStandby:
            macroContents.Add(Common.CmdPrefixStandby);
            break;

          case XmlTagHibernate:
            macroContents.Add(Common.CmdPrefixHibernate);
            break;

          case XmlTagReboot:
            macroContents.Add(Common.CmdPrefixReboot);
            break;

          case XmlTagShutdown:
            macroContents.Add(Common.CmdPrefixShutdown);
            break;

          case XmlTagBeep:
            macroContents.Add(Common.CmdPrefixBeep + commandProperty);
            break;

          case XmlTagDisplay:
            macroContents.Add(Common.CmdPrefixDisplay + commandProperty);
            break;

          case XmlTagSmsKB:
            macroContents.Add(Common.CmdPrefixSmsKB);
            break;

          case XmlTagSound:
            macroContents.Add(Common.CmdPrefixSound + commandProperty);
            break;

          case XmlTagTranslator:
            macroContents.Add(Common.CmdPrefixTranslator);
            break;

          case XmlTagVirtualKB:
            macroContents.Add(Common.CmdPrefixVirtualKB);
            break;

          default:
            throw new Exception("Unrecognised Macro TAG: " + item.Attributes["command"].Value);
        }
      }

      return macroContents.ToArray();
    }

    void SaveMacro(string fileName, string[] contents)
    {
      using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
      {
        writer.Formatting = Formatting.Indented;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("macro");

        foreach (string item in contents)
        {
          writer.WriteStartElement("item");
          writer.WriteAttributeString("command", item);
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
      }
    }

  }

}
