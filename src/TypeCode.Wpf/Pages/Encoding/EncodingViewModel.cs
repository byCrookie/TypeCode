using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.EncodingConversion;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Encoding;

public sealed partial class EncodingViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;
    private readonly ITypeCodeGenerator<EncodingTypeCodeGeneratorParameter> _composerTypeGenerator;

    public EncodingViewModel(
        IOutputBoxViewModelFactory outputBoxViewModelFactory,
        ITypeCodeGenerator<EncodingTypeCodeGeneratorParameter> composerTypeGenerator
    )
    {
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
        _composerTypeGenerator = composerTypeGenerator;
    }

    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();

        var encodings = GetEncodings();
        EncodingsFrom = new ObservableCollection<EncodingItemViewModel>(encodings);
        EncodingsTo = new ObservableCollection<EncodingItemViewModel>(encodings);

        var encodingFrom = encodings.Single(encoding => encoding.Encoding.Equals(System.Text.Encoding.UTF8));
        EncodingFrom = encodingFrom;
        var encodingTo = encodings.Single(encoding => encoding.Encoding.Equals(System.Text.Encoding.ASCII));
        EncodingTo = encodingTo;
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanConvert))]
    private Task ConvertAsync()
    {
        MainThread.BackgroundFireAndForgetAsync(async () =>
        {
            var result = await _composerTypeGenerator.GenerateAsync(new EncodingTypeCodeGeneratorParameter(Input!, EncodingFrom!.Encoding, EncodingTo!.Encoding)).ConfigureAwait(true);
            OutputBoxViewModel?.SetOutput(result);
        }, DispatcherPriority.Normal);
        return Task.CompletedTask;
    }

    private bool CanConvert()
    {
        return !HasErrors
               && !string.IsNullOrEmpty(Input)
               && EncodingFrom is not null
               && EncodingTo is not null;
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    [Required]
    private string? _input;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    [Required]
    private EncodingItemViewModel? _encodingFrom;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    [Required]
    private EncodingItemViewModel? _encodingTo;

    [ObservableProperty]
    private ObservableCollection<EncodingItemViewModel>? _encodingsFrom;

    [ObservableProperty]
    private ObservableCollection<EncodingItemViewModel>? _encodingsTo;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;

    private static List<EncodingItemViewModel> GetEncodings()
    {
        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        return System.Text.Encoding.GetEncodings()
            .OrderBy(info => info.Name)
            .Select(info => new { Info = info, Encoding = info.GetEncoding() })
            .Select(encoding => new EncodingItemViewModel(encoding.Encoding, encoding.Info))
            .ToList();
    }
}