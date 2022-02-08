using Framework.Autofac.Boot;

namespace TypeCode.Console.Boot.Helper;

public static class ContextProvider
{
    private static BootContext? _context;

    public static BootContext Get()
    {
        if (_context is null)
        {
            throw new ArgumentException($"{nameof(BootContext)} is not yet available");
        }
        
        return _context;
    }
    
    public static void Set(BootContext context)
    {
        _context = context;
    }
}