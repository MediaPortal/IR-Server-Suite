namespace InputService.Plugin
{

  #region Enumerations

  /// <summary>
  /// Provides information about the status of learning an infrared command.
  /// </summary>
  public enum LearnStatus
  {
    /// <summary>
    /// Failed to learn infrared command.
    /// </summary>
    Failure,
    /// <summary>
    /// Succeeded in learning infrared command.
    /// </summary>
    Success,
    /// <summary>
    /// Infrared command learning timed out.
    /// </summary>
    Timeout,
  }

  #endregion Enumerations

  /// <summary>
  /// Plugins that implement this interface can learn IR commands.
  /// </summary>
  public interface ILearnIR
  {
    /// <summary>
    /// Learn an infrared command.
    /// </summary>
    /// <param name="data">New infrared command.</param>
    /// <returns>Tells the calling code if the learn was Successful, Failed or Timed Out.</returns>
    LearnStatus Learn(out byte[] data);
  }
}