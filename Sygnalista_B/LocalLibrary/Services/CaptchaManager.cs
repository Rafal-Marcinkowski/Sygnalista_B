using CefSharp.Wpf;

namespace Sygnalista_B.LocalLibrary.Services;

public class CaptchaManager(ChromiumWebBrowser webBrowser)
{
    public static CancellationTokenSource CaptchaCancellationTokenSource { get; set; }
    public bool ForceCaptcha { get; set; } = false;
    public bool ForceFillingCaptcha { get; internal set; }

    public async Task DoCaptchaStuff()
    {
        _ = Audio.PlayCaptchaSoundAsync();
        await Task.Delay(100);

        do
        {
            await Task.Delay(150);
        } while (!webBrowser.IsLoaded);

        if (!ForceCaptcha)
        {
            App.Current.Shutdown();
        }
        else
        {
            CaptchaCancellationTokenSource = new();
            await Features.AutoCaptcha(CaptchaCancellationTokenSource.Token);
        }
    }
}
