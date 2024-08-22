using Microsoft.Extensions.DependencyInjection;

namespace B1TestTask.Main.ViewModels
{
    class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
