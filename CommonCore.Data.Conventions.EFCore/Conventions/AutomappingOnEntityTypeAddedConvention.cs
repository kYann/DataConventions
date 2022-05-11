using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using CommonCore.Data.Conventions.EFCore.Steps;

namespace CommonCore.Data.Conventions.EFCore.Conventions
{
	public class AutomappingOnEntityTypeAddedConvention : IEntityTypeAddedConvention
	{
		private readonly AutomappingEngine engine;

		public AutomappingOnEntityTypeAddedConvention(AutomappingEngine engine)
		{
			this.engine = engine;
		}

		public void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
		{
			if (entityTypeBuilder.Metadata.ClrType is null)
				return;
			if (entityTypeBuilder.Metadata is IMutableEntityType entityType)
			{
#pragma warning disable EF1001 // Internal EF Core API usage.
				var builder = new EntityTypeBuilder(entityType);
#pragma warning restore EF1001 // Internal EF Core API usage.
				engine.MapProperties(builder, entityTypeBuilder.Metadata.ClrType);
			}
		}
	}
}
