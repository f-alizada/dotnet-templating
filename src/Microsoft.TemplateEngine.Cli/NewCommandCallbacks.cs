// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.TemplateEngine.Cli
{
    /// <summary>
    /// The set of callbacks that should be implemented by callers of <c>New3Command.Run</c>.
    /// These callbacks provide a mechanism for the template engine to invoke these operations without
    /// requiring a built-time dependency on the actual implementation.
    /// </summary>
    public sealed class NewCommandCallbacks
    {
        /// <summary>
        /// Delegate to restore project.
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns>true if successful.</returns>
        public delegate bool RestoreProjectDelegate(string projectPath);

        /// <summary>
        /// Delegate to add project references to project.
        /// </summary>
        /// <param name="projectFile">Path to project that is being modified.</param>
        /// <param name="projectReferences">One or more paths to projects to be added as references.</param>
        /// <returns>true if successful.</returns>
        public delegate bool AddProjectReferencesDelegate(string projectFile, params string[] projectReferences);

        /// <summary>
        /// Delegate to add package references to project.
        /// </summary>
        /// <param name="projectFile">Path to project that is being modified.</param>
        /// <param name="packageName">Package indetifier.</param>
        /// <param name="packageVersion">Optional version of package.</param>
        /// <returns>true if successful.</returns>
        public delegate bool AddPackageReferenceDelegate(string projectFile, string packageName, string? packageVersion = null);

        /// <summary>
        /// Delegate to add projects to solution.
        /// </summary>
        /// <param name="solutionFile">Path to solution that is being modified.</param>
        /// <param name="projects">One or more paths to projects to be added to solution.</param>
        /// <param name="solutionFolder">Solution folder where projects should appear in Solution Explorer.</param>
        /// <returns>true if successful.</returns>
        public delegate bool AddProjectsToSolutionDelegate(string solutionFile, IReadOnlyList<string> projects, string solutionFolder = "");

        /// <summary>
        /// Callback to be executed on first run of the template engine.
        /// </summary>
        public Action<IEngineEnvironmentSettings>? OnFirstRun { get; set; }

        /// <summary>
        /// Callback to be executed to restore a project.
        /// </summary>
        public RestoreProjectDelegate? RestoreProject { get; set; }

        /// <summary>
        /// Callback to be executed to add project references to project.
        /// </summary>
        public AddProjectReferencesDelegate? AddProjectReferences { get; set; }

        /// <summary>
        /// Callback to be executed to add package reference to project.
        /// </summary>
        public AddPackageReferenceDelegate? AddPackageReference { get; set; }

        /// <summary>
        /// Callback to be executed to add projects to solution.
        /// </summary>
        public AddProjectsToSolutionDelegate? AddProjectsToSolution { get; set; }
    }
}
