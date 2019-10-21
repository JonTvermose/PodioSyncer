using AutoMapper;
using PodioSyncer.Data.Models;
using PodioSyncer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodioSyncer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Source, Destination>()
            //    .ForMember(dest => dest.property, opt => opt.MapFrom(src => src.prop));


            CreateMap<PodioApp, PodioAppViewModel>()
                .ReverseMap();
        }
    }
}
