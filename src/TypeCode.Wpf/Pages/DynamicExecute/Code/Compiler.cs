using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Serilog;

namespace TypeCode.Wpf.Pages.DynamicExecute.Code;

public class Compiler : ICompiler
{
    public byte[] Compile(string sourceCode)
    {
        Log.Debug("Starting compilation");

        using (var peStream = new MemoryStream())
        {
            var result = GenerateCode(sourceCode).Emit(peStream);

            if (!result.Success)
            {
                Log.Debug("Compilation done with error");

                var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                var error = new StringBuilder();
                
                foreach (var diagnostic in failures)
                {
                    error.AppendLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                }

                Log.Error("{0}", error.ToString());
                throw new ArgumentException($"compilation failed: {error}");
            }

            Log.Debug("Compilation done without any error");

            peStream.Seek(0, SeekOrigin.Begin);

            return peStream.ToArray();
        }
    }

    private static CSharpCompilation GenerateCode(string sourceCode)
    {
        var codeString = SourceText.From(sourceCode);
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);

        var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
        };

        Assembly.GetEntryAssembly()?.GetReferencedAssemblies().ToList()
            .ForEach(a => references.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

        return CSharpCompilation.Create("DynamicProgram.dll",
            new[] { parsedSyntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication,
                optimizationLevel: OptimizationLevel.Release,
                assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
    }
}