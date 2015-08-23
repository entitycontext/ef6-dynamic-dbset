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
            RebuildDbModel();           
        }

        public DatabaseContext()
            : base(_EntityDbModel.CompiledModel)
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

        private static EntityDbModel _EntityDbModel = new EntityDbModel("DynamicDbSet.Models", "DynamicDbSet.Models.Runtime");

        public static void RebuildDbModel()
        {
            var modelBuilder = new DbModelBuilder();
            modelBuilder.RegisterEntityType(typeof(EntityClass));
            ConfigureModel(modelBuilder);
            _EntityDbModel.Compile(modelBuilder);
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
            return _EntityDbModel.CreateEntity(className);
        }

        public EntityDbSet<IEntity> Entities(
            string className)
        {
            return _EntityDbModel.Entities(this, className);
        }

        //
        // EntityAttribute
        //

        public IEntityAttribute CreateEntityAttribute(
            string className)
        {
            return _EntityDbModel.CreateEntityAttribute(className);
        }

        public EntityDbSet<IEntityAttribute> EntityAttributes(
            string className)
        {
            return _EntityDbModel.EntityAttributes(this, className);
        }

        //
        // EntityAttributeType
        //

        public IEntityAttributeType CreateEntityAttributeType(
            string className)
        {
            return _EntityDbModel.CreateEntityAttributeType(className);
        }

        public EntityDbSet<IEntityAttributeType> EntityAttributeTypes(
            string className)
        {
            return _EntityDbModel.EntityAttributeTypes(this, className);
        }

        //
        // EntityRelation
        //

        public IEntityRelation CreateEntityRelation(
            string className)
        {
            return _EntityDbModel.CreateEntityRelation(className);
        }

        public EntityDbSet<IEntityRelation> EntityRelations(
            string className)
        {
            return _EntityDbModel.EntityRelations(this, className);
        }

        //
        // EntityRelationType
        //

        public IEntityRelationType CreateEntityRelationType(
            string className)
        {
            return _EntityDbModel.CreateEntityRelationType(className);
        }

        public EntityDbSet<IEntityRelationType> EntityRelationTypes(
            string className)
        {
            return _EntityDbModel.EntityRelationTypes(this, className);
        }

        #endregion
    }
}
