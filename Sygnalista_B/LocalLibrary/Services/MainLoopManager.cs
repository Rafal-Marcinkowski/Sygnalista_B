using Library;
using Sygnalista_B.LocalLibrary.Html;
using Sygnalista_B.LocalLibrary.MouseKeyPasterino;

namespace Sygnalista_B.LocalLibrary.Services;

public class MainLoopManager(HtmlProcessingManager htmlProcessingManager, CommunicationManager communicationManager,
    CompanyInfoManager companyInfoManager, CaptchaManager captchaManager, Background background)
{
    public bool IsSearching { get; private set; } = false;

    public async Task StartLoop()
    {
        await SetStartingFields();
        IsSearching = true;

        while (IsSearching)
        {
            await htmlProcessingManager.DownloadSygnalistaSourceAsync();
            await CoreMechanics();
        }
    }

    private async Task SetStartingFields()
    {
        companyInfoManager.Clear();
        await htmlProcessingManager.ResetProperties();
    }

    private async Task CoreMechanics()
    {
        if (await htmlProcessingManager.IsCaptcha())
        {
            await captchaManager.DoCaptchaStuff();
            return;
        }

        if (!string.IsNullOrEmpty(htmlProcessingManager.HtmlText))
        {
            await htmlProcessingManager.IsFirstCycle();
            await companyInfoManager.CreateCompanyCode(htmlProcessingManager.Information.Header);

            if (companyInfoManager.HasValidCompanyCode())
            {
                if (await htmlProcessingManager.IsValidInformation(companyInfoManager.CompanyCode))
                {
                    await EvaluateInformation();
                }
            }
        }
    }

    private async Task EvaluateInformation()
    {
        if (await htmlProcessingManager.IsNewInformation())
        {
            _ = SaveTextToFile.SaveAsync("NewInformationHeader", htmlProcessingManager.Information.Time + htmlProcessingManager.Information.Header);
            await ReactToNewInformation();
        }
    }

    private async Task ReactToNewInformation()
    {
        var code = companyInfoManager.CompanyCode;

        if (!await communicationManager.IsEspiAlreadySeen(code))
        {
            _ = communicationManager.SendMessage([code, "readinginfo"]);
            _ = communicationManager.SendToBiznesRadar(code);

            await MouseHookManager.SetClipboard(code);
            await Features.RunScripts(communicationManager.ShouldIAutoScript && companyInfoManager.ForceAutoScript);
            _ = Audio.PlaySoundAsync();

            await companyInfoManager.CreateMedianaText();

            if (!string.IsNullOrEmpty(companyInfoManager.MedianaText))
            {
                _ = SaveTextToFile.SaveAsync("medianaTextFormatToBackground", companyInfoManager.MedianaText);
                background.ChangeBackgroundColor(companyInfoManager.MedianaText);
            }
            else
            {
                background.ChangeBackgroundColor("default");
            }

            _ = new BiznesRadarStep(companyInfoManager).Execute(code);
            TurnMainLoopOff();
        }
        else
        {
            communicationManager.EspiToAvoid.Remove(code);
        }

        htmlProcessingManager.FirstCycle = true;
        htmlProcessingManager.IsReferenceSet = false;
        _ = SaveTextToFile.AddAsync("ESPI_Godzina", DateTime.Now + ": " + code + Environment.NewLine);
    }

    public void TurnMainLoopOff() => IsSearching = false;

    internal void StopLoop()
    {
        background.BackgroundColor = 1;
        IsSearching = false;
    }

    internal async Task ResetAndStart()
    {
        Background.IsBlinkerWorking = false;
        background.BackgroundColor = 0;
        _ = communicationManager.SendMessage(["finishedreadinginfo"]);
        _ = StartLoop();
    }
}
