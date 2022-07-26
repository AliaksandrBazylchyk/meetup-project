using AutoMapper;
using MeetupProject.BLL.Models;
using MeetupProject.BLL.Queries;
using MeetupProject.Common.Exceptions;
using MeetupProject.DAL.Entities;
using MeetupProject.DAL.Repositories.EventDbRepositories;

namespace MeetupProject.BLL.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(
            IEventRepository eventRepository,
            IMapper mapper
            )
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Create new Event entity by Event Repository
        /// </summary>
        /// <param name="newEvent">Information about new entity</param>
        /// <returns>Mappen Event entity into EventDTO</returns>
        public async Task<Event> CreateAsync(Event newEvent)
        {
            var newEntity = _mapper.Map<EventEntity>(newEvent);
            newEntity.RecordCreatedTime = DateTimeOffset.Now;
            var entity = await _eventRepository.CreateAsync(newEntity);
            var mappedEntity = _mapper.Map<Event>(entity);

            return mappedEntity;
        }

        /// <summary>
        /// Check if entity with "eventId" is exist and after that deleting it.
        /// </summary>
        /// <param name="eventId">Event GUID for deleting</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">If entity with this GUID doesn't exist</exception>
        public async Task<Event> DeleteAsync(Guid eventId)
        {
            var existedEntity = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("User not found");
            var deletedEntity = await _eventRepository.DeleteAsync(existedEntity);
            var entity = _mapper.Map<Event>(deletedEntity);

            return entity;
        }

        /// <summary>
        /// Getting all entities from database and map them into EventDTO
        /// </summary>
        /// <returns>Array of Event objects</returns>
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var entities = _eventRepository.GetAllAsync();

            return entities.Select(e => _mapper.Map<Event>(e)).ToList();
        }

        /// <summary>
        /// Getting Event entity with GUID and map it into EventDTO
        /// </summary>
        /// <param name="eventId">Event Entity GUID for getting</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">If entity with this GUID doesn't exist</exception>
        public async Task<Event> GetByIdAsync(Guid eventId)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("User not found");
            var e = _mapper.Map<Event>(eventEntity);

            return e;
        }

        /// <summary>
        /// Getting query with modified properties. Check this properties on null or possible. Modife existing Event entity by not-null properties from query.
        /// </summary>
        /// <param name="id">Event entity GUID for modification</param>
        /// <param name="updatedEvent">Qury with possible modify properties</param>
        /// <returns>Updated Event entity (mapped into EventDTO)</returns>
        public async Task<Event> UpdateAsync(Guid id, EventUpdateQuery updatedEvent)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            foreach (var prop in typeof(EventUpdateQuery).GetProperties())
            {
                var userProp = typeof(EventEntity).GetProperty(prop.Name);
                var value = prop.GetValue(updatedEvent);
                if (value != null && userProp != null)
                {
                    userProp.SetValue(eventEntity, value);
                }
            }

            await _eventRepository.UpdateAsync(eventEntity);

            var result = _mapper.Map<Event>(eventEntity);

            return result;

        }
    }
}
