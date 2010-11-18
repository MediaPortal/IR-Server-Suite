#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace IRServer.Plugin
{

  #region Structures

  /// <summary>
  /// Girder Command structure.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  internal struct GirCommand
  {
    #region Variables

    /// <summary>
    /// Mutex to control access to this command.
    /// </summary>
    public Mutex critical_section;

    /// <summary>
    /// Command name.
    /// </summary>
    public string name;

    /// <summary>
    /// Command action type.
    /// </summary>
    public int actiontype;

    /// <summary>
    /// Command action sub-type.
    /// </summary>
    public int actionsubtype;

    /// <summary>
    /// String Value 1.
    /// </summary>
    public string svalue1;

    /// <summary>
    /// String Value 2.
    /// </summary>
    public string svalue2;

    /// <summary>
    /// String Value 3.
    /// </summary>
    public string svalue3;

    /// <summary>
    /// Bool Value 1.
    /// </summary>
    public bool bvalue1;

    /// <summary>
    /// Bool Value 2.
    /// </summary>
    public bool bvalue2;

    /// <summary>
    /// Bool Value 3.
    /// </summary>
    public bool bvalue3;

    /// <summary>
    /// Int Value 1.
    /// </summary>
    public int ivalue1;

    /// <summary>
    /// Int Value 2.
    /// </summary>
    public int ivalue2;

    /// <summary>
    /// Int Value 3.
    /// </summary>
    public int ivalue3;

    /// <summary>
    /// Long Value 1.
    /// </summary>
    public int lvalue1;

    /// <summary>
    /// Long Value 2.
    /// </summary>
    public int lvalue2;

    /// <summary>
    /// Long Value 3.
    /// </summary>
    public int lvalue3;

    /// <summary>
    /// Binary data.
    /// </summary>
    public IntPtr binary;

    /// <summary>
    /// Size of the binary allocated area.
    /// </summary>
    public int size;

    #endregion Variables

    /// <summary>
    /// Create a new GirCommand from the supplied XML data.
    /// </summary>
    /// <param name="xmlData">The xml data.</param>
    /// <returns>A new GirCommand object.</returns>
    public static GirCommand FromXml(string xmlData)
    {
      GirCommand newCommand = new GirCommand();
      newCommand.critical_section = new Mutex();

      XmlDocument doc = new XmlDocument();
      using (StringReader stringReader = new StringReader(xmlData))
      {
        doc.Load(stringReader);

        try
        {
          newCommand.name = doc.DocumentElement["Command"].Attributes["Name"].Value;
        }
        catch
        {
        }
        try
        {
          newCommand.actiontype = int.Parse(doc.DocumentElement["Command"].Attributes["ActionType"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.actionsubtype = int.Parse(doc.DocumentElement["Command"].Attributes["ActionSubType"].Value);
        }
        catch
        {
        }

        try
        {
          newCommand.svalue1 = doc.DocumentElement["Command"].Attributes["sValue1"].Value;
        }
        catch
        {
        }
        try
        {
          newCommand.svalue2 = doc.DocumentElement["Command"].Attributes["sValue2"].Value;
        }
        catch
        {
        }
        try
        {
          newCommand.svalue3 = doc.DocumentElement["Command"].Attributes["sValue3"].Value;
        }
        catch
        {
        }

        try
        {
          newCommand.bvalue1 = bool.Parse(doc.DocumentElement["Command"].Attributes["bValue1"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.bvalue2 = bool.Parse(doc.DocumentElement["Command"].Attributes["bValue2"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.bvalue3 = bool.Parse(doc.DocumentElement["Command"].Attributes["bValue3"].Value);
        }
        catch
        {
        }

        try
        {
          newCommand.ivalue1 = int.Parse(doc.DocumentElement["Command"].Attributes["iValue1"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.ivalue2 = int.Parse(doc.DocumentElement["Command"].Attributes["iValue2"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.ivalue3 = int.Parse(doc.DocumentElement["Command"].Attributes["iValue3"].Value);
        }
        catch
        {
        }

        try
        {
          newCommand.lvalue1 = int.Parse(doc.DocumentElement["Command"].Attributes["lValue1"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.lvalue2 = int.Parse(doc.DocumentElement["Command"].Attributes["lValue2"].Value);
        }
        catch
        {
        }
        try
        {
          newCommand.lvalue3 = int.Parse(doc.DocumentElement["Command"].Attributes["lValue3"].Value);
        }
        catch
        {
        }
      }

      return newCommand;
    }

    /// <summary>
    /// Convert this GirCommand the an XML string.
    /// </summary>
    /// <returns>An XML string representing this GirCommand.</returns>
    public string ToXml()
    {
      StringBuilder xmlString = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(xmlString))
      {
        using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
        {
          xmlWriter.Formatting = Formatting.Indented;
          xmlWriter.Indentation = 1;
          xmlWriter.IndentChar = (char) 9;
          xmlWriter.WriteStartDocument(true);
          xmlWriter.WriteStartElement("Command"); // <Command>

          xmlWriter.WriteAttributeString("Name", name);
          xmlWriter.WriteAttributeString("Identifier", "3");
          xmlWriter.WriteAttributeString("Enabled", "TRUE");

          xmlWriter.WriteStartElement("Comments");
          xmlWriter.WriteString("IR Server Suite - Girder plugin command");
          xmlWriter.WriteEndElement();

          xmlWriter.WriteStartElement("ActionType");
          xmlWriter.WriteString(actiontype.ToString());
          xmlWriter.WriteEndElement();

          xmlWriter.WriteStartElement("ActionSubType");
          xmlWriter.WriteString(actionsubtype.ToString());
          xmlWriter.WriteEndElement();


          xmlWriter.WriteEndElement(); // </Command>
          xmlWriter.WriteEndDocument();
        }
      }

      return xmlString.ToString();
    }
  }

  #endregion Structures

  #region Delegates

  /// <summary>
  /// Callback for when the plugin sends an event.
  /// </summary>
  /// <param name="eventstring">Plugin event string.</param>
  /// <param name="payload">Additional data.</param>
  /// <param name="len">Length of additional data.</param>
  /// <param name="device">Device ID, to uniquely identify this plugin.</param>
  internal delegate void PluginEventCallback(string eventstring, IntPtr payload, int len, int device);

  /// <summary>
  /// Callback for when the plugin sets the command it is editing.
  /// </summary>
  /// <param name="command">Command structure.</param>
  internal delegate void PluginCommandSetCallback(GirCommand command);

  #endregion Delegates

  /// <summary>
  /// Wrapper class to work with Girder 3.x plugins.
  /// </summary>
  internal class GirderPluginWrapper : IDisposable
  {
    #region Constants

    /// <summary>
    /// Maximum Girder API version this wrapper can handle.
    /// </summary>
    public const int GirMaxApi = 2;

    /// <summary>
    /// Simulated Girder application version (Major).
    /// </summary>
    public const int GirVerMajor = 3;

    /// <summary>
    /// Simulated Girder application version (Micro).
    /// </summary>
    public const int GirVerMicro = 0;

    /// <summary>
    /// Simulated Girder application version (Minor).
    /// </summary>
    public const int GirVerMinor = 3;

    private const int MaxStringLength = 256;

    #endregion Constants

    #region Enumerations

    private enum OsdSettings
    {
      FGColor = 1,
      FGDColor = 2,
      BGColor = 3,
      Width = 4,
      Height = 5,
      Caption = 6,
      Border = 7,
      Transparent = 8,
      Fontsize = 9,
      FontWeight = 10,
      FontItalic = 11,
      FontUnderline = 12,
      FontStrikeout = 13,
      Left = 14,
      Top = 15,
      Center = 16,
      Monitor = 17,
    }

    #endregion Enumerations

    #region Interop

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetDllDirectory(
      string lpPathName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr LoadLibrary(
      string dllFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
    private static extern IntPtr GetProcAddress(
      IntPtr module,
      string functionName);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FreeLibrary(
      IntPtr handle);

    #endregion Interop

    #region Girder Funtions

    #region Nested type: t_delete_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_delete_var(string name);

    #endregion

    #region Nested type: t_event_cb

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_event_cb(string event_string, int device, IntPtr payload, int len);

    #endregion

    #region Nested type: t_get_double_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate double t_get_double_var(string name);

    #endregion

    #region Nested type: t_get_int_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate int t_get_int_var(string name);

    #endregion

    #region Nested type: t_get_link_name

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_get_link_name(int lvalue, IntPtr szstore, int size);

    #endregion

    #region Nested type: t_get_osd_fontname

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_get_osd_fontname(IntPtr szstore, int size);

    #endregion

    #region Nested type: t_get_osd_settings

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate int t_get_osd_settings(int setting);

    #endregion

    #region Nested type: t_get_string_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_get_string_var(string name, IntPtr szstore, int size);

    #endregion

    #region Nested type: t_gir_free

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_gir_free(IntPtr data);

    #endregion

    #region Nested type: t_gir_malloc

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate IntPtr t_gir_malloc(int size);

    #endregion

    #region Nested type: t_hide_osd

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_hide_osd();

    #endregion

    #region Nested type: t_i18n_translate

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_i18n_translate(string orig, IntPtr szstore, int size);

    #endregion

    #region Nested type: t_parse_girder_reg

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_parse_girder_reg(string orig, IntPtr szstore, int size);

    #endregion

    // GirCommand Ptr?

    #region Nested type: t_realloc_pchar

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_realloc_pchar(IntPtr old, byte newchar);

    #endregion

    #region Nested type: t_register_cb

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_register_cb(int actionplugin, t_event_cb callback, string prefix, int device);

    #endregion

    #region Nested type: t_run_parser

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_run_parser(string str, IntPtr error_value);

    #endregion

    #region Nested type: t_send_event

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_send_event(string eventstring, IntPtr payload, int len, int device);

    #endregion

    #region Nested type: t_set_command

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_set_command(GirCommand command);

    #endregion

    #region Nested type: t_set_double_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_set_double_var(string name, double value);

    #endregion

    #region Nested type: t_set_int_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_set_int_var(string name, int value);

    #endregion

    #region Nested type: t_set_string_var

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_set_string_var(string name, string value);

    #endregion

    #region Nested type: t_show_osd

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_show_osd(int timer);

    #endregion

    #region Nested type: t_start_osd_draw

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_start_osd_draw(IntPtr hw, IntPtr h, int user);

    #endregion

    #region Nested type: t_stop_osd_draw

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_stop_osd_draw(IntPtr h);

    #endregion

    #region Nested type: t_target_callback

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_target_callback(IntPtr hw, GirCommand command);

    #endregion

    #region Nested type: t_target_enum

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void t_target_enum(int id, t_target_callback callback);

    #endregion

    #region Nested type: t_treepicker_show

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_treepicker_show(IntPtr window, int id);

    #endregion

    #region Nested type: t_trigger_command

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool t_trigger_command(int command_id);

    #endregion

    #endregion Girder Funtions

    #region Nested type: GirApiFunctions

    [StructLayout(LayoutKind.Sequential)]
    private struct GirApiFunctions
    {
      public int size;
      public t_parse_girder_reg parse_reg_string;
      public t_get_link_name get_link_name;
      public t_set_command set_command;
      public t_target_enum target_enum;
      public t_realloc_pchar realloc_pchar;
      public t_show_osd show_osd;
      public t_hide_osd hide_osd;
      public t_start_osd_draw start_osd_draw;
      public t_stop_osd_draw stop_osd_draw;
      public t_treepicker_show treepicker_show;
      public t_register_cb register_cb;
      public t_i18n_translate i18n_translate;
      public t_get_osd_settings get_osd_settings;
      public t_get_osd_fontname get_osd_font_name;
      public t_gir_malloc gir_malloc;
      public t_gir_free gir_free;
      public t_get_int_var get_int_var;
      public t_get_double_var get_double_var;
      public t_get_string_var get_string_var;
      public t_set_int_var set_int_var;
      public t_set_double_var set_double_var;
      public t_set_string_var set_string_var;
      public t_delete_var delete_var;
      public t_run_parser run_parser;
      public t_send_event send_event;
      public t_trigger_command trigger_command;
      public IntPtr parent_hwnd;
    }

    #endregion

    #region Plugin functions

    #region Nested type: gir_close

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool gir_close();

    #endregion

    #region Nested type: gir_command_gui

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void gir_command_gui();

    #endregion

    #region Nested type: gir_description

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void gir_description([In, Out] StringBuilder desc, int size);

    #endregion

    #region Nested type: gir_devicenum

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate int gir_devicenum();

    #endregion

    #region Nested type: gir_event

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool gir_event(
      GirCommand command, string eventstring, IntPtr payload, int len, string status, int statuslen);

    #endregion

    #region Nested type: gir_info

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool gir_info(int message, int wParam, int lParam);

    #endregion

    #region Nested type: gir_name

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void gir_name([In, Out] StringBuilder name, int size);

    #endregion

    #region Nested type: gir_open

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool gir_open(int gir_major_ver, int gir_minor_ver, int gir_micro_ver, IntPtr api_functions);

    #endregion

    #region Nested type: gir_requested_api

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate int gir_requested_api(int max_api);

    #endregion

    #region Nested type: gir_start

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool gir_start();

    #endregion

    #region Nested type: gir_stop

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private delegate bool gir_stop();

    #endregion

    #region Nested type: gir_version

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate void gir_version([In, Out] StringBuilder version, int size);

    #endregion

    #endregion Plugin functions

    #region Variables

    private readonly IntPtr _apiFunctionsPtr;
    private GirApiFunctions _apiFunctions;
    private GCHandle _apiFunctionsHandle;
    private PluginCommandSetCallback _commandSetCallback;
    private PluginEventCallback _eventCallback;
    private gir_close _girClose;
    private gir_command_gui _girCommandGui;
    private gir_description _girDescription;
    private gir_devicenum _girDevicenum;
    private gir_event _girEvent;
    private gir_info _girInfo;
    private gir_name _girName;
    private gir_open _girOpen;
    private gir_requested_api _girRequestedApi;
    private gir_start _girStart;
    private gir_stop _girStop;
    private gir_version _girVersion;

    private IntPtr _pluginDll;

    #endregion Variables

    #region Girder Plugin Properties

    /// <summary>
    /// Gets the gir version.
    /// </summary>
    /// <value>The gir version.</value>
    public string GirVersion
    {
      get
      {
        StringBuilder version = new StringBuilder(255);
        _girVersion(version, version.Capacity);
        return version.ToString();
      }
    }

    /// <summary>
    /// Gets the gir name.
    /// </summary>
    /// <value>The gir name.</value>
    public string GirName
    {
      get
      {
        StringBuilder name = new StringBuilder(MaxStringLength);
        _girName(name, name.Capacity);
        return name.ToString();
      }
    }

    /// <summary>
    /// Gets the gir description.
    /// </summary>
    /// <value>The gir description.</value>
    public string GirDescription
    {
      get
      {
        StringBuilder description = new StringBuilder(MaxStringLength);
        _girDescription(description, description.Capacity);
        return description.ToString();
      }
    }

    /// <summary>
    /// Gets the gir devicenum.
    /// </summary>
    /// <value>The gir devicenum.</value>
    public int GirDevicenum
    {
      get { return _girDevicenum(); }
    }

    /// <summary>
    /// Gets the gir requested API.
    /// </summary>
    /// <value>The gir requested API.</value>
    public int GirRequestedApi
    {
      get { return _girRequestedApi(GirMaxApi); }
    }

    #endregion Girder Plugin Properties

    #region Properties

    /// <summary>
    /// Gets or sets the plugin send event callback.
    /// </summary>
    /// <value>The plugin send event callback.</value>
    public PluginEventCallback EventCallback
    {
      get { return _eventCallback; }
      set { _eventCallback = value; }
    }

    /// <summary>
    /// Gets or sets the command set callback.
    /// </summary>
    /// <value>The command set callback.</value>
    public PluginCommandSetCallback CommandSetCallback
    {
      get { return _commandSetCallback; }
      set { _commandSetCallback = value; }
    }

    #endregion Properties

    #region Construction / Destruction

    /// <summary>
    /// Initializes a new instance of the <see cref="GirderPluginWrapper"/> class.
    /// </summary>
    /// <param name="fileName">Name of the plugin dll file.</param>
    public GirderPluginWrapper(string fileName)
    {
      if (String.IsNullOrEmpty(fileName))
        throw new ArgumentException("Empty or null file name", "fileName");

      if (!LoadGirderPlugin(fileName))
        throw new InvalidOperationException(String.Format("Failed to load girder plugin ({0})", fileName));

      int requiredApi = GirRequestedApi;

      if (requiredApi > GirMaxApi)
        throw new InvalidOperationException(
          String.Format("Failed to load girder plugin ({0}), plugin requires newer API version ({1})", fileName,
                        requiredApi));

      _apiFunctions = new GirApiFunctions();
      _apiFunctions.delete_var = delete_var;
      _apiFunctions.get_double_var = get_double_var;
      _apiFunctions.get_int_var = get_int_var;
      _apiFunctions.get_link_name = get_link_name;
      _apiFunctions.get_osd_font_name = get_osd_fontname;
      _apiFunctions.get_osd_settings = get_osd_settings;
      _apiFunctions.get_string_var = get_string_var;
      _apiFunctions.gir_free = gir_free;
      _apiFunctions.gir_malloc = gir_malloc;
      _apiFunctions.hide_osd = hide_osd;
      _apiFunctions.i18n_translate = i18n_translate;
      _apiFunctions.parse_reg_string = parse_girder_reg;
      _apiFunctions.realloc_pchar = realloc_pchar;
      _apiFunctions.register_cb = register_cb;
      _apiFunctions.run_parser = run_parser;
      _apiFunctions.send_event = send_event;
      _apiFunctions.set_command = set_command;
      _apiFunctions.set_double_var = set_double_var;
      _apiFunctions.set_int_var = set_int_var;
      _apiFunctions.set_string_var = set_string_var;
      _apiFunctions.show_osd = show_osd;
      _apiFunctions.start_osd_draw = start_osd_draw;
      _apiFunctions.stop_osd_draw = stop_osd_draw;
      _apiFunctions.target_enum = target_enum;
      _apiFunctions.treepicker_show = treepicker_show;
      _apiFunctions.trigger_command = trigger_command;

      _apiFunctions.parent_hwnd = IntPtr.Zero;
      _apiFunctions.size = Marshal.SizeOf(_apiFunctions);

      byte[] rawData = new byte[_apiFunctions.size];
      _apiFunctionsHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
      _apiFunctionsPtr = _apiFunctionsHandle.AddrOfPinnedObject();
      Marshal.StructureToPtr(_apiFunctions, _apiFunctionsPtr, false);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="GirderPluginWrapper"/> is reclaimed by garbage collection.
    /// </summary>
    ~GirderPluginWrapper()
    {
      Dispose(false);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources ...
      }

      // Free native resources ...
      if (_pluginDll != IntPtr.Zero)
        FreeLibrary(_pluginDll);

      if (_apiFunctionsHandle.IsAllocated)
        _apiFunctionsHandle.Free();

      _apiFunctions = new GirApiFunctions();
    }

    #endregion Construction / Destruction

    #region Plugin Methods

    /// <summary>
    /// Gets a value indicating whether this instance can be configured.
    /// </summary>
    /// <value><c>true</c> if this instance can configure; otherwise, <c>false</c>.</value>
    public bool CanConfigure
    {
      get { return _girCommandGui != null; }
    }

    /// <summary>
    /// GirOpen function.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool GirOpen()
    {
      return _girOpen(GirVerMajor, GirVerMinor, GirVerMicro, _apiFunctionsPtr);
    }

    /// <summary>
    /// GirClose function.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool GirClose()
    {
      return _girClose();
    }

    /// <summary>
    /// GirStart function.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool GirStart()
    {
      if (_girStart != null)
        return _girStart();

      return false;
    }

    /// <summary>
    /// GirStop function.
    /// </summary>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool GirStop()
    {
      if (_girStop != null)
        return _girStop();

      return false;
    }

    /// <summary>
    /// GirCommandGui function.
    /// </summary>
    public void GirCommandGui()
    {
      if (_girCommandGui != null)
        _girCommandGui();
    }

    /// <summary>
    /// GirInfo function.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="wParam">The w param.</param>
    /// <param name="lParam">The l param.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool GirInfo(int message, int wParam, int lParam)
    {
      if (_girInfo != null)
        return _girInfo(message, wParam, lParam);

      return false;
    }

    /// <summary>
    /// GirEvent function.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="eventstring">The event string.</param>
    /// <param name="payload">The event payload.</param>
    /// <param name="len">The event payload length.</param>
    /// <param name="status">The event status.</param>
    /// <param name="statuslen">The event status length.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool GirEvent(GirCommand command, string eventstring, IntPtr payload, int len, string status, int statuslen)
    {
      if (_girEvent != null)
        return _girEvent(command, eventstring, payload, len, status, statuslen);

      return false;
    }

    #endregion Plugin Methods

    #region Plugin Callbacks

    private bool event_cb(string event_string, int device, IntPtr payload, int len)
    {
#if TRACE
      Trace.WriteLine(String.Format("event_cb({0}, {1})", event_string, device));
#endif

      return true;
    }

    private bool parse_girder_reg(string orig, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("parse_girder_reg({0}, {1})", orig, size));
#endif

      return WriteString(szstore, size, "Error");
    }

    private bool get_link_name(int lvalue, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_link_name({0})", lvalue));
#endif

      return WriteString(szstore, size, "Error");
    }

    private void set_command(GirCommand command) // GirCommand Ptr?
    {
#if TRACE
      Trace.WriteLine(String.Format("set_command()"));
#endif

      if (_commandSetCallback != null)
        _commandSetCallback(command);
    }

    private void target_enum(int id, t_target_callback callback)
    {
#if TRACE
      Trace.WriteLine(String.Format("target_enum({0})", id));
#endif
    }

    private void target_callback(IntPtr hw, GirCommand command) // GirCommand Ptr?
    {
#if TRACE
      Trace.WriteLine(String.Format("target_callback()"));
#endif
    }

    private void realloc_pchar(IntPtr old, byte newchar)
    {
#if TRACE
      Trace.WriteLine(String.Format("realloc_pchar({0})", newchar));
#endif

      Marshal.WriteByte(old, newchar);
    }

    private void show_osd(int timer)
    {
#if TRACE
      Trace.WriteLine(String.Format("show_osd({0})", timer));
#endif
    }

    private void hide_osd()
    {
#if TRACE
      Trace.WriteLine("hide_osd()");
#endif
    }

    private bool start_osd_draw(IntPtr hw, IntPtr h, int user)
    {
#if TRACE
      Trace.WriteLine(String.Format("start_osd_draw({0})", user));
#endif
      return true;
    }

    private void stop_osd_draw(IntPtr h)
    {
#if TRACE
      Trace.WriteLine("stop_osd_draw");
#endif
    }

    private bool treepicker_show(IntPtr window, int id)
    {
#if TRACE
      Trace.WriteLine(String.Format("treepicker_show({0})", id));
#endif
      return true;
    }

    private bool register_cb(int actionplugin, t_event_cb callback, string prefix, int device)
    {
#if TRACE
      Trace.WriteLine(String.Format("register_cb({0})", actionplugin));
#endif
      return true;
    }

    private bool i18n_translate(string orig, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("i18n_translate({0})", orig));
#endif

      return WriteString(szstore, size, orig);
    }

    private int get_osd_settings(int setting) // (return int == dword)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_osd_settings({0})", setting));
#endif

      switch ((OsdSettings) setting)
      {
        case OsdSettings.FGColor:
          return Color.Lime.ToArgb();
        case OsdSettings.FGDColor:
          return Color.Green.ToArgb();
        case OsdSettings.BGColor:
          return Color.Black.ToArgb();
        case OsdSettings.Width:
          return 380;
        case OsdSettings.Height:
          return 255;
        case OsdSettings.Caption:
          return 1;
        case OsdSettings.Border:
          return 1;
        case OsdSettings.Transparent:
          return 0;
        case OsdSettings.Fontsize:
          return 15;
        case OsdSettings.FontWeight:
          return 0;
        case OsdSettings.FontItalic:
          return 0;
        case OsdSettings.FontUnderline:
          return 0;
        case OsdSettings.FontStrikeout:
          return 0;
        case OsdSettings.Left:
          return 0;
        case OsdSettings.Top:
          return 0;
        case OsdSettings.Center:
          return 1;
        case OsdSettings.Monitor:
          return 0;
        default:
          return 0;
      }
    }

    private bool get_osd_fontname(IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine("get_osd_fontname");
#endif

      return WriteString(szstore, size, "Times New Roman");
    }

    private IntPtr gir_malloc(int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("gir_malloc({0})", size));
#endif
      return Marshal.AllocHGlobal(size);
    }

    private void gir_free(IntPtr data)
    {
#if TRACE
      Trace.WriteLine(String.Format("gir_free({0})", data.ToString()));
#endif
      Marshal.FreeHGlobal(data);
    }

    private int get_int_var(string name)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_int_var({0})", name));
#endif

      return 0;
    }

    private double get_double_var(string name)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_double_var({0})", name));
#endif

      return 0.0;
    }

    private bool get_string_var(string name, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_string_var({0})", name));
#endif

      return WriteString(szstore, size, "Error");
    }

    private bool set_int_var(string name, int value)
    {
#if TRACE
      Trace.WriteLine(String.Format("set_int_var({0}, {1})", name, value));
#endif
      return true;
    }

    private bool set_double_var(string name, double value)
    {
#if TRACE
      Trace.WriteLine(String.Format("set_double_var({0}, {1})", name, value));
#endif
      return true;
    }

    private bool set_string_var(string name, string value)
    {
#if TRACE
      Trace.WriteLine(String.Format("set_string_var({0}, {1})", name, value));
#endif
      return true;
    }

    private bool delete_var(string name)
    {
#if TRACE
      Trace.WriteLine(String.Format("delete_var({0})", name));
#endif

      return true;
    }

    private bool run_parser(string str, IntPtr error_value)
    {
#if TRACE
      Trace.WriteLine(String.Format("run_parser({0}, {1})", str, error_value.ToString()));
#endif
      return true;
    }

    private bool send_event(string eventstring, IntPtr payload, int len, int device)
    {
#if TRACE
      Trace.WriteLine(String.Format("Girder Plugin event: {0}, {1}, {2}, {3}", eventstring, payload.ToString(), len, device));
#endif

      if (_eventCallback != null)
        _eventCallback(eventstring, payload, len, device);

      return true;
    }

    private bool trigger_command(int command_id)
    {
#if TRACE
      Trace.WriteLine(String.Format("trigger_command({0})", command_id));
#endif
      return true;
    }

    #endregion Plugin Callbacks

    #region Implementation

    private static bool WriteString(IntPtr stringPtr, int maxSize, string value)
    {
      try
      {
        byte[] bytes = Encoding.ASCII.GetBytes(value);

        if (bytes.Length >= maxSize)
          return false;

        // Write original string bytes into output string memory.
        Marshal.Copy(bytes, 0, stringPtr, bytes.Length);

        // Write null terminator.
        Marshal.WriteByte(stringPtr, bytes.Length, 0);
      }
      catch
      {
        return false;
      }

      return true;
    }

    private bool LoadGirderPlugin(string girderPluginFile)
    {
      string folder = Path.GetDirectoryName(girderPluginFile);
      string parent = Directory.GetParent(folder).FullName;
      SetDllDirectory(parent);

      _pluginDll = LoadLibrary(girderPluginFile);
      if (_pluginDll == IntPtr.Zero)
        throw new InvalidOperationException(String.Format("Failed to call LoadLibrary on girder plugin dll ({0})",
                                                          girderPluginFile));

      try
      {
        IntPtr function;

        function = GetProcAddress(_pluginDll, "gir_version");
        _girVersion = (gir_version) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_version));

        function = GetProcAddress(_pluginDll, "gir_name");
        _girName = (gir_name) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_name));

        function = GetProcAddress(_pluginDll, "gir_description");
        _girDescription = (gir_description) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_description));

        function = GetProcAddress(_pluginDll, "gir_devicenum");
        _girDevicenum = (gir_devicenum) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_devicenum));

        function = GetProcAddress(_pluginDll, "gir_requested_api");
        _girRequestedApi =
          (gir_requested_api) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_requested_api));


        function = GetProcAddress(_pluginDll, "gir_open");
        _girOpen = (gir_open) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_open));

        function = GetProcAddress(_pluginDll, "gir_close");
        _girClose = (gir_close) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_close));


        function = GetProcAddress(_pluginDll, "gir_command_gui");
        if (function != IntPtr.Zero) // Optional.
          _girCommandGui = (gir_command_gui) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_command_gui));

        function = GetProcAddress(_pluginDll, "gir_info");
        if (function != IntPtr.Zero) // Optional.
          _girInfo = (gir_info) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_info));

        function = GetProcAddress(_pluginDll, "gir_event");
        if (function != IntPtr.Zero) // Optional.
          _girEvent = (gir_event) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_event));

        function = GetProcAddress(_pluginDll, "gir_start");
        if (function != IntPtr.Zero) // Optional.
          _girStart = (gir_start) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_start));

        function = GetProcAddress(_pluginDll, "gir_stop");
        if (function != IntPtr.Zero) // Optional.
          _girStop = (gir_stop) Marshal.GetDelegateForFunctionPointer(function, typeof (gir_stop));

        return true;
      }
      catch
      {
        FreeLibrary(_pluginDll);
      }

      return false;
    }

    #endregion Implementation

/*
typedef void   (WINAPI *t_target_callback)   (HWND hw, p_command command);
typedef void   (WINAPI *t_set_command)       (p_command command); 
typedef void   (WINAPI *t_target_enum)       (int id, t_target_callback callback); 
typedef void   (WINAPI *t_realloc_pchar)     (PCHAR * old, PCHAR newchar);
typedef void   (WINAPI *t_show_osd)          (int timer);
typedef void   (WINAPI *t_hide_osd)          ();
typedef int    (WINAPI *t_start_osd_draw)    (HWND *hw, HDC *h, int user);
typedef void   (WINAPI *t_stop_osd_draw)     (HDC h);
typedef int    (WINAPI *t_treepicker_show)   (HWND window, int id);
typedef int    (WINAPI *t_event_cb)          (PCHAR event_string, int device, void *payload, int len);
typedef int    (WINAPI *t_register_cb)       (int actionplugin, t_event_cb callback, PCHAR prefix, int device);
typedef int    (WINAPI *t_get_link_name)     (int lvalue, PCHAR szstore, int size);
typedef int    (WINAPI *t_parse_girder_reg)  (PCHAR orig, PCHAR szstore, int size);
typedef int    (WINAPI *t_i18n_translate)    (PCHAR orig, PCHAR szstore, int size);
typedef DWORD  (WINAPI *t_get_osd_settings)  (int setting);
typedef int    (WINAPI *t_get_osd_fontname)  (PCHAR szstore, int size);
typedef int    (WINAPI *t_run_parser)        (PCHAR str, int *error_value);
typedef int    (WINAPI *t_get_int_var)       (PCHAR name);
typedef double (WINAPI *t_get_double_var)    (PCHAR name);
typedef int    (WINAPI *t_get_string_var)    (PCHAR name, PCHAR szstore, int size);
typedef int    (WINAPI *t_set_int_var)       (PCHAR name, int value);
typedef int    (WINAPI *t_set_double_var)    (PCHAR name, double value);
typedef int    (WINAPI *t_set_string_var)    (PCHAR name, PCHAR value);
typedef int    (WINAPI *t_delete_var)        (PCHAR name);
typedef void * (WINAPI *t_gir_malloc)        (int size);
typedef void   (WINAPI *t_gir_free)          (void *data);
typedef int    (WINAPI *t_send_event)        (const char * eventstring, void *payload, int len, int device);
typedef int    (WINAPI *t_trigger_command)   (int command_id);


typedef void   (WINAPI *t_target_callback_ex)       (HWND hw, p_command command, void * data);
typedef void   (WINAPI *t_target_enum_ex)           (int id, t_target_callback_ex callback, void *data);  // added user data
typedef PCHAR  (WINAPI *t_open_script_editor)       (HWND window, PCHAR script); // call from its own thread. free return pchar with gir_free.
typedef int    (WINAPI *t_get_variable_type)        (PCHAR name); // 1 = int, 2 = double, 3= string, -1 = does not exists.
typedef int    (WINAPI *t_open_list_variables)      ( ); // call to list all vars.
typedef int    (WINAPI *t_get_first_variable)       ( ); // move to the beginning of the list
typedef int    (WINAPI *t_close_list_variables)     ( ); // close the list var MUST DO!
typedef int    (WINAPI *t_get_next_int_variable)    (PCHAR name, int len, int value ); 
typedef int    (WINAPI *t_get_next_double_variable) (PCHAR name, int len, double value );
typedef int    (WINAPI *t_get_next_string_variable) (PCHAR name, int len, PCHAR value, int size );
typedef int    (WINAPI *t_set_variable_window)      (HWND window, UINT msg, int add); // call this to be notified of variable changes.
typedef void * (WINAPI *t_get_script_state)         (); // call this to get the lua/script state, CHECK IF RETURN VALUE IS NOT NULL!!
*/
  }
}