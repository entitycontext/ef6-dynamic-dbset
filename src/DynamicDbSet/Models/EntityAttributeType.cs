using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicDbSet.Models
{
    public interface IEntityAttributeType
    {
        long Id { get; set; }
        long? RelationTypeId { get; set; }
        string Name { get; set; }

        IEntityRelationType RelationType { get; set; }

        IEnumerable<IEntityAttribute> Attributes { get; }
    }

    public abstract class EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntityAttributeType
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityType : EntityType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
    {
        #region Fields

        public abstract long Id { get; set; }
        public abstract long? RelationTypeId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(RelationTypeId))]
        public TEntityRelationType RelationType { get; set; }

        IEntityRelationType IEntityAttributeType.RelationType
        {
            get { return RelationType; }
            set { RelationType = (TEntityRelationType)value; }
        }

        #endregion

        #region Collections

        [InverseProperty(nameof(IEntityAttribute.Type))]
        public ICollection<TEntityAttribute> Attributes { get; set; } = new List<TEntityAttribute>();

        IEnumerable<IEntityAttribute> IEntityAttributeType.Attributes
        {
            get { return Attributes?.AsEnumerable<IEntityAttribute>(); }
        }

        #endregion
    }
}
