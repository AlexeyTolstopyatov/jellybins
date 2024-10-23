using jellybins.Views;

namespace jellybins.Middleware;

public class BinaryAnalyser
{
    private BinaryHeaderPage _binaryHeaderPage;
    
    public BinaryAnalyser(ref BinaryHeaderPage binaryHeaderPage)
    {
        _binaryHeaderPage = binaryHeaderPage;
    }
}