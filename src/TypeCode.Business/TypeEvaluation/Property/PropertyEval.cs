using System;
using System.Collections;
using System.Reflection;

namespace TypeCode.Business.TypeEvaluation.Property
{
    internal class PropertyEval
    {
        public static bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }

            return typeInfo.IsPrimitive
                   || typeInfo.IsEnum
                   || type == typeof(string)
                   || type == typeof(DateTime)
                   || type == typeof(decimal);
        }

        public static string GetNestedPropertyTypeNameIfAvailable(Type type)
        {
            var dataType = GetNestedTypeIfAvailable(type).Name;

            switch (dataType)
            {
                case "Int64":
                    return "long";
                case "Int32":
                    return "int";
                case "Int16":
                    return "short";
                case "String":
                    return "string";
                case "Boolean":
                    return "bool";
                default:
                    return dataType;
            }
        }

        public static bool IsList(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type)
                   && type != typeof(string);
        }

        public static Type GetNestedTypeIfAvailable(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsGenericType ? type.GetGenericArguments()[0] : type;
        }
    }
}