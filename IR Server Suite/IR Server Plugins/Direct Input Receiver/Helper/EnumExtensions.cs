#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
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
using System.Collections.Generic;
using System.Linq;

namespace IRServer.Plugin
{
  internal static class EnumExtensions
  {
    public static IEnumerable<Enum> GetFlags(this Enum value)
    {
      return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
    }

    public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
    {
      return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
    }

    private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
    {
      ulong bits = Convert.ToUInt64(value);
      List<Enum> results = new List<Enum>();
      for (int i = values.Length - 1; i >= 0; i--)
      {
        ulong mask = Convert.ToUInt64(values[i]);
        if (i == 0 && mask == 0L)
          break;
        if ((bits & mask) == mask)
        {
          results.Add(values[i]);
          bits -= mask;
        }
      }
      if (bits != 0L)
        return Enumerable.Empty<Enum>();
      if (Convert.ToUInt64(value) != 0L)
        return results.Reverse<Enum>();
      if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
        return values.Take(1);
      return Enumerable.Empty<Enum>();
    }

    private static IEnumerable<Enum> GetFlagValues(Type enumType)
    {
      ulong flag = 0x1;
      foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
      {
        ulong bits = Convert.ToUInt64(value);
        if (bits == 0L)
          //yield return value;
          continue; // skip the zero value
        while (flag < bits) flag <<= 1;
        if (flag == bits)
          yield return value;
      }
    }
  }
}