using Luthetus.CompilerServices.DotNetSolution.Models.Project;

namespace Luthetus.Ide.RazorLib.ComponentRenderers.Models;

public interface ITreeViewCSharpProjectToProjectReferenceRendererType
{
    public CSharpProjectToProjectReference CSharpProjectToProjectReference { get; set; }
}