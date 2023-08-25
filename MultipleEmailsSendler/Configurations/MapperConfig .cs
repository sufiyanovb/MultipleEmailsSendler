using AutoMapper;
using MultipleEmailsSendler.Models;
using MultipleEmailsSendler.Models.Dto;

namespace MultipleEmailsSendler.Migrations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Emails, EmailsDTO>().ReverseMap();
            CreateMap<Recipients, RecipientsDTO>().ReverseMap();
        }
    }
}
