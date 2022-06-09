using CommonCore.Data.Conventions.EFCore.Scanner;
using CommonCore.Data.Conventions.EFCore.Steps;
using Microsoft.EntityFrameworkCore;


namespace CommonCore.Data.Conventions.EFCore;
public abstract class DefaultDbContext : DbContext
{
    private readonly DbContextOptions options;

    public DefaultDbContext(DbContextOptions options) : base(options)
    {
        this.options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = options.GetScannerAssemblies();
        var ignoredTypes = options.GetIgnoredTypes();
        var engine = options.GetAutomappingEngine();

        foreach (var ignoredType in ignoredTypes)
        {
            modelBuilder.Ignore(ignoredType);
        }

        modelBuilder.ApplyAutomappingAndConfigurationFromAssemblies(engine ?? new DefaultAutomappingEngine(), assemblies);
        base.OnModelCreating(modelBuilder);
    }
}
