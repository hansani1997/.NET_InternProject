using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Extensions
{
	public static class NavigationManagerExtension
	{
		public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
		{
			var uri = navManager.ToAbsoluteUri(navManager.Uri);//convert relative uri to  the absolute URI.

			// QueryHelpers - Provides methods for parsing and manipulating query strings.
			// uri.Query - Gets any query information included in the specified URI.
			// QueryHelpers.ParseQuery(uri.Query) - Parse a query string into its component key and value parts.
			// 
			if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))//It allows you to store the value found in the Dictionary after a lookup. This eliminates unnecessary lookups that might occur after ContainsKey returns true.
			{
				if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
				{
					value = (T)(object)valueAsInt;
					return true;
				}
				if (typeof(T) == typeof(long) && long.TryParse(valueFromQueryString, out var valueAsLong))
				{
					value = (T)(object)valueAsLong;
					return true;
				}

				if (typeof(T) == typeof(string))
				{
					value = (T)(object)valueFromQueryString.ToString();
					return true;
				}

				if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
				{
					value = (T)(object)valueAsDecimal;
					return true;
				}
			}

			value = default;
			return false;
		}
	}

    
}
