using AutoMapper;
using MeetupProject.BLL.Models;
using MeetupProject.BLL.Queries;
using MeetupProject.DAL.Entities;

namespace MeetupProject.BLL.MappingProfiles
{
    public class BllMappingProfile : Profile
    {
        public override string ProfileName => "BusinessLoginMappingProfile";
        public BllMappingProfile()
        {
            CreateMap<Event, EventEntity>();
            CreateMap<EventEntity, Event>();

            CreateMap<EventUpdateQuery, Event>();
        }
    }
}
