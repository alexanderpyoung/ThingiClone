using System.ComponentModel.DataAnnotations;

namespace ThingiClone.Models.Things
{
    public class ThingDTO
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Summary { get; set; }

        public string? HowToPrint { get; set; }
        public List<string>? Tags { get; set; }

        [Required]
        public string? License { get; set; }

        [Required]
        public string? ThingFileBase64 { get; set; }

        [Required]
        public string? ImageFileBase64 { get; set; }
    }
}
