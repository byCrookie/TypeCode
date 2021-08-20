using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Extensions.Number;
using Framework.Workflow;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Business.TypeEvaluation.Property;

namespace TypeCode.Business.Mode.Specflow
{
    internal class SpecflowTypeCodeStrategy : ISpecflowTypeCodeStrategy
    {
        private readonly IWorkflowBuilder<SpecflowContext> _workflowBuilder;
        private readonly ITypeProvider _typeProvider;

        public SpecflowTypeCodeStrategy(IWorkflowBuilder<SpecflowContext> workflowBuilder, ITypeProvider typeProvider)
        {
            _workflowBuilder = workflowBuilder;
            _typeProvider = typeProvider;
        }

        public int Number()
        {
            return 1;
        }

        public string Description()
        {
            return "Specflow Table Generation";
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
                .WriteLine(_ => $@"{Cuts.Point()} Input types seperated by ,")
                .Then(context => context.Input = Console.ReadLine())
                .While(context => string.IsNullOrEmpty(context.Input) || !context.Input.Split(',').Select(split => split.Trim()).Any(), step => step
                    .WriteLine(_ => $@"{Cuts.Point()} Please input types seperated by ,")
                    .Then(context => context.Input = Console.ReadLine())
                    .IfFlow(context => string.IsNullOrEmpty(context.Input), ifFlow => ifFlow
                        .WriteLine(_ => $@"{Cuts.Point()} Press enter to exit or space to continue")
                        .IfFlow(_ => Console.ReadKey().Key == ConsoleKey.Enter, ifFlowLeave => ifFlowLeave
                            .StopAsync()
                        )
                    )
                )
                .Then(context => context.Tables, context => GenerateTable(context.Input.Split(',').Select(split => split.Trim()).ToList()))
                .If(context => string.IsNullOrEmpty(context.Tables), context => Console.WriteLine($@"{Cuts.Point()} No tables found"))
                .Build();

            var workflowContext = await workflow.RunAsync(new SpecflowContext()).ConfigureAwait(false);
            return workflowContext.Tables;
        }

        private string GenerateTable(ICollection<string> typeNames)
        {
            var existingTypes = _typeProvider.TryGetByNames(typeNames);
            var notExistingTypes = typeNames.Where(name => !existingTypes.Select(etype => etype.Name).Contains(name));

            var stringBuilder = new StringBuilder();

            foreach (var type in notExistingTypes)
            {
                stringBuilder.AppendLine($@"{Cuts.Point()} {type} not found");
            }

            var tables = CreateTables(existingTypes);

            foreach (var table in tables)
            {
                stringBuilder.AppendLine($@"{Cuts.Medium()}");
                stringBuilder.AppendLine($@"{Cuts.Heading()} {table.Key}");
                stringBuilder.AppendLine(table.Value.Item1);
                stringBuilder.AppendLine(table.Value.Item2);
                stringBuilder.AppendLine($@"{Cuts.Medium()}");
            }

            return stringBuilder.ToString();
        }

        private static IDictionary<Type, (string, string)> CreateTables(IEnumerable<Type> entityTypes)
        {
            var tables = new Dictionary<Type, (string, string)>();

            foreach (var type in entityTypes)
            {
                var properties = RetrieveProperties(type).ToList();
                
                if (properties.Any())
                {
                    CreateTableForType(properties, tables, type);
                }
                else
                {
                    Console.WriteLine($@"{Cuts.Point()} Type has no properties");
                }
            }

            return tables;
        }

        private static void CreateTableForType(IReadOnlyCollection<KeyValuePair<PropertyInfo, string>> properties, IDictionary<Type, (string, string)> tables, Type type)
        {
            var table = string.Concat(new List<string>
            {
                "| ",
                "#",
                " | ",
                string.Join(" | ", properties.Select(property => property.Value)),
                " |"
            });

            var emptyRow = string.Concat(new List<string>
            {
                "| ",
                $"{string.Concat(type.Name.Where(char.IsUpper))}1",
                " | ",
                string.Join(" | ", properties.Select(GetDefault).ToList()),
                " |"
            });

            tables.Add(type, (table, emptyRow));
        }

        private static string GetDefault(KeyValuePair<PropertyInfo, string> propertyInfo)
        {
            const string defaultNull = "TODO";

            var propertyType = propertyInfo.Key.PropertyType;

            if (propertyType.IsEnum)
            {
                return Enum.GetValues(propertyType).GetValue(0)?.ToString();
            }

            if (propertyType == typeof(DateTime))
            {
                var now = DateTime.Now;
                var month = now.Month.IsBetweenInclusive(1, 9) ? $"0{now.Month}" : $"{now.Month}";
                var day = now.Day.IsBetweenInclusive(1, 9) ? $"0{now.Day}" : $"{now.Day}";
                return $"{now.Year}-{month}-{day}";
            }

            if (propertyType == typeof(bool))
            {
                return "false";
            }

            var property = Type.GetType(propertyType.FullName!);

            if (property is null)
            {
                return defaultNull;
            }

            return property.IsValueType ? Activator.CreateInstance(property)?.ToString() : defaultNull;
        }

        private static IEnumerable<KeyValuePair<PropertyInfo, string>> RetrieveProperties(Type type)
        {
            var properties = type.GetProperties()
                .Where(ShouldUseProperty)
                .OrderByDescending(property => property.Name == "Id")
                .ThenByDescending(property => property.Name.Contains("Id"))
                .ThenBy(property => property.Name);

            foreach (var property in properties)
            {
                yield return new KeyValuePair<PropertyInfo, string>(property, CreateHeader(property));
            }
        }

        private static bool ShouldUseProperty(PropertyInfo property)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) == null
                   && property.Name != "ModifiedDate"
                   && !PropertyEval.IsList(property.PropertyType)
                   && property.GetSetMethod() != null
                   && property.Name != "Id";
        }

        private static string CreateHeader(PropertyInfo property)
        {
            if (property.Name.Contains("Id"))
            {
                return property.Name == "Id" ? string.Empty : $"#:Id->{property.Name}";
            }

            if (property.PropertyType.IsClass && !PropertyEval.IsSimple(property.PropertyType))
            {
                return $"#:{property.Name}";
            }

            return property.Name;
        }


    }
}