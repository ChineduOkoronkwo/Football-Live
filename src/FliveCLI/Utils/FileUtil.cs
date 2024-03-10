namespace FliveCLI.Utils;

public static class FileUtil
{
    internal static string GetPath(string folderName = "", string fileName = "")
    {
        string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = "Gol";
        return Path.Combine(appDataFolder, appFolder, folderName, fileName);
    }
}