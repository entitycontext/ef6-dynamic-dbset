using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicDbSet.Models
{
    public interface IEntity
    {
        long Id { get; set; }
        string Name { get; set; }

        IEnumerable<IEntityAttribute> Attributes { get; }
        IEnumerable<IEntityRelation> Relations { get; }
    }

    public abstract class Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        : IEntity
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType>
    {
        #region Fields

        public abstract long Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }

        #endregion

        #region Relations

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
