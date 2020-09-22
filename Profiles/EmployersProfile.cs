using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class EmployersProfile : Profile
    {
        public EmployersProfile()
        {
            CreateMap<Employer, EmployerReadDto>();
            CreateMap<EmployerCreateDto, Employer>();
            CreateMap<Employer, EmployerLoginDto>();
        }
    }
}