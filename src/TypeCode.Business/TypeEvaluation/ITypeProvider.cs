using System;
using System.Collections.Generic;
using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation
{
    internal interface ITypeProvider
    {
        void Initalize(TypeCodeConfiguration configuration);
        bool HasByName(string name);
        IEnumerable<Type> TryGetByName(string name);
        IEnumerable<Type> TryGetByNames(IEnumerable<string> names);
        IEnumerable<Type> TryGetTypesByCondition(Func<Type, bool> condition);
    }
}