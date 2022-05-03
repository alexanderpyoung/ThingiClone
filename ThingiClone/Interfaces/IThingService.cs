using ThingiClone.Models.Things;

namespace ThingiClone.Interfaces
{
    public interface IThingService
    {
        List<Thing> GetAll();
        Thing GetById(int id);

        Thing Add(ThingDTO thingDto, string userId);
    }
}
