using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class DbScriptWriter
    {
        internal static string Foldername => "DbScripts";
        private static string FileName => "DbScripts.sql";

        internal static void Write(TableEntity tableEntity, bool append, string tab4)
        {
            var filePath = FileUtil.GetPath(Foldername, FileName);
            var stringContent = tableEntity.GenerateCreateTableSql();
            using StreamWriter writer = new StreamWriter(filePath, append);
            var lastLine = stringContent.Count - 1;
            for (int index = 0; index <= lastLine; index++)
            {
                if (index > 0 && index < lastLine)
                {
                    writer.Write(tab4);
                }

                writer.WriteLine(stringContent[index]);
            }

            writer.WriteLine();
        }
    }
}