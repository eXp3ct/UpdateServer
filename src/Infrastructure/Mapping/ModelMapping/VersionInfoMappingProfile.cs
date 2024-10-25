using AutoMapper;
using Domain.Dtos;
using Domain.Models;

namespace Infrastructure.Mapping.ModelMapping
{
    public class VersionInfoMappingProfile : Profile
    {
        public VersionInfoMappingProfile()
        {
            CreateMap<VersionInfoDto, VersionInfo>()
                .ForMember(x => x.ReleaseDate, opt => opt.MapFrom(v => DateTime.Now));
        }
    }
}
