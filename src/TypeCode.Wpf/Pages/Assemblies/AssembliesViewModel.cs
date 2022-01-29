using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeCode.Business.Bootstrapping;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Assemblies;

public class AssemblyViewModel : Reactive, IAsyncNavigatedTo
{
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        var configuration = AssemblyLoadProvider.GetConfiguration();
        LoadedAssemblies = configuration.AssemblyRoot
            .OrderBy(r => r.Priority)
            .SelectMany(r => r.AssemblyGroup)
            .SelectMany(r => r.PriorityAssemblyList)
            .OrderBy(r => r.Priority)
            .Select(r => $"{r.Priority} {r.Message}")
            .ToList();
            
        return Task.CompletedTask;
    }

    public List<string> LoadedAssemblies {
        get => Get<List<string>>();
        private set => Set(value);
    }
}