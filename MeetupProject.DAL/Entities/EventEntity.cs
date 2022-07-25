using MeetupProject.Common.Enums;

namespace MeetupProject.DAL.Entities
{
    public class EventEntity : BaseEntity
    {
        public string Name { get; set; }
        public EventType Type { get; set; }
        public string Description {  get; set; }
        public IEnumerable<string> Plan { get; set; }

        public string Organizer { get; set; }
        public string Speaker { get; set; }

        public DateTimeOffset EventTime { get; set; }
        public string Place { get; set; }
    }
}
