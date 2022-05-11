using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.TestUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using CommonCore.Data.Conventions.EFCore.Steps;
using CommonCore.Data.Conventions.EFCore.Scanner;
using CommonCore.SharedKernel;
using Xunit;

namespace CommonCore.Data.Conventions.EFCore.Tests
{
    public class Address : IValueType
    {
        public string StreeName { get; set; }

        public string City { get; set; }
    }

    public class GenericDef<T> : IAggregateRoot<int>
    {
        public int Id { get; set; }
    }

    public class Tag : IEntityType<int>
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Name { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }

    public class Blog : IAggregateRoot<int>
    {
        public int Id { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public Address Address { get; set; }
    }

    public class Post : IAggregate<int>
    {
        public int Id { get; set; }

        public string Text { get; set; }

        [MaxLength(128)]
        public string TestLength { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }

    public class CommonCoreConventionTest
    {
        [Fact]
        public void AutoMappingTest()
        {
            var model = BuildModel((b, engine) =>
            {
                b.ApplyAutomappingFromAssembly(engine, typeof(Blog).Assembly);
                engine.Map(b, typeof(GenericDef<Guid>));
                //engine.Map(b, typeof(Blog));
                //engine.Map(b, typeof(Post));
            });
            var blogEntityType = model.FindEntityType(typeof(Blog));

            Assert.NotNull(blogEntityType);
            Assert.Equal("Data", blogEntityType.GetSchema());

            var postEntityType = model.FindEntityType(typeof(Post));

            var navigation = postEntityType.GetSkipNavigations().Single();
            Assert.Equal("Tags", navigation.Name);

            var property = postEntityType.GetProperty("Text");
            Assert.Null(property.GetMaxLength());

            property = postEntityType.GetProperty("TestLength");
            Assert.Equal(128, property.GetMaxLength().Value);

            property = postEntityType.GetProperty("BlogId");
            Assert.NotNull(property);

            var addressEntityType = model.FindRuntimeEntityType(typeof(Address));
            Assert.NotNull(addressEntityType);
            Assert.True(addressEntityType.IsOwned());
        }

        #region Support

        private IModel BuildModel(Action<ModelBuilder, AutomappingEngine> buildAction, CultureInfo cultureInfo = null)
        {
            var engine = new DefaultAutomappingEngine();
            var modelBuilder = InMemoryTestHelpers.Instance.CreateConventionBuilder(configure: cfg =>
            {
                var conventionSet = cfg.Conventions;
                new CommonCoreConventionSetPlugin(engine).ModifyConventions(conventionSet);
            }
            );
            //ConventionSet.Remove(conventionSet.ModelFinalizedConventions, typeof(ValidatingConvention));

            buildAction(modelBuilder, engine);

            return modelBuilder.FinalizeModel();
        }

        private Microsoft.EntityFrameworkCore.Metadata.IEntityType BuildEntityType(string entityTypeName, Action<EntityTypeBuilder, AutomappingEngine> buildAction, CultureInfo cultureInfo = null)
            => BuildModel((b, e) => buildAction(b.Entity(entityTypeName), e), cultureInfo).GetEntityTypes().Single();

        #endregion
    }
}
