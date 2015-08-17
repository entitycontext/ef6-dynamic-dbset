using System.ComponentModel.DataAnnotations.Schema;

namespace Dynamic.Models
{
    public interface IEntityAttribute
    {
        long Id { get; set; }
        long EntityId { get; set; }
        long TypeId { get; set; }

        IEntity Entity { get; set; }
        IEntityAttributeType Type { get; set; }
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

        public long Id { get; set; }
        public long EntityId { get; set; }
        public long TypeId { get; set; }
        public string Value { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(EntityId))]
        public TEntity Entity { get; set; }

        IEntity IEntityAttribute.Entity
        {
            get { return Entity; }
            set { Entity = (TEntity)value;  }
        }

        [ForeignKey(nameof(TypeId))]
        public TEntityAttributeType Type { get; set; }

        IEntityAttributeType IEntityAttribute.Type
        {
            get { return Type; }
            set { Type = (TEntityAttributeType)value; }
        }

        #endregion

        #region Collections

        #endregion
    }
}
