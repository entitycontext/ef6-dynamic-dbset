using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

using DynamicDbSet.Models;

namespace DynamicDbSet.Database
{
    public class EntityDbModel
    {
        private DbCompiledModel _CompiledModel;
        private EntityCodeGenerator _EntityCodeGenerator;
        private string _NameSpace;
        private string _AssemblyName;

        public EntityDbModel(
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

                var entityCodeGenerator = EntityCodeGenerator.Create(
                    _NameSpace,
                    _AssemblyName,
                    entityClasses);

                foreach (var entityClass in entityClasses)
                {
                    //
                    // Register entity type classes with EF model builder.
                    //

                    var name = entityClass.Name;

                    modelBuilder.RegisterEntityType(entityCodeGenerator.Entities[name]);
                    modelBuilder.RegisterEntityType(entityCodeGenerator.EntityAttributes[name]);
                    modelBuilder.RegisterEntityType(entityCodeGenerator.EntityAttributeTypes[name]);
                    modelBuilder.RegisterEntityType(entityCodeGenerator.EntityRelations[name]);
                    modelBuilder.RegisterEntityType(entityCodeGenerator.EntityRelationTypes[name]);
                }

                //
                // Compile the model and update members.
                //

                var compiledModel = modelBuilder.Build(context.Database.Connection).Compile();

                _CompiledModel = compiledModel;
                _EntityCodeGenerator = entityCodeGenerator;
            }
        }

        private TEntity CreateEntity<TEntity>(
            Type entityType)
        {
            return (TEntity)Activator.CreateInstance(entityType);
        }

        private EntityDbSet<TEntity> GetEntityDbSet<TEntity>(
            DatabaseContext db,
            Type entityType)
        {
            var dynamicSet = db.GetType()
                .GetMethod("Set", new Type[0])
                .MakeGenericMethod(entityType)
                .Invoke(db, new object[0]);

            return new EntityDbSet<TEntity>(dynamicSet, db.Set(entityType));
        }

        //
        // Entity
        //

        public IEntity CreateEntity(
            string className)
        {
            return CreateEntity<IEntity>(_EntityCodeGenerator.Entities[className]);
        }

        public EntityDbSet<IEntity> Entities(
            DatabaseContext db,
            string className)
        {
            return GetEntityDbSet<IEntity>(db, _EntityCodeGenerator.Entities[className]);
        }

        //
        // EntityAttribute
        //

        public IEntityAttribute CreateEntityAttribute(
            string className)
        {
            return CreateEntity<IEntityAttribute>(_EntityCodeGenerator.EntityAttributes[className]);
        }

        public EntityDbSet<IEntityAttribute> EntityAttributes(
            DatabaseContext db,
            string className)
        {
            return GetEntityDbSet<IEntityAttribute>(db, _EntityCodeGenerator.EntityAttributes[className]);
        }
        
        //
        // EntityAttributeType
        //

        public IEntityAttributeType CreateEntityAttributeType(
            string className)
        {
            return CreateEntity<IEntityAttributeType>(_EntityCodeGenerator.EntityAttributeTypes[className]);
        }

        public EntityDbSet<IEntityAttributeType> EntityAttributeTypes(
            DatabaseContext db,
            string className)
        {
            return GetEntityDbSet<IEntityAttributeType>(db, _EntityCodeGenerator.EntityAttributeTypes[className]);
        }
        
        //
        // EntityRelation
        //

        public IEntityRelation CreateEntityRelation(
            string className)
        {
            return CreateEntity<IEntityRelation>(_EntityCodeGenerator.EntityRelations[className]);
        }

        public EntityDbSet<IEntityRelation> EntityRelations(
            DatabaseContext db,
            string className)
        {
            return GetEntityDbSet<IEntityRelation>(db, _EntityCodeGenerator.EntityRelations[className]);
        }
        
        //
        // EntityRelationType
        //

        public IEntityRelationType CreateEntityRelationType(
            string className)
        {
            return CreateEntity<IEntityRelationType>(_EntityCodeGenerator.EntityRelationTypes[className]);
        }

        public EntityDbSet<IEntityRelationType> EntityRelationTypes(
            DatabaseContext db,
            string className)
        {
            return GetEntityDbSet<IEntityRelationType>(db, _EntityCodeGenerator.EntityRelationTypes[className]);
        }
    }
}
