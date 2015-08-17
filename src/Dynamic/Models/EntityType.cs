using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dynamic.Models
{
    public interface IEntityType
    {
        long Id { get; set; }
        string Name { get; set; }

        IEnumerable<IEntity> Entities { get; }
    }

    public abstract class EntityType<TEntity, TEntityAttribute, TEntityAttributeType, TEntityRelation, TEntityRelationType, TEntityType>
        : IEntityType
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

        [InverseProperty(nameof(IEntity.Type))]
        public ICollection<TEntity> Entities { get; set; } = new List<TEntity>();

        IEnumerable<IEntity> IEntityType.Entities
        {
            get { return Entities?.AsEnumerable<IEntity>(); }
        }

        #endregion
    }
}
