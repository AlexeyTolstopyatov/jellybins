using jellybins.Core.Models;
namespace jellybins.Core.Interfaces;
public interface IReader
{
    Dictionary<string, string> GetHeader();
    Dictionary<string, string[]> GetFlags();
    CommonProperties GetProperties();
    void GetImports();
}