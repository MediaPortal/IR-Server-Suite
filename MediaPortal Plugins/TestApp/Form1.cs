using System;
using System.Windows.Forms;
using MediaPortal.Plugins.IRSS.MPBlastZonePlugin;
using MediaPortal.Plugins.IRSS.MPControlPlugin;

namespace TestApp
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void buttonControl_Click(object sender, EventArgs e)
    {
      MPControlPlugin plugin = new MPControlPlugin();
      plugin.ShowPlugin();
    }

    private void buttonBlastZone_Click(object sender, EventArgs e)
    {
      MPBlastZonePlugin plugin = new MPBlastZonePlugin();
      plugin.ShowPlugin();
    }
  }
}
