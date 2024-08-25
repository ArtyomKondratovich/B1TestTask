using B1TestTask.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.Main.Data
{
    internal static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) => services
            .AddDbContext<FirstTaskDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("postgre"));
            });
    }
}
