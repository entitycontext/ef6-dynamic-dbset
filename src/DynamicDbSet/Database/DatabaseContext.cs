using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

using DynamicDbSet.Models;

namespace DynamicDbSet.Database
{
    public class DatabaseContext : DbContext
    {
        #region Entities

        public DbSet<EntityClass> EntityClasses { get; set; }

        #endregion

        #region Configuration

        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());
            RefreshEntityModel();           
        }

        public DatabaseContext()
            : base(_EntityModel.CompiledModel)
        {
            ConfigureContext();
        }

        public DatabaseContext(
            bool isCompiler)
        {
            ConfigureContext();
        }

        private void ConfigureContext()
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        private static EntityModel _EntityModel = new EntityModel("DynamicDbSet.Models", "DynamicDbSet.Models.Runtime");

        public static void RefreshEntityModel()
        {
            var modelBuilder = new DbModelBuilder();
            modelBuilder.RegisterEntityType(typeof(EntityClass));
            ConfigureModel(modelBuilder);
            _EntityModel.Compile(modelBuilder);
        }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);
        }

        private static void ConfigureModel(
            DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DatabaseSettings.SchemaName);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #endregion

        #region Dynamic Entities

        public void GenerateEntityClass(
            EntityClass entityClass)
        {
            if (EntityClasses.Any(o => o.Name == entityClass.Name))
            {
                throw new Exception($"Entity class with name {entityClass.Name} already exists.");
            }

            var createTableSql = EntitySqlGenerator.GenerateCreateTables(entityClass.Name);
            Database.ExecuteSqlCommand(createTableSql);

            EntityClasses.Add(entityClass);
            SaveChanges();
        }

        //
        // Entity
        //

        public IEntity CreateEntity(
            string className)
        {
            return _EntityModel.CreateEntity(className);
        }

        public EntitySet<IEntity> Entities(
            string className)
        {
            return _EntityModel.Entities(this, className);
        }

        //
        // EntityAttribute
        //

        public IEntityAttribute CreateEntityAttribute(
            string className)
        {
            return _EntityModel.CreateEntityAttribute(className);
        }

        public EntitySet<IEntityAttribute> EntityAttributes(
            string className)
        {
            return _EntityModel.EntityAttributes(this, className);
        }

        //
        // EntityAttributeType
        //

        public IEntityAttributeType CreateEntityAttributeType(
            string className)
        {
            return _EntityModel.CreateEntityAttributeType(className);
        }

        public EntitySet<IEntityAttributeType> EntityAttributeTypes(
            string className)
        {
            return _EntityModel.EntityAttributeTypes(this, className);
        }

        //
        // EntityRelation
        //

        public IEntityRelation CreateEntityRelation(
            string className)
        {
            return _EntityModel.CreateEntityRelation(className);
        }

        public EntitySet<IEntityRelation> EntityRelations(
            string className)
        {
            return _EntityModel.EntityRelations(this, className);
        }

        //
        // EntityRelationType
        //

        public IEntityRelationType CreateEntityRelationType(
            string className)
        {
            return _EntityModel.CreateEntityRelationType(className);
        }

        public EntitySet<IEntityRelationType> EntityRelationTypes(
            string className)
        {
            return _EntityModel.EntityRelationTypes(this, className);
        }

        #endregion
    }
}
