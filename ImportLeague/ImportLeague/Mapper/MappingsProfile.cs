using AutoMapper;
using ImportLeague.Dtos;
using ImportLeague.Models;

namespace ImportLeague.WebApi.Mapper
{
    public class MappingsProfile : Profile
    {

        public MappingsProfile()
        {
            FootballDataMappings();

        }
        private void FootballDataMappings()
        {
            CreateMap<Competition, FootballDataCompetition>()
                .ReverseMap();

            CreateMap<FootballDataTeam, Team>()
                .ForMember(m => m.ExternalId, opt => opt.MapFrom(src =>src.Id ))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Player, FootballDataPlayer>()
                .ReverseMap();
        }
    }
}
