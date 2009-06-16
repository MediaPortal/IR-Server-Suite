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

using MediaPortal.GUI.Library;
using MediaPortal.Player;
using MediaPortal.Profile;
using MediaPortal.Util;

namespace Commands.MediaPortal
{
  /// <summary>
  /// Hibernate MediaPortal macro command.
  /// </summary>
  public class CommandHibernate : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHibernate"/> class.
    /// </summary>
    public CommandHibernate()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHibernate"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandHibernate(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory()
    {
      return "MediaPortal Commands";
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText()
    {
      return "Hibernate";
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      bool mpBasicHome = false;
      using (Settings xmlreader = new Settings(Common.MPConfigFile))
        mpBasicHome = xmlreader.GetValueAsBool("general", "startbasichome", false);

      GUIGraphicsContext.ResetLastActivity();
      // Stop all media before hibernating
      g_Player.Stop();

      GUIMessage msg;

      if (mpBasicHome)
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0,
                             (int) GUIWindow.Window.WINDOW_SECOND_HOME, 0, null);
      else
        msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GOTO_WINDOW, 0, 0, 0, (int) GUIWindow.Window.WINDOW_HOME, 0,
                             null);

      GUIWindowManager.SendThreadMessage(msg);

      WindowsController.ExitWindows(RestartOptions.Hibernate, false);
    }

    #endregion Implementation
  }
}