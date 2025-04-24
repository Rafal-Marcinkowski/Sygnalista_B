using Library;
using Sygnalista_B.LocalLibrary.MouseKeyPasterino;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Sygnalista_B.LocalLibrary;

public class Features()
{
    public async static Task ClickStartStopButton()
    {
        await Mouse.MoveLeftClick(2515, 1390);
    }

    public async static Task AutoReset()
    {
        //await Task.Delay(50);
        //Color targetColor = Color.FromArgb(0x000000);
        //await Mouse.Move(1960, 1363); ///Akceptowanie cookies, czarne tło
        //await Task.Delay(50);
        //Color pixelColor = PixelColorHelper.GetPixelColor(Cursor.Position.X, Cursor.Position.Y);
        //int deltaR = Math.Abs(pixelColor.R - targetColor.R);
        //int deltaG = Math.Abs(pixelColor.G - targetColor.G);
        //int deltaB = Math.Abs(pixelColor.B - targetColor.B);
        //int threshold = 10;

        //if (deltaR <= threshold && deltaG <= threshold && deltaB <= threshold)
        //{
        //    await Mouse.MoveLeftClick(Cursor.Position.X, Cursor.Position.Y);
        //    await ClickStartStopButton();
        //}
        await ClickStartStopButton();
    }

    public async static Task AutoCaptcha(CancellationToken token)
    {
        await Task.Delay(2000, token);
        MouseColorDetector detector = new();
        await detector.DetectCaptchaColourChange(token);
        await detector.DetectCaptchaEnded(token);
        await Task.Delay(3500, token);
        await AutoReset();
    }

    public async static Task RunAutoHotkeyScript()
    {
        await Task.Delay(100);
        try
        {
            Process.Start(new ProcessStartInfo("C:\\Users\\rafal\\Desktop\\Pogromcy\\wws.ahk") { Verb = "runas", UseShellExecute = true });
            await Task.Delay(100);
        }

        catch (Exception ex)
        {
            _ = SaveTextToFile.SaveAsync("AutoScriptError", ex.Message);
        }

        await Task.Delay(100);
    }

    public async static Task RunScripts(bool executeScript)
    {
        try
        {
            MouseHookManager.DisableInput();
            await Task.Delay(100);
            await ClickNewInfo();
        }

        finally
        {
            MouseHookManager.EnableInput();
        }

        if (executeScript)
        {
            await RunAutoHotkeyScript();
        }
    }

    private async static Task ClickNewInfo()
    {
        await Task.Delay(50);
        await Mouse.MoveLeftClick(1825, 750);
        await Task.Delay(50);
    }

    private void Shutdown(object sender, RoutedEventArgs e)
    {
        ClearCache();
        App.Current.Shutdown();
    }

    public static void ClearCache()
    {
        var cachePath = Path.Combine(Environment.CurrentDirectory, "CefCache");

        if (Directory.Exists(cachePath))
        {
            Directory.Delete(cachePath, true);
        }
    }
}
