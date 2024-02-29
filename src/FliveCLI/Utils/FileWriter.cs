namespace FliveCLI.Utils
{
    public static class FileWriter
    {
        private static string GetFolderPath(string folderName)
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = "Gol";
            return Path.Combine(appDataFolder, appFolder, folderName);
        }

        internal static void DeleteFile(string fileName, string folderName)
        {
            var fullPath = GetFolderPath(folderName);
            fullPath = Path.Combine(fullPath, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        internal static void WriteDBScript(StringContentList stringContent, string fileName, string folderName)
        {
            var tab = "    ";
            var fullPath = GetFolderPath(folderName);
            Directory.CreateDirectory(fullPath);
            fullPath = Path.Combine(fullPath, fileName);
            bool append = true;

            using StreamWriter writer = new StreamWriter(fullPath, append);
            var lastLine = stringContent.Count - 1;
            for (int index = 0; index <= lastLine; index++)
            {
                if (index > 0 && index < lastLine)
                {
                    writer.Write(tab);
                }

                writer.WriteLine(stringContent[index]);
            }

            writer.WriteLine();
        }
    }
}