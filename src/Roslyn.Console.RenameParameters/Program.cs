using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Rename;
using System.Linq;

namespace Roslyn.Console.RenameParameters
{
    public static class Program
    {
        public static void Main()
        {
            MSBuildLocator.RegisterDefaults();

            using var workspace = MSBuildWorkspace.Create();

            var solution = workspace.OpenSolutionAsync(@"D:\Programowanie\C#\Moje programy\Raytracer\Raytracer.sln").Result;
            var newSolution = solution;

            foreach (var diagnostic in workspace.Diagnostics)
                System.Console.WriteLine(diagnostic);

            var documentIds = solution.Projects.SelectMany(el => el.DocumentIds).ToList();

            foreach (var documentId in documentIds)
            {
                var document = newSolution.GetDocument(documentId);

                if (document.SourceCodeKind == SourceCodeKind.Script)
                    continue;

                System.Console.WriteLine($"{documentIds.IndexOf(documentId)} / {documentIds.Count} - {document.Name}");

                for (; ;)
                {
                    var parameterWithUnderscoresSyntaxWalker = new ParameterWithUnderscoresSyntaxWalker();
                    var syntaxTree = document.GetSyntaxTreeAsync().Result;
                    var syntaxTreeRoot = syntaxTree.GetRoot();
                    parameterWithUnderscoresSyntaxWalker.Visit(syntaxTreeRoot);

                    if (parameterWithUnderscoresSyntaxWalker.ParametersWithUnderscoresList.Count == 0)
                        break;

                    var parameterNodeToRename = parameterWithUnderscoresSyntaxWalker.ParametersWithUnderscoresList[0];

                    System.Console.WriteLine($"  Renaming {parameterNodeToRename.Identifier.ValueText}");

                    var newName = ParameterRenamer.Rename(parameterNodeToRename.Identifier.ValueText);
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    var parameterSymbol = semanticModel.GetDeclaredSymbol(parameterNodeToRename);

                    newSolution = Renamer.RenameSymbolAsync(newSolution, parameterSymbol, newName, newSolution.Options).Result;

                    document = newSolution.GetDocument(documentId);
                }
            }

            if (solution != newSolution)
            {
                System.Console.WriteLine("Applaying changes");
                workspace.TryApplyChanges(newSolution);
            }

            foreach (var diagnostic in workspace.Diagnostics)
                System.Console.WriteLine(diagnostic);

            System.Console.WriteLine("Finished");
            System.Console.ReadKey();
        }
    }
}
