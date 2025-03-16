using jellybins.Core.Models;

namespace jellybins.Core.Interfaces;

public interface ISectionsReader
{
    void ProcessImports();
    void ProcessRuntime();
    void ProcessExports();
    SectionsProperties[] Sections { get; }
}