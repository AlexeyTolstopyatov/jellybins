namespace JellyBins.NewExecutable.Private.Types;

[Flags]
public enum NeResourceTypeFlag
{
    Movable = 0x0010,
    Pure = 0x0020,
    PreLoad = 0x0040
}