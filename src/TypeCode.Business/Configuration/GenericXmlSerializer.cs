using System.Globalization;
using System.Xml.Serialization;

namespace TypeCode.Business.Configuration;

public sealed class GenericXmlSerializer : IGenericXmlSerializer
{
	public string Serialize<T>(T objectToParse) where T : class, new()
	{
		if (objectToParse == null)
		{
			throw new XmlParserException("Unable to parse a object which is null.", new ArgumentNullException(nameof(objectToParse)));
		}

		var stringwriter = new StringWriter(new CultureInfo("de-CH"));
		var serializer = new XmlSerializer(typeof(T));
		try
		{
			serializer.Serialize(stringwriter, objectToParse);
		}
		catch (Exception e)
		{
			throw new XmlParserException($"Unable to serialize the object {objectToParse.GetType()}.", e);
		}

		return stringwriter.ToString();
	}

	public T? Deserialize<T>(string xmlTextToParse) where T : class, new()
	{
		if (string.IsNullOrEmpty(xmlTextToParse))
		{
			throw new XmlParserException("Invalid string input. Cannot parse an empty or null string.", new ArgumentException("xmlTestToParse"));

		}

		var stringReader = new StringReader(xmlTextToParse);
		var serializer = new XmlSerializer(typeof(T));
		try
		{
			return serializer.Deserialize(stringReader) as T;
		}
		catch (Exception e)
		{
			throw new XmlParserException($"Unable to convert to given string into the type {typeof(T)}. See inner exception for details.", e);
		}
	}
}