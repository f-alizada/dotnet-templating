// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Microsoft.TemplateEngine.Abstractions
{
    /// <summary>
    /// Constraint that can be used to filter out <see cref="ITemplateInfo"/>.
    /// </summary>
    public interface ITemplateConstraint
    {
        string Type { get; }

        /// <summary>
        /// Evaluates <see cref="ITemplateInfo"/> and returns <see cref="TemplateConstraintResult"/> containing result.
        /// </summary>
        public TemplateConstraintResult Evaluate(ITemplateConstraintInfo templateInfo);
    }
}

