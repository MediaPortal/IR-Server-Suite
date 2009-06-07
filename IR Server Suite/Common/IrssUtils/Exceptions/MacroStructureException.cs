using System;
using System.Runtime.Serialization;

namespace IrssUtils.Exceptions
{
  /// <summary>
  /// The exception that is thrown when a structural error in a macro is discovered.
  /// </summary>
  [Serializable]
  public class MacroStructureException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MacroStructureException"/> class.
    /// </summary>
    public MacroStructureException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacroStructureException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public MacroStructureException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacroStructureException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public MacroStructureException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacroStructureException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected MacroStructureException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}