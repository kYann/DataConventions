using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public static class StringExtensions
	{
		public static string CamelCaseToCConvention(this string input)
		{
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			var str = new StringBuilder();
			bool prevCharUpper = true;


			for (int i = 0; i < input.Length; i++)
			{
				if (Char.IsUpper(input[i]))
				{
					if (i > 0 && !prevCharUpper)
						str.Append("_");
					prevCharUpper = true;
				}
				else
					prevCharUpper = false;
				str.Append(Char.ToLower(input[i]));
			}

			return str.ToString();
		}
	}
}
