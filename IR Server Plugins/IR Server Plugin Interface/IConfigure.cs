using System;

namespace IRServerPluginInterface
{

  /// <summary>
  /// Plugins that implement this interface are configurable.
  /// </summary>
  public interface IConfigure
  {

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    void Configure();

  }

}
