using System;
using System.Threading.Tasks;
using Autofac;
using Framework.Boot;
using Workflow;

namespace TypeCode.Wpf.Helper.Boot.SetupWpfApplication
{
    internal class SetupWpfApplicationStep<TContext> : ISetupWpfApplicationStep<TContext>
        where TContext : WorkflowBaseContext, IBootContext
    {
        public Task ExecuteAsync(TContext context)
        {
            var mainView = new MainWindow();
            var frame = mainView.FindName("NavigationFrame");

            if (frame is null)
            {
                throw new ApplicationException("MainView does not implement navigation frame");
            }

            context.RegistrationActions
                .Add(builder => builder.RegisterInstance(mainView).AsSelf().SingleInstance());
            context.RegistrationActions
                .Add(builder => builder.RegisterType<MainViewModel>().AsSelf());

            return Task.CompletedTask;
        }

        public Task<bool> ShouldExecuteAsync(TContext context)
        {
            return Task.FromResult(true);
        }
    }
}