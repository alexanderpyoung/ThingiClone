using System.ComponentModel.DataAnnotations.Schema;

namespace ThingiClone.Models.Things
{
    public class Attachment
    {
        public Attachment(string name, string content)
        {
            Name = name;
            Content = content;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AttachmentId { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }

        public int ThingId { get; set; }
    }
}
