﻿using Autofac;

namespace TypeCode.Console.Mode.ExitOrContinue;

internal class ExitOrContinueModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(ExitOrContinueStep<>)).As(typeof(IExitOrContinueStep<>));

        base.Load(builder);
    }
}