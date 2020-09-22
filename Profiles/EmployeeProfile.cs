using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeReadDto>();
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<Employee, EmployeeLoginDto>();
        }
    }
}