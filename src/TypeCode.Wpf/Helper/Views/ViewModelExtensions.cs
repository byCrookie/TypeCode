using TypeCode.Business.Format;

namespace TypeCode.Wpf.Helper.Views;

public static class ViewModelExtensions
{
    public static Type? GetViewType(this Type viewModelType)
    {
        var viewName = NameBuilder.GetNameWithoutGeneric(viewModelType)[..^"Model".Length];
        return Type.GetType($"{viewModelType.Namespace}.{viewName}");
    }
}