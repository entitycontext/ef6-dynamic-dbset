using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dynamic.Models
{
    public interface IEntityAttributeType
    {
        long Id { get; set; }
        string Name { get; set; }

        IEnumerable<IEntityAttribute> Attributes { get; }
    }

    public class EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntityAttributeType
        where TEntity : Entity<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttribute : EntityAttribute<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityAttributeType : EntityAttributeType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelation : EntityRelation<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityRelationType : EntityRelationType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        where TEntityType : EntityType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
    {
        #region Fields

        public long Id { get; set; }
        public string Name { get; set; }

        #endregion

        #region Relations

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
