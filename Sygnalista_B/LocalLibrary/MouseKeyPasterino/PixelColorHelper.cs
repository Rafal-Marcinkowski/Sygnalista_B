﻿using System.Runtime.InteropServices;

namespace Sygnalista_B.LocalLibrary.MouseKeyPasterino;

public class PixelColorHelper
{
    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

    public static Color GetPixelColor(int x, int y)
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        uint pixel = GetPixel(hdc, x, y);
        ReleaseDC(IntPtr.Zero, hdc);
        return Color.FromArgb((int)(pixel & 0x000000FF),
                              (int)(pixel & 0x0000FF00) >> 8,
                              (int)(pixel & 0x00FF0000) >> 16);
    }
}
