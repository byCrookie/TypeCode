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







