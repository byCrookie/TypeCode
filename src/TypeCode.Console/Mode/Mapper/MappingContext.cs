using System;
using System.Collections.Generic;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;
using Workflow;

namespace TypeCode.Console.Mode.Mapper
{
    internal class MappingContext : WorkflowBaseContext, IMultipleTypesSelectionContext, ISelectionContext
    {
        public string Input { get; set; }
        public IEnumerable<string> TypeNames { get; set; }
        public string MappingCode { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
        public IEnumerable<Type> FirstTypeNames { get; set; }
        public IEnumerable<Type> SecondTypeNames { get; set; }
        public MappingType SelectedFirstType { get; set; }
        public MappingType SelectedSecondType { get; set; }
        public short Selection { get; set; }
    }
}