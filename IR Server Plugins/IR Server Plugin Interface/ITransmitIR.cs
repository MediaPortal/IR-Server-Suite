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
    /// <value>The available ports.</value>
    string[] AvailablePorts { get; }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns>true if successful, otherwise false.</returns>
    bool Transmit(string port, byte[] data);

  }

}
