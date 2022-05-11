using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using CommonCore.Data.Conventions.EFCore.Steps.Defaults;

namespace CommonCore.Data.Conventions.EFCore.Steps
{
	public class DefaultAutomappingEngine : AutomappingEngine
	{
		public static readonly IEntityAutomappingStep[] entitySteps = new IEntityAutomappingStep[] {
			new EntityAutomappingStep()
			};

		public static readonly IPropertyAutomappingStep[] propertySteps = new IPropertyAutomappingStep[] {
			new ManyToManyAutomappingStep(),
			new OwnOnesAutomappingStep()
			};

		public DefaultAutomappingEngine() : base(entitySteps, propertySteps)
		{
		}
	}

	public class AutomappingEngine
	{
		private readonly IEnumerable<IEntityAutomappingStep> entityAutomappingSteps;
		private readonly IEnumerable<IPropertyAutomappingStep> propertyAutomappingSteps;

		public AutomappingEngine(IEnumerable<IEntityAutomappingStep> entityAutomappingSteps,
			IEnumerable<IPropertyAutomappingStep> propertyAutomappingSteps)
		{
			this.entityAutomappingSteps = entityAutomappingSteps;
			this.propertyAutomappingSteps = propertyAutomappingSteps;
		}

		private void Map(EntityTypeBuilder entityTypeBuilder, Type entityType, PropertyInfo propertyInfo)
		{
			foreach (var propertyStep in this.propertyAutomappingSteps)
			{
				if (propertyStep.ShouldMap(entityType, propertyInfo))
					propertyStep.Map(entityTypeBuilder, entityType, propertyInfo);
			}
		}

		internal void MapProperties(EntityTypeBuilder entityBuilder, Type entityType)
		{
			foreach (var propertyInfo in entityType.GetProperties())
			{
				this.Map(entityBuilder, entityType, propertyInfo);
			}
		}

		public void Map(ModelBuilder modelBuilder, Type type)
		{
			foreach (var entityStep in this.entityAutomappingSteps)
			{
				if (entityStep.ShouldMap(type))
				{
					entityStep.Map(modelBuilder, type);
				}
			}
		}
	}
}
