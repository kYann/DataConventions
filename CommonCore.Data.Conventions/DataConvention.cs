using CommonCore.SharedKernel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class DataConvention
	{
		public static string GetSchemaFromType(Type entityType)
		{
			string schema;

			schema = entityType.Namespace.Replace(".Concretes", "");
			int firstPoint = entityType.Namespace.IndexOf(".") + 1;
			int secondPoint = entityType.Namespace.IndexOf(".", firstPoint);
			if (secondPoint < 0)
				secondPoint = entityType.Namespace.Length;
			schema = schema.Substring(firstPoint, secondPoint - firstPoint).ToLowerInvariant();

			return schema;
		}

		public static string GetTableFromType(Type entityType)
		{
			return entityType.Name.Split('`').First().CamelCaseToCConvention();
		}

		public static string GetColumnNameFromPropertyName(string propertyName)
		{
			return propertyName.CamelCaseToCConvention();
		}

		public static Type ResolveConcreteTypeFromInterface(Type interfaceType)
		{
			if (interfaceType.IsInterface)
			{
				string modelName = interfaceType.Namespace + ".Concretes." + interfaceType.Name.Substring(1)
					+ ", " + interfaceType.Assembly.FullName;
				return Type.GetType(modelName);
			}
			return interfaceType;
		}

		public static bool IsNullable(MemberInfo info)
		{
			if (info is PropertyInfo)
			{
				var propInfo = info as PropertyInfo;
				// Check if it's a value type but not a nullable
				if (propInfo.PropertyType.IsValueType
					&& !(propInfo.PropertyType.IsGenericType
						&& propInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
					return false;
				var attributes = propInfo.GetCustomAttributes<RequiredAttribute>(true);
				foreach (var attr in attributes)
				{
					return false;
				}
			}
			return true;
		}

		public static int? StringLength(MemberInfo info)
		{
			int? len = null;
			if (info is PropertyInfo)
			{
				var propInfo = info as PropertyInfo;
				var attributes = propInfo.GetCustomAttributes<StringLengthAttribute>(true);
				foreach (var attr in attributes)
				{
					len = (attr as StringLengthAttribute).MaximumLength;
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
					if (propInfo.PropertyType.Namespace.Contains(".Types"))
						return true;
					if (propInfo.PropertyType.IsEntityType())
						return true;
					break;
				case TypeInfo typeInfo:
					var memberType = typeInfo.AsType();
					if (memberType.Namespace.Contains(".Types"))
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
			if (memberInfo is PropertyInfo)
			{
				var propInfo = memberInfo as PropertyInfo;
				if (propInfo.PropertyType.IsAggregateRoot())
					return true;
			}
			return false;
		}
	}
}
