using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using TypefaceUtil.Avalonia;

[assembly:SupportedOSPlatform("browser")]

internal class Program
{
    private static async Task Main(string[] args) 
        => await BuildAvaloniaApp().StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder
            .Configure<App>();
}
