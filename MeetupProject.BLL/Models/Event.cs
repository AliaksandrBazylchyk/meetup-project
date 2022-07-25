using MeetupProject.Common.Enums;

namespace MeetupProject.BLL.Models
{
    public class Event
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public EventType Type { get; set; }
        public string Description { get; set; }
        public List<string> Plan { get; set; }

        public string Organizer { get; set; }
        public string Speaker { get; set; }

        public DateTimeOffset EventTime { get; set; }
        public string Place { get; set; }
    }
}
