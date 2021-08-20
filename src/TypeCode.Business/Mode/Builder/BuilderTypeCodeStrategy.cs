using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.Mode.MultipleTypes;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Builder
{
    internal class BuilderTypeCodeStrategy : IBuilderTypeCodeStrategy
    {
        private readonly IWorkflowBuilder<BuilderContext> _workflowBuilder;
        private readonly ITypeProvider _typeProvider;

        public BuilderTypeCodeStrategy(
            IWorkflowBuilder<BuilderContext> workflowBuilder,
            ITypeProvider typeProvider)
        {
            _workflowBuilder = workflowBuilder;
            _typeProvider = typeProvider;
        }

        public int Number()
        {
            return 5;
        }

        public string Description()
        {
            return "Builder Generation";
        }

        public bool IsPlanned()
        {
            return false;
        }

        public bool IsBeta()
        {
            return false;
        }

        public bool IsResponsibleFor(string mode)
        {
            return mode == $"{Number()}" && !IsPlanned();
        }

        public async Task<string> GenerateAsync()
        {
            var workflow = _workflowBuilder
                .WriteLine(context => $@"{Cuts.Point()} Input type")
                .Then(context => context.TypeName, context => Console.ReadLine())
                .While(context => !_typeProvider.HasByName(context.TypeName.Trim()), whileFlow => whileFlow
                    .WriteLine(context => $@"{Cuts.Point()} Type not found")
                    .WriteLine(context => $@"{Cuts.Point()} Please input input type")
                    .Then(context => context.TypeName, context => Console.ReadLine())
                    .IfFlow(context => string.IsNullOrEmpty(context.TypeName), ifFlow => ifFlow
                        .WriteLine(context => $@"{Cuts.Point()} Press enter to exit or space to continue")
                        .IfFlow(context => Console.ReadKey().Key == ConsoleKey.Enter, ifFlowLeave => ifFlowLeave
                            .StopAsync()
                        )
                    )
                )
                .Then(context => context.SelectedTypes, context => _typeProvider.TryGetByName(context.TypeName.Trim()).ToList())
                .ThenAsync<IMultipleTypeSelectionStep<BuilderContext>>()
                .Stop(c => !c.SelectedType.IsClass, c => Console.WriteLine($@"{Cuts.Point()} Type has to be a class"))
                .Then(context => context.BuilderCode, context => GenerateBuilderCode(context.SelectedType))
                .Build();

            var workflowContext = await workflow.RunAsync(new BuilderContext()).ConfigureAwait(false);
            return workflowContext.BuilderCode;
        }

        private static string GenerateBuilderCode(Type type)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($@"{Cuts.Long()}");
            stringBuilder.AppendLine($@"{Cuts.Heading()} {type.FullName}");

            var typeName = NameBuilder.GetNameWithoutGeneric(type);
            var classFieldName = NameBuilder.FieldNameFromClass(type);
            var classVariableName = NameBuilder.VariableNameFromClass(type);

            stringBuilder.AppendLine($"public class {typeName}Builder");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}private static {typeName} {classFieldName};");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder()");

            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName} = new {typeName}();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");

            foreach (var property in type.GetProperties().Where(p => p.GetSetMethod() != null || p.Name == "Id"))
            {
                stringBuilder.AppendLine();
                CreatePropertyMethod(stringBuilder, typeName, classFieldName, classVariableName, property);
            }

            stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName} Build()");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var {classVariableName} = {classFieldName};");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName} = new {typeName}();");
            stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return {classVariableName};");
            stringBuilder.AppendLine($@"{Cuts.Tab()}}}");

            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private static void CreatePropertyMethod(StringBuilder stringBuilder, string typeName, string classFieldName, string classVariableName, PropertyInfo property)
        {
            var propertyName = property.Name;
            var dataType = PropertyEval.GetNestedPropertyTypeNameIfAvailable(property.PropertyType);

            if (PropertyEval.IsSimple(property.PropertyType) || property.PropertyType.IsEnum)
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder {propertyName}({dataType} value)");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName} = value;");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
                stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
            }
            else if (PropertyEval.IsList(property.PropertyType) && PropertyEval.IsSimple(PropertyEval.GetNestedTypeIfAvailable(property.PropertyType)))
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder Add{propertyName}({dataType} value)");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName}.Add(value);");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
                stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
            } 
            else if (PropertyEval.IsList(property.PropertyType) && !PropertyEval.IsSimple(PropertyEval.GetNestedTypeIfAvailable(property.PropertyType)))
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder Add{propertyName}(Action<{propertyName}Builder> configure)");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var builder = new {propertyName}Builder();");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}configure(builder);");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName}.Add(builder.Build());");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
                stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
            }
            else
            {
                stringBuilder.AppendLine($@"{Cuts.Tab()}public {typeName}Builder {propertyName}(Action<{propertyName}Builder> configure)");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{{");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}var builder = new {propertyName}Builder();");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}configure(builder);");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}{classFieldName}.{propertyName} = builder.Build();");
                stringBuilder.AppendLine($@"{Cuts.Tab()}{Cuts.Tab()}return this;");
                stringBuilder.AppendLine($@"{Cuts.Tab()}}}");
            }
        }
    }
}