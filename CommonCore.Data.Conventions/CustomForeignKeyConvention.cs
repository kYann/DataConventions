using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class CustomForeignKeyConvention : ForeignKeyConvention
	{
		protected override string GetKeyName(FluentNHibernate.Member property, Type type)
		{
			if (property == null)
				return DataConvention.GetColumnNameFromPropertyName(type.Name) + "_id";
			if (property.Name == "Children")
				return "parent_id";

			return DataConvention.GetColumnNameFromPropertyName(property.Name) + "_id";
		}
	}
}
