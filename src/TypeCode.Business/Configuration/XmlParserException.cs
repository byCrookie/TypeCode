using System;

namespace TypeCode.Business.Configuration;

public class XmlParserException : Exception
{
	public XmlParserException(string message, Exception argumentException) : base(message, argumentException) { }
}