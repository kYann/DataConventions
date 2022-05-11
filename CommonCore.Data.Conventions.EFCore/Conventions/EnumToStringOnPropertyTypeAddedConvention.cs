using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCore.Data.Conventions.EFCore.Conventions
{
    public class EnumToStringOnPropertyTypeAddedConvention : IPropertyAddedConvention
    {
        public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
        {
            var enumType = this.GetEnumType(propertyBuilder.Metadata.ClrType);
            if (enumType is null)
                return;

            var valueConverter = CreateValueConverter(enumType);
            if (valueConverter is null)
                throw new InvalidOperationException("Cannot create value converter");
            propertyBuilder.HasConversion(valueConverter);
        }

        private ValueConverter? CreateValueConverter(Type enumType)
        {
            var converterType = typeof(EnumToStringConverter<>).MakeGenericType(enumType);

            return (ValueConverter?)Activator.CreateInstance(converterType, (object?)null);
        }

        private Type? GetEnumType(Type clrType)
        {
            if (clrType.IsEnum)
                return clrType;
            else
            {
                var u = Nullable.GetUnderlyingType(clrType);
                return (u != null) && u.IsEnum ? u : null;
            }
        }
    }
}
