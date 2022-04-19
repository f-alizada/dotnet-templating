// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.PhysicalFileSystem;
using Microsoft.TemplateEngine.Utils;
using Newtonsoft.Json.Linq;

namespace Microsoft.TemplateEngine.Cli.PostActionProcessors;

/// <summary>
/// A PostAction that opens the specified file in the default handler for that file type.
/// </summary>
internal class OpenFilePostActionProcessor : IPostActionProcessor
{
    internal static readonly Guid ActionProcessorId = new Guid("84C0DA21-51C8-4541-9940-6CA19AF04EE6");

    public Guid Id => ActionProcessorId;

    public bool Process(IEngineEnvironmentSettings environment, IPostAction action, ICreationEffects creationEffects, ICreationResult templateCreationResult, string outputBasePath)
    {
        var filesToOpen = DiscoverFilePaths(action.Args, outputBasePath, creationEffects.CreationResult);
        if (filesToOpen is null)
        {
            return false;
        }
        return ExecuteProcesses(filesToOpen);
    }

    private static bool ExecuteProcesses(IReadOnlyList<string> filesToOpen)
    {
        return filesToOpen.All(ExecuteProcess);
    }

    private static bool ExecuteProcess(string fileToOpen)
    {
        // Process.Start with UseShellExecute set to true does all of the finding of the default handler for the file type
        // for us, so we basically have zero work to do.
        var procArgs = new ProcessStartInfo(fileToOpen)
        {
            UseShellExecute = true
        };

        var proc = System.Diagnostics.Process.Start(procArgs);
        if (proc == null)
        {
            Reporter.Error.WriteLine(LocalizableStrings.CommandFailed);
            return false;
        }

        proc.WaitForExit();
        if (proc.ExitCode != 0)
        {
            Reporter.Error.WriteLine(LocalizableStrings.CommandFailed);
            Reporter.Error.WriteCommandOutput(proc);
            Reporter.Error.WriteLine(string.Empty);
            return false;
        }
        else
        {
            Reporter.Output.WriteLine(LocalizableStrings.CommandSucceeded);
            return true;
        }
    }

    private static IReadOnlyList<string> DiscoverFilePaths(IReadOnlyDictionary<string, string> actionArgs, string basePath, ICreationResult creationResults)
    {
        var fileIndexes = FileIndexes(actionArgs);
        var filePaths = new List<string>(fileIndexes.Count);
        foreach (var index in fileIndexes)
        {
            if (index < creationResults.PrimaryOutputs.Count && creationResults.PrimaryOutputs[index] is { } output)
            {
                filePaths.Add(Path.Combine(basePath, output.Path));
            }
        }
        return filePaths;
    }

    private static List<int> FileIndexes(IReadOnlyDictionary<string, string> actionArgs)
    {
        if (actionArgs.TryGetValue("files", out string? file))
        {
            return file.Split(';').Select(int.Parse).ToList();
        }
        else
        {
            Reporter.Error.Write(""); //LocalizableStrings.PostAction_OpenFileProcessor_Error_ConfigMissingFile)
            return new List<int>();
        }
    }

}
