using CommonCore.Data.Conventions.EFCore.Steps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonCore.Data.Conventions.EFCore
{
    public static class DbContextOptionsExtension
    {
        public static IEnumerable<Assembly> GetScannerAssemblies(this DbContextOptions options)
        {
            var optExt = options.FindExtension<ScannerContextOptionsExtension>();
            var assemblies = optExt?.Assemblies ?? Enumerable.Empty<Assembly>();

            return assemblies;
        }
    }

    public static class DbContextOptionsBuilderExtension
    {
        public static DbContextOptionsBuilder UseScannerAndConventions(this DbContextOptionsBuilder options, IEnumerable<Assembly> assemblies)
        {
            var extension = new ScannerContextOptionsExtension(assemblies);
            ((IDbContextOptionsBuilderInfrastructure)options).AddOrUpdateExtension(extension);

            return options;
        }

        public static DbContextOptionsBuilder UseScannerAndConventions(this DbContextOptionsBuilder options, IEnumerable<Assembly> assemblies, AutomappingEngine automappingEngine)
        {
            var extension = new ScannerContextOptionsExtension(assemblies, automappingEngine);
            ((IDbContextOptionsBuilderInfrastructure)options).AddOrUpdateExtension(extension);

            return options;
        }
    }

    public class ScannerContextOptionsExtension : IDbContextOptionsExtension
    {
        public ScannerContextOptionsExtension() : this(Enumerable.Empty<Assembly>())
        {
        }

        public ScannerContextOptionsExtension(IEnumerable<Assembly> assemblies) : this(assemblies, new DefaultAutomappingEngine())
        {
        }

        public ScannerContextOptionsExtension(IEnumerable<Assembly> assemblies, AutomappingEngine automappingEngine)
        {
            this.Assemblies = assemblies;
            AutomappingEngine = automappingEngine;
        }

        public IEnumerable<Assembly> Assemblies { get; private set; }

        public AutomappingEngine AutomappingEngine { get; private set; }

        public DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);

        public void ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkCommonCoreConventions(AutomappingEngine);
        }

        public void Validate(IDbContextOptions options)
        {
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            private string _logFragment = string.Empty;

            public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }

            private new ScannerContextOptionsExtension Extension
                => (ScannerContextOptionsExtension)base.Extension;

            public override bool IsDatabaseProvider => false;

            public override string LogFragment
            {
                get
                {
                    if (_logFragment == null)
                    {
                        var builder = new StringBuilder();

                        foreach (var assembly in Extension.Assemblies)
                        {
                            builder.Append($"EFCore Scan of Assembly : {assembly.GetName()}");
                        }

                        _logFragment = builder.ToString();
                    }

                    return _logFragment;
                }
            }

            public override int GetServiceProviderHashCode()
            {
                var combiner = new CommonCore.SharedKernel.HashCodeCombiner();
                combiner.Add(Extension.Assemblies);
                return combiner.CombinedHash;
            }

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            {
                return other.GetServiceProviderHashCode() == this.GetServiceProviderHashCode();
            }

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["Assemblies"]
                    = string.Join(", ", this.Extension.Assemblies.Select(_ => _.GetName()));
            }
        }
    }
}
