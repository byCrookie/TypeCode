﻿using Autofac;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Configuration;
using TypeCode.Business.Mode;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Modules
{
    public class TypeCodeBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>().SingleInstance();

            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<ModeModule>();
            builder.RegisterModule<TypeEvaluationModule>();
            builder.RegisterModule<BootstrappingModule>();

            base.Load(builder);
        }
    }
}