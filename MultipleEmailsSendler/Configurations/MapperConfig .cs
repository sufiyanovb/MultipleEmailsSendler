using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using MultipleEmailsSendler.Models;
using MultipleEmailsSendler.Models.Dto;
using System.Collections.Generic;

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
