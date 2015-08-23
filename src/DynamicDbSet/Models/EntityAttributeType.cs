using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public abstract class EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        : IEntityAttributeType
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
    {
        #region Fields

        public abstract long Id { get; set; }
        public abstract long? RelationTypeId { get; set; }
        [Required, MaxLength(255)]
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
