using System.Text;
using TypeCode.Business.Format;
using Workflow;

namespace TypeCode.Console.Mode.MultipleTypes;

internal class MultipleTypeSelectionStep<TContext> :
    IMultipleTypeSelectionStep<TContext>
    where TContext : WorkflowBaseContext, IMultipleTypesSelectionContext
{
    private readonly IWorkflowBuilder<MultipleTypesSelectionContext> _workflowBuilder;

    public MultipleTypeSelectionStep(IWorkflowBuilder<MultipleTypesSelectionContext> workflowBuilder)
    {
        _workflowBuilder = workflowBuilder;
    }
        
    public async Task ExecuteAsync(TContext context)
    {
        var multipleContext = new MultipleTypesSelectionContext
        {
            SelectedTypes = context.SelectedTypes
        };
        context.MapTo(multipleContext);

        var workflow = _workflowBuilder
            .IfElseFlow(c => c.SelectedTypes.Count > 1,
                ifFlow => ifFlow
                    .WriteLine(_ => $@"{Cuts.Medium()}")
                    .WriteLine(_ => $@"{Cuts.Heading()} Multiple types found, select one")
                    .WriteLine(c => CreateTypeSelectionMenu(c.SelectedTypes.ToList()))
                    .ReadLine(c => c.Input)
                    .IfElse(c => !string.IsNullOrEmpty(c.Input),
                        c => c.SelectedType,
                        c => c.SelectedTypes[Convert.ToInt32(c.Input.Trim())],
                        c => c.SelectedType,
                        c => c.SelectedTypes[0]
                    ),
                elseFlow => elseFlow
                    .Then(c => c.SelectedType, c => c.SelectedTypes.First())
            )
            .Build();

        var workflowContext = await workflow.RunAsync(multipleContext).ConfigureAwait(false);
        workflowContext.MapTo(context);
        context.SelectedType = workflowContext.SelectedType;
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }

    private static string CreateTypeSelectionMenu(IReadOnlyCollection<Type> types)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($@"{Cuts.Medium()}");
        for (var index = 0; index < types.Count; index++)
        {
            stringBuilder.AppendLine($@"{Cuts.Point()} {index} - {types.ToList()[index].FullName}");
        }

        stringBuilder.Append($@"{Cuts.Medium()}");
        return stringBuilder.ToString();
    }
}