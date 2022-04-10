namespace TypeCode.Business.Configuration;

public interface IConfigurationMapper
{
    TypeCodeConfiguration MapToConfiguration(XmlTypeCodeConfiguration xmlConfiguration);
    XmlTypeCodeConfiguration MapToXml(TypeCodeConfiguration configuration);
}