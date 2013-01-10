using System.Windows.Forms;

namespace IrssCommands
{
  public partial class BaseCommandConfig : UserControl
  {
    public BaseCommandConfig()
    {
      InitializeComponent();
    }

    public virtual string[] Parameters
    {
      get { return null; }
    }
  }
}
