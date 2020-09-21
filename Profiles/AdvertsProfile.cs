using AutoMapper;
using Commander.Dtos;
using Commander.Models;

namespace Commander.Profiles
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