using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommonCore.SharedKernel.Helpers;

namespace CommonCore.Data.Conventions.EFCore.Steps.Defaults
{
	public class EntityAutomappingStep : IEntityAutomappingStep
	{
		public EntityTypeBuilder Map(ModelBuilder modelBuilder, Type type)
		{
			return modelBuilder.Entity(type);
		}

		public bool ShouldMap(Type type)
		{
			var isWhyseEntityType = type.IsAggregateRoot()
				|| type.IsAggregate()
				|| type.IsEntityType();
			var isMappableType = !type.IsGenericTypeDefinition;
			return isWhyseEntityType && isMappableType;
		}
	}
}
