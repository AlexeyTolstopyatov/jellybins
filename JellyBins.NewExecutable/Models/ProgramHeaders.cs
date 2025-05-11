using JellyBins.NewExecutable.Headers;

namespace JellyBins.NewExecutable.Models;

public class ProgramHeaders
{
    public NeHeader Header { get; private set; }
    public String RuntimeWord { get; } = "OS ABI";
}