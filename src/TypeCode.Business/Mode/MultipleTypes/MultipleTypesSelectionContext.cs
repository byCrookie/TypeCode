﻿using System;
using System.Collections.Generic;
using Framework.Workflow;

namespace TypeCode.Business.Mode.MultipleTypes
{
    internal class MultipleTypesSelectionContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {
        public List<Type> SelectedTypes { get; set; }
        public Type SelectedType { get; set; }
        public string Input { get; set; }
    }
}