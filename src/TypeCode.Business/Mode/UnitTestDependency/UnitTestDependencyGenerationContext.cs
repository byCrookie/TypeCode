using System;
using System.Collections.Generic;
using System.Reflection;
using Framework.Workflow;
using TypeCode.Business.Mode.MultipleTypes;

namespace TypeCode.Business.Mode.UnitTestDependency
{
    internal class UnitTestDependencyGenerationContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {
        public ConstructorInfo[] EvaluatedConstructors { get; set; }
        public ConstructorInfo EvaluatedConstructor { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public string DependencyCode { get; set; }
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
    }
}