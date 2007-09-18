using System;

namespace IrssComms
{

  /// <summary>
  /// Encapsulates an IrssMessage and a ClientManager object instance for queueing.
  /// </summary>
  public class MessageManagerCombo
  {

    #region Variables

    IrssMessage _message;
    ClientManager _manager;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Create a new MessageManagerCombo class instance.
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

  }

}
