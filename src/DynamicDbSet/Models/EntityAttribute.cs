using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicDbSet.Models
{
    public interface IEntityAttribute
    {
        long Id { get; set; }
        long TypeId { get; set; }
        long EntityId { get; set; }
        long? RelationId { get; set; }
        string Value { get; set; }

        IEntityAttributeType Type { get; set; }
        IEntity Entity { get; set; }
        IEntityRelation Relation { get; set; }
    }

    public abstract class EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        : IEntityAttribute
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
    {
        #region Fields

        public abstract long Id { get; set; }
        public abstract long TypeId { get; set; }
        public abstract long EntityId { get; set; }
        public abstract long? RelationId { get; set; }
        [Required, MaxLength(255)]
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
