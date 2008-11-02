using System;
using System.Runtime.Serialization;

namespace IrssUtils.Exceptions
{
  /// <summary>
  /// The exception that is thrown when a structural error in a command is discovered.
  /// </summary>
  [Serializable]
  public class CommandStructureException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStructureException"/> class.
    /// </summary>
    public CommandStructureException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStructureException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public CommandStructureException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStructureException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public CommandStructureException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStructureException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected CommandStructureException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}