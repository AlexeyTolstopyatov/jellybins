using System.Windows.Controls;

namespace jellybins.Config;


public class JbConfig
{
    private static JbConfig Instance { get; set; }
    public static void SetInstance(int heads, int flags, int filter) =>
        Instance = new JbConfig(heads, flags, filter);
    public static void SetInstance() => 
        Instance = new JbConfig();

    public static JbConfig GetInstance => Instance;
    
    public bool ExpandHeaders { get; private set; } = true;
    public bool ExpandFlags { get; private set; } = true;
    public int FilterIndex { get; private set; } = 0;

    private JbConfig(int expandHeaders, int expandFlags, int filter)
    {
        ExpandFlags = Convert.ToBoolean(expandFlags);
        ExpandHeaders = Convert.ToBoolean(expandHeaders);
        FilterIndex = filter;
    }

    private JbConfig() { }
}