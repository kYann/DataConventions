using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class JoinedSubClassConvention : IJoinedSubclassConvention
	{
		public void Apply(FluentNHibernate.Conventions.Instances.IJoinedSubclassInstance instance)
		{
			string schema = DataConvention.GetSchemaFromType(instance.EntityType);

			instance.Schema(schema);

			instance.Table(DataConvention.GetTableFromType(instance.EntityType));
		}
	}
}
