using System;
using System.Collections.Generic;

namespace TypeCode.Business.Mode.Composer
{
    public class ComposerTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
    {
        public Type Type { get; set; }
        public List<Type> Interfaces { get; set; }
    }
}