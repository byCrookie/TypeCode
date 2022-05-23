using TypeCode.Business.Mode.Mapper.Style;

namespace TypeCode.Business.Mode.Mapper;

public class MapperTypeCodeGenerator : IMapperTypeCodeGenerator
{
    private readonly IMapperStyleComposer _mapperStyleComposer;

    public MapperTypeCodeGenerator(IMapperStyleComposer mapperStyleComposer)
    {
        _mapperStyleComposer = mapperStyleComposer;
    }

    public Task<string?> GenerateAsync(MapperTypeCodeGeneratorParameter parameter)
    {
        return Task.FromResult(GenerateMappingCode(parameter));
    }

    private string? GenerateMappingCode(MapperTypeCodeGeneratorParameter parameter)
    {
        var styles = _mapperStyleComposer.Compose();
        var selectedStyle = styles.SingleOrDefault(style => style.IsResponsibleFor(parameter.MappingStyle));
        return selectedStyle?.Generate(parameter);
    }
}