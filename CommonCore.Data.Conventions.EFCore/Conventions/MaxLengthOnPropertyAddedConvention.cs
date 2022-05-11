using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCore.Data.Conventions.EFCore
{
	public class MaxLengthOnPropertyAddedConvention : IPropertyAddedConvention
	{
		public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
		{
			if (propertyBuilder.Metadata.PropertyInfo is null)
				return;
			int? len = DataConventionHelpers.StringLength(propertyBuilder.Metadata.PropertyInfo);
			if (len.HasValue && len.Value < 8000)
			{
				propertyBuilder.HasMaxLength(len.Value);
			}
		}
	}
}
