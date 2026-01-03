// Acelerador/Program.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Acelerador;

class Program
{
    static async Task Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();

        // 1. Obter entrada do usuário
        var input = UserInputProvider.GatherInput();
        if (input == null)
        {
            Console.WriteLine("Operação cancelada.");
            return;
        }

        if (input.Type == "New Project")
        {
            // 2. Criar runner e scaffolder
            var baseDir = DirectoryHelper.GetBaseDirectory();
            var projectDir = DirectoryHelper.GetProjectDirectory(baseDir, input.ApiName);
            Directory.CreateDirectory(projectDir);

            var runner = new DotnetRunner(projectDir);
            var scaffolder = new ProjectScaffolder(projectDir, runner);

            // 3. Executar scaffold
            Console.WriteLine($"\nCriando projeto {input.ApiName}...\n");
            var scaffoldOptions = ScaffoldOptionsBuilder.ProjectBuild(input);
            var result = await scaffolder.ScaffoldAsync(scaffoldOptions);

            // 4. Exibir resultado
            ResultPresenter.Display(result);

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadLine();
        }
        else
        {
            // 2. Criar runner e scaffolder
            var baseDir = DirectoryHelper.GetBaseDirectory();
            var projectDir = DirectoryHelper.GetProjectDirectory(baseDir, input.ApiName);

            var runner = new DotnetRunner(projectDir);
            var scaffolder = new ProjectScaffolder(projectDir, runner);

            // 3. Executar scaffold
            Console.WriteLine($"\nCriando entidade {input.EntityName}...\n");
            var scaffoldOptions = ScaffoldOptionsBuilder.EntityBuild(input);
            var result = await scaffolder.ScaffoldEntityAsync(scaffoldOptions);

            // 4. Exibir resultado
            ResultPresenter.Display(result);

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadLine();
        }
    }
}