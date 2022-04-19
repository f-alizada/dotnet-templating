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
/// <remarks>
/// By necessity, this uses a different processor on each platform.
/// For windows, it uses powershell's Invoke-Item.
/// For linux, it uses xdg-open.
/// For mac, it uses open.
/// </remarks>
internal class OpenFilePostActionProcessor : IPostActionProcessor
{
    internal static readonly Guid ActionProcessorId = new Guid("D396686C-DE0E-4DE6-906D-291CD29FC5DE");

    public Guid Id => ActionProcessorId;

    public bool Process(IEngineEnvironmentSettings environment, IPostAction action, ICreationEffects creationEffects, ICreationResult templateCreationResult, string outputBasePath)
    {
        var fileToOpen = DiscoverFilePath(outputBasePath, action.Args);
        if (fileToOpen is null)
        {
            return false;
        }
        (string cmd, string[] args) = GetCommandAndArgs(fileToOpen);
        return ExecuteProcess(cmd, args);
    }

    private static bool ExecuteProcess(string cmd, string[] args)
    {
        var procArgs = new ProcessStartInfo(cmd, string.Join(" ", args))
        {
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
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

    private static (string Cmd, string[] Args) GetCommandAndArgs(string fileToOpen)
    {
        if (System.OperatingSystem.IsLinux())
        {
            return ("xdg-open", new[] { fileToOpen });
        }
        else if (System.OperatingSystem.IsMacOS())
        {
            return ("open", new[] { fileToOpen });
        }
        else
        {
            return ("powershell", new[] { "-NoProfile", "-NoLogo", "-NonInteractive", "-Command", $"Invoke-Item \"{fileToOpen}\"" });
        }
    }

    private static string? DiscoverFilePath(string outputBasePath, IReadOnlyDictionary<string, string> actionArgs)
    {
        if (actionArgs.TryGetValue("file", out string? file))
        {
            return file;
        }
        else
        {
            Reporter.Error.Write(""); //LocalizableStrings.PostAction_OpenFileProcessor_Error_ConfigMissingFile)
            return null;
        }
    }

}
