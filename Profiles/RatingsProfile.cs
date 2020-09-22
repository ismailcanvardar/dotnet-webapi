using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class RatingsProfile : Profile
    {
        public RatingsProfile()
        {
            CreateMap<RatingCreateDto, Rating>();
        }
    }
}