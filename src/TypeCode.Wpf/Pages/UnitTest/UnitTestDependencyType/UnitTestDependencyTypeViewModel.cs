﻿using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyType;

public sealed partial class UnitTestDependencyTypeViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> _unitTestDependencyTypeGenerator;
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;
    private readonly IInputBoxViewModelFactory _inputBoxViewModelFactory;
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;

    public UnitTestDependencyTypeViewModel(
        ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> unitTestDependencyTypeGenerator,
        ILazyTypeProviderFactory lazyTypeProviderFactory,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _unitTestDependencyTypeGenerator = unitTestDependencyTypeGenerator;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;
        _inputBoxViewModelFactory = inputBoxViewModelFactory;
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
    }
    
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input type name. Multiple type names can be seperated by using ','."
        };

        InputBoxViewModel = _inputBoxViewModelFactory.Create(parameter);
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();
        return Task.CompletedTask;
    }

    private async Task GenerateAsync(bool regex, string? input)
    {
        var inputNames = input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
        var typeProvider = await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByNames(inputNames, new TypeEvaluationOptions { Regex = regex }).ToList();

        if (types.Count > 1)
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = true,
                Types = types
            };

            await _typeSelectionWizardStarter
                .StartAsync(typeSelectionParameter, viewModel => GenerateOutputAsync(viewModel.SelectedTypes.ToList()))
                .ConfigureAwait(true);
        }
        else
        {
            await GenerateOutputAsync(types).ConfigureAwait(true);
        }
    }

    private async Task GenerateOutputAsync(List<Type> types)
    {
        var parameter = new UnitTestDependencyTypeGeneratorParameter
        {
            Types = types
        };

        var result = await _unitTestDependencyTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        OutputBoxViewModel?.SetOutput(result);
    }

    [ObservableProperty]
    [ChildViewModel]
    private InputBoxViewModel? _inputBoxViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}