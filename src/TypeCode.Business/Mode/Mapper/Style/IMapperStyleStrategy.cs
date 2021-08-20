namespace TypeCode.Business.Mode.Mapper.Style
{
    internal interface IMapperStyleStrategy
    {
        int Number();
        string Description();
        bool IsResponsibleFor(string style);
        string Generate(MappingContext context);
    }
}