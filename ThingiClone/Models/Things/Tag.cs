using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThingiClone.Models.Things
{
    public class Tag
    {
        public Tag(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Thing>? Things { get; set; }
    }
}
