using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ThingiClone.Interfaces;
using ThingiClone.Models.Things;

namespace ThingiClone.Services
{
    public class ThingService : IThingService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ThingService(ApplicationDbContext ctx) {
            _applicationDbContext = ctx;
        }

        public List<Thing> GetAll() {
            throw new NotImplementedException();
        }

        public async Task<List<Thing>> GetAllAsync() {
            return await _applicationDbContext.Set<Thing>().ToListAsync();
        }

        public Thing GetById(int id) {
            throw new NotImplementedException();
        }

        public Thing Add(ThingDTO thing, string userId)
        {


            var attachmentList = new List<Attachment>();
            var thing_file = thing.ThingFileBase64;
            attachmentList.Add(new Attachment("ThingFile", thing_file));
            var image_file = thing.ImageFileBase64;
            attachmentList.Add(new Attachment("ImageFile", image_file));
            var license = new License() { Name = thing.License, Description = "Sample" };
            var name = thing.Name;
            var summary = thing.Summary;

            var newThing = new Thing() { Name = name, Summary = summary, Attachments = attachmentList, Tags = new List<Tag>(), License = license, UserId = userId };

            var stringTags = thing.Tags;
            foreach (string tag in stringTags)
            {
                var present = _applicationDbContext.Set<Tag>().Where(c => (c.Name == tag)).FirstOrDefault();
                if (present != null)
                {
                    newThing.Tags.Add(present);
                }
                else
                {
                    var newTag = new Tag(tag);
                    newThing.Tags.Add(newTag);
                }
            }

            _applicationDbContext.Set<Thing>().Add(newThing);
            _applicationDbContext.SaveChanges();
            return newThing;
        }
    }
}