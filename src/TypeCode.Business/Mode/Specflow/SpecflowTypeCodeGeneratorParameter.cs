using System;
using System.Collections.Generic;

namespace TypeCode.Business.Mode.Specflow
{
    public class SpecflowTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
    {
        public List<Type> Types { get; set; }
    }
}