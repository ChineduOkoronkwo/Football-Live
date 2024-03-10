using FliveCLI.TableEntities;

namespace FliveCLI.Writers
{
    public static class DbScriptWriter
    {
        internal static string Foldername => "DbScripts";
        internal static string FileNamePrefix => "DbScripts.sql";
        internal static void Write(TableEntity tableEntity, string filePath, bool append, string tab4)
        {
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