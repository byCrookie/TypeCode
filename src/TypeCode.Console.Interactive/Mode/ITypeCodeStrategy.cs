﻿namespace TypeCode.Console.Interactive.Mode;

internal interface ITypeCodeStrategy
{
	int Number();
	string Description();
	bool IsPlanned();
	bool IsBeta();
	bool IsResponsibleFor(string? mode);
	Task<string?> GenerateAsync(CancellationToken? ct = null);
	bool IsExit();
}