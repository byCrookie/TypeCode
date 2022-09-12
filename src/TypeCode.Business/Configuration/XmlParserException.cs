namespace TypeCode.Business.Configuration;

public sealed class XmlParserException : Exception
{
	public XmlParserException(string message, Exception argumentException) : base(message, argumentException) { }
}