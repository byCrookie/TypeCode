using System.Text;
using TypeCode.Business.Format;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Selection;

internal class SelectionStep<TContext, TOptions> :
    ISelectionStep<TContext, TOptions>
    where TContext : WorkflowBaseContext, ISelectionContext
{
    private readonly IWorkflowBuilder<SelectionContext> _workflowBuilder;
    private Lazy<SelectionStepOptions>? _options;

    public SelectionStep(IWorkflowBuilder<SelectionContext> workflowBuilder)
    {
        _workflowBuilder = workflowBuilder;
    }

    public async Task ExecuteAsync(TContext context)
    {
        var selectionContext = new SelectionContext();
        context.MapTo(selectionContext);

        var workflow = _workflowBuilder
            .While(c => string.IsNullOrEmpty(c.Input) || c.Selection == 0 || c.Selection > _options!.Value.Selections.Count, whileFlow => whileFlow
                .WriteLine(_ => $@"{Cuts.Medium()}")
                .WriteLine(_ => $@"{Cuts.Heading()} Select an option")
                .WriteLine(_ => CreateSelectionMenu(_options!.Value.Selections))
                .ReadLine(c => c.Input)
                .ThenAsync<IExitOrContinueStep<SelectionContext>>()
                .IfFlow(c => short.TryParse(c.Input?.Trim(), out _), ifFlow => ifFlow
                    .Then(c => c.Selection, c => Convert.ToInt16(c.Input?.Trim()))
                    .If(c => c.Selection > _options!.Value.Selections.Count || c.Selection < 1, _ => System.Console.WriteLine($@"{Cuts.Point()} Option is not valid"))
                )
            )
            .Build();

        var workflowContext = await workflow.RunAsync(selectionContext).ConfigureAwait(false);
        workflowContext.MapTo(context);
        context.Selection = workflowContext.Selection;
    }

    private static string CreateSelectionMenu(IReadOnlyList<string> selections)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($@"{Cuts.Medium()}");
        for (var index = 0; index < selections.Count; index++)
        {
            stringBuilder.AppendLine($@"{Cuts.Point()} {index + 1} - {selections[index]}");
        }

        stringBuilder.Append($@"{Cuts.Medium()}");
        return stringBuilder.ToString();
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }

    public void SetOptions(Lazy<TOptions> options)
    {
        _options = options as Lazy<SelectionStepOptions>;
    }
}