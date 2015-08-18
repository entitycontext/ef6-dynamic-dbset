using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicDbSet.Models
{
    public interface IEntityAttribute
    {
        long Id { get; set; }
        long TypeId { get; set; }
        long EntityId { get; set; }
        long? RelationId { get; set; }

        IEntityAttributeType Type { get; set; }
        IEntity Entity { get; set; }
        IEntityRelation Relation { get; set; }
    }

    public abstract class EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntityAttribute
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityType : EntityType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
    {
        #region Fields

        public abstract long Id { get; set; }
        public abstract long TypeId { get; set; }
        public abstract long EntityId { get; set; }
        public abstract long? RelationId { get; set; }
        public string Value { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(TypeId))]
        public TEntityAttributeType Type { get; set; }

        IEntityAttributeType IEntityAttribute.Type
        {
            get { return Type; }
            set { Type = (TEntityAttributeType)value; }
        }

        [ForeignKey(nameof(EntityId))]
        public TEntity Entity { get; set; }

        IEntity IEntityAttribute.Entity
        {
            get { return Entity; }
            set { Entity = (TEntity)value;  }
        }

        [ForeignKey(nameof(RelationId))]
        public TEntityRelation Relation { get; set; }

        IEntityRelation IEntityAttribute.Relation
        {
            get { return Relation; }
            set { Relation = (TEntityRelation)value; }
        }

        #endregion

        #region Collections

        #endregion
    }
}
