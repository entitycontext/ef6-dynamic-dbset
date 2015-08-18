using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using DynamicDbSet.Models;

namespace DynamicDbSet.Database
{
    public class DatabaseContext : DbContext
    {
        #region Entities

        public IDbSet<EntityClass> EntityClasses { get; set; }

        #endregion

        #region Configuration

        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());
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

        private static EntityModel _EntityModel = new EntityModel("Dynamic.Models", "Dynamic.Models.Runtime");

        public static void RefreshModel()
        {
            var modelBuilder = new DbModelBuilder();
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
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #endregion

        #region Dynamic Entities

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

        //
        // EntityType
        //

        public IEntityType CreateEntityType(
            string className)
        {
            return _EntityModel.CreateEntityType(className);
        }

        public EntitySet<IEntityType> EntityTypes(
            string className)
        {
            return _EntityModel.EntityTypes(this, className);
        }
        #endregion
    }
}
