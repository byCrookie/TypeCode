﻿using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Composer;

public sealed partial class ComposerViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerTypeGenerator;
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;
    private readonly IInputBoxViewModelFactory _inputBoxViewModelFactory;
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;

    public ComposerViewModel(
        ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerTypeGenerator,
        ILazyTypeProviderFactory lazyTypeProviderFactory,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _composerTypeGenerator = composerTypeGenerator;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;
        _inputBoxViewModelFactory = inputBoxViewModelFactory;
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
    }

    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input type name."
        };

        InputBoxViewModel = _inputBoxViewModelFactory.Create(parameter);
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();
        return Task.CompletedTask;
    }

    private async Task GenerateAsync(bool regex, string? input)
    {
        var typeProvider = await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions { Regex = regex }).ToList();

        if (types.Count > 1)
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = false,
                Types = types
            };

            await _typeSelectionWizardStarter.StartAsync(typeSelectionParameter, async viewModel =>
            {
                var selectedType = viewModel.SelectedTypes.Single();
                await GenerateAsync(selectedType).ConfigureAwait(true);
            }).ConfigureAwait(true);
        }
        else
        {
            await GenerateAsync(types.FirstOrDefault()).ConfigureAwait(true);
        }
    }

    private async Task GenerateAsync(Type? selectedType)
    {
        if (selectedType is not null)
        {
            var typeProvider = await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
            var parameter = new ComposerTypeCodeGeneratorParameter();
            parameter.ComposerTypes.Add(new ComposerType(
                selectedType,
                typeProvider.TryGetTypesByCondition(typ => typ.GetInterface(selectedType.Name) != null).ToList()
            ));
            var result = await _composerTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
            OutputBoxViewModel?.SetOutput(result);
        }
    }

    [ObservableProperty]
    [ChildViewModel]
    private InputBoxViewModel? _inputBoxViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}