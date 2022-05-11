using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CommonCore.Data.Conventions.EFCore.Steps;

namespace CommonCore.Data.Conventions.EFCore.Scanner
{
	public static class AssemblyScannerExtension
	{
		public static ModelBuilder ApplyAutomappingFromAssembly(this ModelBuilder modelBuilder, AutomappingEngine engine, Assembly assembly)
		{
			foreach (var type in assembly.GetTypes())
			{
				engine.Map(modelBuilder, type);
			}

			return modelBuilder;
		}

		public static ModelBuilder ApplyAutomappingAndConfigurationFromAssemblies(this ModelBuilder modelBuilder, AutomappingEngine engine, IEnumerable<Assembly> assemblies)
		{
			foreach (var assembly in assemblies)
			{
				modelBuilder.ApplyConfigurationsFromAssembly(assembly)
					.ApplyAutomappingFromAssembly(engine, assembly);
			}

			return modelBuilder;
		}
	}
}
