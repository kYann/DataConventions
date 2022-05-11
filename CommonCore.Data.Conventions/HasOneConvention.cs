using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class HasOneConvention : IHasOneConvention
	{
		public void Apply(FluentNHibernate.Conventions.Instances.IOneToOneInstance instance)
		{
			var type = (instance as FluentNHibernate.Conventions.Inspections.OneToOneInspector)
				.Class.GetUnderlyingSystemType();
			instance.Class(DataConvention.ResolveConcreteTypeFromInterface(type));
		}
	}
}
