// csharp
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Acelerador;

public class TemplateRenderer
{
    private readonly IDictionary<string, string> _tokens;

    public TemplateRenderer(IDictionary<string, string> tokens)
    {
        _tokens = tokens ?? new Dictionary<string, string>();
    }

    // Substitui placeholders do tipo [Key] por valores em _tokens
    public string Render(string template)
    {
        if (string.IsNullOrEmpty(template)) return template ?? string.Empty;

        string result = template;
        foreach (var kv in _tokens)
        {
            var placeholder = $"[{kv.Key}]";
            result = result.Replace(placeholder, kv.Value ?? string.Empty);
        }

        return result;
    }

    // Renderiza e grava todos os templates em targetRoot, retornando os caminhos gravados
    public async Task<List<string>> RenderAndWriteAllAsync(IEnumerable<Template> templates, string targetRoot)
    {
        var created = new List<string>();
        foreach (var t in templates)
        {
            var relPath = Render(t.RelativePathTemplate);
            var fullPath = Path.GetFullPath(Path.Combine(targetRoot, relPath));
            var dir = Path.GetDirectoryName(fullPath) ?? targetRoot;
            Directory.CreateDirectory(dir);
            await File.WriteAllTextAsync(fullPath, Render(t.ContentTemplate)).ConfigureAwait(false);
            created.Add(fullPath);
        }
        return created;
    }
}