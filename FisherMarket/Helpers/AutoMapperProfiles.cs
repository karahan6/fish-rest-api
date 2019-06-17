using AutoMapper;
using FisherMarket.DTOs;
using FisherMarket.Models;
using System;

namespace FisherMarket.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Fish, FishDto>()
                .ForMember(dest => dest.Base64Photo, opt => {
                    opt.MapFrom(src => Convert.ToBase64String(src.Photo));
                });
            CreateMap<FishDto, Fish>()
                .ForMember(dest => dest.Photo, opt => {
                    opt.MapFrom(src => Convert.FromBase64String(src.Base64Photo));
                });
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
        
    }
}
