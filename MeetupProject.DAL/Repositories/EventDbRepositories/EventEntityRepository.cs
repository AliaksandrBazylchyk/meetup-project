using MeetupProject.DAL.Contextes;
using MeetupProject.DAL.Entities;

namespace MeetupProject.DAL.Repositories.EventDbRepositories
{
    public class EventEntityRepository : BaseRepository<EventEntity>, IEventEntityRepository
    {
        public EventEntityRepository(EventDbContext context) : base(context)
        { }
    }
}
