using System.Windows.Forms;

namespace Commands.General
{
  /// <summary>
  /// Hibernate macro command.
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
      return "General Commands";
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
      if (!Application.SetSuspendState(PowerState.Hibernate, false, false))
        throw new CommandExecutionException("Hibernate command refused");
    }

    #endregion Implementation
  }
}