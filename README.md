# TypeCode

## About
Develop c# code faster by generating .NET specific boilerplate code using reflection on assemblies.

## Features

* Generate specflow tables for classes
* Generate fake constructor for unit-test class
* Generate composer for strategies by interface
* Generate mapper for two classes
* Generate builder for class using builder pattern

## How to use

1. Download github repository https://github.com/byCrookie/TypeCode.
2. Build project using your favorite .NET IDE
3. Change paths in Configuration.cfg.xml to reflect parent directories for your local assemblies
4. Start TypeCode.exe

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






