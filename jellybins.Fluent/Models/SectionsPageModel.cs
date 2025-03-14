using jellybins.Core.Models;
using jellybins.Core.Readers.PortableExecutable;

namespace jellybins.Fluent.Models;

public class SectionsPageModel
{
    public SectionsPageModel(string _fileName)
    {
        PortableExecutableSectionsReader reader = new(_fileName);
        Sections = reader.Sections;
    }
    public SectionsProperties[]? Sections { get; set; }
}