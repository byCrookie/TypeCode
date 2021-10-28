using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Extensions.List;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Mode;
using TypeCode.Console.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console
{
    internal class TypeCode : ITypeCode
    {
        private readonly IModeComposer _modeComposer;
        private readonly ITypeEvaluator _typeEvaluator;
        private readonly IWorkflowBuilder<TypeCodeContext> _workflowBuilder;
        private readonly ITypeProvider _typeProvider;

        public TypeCode(
            IModeComposer modeComposer,
            ITypeEvaluator typeEvaluator,
            IWorkflowBuilder<TypeCodeContext> workflowBuilder,
            ITypeProvider typeProvider
        )
        {
            _modeComposer = modeComposer;
            _typeEvaluator = typeEvaluator;
            _workflowBuilder = workflowBuilder;
            _typeProvider = typeProvider;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            ITypeCodeStrategy mode = null;

            var tasks = new List<Task>
            {
                Task.Run(InitializeTypes, cancellationToken),
                Task.Run(async () => mode = await EvaluateModeAsync().ConfigureAwait(false), cancellationToken)
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);

            if (mode is not null)
            {
                while (!mode.IsExit())
                {
                    var result = await mode.GenerateAsync().ConfigureAwait(false);
                    System.Console.WriteLine(result);
                    System.Console.Read();
                    mode = await EvaluateModeAsync().ConfigureAwait(false);
                }
            }

            System.Console.Read();
        }

        private async Task<ITypeCodeStrategy> EvaluateModeAsync()
        {
            var workflow = _workflowBuilder
                .Then(context => context.Modes, _ => _modeComposer.ComposeOrdered())
                .WriteLine(context => CreateInputMessage(context.Modes))
                .ReadLine(context => context.Input)
                .While(context => !ModeExists(context.Modes, context), whileFlow => whileFlow
                    .WriteLine(_ => $@"{Cuts.Point()} Please select a valid mode")
                    .ReadLine(context => context.Input)
                    .ThenAsync<IExitOrContinueStep<TypeCodeContext>>()
                )
                .Then(context => context.Mode, context => context.Modes
                    .SingleOrDefault(strategy => strategy
                        .IsResponsibleFor(context.Input)))
                .Build();

            var workflowContext = await workflow.RunAsync(new TypeCodeContext()).ConfigureAwait(false);
            return workflowContext.Mode;
        }

        private static bool ModeExists(IEnumerable<ITypeCodeStrategy> modes, TypeCodeContext context)
        {
            return modes
                .Where(mode => !mode.IsPlanned())
                .Select(mode => mode.Number())
                .Any(modeNumber => $"{modeNumber}" == context.Input.Trim());
        }

        private static string CreateInputMessage(IEnumerable<ITypeCodeStrategy> modes)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.AppendLine($@"{Cuts.Heading()} Modes");
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            modes.ForEach(mode => PrintModeOption(stringBuilder, mode));
            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.Append($@"{Cuts.Point()} Select a mode");
            return stringBuilder.ToString();
        }

        private static void PrintModeOption(StringBuilder stringBuilder, ITypeCodeStrategy mode)
        {
            if (mode.IsPlanned())
            {
                stringBuilder.AppendLine($@"{Cuts.Point()} (Planned) - {mode.Description()}");
            }
            else
            {
                if (mode.IsBeta())
                {
                    stringBuilder.AppendLine($@"{Cuts.Point()} {mode.Number()} - {mode.Description()} (Beta)");
                }
                else
                {
                    stringBuilder.AppendLine($@"{Cuts.Point()} {mode.Number()} - {mode.Description()}");
                }
            }
        }

        private void InitializeTypes()
        {
            var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
            _typeProvider.Initalize(configuration);
        }
    }
}