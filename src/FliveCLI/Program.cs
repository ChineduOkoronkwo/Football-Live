using System.CommandLine;
using FliveCLI.EntityFileProcessors;
using FliveCLI.Writers;

namespace flive;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var entityDirOption = new Option<string>(
            name: "--entity-dir",
            description: "Path to the directory that contains the .cs entity files."
        );

        var rootCommand = new RootCommand("A CLI that generates repos and sql commands for a given entity");
        rootCommand.AddOption(entityDirOption);
        rootCommand.SetHandler(GenRepo, entityDirOption);
        return await rootCommand.InvokeAsync(args);
    }

    internal static void GenRepo(string dirPath)
    {
        var types = TypeLoader.LoadTypesFromFile(dirPath);
        var entities = TypeLoader.CreateTableEntitiesFromType(types);
        FileWriter.CreateRepo(entities);
    }
}