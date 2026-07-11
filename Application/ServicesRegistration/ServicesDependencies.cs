using Application.Interfaces;
using Application.MapperProfile;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServicesRegistration
{
    public static class ServicesDependencies
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(MapProfile).Assembly);
            services.AddScoped<ILibrosService, LibrosService>();
            services.AddScoped<IPrestamoServices, PrestamoServices>();

        }
    }
}
