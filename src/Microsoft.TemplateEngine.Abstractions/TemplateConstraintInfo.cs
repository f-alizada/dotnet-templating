// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.TemplateEngine.Abstractions
{
    public class TemplateConstraintInfo
    {
        public string Type { get; }

        public Dictionary<string, string> Args { get; }
    }
}
