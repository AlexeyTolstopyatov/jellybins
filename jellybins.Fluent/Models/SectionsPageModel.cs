using jellybins.Core.Interfaces;
using jellybins.Core.Models;
using jellybins.Core.Readers.Factory;
using jellybins.Core.Readers.PortableExecutable;

namespace jellybins.Fluent.Models;

public class SectionsPageModel
{
    public SectionsPageModel(string _fileName, ushort wordSizedSign)
    {
        ISectionsReader reader = SectionsFactory.CreateReader(_fileName, wordSizedSign);
        Sections = reader.Sections;
        reader.ProcessExports();
        reader.ProcessImports();
    }

    public SectionsPageModel(string _fileName, ulong qwordSizedSign)
    {
        ISectionsReader reader = SectionsFactory.CreateReader(_fileName, qwordSizedSign);
        Sections = reader.Sections;
    }
    public SectionsProperties[]? Sections { get; set; }
}