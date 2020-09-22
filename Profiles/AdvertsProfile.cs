using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class AdvertsProfile : Profile
    {
        public AdvertsProfile()
        {
            CreateMap<AdvertCreateDto, Advert>();
            CreateMap<Advert, AdvertReadDto>();
        }
    }
}