using jellybins.Middleware;

namespace jellybins.Models;

public class JbStringHead
{
    public string Name { get; private set; } = string.Empty;
    public List<string> List { get; private set; } = new();

    public JbStringHead(string name, List<string> list)
    {
        Name = name;
        List = list;
    }

    public JbStringHead(string name)
    {
        Name = name;
    }
}