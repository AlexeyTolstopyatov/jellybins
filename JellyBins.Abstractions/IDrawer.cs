namespace JellyBins.Abstractions;

public interface IDrawer
{
    public Dictionary<String, String> TabledStruct { get; }
    public void DrawStruct();
}