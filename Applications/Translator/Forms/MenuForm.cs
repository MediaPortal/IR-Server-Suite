using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

    #region Constants

    const string WindowTitle = "Translator OSD";

    // Menus
    const string MenuMain     = "Main";
    const string MenuLaunch   = "Launch";
    const string MenuTasks    = "Tasks";
    const string MenuMacros   = "Macros";
    const string MenuSystem   = "System";
    const string MenuWindows  = "Windows";
    const string MenuPower    = "Power";
    const string MenuAudio    = "Audio";
    const string MenuEject    = "Eject";

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

    List<string> _menuStack;

    string _startMenu = "Main";

    #endregion Variables

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuForm"/> class.
    /// </summary>
    public MenuForm()
    {
      InitializeComponent();

      CreateMainImageList();

      _launch = new ListViewItem(UITextLaunch, 0);
      _launch.ToolTipText = DescLaunch;
      _launch.Tag = TagMenu + MenuLaunch;

      _taskSwitch = new ListViewItem(UITextTasks, 1);
      _taskSwitch.ToolTipText = DescTasks;
      _taskSwitch.Tag = TagMenu + MenuTasks;

      _macro = new ListViewItem(UITextMacros, 2);
      _macro.ToolTipText = DescMacros;
      _macro.Tag = TagMenu + MenuMacros;

      _system = new ListViewItem(UITextSystem, 3);
      _system.ToolTipText = DescSystem;
      _system.Tag = TagMenu + MenuSystem;

      _menuStack = new List<string>();
    }
    
    private void MenuForm_Shown(object sender, EventArgs e)
    {
      SwitchMenu(_startMenu);
      _startMenu = MenuMain;

      Win32.SetForegroundWindow(this.Handle, true);

      //listViewMenu.Select();
    }

    private void MenuForm_Deactivate(object sender, EventArgs e)
    {
      this.Close();
    }


    
    void CreateMainImageList()
    {
      _listMain = new ImageList();
      _listMain.ColorDepth = ColorDepth.Depth32Bit;
      _listMain.ImageSize = new Size(32, 32);

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

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
      listViewMenu.Scrollable = true;

    }
    void SetToListStyle()
    {
      listViewMenu.View = View.Details;
      listViewMenu.Alignment = ListViewAlignment.Top;
      listViewMenu.Scrollable = true;

    }
    
    void SetWindowTitle(string subMenuName)
    {
      if (String.IsNullOrEmpty(subMenuName))
        labelHeader.Text = WindowTitle;
      else
        labelHeader.Text = WindowTitle + " - " + subMenuName;
    }


    void LoadMenuMain()
    {
      SetWindowTitle(UITextMain);

      listViewMenu.LargeImageList = _listMain;

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToIconStyle();

      listViewMenu.Items.Add(_launch);
      listViewMenu.Items.Add(_taskSwitch); 
      listViewMenu.Items.Add(_macro);
      listViewMenu.Items.Add(_system);

      _launch.Selected = true;
    }

    void LoadMenuLaunch()
    {
      SetWindowTitle(UITextLaunch);

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      int index = 0;
      foreach (ProgramSettings program in Program.Config.Programs)
      {
        newList.Images.Add(Win32.GetIconFor(program.FileName));
        ListViewItem item = new ListViewItem(program.Name, index++);
        item.ToolTipText = program.FileName;
        item.Tag = TagLaunch + program.FileName;

        listViewMenu.Items.Add(item);
      }

      listViewMenu.Items[0].Selected = true;
    }
    void LoadMenuTasks()
    {
      SetWindowTitle(UITextTasks);

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      SetToIconStyle();

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

      Win32.ExtractIcons(file, IconDesktop, out large, out small);
      newList.Images.Add(large);

      ListViewItem desktop = new ListViewItem("Desktop", 0);
      desktop.ToolTipText = "Show the Desktop";
      desktop.Tag = IntPtr.Zero;
      desktop.Selected = true;
      listViewMenu.Items.Add(desktop);

      PopulateTaskList();
    }
    void LoadMenuMacros()
    {
      SetWindowTitle(UITextMacros);

      listViewMenu.LargeImageList = null;

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToListStyle();

      string[] macros = Program.GetMacroList(false);
      if (macros.Length == 0)
        return;

      foreach (string macro in macros)
      {
        ListViewItem item = new ListViewItem(macro);
        item.Tag = TagMacro + macro;

        listViewMenu.Items.Add(item);
      }

      listViewMenu.Items[0].Selected = true;
    }
    void LoadMenuSystem()
    {
      SetWindowTitle(UITextSystem);

      listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

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
      item.Tag = TagMenu + MenuWindows;
      listViewMenu.Items.Add(item);

      item = new ListViewItem(UITextPower, 1);
      item.ToolTipText = DescPower;
      item.Tag = TagMenu + MenuPower;
      listViewMenu.Items.Add(item);

      item = new ListViewItem(UITextAudio, 2);
      item.ToolTipText = DescAudio;
      item.Tag = TagMenu + MenuAudio;
      listViewMenu.Items.Add(item);

      item = new ListViewItem(UITextEject, 3);
      item.ToolTipText = DescEject;
      item.Tag = TagMenu + MenuEject;
      listViewMenu.Items.Add(item);

      listViewMenu.Items[0].Selected = true;
    }

    void LoadMenuWindows()
    {
      SetWindowTitle(UITextWindows);

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

      Win32.ExtractIcons(file, IconDesktop, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem("Minimize all", 0);
      item.ToolTipText = "Minimize all open windows";
      item.Tag = IntPtr.Zero;
      listViewMenu.Items.Add(item);

      item.Selected = true;

    }
    void LoadMenuPower()
    {
      SetWindowTitle("Power");

      listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

      Win32.ExtractIcons(file, 15, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem("Shutdown", 0);
      item.ToolTipText = "Shutdown windows";
      item.Tag = TagCommand + CommandShutdown;
      listViewMenu.Items.Add(item);
      
      item.Selected = true;

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

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

      Win32.ExtractIcons(file, IconAudio, out large, out small);
      newList.Images.Add(large);

      ListViewItem item;

      item = new ListViewItem("Volume Up", 0);
      item.ToolTipText = "Increase the audio volume";
      item.Tag = TagCommand + "Volume Up";
      listViewMenu.Items.Add(item);

      item.Selected = true;

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
      SetWindowTitle(UITextAudio);

      //listViewMenu.SelectedItems.Clear();
      listViewMenu.Items.Clear();

      SetToIconStyle();

      ImageList newList = new ImageList();
      newList.ColorDepth = ColorDepth.Depth32Bit;
      newList.ImageSize = new Size(32, 32);

      listViewMenu.LargeImageList = newList;

      Icon large;
      Icon small;

      string folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
      string file = folder + "\\shell32.dll";

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

      if (drives.Length > 0)
        listViewMenu.Items[0].Selected = true;
    }

    void SwitchMenu(string menuName)
    {
      _menuStack.Add(menuName);

      switch (menuName)
      {
        case MenuMain:    LoadMenuMain(); break;

        case MenuLaunch:  LoadMenuLaunch(); break;
        case MenuTasks:   LoadMenuTasks(); break;
        case MenuMacros:  LoadMenuMacros(); break;
        case MenuSystem:  LoadMenuSystem(); break;

        case MenuWindows: LoadMenuWindows(); break;
        case MenuPower:   LoadMenuPower(); break;
        case MenuAudio:   LoadMenuAudio(); break;
        case MenuEject:   LoadMenuEject(); break;
      }

      ResizeWindow();

      //listViewMenu.Focus();
    }

    void ResizeWindow()
    {
      if (listViewMenu.View == View.Details)
      {
        this.Size = this.MinimumSize;
      }
      else
      {
        int requiredWidth = 0;
        int requiredHeight = 0;

        foreach (ListViewItem item in listViewMenu.Items)
        {
          requiredWidth += item.Bounds.Width + 4;

          if (requiredHeight < item.Bounds.Height)
            requiredHeight = item.Bounds.Height;
        }

        int finalWidth = requiredWidth;
        int finalHeight = requiredHeight + 16;

        if (finalWidth > this.MaximumSize.Width)
          finalWidth = this.MaximumSize.Width;
        else if (finalWidth < this.MinimumSize.Width)
          finalWidth = this.MinimumSize.Width;

        if (finalHeight > this.MaximumSize.Height)
          finalHeight = this.MaximumSize.Height;
        else if (finalHeight < this.MinimumSize.Height)
          finalHeight = this.MinimumSize.Height;

        this.Size = new Size(finalWidth, finalHeight);
      }

      Screen thisScreen = Screen.FromPoint(this.Location);
      Rectangle workingArea = thisScreen.Bounds;

      this.Location =
        new Point(
          workingArea.X + (workingArea.Width / 2) - (this.Size.Width / 2),
          workingArea.Y + (workingArea.Height / 2) - (this.Size.Height / 2));
    }

    void Launch(string programFile)
    {
      this.Close();

      foreach (ProgramSettings settings in Program.Config.Programs)
      {
        if (settings.FileName.Equals(programFile))
        {
          string launchCommand = settings.LaunchCommand();
          string[] commands = Common.SplitRunCommand(launchCommand);

          Common.ProcessRunCommand(commands);
          break;
        }
      }
    }
    void Macro(string macroName)
    {
      this.Close();

      Program.ProcessMacro(Program.FolderMacros + macroName);
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
        case CommandShutdown:   Shutdown(); break;
        case CommandReboot:     Reboot(); break;
        case CommandLogOff:     LogOff(); break;
        case CommandStandby:    Standby(); break;
        case CommandHibernate:  Hibernate(); break;


      }
    }
    void Eject(string drive)
    {
      this.Close();

      CDRom.Open(drive);
    }


    static void Hibernate()
    {
      IrssLog.Info("Hibernate");

      if (!Application.SetSuspendState(PowerState.Hibernate, false, false))
        IrssLog.Warn("Hibernate request was rejected by another application.");
    }
    static void Standby()
    {
      IrssLog.Info("Standby");

      if (!Application.SetSuspendState(PowerState.Suspend, false, false))
        IrssLog.Warn("Standby request was rejected by another application.");
    }
    static void Reboot()
    {
      IrssLog.Info("Reboot");
      Win32.WindowsExit(Win32.ExitWindows.Reboot, Win32.ShutdownReasons.FlagUserDefined);
    }
    static void LogOff()
    {
      IrssLog.Info("LogOff");
      Win32.WindowsExit(Win32.ExitWindows.LogOff, Win32.ShutdownReasons.FlagUserDefined);
    }
    static void Shutdown()
    {
      IrssLog.Info("ShutDown");
      Win32.WindowsExit(Win32.ExitWindows.ShutDown, Win32.ShutdownReasons.FlagUserDefined);
    }

    private void listViewMenu_ItemActivate(object sender, EventArgs e)
    {
      ListViewItem selectedItem = listViewMenu.SelectedItems[0];

      if (selectedItem.Tag is string)
      {
        string tag = selectedItem.Tag as string;

        if (tag.StartsWith(TagMenu))
        {
          string menu = tag.Substring(TagMenu.Length);
          SwitchMenu(menu);
        }
        else if (tag.StartsWith(TagLaunch))
        {
          string program = tag.Substring(TagLaunch.Length);
          Launch(program);
        }
        else if (tag.StartsWith(TagMacro))
        {
          string macro = tag.Substring(TagMacro.Length);
          Macro(macro);
        }
        else if (tag.StartsWith(TagCommand))
        {
          string command = tag.Substring(TagCommand.Length);
          Command(command);
        }
        else if (tag.StartsWith(TagEject))
        {
          string drive = tag.Substring(TagEject.Length);
          Eject(drive);
        }
      }
      else if (selectedItem.Tag is IntPtr)
      {
        TaskSwap((IntPtr)selectedItem.Tag);
      }

    }

    private void listViewMenu_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 27)
      {
        if (_menuStack.Count == 1)
        {
          this.Close();
        }
        else
        {
          // Remove current
          _menuStack.RemoveAt(_menuStack.Count - 1);
          
          // Get previous
          string previous = _menuStack[_menuStack.Count - 1];
          
          // Remove previous
          _menuStack.RemoveAt(_menuStack.Count - 1);

          // Switch to previous
          SwitchMenu(previous);
        }
        
        e.Handled = true;
      }
    }

    void PopulateTaskList()
    {
      Win32.EnumWindowsProc ewp = new Win32.EnumWindowsProc(AddTask);

      Win32.EnumerateWindows(ewp, IntPtr.Zero);
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
