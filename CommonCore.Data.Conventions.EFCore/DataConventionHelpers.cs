using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonCore.SharedKernel.Helpers;

namespace CommonCore.Data.Conventions.EFCore
{
	public static class DataConventionHelpers
	{
		public static string? GetSchemaFromType(Type entityType)
		{
			string schema;

			if (entityType.Namespace is null)
				return null;

			schema = entityType.Namespace.Replace(".Concretes", "");
			int firstPoint = entityType.Namespace.IndexOf(".") + 1;
			int secondPoint = entityType.Namespace.IndexOf(".", firstPoint);
			if (secondPoint < 0)
				secondPoint = entityType.Namespace.Length;
			schema = schema.Substring(firstPoint, secondPoint - firstPoint);

			return schema;
		}

		public static int? StringLength(MemberInfo info)
		{
			int? len = null;
			if (info is PropertyInfo propInfo)
			{
				var attributes = propInfo.GetCustomAttributes<StringLengthAttribute>(true);
				foreach (var attr in attributes)
				{
					len = attr.MaximumLength;
				}

				if (!len.HasValue)
				{
					switch (propInfo.Name)
					{
						case "Name":
							len = 50;
							break;
						case "Label":
							len = 255;
							break;
						case "PhoneNumber":
						case "FaxNumber":
							len = 64;
							break;
						default:
							if (propInfo.Name.Contains("Description")
								|| propInfo.Name.Contains("Text"))
								len = 8000;
							break;
					}
				}
			}
			return len;
		}

		public static bool IsType(MemberInfo memberInfo)
		{
			if (memberInfo.Name.Contains("Type"))
				return true;
			switch (memberInfo)
			{
				case PropertyInfo propInfo:
					if (propInfo.PropertyType.Namespace?.Contains(".Types") ?? false)
						return true;
					if (propInfo.PropertyType.IsEntityType())
						return true;
					break;
				case TypeInfo typeInfo:
					var memberType = typeInfo.AsType();
					if (memberType.Namespace?.Contains(".Types") ?? false)
						return true;
					if (memberType.IsEntityType())
						return true;
					break;
			}
			return false;
		}

		public static bool IsInSameAssembly(Type firstType, Type secondType)
		{
			return firstType.Assembly == secondType.Assembly;
		}

		public static bool TypeContainsPropertyOfType(Type primaryType, Type propertyType)
		{
			foreach (var properties in primaryType.GetProperties())
			{
				if (propertyType.IsAssignableFrom(properties.PropertyType))
					return true;
			}
			return false;
		}

		public static bool IsBaseTypeOfMappedEntity(Type type)
		{
			return type.Assembly.GetTypes().Where(c => c.BaseType == type).Any();
		}

		internal static bool IsAggregateRoot(MemberInfo memberInfo)
		{
			if (memberInfo is PropertyInfo propInfo)
			{
				return propInfo.PropertyType.IsAggregateRoot();
			}
			return false;
		}
	}
}
