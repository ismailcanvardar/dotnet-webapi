using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class AboutEmployeeProfile : Profile
    {
        public AboutEmployeeProfile()
        {
            CreateMap<AboutEmployeeCreateDto, AboutEmployee>();
        }
    }
}