using TypeCode.Business.Mode.Mapper;

namespace TypeCode.Business.StringProximity;

internal class ProximityProperty
{
    public ProximityProperty(TypeCodeProperty property, double jaro)
    {
        Property = property;
        Jaro = jaro;
    }
    
    public TypeCodeProperty Property { get; set; }
    public double Jaro { get; set; }
}