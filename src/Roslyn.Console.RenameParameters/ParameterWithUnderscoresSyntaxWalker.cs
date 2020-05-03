using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Roslyn.Console.RenameParameters
{
    public class ParameterWithUnderscoresSyntaxWalker : CSharpSyntaxWalker
    {
        private readonly List<ParameterSyntax> _parametersWithUnderscoresList = new List<ParameterSyntax>();

        public IReadOnlyList<ParameterSyntax> ParametersWithUnderscoresList => _parametersWithUnderscoresList.AsReadOnly();

        public override void VisitParameter(ParameterSyntax node)
        {
            if (node.Identifier.ValueText.Contains("_") && (node.Identifier.ValueText.Length > 1))
                _parametersWithUnderscoresList.Add(node);

            base.VisitParameter(node);
        }
    }
}
