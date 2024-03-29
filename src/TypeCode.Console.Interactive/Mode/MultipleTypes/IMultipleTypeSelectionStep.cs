﻿using Workflow;

namespace TypeCode.Console.Interactive.Mode.MultipleTypes;

internal interface IMultipleTypeSelectionStep<in TContext> : 
    IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IMultipleTypesSelectionContext
{

}