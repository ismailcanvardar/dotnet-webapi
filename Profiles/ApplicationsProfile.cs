using System;
using AutoMapper;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;

namespace KariyerAppApi.Profiles
{
    public class ApplicationsProfile : Profile
    {
        public ApplicationsProfile()
        {
            CreateMap<ApplicationCreateDto, Application>();
        }
    }
}
