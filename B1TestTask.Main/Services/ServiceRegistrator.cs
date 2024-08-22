using B1TestTask.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.Main.Services
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddService(this IServiceCollection services) => services
            .AddSingleton<IFilesService>(options => 
            new FilesService(
                options.GetRequiredService<IFileLineRepository>(),
                options.GetRequiredService<IFileRepository>()
            ))
        ;
    }
}
