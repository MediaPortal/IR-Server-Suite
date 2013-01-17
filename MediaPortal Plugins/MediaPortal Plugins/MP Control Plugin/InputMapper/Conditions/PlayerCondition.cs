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
using MediaPortal.Player;

namespace MediaPortal.Input
{
  /// <summary>
  /// Condition to check for the current active player.
  /// </summary>
  public class PlayerCondition : Condition
  {
    #region Constants

    internal readonly static string[] PlayerTexts = new string[] { "TV is running", "DVD is playing", "Media is playing" };

    #endregion

    #region Enums

    internal enum PlayerType
    {
      TV = 0,
      DVD = 1,
      Media = 2
    }

    #endregion Enums

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerCondition"/> class.
    /// </summary>
    public PlayerCondition()
    {
      Property = Enum.GetName(typeof(PlayerType), PlayerType.Media);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerCondition"/> class.
    /// </summary>
    /// <param name="property">The condition property.</param>
    public PlayerCondition(string property)
      : base(property)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Player Condition"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseConditionConfig GetEditControl()
    {
      return new PlayerConditionConfig(Property);
    }

    /// <summary>
    /// Validate the condition.
    /// </summary>
    public override bool Validate()
    {
      PlayerType playerType = 
        (PlayerType) Enum.Parse(typeof (PlayerType), Property, true);

      switch (playerType)
      {
        case PlayerType.TV:
          return (g_Player.IsTimeShifting || g_Player.IsTV || g_Player.IsTVRecording);

        case PlayerType.DVD:
          return (g_Player.IsDVD);

        case PlayerType.Media:
          return (g_Player.Playing);
      }

      return false;
    }

    #endregion Implementation
  }
}