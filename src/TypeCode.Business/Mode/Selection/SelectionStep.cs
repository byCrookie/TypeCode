using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;

namespace TypeCode.Business.Mode.Selection
{
    internal class SelectionStep<TContext, TOptions> :
        ISelectionStep<TContext, TOptions>
        where TContext : WorkflowBaseContext, ISelectionContext
    {
        private readonly IWorkflowBuilder<SelectionContext> _workflowBuilder;
        private SelectionStepOptions _options;

        public SelectionStep(IWorkflowBuilder<SelectionContext> workflowBuilder)
        {
            _workflowBuilder = workflowBuilder;
        }

        public async Task ExecuteAsync(TContext context)
        {
            var selectionContext = new SelectionContext();

            var workflow = _workflowBuilder
                .While(c => c.Selection == 0 || c.Selection > _options.Selections.Count, whileFlow => whileFlow
                    .WriteLine(_ => $@"{Cuts.Medium()}")
                    .WriteLine(_ => $@"{Cuts.Heading()} Select an option")
                    .WriteLine(_ => CreateSelectionMenu(_options.Selections))
                    .ReadLine(c => c.Input)
                    .IfFlow(c => string.IsNullOrEmpty(c.Input.Trim()), ifFlow => ifFlow
                        .WriteLine(_ => $@"{Cuts.Point()} Press enter to exit or space to continue")
                        .IfFlow(_ => Console.ReadKey().Key == ConsoleKey.Enter, ifFlowLeave => ifFlowLeave
                            .StopAsync(context)
                        )
                    )
                    .Then(c => c.Selection, c => Convert.ToInt16(c.Input.Trim()))
                    .If(c => c.Selection > _options.Selections.Count || c.Selection < 1, _ => Console.WriteLine($@"{Cuts.Point()} Option is not valid"))
                )
                .Build();

            var workflowContext = await workflow.RunAsync(selectionContext).ConfigureAwait(false);
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
            return Task.FromResult(context.ShouldExecute());
        }

        public void SetOptions(TOptions options)
        {
            _options = options as SelectionStepOptions;
        }
    }
}