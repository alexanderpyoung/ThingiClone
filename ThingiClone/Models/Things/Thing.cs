using System.ComponentModel.DataAnnotations;

namespace ThingiClone.Models.Things
{
    public class Thing
    {
        [Required]
        public int ThingId { get; set; }   

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Summary { get; set; }

        public string? HowToPrint { get; set; }
        
        public ICollection<Tag>? Tags { get; set; }

        [Required]
        public License? License { get; set; }

        [Required]
        public List<Attachment>? Attachments { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
