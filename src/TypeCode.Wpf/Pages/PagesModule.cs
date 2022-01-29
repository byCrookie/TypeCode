﻿using Jab;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Common;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.TypeSelection;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Pages;

[ServiceProviderModule]
[Import(typeof(ISpecflowModule))]
[Import(typeof(IUnitTestDependencyTypeModule))]
[Import(typeof(IUnitTestDependencyManuallyModule))]
[Import(typeof(IComposerModule))]
[Import(typeof(IMapperModule))]
[Import(typeof(IBuilderModule))]
[Import(typeof(IAssemblyModule))]
[Import(typeof(ITypeSelectionModule))]
[Import(typeof(ICommonWizardModule))]
public partial interface IPagesModule
{
}