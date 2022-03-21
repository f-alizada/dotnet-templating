// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.TemplateFiltering;
using Microsoft.TemplateEngine.Cli.Commands;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Utils;

namespace Microsoft.TemplateEngine.Cli.TemplateResolution
{
    /// <summary>
    /// Resolves the templates to be used for 'dotnet new &lt;name part&gt; --list'.
    /// </summary>
    internal class ListTemplateResolver : BaseTemplateResolver<ListCommandArgs>
    {
        private readonly IEngineEnvironmentSettings _engineEnvironmentSettings;

        public ListTemplateResolver(IEngineEnvironmentSettings engineEnvironmentSettings, TemplatePackageManager templatePackageManager, IHostSpecificDataLoader hostSpecificDataLoader)
            : base(templatePackageManager, hostSpecificDataLoader)
        {
            _engineEnvironmentSettings = engineEnvironmentSettings;
        }

        public ListTemplateResolver(IEngineEnvironmentSettings engineEnvironmentSettings, IEnumerable<ITemplateInfo> templateList, IHostSpecificDataLoader hostSpecificDataLoader)
       : base(templateList, hostSpecificDataLoader)
        {
            _engineEnvironmentSettings = engineEnvironmentSettings;
        }

        internal override async Task<TemplateResolutionResult> ResolveTemplatesAsync(ListCommandArgs args, string? defaultLanguage, CancellationToken cancellationToken)
        {
            IEnumerable<TemplateGroup> templateGroups = await GetTemplateGroupsAsync(cancellationToken).ConfigureAwait(false);
            IEnumerable<Func<TemplateGroup, MatchInfo?>> groupFilters = new[]
            {
                CliFilters.NameTemplateGroupFilter(args.ListNameCriteria)
            };
            //--ignore-constraints
            var constraintsFactories = _engineEnvironmentSettings.Components.OfType<ITemplateConstraintFactory>();
            var constraints = constraintsFactories.Select(f => f.CreateTemplateConstraint(_engineEnvironmentSettings));

            IEnumerable<Func<ITemplateInfo, MatchInfo?>> templateFilters =
                args.AppliedFilters
                    .OfType<TemplateFilterOptionDefinition>()
                    .Select(filter => filter.TemplateMatchFilter(args.GetFilterValue(filter)))
                    .Append(WellKnownSearchFilters.ConstraintFilter(constraints));

            IEnumerable<TemplateGroupMatchInfo> matchInformation =
                templateGroups.Select(
                    group =>
                        TemplateGroupMatchInfo.ApplyFilters(
                            group,
                            groupFilters,
                            templateFilters,
                            CliFilters.EmptyTemplateParameterFilter()));

            return new TemplateResolutionResult(matchInformation, this);
        }
    }
}
