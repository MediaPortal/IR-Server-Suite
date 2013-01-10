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
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace IrssCommands.General
{
  /// <summary>
  /// HTTP Message command.
  /// </summary>
  public class SendWOLCommand : Command
  {
    #region Constants

    /// <summary>
    /// WakeOnLan packet header.
    /// </summary>
    private static readonly byte[] Header = new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF};

    #endregion Constants

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SendWOLCommand"/> class.
    /// </summary>
    public SendWOLCommand()
    {
      InitParameters(3);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendWOLCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public SendWOLCommand(string[] parameters)
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
      get { return "Send WakeOnLan"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new SendWOLConfig(Parameters);
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

      int port = int.Parse(processed[1]);

      SendWOL(processed[0], port, processed[2]);
    }

    #endregion Public Methods

    /// <summary>
    /// Send a Wake On LAN packet.
    /// </summary>
    /// <param name="mac">The mac address.</param>
    /// <param name="port">The destination port.</param>
    /// <param name="password">Optional password. Must be null, 4 or 6 bytes.</param>
    private static void SendWOL(string mac, int port, string password)
    {
      // Convert mac address to byte[]
      string[] macParts = mac.Split(new[] {':', '-', '.', ' '}, StringSplitOptions.RemoveEmptyEntries);
      if (macParts.Length != 6)
        throw new ArgumentException("Not a valid mac address", "mac");

      byte[] macAddress = new byte[macParts.Length];
      for (int index = 0; index < macAddress.Length; index++)
        macAddress[index] = byte.Parse(macParts[5 - index], NumberStyles.HexNumber);

      // Convert password to byte[]
      byte[] pass = null;
      if (!String.IsNullOrEmpty(password))
        pass = Encoding.ASCII.GetBytes(password);

      if (pass != null && pass.Length != 4 && pass.Length != 6)
        throw new ArgumentException("Not a valid password (must be null, 4 or 6 bytes)", "password");

      int packetLength = Header.Length + (16*macAddress.Length);
      if (pass != null)
        packetLength += pass.Length;

      byte[] packet = new byte[packetLength];

      // Packet Header ...
      Buffer.BlockCopy(Header, 0, packet, 0, Header.Length);

      // MAC repeated 16 times ...
      int offset = Header.Length;
      for (int i = 0; i < 16; i++)
      {
        Buffer.BlockCopy(macAddress, 0, packet, offset, macAddress.Length);
        offset += macAddress.Length;
      }

      // Add password (optional) ...
      if (pass != null)
        Buffer.BlockCopy(pass, 0, packet, offset, pass.Length);

      // Create connection ...
      UdpClient client = new UdpClient();
      client.Connect(IPAddress.Broadcast, port);

      // Send ...
      client.Send(packet, packet.Length);
    }
  }
}