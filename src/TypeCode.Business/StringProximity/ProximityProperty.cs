namespace TypeCode.Business.StringProximity;

internal class ProximityProperty
{
    public ProximityProperty(string property, double jaro)
    {
        Property = property;
        Jaro = jaro;
    }
    
    public string Property { get; set; }
    public double Jaro { get; set; }
}