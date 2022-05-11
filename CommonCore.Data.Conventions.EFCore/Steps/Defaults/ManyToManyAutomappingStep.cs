using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using CommonCore.SharedKernel.Helpers;

namespace CommonCore.Data.Conventions.EFCore.Steps.Defaults
{
	public class ManyToManyAutomappingStep : IPropertyAutomappingStep
	{
		private static Type GetChildType(Type propertyType)
		{
			var argument = propertyType.GetGenericArguments()[0];
			return argument;
		}

		private static PropertyInfo? GetInverseProperty(Type declaringType, Type propertyType)
		{
			var type = propertyType;
			var expectedInversePropertyType = type.GetGenericTypeDefinition()
				.MakeGenericType(declaringType);

			var argument = type.GetGenericArguments()[0];
			return argument.GetProperties()
				.Where(x => x.PropertyType == expectedInversePropertyType && x != propertyType)
				.FirstOrDefault();
		}

		private static bool HasInverseSingleProperty(Type declaringType, Type propertyType) => propertyType.GetProperties()
				.Any(x => x.PropertyType.IsAssignableFrom(declaringType));

		public void Map(EntityTypeBuilder entityTypeBuilder, Type entityType, PropertyInfo propertyInfo)
		{
			var inverseProperty = GetInverseProperty(entityType, propertyInfo.PropertyType);
			entityTypeBuilder.HasMany(propertyInfo.Name)
				.WithMany(inverseProperty?.Name ?? entityType.Name);
		}

		private bool ShouldMapBecauseNotFromSameAssembly(Type entityType, PropertyInfo propertyInfo, Type childType, bool hasInverse)
        {
			var diffAssembly = !DataConventionHelpers.IsInSameAssembly(entityType, childType);
			if (diffAssembly == false)
				return diffAssembly;
			if (hasInverse)
				return diffAssembly;
			var hasInverseSingleProperty = HasInverseSingleProperty(entityType, propertyInfo.PropertyType.GetGenericArguments()[0]);
			if (hasInverseSingleProperty)
				return false;
			return diffAssembly;
		}

		public virtual bool ShouldMap(Type entityType, PropertyInfo propertyInfo)
		{
			var type = propertyInfo.PropertyType;
			if (type.Namespace != "System.Collections.Generic")
				return false;

			if (typeof(IDictionary).IsAssignableFrom(entityType))
				return false;

			var hasInverse = GetInverseProperty(entityType, propertyInfo.PropertyType) != null;
			var childType = GetChildType(propertyInfo.PropertyType);
			var isType = childType.IsAggregateRoot();
			var diffAssembly = ShouldMapBecauseNotFromSameAssembly(entityType, propertyInfo, childType, hasInverse);
			if (entityType == childType && propertyInfo.Name == "Children")
				return false;
			if (childType.IsInterface)
				return false;
			if (childType.IsValueType || childType == typeof(string))
				return false;
			return hasInverse || isType || diffAssembly;
		}
	}
}
