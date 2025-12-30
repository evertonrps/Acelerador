using System.Collections.Generic;

namespace Acelerador;

public class ProjectDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string? RelativeDirectory { get; set; }
    public List<string> References { get; set; } = new();
    public List<string> Packages { get; set; } = new();
}