using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Persistance.Seeds;
using PersistanceLayer.DBContext;


namespace Persistance.RepoRegistration
{
    public static class RepositoryRegistration
    {

        public static void AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<API_LibraryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            m => m.MigrationsAssembly(typeof(API_LibraryContext).Assembly.FullName)));



            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ILibrosRepository, LibrosRepository>();
            services.AddScoped<IAutoresRepository, AutoresRepository>();
            services.AddScoped<IPrestamosRepository, PrestamosRepository>();

        }

        public static async Task EntityRunSeeds(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            try
            {
                var context = serviceProvider.GetRequiredService<API_LibraryContext>();

                await AutorSeed.SeedAsync(context);
                await LibroSeed.SeedAsync(context);
                await PrestamoSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                // Registrar el error usando ILogger si aplica
                throw;
            }
        }
    }
}
