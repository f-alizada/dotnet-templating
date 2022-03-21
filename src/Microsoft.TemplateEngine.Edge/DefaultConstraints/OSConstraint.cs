// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.TemplateEngine.Edge.DefaultConstraints
{
    public class OSConstraintFactory : ITemplateConstraintFactory
    {
        public Guid Id { get; } = Guid.Parse("{73DE9788-264A-427B-A26F-2CA3911EE424}");

        public ITemplateConstraint CreateTemplateConstraint(IEngineEnvironmentSettings environmentSettings) => throw new NotImplementedException();

        internal class OSConstraint : ITemplateConstraint
        {
            public string Type => "os";

            public TemplateConstraintResult Evaluate(ITemplateInfo templateInfo)
            {
                var contstr = templateInfo.Constraints.Where(c => c.Type == Type);
                if (!contstr.Any())
                {
                    return new TemplateConstraintResult(true, null);
                }

            }
        }
    }
}
