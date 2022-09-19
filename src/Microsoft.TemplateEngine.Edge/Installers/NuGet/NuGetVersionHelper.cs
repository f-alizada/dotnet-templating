﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using NuGet.Versioning;

namespace Microsoft.TemplateEngine.Edge.Installers.NuGet
{
    internal static class NuGetVersionHelper
    {
        public static bool IsSupportedVersionString(string? versionString)
        {
            return
                string.IsNullOrEmpty(versionString)
                ||
                NuGetVersion.TryParse(versionString, out _)
                ||
                FloatRange.TryParse(versionString, out _);
        }

        /// <summary>
        /// Tries to parse given string and return <see cref="FloatRange"/> provided there
        ///  is an existing floating range behavior in the provided string.
        ///  null or empty string are regarded to be requests to any release version (behavior identical to '*').
        /// </summary>
        /// <param name="versionString">Input string to be parsed.</param>
        /// <param name="floatRange">Output <see cref="FloatRange"/> parameter, populated in case function returned true.</param>
        /// <returns></returns>
        public static bool TryParseFloatRangeEx(string? versionString, out FloatRange floatRange)
        {
            floatRange =
                string.IsNullOrEmpty(versionString) ?
                new FloatRange(NuGetVersionFloatBehavior.Major) : FloatRange.Parse(versionString);

            return floatRange != null && floatRange.FloatBehavior != NuGetVersionFloatBehavior.None;
        }
    }
}