using System.Reflection;

namespace TypeCode.Business.Mode.Mapper;

public class TypeCodeProperty
{
    public TypeCodeProperty(string name, PropertyInfo prop)
    {
        Name = name;
        Prop = prop;
    }
    
    public string Name { get; }
    public PropertyInfo Prop { get; }
}