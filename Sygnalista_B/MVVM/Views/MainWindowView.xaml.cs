using CefSharp;
using CefSharp.Wpf;
using Sygnalista_B.LocalLibrary;
using Sygnalista_B.LocalLibrary.Services;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Sygnalista_B.MVVM.Views;

public partial class MainWindowView
{
    public MainWindowView()
    {
        InitializeCefSharp();
        InitializeComponent();

        this.Top = 710;
        this.Left = 1670;

        PreviewKeyDown += HandlePreviewKeyDown;
        chrome.FrameLoadEnd += Chrome_FrameLoadEnd;
    }

    private void InitializeCefSharp()
    {
        var settings = new CefSettings
        {
            LogSeverity = LogSeverity.Disable,
            IgnoreCertificateErrors = true,
            //RootCachePath = ""
            RootCachePath = Path.Combine(Environment.CurrentDirectory, "CefCache"),
        };

        Cef.Initialize(settings);
    }

    private void Chrome_FrameLoadEnd(object? sender, FrameLoadEndEventArgs e)
    {
        string cssCode = @"
        body { overflow: hidden; }
        .styles_container__1t_Oy { display: none !important; }
        .styles_container__cv0VW { display: none!important; }
        .styles_nav__A_fC1 { display: none !important; }
        .styles_calendar__hTz7L { display: none !important; }
        #bb-that { display: none !important; }
    ";
        string script = $"var style = document.createElement('style'); style.innerHTML = `{cssCode}`; document.head.appendChild(style);";
        chrome.ExecuteScriptAsync(script);
    }


    private void Shutdown(object sender, RoutedEventArgs e) => App.Current.Shutdown();

    private void ChromGoBack(object sender, RoutedEventArgs e)
    {
        //if (chrome.CanGoBack)
        //{
        //    chrome.BackCommand.Execute(null);
        //}

        chrome.LoadUrlAsync("https://-----------");
    }

    private void HandlePreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            CaptchaManager.CaptchaCancellationTokenSource.Cancel();
        }

        if (e.Key == Key.Down)
        {
            _ = Features.AutoReset();
        }

        if (e.Key == Key.Up)
        {
            CaptchaManager.CaptchaCancellationTokenSource = new();
            _ = Features.AutoCaptcha(CaptchaManager.CaptchaCancellationTokenSource.Token);
        }
    }
}