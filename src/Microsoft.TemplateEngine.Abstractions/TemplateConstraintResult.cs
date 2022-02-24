// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.TemplateEngine.Abstractions
{
    /// <summary>
    /// Result of <see cref="ITemplateConstraint.EvaluateAsync(ITemplateInfo)"/>.
    /// </summary>
    public class TemplateConstraintResult
    {
        public TemplateConstraintResult(bool visible, string localizedMessage)
        {
            Visible = visible;
            LocalizedMessage = localizedMessage;
        }

        /// <summary>
        /// Determines if <see cref="ITemplateInfo"/> should be visible or not.
        /// </summary>
        public bool Visible { get; }

        /// <summary>
        /// Localized message explaining to user why this template is filtered out.
        /// </summary>
        public string LocalizedMessage { get; }
    }
}

