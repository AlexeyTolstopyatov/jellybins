using jellybins.Core.Interfaces;
using jellybins.Core.Models;

namespace jellybins.Core.Readers.COM;

/// <summary>
/// Represents Reader for xx-DOS .COM files.
/// NOT Component Object Model assemblies.
/// </summary>
public class DosCommandReader : IReader
{
    public Dictionary<string, string> GetHeader()
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string[]> GetFlags()
    {
        throw new NotImplementedException();
    }

    public CommonProperties GetProperties()
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetImports()
    {
        throw new NotImplementedException();
    }
}