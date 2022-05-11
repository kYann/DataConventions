using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using CommonCore.Data.Conventions.EFCore.Conventions;
using CommonCore.Data.Conventions.EFCore.Steps;

namespace CommonCore.Data.Conventions.EFCore
{
	public class CommonCoreConventionSetPlugin : IConventionSetPlugin
	{
		private readonly AutomappingEngine engine;

		public CommonCoreConventionSetPlugin() : this(new DefaultAutomappingEngine())
		{
		}

		public CommonCoreConventionSetPlugin(AutomappingEngine engine)
		{
			this.engine = engine;
		}

		public ConventionSet ModifyConventions(ConventionSet conventionSet)
		{
			conventionSet.EntityTypeAddedConventions.Insert(0, new AutomappingOnEntityTypeAddedConvention(engine));
			conventionSet.EntityTypeAddedConventions.Add(new SetSchemaOnEntityTypeAddedConvention());
			conventionSet.EntityTypeAddedConventions.Add(new TableNameGenericTypeOnEntityTypeAddedConvention());
			conventionSet.PropertyAddedConventions.Add(new MaxLengthOnPropertyAddedConvention());
            conventionSet.PropertyAddedConventions.Add(new EnumToStringOnPropertyTypeAddedConvention());

            return conventionSet;
		}
	}
}
