using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

using DynamicDbSet.Models;

namespace DynamicDbSet.Database
{
    public class EntityModel
    {
        private DbCompiledModel _CompiledModel;
        private EntityClassMap _EntityClassMap;
        private string _NameSpace;
        private string _AssemblyName;

        public EntityModel(
            string nameSpace,
            string assemblyName)
        {
            _NameSpace = nameSpace;
            _AssemblyName = assemblyName;
        }

        public DbCompiledModel CompiledModel
        {
            get { return _CompiledModel; }
        }

        public void Compile(
            DbModelBuilder modelBuilder)
        {
            //
            // Get list of domains from database for creating dynamic classes.
            //

            using (var context = new DatabaseContext(isCompiler: true))
            {
                var entityClasses = context.EntityClasses.ToList();

                var entityClassMap = EntityClassMap.Create(
                    _NameSpace,
                    _AssemblyName,
                    entityClasses);

                foreach (var entityClass in entityClasses)
                {
                    //
                    // Register entity type classes with EF model builder.
                    //

                    var name = entityClass.Name;

                    modelBuilder.RegisterEntityType(entityClassMap.Entities[name]);
                    modelBuilder.RegisterEntityType(entityClassMap.EntityAttributes[name]);
                    modelBuilder.RegisterEntityType(entityClassMap.EntityAttributeTypes[name]);
                    modelBuilder.RegisterEntityType(entityClassMap.EntityRelations[name]);
                    modelBuilder.RegisterEntityType(entityClassMap.EntityRelationTypes[name]);
                }

                //
                // Compile the model and update members.
                //

                var compiledModel = modelBuilder.Build(context.Database.Connection).Compile();

                _CompiledModel = compiledModel;
                _EntityClassMap = entityClassMap;
            }
        }

        private TEntity CreateEntity<TEntity>(
            Type entityType)
        {
            return (TEntity)Activator.CreateInstance(entityType);
        }

        private EntitySet<TEntity> GetEntitySet<TEntity>(
            DatabaseContext db,
            Type entityType)
        {
            var dynamicSet = db.GetType()
                .GetMethod("Set", new Type[0])
                .MakeGenericMethod(entityType)
                .Invoke(db, new object[0]);

            return new EntitySet<TEntity>(dynamicSet, db.Set(entityType));
        }

        //
        // Entity
        //

        public IEntity CreateEntity(
            string className)
        {
            return CreateEntity<IEntity>(_EntityClassMap.Entities[className]);
        }

        public EntitySet<IEntity> Entities(
            DatabaseContext db,
            string className)
        {
            return GetEntitySet<IEntity>(db, _EntityClassMap.Entities[className]);
        }

        //
        // EntityAttribute
        //

        public IEntityAttribute CreateEntityAttribute(
            string className)
        {
            return CreateEntity<IEntityAttribute>(_EntityClassMap.EntityAttributes[className]);
        }

        public EntitySet<IEntityAttribute> EntityAttributes(
            DatabaseContext db,
            string className)
        {
            return GetEntitySet<IEntityAttribute>(db, _EntityClassMap.EntityAttributes[className]);
        }
        
        //
        // EntityAttributeType
        //

        public IEntityAttributeType CreateEntityAttributeType(
            string className)
        {
            return CreateEntity<IEntityAttributeType>(_EntityClassMap.EntityAttributeTypes[className]);
        }

        public EntitySet<IEntityAttributeType> EntityAttributeTypes(
            DatabaseContext db,
            string className)
        {
            return GetEntitySet<IEntityAttributeType>(db, _EntityClassMap.EntityAttributeTypes[className]);
        }
        
        //
        // EntityRelation
        //

        public IEntityRelation CreateEntityRelation(
            string className)
        {
            return CreateEntity<IEntityRelation>(_EntityClassMap.EntityRelations[className]);
        }

        public EntitySet<IEntityRelation> EntityRelations(
            DatabaseContext db,
            string className)
        {
            return GetEntitySet<IEntityRelation>(db, _EntityClassMap.EntityRelations[className]);
        }
        
        //
        // EntityRelationType
        //

        public IEntityRelationType CreateEntityRelationType(
            string className)
        {
            return CreateEntity<IEntityRelationType>(_EntityClassMap.EntityRelationTypes[className]);
        }

        public EntitySet<IEntityRelationType> EntityRelationTypes(
            DatabaseContext db,
            string className)
        {
            return GetEntitySet<IEntityRelationType>(db, _EntityClassMap.EntityRelationTypes[className]);
        }
    }
}
