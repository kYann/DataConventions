using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class ModelReferenceConvention : IReferenceConvention
	{
		public void Apply(IManyToOneInstance instance)
		{
			var concreteType = DataConvention
				.ResolveConcreteTypeFromInterface(instance.Property.PropertyType);
			instance.CustomClass(concreteType);
			if (!DataConvention.IsNullable(instance.Property.MemberInfo))
				instance.Not.Nullable();
			if (DataConvention.IsType(instance.Property.MemberInfo)
				|| DataConvention.IsAggregateRoot(instance.Property.MemberInfo))
				instance.Cascade.None();
			else
				instance.Cascade.All();
			if (DataConvention.IsBaseTypeOfMappedEntity(concreteType))
				instance.Not.LazyLoad();
		}
	}
}
