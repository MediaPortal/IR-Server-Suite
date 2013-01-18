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
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace IrssCommands.General
{
  /// <summary>
  /// HTTP Message command.
  /// </summary>
  public class HttpMessageCommand : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageCommand"/> class.
    /// </summary>
    public HttpMessageCommand()
    {
      InitParameters(4);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageCommand"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public HttpMessageCommand(string[] parameters)
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
      get { return "HTTP Message"; }
    }

    /// <summary>
    /// Gets the edit control to be used within a common edit form.
    /// </summary>
    /// <returns>The edit control.</returns>
    public override BaseCommandConfig GetEditControl()
    {
      return new HttpMessageConfig(Parameters);
    }

    /// <summary>
    /// Execute this command.
    /// </summary>
    /// <param name="variables">The variable list of the calling code.</param>
    public override void Execute(VariableList variables)
    {
      string[] processed = ProcessParameters(variables, Parameters);

      Uri uri = new Uri(processed[0]);
      int timeout = int.Parse(processed[1]);
      bool UseCredentials = !(string.IsNullOrEmpty(processed[2]) && string.IsNullOrEmpty(processed[3]));

      WebRequest request = WebRequest.Create(uri);
      request.Timeout = timeout;
      if (UseCredentials)
        request.Credentials = new NetworkCredential(processed[2], processed[3]);

#warning check if result needs to be publsihed (log or msg box)
      using (WebResponse response = request.GetResponse())
      using (Stream responseStream = response.GetResponseStream())
      using (StreamReader streamReader = new StreamReader(responseStream))
        streamReader.ReadToEnd();
    }

    #endregion Public Methods
  }
}