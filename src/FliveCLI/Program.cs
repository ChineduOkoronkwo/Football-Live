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
            description: "Path to the directory that contains the .cs entity files"
        );

        var excludeOption = new Option<string>(
            name: "--exclude",
            description: "Entites to be excluded in the generated repos classes",
            getDefaultValue: () => string.Empty
        );

        var rootCommand = new RootCommand("A CLI that generates repos and sql commands for a given entity");
        rootCommand.AddOption(entityDirOption);
        rootCommand.AddOption(excludeOption);
        rootCommand.SetHandler(GenRepo, entityDirOption, excludeOption);
        return await rootCommand.InvokeAsync(args);
    }

    internal static void GenRepo(string dirPath, string exclude)
    {
        var entitesToExclude = new HashSet<string>(exclude.Split(','));
        var types = TypeLoader.LoadTypesFromFile(dirPath);
        var entities = TypeLoader.CreateTableEntitiesFromType(types);
        FileWriter.CreateRepo(entities, entitesToExclude);
    }
}