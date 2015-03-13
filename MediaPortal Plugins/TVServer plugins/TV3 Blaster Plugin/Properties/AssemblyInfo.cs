#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Reflection;
using System.Resources;
using MediaPortal.Common.Utils;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//

[assembly: AssemblyTitle("TV3 Blaster Plugin")]
[assembly: AssemblyDescription("External Channel Changer for TV Engine 3 using IR Server")]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en")]

[assembly: CompatibleVersion("9.9.9.9", "1.1.6.27644")]
[assembly: UsesSubsystem("TVE.DB")]
[assembly: UsesSubsystem("TVE.Controller")]
[assembly: UsesSubsystem("TVE.Config")]

