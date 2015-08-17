using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dynamic.Models
{
    public interface IEntity
    {
        long Id { get; set; }
        long SetId { get; set; }
        long TypeId { get; set; }

        EntityClass Class { get; set; }
        IEntityType Type { get; set; }

        IEnumerable<IEntityAttribute> Attributes { get; }
        IEnumerable<IEntityRelation> Relations { get; }
    }

    public abstract class Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntity
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityType : EntityType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
    {
        #region Fields

        public long Id { get; set; }
        public long SetId { get; set; }
        public long TypeId { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(SetId))]
        public EntityClass Class { get; set; }

        [ForeignKey(nameof(TypeId))]
        TEntityType Type { get; set; }

        IEntityType IEntity.Type
        {
            get { return Type; }
            set { Type = (TEntityType)value; }
        }

        #endregion

        #region Collections

        [InverseProperty(nameof(IEntityAttribute.Entity))]
        public ICollection<TEntityAttribute> Attributes { get; set; } = new List<TEntityAttribute>();

        IEnumerable<IEntityAttribute> IEntity.Attributes
        {
            get { return Attributes?.AsEnumerable<IEntityAttribute>(); }
        }

        [InverseProperty(nameof(IEntityRelation.Entity))]
        public ICollection<TEntityRelation> Relations { get; set; } = new List<TEntityRelation>();

        IEnumerable<IEntityRelation> IEntity.Relations
        {
            get { return Relations?.AsEnumerable<IEntityRelation>(); }
        }

        #endregion
    }
}
