using System.Windows.Forms;

namespace Commands.General
{
  /// <summary>
  /// StandBy macro command.
  /// </summary>
  public class CommandStandBy : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStandBy"/> class.
    /// </summary>
    public CommandStandBy()
    {
      InitParameters(0);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandStandBy"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandStandBy(string[] parameters) : base(parameters)
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
      return "StandBy";
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      if (!Application.SetSuspendState(PowerState.Suspend, false, false))
        throw new CommandExecutionException("StandBy command refused");
    }

    #endregion Implementation
  }
}