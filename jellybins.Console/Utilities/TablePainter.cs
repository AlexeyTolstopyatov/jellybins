namespace jellybins.Console.Utilities;

using static System.Console;

public static class TablePainter
{
    public static void Paint(Dictionary<string, string> dictionary)
    {
        if (dictionary.Count == 0)
        {
            WriteLine("Empty object.");
            return;
        }

        int maxKeyLength = dictionary.Keys.Max(key => key.Length);
        int maxValueLength = dictionary.Values.Max(value => value.Length);
        
        foreach (var pair in dictionary)
        {
            string key = pair.Key;
            string value = pair.Value;
            // strings align
            string formattedKey = key.PadRight(maxKeyLength);
            string formattedValue = value.PadRight(maxValueLength);

            WriteLine($"{formattedKey} {formattedValue}");
        }
    }

}