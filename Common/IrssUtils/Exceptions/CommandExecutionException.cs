using System;
using System.Runtime.Serialization;

namespace IrssUtils.Exceptions
{

  /// <summary>
  /// The exception that is thrown when an error executing a command occurs.
  /// </summary>
  [Serializable]
  public class CommandExecutionException : Exception
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
    /// </summary>
    public CommandExecutionException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public CommandExecutionException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public CommandExecutionException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutionException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected CommandExecutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

  }

}
