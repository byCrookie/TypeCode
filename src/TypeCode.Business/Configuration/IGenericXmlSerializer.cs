namespace TypeCode.Business.Configuration
{
    public interface IGenericXmlSerializer
    {
        string Serialize<T>(T objectToParse) where T : class, new();
    }
}