using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Extensions.List;
using Framework.Workflow;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business
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

        public async Task StartAsync()
        {
            ITypeCodeStrategy mode = null;

            var tasks = new List<Task>
            {
                Task.Run(InitializeTypes),
                Task.Run(async () => mode = await EvaluateModeAsync().ConfigureAwait(false))
            };

           await Task.WhenAll(tasks).ConfigureAwait(false);

           if (mode is not null)
           {
               var result = await mode.GenerateAsync().ConfigureAwait(false);
               Console.WriteLine(result);
           }

           Console.Read();
        }

        private async Task<ITypeCodeStrategy> EvaluateModeAsync()
        {
            var workflow = _workflowBuilder
                .Then(context => context.Modes, _ => _modeComposer.ComposeOrdered())
                .WriteLine(context => CreateInputMessage(context.Modes))
                .Then(context => context.Input, _ => Console.ReadLine())
                .While(context => !ModeExists(context.Modes, context), whileFlow => whileFlow
                    .WriteLine(_ => $@"{Cuts.Point()} Please select a valid mode")
                        .Then(context => context.Input, _ => Console.ReadLine())
                        .IfFlow(context => string.IsNullOrEmpty(context.Input), ifFlow => ifFlow
                            .WriteLine(_ => $@"{Cuts.Point()} Press enter to exit or space to continue")
                            .IfFlow(_ => Console.ReadKey().Key == ConsoleKey.Enter, ifFlowLeave => ifFlowLeave
                                .StopAsync()
                            )
                        )
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
                
            } else
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