// Acelerador/Services/UserInputProvider.cs
using System;
using System.Collections.Generic;

namespace Acelerador;

public static class UserInputProvider
{
    public static UserInput? GatherInput()
    {
        var apis = new Dictionary<string, string>
        {
            ["1"] = "MyProducts",
            ["2"] = "Basket"
        };

        Console.WriteLine("Selecione o código da API:");
        foreach (var item in apis)
            Console.WriteLine($"{item.Key} - {item.Value}");

        if (!apis.TryGetValue(Console.ReadLine() ?? string.Empty, out var apiName))
            return null;

        Console.WriteLine($"API: {apiName}");

        Console.WriteLine("Digite 1 para 1 => n ou 2 para n => n:");
        var relationType = Console.ReadLine();

        Console.WriteLine("Digite o nome da entidade agregadora:");
        var aggregateName = Console.ReadLine();

        Console.WriteLine("Digite o nome da entidade:");
        var entityName = Console.ReadLine()?.Replace(" ", string.Empty) ?? string.Empty;

        Console.WriteLine("Digite o número da versão da API:");
        var apiVersion = Console.ReadLine() ?? "1";

        return new UserInput(apiName, entityName, aggregateName, relationType, apiVersion);
    }
}
