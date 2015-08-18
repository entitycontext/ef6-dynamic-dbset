using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicDbSet.Models
{
    public interface IEntityRelation
    {
        long Id { get; set; }
        long RelationTypeId { get; set; }
        long EntityId { get; set; }
        long ToEntityId { get; set; }

        IEntityRelationType RelationType { get; set; }
        IEntity Entity { get; set; }

        IEnumerable<IEntityAttribute> Attributes { get; }
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

        public abstract long Id { get; set; }
        public abstract long RelationTypeId { get; set; }
        public abstract long EntityId { get; set; }
        public long ToEntityId { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(RelationTypeId))]
        public TEntityRelationType RelationType { get; set; }

        IEntityRelationType IEntityRelation.RelationType
        {
            get { return RelationType; }
            set { RelationType = (TEntityRelationType)value; }
        }

        [ForeignKey(nameof(EntityId))]
        public TEntity Entity { get; set; }

        IEntity IEntityRelation.Entity
        {
            get { return Entity; }
            set { Entity = (TEntity)value; }
        }

        #endregion

        #region Collections

        [InverseProperty(nameof(IEntityAttribute.Relation))]
        public ICollection<TEntityAttribute> Attributes { get; set; } = new List<TEntityAttribute>();

        IEnumerable<IEntityAttribute> IEntityRelation.Attributes
        {
            get { return Attributes?.AsEnumerable<IEntityAttribute>(); }
        }

        #endregion
    }
}
