using System;
#if TRACE
using System.Diagnostics;
#endif
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GirderPlugin
{

  /// <summary>
  /// Wrapper class to work with Girder 3.x plugins.
  /// </summary>
  public class GirderPluginWrapper : IDisposable
  {

    #region Constants

    /// <summary>
    /// Simulated Girder application version (Major).
    /// </summary>
    public const int GirVerMajor = 3;
    /// <summary>
    /// Simulated Girder application version (Minor).
    /// </summary>
    public const int GirVerMinor = 3;
    /// <summary>
    /// Simulated Girder application version (Micro).
    /// </summary>
    public const int GirVerMicro = 0;

    /// <summary>
    /// Maximum Girder API version this wrapper can handle.
    /// </summary>
    public const int GirMaxApi = 2;

    const int MaxStringLength = 256;

    #endregion Constants

    #region Interop

    [DllImport("kernel32")]
    extern static IntPtr LoadLibrary(
      string dllFileName);

    [DllImport("kernel32")]
    static extern IntPtr GetProcAddress(
      IntPtr module,
      string functionName);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    extern static bool FreeLibrary(
      IntPtr handle);

    #endregion Interop

    #region Girder Plugin Delegates

    #region Girder Funtions

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_event_cb(string event_string, int device, IntPtr payload, int len);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_parse_girder_reg(string orig, IntPtr szstore, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_get_link_name(int lvalue, IntPtr szstore, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_set_command(GirCommand command); // GirCommand Ptr?

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_target_enum(int id, t_target_callback callback);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_target_callback(IntPtr hw, GirCommand command); // GirCommand Ptr?

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_realloc_pchar(IntPtr old, byte newchar);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_show_osd(int timer);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_hide_osd();

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_start_osd_draw(IntPtr hw, IntPtr h, int user);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_stop_osd_draw(IntPtr h);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_treepicker_show(IntPtr window, int id);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_register_cb(int actionplugin, t_event_cb callback, string prefix, int device);
    
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_i18n_translate(string orig, IntPtr szstore, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate int t_get_osd_settings(int setting); // (return int == dword)

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_get_osd_fontname(IntPtr szstore, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate IntPtr t_gir_malloc(int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void t_gir_free(IntPtr data);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate int t_get_int_var(string name);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate double t_get_double_var(string name);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_get_string_var(string name, IntPtr szstore, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_set_int_var(string name, int value);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_set_double_var(string name, double value);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_set_string_var(string name, string value);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_delete_var(string name);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_run_parser(string str, IntPtr error_value);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_send_event(string eventstring, IntPtr payload, int len, int device);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool t_trigger_command(int command_id);

    #endregion Girder Funtions

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
    [StructLayout(LayoutKind.Sequential)]
    struct GirApiFunctions
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

    [StructLayout(LayoutKind.Sequential)]
    struct GirCommand
    {
      public Mutex critical_section;
      public string name;
      public int actiontype;
      public int actionsubtype;
      public string svalue1;
      public string svalue2;
      public string svalue3;
      public int bvalue1;
      public int bvalue2;
      public int bvalue3;
      public int ivalue1;
      public int ivalue2;
      public int ivalue3;
      public int lvalue1;
      public int lvalue2;
      public int lvalue3;
      public IntPtr binary;
      public int size;
    }

    #region Plugin functions

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void gir_version([In, Out] StringBuilder version, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void gir_name([In, Out] StringBuilder name, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void gir_description([In, Out] StringBuilder desc, int size);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate int gir_devicenum();

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate int gir_requested_api(int max_api);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool gir_open(int gir_major_ver, int gir_minor_ver, int gir_micro_ver, IntPtr api_functions);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool gir_close();

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool gir_start();

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool gir_stop();

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    delegate void gir_command_gui();

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool gir_info(int message, int wParam, int lParam);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool gir_event(GirCommand command, string eventstring, IntPtr payload, int len, string status, int statuslen);

    #endregion Plugin functions

    #endregion Girder Plugin Delegates

    #region Variables

    gir_version _girVersion;
    gir_name _girName;
    gir_description _girDescription;
    gir_devicenum _girDevicenum;
    gir_requested_api _girRequestedApi;
    gir_open _girOpen;
    gir_close _girClose;
    gir_start _girStart;
    gir_stop _girStop;
    gir_command_gui _girCommandGui;
    gir_info _girInfo;

    IntPtr _pluginDll;

    PluginEventCallback _eventCallback;

    GirApiFunctions _apiFunctions;
    GCHandle _apiFunctionsHandle;
    IntPtr _apiFunctionsPtr;

    #endregion Variables

    /// <summary>
    /// Callback for when the plugin sends an event.
    /// </summary>
    /// <param name="eventstring">Plugin event string.</param>
    /// <param name="payload">Additional data.</param>
    /// <param name="len">Length of additional data.</param>
    /// <param name="device">Device ID, to uniquely identify this plugin.</param>
    public delegate void PluginEventCallback(string eventstring, IntPtr payload, int len, int device);

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
      get
      {
        return _girDevicenum();
      }
    }

    /// <summary>
    /// Gets the gir requested API.
    /// </summary>
    /// <value>The gir requested API.</value>
    public int GirRequestedApi
    {
      get
      {
        return _girRequestedApi(GirMaxApi);
      }
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

    #endregion Properties

    #region Construction / Destruction

    /// <summary>
    /// Initializes a new instance of the <see cref="GirderPluginWrapper"/> class.
    /// </summary>
    /// <param name="fileName">Name of the plugin dll file.</param>
    public GirderPluginWrapper(string fileName)
    {
      if (!LoadGirderPlugin(fileName))
        throw new ApplicationException(String.Format("Failed to load girder plugin ({0})", fileName));

      int requiredApi = GirRequestedApi;

      if (requiredApi > GirMaxApi)
        throw new ApplicationException(String.Format("Failed to load girder plugin ({0}), plugin requires newer API version ({1})", fileName, requiredApi));

      _apiFunctions = new GirApiFunctions();
      _apiFunctions.delete_var = new t_delete_var(delete_var);
      _apiFunctions.get_double_var = new t_get_double_var(get_double_var);
      _apiFunctions.get_int_var = new t_get_int_var(get_int_var);
      _apiFunctions.get_link_name = new t_get_link_name(get_link_name);
      _apiFunctions.get_osd_font_name = new t_get_osd_fontname(get_osd_fontname);
      _apiFunctions.get_osd_settings = new t_get_osd_settings(get_osd_settings);
      _apiFunctions.get_string_var = new t_get_string_var(get_string_var);
      _apiFunctions.gir_free = new t_gir_free(gir_free);
      _apiFunctions.gir_malloc = new t_gir_malloc(gir_malloc);
      _apiFunctions.hide_osd = new t_hide_osd(hide_osd);
      _apiFunctions.i18n_translate = new t_i18n_translate(i18n_translate);
      _apiFunctions.parse_reg_string = new t_parse_girder_reg(parse_girder_reg);
      _apiFunctions.realloc_pchar = new t_realloc_pchar(realloc_pchar);
      _apiFunctions.register_cb = new t_register_cb(register_cb);
      _apiFunctions.run_parser = new t_run_parser(run_parser);
      _apiFunctions.send_event = new t_send_event(send_event);
      _apiFunctions.set_command = new t_set_command(set_command);
      _apiFunctions.set_double_var = new t_set_double_var(set_double_var);
      _apiFunctions.set_int_var = new t_set_int_var(set_int_var);
      _apiFunctions.set_string_var = new t_set_string_var(set_string_var);
      _apiFunctions.show_osd = new t_show_osd(show_osd);
      _apiFunctions.start_osd_draw = new t_start_osd_draw(start_osd_draw);
      _apiFunctions.stop_osd_draw = new t_stop_osd_draw(stop_osd_draw);
      _apiFunctions.target_enum = new t_target_enum(target_enum);
      _apiFunctions.treepicker_show = new t_treepicker_show(treepicker_show);
      _apiFunctions.trigger_command = new t_trigger_command(trigger_command);

      _apiFunctions.parent_hwnd = IntPtr.Zero;
      _apiFunctions.size = Marshal.SizeOf(_apiFunctions);

      byte[] rawData = new byte[_apiFunctions.size];
      _apiFunctionsHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
      _apiFunctionsPtr = _apiFunctionsHandle.AddrOfPinnedObject();
      Marshal.StructureToPtr(_apiFunctions, _apiFunctionsPtr, false);
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
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
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
    /// Gets a value indicating whether this instance can be configured.
    /// </summary>
    /// <value><c>true</c> if this instance can configure; otherwise, <c>false</c>.</value>
    public bool CanConfigure
    {
      get { return _girCommandGui != null; }
    }

    #endregion Plugin Methods

    #region Plugin Callbacks

    bool event_cb(string event_string, int device, IntPtr payload, int len)
    {
#if TRACE
      Trace.WriteLine(String.Format("event_cb({0}, {1})", event_string, device));
#endif

      return true;
    }

    bool parse_girder_reg(string orig, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("parse_girder_reg({0}, {1})", orig, size));
#endif

      return WriteString(szstore, size, "Error");
    }

    bool get_link_name(int lvalue, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_link_name({0})", lvalue));
#endif

      return WriteString(szstore, size, "Error");
    }

    void set_command(GirCommand command) // GirCommand Ptr?
    {
#if TRACE
      Trace.WriteLine(String.Format("set_command()"));
#endif
    }

    void target_enum(int id, t_target_callback callback)
    {
#if TRACE
      Trace.WriteLine(String.Format("target_enum({0})", id));
#endif
    }

    void target_callback(IntPtr hw, GirCommand command) // GirCommand Ptr?
    {
#if TRACE
      Trace.WriteLine(String.Format("target_callback()"));
#endif
    }

    void realloc_pchar(IntPtr old, byte newchar)
    {
#if TRACE
      Trace.WriteLine(String.Format("realloc_pchar({0})", newchar));
#endif

      Marshal.WriteByte(old, newchar);
    }

    void show_osd(int timer)
    {
#if TRACE
      Trace.WriteLine(String.Format("show_osd({0})", timer));
#endif
    }

    void hide_osd()
    {
#if TRACE
      Trace.WriteLine("hide_osd()");
#endif
    }

    bool start_osd_draw(IntPtr hw, IntPtr h, int user)
    {
#if TRACE
      Trace.WriteLine(String.Format("start_osd_draw({0})", user));
#endif
      return true;
    }

    void stop_osd_draw(IntPtr h)
    {
#if TRACE
      Trace.WriteLine("stop_osd_draw");
#endif
    }

    bool treepicker_show(IntPtr window, int id)
    {
#if TRACE
      Trace.WriteLine(String.Format("treepicker_show({0})", id));
#endif
      return true;
    }

    bool register_cb(int actionplugin, t_event_cb callback, string prefix, int device)
    {
#if TRACE
      Trace.WriteLine(String.Format("register_cb({0})", actionplugin));
#endif
      return true;
    }

    bool i18n_translate(string orig, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine("i18n_translate(" + orig + ")");
#endif

      return WriteString(szstore, size, orig);
    }

    int get_osd_settings(int setting) // (return int == dword)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_osd_settings({0})", setting));
#endif

      return 1;
    }

    bool get_osd_fontname(IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine("get_osd_fontname");
#endif

      return WriteString(szstore, size, "Error");
    }

    IntPtr gir_malloc(int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("gir_malloc({0})", size));
#endif
      return Marshal.AllocHGlobal(size);
    }

    void gir_free(IntPtr data)
    {
#if TRACE
      Trace.WriteLine(String.Format("gir_free({0})", data.ToString()));
#endif
      Marshal.FreeHGlobal(data);
    }

    int get_int_var(string name)
    {
#if TRACE
      Trace.WriteLine("get_int_var(" + name + ")");
#endif

      return 0;
    }

    double get_double_var(string name)
    {
#if TRACE
      Trace.WriteLine("get_double_var(" + name + ")");
#endif

      return 0.0;
    }

    bool get_string_var(string name, IntPtr szstore, int size)
    {
#if TRACE
      Trace.WriteLine(String.Format("get_string_var({0})", name));
#endif

      return WriteString(szstore, size, "Error");
    }

    bool set_int_var(string name, int value)
    {
#if TRACE
      Trace.WriteLine(String.Format("set_int_var({0}, {1})", name, value));
#endif
      return true;
    }

    bool set_double_var(string name, double value)
    {
#if TRACE
      Trace.WriteLine(String.Format("set_double_var({0}, {1})", name, value));
#endif
      return true;
    }

    bool set_string_var(string name, string value)
    {
#if TRACE
      Trace.WriteLine(String.Format("set_string_var({0}, {1})", name, value));
#endif
      return true;
    }

    bool delete_var(string name)
    {
#if TRACE
      Trace.WriteLine("delete_var(" + name + ")");
#endif

      return true;
    }

    bool run_parser(string str, IntPtr error_value)
    {
#if TRACE
      Trace.WriteLine(String.Format("run_parser({0}, {1})", str, error_value.ToString()));
#endif
      return true;
    }

    bool send_event(string eventstring, IntPtr payload, int len, int device)
    {
#if TRACE
      Trace.WriteLine(String.Format("Girder Plugin event: {0}, {1}, {2}, {3}", eventstring, payload.ToString(), len, device));
#endif

      if (_eventCallback != null)
        _eventCallback(eventstring, payload, len, device);

      return true;
    }

    bool trigger_command(int command_id)
    {
#if TRACE
      Trace.WriteLine(String.Format("trigger_command({0})", command_id));
#endif
      return true;
    }

    #endregion Plugin Callbacks

    #region Implementation

    bool WriteString(IntPtr stringPtr, int maxSize, string value)
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

    bool LoadGirderPlugin(string girderPluginFile)
    {
      _pluginDll = LoadLibrary(girderPluginFile);
      if (_pluginDll == IntPtr.Zero)
        throw new ApplicationException(String.Format("Failed to load girder plugin ({0})", girderPluginFile));

      try
      {
        IntPtr function;

        function = GetProcAddress(_pluginDll, "gir_version");
        _girVersion = (gir_version)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_version));

        function = GetProcAddress(_pluginDll, "gir_name");
        _girName = (gir_name)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_name));

        function = GetProcAddress(_pluginDll, "gir_description");
        _girDescription = (gir_description)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_description));

        function = GetProcAddress(_pluginDll, "gir_devicenum");
        _girDevicenum = (gir_devicenum)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_devicenum));

        function = GetProcAddress(_pluginDll, "gir_requested_api");
        _girRequestedApi = (gir_requested_api)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_requested_api));


        function = GetProcAddress(_pluginDll, "gir_open");
        _girOpen = (gir_open)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_open));

        function = GetProcAddress(_pluginDll, "gir_close");
        _girClose = (gir_close)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_close));
        

        function = GetProcAddress(_pluginDll, "gir_command_gui");
        if (function != IntPtr.Zero) // Optional.
          _girCommandGui = (gir_command_gui)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_command_gui));

        function = GetProcAddress(_pluginDll, "gir_info");
        if (function != IntPtr.Zero) // Optional.
          _girInfo = (gir_info)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_info));
        
        function = GetProcAddress(_pluginDll, "gir_start");
        if (function != IntPtr.Zero) // Optional.
          _girStart = (gir_start)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_start));

        function = GetProcAddress(_pluginDll, "gir_stop");
        if (function != IntPtr.Zero) // Optional.
          _girStop = (gir_stop)Marshal.GetDelegateForFunctionPointer(function, typeof(gir_stop));

        return true;
      }
      catch
      {
        FreeLibrary(_pluginDll);
      }

      return false;
    }
    
    #endregion Implementation
    
  }

}
