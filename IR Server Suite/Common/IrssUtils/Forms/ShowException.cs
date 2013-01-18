using System;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  public partial class ShowException : Form
  {
    protected Exception _exception;

    protected ShowException()
    {
      InitializeComponent();
    }

    public ShowException(Exception exception) : this()
    {
      if (!ReferenceEquals(Parent, null))
        Icon = Parent.FindForm().Icon;

      _exception = exception;

      textBox1.Text = GenerateExceptionText(_exception);
    }

    private static string GenerateExceptionText(Exception exception)
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("---=== Unhandled Exception ===---");
      sb.AppendLine("---------------------------------");
      sb.AppendLine("");
      sb.AppendLine("----- Message -----");
      sb.AppendLine(exception.Message);
      sb.AppendLine("");
      sb.AppendLine("----- Source -----");
      sb.AppendLine(exception.Source);
      sb.AppendLine("");
      sb.AppendLine("----- InnerException -----");
      sb.AppendLine(exception.InnerException.ToString());
      sb.AppendLine("");
      sb.AppendLine("----- StackTrace -----");
      sb.AppendLine(exception.StackTrace);
      sb.AppendLine("");
      sb.AppendLine("----- Data -----");
      sb.AppendLine(exception.Data.ToString());
      sb.AppendLine("");

      return sb.ToString();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
