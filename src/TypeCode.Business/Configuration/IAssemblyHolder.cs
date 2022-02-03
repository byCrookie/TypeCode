using System.Collections.Concurrent;
using TypeCode.Business.Bootstrapping;

namespace TypeCode.Business.Configuration;

internal interface IAssemblyHolder
{
    string Path { get; set; }
    ConcurrentBag<AssemblyDirectory> AssemblyDirectories { get; set; }
}