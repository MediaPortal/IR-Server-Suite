using System;
using System.Windows.Forms;

namespace VirtualRemote
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [MTAThread]
    private static void Main()
    {
      Application.Run(new FormMain());
    }
  }
}