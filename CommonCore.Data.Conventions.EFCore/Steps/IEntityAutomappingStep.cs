using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.EFCore.Steps
{
	public interface IEntityAutomappingStep
	{
		public EntityTypeBuilder Map(ModelBuilder modelBuilder, Type type);
		public bool ShouldMap(Type type);
	}
}
