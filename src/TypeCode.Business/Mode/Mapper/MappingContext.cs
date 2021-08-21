using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Workflow;
using TypeCode.Business.Mode.MultipleTypes;

namespace TypeCode.Business.Mode.Mapper
{
    internal class MappingContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {
        public string Input { get; set; }
        public IEnumerable<string> TypeNames { get; set; }
        public string MappingCode { get; set; }
        public string MappingStyle { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
        public IEnumerable<Type> FirstTypeNames { get; set; }
        public IEnumerable<Type> SecondTypeNames { get; set; }
        public MappingType SelectedFirstType { get; set; }
        public MappingType SelectedSecondType { get; set; }
    }
}