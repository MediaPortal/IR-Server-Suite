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

namespace IrssComms
{
  /// <summary>
  /// Encapsulates an IrssMessage and a ClientManager object instance for queueing.
  /// </summary>
  public class MessageManagerCombo : IEquatable<MessageManagerCombo>
  {
    #region Variables

    private ClientManager _manager;
    private IrssMessage _message;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Create a new MessageManagerCombo structure instance.
    /// </summary>
    /// <param name="message">The IrssMessage to encapsulate.</param>
    /// <param name="manager">The ClientManager to encapsulate.</param>
    public MessageManagerCombo(IrssMessage message, ClientManager manager)
    {
      _message = message;
      _manager = manager;
    }

    #endregion Constructor

    #region Properties

    /// <summary>
    /// Gets or Sets the encapsulated IrssMessage object.
    /// </summary>
    public IrssMessage Message
    {
      get { return _message; }
      set { _message = value; }
    }

    /// <summary>
    /// Gets or Sets the encapsulated ClientManager object.
    /// </summary>
    public ClientManager Manager
    {
      get { return _manager; }
      set { _manager = value; }
    }

    #endregion Properties

    #region IEquatable<MessageManagerCombo> Members

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
    public bool Equals(MessageManagerCombo other)
    {
      return (Message == other.Message && Manager == other.Manager);
    }

    #endregion

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
      MessageManagerCombo asCombo = obj as MessageManagerCombo;

      if (asCombo == null)
        return false;

      return Equals(asCombo);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj1">First object to compare.</param>
    /// <param name="obj2">Second object to compare.</param>
    /// <returns>
    /// true if the current object is equal to the other parameter; otherwise, false.
    /// </returns>
    public static bool operator ==(MessageManagerCombo obj1, MessageManagerCombo obj2)
    {
      return obj1.Equals(obj2);
    }

    /// <summary>
    /// Indicates whether the current object is not equal to another object of the same type.
    /// </summary>
    /// <param name="obj1">First object to compare.</param>
    /// <param name="obj2">Second object to compare.</param>
    /// <returns>
    /// true if the current object is not equal to the other parameter; otherwise, false.
    /// </returns>
    public static bool operator !=(MessageManagerCombo obj1, MessageManagerCombo obj2)
    {
      return !obj1.Equals(obj2);
    }

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
      return _message.GetHashCode() + _manager.GetHashCode();
    }
  }
}