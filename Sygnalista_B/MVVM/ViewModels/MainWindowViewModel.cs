using CefSharp.Wpf;
using Library.Events;
using Sygnalista_B.LocalLibrary;
using Sygnalista_B.LocalLibrary.Services;
using System.Windows.Input;

namespace Sygnalista_B.MVVM.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly CommunicationManager communicationManager;
    private readonly MainLoopManager mainLoopManager;
    private readonly CaptchaManager captchaManager;
    public CompanyInfoManager CompanyInfoManager { get; set; }
    public int ForceButtonBackgroundColor => CompanyInfoManager.ForceAutoScript ? 2 : 1;
    public int ForceFillingCaptchaBackgroundColor => captchaManager.ForceFillingCaptcha ? 2 : 1;
    public Background Background { get; set; }

    public MainWindowViewModel(
        IEventAggregator eventAggregator,
        CommunicationManager communicationManager,
        ChromiumWebBrowser webBrowser,
        MainLoopManager mainLoopManager,
        CompanyInfoManager companyInfoManager,
        Background background)
    {
        this.communicationManager = communicationManager;
        this.mainLoopManager = mainLoopManager;
        captchaManager = new(webBrowser);
        this.Background = background;
        this.CompanyInfoManager = companyInfoManager;

        eventAggregator.GetEvent<CommunicationEvent>().Subscribe((payload) => this.communicationManager.OnMessageReceived(payload));
        _ = communicationManager.StartListeningAsync();

        NameTranslation.InitializeTranslations();
        _ = NameTranslation.InitializeTurnoverMedian();
    }

    public ICommand ExecuteSygnalista => new DelegateCommand(async () =>
    {
        if (mainLoopManager.IsSearching)
            mainLoopManager.StopLoop();
        else
            await mainLoopManager.ResetAndStart();
    });

    public IAsyncCommand ForceCaptchaCommand => new AsyncDelegateCommand(async () =>
    {
        captchaManager.ForceFillingCaptcha = !captchaManager.ForceFillingCaptcha;
        RaisePropertyChanged(nameof(ForceFillingCaptchaBackgroundColor));
    });

    public ICommand ForceAutoScriptCommand => new DelegateCommand(() =>
    {
        CompanyInfoManager.ForceAutoScript = !CompanyInfoManager.ForceAutoScript;
        RaisePropertyChanged(nameof(ForceButtonBackgroundColor));
    });
}
