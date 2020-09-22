using System;
using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class PickedEmloyeesProfile : Profile
    {
        public PickedEmloyeesProfile()
        {
            CreateMap<PickedEmployeeCreateDto, PickedEmployee>();
        }
    }
}
