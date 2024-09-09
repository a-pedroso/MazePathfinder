namespace MazePathfinder.Infrastructure;

using MazePathfinder.Domain.Maze;
using MazePathfinder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public class InfrastructureOptions
    {
        public string? DatabaseConnectionString { get; set; }
        public bool? UseInMemoryDatabase { get; set; }
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<InfrastructureOptions> options)
    {
        ArgumentNullException.ThrowIfNull(services);

        InfrastructureOptions infrastructureOptions = new();
        options(infrastructureOptions);

        if (infrastructureOptions.UseInMemoryDatabase.HasValue && infrastructureOptions.UseInMemoryDatabase.Value)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"));
        }
        else
        {
            // For future usage once we start using a database

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        infrastructureOptions.DatabaseConnectionString.Value,
            //        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        #region Repositories
        services.AddScoped<IMazeRepository, MazeRepository>();
        #endregion

        return services;
    }
}
