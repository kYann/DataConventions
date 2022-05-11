using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommonCore.Data.Conventions.EFCore.Steps
{
	public interface IPropertyAutomappingStep
	{
		void Map(EntityTypeBuilder entityTypeBuilder, Type entityType, PropertyInfo propertyInfo);

		bool ShouldMap(Type entityType, PropertyInfo propertyInfo);
	}
}
