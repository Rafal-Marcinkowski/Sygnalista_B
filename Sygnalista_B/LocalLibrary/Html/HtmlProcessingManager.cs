using CefSharp;
using CefSharp.Wpf;
using Library;
using Sygnalista_B.MVVM.Models;

namespace Sygnalista_B.LocalLibrary.Html;

public class HtmlProcessingManager(ChromiumWebBrowser webBrowser, ProcessHtml processHtml)
{
    public string HtmlText { get; set; } = string.Empty;
    public string TextReference = string.Empty;
    private TaskCompletionSource<bool> reloadTcs;
    public Information Information = new();
    public bool FirstCycle = true;
    public bool IsReferenceSet = false;
    private bool isProcessingSuccesful = false;

    public async Task IsFirstCycle()
    {
        if (FirstCycle && !IsReferenceSet)
        {
            await SetTextReference();

            if (!string.IsNullOrEmpty(TextReference))
            {
                IsReferenceSet = true;
                FirstCycle = false;
            }

            _ = SaveTextToFile.SaveAsync("referencyjnyTekst", TextReference);
        }
    }

    public async Task<bool> IsValidInformation(string companyCode)
    {
        return !await processHtml.IsForbiddenTags(companyCode);
    }

    public async Task SetTextReference()
    {
        TextReference = Information.Time + Information.Header;
    }
    public async Task<bool> IsCaptcha()
    {
        return processHtml.IsCaptchaText(HtmlText);
    }

    public async Task ProcessHtmlAsync()
    {
        Information = await processHtml.PrepareInformation(HtmlText);
    }

    public async Task DownloadSygnalistaSourceAsync()
    {
        await ReloadWebsite();
        await DownloadContent();
    }

    private async Task ReloadWebsite()
    {
        reloadTcs = new();
        webBrowser.Reload(true);
        webBrowser.FrameLoadStart += handler;

        void handler(object? sender, FrameLoadStartEventArgs args)
        {
            if (args.Frame.IsMain)
            {
                webBrowser.FrameLoadStart -= handler;
                reloadTcs.SetResult(true);
            }
        }

        await reloadTcs.Task;
    }

    private async Task DownloadContent()
    {
        await Task.Delay(new Random().Next(2500, 3000));

        do
        {
            await TryProcessHtml();
        } while (!isProcessingSuccesful);

        isProcessingSuccesful = false;
    }

    private async Task TryProcessHtml()
    {
        try
        {
            HtmlText = await webBrowser.GetSourceAsync();
            await ProcessHtmlAsync();
            isProcessingSuccesful = true;
        }

        catch
        {
            await Task.Delay(100);
            await ResetProperties();
            return;
        }
    }

    public async Task<bool> IsNewInformation()
    {
        webBrowser.Reload();
        return Information.Time + Information.Header != TextReference;
    }

    public async Task ResetProperties()
    {
        HtmlText = string.Empty;
        //TextReference = string.Empty;
        //FirstCycle = true;
        //Information = new();
    }
}
