﻿namespace Library;

public static class SaveTextToFile
{
    public static async Task SaveAsync(string saveAs, string textToSave)
    {
        File.WriteAllText($"C:/Users/rafal/Desktop/Pogromcy/Sygnalista_B/{saveAs}", textToSave);
    }

    public static async Task AddAsync(string whereToAdd, string textToAdd)
    {
        File.AppendAllText($"C:/Users/rafal/Desktop/Pogromcy/Sygnalista_B/{whereToAdd}", textToAdd);
    }
}