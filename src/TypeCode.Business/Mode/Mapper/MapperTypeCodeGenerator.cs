using System.Linq;
using System.Threading.Tasks;
using TypeCode.Business.Mode.Mapper.Style;

namespace TypeCode.Business.Mode.Mapper;

internal class MapperTypeCodeGenerator : IMapperTypeCodeGenerator
{
    private readonly IMapperStyleComposer _mapperStyleComposer;

    public MapperTypeCodeGenerator(IMapperStyleComposer mapperStyleComposer)
    {
        _mapperStyleComposer = mapperStyleComposer;
    }

    public Task<string> GenerateAsync(MapperTypeCodeGeneratorParameter parameter)
    {
        if (parameter.MapFrom is not null && parameter.MapTo is not null)
        {
            return Task.FromResult(GenerateMappingCode(parameter));
        }

        return Task.FromResult<string>(null);
    }

    private string GenerateMappingCode(MapperTypeCodeGeneratorParameter parameter)
    {
        var styles = _mapperStyleComposer.Compose();
        var selectedStyle = styles.SingleOrDefault(style => style.IsResponsibleFor(parameter.MappingStyle));
        return selectedStyle?.Generate(parameter);
    }
}