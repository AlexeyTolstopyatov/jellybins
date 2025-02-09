namespace jellybins.Console.Interfaces;

public interface IHandler
{
    void BuildPropertiesTable();
    void BuildHeaderTable();
    void BuildImportsTable();
    void BuildFlagsTable();
}