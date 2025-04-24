using Library.Communication;
using Library.Interfaces;
using Sygnalista_B.LocalLibrary;
using Sygnalista_B.LocalLibrary.Html;
using Sygnalista_B.LocalLibrary.Services;
using Sygnalista_B.MVVM.ViewModels;
using Sygnalista_B.MVVM.Views;
using System.Windows;

namespace Sygnalista_B;

public partial class App : PrismApplication
{
    protected override void OnExit(ExitEventArgs e)
    {
        _ = Container.Resolve<CommunicationManager>().SendMessage(["stoppedlistening"]);
        Features.ClearCache();
        base.OnExit(e);
    }

    protected override Window CreateShell()
    {
        var mainWindow = Container.Resolve<MainWindowView>();

        Container.GetContainer().RegisterInstance(mainWindow.chrome);

        return mainWindow;
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<MainWindowViewModel>();
        containerRegistry.RegisterSingleton<MainWindowView>();
        containerRegistry.RegisterSingleton<CompanyInfoManager>();
        containerRegistry.RegisterSingleton<CaptchaManager>();
        containerRegistry.RegisterSingleton<Background>();
        containerRegistry.RegisterSingleton<CommunicationManager>();
        containerRegistry.RegisterSingleton<HtmlProcessingManager>();
        containerRegistry.RegisterSingleton<MainLoopManager>();

        containerRegistry.RegisterSingleton<ICommunicationService, CommunicationService>();
    }
}