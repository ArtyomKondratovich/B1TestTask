using B1TestTask.DataAccess.Repositories.Interfaces;
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
            .AddSingleton<IExelService>(options => 
            new ExelService(
                options.GetRequiredService<IExelReportRepository>(),
                options.GetRequiredService<IExelRowRepository>(),
                options.GetRequiredService<IAccountClassRepository>()
                ))
        ;
    }
}
