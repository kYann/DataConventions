using CommonCore.SharedKernel.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommonCore.Data.Conventions.EFCore.Steps;

namespace CommonCore.Data.Conventions.EFCore.Steps.Defaults
{
    public class OwnOnesAutomappingStep : IPropertyAutomappingStep
    {
        public void Map(EntityTypeBuilder entityTypeBuilder, Type entityType, PropertyInfo propertyInfo)
        {
            entityTypeBuilder.OwnsOne(propertyInfo.PropertyType, propertyInfo.Name);
        }

        public virtual bool ShouldMap(Type entityType, PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsValueType();
        }
    }
}
