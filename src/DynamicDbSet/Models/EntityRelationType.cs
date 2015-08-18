using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicDbSet.Models
{
    public interface IEntityRelationType
    {
        long Id { get; set; }
        long ToClassId { get; set; }
        string Name { get; set; }

        EntityClass ToClass { get; set; }

        IEnumerable<IEntityAttributeType> AttributeTypes { get; }
        IEnumerable<IEntityRelation> Relations { get; }
    }

    public abstract class EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntityRelationType
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityType : EntityType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
    {
        #region Fields

        public abstract long Id { get; set; }
        public long ToClassId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(ToClassId))]
        public EntityClass ToClass { get; set; }

        #endregion

        #region Collections

        [InverseProperty(nameof(IEntityAttributeType.RelationType))]
        public ICollection<TEntityAttributeType> AttributeTypes { get; set; } = new List<TEntityAttributeType>();

        IEnumerable<IEntityAttributeType> IEntityRelationType.AttributeTypes
        {
            get { return AttributeTypes?.AsEnumerable<IEntityAttributeType>(); }
        }

        [InverseProperty(nameof(IEntityRelation.RelationType))]
        public ICollection<TEntityRelation> Relations { get; set; } = new List<TEntityRelation>();

        IEnumerable<IEntityRelation> IEntityRelationType.Relations
        {
            get { return Relations?.AsEnumerable<IEntityRelation>(); }
        }

        #endregion
    }
}
