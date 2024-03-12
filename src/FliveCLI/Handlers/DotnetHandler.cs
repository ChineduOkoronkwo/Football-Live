using System.Diagnostics;
using FliveCLI.Utils;

namespace FliveCLI.Handlers
{
    internal static class DotnetHandler
    {
        private static void RunCommands(string cmdArgs)
        {
            // Create a new process
            Process process = new Process();

            // Set the file name (in this case, dotnet)
            process.StartInfo.FileName = "dotnet";

            // Set arguments to create a new project dotnet new classlib -n YourLibraryName
            process.StartInfo.Arguments = cmdArgs;

            // Redirect standard output and error to read the output
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            // Enable process to raise events
            process.EnableRaisingEvents = true;

            // Event handler for reading output
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("Output: " + e.Data);
                }
            };

            // Event handler for reading error output
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("Error: " + e.Data);
                }
            };

            // Start the process
            process.Start();

            // Begin asynchronous reading of the output stream
            process.BeginOutputReadLine();

            // Begin asynchronous reading of the error output stream
            process.BeginErrorReadLine();

            // Wait for the process to exit
            process.WaitForExit();

            // Close the process after it's done
            process.Close();
        }

        internal static void CreateProject(string projectName)
        {
            string projectDirectory = FileUtil.GetPath(projectName);

            // Create project
            RunCommands($"new classlib -n {projectName} -o {projectDirectory}");
            Console.WriteLine("Project creation completed.");

            // Add reference to dapper package
            RunCommands($"add {projectDirectory}/{projectName}.csproj package Dapper");
            Console.WriteLine("Dapper package reference added.");
        }
    }
}