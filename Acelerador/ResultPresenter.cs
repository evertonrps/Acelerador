// Acelerador/Presenters/ResultPresenter.cs
using System;

namespace Acelerador;

public static class ResultPresenter
{
    public static void Display(ScaffoldResult result)
    {
        Console.WriteLine("\n" + new string('=', 60));

        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Projeto criado com sucesso!");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("✗ Erro ao criar projeto!");
            if (!string.IsNullOrEmpty(result.ErrorMessage))
                Console.WriteLine($"Detalhes: {result.ErrorMessage}");
            Console.ResetColor();
        }

        if (result.CommandResults.Count > 0)
        {
            Console.WriteLine($"\nComandos executados: {result.CommandResults.Count}");
            foreach (var cmd in result.CommandResults)
            {
                if (!cmd.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Aviso: {cmd.StdErr}");
                    Console.ResetColor();
                }
            }
        }

        Console.WriteLine(new string('=', 60));
    }
}