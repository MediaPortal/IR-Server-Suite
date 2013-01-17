using System.Windows.Forms;

namespace MediaPortal.Input
{
  public partial class BaseConditionConfig : UserControl
  {
    private string _property;

    public delegate void PropertyChangedDelegate();
    public event PropertyChangedDelegate OnPropertyChanged;

    public BaseConditionConfig()
    {
      InitializeComponent();
    }

    public string Property
    {
      get { return _property; }
      protected set
      {
        _property = value;

        if (OnPropertyChanged != null)
          OnPropertyChanged();
      }
    }
  }
}
