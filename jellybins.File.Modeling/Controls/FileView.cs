namespace jellybins.File.Modeling.Controls;

/// <summary>
/// Общее представление
/// Будет передаваться в jellybins.Middleware и обрабатываться
/// записываться в окно.
/// </summary>
public class FileView
{
    public FileInfo File { get; private set; }
    public Dictionary<string, string[]> Headers { get; private set; }
    public Dictionary<string, string[]> Flags { get; set; }
    
    public FileView(
        FileInfo file,
        Dictionary<string, string[]> headers,
        Dictionary<string, string[]> flags)
    {
        Headers = headers;
        Flags = flags;
        File = file;
    }

    public FileView(FileInfo info)
    {
        File = info;
        Headers = new();
        Flags = new();
    }
    
    public void PushSection(ref Dictionary<string, string[]> section)
    {
        Parallel.ForEach(section, pair =>
        {
            Headers.Add(pair.Key, pair.Value);
        });
    }

    public void PushFlags(Dictionary<string, string[]> flags)
    {
        Parallel.ForEach(flags, pair =>
        {
            Flags.Add(pair.Key, pair.Value);
        });
    }
}