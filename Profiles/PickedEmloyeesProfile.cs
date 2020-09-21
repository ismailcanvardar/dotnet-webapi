using System;
using AutoMapper;
using Commander.Dtos;
using Commander.Models;

namespace Commander.Profiles
{
    public class PickedEmloyeesProfile : Profile
    {
        public PickedEmloyeesProfile()
        {
            CreateMap<PickedEmployeeCreateDto, PickedEmployee>();
        }
    }
}
