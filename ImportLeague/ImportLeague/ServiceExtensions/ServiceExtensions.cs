using AutoMapper;
using ImportLeague.WebApi.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace ImportLeague.WebApi.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
