using System;
using System.Runtime.Serialization;

namespace IrssCommands
{
  /// <summary>
  /// The exception that is thrown when a macro execution error occurs.
  /// </summary>
  [Serializable]
  public class MacroExecutionException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MacroExecutionException"/> class.
    /// </summary>
    public MacroExecutionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacroExecutionException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public MacroExecutionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacroExecutionException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public MacroExecutionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacroExecutionException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected MacroExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}