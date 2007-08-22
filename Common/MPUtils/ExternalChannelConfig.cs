using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MPUtils
{

  /// <summary>
  /// External Channel Changing configuration file for tuning Set Top Boxes in MediaPortal.
  /// </summary>
  public class ExternalChannelConfig
  {

    #region Constants

    const int DefaultCardID                 = 0;

    const int DefaultPauseTime              = 250;
    const bool DefaultSendSelect            = false;
    const bool DefaultDoubleChannelSelect   = false;
    const int DefaultRepeatChannelCommands  = 0;
    const int DefaultChannelDigits          = 0;
    const int DefaultRepeatPauseTime        = 1000;
    const bool DefaultUsePreChangeCommand   = false;

    #endregion Constants

    #region Variables

    string _fileName;

    int _cardID;

    int _pauseTime;
    bool _sendSelect;
    bool _doubleChannelSelect;
    int _repeatChannelCommands;
    int _channelDigits;
    int _repeatPauseTime;
    bool _usePreChangeCommand;

    string _selectCommand;
    string _preChangeCommand;
    string[] _digits;

    #endregion Variables

    #region Properties

    public string FileName
    {
      get { return _fileName; }
    }

    public int CardId
    {
      get { return _cardID; }
      set { _cardID = value; }
    }

    public int PauseTime
    {
      get { return _pauseTime; }
      set { _pauseTime = value; }
    }
    public bool SendSelect
    {
      get { return _sendSelect; }
      set { _sendSelect = value; }
    }
    public bool DoubleChannelSelect
    {
      get { return _doubleChannelSelect; }
      set { _doubleChannelSelect = value; }
    }
    public int RepeatChannelCommands
    {
      get { return _repeatChannelCommands; }
      set { _repeatChannelCommands = value; }
    }
    public int ChannelDigits
    {
      get { return _channelDigits; }
      set { _channelDigits = value; }
    }
    public int RepeatPauseTime
    {
      get { return _repeatPauseTime; }
      set { _repeatPauseTime = value; }
    }
    public bool UsePreChangeCommand
    {
      get { return _usePreChangeCommand; }
      set { _usePreChangeCommand = value; }
    }

    public string[] Digits
    {
      get { return _digits; }
      set { _digits = value; }
    }
    public string SelectCommand
    {
      get { return _selectCommand; }
      set { _selectCommand = value; }
    }
    public string PreChangeCommand
    {
      get { return _preChangeCommand; }
      set { _preChangeCommand = value; }
    }
    
    #endregion Properties

    #region Constructor

    public ExternalChannelConfig(string fileName)
    {
      _fileName               = fileName;

      _cardID                 = DefaultCardID;

      _pauseTime              = DefaultPauseTime;
      _sendSelect             = DefaultSendSelect;
      _doubleChannelSelect    = DefaultDoubleChannelSelect;
      _repeatChannelCommands  = DefaultRepeatChannelCommands;
      _channelDigits          = DefaultChannelDigits;
      _repeatPauseTime        = DefaultRepeatPauseTime;
      _usePreChangeCommand    = DefaultUsePreChangeCommand;

      _selectCommand          = String.Empty;
      _preChangeCommand       = String.Empty;
      _digits                 = new string[10];

      for (int i = 0; i < 10; i++)
        _digits[i] = String.Empty;
    }

    #endregion Constructor

    public void Save()
    {
      XmlTextWriter writer = new XmlTextWriter(_fileName, System.Text.Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.Indentation = 1;
      writer.IndentChar = (char)9;
      writer.WriteStartDocument(true);
      writer.WriteStartElement("config"); // <config>

      writer.WriteElementString("PauseTime",              PauseTime.ToString());
      writer.WriteElementString("UsePreChangeCommand",    UsePreChangeCommand.ToString());
      writer.WriteElementString("SendSelect",             SendSelect.ToString());
      writer.WriteElementString("DoubleChannelSelect",    DoubleChannelSelect.ToString());
      writer.WriteElementString("ChannelDigits",          ChannelDigits.ToString());
      writer.WriteElementString("RepeatChannelCommands",  RepeatChannelCommands.ToString());
      writer.WriteElementString("RepeatDelay",            RepeatPauseTime.ToString());

      writer.WriteElementString("SelectCommand",          SelectCommand);
      writer.WriteElementString("PreChangeCommand",       PreChangeCommand);

      for (int i = 0; i < 10; i++)
        writer.WriteElementString("Digit" + i.ToString(), Digits[i]);

      writer.WriteEndElement(); // </config>
      writer.WriteEndDocument();
      writer.Close();
    }

    static string GetString(XmlDocument doc, string element, string defaultValue)
    {
      if (String.IsNullOrEmpty(element))
        return defaultValue;
      
      XmlNode node = doc.DocumentElement.SelectSingleNode(element);
      if (node == null)
        return defaultValue;

      return node.InnerText;
    }
    static int GetInt(XmlDocument doc, string element, int defaultValue)
    {
      if (String.IsNullOrEmpty(element))
        return defaultValue;

      XmlNode node = doc.DocumentElement.SelectSingleNode(element);
      if (node == null)
        return defaultValue;

      int returnValue;
      if (int.TryParse(node.InnerText, out returnValue))
        return returnValue;

      return defaultValue;
    }
    static bool GetBool(XmlDocument doc, string element, bool defaultValue)
    {
      if (String.IsNullOrEmpty(element))
        return defaultValue;

      XmlNode node = doc.DocumentElement.SelectSingleNode(element);
      if (node == null)
        return defaultValue;

      bool returnValue;
      if (bool.TryParse(node.InnerText, out returnValue))
        return returnValue;

      return defaultValue;
    }

    public static ExternalChannelConfig Load(string fileName)
    {
      ExternalChannelConfig newECC = new ExternalChannelConfig(fileName);

      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      newECC.PauseTime              = GetInt(doc, "PauseTime", DefaultPauseTime);
      newECC.UsePreChangeCommand    = GetBool(doc, "UsePreChangeCommand", DefaultUsePreChangeCommand);
      newECC.SendSelect             = GetBool(doc, "SendSelect", DefaultSendSelect);
      newECC.DoubleChannelSelect    = GetBool(doc, "DoubleChannelSelect", DefaultDoubleChannelSelect);
      newECC.RepeatChannelCommands  = GetInt(doc, "RepeatChannelCommands", DefaultRepeatChannelCommands);
      newECC.ChannelDigits          = GetInt(doc, "ChannelDigits", DefaultChannelDigits);
      newECC.RepeatPauseTime        = GetInt(doc, "RepeatDelay", DefaultRepeatPauseTime);

      newECC.SelectCommand          = GetString(doc, "SelectCommand", String.Empty);
      newECC.PreChangeCommand       = GetString(doc, "PreChangeCommand", String.Empty);
      
      for (int index = 0; index < 10; index++)
        newECC.Digits[index]        = GetString(doc, "Digit" + index.ToString(), String.Empty);

      return newECC;
    }

  }

}
