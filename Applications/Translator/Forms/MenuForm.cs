using System;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Translator
{

  /// <summary>
  /// Translator Menu.
  /// </summary>
  public partial class MenuForm : Form
  {

    #region Enumerations

    enum Menus
    {
      Main,

      Launch,
      Tasks,
      Macros,
      System,
      Windows,
      Power,
      Audio,
      Eject,

      WindowActivate,
      WindowClose,
      WindowMinimize,
      WindowMaximize,

    }

    #endregion Enumerations

    #region Constants

    const string WindowTitle = "Translator OSD";

    // Menu UI Text
    const string UITextMain     = "Main Menu";
    const string UITextLaunch   = "Launch App";
    const string UITextTasks    = "Task Swap";
    const string UITextMacros   = "Macros";
    const string UITextSystem   = "System Commands";
    const string UITextWindows  = "Window Management";
    const string UITextPower    = "Power";
    const string UITextAudio    = "Audio";
    const string UITextEject    = "Eject";
    
    // Menu descriptions
    const string DescLaunch   = "Launch an application";
    const string DescTasks    = "Switch between running programs";
    const string DescMacros   = "Run a macro";
    const string DescSystem   = "Perform a system command";
    const string DescWindows  = "Manage open windows";
    const string DescPower    = "Modify the computer's power state";
    const string DescAudio    = "Adjust the system volume";
    const string DescEject    = "Eject a disc";

    // Tags
    const string TagMenu    = "Menu: ";
    const string TagMacro   = "Macro: ";
    const string TagLaunch  = "Launch: ";
    const string TagCommand = "Command: ";
    const string TagEject   = "Eject: ";

    // Commands
    const string CommandShutdown  = "Shutdown";
    const string CommandReboot    = "Reboot";
    const string CommandLogOff    = "LogOff";
    const string CommandStandby   = "Standby";
    const string CommandHibernate = "Hibernate";

    // Icon indexes
    const int IconLaunch  = 24;
    const int IconTasks   = 98;
    const int IconMacros  = 70;
    const int IconDesktop = 34;
    const int IconWindows = 15;
    const int IconPower   = 27;
    const int IconAudio   = 168;
    const int IconEject   = 188;

    #endregion Constants

    #region Variables

    ImageList _listMain;

    ListViewItem _launch;
    ListViewItem _taskSwitch;
    ListViewItem _macro;
    ListViewItem _system;

    List<Menus> _menuStack;

    Win32.EnumWindowsProc _ewp;

    #endregion Variables

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuForm"/> class.
    /// </summary>
    public MenuForm()
    {
      InitializeComponent();

      _ewp = new Win32.EnumWindowsProc(AddTask);

      CreateMainImageList();

      _launch = new ListViewItem(UITextLaunch, 0);
      _launch.ToolTipText = DescLaunch;
      _launch.Tag = Menus.Launch;

      _taskSwitch = new ListViewItem(UITextTasks, 1);
      _taskSwitch.ToolTipText = DescTasks;
      _taskSwitch.Tag = Menus.Tasks;

      _macro = new ListViewItem(UITextMacros, 2);
      _macro.ToolTipText = DescMacros;
      _macro.Tag = Menus.Macros;

      _system = new ListViewItem(UITextSystem, 3);
      _system.ToolTipText = DescSystem;
      _system.Tag = Menus.System;
    }

    private void MenuForm_Load(object sender, EventArgs e)
    {
      _menuStack = new List<Menus>();

      SwitchMenu(Menus.Main);
    }
    private void MenuForm_Shown(object sender, EventArgs e)
    {
      Win32.SetForegroundWindow(this.Handle, true);
    }
    private void MenuForm_Deactivate(object sender, EventArgs e)
    {
      this.Close();
    }
    private void MenuForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      _menuStack = null;
    }

    void CreateMainImageList()
    {
      _listMain = new ImageList();
      _listMain.ColorDepth = ColorDepth.Depth32Bit;
      _listMain.ImageSize = new Size(32, 32);

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, IconLaunch, out large, out small);
      _listMain.Images.Add(large);

      Win32.ExtractIcons(file, IconTasks, out large, out small);
      _listMain.Images.Add(large);

      Win32.ExtractIcons(file, IconMacros, out large, out small);
      _listMain.Images.Add(large);

      _listMain.Images.Add(Properties.Resources.WinLogo);
    }

    void SetToIconStyle()
    {
      listViewMenu.View = View.LargeIcon;
      listViewMenu.Alignment = ListViewAlignment.Left;
    }
    void SetToListStyle()
    {
      listViewMenu.View = View.Details;
      listViewMenu.Alignment = ListViewAlignment.Top;
    }

    void SetWindowTitle(string subMenuName)
    {
      if (String.IsNullOrEmpty(subMenuName))
        labelHeader.Text = WindowTitle;
      else
        labelHeader.Text = String.Format("{0} - {1}", WindowTitle, subMenuName);
    }


    void LoadMenuMain()
    {
      SetWindowTitle(UITextMain);

      listViewMenu.LargeImageList = _listMain;

      SetToIconStyle();

      listViewMenu.Items.Add(_taskSwitch);
      listViewMenu.Items.Add(_launch);
      listViewMenu.Items.Add(_macro);
      listViewMenu.Items.Add(_system);
    }

    void LoadMenuTasks()
    {
      SetWindowTitle(UITextTasks);

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, IconDesktop, out large, out small);
      newList.Images.Add(large);

      ListViewItem desktop = new ListViewItem("Desktop", 0);
      desktop.ToolTipText = "Show the Desktop";
      desktop.Tag = IntPtr.Zero;
      listViewMenu.Items.Add(desktop);

      PopulateTaskList();
    }
    void LoadMenuLaunch()
    {
      SetWindowTitle(UITextLaunch);

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      int index = 0;
      foreach (ProgramSettings program in Program.Config.Programs)
      {
        if (String.IsNullOrEmpty(program.FileName))
          continue;

        Icon icon = Win32.GetIconFor(program.FileName);
        newList.Images.Add(icon);

        ListViewItem item = new ListViewItem(program.Name, index++);
        item.ToolTipText = program.FileName;
        item.Tag = TagLaunch + program.FileName;

        listViewMenu.Items.Add(item);
      }
    }
    void LoadMenuMacros()
    {
      SetWindowTitle(UITextMacros);

      listViewMenu.LargeImageList = null;

      SetToListStyle();

      string[] macros = IrssMacro.GetMacroList(Program.FolderMacros, false);
      if (macros.Length == 0)
        return;

      foreach (string macro in macros)
      {
        ListViewItem item = new ListViewItem(macro);
        item.Tag = TagMacro + macro;

        listViewMenu.Items.Add(item);
      }
    }
    void LoadMenuSystem()
    {
      SetWindowTitle(UITextSystem);

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, IconWindows, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, IconPower, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, IconAudio, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, IconEject, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem(UITextWindows, 0);
      item.ToolTipText = DescWindows;
      item.Tag = Menus.Windows;
      listViewMenu.Items.Add(item);

      item = new ListViewItem(UITextPower, 1);
      item.ToolTipText = DescPower;
      item.Tag = Menus.Power;
      listViewMenu.Items.Add(item);

      item = new ListViewItem(UITextAudio, 2);
      item.ToolTipText = DescAudio;
      item.Tag = Menus.Audio;
      listViewMenu.Items.Add(item);

      item = new ListViewItem(UITextEject, 3);
      item.ToolTipText = DescEject;
      item.Tag = Menus.Eject;
      listViewMenu.Items.Add(item);
    }

    void LoadMenuWindows()
    {
      SetWindowTitle(UITextWindows);

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, IconTasks, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, 131, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, IconDesktop, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, IconDesktop, out large, out small);
      newList.Images.Add(large);

      Win32.ExtractIcons(file, IconDesktop, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem("Activate", 0);
      item.ToolTipText = "Activate a window";
      item.Tag = Menus.WindowActivate;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Close", 1);
      item.ToolTipText = "Close a window";
      item.Tag = Menus.WindowClose;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Maximize", 2);
      item.ToolTipText = "Maximize a window";
      item.Tag = Menus.WindowMaximize;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Minimize", 3);
      item.ToolTipText = "Minimize a window";
      item.Tag = Menus.WindowMinimize;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Minimize all", 4);
      item.ToolTipText = "Minimize all open windows";
      item.Tag = IntPtr.Zero;
      listViewMenu.Items.Add(item);

    }
    void LoadMenuPower()
    {
      SetWindowTitle(UITextPower);

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, 15, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem("Shutdown", 0);
      item.ToolTipText = "Shutdown windows";
      item.Tag = TagCommand + CommandShutdown;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Reboot", 0);
      item.ToolTipText = "Reboot windows";
      item.Tag = TagCommand + CommandReboot;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Logoff", 0);
      item.ToolTipText = "Logoff the current user";
      item.Tag = TagCommand + CommandLogOff;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Standby", 0);
      item.ToolTipText = "Enter low-power standby mode";
      item.Tag = TagCommand + CommandStandby;
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Hibernate", 0);
      item.ToolTipText = "Enter hibernate mode";
      item.Tag = TagCommand + CommandHibernate;
      listViewMenu.Items.Add(item);
    }
    void LoadMenuAudio()
    {
      SetWindowTitle(UITextAudio);

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, IconAudio, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem("Volume Up", 0);
      item.ToolTipText = "Increase the audio volume";
      item.Tag = TagCommand + "Volume Up";
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Volume Down", 0);
      item.ToolTipText = "Decreases the audio volume";
      item.Tag = TagCommand + "Volume Down";
      listViewMenu.Items.Add(item);

      item = new ListViewItem("Volume Mute", 0);
      item.ToolTipText = "Mutes the audio";
      item.Tag = TagCommand + "Mute";
      listViewMenu.Items.Add(item);

    }
    void LoadMenuEject()
    {
      SetWindowTitle(UITextEject);

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = Path.Combine(folder, "shell32.dll");

      Win32.ExtractIcons(file, IconEject, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      DriveInfo[] drives = DriveInfo.GetDrives();
      foreach (DriveInfo drive in drives)
      {
        if (drive.DriveType == DriveType.CDRom)
        {
          item = new ListViewItem("Eject " + drive.Name, 0);
          item.ToolTipText = "Eject drive " + drive.Name;
          item.Tag = TagEject + drive.Name;
          listViewMenu.Items.Add(item);
        }
      }
    }

    void LoadMenuWindowActivate()
    {
      SetWindowTitle("Activate Window");

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      PopulateTaskList();
    }
    void LoadMenuWindowClose()
    {
      SetWindowTitle("Close Window");

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      PopulateTaskList();
    }
    void LoadMenuWindowMaximize()
    {
      SetWindowTitle("Maximize Window");

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      PopulateTaskList();
    }
    void LoadMenuWindowMinimize()
    {
      SetWindowTitle("Minimize Window");

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      PopulateTaskList();
    }



    void SwitchMenu(Menus menu)
    {
      _menuStack.Add(menu);

      listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      switch (menu)
      {
        case Menus.Tasks:   LoadMenuTasks();    break;
        case Menus.Launch:  LoadMenuLaunch();   break;
        case Menus.Macros:  LoadMenuMacros();   break;
        case Menus.System:  LoadMenuSystem();   break;

        case Menus.Windows: LoadMenuWindows();  break;
        case Menus.Power:   LoadMenuPower();    break;
        case Menus.Audio:   LoadMenuAudio();    break;
        case Menus.Eject:   LoadMenuEject();    break;

        case Menus.WindowActivate:  LoadMenuWindowActivate(); break;
        case Menus.WindowClose:     LoadMenuWindowClose();    break;
        case Menus.WindowMaximize:  LoadMenuWindowMaximize(); break;
        case Menus.WindowMinimize:  LoadMenuWindowMinimize(); break;

        default:            LoadMenuMain();     break;
      }

      ResizeWindow();

      if (listViewMenu.Items.Count > 0)
      {
        listViewMenu.Items[0].Selected = true;
        listViewMenu.Items[0].Focused = true;
      }
    }

    void ResizeWindow()
    {
      int newWidth = 0;
      int newHeight = 0;

      if (listViewMenu.View == View.Details)
      {
        newWidth = this.MinimumSize.Width;
        foreach (ListViewItem item in listViewMenu.Items)
          newHeight += item.Bounds.Height + 2;

        //newHeight += 1;
      }
      else
      {
        foreach (ListViewItem item in listViewMenu.Items)
        {
          newWidth += item.Bounds.Width + 4;

          if (newHeight < item.Bounds.Height)
            newHeight = item.Bounds.Height;
        }

        newHeight += 16;
        newWidth += 8;
      }

      if (newWidth > this.MaximumSize.Width)
        newWidth = this.MaximumSize.Width;
      else if (newWidth < this.MinimumSize.Width)
        newWidth = this.MinimumSize.Width;

      if (newHeight > this.MaximumSize.Height)
        newHeight = this.MaximumSize.Height;
      else if (newHeight < this.MinimumSize.Height)
        newHeight = this.MinimumSize.Height;

      this.Size = new Size(newWidth, newHeight);

      Screen thisScreen = Screen.FromPoint(this.Location);
      Rectangle workingArea = thisScreen.Bounds;

      this.Location =
        new Point(
          workingArea.X + (workingArea.Width / 2) - (newWidth / 2),
          workingArea.Y + (workingArea.Height / 2) - (newHeight / 2));
    }

    void Launch(string programFile)
    {
      this.Close();

      foreach (ProgramSettings settings in Program.Config.Programs)
      {
        if (settings.FileName.Equals(programFile))
        {
          Program.ProcessCommand(Common.CmdPrefixRun + settings.RunCommandString, true);
          break;
        }
      }
    }
    void RunMacro(string macroName)
    {
      this.Close();

      Program.ProcessCommand(Common.CmdPrefixMacro + macroName, true);
    }
    void TaskSwap(IntPtr window)
    {
      this.Close();

      if (window == IntPtr.Zero)
        Win32.ShowDesktop();
      else
        Win32.ActivateWindowByHandle(window);
    }
    void Command(string command)
    {
      this.Close();

      switch (command)
      {
        case CommandShutdown:   Program.ProcessCommand(Common.CmdPrefixShutdown, true);   break;
        case CommandReboot:     Program.ProcessCommand(Common.CmdPrefixReboot, true);     break;
        case CommandLogOff:     Program.ProcessCommand(Common.CmdPrefixLogOff, true);     break;
        case CommandStandby:    Program.ProcessCommand(Common.CmdPrefixStandby, true);    break;
        case CommandHibernate:  Program.ProcessCommand(Common.CmdPrefixHibernate, true);  break;


      }
    }
    void Eject(string drive)
    {
      this.Close();

      Program.ProcessCommand(Common.CmdPrefixEject + drive, true);
    }

    void Close(IntPtr window)
    {
      this.Close();

      if (window == IntPtr.Zero)
        return;

      Win32.SendWindowsMessage(window, (int)Win32.WindowsMessage.WM_SYSCOMMAND, (int)Win32.SysCommand.SC_CLOSE, 0);
    }
    void Minimize(IntPtr window)
    {
      this.Close();

      if (window == IntPtr.Zero)
        return;

      Win32.SendWindowsMessage(window, (int)Win32.WindowsMessage.WM_SYSCOMMAND, (int)Win32.SysCommand.SC_MINIMIZE, 0);
    }
    void Maximize(IntPtr window)
    {
      this.Close();

      if (window == IntPtr.Zero)
        return;

      Win32.SendWindowsMessage(window, (int)Win32.WindowsMessage.WM_SYSCOMMAND, (int)Win32.SysCommand.SC_MAXIMIZE, 0);
    }

    void ActivateItem(int index)
    {
      if (index >= listViewMenu.Items.Count)
        return;

      try
      {
        ListViewItem selectedItem = listViewMenu.Items[index];

        if (selectedItem.Tag == null)
        {
          return;
        }
        else if (selectedItem.Tag is Menus)
        {
          SwitchMenu((Menus)selectedItem.Tag);
        }
        else if (selectedItem.Tag is IntPtr)
        {
          IntPtr handle = (IntPtr)selectedItem.Tag;

          switch (GetCurrentMenu())
          {
            case Menus.Tasks:           TaskSwap(handle); break;
            case Menus.Windows:         TaskSwap(handle); break;
            case Menus.WindowActivate:  TaskSwap(handle); break;
            case Menus.WindowClose:     Close(handle);    break;
            case Menus.WindowMaximize:  Maximize(handle); break;
            case Menus.WindowMinimize:  Minimize(handle); break;
          }
        }
        else if (selectedItem.Tag is string)
        {
          string tag = (string)selectedItem.Tag;

          if (tag.StartsWith(TagLaunch, StringComparison.OrdinalIgnoreCase))
            Launch(tag.Substring(TagLaunch.Length));
          else if (tag.StartsWith(TagMacro, StringComparison.OrdinalIgnoreCase))
            RunMacro(tag.Substring(TagMacro.Length));
          else if (tag.StartsWith(TagCommand, StringComparison.OrdinalIgnoreCase))
            Command(tag.Substring(TagCommand.Length));
          else if (tag.StartsWith(TagEject, StringComparison.OrdinalIgnoreCase))
            Eject(tag.Substring(TagEject.Length));
        }
        else
        {
          throw new InvalidOperationException("Unexpected selectedItem Tag data type");
        }
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }
    }


    private void listViewMenu_ItemActivate(object sender, EventArgs e)
    {
      ActivateItem(listViewMenu.SelectedItems[0].Index);
    }

    private void listViewMenu_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = true;

      switch (e.KeyChar)
      {
        case (char)27:  // ESC
          if (_menuStack.Count == 1)
          {
            this.Close();
          }
          else
          {
            // Remove current
            _menuStack.RemoveAt(_menuStack.Count - 1);

            // Get previous
            Menus previous = _menuStack[_menuStack.Count - 1];

            // Remove previous
            _menuStack.RemoveAt(_menuStack.Count - 1);

            // Switch to previous
            SwitchMenu(previous);
          }
          break;

        case '1': ActivateItem(0); break;
        case '2': ActivateItem(1); break;
        case '3': ActivateItem(2); break;
        case '4': ActivateItem(3); break;
        case '5': ActivateItem(4); break;
        case '6': ActivateItem(5); break;
        case '7': ActivateItem(6); break;
        case '8': ActivateItem(7); break;
        case '9': ActivateItem(8); break;

        default:
          e.Handled = false;
          break;
      }

    }

    Menus GetCurrentMenu()
    {
      return _menuStack[_menuStack.Count - 1];
    }

    void PopulateTaskList()
    {
      Win32.EnumerateWindows(_ewp, IntPtr.Zero);
    }

    bool AddTask(IntPtr hWnd, IntPtr lParam)
    {
      IntPtr style = Win32.GetWindowLongPtr(hWnd, Win32.GWL.GWL_STYLE);
      if (!Win32.CheckMask(style.ToInt32(), (int)Win32.WindowStyles.WS_VISIBLE))
        return true;

      IntPtr exStyle = Win32.GetWindowLongPtr(hWnd, Win32.GWL.GWL_EXSTYLE);
      if (Win32.CheckMask(exStyle.ToInt32(), (int)Win32.WindowExStyles.WS_EX_TOOLWINDOW))
          return true;

      string title = Win32.GetWindowTitle(hWnd);
      if (String.IsNullOrEmpty(title))
        return true;
              
      Icon icon = Win32.GetWindowIcon(hWnd);
      if (icon == null)
        return true;

      int index = listViewMenu.LargeImageList.Images.Count;

      listViewMenu.LargeImageList.Images.Add(icon);

      ListViewItem item = new ListViewItem(title, index);
      item.Tag = hWnd;
      item.ToolTipText = title;
      listViewMenu.Items.Add(item);

      return true;
    }

  }

}
