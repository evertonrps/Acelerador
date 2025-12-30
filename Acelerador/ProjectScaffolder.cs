// csharp
// Acelerador/ProjectScaffolder.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Acelerador;

public class ProjectScaffolder
{
    private readonly string _baseDirectory;
    private readonly DotnetRunner _runner;

    public ProjectScaffolder(string baseDirectory, DotnetRunner runner)
    {
        _baseDirectory = Path.GetFullPath(baseDirectory ?? Environment.CurrentDirectory);
        _runner = runner ?? throw new ArgumentNullException(nameof(runner));
    }

    public async Task<ScaffoldResult> ScaffoldAsync(ScaffoldOptions options, CancellationToken cancellationToken = default)
{
    var results = new List<CommandResult>();
    try
    {
        var targetRoot = Path.GetFullPath(Path.Combine(_baseDirectory, options.RelativePath ?? string.Empty));
        Directory.CreateDirectory(targetRoot);

        // criar solução
        var resSln = await _runner.RunAsync($"new sln -n {options.SolutionName}", workingDirectory: targetRoot, cancellationToken: cancellationToken);
        results.Add(resSln);
        if (!resSln.Success && options.FailOnCommandError) return new ScaffoldResult(false, results);

        // PRIMEIRO PASS: criar projetos e adicionar à solução
        foreach (var p in options.Projects)
        {
            var projectDir = Path.Combine(targetRoot, p.RelativeDirectory ?? p.Name);
            Directory.CreateDirectory(projectDir);

            var resNew = await _runner.RunAsync($"new {p.Template} -n {p.Name} -o .", workingDirectory: projectDir, cancellationToken: cancellationToken);
            results.Add(resNew);
            if (!resNew.Success && options.FailOnCommandError) return new ScaffoldResult(false, results);

            // excluir arquivo Class1.cs gerado automaticamente
            var class1Path = Path.Combine(projectDir, "Class1.cs");
            if (File.Exists(class1Path))
            {
                File.Delete(class1Path);
            }
            
            var csprojPath = Path.Combine(projectDir, $"{p.Name}.csproj");
            var relCsproj = Path.GetRelativePath(targetRoot, csprojPath);
            var resAdd = await _runner.RunAsync($"sln add \"{relCsproj}\"", workingDirectory: targetRoot, cancellationToken: cancellationToken);
            results.Add(resAdd);
            if (!resAdd.Success && options.FailOnCommandError) return new ScaffoldResult(false, results);

            // instalar pacotes (se houver)
            if (p.Packages != null && p.Packages.Count > 0)
            {
                foreach (var pkg in p.Packages)
                {
                    var parts = pkg.Split(new[] { ':' }, 2);
                    var pkgName = parts[0];
                    var versionArg = parts.Length == 2 ? $" --version {parts[1]}" : string.Empty;

                    var resPkg = await _runner.RunAsync($"add \"{p.Name}.csproj\" package {pkgName}{versionArg}", workingDirectory: projectDir, cancellationToken: cancellationToken);
                    results.Add(resPkg);
                    if (!resPkg.Success && options.FailOnCommandError) return new ScaffoldResult(false, results);
                }
            }
        }

        // SEGUNDO PASS: adicionar referências entre projetos (após todos serem criados)
        foreach (var p in options.Projects)
        {
            if (p.References == null || p.References.Count == 0) continue;

            var projectDir = Path.Combine(targetRoot, p.RelativeDirectory ?? p.Name);

            foreach (var refName in p.References)
            {
                var refProject = options.Projects.FirstOrDefault(x => string.Equals(x.Name, refName, StringComparison.OrdinalIgnoreCase));
                if (refProject == null) continue;

                var refCsprojAbsolute = Path.Combine(targetRoot, refProject.RelativeDirectory ?? refProject.Name, $"{refProject.Name}.csproj");
                var relPathFromProject = Path.GetRelativePath(projectDir, refCsprojAbsolute);

                var resRef = await _runner.RunAsync($"add \"{p.Name}.csproj\" reference \"{relPathFromProject}\"", workingDirectory: projectDir, cancellationToken: cancellationToken);
                results.Add(resRef);
                if (!resRef.Success && options.FailOnCommandError) return new ScaffoldResult(false, results);
            }
        }

        // escrever arquivos de template (com renderização de variáveis)
        foreach (var t in options.Templates)
        {
            var renderedRelativePath = RenderTemplate(t.RelativePathTemplate, options.Variables);
            var filePath = Path.Combine(targetRoot, renderedRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? targetRoot);

            var renderedContent = RenderTemplate(t.ContentTemplate, options.Variables);
            await File.WriteAllTextAsync(filePath, renderedContent, cancellationToken);
        }

        return new ScaffoldResult(true, results);
    }
    catch (OperationCanceledException)
    {
        return new ScaffoldResult(false, results, "Operação cancelada");
    }
    catch (Exception ex)
    {
        return new ScaffoldResult(false, results, ex.Message);
    }
}


    private static string RenderTemplate(string template, IDictionary<string, string>? variables)
    {
        if (string.IsNullOrEmpty(template) || variables == null || variables.Count == 0) return template ?? string.Empty;

        var result = template;
        foreach (var kv in variables)
        {
            // substitui ocorrências como [apiName] pelo valor
            var token = $"[{kv.Key}]";
            result = result.Replace(token, kv.Value, StringComparison.OrdinalIgnoreCase);
        }
        return result;
    }
}
