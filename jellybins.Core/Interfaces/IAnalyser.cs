using System.Runtime.InteropServices;

namespace jellybins.Core.Models.Workers;

public interface IAnalyser
{
    /// <summary>
    /// Collects all properties from exciting binary
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    CommonProperties Properties(string path);
    /// <summary>
    /// Collects all properties from exciting binary
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    Task<CommonProperties> PropertiesAsync(string path);
    /// <summary>
    /// Collects 
    /// </summary>
    /// <param name="str"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<(string[], string[])> HeaderTableAsync<T>([Optional] T str) where T : struct;
    
}