using AutoMapper;
using Domain.Dtos;
using Domain.Models;

namespace Infrastructure.Mapping.ModelMapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<ApplicationDto, Application>();
        }
    }
}