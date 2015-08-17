using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dynamic.Models
{
    public interface IEntityRelation
    {
        long Id { get; set; }
        long EntityId { get; set; }
        long RelationTypeId { get; set; }
        long ToEntityId { get; set; }

        IEntity Entity { get; set; }
        IEntityRelationType RelationType { get; set; }
    }

    public abstract class EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntityRelation
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
        public long RelationTypeId { get; set; }
        public long ToEntityId { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(EntityId))]
        public TEntity Entity { get; set; }

        IEntity IEntityRelation.Entity
        {
            get { return Entity; }
            set { Entity = (TEntity)value; }
        }

        [ForeignKey(nameof(RelationTypeId))]
        public TEntityRelationType RelationType { get; set; }

        IEntityRelationType IEntityRelation.RelationType
        {
            get { return RelationType; }
            set { RelationType = (TEntityRelationType)value; }
        }

        #endregion

        #region Collections

        #endregion
    }
}
