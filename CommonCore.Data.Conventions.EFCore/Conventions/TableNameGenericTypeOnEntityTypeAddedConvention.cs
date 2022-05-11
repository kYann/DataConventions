using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCore.Data.Conventions.EFCore.Conventions
{
	public class TableNameGenericTypeOnEntityTypeAddedConvention : IEntityTypeAddedConvention
	{
		private string? GetSchema(Type clrType) => DataConventionHelpers.GetSchemaFromType(clrType);

		private string GetName(Type clrType) => clrType.Name.Split('`').FirstOrDefault() ?? clrType.Name;

		public void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
		{
			var clrType = entityTypeBuilder.Metadata.ClrType;
			if (clrType is null)
				return;
			if (entityTypeBuilder.Metadata.HasSharedClrType)
				return;
			if (!clrType.IsGenericType)
				return;
			var schema = GetSchema(clrType);
			var name = GetName(clrType);
			entityTypeBuilder.ToTable(name, schema);
		}
	}
}
