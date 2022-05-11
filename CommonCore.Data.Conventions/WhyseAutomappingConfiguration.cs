using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using CommonCore.SharedKernel.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommonCore.Data.Conventions.NHibernate
{
	public class WhyseAutomappingConfiguration : DefaultAutomappingConfiguration
	{
		public override bool ShouldMap(Type type)
		{
			if (type.IsDefined(typeof(CompilerGeneratedAttribute), false))
				return false;
			var declaringType = type.DeclaringType ?? type;
			return (declaringType.IsAggregate() || declaringType.IsAggregateRoot())
				&& type.IsClass && !type.IsGenericTypeDefinition;
		}

		public override bool ShouldMap(Member member)
		{
			if (member.IsProperty)
			{
				var prop = member.MemberInfo as PropertyInfo;
				if (!prop.CanRead || !prop.CanWrite)
					return false;
			}
			return base.ShouldMap(member);
		}

		public override bool IsComponent(Type type)
		{
			var isNotAggregate = !(type.IsAggregate() || type.IsAggregateRoot());
			var declaringType = type.DeclaringType;
			var isPartOfAggregate = false;
			if (declaringType != null)
				isPartOfAggregate = declaringType.IsAggregate() || declaringType.IsAggregateRoot();
			return (isNotAggregate && isPartOfAggregate) || type.IsValueType();
		}

		public override string GetComponentColumnPrefix(Member member)
		{
			var prefix = member.Name.CamelCaseToCConvention() + "_";
			return prefix;
		}

		public override bool IsId(Member member)
		{
			if (IsComponent(member.DeclaringType))
				return false;
			return base.IsId(member);
		}

		public override IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder)
		{
			yield return new IdentityStep(this);
			yield return new VersionStep(this);
			yield return new ComponentStep(this);
			yield return new PropertyStep(conventionFinder, this);
			yield return new CustomHasManyToManyStep(this);
			yield return new ReferenceStep(this);
			yield return new HasManyStep(this);
		}
	}
}
