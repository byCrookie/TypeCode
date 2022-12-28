using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeCode.Business.Mode.UnitTestDependency.Manually;

namespace TypeCode.Business.Tests.Mode;

[TestClass]
public class UnitTestDependencyManuallyTypeCodeGeneratorTest
{
    private UnitTestDependencyManuallyTypeCodeGenerator _testee = null!;

    [TestInitialize]
    public void Initialize()
    {
        _testee = new UnitTestDependencyManuallyTypeCodeGenerator();
    }

    [TestMethod]
    public async Task GenerateAsync_WhenGenerateUnitTest_ThenGenerate()
    {
        var parameter = new UnitTestDependencyManuallyGeneratorParameter
        {
            Input = await LoadEmbeddedResource(GetType().Assembly, $"{GetType().Namespace}.Input.txt")
        };
        
        var result = await _testee.GenerateAsync(parameter).ConfigureAwait(false);

        var expected = await LoadEmbeddedResource(GetType().Assembly, $"{GetType().Namespace}.Output.txt");
        
        result.Should().Be(expected);
    }
    
    private static async Task<string> LoadEmbeddedResource(Assembly assembly, string path)
    {
        var stream = assembly.GetManifestResourceStream(path)
               ?? throw new FileNotFoundException($"Embedded resource {path} missing");

        using (var streamReader = new StreamReader(stream))
        {
            return await streamReader.ReadToEndAsync();
        }
    }
}