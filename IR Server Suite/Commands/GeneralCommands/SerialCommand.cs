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
using System.IO.Ports;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.General
{
  /// <summary>
  /// HTTP Message command.
  /// </summary>
  public class SerialCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommand"/> class.
    /// </summary>
    public SerialCommand()
    {
      InitParameters(7);

      // setting default values
      Parameters[2] = 9600.ToString();
      Parameters[3] = Enum.GetName(typeof (Parity), Parity.None);
      Parameters[4] = 8.ToString();
      Parameters[5] = Enum.GetName(typeof (StopBits), StopBits.One);
      Parameters[6] = false.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerialCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public SerialCommand(string[] parameters)
      : base(parameters)
    {
    }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <value>The category of this command.</value>
    public override string Category
    {
      get { return "General Commands"; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Serial Command"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new SerialConfig(Parameters);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      CommandConfigForm edit = new CommandConfigForm(this);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      // get parameters and error / exception handling
      byte[] message = Common.GetStringAsByte(processed[0]);
      string comPort = processed[1];
      int baudRate = int.Parse(processed[2]);
      Parity parity = (Parity) Enum.Parse(typeof (Parity), processed[3], true);
      int dataBits = int.Parse(processed[4]);
      StopBits stopBits = (StopBits) Enum.Parse(typeof (StopBits), processed[5], true);
      bool waitForResponse = bool.Parse(processed[6]);

      // do actual execute
      SerialPort serialPort = new SerialPort(comPort, baudRate, parity, dataBits, stopBits);
      serialPort.Open();

      try
      {
        serialPort.Write(message, 0, message.Length);

        if (waitForResponse)
        {
          try
          {
            serialPort.ReadTimeout = 5000;
            serialPort.ReadByte();
          }
          catch (Exception ex)
          {
            IrssLog.Debug("ProcessSerialCommand: {0}", ex.Message);
          }
        }
      }
      finally
      {
        serialPort.Close();
      }
      serialPort.Dispose();

#warning check if result needs to be publsihed (log or msg box)
    }

    #endregion Public Methods
  }
}