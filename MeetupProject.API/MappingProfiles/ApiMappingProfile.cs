using AutoMapper;
using MeetupProject.API.Requests;
using MeetupProject.BLL.Models;

namespace MeetupProject.API.MappingProfiles
{
    public class ApiMappingProfile : Profile
    {
        public override string ProfileName => "ApiMappingProfile";
        public ApiMappingProfile()
        {
            CreateMap<CreateEventRequest, Event>();
        }
    }
}
