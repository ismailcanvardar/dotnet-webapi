using AutoMapper;
using Commander.Dtos;
using Commander.Models;

namespace Commander.Profiles
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