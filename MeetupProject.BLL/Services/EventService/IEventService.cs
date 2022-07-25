using MeetupProject.BLL.Models;
using MeetupProject.BLL.Queries;

namespace MeetupProject.BLL.Services.EventService
{
    public interface IEventService
    {
        Task<Event> GetByIdAsync(Guid eventId);
        Task<Event> DeleteAsync(Guid eventId);
        Task<Event> UpdateAsync(Guid id, EventUpdateQuery updatedEvent);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> CreateAsync(Event newEvent);
    }
}
