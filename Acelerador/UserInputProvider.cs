// Acelerador/Services/UserInputProvider.cs

using System;
using System.Collections.Generic;

namespace Acelerador;

public static class UserInputProvider
{
    public static UserInput? GatherInput()
    {
        var actions = new Dictionary<string, string>
        {
            ["1"] = "New Project",
            ["2"] = "Add new Entity"
        };

        Console.WriteLine("Selecione o código da opção desejada:");
        foreach (var item in actions)
            Console.WriteLine($"{item.Key} - {item.Value}");

        if (!actions.TryGetValue(Console.ReadLine() ?? string.Empty, out var option))
            return null;

        if (option != "New Project")
            throw new NotImplementedException();

        Console.WriteLine("Digite o nome do projeto: ");
        var apiName = Console.ReadLine();
        
        Console.WriteLine("Digite o nome da entidade:");
        var entityName = Console.ReadLine()?.Replace(" ", string.Empty) ?? string.Empty;

        Console.WriteLine("Digite o número da versão da API:");
        var apiVersion = Console.ReadLine() ?? "1";
        
        return new UserInput(apiName, entityName, apiVersion);
    }
}