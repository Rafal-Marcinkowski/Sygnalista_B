using WindowsInput;

namespace Sygnalista_B.LocalLibrary.MouseKeyPasterino;

public class MouseColorDetector
{
    public async Task DetectCaptchaColourChange(CancellationToken token)
    {
        Color targetColor = Color.FromArgb(0xFFFFFF);
        Cursor.Position = new Point(2267, 962);
        InputSimulator inputSimulator = new();
        inputSimulator.Mouse.LeftButtonDown();

        while (!token.IsCancellationRequested)
        {
            Color pixelColor = PixelColorHelper.GetPixelColor(Cursor.Position.X, Cursor.Position.Y);
            await Task.Delay(50, token);
            int deltaR = Math.Abs(pixelColor.R - targetColor.R);
            int deltaG = Math.Abs(pixelColor.G - targetColor.G);
            int deltaB = Math.Abs(pixelColor.B - targetColor.B);
            int threshold = 10;

            if (deltaR > threshold || deltaG > threshold || deltaB > threshold)
            {
                break;
            }
        }

        await Task.Delay(1000, token);
        inputSimulator.Mouse.LeftButtonUp();
    }

    public async Task DetectCaptchaEnded(CancellationToken token)
    {
        Color targetColor = Color.FromArgb(0xFFFFFF);

        while (!token.IsCancellationRequested)
        {
            Color pixelColor = PixelColorHelper.GetPixelColor(Cursor.Position.X, Cursor.Position.Y);
            await Task.Delay(50, token);
            int deltaR = Math.Abs(pixelColor.R - targetColor.R);
            int deltaG = Math.Abs(pixelColor.G - targetColor.G);
            int deltaB = Math.Abs(pixelColor.B - targetColor.B);
            int threshold = 10;

            if (deltaR <= threshold && deltaG <= threshold && deltaB <= threshold)
                break;
        }
    }
}
