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