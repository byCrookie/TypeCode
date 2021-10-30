using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Mapper
{
    public class MapperViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> _mapperGenerator;
        private readonly ITypeProvider _typeProvider;
        private MappingStyle _mappingStyle;

        public MapperViewModel(
            ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> mapperGenerator,
            ITypeProvider typeProvider
        )
        {
            _mapperGenerator = mapperGenerator;
            _typeProvider = typeProvider;
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
            var mapFromType = _typeProvider.TryGetByName(inputNames.FirstOrDefault()).FirstOrDefault();
            var mapToType = _typeProvider.TryGetByName(inputNames.LastOrDefault()).FirstOrDefault();

            if (mapFromType is not null && mapToType is not null)
            {
                var parameter = new MapperTypeCodeGeneratorParameter
                {
                    MapFrom = new MappingType(mapFromType),
                    MapTo = new MappingType(mapToType),
                    MappingStyle = _mappingStyle
                };
            
                var result = await _mapperGenerator.GenerateAsync(parameter);
                Output = result;
            }
        }

        public ICommand GenerateCommand { get; set; }
        public ICommand StyleCommand { get; set; }
        
        public string Input {
            get => Get<string>();
            set => Set(value);
        }

        public string Output {
            get => Get<string>();
            private set => Set(value);
        }
        
        public bool NewStyle {
            get => Get<bool>();
            private set => Set(value);
        }
        
        public bool ExistingStyle {
            get => Get<bool>();
            private set => Set(value);
        }
    }
}