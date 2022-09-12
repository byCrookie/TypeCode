using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Guid;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Guid;

public sealed partial class GuidViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly ITypeCodeGenerator<GuidTypeCodeGeneratorParameter> _guidGenerator;
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;

    public GuidViewModel(
        ITypeCodeGenerator<GuidTypeCodeGeneratorParameter> guidGenerator,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _guidGenerator = guidGenerator;
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
    }

    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();
        GuidFormat = GuidFormat.D;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task GuidFormatAsync(GuidFormat format)
    {
        GuidFormat = format;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task GenerateSingleAsync()
    {
        var guid = await _guidGenerator.GenerateAsync(new GuidTypeCodeGeneratorParameter(_guidFormat)).ConfigureAwait(true);

        if (guid is not null)
        {
            Clipboard.SetText(guid);
        }

        OutputBoxViewModel?.SetOutput(guid);
        await Task.Delay(TimeSpan.FromSeconds(0.25)).ConfigureAwait(true);
    }

    [RelayCommand]
    private async Task GenerateMultipleAsync()
    {
        var guids = new StringBuilder();
        for (var i = 0; i < 25; i++)
        {
            var guid = await _guidGenerator.GenerateAsync(new GuidTypeCodeGeneratorParameter(_guidFormat)).ConfigureAwait(true);
            guids.AppendLine(guid);
        }

        OutputBoxViewModel?.SetOutput(guids.ToString());
    }

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;

    [ObservableProperty]
    private GuidFormat _guidFormat;
}