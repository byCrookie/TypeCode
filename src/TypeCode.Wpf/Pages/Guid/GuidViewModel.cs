using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Guid;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Guid;

public partial class GuidViewModel : ViewModelBase, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<GuidTypeCodeGeneratorParameter> _guidGenerator;

    public GuidViewModel(
        ITypeCodeGenerator<GuidTypeCodeGeneratorParameter> guidGenerator,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _guidGenerator = guidGenerator;

        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }
    
    public Task OnNavigatedToAsync(NavigationContext context)
    {
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
    }

    [RelayCommand]
    private async Task GenerateMultipleAsync()
    {
        var guids = new StringBuilder();
        for (var i = 0; i < 100; i++)
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