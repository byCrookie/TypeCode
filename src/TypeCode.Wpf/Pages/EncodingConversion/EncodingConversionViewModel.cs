using System.Collections;
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

namespace TypeCode.Wpf.Pages.EncodingConversion;

public partial class EncodingConversionViewModel : ViewModelBase, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<EncodingTypeCodeGeneratorParameter> _composerTypeGenerator;

    public EncodingConversionViewModel(
        IOutputBoxViewModelFactory outputBoxViewModelFactory,
        ITypeCodeGenerator<EncodingTypeCodeGeneratorParameter> composerTypeGenerator
    )
    {
        _composerTypeGenerator = composerTypeGenerator;

        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        var encodings = GetEncodings();
        EncodingsFrom = encodings;
        EncodingsTo = encodings;

        EncodingFrom = Encoding.UTF8;
        EncodingTo = Encoding.ASCII;
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanConvert))]
    private Task ConvertAsync()
    {
        MainThread.BackgroundFireAndForgetAsync(async () =>
        {
            var result = await _composerTypeGenerator.GenerateAsync(new EncodingTypeCodeGeneratorParameter(Input!, EncodingFrom!, EncodingTo!)).ConfigureAwait(true);
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
    private Encoding? _encodingFrom;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    [Required]
    private Encoding? _encodingTo;

    [ObservableProperty]
    private IDictionary<Encoding, string>? _encodingsFrom;

    [ObservableProperty]
    private IDictionary<Encoding, string>? _encodingsTo;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;

    private static IDictionary<Encoding, string> GetEncodings()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        return Encoding.GetEncodings()
            .OrderBy(info => info.Name)
            .Select(info => new {Info = info, Encoding = info.GetEncoding()})
            .ToDictionary(encoding => encoding.Encoding, encoding => $"{encoding.Info.Name} {encoding.Encoding.EncodingName} {encoding.Info.DisplayName} {encoding.Info.CodePage}");
    }
}