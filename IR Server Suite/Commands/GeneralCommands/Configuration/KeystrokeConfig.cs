using System;
using System.Diagnostics;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.General
{
  public partial class KeystrokeConfig : BaseCommandConfig
  {
    private const string HELP_URL =
      "http://wiki.team-mediaportal.com/3_IRSS_(IR_Server_Suite)/Reference/Keystrokes_Syntax";

    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get
      {
        return new[]
          {
            textBoxKeys.Text
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="KeystrokeConfig"/> class.
    /// </summary>
    private KeystrokeConfig()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeystrokeConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public KeystrokeConfig(string[] parameters) : this()
    {
      if (ReferenceEquals(parameters, null)) return;
      if (parameters.Length == 0) return;
      textBoxKeys.Text = parameters[0];
    }

    #endregion Constructors

    #region Implementation

    private void InsertKeystroke(char key)
    {
      textBoxKeys.Paste(key.ToString());
    }

    private void InsertKeystroke(string keystroke)
    {
      textBoxKeys.Paste(keystroke);
    }

    private void KeystrokeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem origin = sender as ToolStripMenuItem;

      if (origin == null)
        return;

      switch (origin.Name)
      {
        case "upToolStripMenuItem":
          InsertKeystroke("{UP}");
          break;
        case "downToolStripMenuItem":
          InsertKeystroke("{DOWN}");
          break;
        case "leftToolStripMenuItem":
          InsertKeystroke("{LEFT}");
          break;
        case "rightToolStripMenuItem":
          InsertKeystroke("{RIGHT}");
          break;

        case "f1ToolStripMenuItem":
          InsertKeystroke("{F1}");
          break;
        case "f2ToolStripMenuItem":
          InsertKeystroke("{F2}");
          break;
        case "f3ToolStripMenuItem":
          InsertKeystroke("{F3}");
          break;
        case "f4ToolStripMenuItem":
          InsertKeystroke("{F4}");
          break;
        case "f5ToolStripMenuItem":
          InsertKeystroke("{F5}");
          break;
        case "f6ToolStripMenuItem":
          InsertKeystroke("{F6}");
          break;
        case "f7ToolStripMenuItem":
          InsertKeystroke("{F7}");
          break;
        case "f8ToolStripMenuItem":
          InsertKeystroke("{F8}");
          break;
        case "f9ToolStripMenuItem":
          InsertKeystroke("{F9}");
          break;
        case "f10ToolStripMenuItem":
          InsertKeystroke("{F10}");
          break;
        case "f11ToolStripMenuItem":
          InsertKeystroke("{F11}");
          break;
        case "f12ToolStripMenuItem":
          InsertKeystroke("{F12}");
          break;
        case "f13ToolStripMenuItem":
          InsertKeystroke("{F13}");
          break;
        case "f14ToolStripMenuItem":
          InsertKeystroke("{F14}");
          break;
        case "f15ToolStripMenuItem":
          InsertKeystroke("{F15}");
          break;
        case "f16ToolStripMenuItem":
          InsertKeystroke("{F16}");
          break;

        case "addToolStripMenuItem":
          InsertKeystroke("{ADD}");
          break;
        case "subtractToolStripMenuItem":
          InsertKeystroke("{SUBTRACT}");
          break;
        case "multiplyToolStripMenuItem":
          InsertKeystroke("{MULTIPLY}");
          break;
        case "divideToolStripMenuItem":
          InsertKeystroke("{DIVIDE}");
          break;

        case "altToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierAlt);
          break;
        case "controlToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierControl);
          break;
        case "shiftToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierShift);
          break;
        case "windowsToolStripMenuItem":
          InsertKeystroke(Keyboard.ModifierWinKey);
          break;

        case "mediaPlayPauseToolStripMenuItem":
          InsertKeystroke("{MEDIA_PLAY_PAUSE}");
          break;
        case "mediaStopToolStripMenuItem":
          InsertKeystroke("{MEDIA_STOP}");
          break;
        case "mediaNextToolStripMenuItem":
          InsertKeystroke("{MEDIA_NEXT_TRACK}");
          break;
        case "mediaPreviousToolStripMenuItem":
          InsertKeystroke("{MEDIA_PREV_TRACK}");
          break;
        case "volumeUpToolStripMenuItem":
          InsertKeystroke("{VOLUME_UP}");
          break;
        case "volumeDownToolStripMenuItem":
          InsertKeystroke("{VOLUME_DOWN}");
          break;
        case "volumeMuteToolStripMenuItem":
          InsertKeystroke("{VOLUME_MUTE}");
          break;

        case "backspaceToolStripMenuItem":
          InsertKeystroke("{BACKSPACE}");
          break;
        case "breakToolStripMenuItem":
          InsertKeystroke("{BREAK}");
          break;
        case "capsLockToolStripMenuItem":
          InsertKeystroke("{CAPSLOCK}");
          break;
        case "delToolStripMenuItem":
          InsertKeystroke("{DEL}");
          break;

        case "endToolStripMenuItem":
          InsertKeystroke("{END}");
          break;
        case "enterToolStripMenuItem":
          InsertKeystroke("{ENTER}");
          break;
        case "escapeToolStripMenuItem":
          InsertKeystroke("{ESC}");
          break;
        case "helpToolStripMenuItem":
          InsertKeystroke("{HELP}");
          break;
        case "homeToolStripMenuItem":
          InsertKeystroke("{HOME}");
          break;
        case "insToolStripMenuItem":
          InsertKeystroke("{INS}");
          break;
        case "numLockToolStripMenuItem":
          InsertKeystroke("{NUMLOCK}");
          break;
        case "pageDownToolStripMenuItem":
          InsertKeystroke("{PGDN}");
          break;
        case "pageUpToolStripMenuItem":
          InsertKeystroke("{PGUP}");
          break;
        case "scrollLockToolStripMenuItem":
          InsertKeystroke("{SCROLLLOCK}");
          break;
        case "tabToolStripMenuItem":
          InsertKeystroke("{TAB}");
          break;
        case "windowsKeyToolStripMenuItem":
          InsertKeystroke("{WIN}");
          break;
      }
    }

    private void cutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.Cut();
    }

    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.Copy();
    }

    private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.Paste();
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.SelectAll();
    }

    private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      textBoxKeys.SelectionLength = 0;
    }

    #endregion Implementation
  }
}