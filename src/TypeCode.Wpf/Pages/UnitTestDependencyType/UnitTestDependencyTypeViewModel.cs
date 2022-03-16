using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.UnitTestDependencyType;

public class UnitTestDependencyTypeViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> _unitTestDependencyTypeGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;

    public UnitTestDependencyTypeViewModel(
        ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> unitTestDependencyTypeGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter
    )
    {
        _unitTestDependencyTypeGenerator = unitTestDependencyTypeGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        GenerateCommand = new AsyncCommand(GenerateAsync);
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    private async Task GenerateAsync()
    {
        var inputNames = Input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
        var types = _typeProvider.TryGetByNames(inputNames).ToList();

        if (types.Count > 1)
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = true,
                Types = types
            };

            await _typeSelectionWizardStarter.StartAsync(typeSelectionParameter, selectedTypes =>
            {
                types = selectedTypes.ToList();
                return Task.CompletedTask;
            }, _ =>
            {
                types = new List<Type>();
                return Task.CompletedTask;
            }).ConfigureAwait(true);
        }
            
        var parameter = new UnitTestDependencyTypeGeneratorParameter
        {
            Types = types
        };
            
        var result = await _unitTestDependencyTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        Output = result;
    }
        
    public ICommand GenerateCommand { get; set; }
        
    public string? Input {
        get => Get<string?>();
        set => Set(value);
    }

    public string? Output {
        get => Get<string?>();
        private set => Set(value);
    }
}