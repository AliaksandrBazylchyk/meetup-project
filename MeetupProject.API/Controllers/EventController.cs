using MeetupProject.BLL.Models;
using MeetupProject.BLL.Queries;
using MeetupProject.BLL.Services.EventService;
using Microsoft.AspNetCore.Mvc;

namespace MeetupProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(
            IEventService eventService
            )
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var result = await _eventService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _eventService.GetAllAsync();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEventAsync(Guid id, [FromQuery] EventUpdateQuery model)
        {
            var result = await _eventService.UpdateAsync(id, model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventAsync(Guid id)
        {
            var result = await _eventService.DeleteAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventAsync(Event newEvent)
        {
            var result = await _eventService.CreateAsync(newEvent);

            return Ok(result);
        }
    }
}
