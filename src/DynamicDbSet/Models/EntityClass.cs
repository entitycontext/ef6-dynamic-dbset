using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicDbSet.Models
{
    public class EntityClass
    {
        [Key, Column("EntityClassId")]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
