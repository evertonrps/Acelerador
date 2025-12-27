using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Acelerador
{
    class Program
    {
        static string apiName;
        static string projectDirectory;
        private static string tipo;
        private static string aggregate;
        private static string ClassName;
        private static string LowerName;
        private static string versao;
        private static string output;
        private static string error;
        const string quote = "\"";

        static void Main(string[] args)
        {


            var lista = new Dictionary<string, string>();

            lista.Add("1", "MyProducts");
            lista.Add("2", "Basket");

            Console.WriteLine("Hello World!");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.WriteLine($"Selecione o codigo da API");

            foreach (var item in lista)
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }

            lista.TryGetValue(Console.ReadLine(), out apiName);

            Console.WriteLine($"API {apiName}");

            projectDirectory =@$"{GetDirectory}{apiName}\src\";

            Console.WriteLine($"Digite 1 para 1 => n ou 2 para n => n");
            tipo = Console.ReadLine();

            Console.WriteLine($"Digite o nome da entidade agregadora");
            aggregate = Console.ReadLine();

            Console.WriteLine($"Digite o nome da entidade");
            ClassName = Console.ReadLine();
            ClassName = ClassName.Replace(" ", string.Empty);
            LowerName = Char.ToLowerInvariant(ClassName[0]) + ClassName.Substring(1);
            
            Console.WriteLine($"Digite o número da versão da API");
            versao = Console.ReadLine();
            System.IO.Directory.CreateDirectory(projectDirectory);
            RunDotNetCommand($"new sln -n {apiName}", out output, out error); //Blank Solution
            RunDotNetCommand($"new webapi -n {apiName}.API -o {apiName}.API", out output, out error); //Web API Project
            RunDotNetCommand($"new classlib -n {apiName}.Domain -o {apiName}.Domain", out output, out error); //Domain Project
            RunDotNetCommand($"new classlib -n {apiName}.Data -o {apiName}.Data", out output, out error); //Infrastructure Data Project
            RunDotNetCommand($"new classlib -n {apiName}.IoC -o {apiName}.IoC", out output, out error); //Infrastructure IoC Project
            
            RunDotNetCommand($"sln add {apiName}.API/{apiName}.API.csproj", out output, out error); //Add API to Solution
            RunDotNetCommand($"sln add {apiName}.Domain/{apiName}.Domain.csproj", out output, out error); //Add Domain to Solution
            RunDotNetCommand($"sln add {apiName}.Data/{apiName}.Data.csproj", out output, out error); //Add Data to Solution
            RunDotNetCommand($"sln add {apiName}.IoC/{apiName}.IoC.csproj", out output, out error); //Add IoC to Solution
            
            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Saída do comando dotnet:");
                Console.WriteLine(output);
            }

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Erros do comando dotnet:");
                Console.WriteLine(error);
            }

            if (tipo.Equals("1"))
            {
                FileText();
            }
            else
            {
                FileTextManyToMany();
            }
            
            Console.ReadLine();
        }

        private static void FileText()
        {
            string[,] array = new string[15, 3] {
            #region Controller
                {@$"{projectDirectory}\{apiName}.API\Controllers\v{versao}\{ClassName}Controller.cs",
                    @"
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace [apiName].API.Controllers.v1
{        
    [ApiVersion(""1"")]
    [Route(""api/v{version:apiVersion}/[Controller]"")]
    public class [Class]Controller : ControllerBase
    {                
        private readonly I[Class]Service _[LowerName]Service;

        public [Class]Controller(I[Class]Service [LowerName]Service)        
        {            
            _[LowerName]Service = [LowerName]Service;
        }
    }
}
",
                    "" },
            #endregion
            #region Mapper
                {@$"{projectDirectory}\{apiName}.API\MapperExtensions\{ClassName}MapperExtension.cs",
@"using System.Linq;
using System.Collections.Generic;

namespace [apiName].API.MapperExtensions
{
    public static class [Class]Extensions
    {
        public static [Class]Model ToViewModel(this [Class] [LowerName])
        {                                  
            if ([LowerName] == null) return null;

            return new [Class]Model
            {
                [Class]ID = [LowerName].[Class]ID
            };
        }

        public static [Class] ToEntity(this [Class]Model [LowerName])
        {                                  
            if ([LowerName] == null) return null;

            return new [Class]
            {
                [Class]ID = [LowerName].[Class]ID
            };
        }

        public static IEnumerable<[Class]Model> ToViewModelList(this IEnumerable<[Class]> [LowerName]List)
        {
            if ([LowerName]List == null) return null;

            return [LowerName]List.Select(c => c.ToViewModel());
        }

        public static IEnumerable<[Class]> ToEntityList(this IEnumerable<[Class]Model> [LowerName]ModelList)
        {
            if ([LowerName]ModelList == null) return null;

            return [LowerName]ModelList.Select(c => c.ToEntity());
        }
    }
}
",
                   "" },
                #endregion
            #region ViewModel	
                 {@$"{projectDirectory}\{apiName}.API\ViewModels\{ClassName}ViewModel.cs",
                    @"
using System;
using System.Collections.Generic;

namespace [apiName].API.ViewModel
{
    public class [Class]Model : EntityModel
    {
        public [Class]Model() { }
        public Guid [Class]ID { get; set; }      
    }
}",
                    ""
                },
#endregion
            #region Domain               
               {@$"{projectDirectory}\{apiName}.Domain\Entities\{ClassName}.cs",
@"using System;
using System.Collections.Generic;

namespace [apiName].Domain.Entities
{
    public class [Class] : Entity
    {
        protected [Class]() { }

        private [Class](int id)
        { 
            
        }

        public static [Class] Factory(int id) 
        {
            [Class] resource = new [Class](id);
                
            resource.ValidateNow(new [Class]Validator(), resource);

            return resource;
        }

    }
}", ""
               },
               {@$"{projectDirectory}\{apiName}.Domain\SeedWork\BaseEntity.cs",
                   @"namespace [apiName].Domain.SeedWork;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
", ""
               },               
                #endregion
            #region Validator                                   
                    {@$"{projectDirectory}\{apiName}.Domain\Validators\{ClassName}Validator.cs",
@"
using FluentValidation;

namespace [apiName].Domain.Validators
{
    public class [Class]Validator : EntityValidator<[Class]>
    {
        public [Class]Validator()
        {
        }
    }
}",
"",
                },
            #endregion
            #region Domain.Interfaces.Services  		       
                {@$"{projectDirectory}\{apiName}.Domain\Iterfaces\Services\I{ClassName}Service.cs",
@"using System;
using System.Threading.Tasks;
using [apiName].Domain.Interfaces

namespace [apiName].Domain.Interfaces.Services

{
    public interface I[Class]Service
    {
        Task<[Class]?> GetById(Guid id);
        Task<IEnumerable<[Class]>> GetAll();
        Task<IEnumerable<[Class]>> GetActiveProducts();
        Task<[Class]> Create([Class] createDto);
        Task<[Class]> Update(Guid id, [Class] updateDto);
        Task<bool> Delete(Guid id);
        Task<bool> UpdateStock(Guid id, int quantity);
    }
}", ""
               },
                #endregion
            #region Domain.Interfaces.Repositories
                {@$"{projectDirectory}\{apiName}.Domain\Iterfaces\Repositories\IRepository.cs",
                    @"using System;
using System.Threading.Tasks;

namespace [apiName].Domain.Interfaces.Repositories

{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(Guid id);
        Task<bool> Exists(Guid id);
    }
}", ""
                },                
                {@$"{projectDirectory}\{apiName}.Domain\Iterfaces\Repositories\I{ClassName}Repository.cs",
                    @"using System;
using System.Threading.Tasks;

namespace [apiName].Domain.Interfaces.Repositories

{
    public interface I[Class]Repository : IRepository<[Class]>
    {
    }
}", ""
                },
                #endregion
            #region Domain.Interfaces.UnitOfWork
                
                {@$"{projectDirectory}\{apiName}.Domain\Iterfaces\Repositories\IUnitOfWork.cs",
                    @"
namespace [apiName].Domain.Interfaces.Repositories

{
public interface IUnitOfWork : IDisposable
{
    I[Class]Repository Products { get; }
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
}
}", ""
                },  
                

                #endregion
            #region Domain.Interfaces.Infrastructure
                
                {@$"{projectDirectory}\{apiName}.Domain\Iterfaces\Repositories\IDbConnectionFactory.cs",
                    @"
namespace [apiName].Domain.Interfaces.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
", ""
                },  
                

                #endregion                
            #region Domain.Services  		       
                {@$"{projectDirectory}\{apiName}.Domain\Services\{ClassName}Service.cs",
                    @"
namespace [apiName].Domain.Services;

public class [Class]Service : I[Class]Service
{
    private readonly IUnitOfWork _unitOfWork;

    public [Class]Service(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<[Class]Dto?> GetByIdAsync(Guid id)
    {
        var [Class] = await _unitOfWork.[Class]s.GetByIdAsync(id);
        return [Class] == null ? null : MapToDto([Class]);
    }

    public async Task<IEnumerable<[Class]Dto>> GetAllAsync()
    {
        var [Class]s = await _unitOfWork.[Class]s.GetAllAsync();
        return [Class]s.Select(MapToDto);
    }

    public async Task<IEnumerable<[Class]Dto>> GetActive[Class]sAsync()
    {
        var [Class]s = await _unitOfWork.[Class]s.GetActive[Class]sAsync();
        return [Class]s.Select(MapToDto);
    }

    public async Task<[Class]Dto> CreateAsync(Create[Class]Dto createDto)
    {
        var [Class] = new [Class]
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _unitOfWork.[Class]s.AddAsync([Class]);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto([Class]);
    }

    public async Task<[Class]Dto> UpdateAsync(Guid id, Create[Class]Dto updateDto)
    {
        var [Class] = await _unitOfWork.[Class]s.GetByIdAsync(id);
        
        if ([Class] == null)
            throw new KeyNotFoundException($""Produto com ID {id} não encontrado"");

        [Class].Name = updateDto.Name;
        [Class].Description = updateDto.Description;
        [Class].Price = updateDto.Price;
        [Class].StockQuantity = updateDto.StockQuantity;
        [Class].UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.[Class]s.UpdateAsync([Class]);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto([Class]);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _unitOfWork.[Class]s.DeleteAsync(id);
        
        if (result)
            await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<bool> UpdateStockAsync(Guid id, int quantity)
    {
        var [Class] = await _unitOfWork.[Class]s.GetByIdAsync(id);
        
        if ([Class] == null)
            return false;

        [Class].UpdateStock(quantity);
        await _unitOfWork.[Class]s.UpdateAsync([Class]);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static [Class]Dto MapToDto([Class] [Class])
    {
        return new [Class]Dto
        {
            Id = [Class].Id,
            Name = [Class].Name,
            Description = [Class].Description,
            Price = [Class].Price,
            StockQuantity = [Class].StockQuantity,
            IsActive = [Class].IsActive,
            CreatedAt = [Class].CreatedAt,
            UpdatedAt = [Class].UpdatedAt
        };
    }
}", ""
                }, 
                #endregion    
            #region Repository               
                {@$"{projectDirectory}\{apiName}.Data\Repositories\{ClassName}Repository.cs",
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace [apiName].Data.Repositories;
         
public class [Class]Repository : I[Class]Repository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = @""
            SELECT Id, Name, Description, Price, StockQuantity, IsActive, CreatedAt, UpdatedAt 
            FROM Products 
            WHERE Id = @Id"";

        return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = @""
            SELECT Id, Name, Description, Price, StockQuantity, IsActive, CreatedAt, UpdatedAt 
            FROM Products"";

        return await connection.QueryAsync<Product>(sql);
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = @""
            SELECT Id, Name, Description, Price, StockQuantity, IsActive, CreatedAt, UpdatedAt 
            FROM Products 
            WHERE IsActive = 1"";

        return await connection.QueryAsync<Product>(sql);
    }

    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = @""
            SELECT Id, Name, Description, Price, StockQuantity, IsActive, CreatedAt, UpdatedAt 
            FROM Products 
            WHERE Price BETWEEN @MinPrice AND @MaxPrice"";

        return await connection.QueryAsync<Product>(sql, new { MinPrice = minPrice, MaxPrice = maxPrice });
    }

    public async Task<Product> AddAsync(Product entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = @""
            INSERT INTO Products (Id, Name, Description, Price, StockQuantity, IsActive, CreatedAt)
            VALUES (@Id, @Name, @Description, @Price, @StockQuantity, @IsActive, @CreatedAt)"";

        await connection.ExecuteAsync(sql, entity);
        
        return entity;
    }

    public async Task<Product> UpdateAsync(Product entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = @""
            UPDATE Products 
            SET Name = @Name, 
                Description = @Description, 
                Price = @Price, 
                StockQuantity = @StockQuantity, 
                IsActive = @IsActive, 
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id"";

        await connection.ExecuteAsync(sql, entity);
        
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = ""DELETE FROM Products WHERE Id = @Id"";

        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        
        return affectedRows > 0;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        
        const string sql = ""SELECT COUNT(1) FROM Products WHERE Id = @Id"";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
        
        return count > 0;
    }
}",
                "",
                
                },
	        #endregion
            #region Domain.Infrastructure.Data
                
            {@$"{projectDirectory}\{apiName}.Data\Repositories\SqlConnectionFactory.cs",
                @"using System.Data;
using Microsoft.Data.SqlClient;

namespace [apiName].Data.Repositories;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}", ""
            },  
                

            #endregion
            #region Domain.Infrastructure.Data2
                
            {@$"{projectDirectory}\{apiName}.Data\Repositories\UnitOfWork.cs",
                @"using System.Data;
using [apiName].Domain.Interfaces;
using [apiName].Repositories;

namespace [apiName].Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private I[Class]Repository? _[Class]s;
    private bool _disposed;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public I[Class]Repository [Class]s
    {
        get
        {
            _[Class]s ??= new [Class]Repository(_connectionFactory);
            return _[Class]s;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Com Dapper, as mudanças são salvas imediatamente
        // Este método é mantido para compatibilidade com o padrão Unit of Work
        return await Task.FromResult(0);
    }

    public async Task BeginTransactionAsync()
    {
        _connection ??= _connectionFactory.CreateConnection();
        
        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        _transaction = _connection.BeginTransaction();
        
        await Task.CompletedTask;
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException(""Nenhuma transação ativa"");

        try
        {
            _transaction.Commit();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }

        await Task.CompletedTask;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException(""Nenhuma transação ativa"");

        try
        {
            _transaction.Rollback();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }
}", ""
            },  
                

            #endregion              
            };

            for (int i = 0; i < array.Length / 3; i++)
            {
                string path = array[i, 0].Replace("[Class]", ClassName).Replace("[Quote]", quote).Replace("[apiName]", apiName).Replace("[aggregate]", aggregate);
                string text = array[i, 1].Replace("[Class]", ClassName).Replace("[Quote]", quote).Replace("[apiName]", apiName).Replace("[aggregate]", aggregate)
                    .Replace("[LowerName]", LowerName).Replace("[Quote]", quote); ;

                WriteCs(path, text);
            }
        }

        private static void WriteCs(string path, string lines)
        {
            if (!System.IO.Directory.Exists(path))
            {

                if (!string.IsNullOrEmpty(projectDirectory))
                {
                    //https://stackoverflow.com/questions/2660723/remove-characters-after-specific-character-in-string-then-remove-substring/25965143
                    string path1 = path.Substring(0, path.LastIndexOf("\\") + 1);
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(path1);

                }

                string localPath = new Uri(path).LocalPath;

                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(localPath);
                file.WriteLine(lines);
                file.Close();

                Console.Write("File created to " + path + "\n");
            }
        }

        private static void FileTextManyToMany()
        {
            throw new NotImplementedException();
        }

        private static string GetDirectory
        {
            get
            {
                return Path.GetFullPath(@"..\..\..\..\..\");
            }
        }
        
        static void RunDotNetCommand(string arguments, out string output, out string error, string workingDirectory = null)
        {
            // Configurações para iniciar o processo 'dotnet'
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "dotnet", // O nome do executável (deve estar no PATH do sistema)
                Arguments = arguments, // Os argumentos para o comando dotnet (ex: "--version", "run", "build")
                UseShellExecute = false, // Não usar o shell do sistema operacional para iniciar o processo
                RedirectStandardOutput = true, // Redirecionar a saída padrão para o C#
                RedirectStandardError = true, // Redirecionar a saída de erro para o C#
                CreateNoWindow = true // Não criar uma janela de console separada
            };
            
            if (!string.IsNullOrWhiteSpace(projectDirectory))
            {
                startInfo.WorkingDirectory = @$"{projectDirectory}\{workingDirectory}";
            }

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Lê toda a saída e erro padrão de forma assíncrona ou síncrona
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();

                process.WaitForExit(); // Aguarda o processo terminar
            }
        }
    }
}
