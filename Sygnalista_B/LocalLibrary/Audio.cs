using System.Media;

namespace Sygnalista_B.LocalLibrary;

public class Audio
{
    public async static Task PlayCaptchaSoundAsync()
    {
        using SoundPlayer player = new();
        player.SoundLocation = "D:\\VisualStudioProjekty\\Giełda\\Sygnalista\\Sygnalista_B\\Miscellaneous\\captcha.wav";
        player.Play();
    }

    public async static Task PlaySoundAsync()
    {
        using SoundPlayer player = new();
        player.SoundLocation = "D:\\VisualStudioProjekty\\Giełda\\Sygnalista\\Sygnalista_B\\Miscellaneous\\waterdrop.wav";
        player.Play();
    }
}