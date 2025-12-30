// Acelerador/ScaffoldOptions.cs
using System.Collections.Generic;

namespace Acelerador;

public class ScaffoldOptions
{
    public string SolutionName { get; set; } = string.Empty;
    public string? RelativePath { get; set; }
    public List<ProjectDefinition> Projects { get; set; } = new();
    public List<Template> Templates { get; set; } = new();
    public Dictionary<string, string> Variables { get; set; } = new();
    public bool FailOnCommandError { get; set; } = true;
}

public record ProjectDescriptor(string Name, string Template = "webapi", string? RelativeDirectory = null);