using System.IO;
using System.Windows.Forms;

namespace IrssCommands
{
  public partial class CallMacroConfig : BaseCommandConfig
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public override string[] Parameters
    {
      get
      {
        if (listView.SelectedItems.Count != 1)
          return null;

        return new[]
          {
            listView.SelectedItems[0].Tag as string
          };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CallMacroConfig"/> class.
    /// </summary>
    private CallMacroConfig()
    {
      InitializeComponent();
      UpdateMacroList();

      if (listView.Items.Count > 0)
        listView.Items[0].Selected = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CallMacroConfig"/> class.
    /// </summary>
    /// <param name="parameters">The command elements.</param>
    public CallMacroConfig(string[] parameters)
      : this()
    {
      if (ReferenceEquals(parameters,null)) return;
      if (ReferenceEquals(parameters, new string[] {})) return;

      string macroName = Path.GetFileNameWithoutExtension(parameters[0]);
      foreach (ListViewItem item in listView.Items)
      {
        if (item.Text.Equals(macroName))
          item.Selected = true;
        break;
      }
    }

    #endregion Constructors

    public void UpdateMacroList()
    {
      listView.Items.Clear();

      string[] files = Processor.GetListMacro(Processor.MacroFolder);

      foreach (string file in files)
      {
        ListViewItem item = new ListViewItem();
        item.Text = Path.GetFileNameWithoutExtension(file);
        item.Tag = file;

        listView.Items.Add(item);
      }
    }
  }
}