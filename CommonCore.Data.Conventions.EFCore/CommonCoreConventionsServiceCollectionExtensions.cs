using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using CommonCore.Data.Conventions.EFCore.Steps;

namespace CommonCore.Data.Conventions.EFCore
{
	/// <summary>
	/// Extension methods for <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
	/// </summary>
	public static class CommonCoreConventionsServiceCollectionExtensions
	{
		public static IServiceCollection AddEntityFrameworkCommonCoreConventions(this IServiceCollection serviceCollection)
		{
			new EntityFrameworkServicesBuilder(serviceCollection)
				.TryAdd<IConventionSetPlugin, CommonCoreConventionSetPlugin>();

			return serviceCollection;
		}

		public static IServiceCollection AddEntityFrameworkCommonCoreConventions(this IServiceCollection serviceCollection, AutomappingEngine engine)
		{
			new EntityFrameworkServicesBuilder(serviceCollection)
				.TryAdd<IConventionSetPlugin>(t => new CommonCoreConventionSetPlugin(engine));

			return serviceCollection;
		}
	}
}
