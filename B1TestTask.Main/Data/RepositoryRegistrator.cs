using B1TestTask.DataAccess.Context;
using B1TestTask.Domain.Repositories.Impllementations;
using B1TestTask.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.Main.Data
{
    internal static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoryInDb(this IServiceCollection services) => services
            .AddTransient<IFileRepository>(options => new FileRepository(options.GetRequiredService<FirstTaskDbContext>()))
            .AddTransient<IFileLineRepository>(options => new FileLineRepository(options.GetRequiredService<FirstTaskDbContext>()))
        ;
    }
}
