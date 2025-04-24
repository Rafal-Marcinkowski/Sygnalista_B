using WindowsInput;

namespace Sygnalista_B.LocalLibrary.MouseKeyPasterino;

public static class Mouse
{
    public async static Task Move(int x, int y)
    {
        Cursor.Position = new Point(x, y);
    }

    public async static Task MoveLeftClick(int x, int y)
    {
        InputSimulator inputSimulator = new();
        await Move(x, y);
        inputSimulator.Mouse.LeftButtonClick();
    }

    private static void BlockMouseInput()
    {
        Cursor.Hide();
    }

    private static void UnblockMouseInput()
    {
        Cursor.Show();
    }
}
