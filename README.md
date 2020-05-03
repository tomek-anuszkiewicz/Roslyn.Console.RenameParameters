## Roslyn.Console.RenameParameters


#### Notes

Roslyn models are immutable, so when you apply change you create new solution.
So how to apply changes in loop. 

Lets apply first change. You have new solution.
Lets apply second change to original solution, you have new solution.
Now you have two solutions. 
Using `workspace.TryApplyChanges` on latter will overwrite first change.

Lets apply changes to `newSolution` and update it after every change.
When we try to apply changes for second parameter in 
`parameterWithUnderscoresSyntaxWalker.ParametersWithUnderscoresList`
it's point to old solution - roslyn throw exception.

That's why we are applying one rename at once in document.
Then we are searching in new document again.
Document.Id doesn't change in new solution.
