using jellybins.Console.Handlers;

namespace jellybins.Console;
internal class Program
{
    public static void Main(string[] args) =>
        _ = (args.Length < 2) 
            ? HandlerFactory.CreateHandler() 
            : HandlerFactory.CreateHandler(ref args);
}