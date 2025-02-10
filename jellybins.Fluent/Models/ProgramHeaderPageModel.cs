using System.Collections.Generic;
using jellybins.Core.Attributes;
using jellybins.Core.Readers.Factory;

namespace jellybins.Fluent.Models;

public class ProgramHeaderPageModel
{
    [UnderConstruction("Runtime header not ready!")]
    public ProgramHeaderPageModel(string path)
    {
        // Model ware contains all Core API connections. It helps hiding insights.
        var programHeader = new ReaderFactory(path)
            .CreateReader()
            .GetHeader(); // returns main program header.
        
        
        ProgramHeader = programHeader;
        RuntimeHeader = new ();
        
        // check model's ~black existence~
        if (ProgramHeader.Count > 0)
            ProgramHeaderExists = true;
        if (RuntimeHeader.Count > 0)
            RuntimeHeaderExists = true;
    }
    public bool ProgramHeaderExists { get; set; }
    public bool RuntimeHeaderExists { get; set; }
    public Dictionary<string, string> ProgramHeader { get; set; }
    public Dictionary<string, string>? RuntimeHeader { get; set; }
}