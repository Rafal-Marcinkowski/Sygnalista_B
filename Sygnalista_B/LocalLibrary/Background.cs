using Library;

namespace Sygnalista_B.LocalLibrary;

public class Background : BindableBase
{
    public static bool IsBlinkerWorking { get; set; } = false;

    private int backgroundColor = 1;
    public int BackgroundColor
    {
        get => backgroundColor;
        set => SetProperty(ref backgroundColor, value);
    }

    public void ChangeBackgroundColor(string value)
    {
        try
        {
            string medValueString = value;
            medValueString = RemoveWhitespacesUsingLinq(medValueString);
            int medValue = int.Parse(medValueString);
            SetColor(1);

            switch (medValue)
            {
                case < 10000:
                    IsBlinkerWorking = true;
                    Task.Run(() => BackgroundColorBlinker(2));
                    break;
                case <= 25000:
                    SetColor(2);
                    break;
                case <= 100000:
                    SetColor(3);
                    break;
                default:
                    SetColor(4);
                    break;
            }
        }

        catch (Exception ex)
        {
            _ = SaveTextToFile.SaveAsync("ErrorDuringBackgroundChanging", ex.Message);
            SetColor(1);
        }
    }

    private void SetColor(int colorId)
    {
        BackgroundColor = colorId;
    }

    private async Task BackgroundColorBlinker(int colorId)
    {
        while (IsBlinkerWorking)
        {
            BackgroundColor = BackgroundColor == colorId ? 0 : colorId;
            await Task.Delay(600);
        }
    }

    private static string RemoveWhitespacesUsingLinq(string source)
    {
        return new string([.. source.Where(c => !char.IsWhiteSpace(c))]);
    }
}
