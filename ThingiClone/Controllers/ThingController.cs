using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using ThingiClone.Models.Things;
using System.Security.Claims;
using ThingiClone.Interfaces;

namespace ThingiClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IThingService _thingService;

        public ThingController(ApplicationDbContext context, IThingService thingService)
        {
            _context = context;
            _thingService = thingService;
        }

        [HttpPost]
        [Authorize]
        [Route("thing")]
        public async Task<IActionResult> Post([FromBody] ThingDTO thing)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                Thing newThing = _thingService.Add(thing, userId);
                var newThingDto = new ThingDTO() { Name = newThing.Name, HowToPrint = newThing.HowToPrint, Tags = (from tag in newThing.Tags select tag.Name).ToList(), License = newThing.License.Name, Summary = newThing.Summary, ImageFileBase64 = newThing.Attachments!.ToList().ElementAt(0).Content, ThingFileBase64 = newThing.Attachments!.ToList().ElementAt(1).Content};
                return new OkObjectResult(newThingDto);
            }
            else { return new BadRequestObjectResult(ModelState); }
        }
    }
}
