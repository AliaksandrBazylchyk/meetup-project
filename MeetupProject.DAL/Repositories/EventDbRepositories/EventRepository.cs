using MeetupProject.DAL.Contextes;
using MeetupProject.DAL.Entities;

namespace MeetupProject.DAL.Repositories.EventDbRepositories
{
    public class EventRepository : BaseRepository<EventEntity>, IEventRepository
    {
        public EventRepository(EventDbContext context) : base(context)
        { }
    }
}
