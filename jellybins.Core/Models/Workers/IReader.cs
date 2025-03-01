namespace jellybins.Core.Models.Workers;

public interface IReader
{
    void ScanHeader();
    void ScanImports();
}