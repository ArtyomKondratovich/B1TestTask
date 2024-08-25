using B1TestTask.Main.Services;
using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.Main.ViewModels
{
    public static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
        ;
    }
}
