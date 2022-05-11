# Introduction 
Contains all Data conventions for NHibernate and Entity Framework that should be the default conventions.

# Entity Framework

On Entity Framework we created something very similar to Fluent Nhibernate.
We auto register model by scanning assembly.
Then we register our plugin as the first efcore plugin (`CommonCoreConventionSetPlugin`) to decide how to map properties.
It is done in the `AutomappingEngine`
We add more conventions to handle max field length, enum to string, schema name, generic table name etc

## Setup

```csharp
using CommonCore.Data.Conventions.EFCore;

// asemblies is the list of assemblies that you want to scan
services.AddPooledDbContextFactory<DbContext>(options => options
                    .UseScannerAndConventions(assemblies);
);
```