using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IrssCommands.General
{
  public partial class MouseConfig : BaseCommandConfig
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get
      {
        string cmd = GetMouseCommand();
        string param1 = string.Empty;
        string param2 = string.Empty;

        if (cmd.Equals(MouseCommand.MouseMoveToPos))
        {
          param1 = numericUpDownX.Value.ToString();
          param2 = numericUpDownY.Value.ToString();
        }

        if (cmd.Equals(MouseCommand.MouseMoveUp) || cmd.Equals(MouseCommand.MouseMoveDown) ||
            cmd.Equals(MouseCommand.MouseMoveLeft) || cmd.Equals(MouseCommand.MouseMoveRight))
        {
          param1 = numericUpDownMouseMove.Value.ToString();
        }

        return new[]
          {
            cmd,
            param1,
            param2
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseConfig"/> class.
    /// </summary>
    private MouseConfig()
    {
      InitializeComponent();

      // Create an instance of HookProc.
      MouseHookProcedure = UpdateCurrentPosition;
      hHook = SetWindowsHookEx(WH_MOUSE,
                               MouseHookProcedure,
                               (IntPtr) 0,
                               AppDomain.GetCurrentThreadId());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public MouseConfig(string[] parameters)
      : this()
    {
      string cmd = parameters[0];

      checkBoxMouseClickLeft.Checked = cmd.Equals(MouseCommand.MouseClickLeft);
      checkBoxMouseClickMiddle.Checked = cmd.Equals(MouseCommand.MouseClickMiddle);
      checkBoxMouseClickRight.Checked = cmd.Equals(MouseCommand.MouseClickRight);

      checkBoxMouseDoubleLeft.Checked = cmd.Equals(MouseCommand.MouseDoubleClickLeft);
      checkBoxMouseDoubleMiddle.Checked = cmd.Equals(MouseCommand.MouseDoubleClickMiddle);
      checkBoxMouseDoubleRight.Checked = cmd.Equals(MouseCommand.MouseDoubleClickRight);

      checkBoxMouseScrollUp.Checked = cmd.Equals(MouseCommand.MouseScrollUp);
      checkBoxMouseScrollDown.Checked = cmd.Equals(MouseCommand.MouseScrollDown);

      checkBoxMouseMoveToPos.Checked = cmd.Equals(MouseCommand.MouseMoveToPos);

      checkBoxMouseMoveUp.Checked = cmd.Equals(MouseCommand.MouseMoveUp);
      checkBoxMouseMoveDown.Checked = cmd.Equals(MouseCommand.MouseMoveDown);
      checkBoxMouseMoveLeft.Checked = cmd.Equals(MouseCommand.MouseMoveLeft);
      checkBoxMouseMoveRight.Checked = cmd.Equals(MouseCommand.MouseMoveRight);

      if (cmd.Equals(MouseCommand.MouseMoveToPos))
      {
        numericUpDownX.Value = int.Parse(parameters[1]);
        numericUpDownY.Value = int.Parse(parameters[2]);
      }

      if (cmd.Equals(MouseCommand.MouseMoveUp) || cmd.Equals(MouseCommand.MouseMoveDown) ||
          cmd.Equals(MouseCommand.MouseMoveLeft) || cmd.Equals(MouseCommand.MouseMoveRight))
      {
        numericUpDownMouseMove.Value = int.Parse(parameters[1]);
      }
    }

    #endregion Constructors

    #region Implementation

    private void checkBoxMouse_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox origin = (CheckBox) sender;

      if (!origin.Checked)
        return;

      if (origin != checkBoxMouseClickLeft) checkBoxMouseClickLeft.Checked = false;
      if (origin != checkBoxMouseClickRight) checkBoxMouseClickRight.Checked = false;
      if (origin != checkBoxMouseClickMiddle) checkBoxMouseClickMiddle.Checked = false;

      if (origin != checkBoxMouseDoubleLeft) checkBoxMouseDoubleLeft.Checked = false;
      if (origin != checkBoxMouseDoubleRight) checkBoxMouseDoubleRight.Checked = false;
      if (origin != checkBoxMouseDoubleMiddle) checkBoxMouseDoubleMiddle.Checked = false;

      if (origin != checkBoxMouseScrollUp) checkBoxMouseScrollUp.Checked = false;
      if (origin != checkBoxMouseScrollDown) checkBoxMouseScrollDown.Checked = false;

      if (origin != checkBoxMouseMoveToPos) checkBoxMouseMoveToPos.Checked = false;

      if (origin != checkBoxMouseMoveUp) checkBoxMouseMoveUp.Checked = false;
      if (origin != checkBoxMouseMoveDown) checkBoxMouseMoveDown.Checked = false;
      if (origin != checkBoxMouseMoveLeft) checkBoxMouseMoveLeft.Checked = false;
      if (origin != checkBoxMouseMoveRight) checkBoxMouseMoveRight.Checked = false;
    }

    public string GetMouseCommand()
    {
      if (checkBoxMouseClickLeft.Checked) return MouseCommand.MouseClickLeft;
      if (checkBoxMouseClickMiddle.Checked) return MouseCommand.MouseClickMiddle;
      if (checkBoxMouseClickRight.Checked) return MouseCommand.MouseClickRight;

      if (checkBoxMouseDoubleLeft.Checked) return MouseCommand.MouseDoubleClickLeft;
      if (checkBoxMouseDoubleMiddle.Checked) return MouseCommand.MouseDoubleClickMiddle;
      if (checkBoxMouseDoubleRight.Checked) return MouseCommand.MouseDoubleClickRight;

      if (checkBoxMouseScrollUp.Checked) return MouseCommand.MouseScrollUp;
      if (checkBoxMouseScrollDown.Checked) return MouseCommand.MouseScrollDown;

      if (checkBoxMouseMoveToPos.Checked) return MouseCommand.MouseMoveToPos;

      if (checkBoxMouseMoveUp.Checked) return MouseCommand.MouseMoveUp;
      if (checkBoxMouseMoveDown.Checked) return MouseCommand.MouseMoveDown;
      if (checkBoxMouseMoveLeft.Checked) return MouseCommand.MouseMoveLeft;
      if (checkBoxMouseMoveRight.Checked) return MouseCommand.MouseMoveRight;

      return "none";
    }

    public int UpdateCurrentPosition(int nCode, IntPtr wParam, IntPtr lParam)
    {
      //Marshall the data from the callback.
      MouseHookStruct MyMouseHookStruct = (MouseHookStruct) Marshal.PtrToStructure(lParam, typeof (MouseHookStruct));

      if (nCode < 0)
        return CallNextHookEx(hHook, nCode, wParam, lParam);

      //Create a string variable that shows the current mouse coordinates.
      labelCurrentX.Text = MyMouseHookStruct.pt.x.ToString("d");
      labelCurrentY.Text = MyMouseHookStruct.pt.y.ToString("d");

      return CallNextHookEx(hHook, nCode, wParam, lParam);
    }

    #endregion Implementation

    #region Win32

    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    //Declare the hook handle as an int.
    private static int hHook;

    //Declare the mouse hook constant.
    //For other hook types, you can obtain these values from Winuser.h in the Microsoft SDK.
    public const int WH_MOUSE = 7;

    //Declare MouseHookProcedure as a HookProc type.
    private HookProc MouseHookProcedure;

    //Declare the wrapper managed POINT class.
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
      public int x;
      public int y;
    }

    //Declare the wrapper managed MouseHookStruct class.
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
      public POINT pt;
      public int hwnd;
      public int wHitTestCode;
      public int dwExtraInfo;
    }

    //This is the Import for the SetWindowsHookEx function.
    //Use this function to install a thread-specific hook.
    [DllImport("user32.dll", CharSet = CharSet.Auto,
      CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
                                              IntPtr hInstance, int threadId);

    //This is the Import for the UnhookWindowsHookEx function.
    //Call this function to uninstall the hook.
    [DllImport("user32.dll", CharSet = CharSet.Auto,
      CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);

    //This is the Import for the CallNextHookEx function.
    //Use this function to pass the hook information to the next hook procedure in chain.
    [DllImport("user32.dll", CharSet = CharSet.Auto,
      CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode,
                                            IntPtr wParam, IntPtr lParam);

    #endregion Win32
  }
}