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

        public async Task<Event> CreateAsync(Event newEvent)
        {
            var newEntity = _mapper.Map<EventEntity>(newEvent);
            newEntity.RecordCreatedTime = DateTimeOffset.Now;
            var entity = await _eventRepository.CreateAsync(newEntity);
            var mappedEntity = _mapper.Map<Event>(entity);

            return mappedEntity;
        }

        public async Task<Event> DeleteAsync(Guid eventId)
        {
            var existedEntity = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("User not found");
            var deletedEntity = await _eventRepository.DeleteAsync(existedEntity);
            var entity = _mapper.Map<Event>(deletedEntity);

            return entity;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var entities = _eventRepository.GetAllAsync();

            return entities.Select(e => _mapper.Map<Event>(e)).ToList();
        }

        public async Task<Event> GetByIdAsync(Guid eventId)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("User not found");
            var e = _mapper.Map<Event>(eventEntity);

            return e;
        }

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
