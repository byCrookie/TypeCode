# TypeCode

## About
Develop c# code faster by generating .NET specific boilerplate code using reflection on assemblies.

## Features

* Generate specflow tables for classes
* Generate fake constructor for unit-test class
* Generate composer for strategies by interface
* Generate mapper for two classes
* Generate builder for class using builder pattern

## Framework
This project is built on top of https://github.com/byCrookie/Framework.NET.

## How to use

> :warning: **Releases**: Releases are not working at the moment, please build the project yourself using the sourcecode

1. Download github repository https://github.com/byCrookie/TypeCode
2. Build project using your favorite .NET IDE
3. Change paths in Configuration.cfg.xml to reflect parent directories for your local assemblies
4. Start TypeCode.exe

## Contributing / Issues
All contributions are welcome! If you have any issues or feature requests, either implement it yourself or create an issue, thank you.

## Donation
If you like this project, feel free to donate and support the further development. Thank you.

Bitcoin (BTC) Donations using Bitcoin (BTC) Network -> 14vZ2rRTEWXhvfLrxSboTN15k5XuRL1AHq

## Docs

### Configuration.cfg.xml

#### AssemblyRoot
Combine multiple assembly targets into group to prioritize.

* Path -> Root-Path
* Priority -> Priority over other assembly-roots

```XML
<AssemblyRoot Path="M:\Development\Home\" Priority="1">
  ...
</AssemblyRoot>
```

#### IncludeAssemblyPattern
Use regex patterns for including assemblies in assembly root

* Value -> Regex-Pattern

```XML
<IncludeAssemblyPattern>^(?=.*\bNonic\b)(?=.*\b.dll\b).*$</IncludeAssemblyPattern>
```

#### AssemblyGroup
Combine multiple assembly targets into group to prioritize.

* Name -> Used to display assembly priority menu
* Priority -> Priority over other assembly-groups

```XML
<AssemblyGroup Name="TypeCode" Priority="1">
    ...
</AssemblyGroup>
```

#### AssemblyPath
Select parent directories of assemblies by using relative path to assembly root

* Value -> Relative path to parent directory of assemblies
* Priority -> Priority over other assembly-paths and assembly-selectors

```XML
<AssemblyPath Priority="1">TypeCode\src\TypeCode.Console\bin\Debug\net5.0</AssemblyPath>
```

#### AssemblyPathSelector
Select parent directories of assemblies by using regex pattern and relative path

* Value -> Relative path to parent directory selected by regex pattern
* Priority -> Priority over other assembly-paths and assembly-selectors

```XML
<AssemblyPathSelector Priority="1" Selector="TypeCode_(*)">bin\Debug\net5.0</AssemblyPathSelector>
```

#### Full Example
```XML
<AssemblyRoot Path="M:\Development\Home\" Priority="1">
    <IncludeAssemblyPattern>^(?=.*\bNonic\b)(?=.*\b.dll\b).*$</IncludeAssemblyPattern>
    <IncludeAssemblyPattern>^(?=.*\bTypeCode\b)(?=.*\b.dll\b).*$</IncludeAssemblyPattern>
    <IncludeAssemblyPattern>^(?=.*\bFramework\b)(?=.*\b.dll\b).*$</IncludeAssemblyPattern>

    <AssemblyGroup Name="TypeCode" Priority="1">
        <AssemblyPathSelector Priority="1" Selector="TypeCode_(*)">bin\Debug\net5.0</AssemblyPathSelector>
        <AssemblyPath Priority="2">TypeCode\src\TypeCode.Console\bin\Debug\net5.0</AssemblyPath>
    </AssemblyGroup>
    <AssemblyGroup Name="Nonic-Bot" Priority="2">
        <AssemblyPath Priority="1">nonic-bot\Nonic\Nonic.Bot\bin\Debug\net5.0</AssemblyPath>
    </AssemblyGroup>
</AssemblyRoot>
```

### Features

#### Specflow-Table Generation
Generates specflow tables for classes using the required properties for the specflow test not failing.

```C#
| # | Name | Priority | Text |
| AG1 | TODO | 0 | TODO |
```

#### Unit-Test Dependency Generation
Generates the needed dependencies for the unit-test class. It uses FakeItEasy-Syntax.

```C#
private UnitTestDependencyTypeCodeStrategy _testee;

private IWorkflowBuilder<UnitTestDependencyEvaluationContext> _workflowEvaluationBuilder;
private IWorkflowBuilder<UnitTestDependencyGenerationContext> _workflowGenerationBuilder;
private ITypeProvider _typeProvider;

[TestInitialize]
public void TestInitialize()
{
        _workflowEvaluationBuilder = A.Fake<IWorkflowBuilder<UnitTestDependencyEvaluationContext>>();
        _workflowGenerationBuilder = A.Fake<IWorkflowBuilder<UnitTestDependencyGenerationContext>>();
        _typeProvider = A.Fake<ITypeProvider>();

        _testee = new UnitTestDependencyTypeCodeStrategy(
                _workflowEvaluationBuilder,
                _workflowGenerationBuilder,
                _typeProvider
        );
}
```

#### Unit-Test Dependency Generation
Generates the needed dependencies for the unit-test class. It uses FakeItEasy-Syntax.

```C#
private UnitTestDependencyTypeCodeStrategy _testee;

private IWorkflowBuilder<UnitTestDependencyEvaluationContext> _workflowEvaluationBuilder;
private IWorkflowBuilder<UnitTestDependencyGenerationContext> _workflowGenerationBuilder;
private ITypeProvider _typeProvider;

[TestInitialize]
public void TestInitialize()
{
        _workflowEvaluationBuilder = A.Fake<IWorkflowBuilder<UnitTestDependencyEvaluationContext>>();
        _workflowGenerationBuilder = A.Fake<IWorkflowBuilder<UnitTestDependencyGenerationContext>>();
        _typeProvider = A.Fake<ITypeProvider>();

        _testee = new UnitTestDependencyTypeCodeStrategy(
                _workflowEvaluationBuilder,
                _workflowGenerationBuilder,
                _typeProvider
        );
}
```

#### Composer Generation
Generates a composer with all subclasses by an interface. It uses factory design from https://github.com/byCrookie/Framework.NET.

```C#
private IFactory _factory;

public Composer(IFactory factory)
{
        _factory = factory
}

public IEnumerable<ITypeCodeStrategy> Compose()
{
        yield return _factory.Create<IBuilderTypeCodeStrategy>();
        yield return _factory.Create<BuilderTypeCodeStrategy>();
        yield return _factory.Create<IComposerTypeCodeStrategy>();
        yield return _factory.Create<ComposerTypeCodeStrategy>();
        yield return _factory.Create<MapperTypeCodeStrategy>();
        yield return _factory.Create<IMapperTypeCodeStrategy>();
        yield return _factory.Create<SpecflowTypeCodeStrategy>();
        yield return _factory.Create<ISpecflowTypeCodeStrategy>();
        yield return _factory.Create<UnitTestDependencyTypeCodeStrategy>();
        yield return _factory.Create<IUnitTestDependencyTypeCodeStrategy>();
}
```

#### Mapper Generation
Generates a mapper between two classes using properties with similar names.

```C#
public TypeCodeConfiguration MapTo(XmlTypeCodeConfiguration xmlTypeCodeConfiguration)
{
        return new TypeCodeConfiguration
        {
                CloseCmd = xmlTypeCodeConfiguration.CloseCmd,
                SpaceKey = xmlTypeCodeConfiguration.SpaceKey,
                Username = xmlTypeCodeConfiguration.Username,
                Password = xmlTypeCodeConfiguration.Password,
                VersionPageName = xmlTypeCodeConfiguration.VersionPageName,
                BaseUrl = xmlTypeCodeConfiguration.BaseUrl,
                AssemblyRoot = xmlTypeCodeConfiguration.AssemblyRoot
        }
};
```

#### Builder Generation
Generates a builder for a class using builder pattern.

```C#
public class TypeCodeConfigurationBuilder
{
        private static TypeCodeConfiguration _typeCodeConfiguration;

        public TypeCodeConfigurationBuilder()
        {
                _typeCodeConfiguration = new TypeCodeConfiguration();
        }

        public TypeCodeConfigurationBuilder BaseUrl(string value)
        {
                _typeCodeConfiguration.BaseUrl = value;
                return this;
        }

        public TypeCodeConfigurationBuilder AddAssemblyRoot(Action<AssemblyRootBuilder> configure)
        {
                var builder = new AssemblyRootBuilder();
                configure(builder);
                _typeCodeConfiguration.AssemblyRoot.Add(builder.Build());
                return this;
        }
        
        public TypeCodeConfiguration Build()
        {
                var typeCodeConfiguration = _typeCodeConfiguration;
                _typeCodeConfiguration = new TypeCodeConfiguration();
                return typeCodeConfiguration;
        }
}
```







