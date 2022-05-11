using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class CacheHelper
	{
		static string[] notCachedList = new string[]{
		};

		public static bool ShouldCache(string fullName)
		{
			if (notCachedList.Contains(fullName))
				return false;
			return true;
		}
	}
}
