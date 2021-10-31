﻿using Autofac;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Modal.View;

namespace TypeCode.Wpf.Helper.Navigation.Modal
{
    public class ModalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ModalNavigationService>().As<IModalNavigationService>().SingleInstance();
            
            builder.RegisterType<ModalViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}