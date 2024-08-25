using B1TestTask.DataAccess.Context;
using B1TestTask.DataAccess.Repositories.Implementations;
using B1TestTask.DataAccess.Repositories.Interfaces;
using B1TestTask.Domain.Repositories.Implementations;
using B1TestTask.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.Main.Data
{
    internal static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoryInDb(this IServiceCollection services) => services
            .AddTransient<IFileRepository>(options => new FileRepository(options.GetRequiredService<TaskDbContext>()))
            .AddTransient<IFileLineRepository>(options => new FileLineRepository(options.GetRequiredService<TaskDbContext>()))
            .AddTransient<IExelReportRepository>(options => new ExelReportRepository(options.GetRequiredService<TaskDbContext>()))
            .AddTransient<IExelRowRepository>(options => new ExelRowRepository(options.GetRequiredService<TaskDbContext>()))
            .AddTransient<IAccountClassRepository>(options => new AccountClassRepository(options.GetRequiredService<TaskDbContext>()))
        ;
    }
}
