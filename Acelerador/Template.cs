// csharp
namespace Acelerador;

public class Template
{
    public Template(string relativePathTemplate, string contentTemplate)
    {
        RelativePathTemplate = relativePathTemplate;
        ContentTemplate = contentTemplate;
    }

    public string RelativePathTemplate { get; set; }
    public string ContentTemplate { get; set; }
}