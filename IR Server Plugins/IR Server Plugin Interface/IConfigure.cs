using System;
using System.Windows.Forms;

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
    /// <param name="owner">The owner window to use for creating modal dialogs.</param>
    void Configure(IWin32Window owner);

  }

}
