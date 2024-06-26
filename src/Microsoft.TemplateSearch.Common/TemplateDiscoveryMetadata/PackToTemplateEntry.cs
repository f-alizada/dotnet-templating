// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Microsoft.TemplateSearch.Common
{
    [Obsolete("The class is deprecated. Use TemplateSearchCache instead to create search cache data.")]
    internal class PackToTemplateEntry
    {
        internal PackToTemplateEntry(string version, List<TemplateIdentificationEntry> templateIdentificationEntry)
        {
            Version = version;
            TemplateIdentificationEntry = templateIdentificationEntry;
        }

        [JsonProperty]
        internal string Version { get; }

        [JsonProperty]
        internal long TotalDownloads { get; set; }

        [JsonProperty]
        internal IReadOnlyList<string> Owners { get; set; } = [];

        [JsonProperty]
        internal bool Reserved { get; set; }

        [JsonProperty]
        internal IReadOnlyList<TemplateIdentificationEntry> TemplateIdentificationEntry { get; }
    }
}
