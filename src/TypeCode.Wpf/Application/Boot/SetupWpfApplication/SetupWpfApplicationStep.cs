using System.Diagnostics;
using System.Windows;
using Framework.Jab.Boot;
using Workflow;

namespace TypeCode.Wpf.Application.Boot.SetupWpfApplication;

public class SetupWpfApplicationStep<TContext> : ISetupWpfApplicationStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    public Task ExecuteAsync(TContext context)
    {
        EventManager.RegisterClassHandler(
            typeof(System.Windows.Documents.Hyperlink),
            System.Windows.Documents.Hyperlink.RequestNavigateEvent,
            new System.Windows.Navigation.RequestNavigateEventHandler(
                (_, en) =>
                {
                    Process.Start(new ProcessStartInfo(en.Uri.ToString()) { UseShellExecute = true });
                    en.Handled = true;
                })
        );

        return Task.CompletedTask;
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return Task.FromResult(true);
    }
}