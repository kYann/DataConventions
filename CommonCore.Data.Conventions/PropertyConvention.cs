using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class PropertyConvention : IPropertyConvention
	{
		public void Apply(FluentNHibernate.Conventions.Instances.IPropertyInstance instance)
		{
			var columnName = DataConvention.GetColumnNameFromPropertyName(instance.Name);
			instance.Column(columnName);
			if (!DataConvention.IsNullable(instance.Property.MemberInfo))
				instance.Not.Nullable();
			int? len = DataConvention.StringLength(instance.Property.MemberInfo);
			if (len.HasValue)
			{
				instance.Length(len.Value);
			}
			Type type = instance.Property.PropertyType;
			if (type == typeof(DateTime) || type == typeof(DateTime?))
				instance.CustomType("Timestamp");
		}
	}
}
