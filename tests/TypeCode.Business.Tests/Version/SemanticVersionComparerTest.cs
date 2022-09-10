using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeCode.Business.Version;

namespace TypeCode.Business.Tests.Version;

[TestClass]
public class SemanticVersionComparerTest
{
    private SemanticVersionComparer _testee = null!;

    [TestInitialize]
    public void Initialize()
    {
        _testee = new SemanticVersionComparer();
    }

    [TestMethod]
    [DataRow("1.1.1", "1.1.1", false)]
    [DataRow("1.1.1", "1.1.2", true)]
    [DataRow("1.1.1", "1.2.1", true)]
    [DataRow("1.1.1", "2.1.1", true)]
    [DataRow("1.1.1", "2.2.1", true)]
    [DataRow("1.1.1", "2.2.2", true)]

    [DataRow("1.1.0", "1.1.1", true)]
    [DataRow("1.1.2", "1.1.1", false)]
    [DataRow("1.2.1", "1.1.1", false)]
    [DataRow("2.1.1", "1.1.1", false)]
    [DataRow("2.2.1", "1.1.1", false)]
    [DataRow("1.1.2", "1.1.1", false)]
    public void IsNewer_WhenData_ThenReturn(string current, string newer, bool expected)
    {
        var result = _testee.IsNewer(current, newer);
        result.Should().Be(expected);
    }
}