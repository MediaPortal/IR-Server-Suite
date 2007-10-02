using System;

namespace IRServerPluginInterface
{

  /// <summary>
  /// Plugins that implement this interface can transmit IR commands.
  /// </summary>
  public interface ITransmitIR
  {

    /// <summary>
    /// Lists the available blaster ports.
    /// </summary>
    string[] AvailablePorts { get; }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="file">Infrared command file.</param>
    /// <returns>Success.</returns>
    bool Transmit(string port, byte[] data);

  }

}
