using AutoMapper;
using MeetupProject.API.Requests;
using MeetupProject.BLL.Models;
using MeetupProject.BLL.Queries;
using MeetupProject.BLL.Services.EventService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetupProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(
            IEventService eventService,
            IMapper mapper
            )
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint to search for an event by its GUID.
        /// </summary>
        /// <param name="id">Event Guid from database</param>
        /// <returns>Event object with id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var result = await _eventService.GetByIdAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Endpoint to view all events from database
        /// </summary>
        /// <returns>Array of Event objects</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var result = await _eventService.GetAllAsync();

            return Ok(result);
        }

        /// <summary>
        /// Update existed event record (if exception throwed doesn't do anything)
        /// </summary>
        /// <param name="id">Event GUID for modification</param>
        /// <param name="model">Query with possible elements to change</param>
        /// <returns>Updated event object</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEventAsync(Guid id, [FromQuery] EventUpdateQuery model)
        {
            var result = await _eventService.UpdateAsync(id, model);

            return Ok(result);
        }

        /// <summary>
        /// Delete existed event record (if exception throwed doesn't do anything)
        /// </summary>
        /// <param name="id">vent GUID for deleting</param>
        /// <returns>Deleted event object</returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteEventAsync(Guid id)
        {
            var result = await _eventService.DeleteAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Create new record with new event
        /// </summary>
        /// <param name="newEvent">object contain whole information about new event</param>
        /// <returns>Created object</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEventAsync(CreateEventRequest newEvent)
        {
            var mappedEvent = _mapper.Map<Event>(newEvent);
            var result = await _eventService.CreateAsync(mappedEvent);

            return Ok(result);
        }
    }
}
