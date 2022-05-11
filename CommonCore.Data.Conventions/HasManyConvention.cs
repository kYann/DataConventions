using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class HasManyConvention : IHasManyConvention, IHasManyConventionAcceptance
	{
		public void Apply(FluentNHibernate.Conventions.Instances.IOneToManyCollectionInstance instance)
		{
			var childType = GetChildType(instance);
			if (DataConvention.TypeContainsPropertyOfType(childType, instance.EntityType))
				instance.Inverse();
			instance.Cascade.AllDeleteOrphan();
			instance.Fetch.Subselect();
			if (childType.GetProperty("CreationDate") != null)
				instance.OrderBy("creation_date");
			// TODO : Find a way to remove cache on AutoMappingOverride
			/*if (!CacheHelper.ShouldCache(instance.EntityType.FullName))
				return;
			instance.Cache.IncludeAll();
			instance.Cache.ReadWrite();*/
		}

		public void Accept(FluentNHibernate.Conventions.AcceptanceCriteria.IAcceptanceCriteria<FluentNHibernate.Conventions.Inspections.IOneToManyCollectionInspector> criteria)
		{
			criteria.Expect(c =>
				Check(c)
				);
		}

		protected bool Check(FluentNHibernate.Conventions.Inspections.IOneToManyCollectionInspector c)
		{
			return DataConvention.IsInSameAssembly(c.EntityType, GetChildType(c))
				&& !DataConvention.IsType(GetChildType(c));
		}

		protected Type GetChildType(FluentNHibernate.Conventions.Inspections.IOneToManyCollectionInspector c)
		{
			if (c.ChildType != null)
				return c.ChildType;
			return (c.Member as PropertyInfo).PropertyType.GetGenericArguments().First();
		}
	}
}
