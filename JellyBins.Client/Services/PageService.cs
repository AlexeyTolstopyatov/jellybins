using System.Windows;
using Wpf.Ui.Abstractions;

namespace JellyBins.Client.Services;

public class PageService : INavigationViewPageProvider
{
    private readonly IServiceProvider _serviceProvider;
    
    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public T? GetPage<T>() where T : class
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException($"The page should be a Page, but {typeof(T)}");
        }

        return (T?)_serviceProvider.GetService(typeof(T));
    }
    
    public Object? GetPage(Type pageType)
    {
        if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
        {
            throw new InvalidOperationException($"The page should be a Page, but {pageType}.");
        }

        return _serviceProvider.GetService(pageType);
    }
}