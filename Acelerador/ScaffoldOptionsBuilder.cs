// Acelerador/Builders/ScaffoldOptionsBuilder.cs

using System.Collections.Generic;

namespace Acelerador;

public static class ScaffoldOptionsBuilder
{
    public static ScaffoldOptions Build(UserInput input)
    {
        var tokens = new Dictionary<string, string>
        {
            ["apiName"] = input.ApiName,
            ["Class"] = input.EntityName,
            ["LowerName"] = ToLowerFirstChar(input.EntityName),
            ["versao"] = input.ApiVersion
        };

        var templates = TemplateProvider.GetTemplates();
        var renderer = new TemplateRenderer(tokens);

        // Renderizar os templates ANTES de criar ScaffoldOptions
        var renderedTemplates = RenderTemplates(templates, renderer);

        var options = new ScaffoldOptions
        {
            SolutionName = input.ApiName,
            FailOnCommandError = false,
            Projects = BuildProjectDescriptors(input.ApiName),
            Templates = renderedTemplates
        };

        return options;
    }

    private static List<Template> RenderTemplates(List<Template> templates, TemplateRenderer renderer)
    {
        var renderedTemplates = new List<Template>();

        foreach (var template in templates)
        {
            var renderedPath = renderer.Render(template.RelativePathTemplate);
            var renderedContent = renderer.Render(template.ContentTemplate);

            renderedTemplates.Add(new Template(renderedPath, renderedContent));
        }

        return renderedTemplates;
    }

    private static List<ProjectDefinition> BuildProjectDescriptors(string apiName)
    {
        return new List<ProjectDefinition>
        {
            new()
            {
                Name = apiName + ".API", 
                Template = "webapi", 
                Packages = new List<string>
                {
                    "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer", "Swashbuckle.AspNetCore"
                },
                References = new List<string>
                {
                    apiName + ".Domain",
                    apiName + ".IoC"
                }
            },
            new() { Name = apiName + ".Domain", Template = "classlib" },
            new()
            {
                Name = apiName + ".Data", 
                Template = "classlib", 
                Packages = new List<string>
                {
                    "Dapper", 
                    "Oracle.ManagedDataAccess.Core",
                    "Microsoft.Data.Sqlite"
                },References = new List<string>
                {
                    apiName + ".Domain"
                }
            },
            new()
            {
                Name = apiName + ".IoC", 
                Template = "classlib", 
                Packages = new List<string>
                {
                    "Microsoft.Extensions.DependencyInjection.Abstractions",
                    "Microsoft.Extensions.Configuration",
                    "Microsoft.Extensions.Options.ConfigurationExtensions",
                    "Microsoft.Data.Sqlite",
                    "Serilog",
                    "Serilog.AspNetCore",
                    "Serilog.Exceptions",
                    "Serilog.Sinks.Console",
                },
                References = new List<string>
                {
                    apiName + ".Domain",
                    apiName + ".Data"
                }
            }
        };
    }

    private static string ToLowerFirstChar(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}