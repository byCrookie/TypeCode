﻿using System;
using System.Threading.Tasks;
using Autofac;
using Framework.Boot;
using TypeCode.Wpf.Main;
using Workflow;

namespace TypeCode.Wpf.Application.Boot.SetupWpfApplication
{
    internal class SetupWpfApplicationStep<TContext> : ISetupWpfApplicationStep<TContext>
        where TContext : WorkflowBaseContext, IBootContext
    {
        public Task ExecuteAsync(TContext context)
        {
            var mainView = new MainWindow();
            var navigationFrame = mainView.MainContent.FindName("NavigationFrame");
            var modalFrame = mainView.FindName("ModalFrame");
            var wizardFrame = mainView.FindName("WizardFrame");

            if (navigationFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(navigationFrame)}");
            if (modalFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(modalFrame)}");
            if (wizardFrame is null) throw new ApplicationException($"{nameof(MainWindow)} does not implement {nameof(wizardFrame)}");

            context.RegistrationActions
                .Add(builder => builder.RegisterInstance(mainView).AsSelf().SingleInstance());
            context.RegistrationActions
                .Add(builder => builder.RegisterInstance(mainView.MainContent).AsSelf().SingleInstance());

            return Task.CompletedTask;
        }

        public Task<bool> ShouldExecuteAsync(TContext context)
        {
            return Task.FromResult(true);
        }
    }
}