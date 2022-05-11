using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class PrimaryKeyConvention : IIdConvention
	{
		public void Apply(IIdentityInstance instance)
		{
			if (instance.Type == typeof(Guid))
				instance.GeneratedBy.Assigned();
		}
	}
}
