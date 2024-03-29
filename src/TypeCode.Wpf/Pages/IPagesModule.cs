﻿using Jab;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Configuration;
using TypeCode.Wpf.Pages.DynamicExecution;
using TypeCode.Wpf.Pages.Encoding;
using TypeCode.Wpf.Pages.Guid;
using TypeCode.Wpf.Pages.Home;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.String;
using TypeCode.Wpf.Pages.TypeSelection;
using TypeCode.Wpf.Pages.UnitTest;

namespace TypeCode.Wpf.Pages;

[ServiceProviderModule]
[Import(typeof(ISpecflowModule))]
[Import(typeof(IUnitTestModule))]
[Import(typeof(IComposerModule))]
[Import(typeof(IMapperModule))]
[Import(typeof(IBuilderModule))]
[Import(typeof(IAssembliesModule))]
[Import(typeof(ITypeSelectionModule))]
[Import(typeof(IConfigurationWizardModule))]
[Import(typeof(IHomeModule))]
[Import(typeof(IDynamicExecutionModule))]
[Import(typeof(IGuidModule))]
[Import(typeof(IEncodingModule))]
[Import(typeof(IStringModule))]
public interface IPagesModule
{
}