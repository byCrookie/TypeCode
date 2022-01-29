using Framework.Jab;
using Framework.Jab.Boot;
using Framework.Jab.Boot.Logger;
using Jab;
using TypeCode.Business.Modules;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;
using TypeCode.Wpf.Main;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;
using Workflow;

namespace TypeCode.Wpf.Application.Boot;

[ServiceProvider]
[Import(typeof(IFrameworkModule))]
[Transient(typeof(LoggerBootStepOptions))]
[Transient(typeof(BootContext))]
[Transient(typeof(IWorkflowBuilder<BootContext>), typeof(WorkflowBuilder<BootContext>))]
[Transient(typeof(MainSidebarViewModel))]
[Transient(typeof(MainContentViewModel))]
[Transient(typeof(MainViewModel))]
[Singleton(typeof(IWpfWindowProvider), typeof(WpfWindowProvider))]
[Import(typeof(ITypeCodeWpfModule))]
[Import(typeof(ITypeCodeBusinessModule))]
public partial class TypeCodeWpfServiceProvider
{
}