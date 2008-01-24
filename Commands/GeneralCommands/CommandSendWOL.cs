using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

using IrssUtils;

namespace Commands.General
{

  /// <summary>
  /// Send WakeOnLan general command.
  /// </summary>
  public class CommandSendWOL : Command
  {

    #region Constants

    /// <summary>
    /// WakeOnLan packet header.
    /// </summary>
    static readonly byte[] Header = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

    #endregion Constants

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendWOL"/> class.
    /// </summary>
    public CommandSendWOL() { InitParameters(3); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSendWOL"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandSendWOL(string[] parameters) : base(parameters) { }

    #endregion Constructors

    #region Public Methods

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <returns>The category of this command.</returns>
    public override string GetCategory() { return "General Commands"; }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <returns>User interface text.</returns>
    public override string GetUserInterfaceText() { return "Send WakeOnLan"; }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <returns>The user display text.</returns>
    public override string GetUserDisplayText()
    {
      return String.Format("{0} ({1})", GetUserInterfaceText(), String.Join(", ", Parameters));
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string mac = Parameters[0];
      if (mac.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        mac = variables.VariableGet(mac);
      mac = Common.ReplaceSpecial(mac);

      string port = Parameters[1];
      if (port.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
        port = variables.VariableGet(port);
      port = Common.ReplaceSpecial(port);

      string password = Parameters[2];
      if (!String.IsNullOrEmpty(password))
      {
        if (password.StartsWith(VariableList.VariablePrefix, StringComparison.OrdinalIgnoreCase))
          password = variables.VariableGet(password);
        password = Common.ReplaceSpecial(password);
      }

      SendWOL(mac, int.Parse(port), password);
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditSendWOL edit = new EditSendWOL(Parameters);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters = edit.Parameters;
        return true;
      }

      return false;
    }

    #endregion Public Methods

    /// <summary>
    /// Send a Wake On LAN packet.
    /// </summary>
    /// <param name="mac">The mac address.</param>
    /// <param name="port">The destination port.</param>
    /// <param name="password">Optional password. Must be null, 4 or 6 bytes.</param>
    static void SendWOL(string mac, int port, string password)
    {
      // Convert mac address to byte[]
      string[] macParts = mac.Split(new char[] { ':', '-', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);
      if (macParts.Length != 6)
        throw new ArgumentException("Not a valid mac address", "mac");

      byte[] macAddress = new byte[macParts.Length];
      for (int index = 0; index < macAddress.Length; index++)
        macAddress[index] = byte.Parse(macParts[5 - index], System.Globalization.NumberStyles.HexNumber);

      // Convert password to byte[]
      byte[] pass = null;
      if (!String.IsNullOrEmpty(password))
        pass = Encoding.ASCII.GetBytes(password);
      
      if (pass != null && pass.Length != 4 && pass.Length != 6)
        throw new ArgumentException("Not a valid password (must be null, 4 or 6 bytes)", "password");

      int packetLength = Header.Length + (16 * macAddress.Length);
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
