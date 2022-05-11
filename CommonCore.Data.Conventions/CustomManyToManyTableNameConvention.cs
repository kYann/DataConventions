using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class CustomManyToManyTableNameConvention : ManyToManyTableNameConvention
	{
		private string GetNameFromType(Type type)
		{
			string entityName = type.Name;
			if (type.IsInterface && entityName[0] == 'I')
				entityName = entityName.Substring(1);
			return entityName;
		}

		private string GetChildName(string childName, string entityName)
		{
			return childName.Replace(entityName, "").Trim('_').CamelCaseToCConvention();
		}

		protected override string GetBiDirectionalTableName(FluentNHibernate.Conventions.Inspections.IManyToManyCollectionInspector collection, FluentNHibernate.Conventions.Inspections.IManyToManyCollectionInspector otherSide)
		{
			string entityName = GetNameFromType(collection.EntityType);
			string otherName = GetNameFromType(otherSide.EntityType);

			return entityName.CamelCaseToCConvention() + "_has_" + GetChildName(otherName, entityName);
		}

		protected override string GetUniDirectionalTableName(FluentNHibernate.Conventions.Inspections.IManyToManyCollectionInspector collection)
		{
			string entityName = GetNameFromType(collection.EntityType);
			string otherName = GetNameFromType(collection.ChildType);

			var propType = (collection.Member as PropertyInfo).PropertyType;
			var isNotUnique = collection.EntityType
				.GetProperties().Any(_ => _.PropertyType == propType && _.Name != collection.Member.Name);

			if (collection.Inverse)
			{
				var tmp = entityName;
				entityName = otherName;
				otherName = tmp;
			}

			var finalOtherName = GetChildName(otherName, entityName);
			if (isNotUnique)
				finalOtherName = $"{collection.Member.Name.CamelCaseToCConvention()}_{finalOtherName}";
			return $"{entityName.CamelCaseToCConvention()}_has_{finalOtherName}";
		}
	}
}
