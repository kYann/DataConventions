using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class HasManyToManyConvention : IHasManyToManyConvention, IHasManyToManyConventionAcceptance
	{
		public void Apply(IManyToManyCollectionInstance instance)
		{
			Type entityType = instance.EntityType;

			instance.Schema(DataConvention.GetSchemaFromType(entityType));
			instance.Fetch.Subselect();
		}

		public void Accept(FluentNHibernate.Conventions.AcceptanceCriteria.IAcceptanceCriteria<FluentNHibernate.Conventions.Inspections.IManyToManyCollectionInspector> criteria)
		{
			criteria.Expect(c =>
				Check(c));
		}

		public bool Check(FluentNHibernate.Conventions.Inspections.IManyToManyCollectionInspector c)
		{
			return /*c.HasExplicitTable ||*/
				(!DataConvention.IsInSameAssembly(c.EntityType, c.ChildType)
				|| DataConvention.IsType(c.ChildType));
		}
	}
}
