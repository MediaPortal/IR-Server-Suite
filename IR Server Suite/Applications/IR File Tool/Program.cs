#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Threading;
using System.Windows.Forms;
using IrssUtils;

namespace IrFileTool
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
      IrssLog.LogLevel = IrssLog.Level.Debug;
#else
      IrssLog.LogLevel = IrssLog.Level.Info;
#endif
      IrssLog.Open("IR File Tool.log");

      Application.ThreadException += Application_ThreadException;

      FormMain main = new FormMain();

      Application.Run(main);

      Application.ThreadException -= Application_ThreadException;

      IrssLog.Close();
    }

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event args.</param>
    public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      IrssLog.Error(e.Exception);
    }
  }
}