﻿using Autofac;

namespace TypeCode.Wpf.Builder
{
    public class BuilderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BuilderViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}