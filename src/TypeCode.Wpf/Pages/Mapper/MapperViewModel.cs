using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Mapper
{
    public class MapperViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> _mapperGenerator;
        private readonly ITypeProvider _typeProvider;
        private readonly IWizardNavigationService _wizardNavigationService;
        private MappingStyle _mappingStyle;

        public MapperViewModel(
            ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> mapperGenerator,
            ITypeProvider typeProvider,
            IWizardNavigationService wizardNavigationService
        )
        {
            _mapperGenerator = mapperGenerator;
            _typeProvider = typeProvider;
            _wizardNavigationService = wizardNavigationService;
        }

        public Task OnNavigatedToAsync(NavigationContext context)
        {
            GenerateCommand = new AsyncCommand(GenerateAsync);
            StyleCommand = new AsyncCommand<MappingStyle>(StyleAsync);
            NewStyle = true;
            ExistingStyle = false;
            return Task.CompletedTask;
        }

        private Task StyleAsync(MappingStyle style)
        {
            _mappingStyle = style;
            return Task.CompletedTask;
        }

        private async Task GenerateAsync()
        {
            var inputNames = Input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
            var types = _typeProvider.TryGetByName(inputNames.FirstOrDefault())
                .Union(_typeProvider.TryGetByName(inputNames.LastOrDefault()))
                .ToList();

            if (types.Any())
            {
                var navigationContext = new NavigationContext();
                navigationContext.AddParameter(new TypeSelectionParameter
                {
                    AllowMultiSelection = true,
                    Types = types
                });

                var selectionViewModel = await _wizardNavigationService
                    .OpenWizard(new WizardParameter<TypeSelectionViewModel>
                    {
                        FinishButtonText = "Select"
                    }, navigationContext);

                var parameter = new MapperTypeCodeGeneratorParameter
                {
                    MapFrom = new MappingType(selectionViewModel.SelectedTypes.First()),
                    MapTo = new MappingType(selectionViewModel.SelectedTypes.Last()),
                    MappingStyle = _mappingStyle
                };

                var result = await _mapperGenerator.GenerateAsync(parameter);
                Output = result;
            }
        }

        public ICommand GenerateCommand { get; set; }
        public ICommand StyleCommand { get; set; }

        public string Input
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Output
        {
            get => Get<string>();
            private set => Set(value);
        }

        public bool NewStyle
        {
            get => Get<bool>();
            private set => Set(value);
        }

        public bool ExistingStyle
        {
            get => Get<bool>();
            private set => Set(value);
        }
    }
}