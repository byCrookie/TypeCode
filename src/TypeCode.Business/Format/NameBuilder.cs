using System;
using System.Linq;

namespace TypeCode.Business.Format
{
    public class NameBuilder
    {
        public static string VariableNameFromInterface(Type @interface)
        {
            var name = GetNameWithoutGeneric(@interface).Remove(0, 1);
            return $"{VariablePrefix(name)}{name.Remove(0, 1)}";
        }
        
        public static string VariableNameFromClass(Type @class)
        {
            var name = GetNameWithoutGeneric(@class);
            return $"{VariablePrefix(name)}{name.Remove(0, 1)}";
        }

        private static string VariablePrefix(string name)
        {
            return $"{name.First().ToString().ToLower()}";
        }

        public static string FieldNameFromInterface(Type @interface)
        {
            var name = GetNameWithoutGeneric(@interface).Remove(0, 1);
            return $"{FieldPrefix(name)}{name.Remove(0, 1)}";
        }
        
        public static string FieldNameFromClass(Type @class)
        {
            var name = GetNameWithoutGeneric(@class);
            return $"{FieldPrefix(name)}{name.Remove(0, 1)}";
        }

        private static string FieldPrefix(string name)
        {
            return $"_{name.First().ToString().ToLower()}";
        }

        public static string GetInterfaceName(Type @interface)
        {
            if (@interface.GetGenericArguments().Any())
            {
                var genericNames = @interface.GetGenericArguments().Select(arg => arg.Name);
                var parameterName = @interface.Name.Remove(@interface.Name.IndexOf("`", StringComparison.Ordinal), 2);
                return $"{parameterName}<{string.Join(",", genericNames)}>";
            }

            return @interface.Name;
        }

        public static string GetNameWithoutGeneric(Type type)
        {
            return type.Name.Contains("`") ? type.Name.Remove(type.Name.IndexOf("`", StringComparison.Ordinal), 2) : type.Name;
        }
    }
}