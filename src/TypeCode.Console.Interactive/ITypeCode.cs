﻿using Framework.Boot;

namespace TypeCode.Console.Interactive;

public interface ITypeCode<in TContext> : IApplication<TContext> where TContext : BootContext
{
}