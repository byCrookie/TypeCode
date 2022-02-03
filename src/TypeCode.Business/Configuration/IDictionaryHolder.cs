namespace TypeCode.Business.Configuration;

internal interface IDictionaryHolder
{
    IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
    IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
}