using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public enum CacheMode
	{
		None,
		All,
		OnlyTypes
	}

	public class TableConvention : IClassConvention
	{
		public static CacheMode CacheMode
		{
			get;
			set;
		}

		public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
		{

			string schema = DataConvention.GetSchemaFromType(instance.EntityType);

			instance.Schema(schema);

			instance.Table(DataConvention.GetTableFromType(instance.EntityType));
			// This allow nhibernate to update only fields that changed
			instance.DynamicUpdate();
			// This allow nhibernate to check concurrency on changed fields
			//instance.OptimisticLock.Dirty();

			// TODO : Find a way to remove cache on AutoMappingOverride
			/*if (!CacheHelper.ShouldCache(instance.EntityType.FullName))
				return;*/
			if (CacheMode != CacheMode.None)
			{
				// Non strict cache is used for entity that as few updates
				if (DataConvention.IsType(instance.EntityType))
				{
					instance.Cache.IncludeAll();
					instance.Cache.NonStrictReadWrite();
				}
				else if (CacheMode == CacheMode.All)
					instance.Cache.ReadWrite();
			}
		}
	}
}
