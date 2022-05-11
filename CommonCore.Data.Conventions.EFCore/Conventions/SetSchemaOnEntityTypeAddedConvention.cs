using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CommonCore.Data.Conventions.EFCore.Conventions
{
	public class SetSchemaOnEntityTypeAddedConvention : IEntityTypeAddedConvention
	{
		private bool IsShadowTableManyToMany(IConventionEntityTypeBuilder entityTypeBuilder)
		{
			var isDictionary = typeof(System.Collections.IDictionary).IsAssignableFrom(entityTypeBuilder.Metadata.ClrType);
			if (!isDictionary)
				return false;

			var allPropertiesForeignKey = entityTypeBuilder.Metadata.GetProperties()
				.All(_ => _.IsForeignKey());
			if (!allPropertiesForeignKey)
				return false;

			return true;
		}

		private string? GetSchema(IConventionEntityTypeBuilder entityTypeBuilder) => DataConventionHelpers.GetSchemaFromType(entityTypeBuilder.Metadata.ClrType);

		public void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
		{
			if (entityTypeBuilder.Metadata.ClrType is null)
				return;
			if (IsShadowTableManyToMany(entityTypeBuilder))
				return;
			var schema = GetSchema(entityTypeBuilder);
			if (schema is null)
				return;
			entityTypeBuilder.ToTable(entityTypeBuilder.Metadata.GetTableName(), schema);
		}
	}
}
