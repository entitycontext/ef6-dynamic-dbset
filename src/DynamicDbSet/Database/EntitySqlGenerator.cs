using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

using DynamicDbSet.Models;

namespace DynamicDbSet.Database
{
    public static class EntitySqlGenerator
    {
        public static string GenerateCreateTables(
            string entityClassName)
        {
            using (var db = new MockEntityContext())
            {
                var sql = ((IObjectContextAdapter)db).ObjectContext.CreateDatabaseScript();
                sql = sql.Replace("MockEntity", entityClassName);
                return sql;
            }
        }
    }

    internal class MockEntity : Entity<MockEntity, MockEntityAttribute, MockEntityAttributeType, MockEntityRelation, MockEntityRelationType> 
    {                        
        [Key, Column("MockEntityId")]
        public override long Id { get; set; }
    }

    internal class MockEntityAttribute : EntityAttribute<MockEntity, MockEntityAttribute, MockEntityAttributeType, MockEntityRelation, MockEntityRelationType> 
    {
        [Key, Column("MockEntityAttributeId")]
        public override long Id { get; set; }

        [Column("MockEntityAttributeTypeId")]
        public override long TypeId { get; set; }

        [Column("MockEntityId")]
        public override long EntityId { get; set; }

        [Column("MockEntityRelationId")]
        public override long? RelationId { get; set; }
    }

    internal class MockEntityAttributeType : EntityAttributeType<MockEntity, MockEntityAttribute, MockEntityAttributeType, MockEntityRelation, MockEntityRelationType> 
    {
        [Key, Column("MockEntityAttributeTypeId")]
        public override long Id { get; set; }

        [Column("MockEntityRelationTypeId")]
        public override long? RelationTypeId { get; set; }
    }

    internal class MockEntityRelation : EntityRelation<MockEntity, MockEntityAttribute, MockEntityAttributeType, MockEntityRelation, MockEntityRelationType> 
    {
        [Key, Column("MockEntityRelationId")]
        public override long Id { get; set; }

        [Column("MockEntityId")]
        public override long EntityId { get; set; }

        [Column("MockEntityRelationTypeId")]
        public override long TypeId { get; set; }
    }

    internal class MockEntityRelationType : EntityRelationType<MockEntity, MockEntityAttribute, MockEntityAttributeType, MockEntityRelation, MockEntityRelationType> 
    {
        [Key, Column("MockEntityRelationTypeId")]
        public override long Id { get; set; }
    }

    internal class MockEntityContext : DbContext
    {
        static MockEntityContext()
        {
            System.Data.Entity.Database.SetInitializer<MockEntityContext>(null);
        }

        public DbSet<MockEntity> MockEntities { get; set; }
        public DbSet<MockEntityAttribute> MockEntityAttributes { get; set; }
        public DbSet<MockEntityAttributeType> MockEntityAttributeTypes { get; set; }
        public DbSet<MockEntityRelation> MockEntityRelations { get; set; }
        public DbSet<MockEntityRelationType> MockEntityRelationTypes { get; set; }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DatabaseSettings.SchemaName);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //
            // NOTE: If there are more "built-in" DB sets, those should be ignored as well.
            //
            // The primary drawback to this approach is that foreign key constraints from 
            // the dynamic entity tables to the built-in tables are lost when they are ignored.
            //
            // If this is a problem, there are several alternative approaches that could be taken:
            // * Add the foreign key constraints into the generated SQL DDL returned by EntitySqlGenerator.
            // * Add the foreign key constraints later through separate ExecuteSqlCommand calls.
            // * Take out the ignores and modify the generated SQL DDL to wrap all the statements with existence checks.
            // * Punt and just manually create all the SQL DDL instead of using EntitySqlGenerator.
            //
            modelBuilder.Ignore<EntityClass>();
        }
    }
}
