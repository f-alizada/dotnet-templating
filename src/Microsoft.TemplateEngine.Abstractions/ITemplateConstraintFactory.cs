// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.TemplateEngine.Abstractions
{
    /// <summary>
    /// Template Constraint Factory.
    /// </summary>
    public interface ITemplateConstraintFactory : IIdentifiedComponent
    {
        /// <summary>
        /// Creates new <see cref="ITemplateConstraint"/> based on current <see cref="IEngineEnvironmentSettings"/>.
        /// </summary>
        public ITemplateConstraint CreateTemplateConstraint(IEngineEnvironmentSettings environmentSettings);
    }
}

