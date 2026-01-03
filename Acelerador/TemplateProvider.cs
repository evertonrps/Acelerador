// Acelerador/Templates/TemplateProvider.cs

using System.Collections.Generic;

namespace Acelerador;

public static class TemplateProvider
{
    public static List<Template> GetProjectTemplates()
    {
        return new List<Template>
        {
            // Program.cs
            new Template(
                relativePathTemplate: @"[apiName].API\Program.cs",
                contentTemplate: GetProgramTemplate()
            ),

            // Controller
            new Template(
                relativePathTemplate: @"[apiName].API\Controllers\v[versao]\[Class]Controller.cs",
                contentTemplate: GetControllerTemplate()
            ),

            // ViewModel
            new Template(
                relativePathTemplate: @"[apiName].API\ViewModels\[Class]ViewModel.cs",
                contentTemplate: GetViewModelTemplate()
            ),

            // Mapper
            new Template(
                relativePathTemplate: @"[apiName].API\MapperExtensions\[Class]MapperExtension.cs",
                contentTemplate: GetMapperTemplate()
            ),

            // Domain - Entity
            new Template(
                relativePathTemplate: @"[apiName].Domain\Entities\[Class].cs",
                contentTemplate: GetEntityTemplate()
            ),

            // Domain - BaseEntity
            new Template(
                relativePathTemplate: @"[apiName].Domain\SeedWork\BaseEntity.cs",
                contentTemplate: GetBaseEntityTemplate()
            ),

            // Domain - AppSettings
            new Template(
                relativePathTemplate: @"[apiName].Domain\SeedWork\AppSettings.cs",
                contentTemplate: GetAppSettingsTemplate()
            ),

            // Domain - Service Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Services\I[Class]Service.cs",
                contentTemplate: GetServiceInterfaceTemplate()
            ),

            // Domain - Service
            new Template(
                relativePathTemplate: @"[apiName].Domain\Services\[Class]Service.cs",
                contentTemplate: GetServiceTemplate()
            ),

            // Domain - Repository Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Repositories\IRepository.cs",
                contentTemplate: GetRepositoryInterfaceTemplate()
            ),

            // Domain - Generic Repository Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Repositories\I[Class]Repository.cs",
                contentTemplate: GetGenericRepositoryInterfaceTemplate()
            ),

            // Domain - UnitOfWork Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Repositories\IUnitOfWork.cs",
                contentTemplate: GetUnitOfWorkInterfaceTemplate()
            ),

            // Domain - DbConnection Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Repositories\IDbConnectionFactory.cs",
                contentTemplate: GetDbConnectionFactoryTemplate()
            ),

            // Data - Repository
            new Template(
                relativePathTemplate: @"[apiName].Data\Repositories\[Class]Repository.cs",
                contentTemplate: GetRepositoryTemplate()
            ),

            // Data - OracleConnectionFactory
            new Template(
                relativePathTemplate: @"[apiName].Data\Repositories\OracleConnectionFactory.cs",
                contentTemplate: GetOracleConnectionFactoryTemplate()
            ),

            // Data - SqliteConnectionFactory
            new Template(
                relativePathTemplate: @"[apiName].Data\Repositories\SqliteConnectionFactory.cs",
                contentTemplate: GetSqliteConnectionFactoryTemplate()
            ),

            // Data - UnitOfWork
            new Template(
                relativePathTemplate: @"[apiName].Data\Repositories\UnitOfWork.cs",
                contentTemplate: GetUnitOfWorkTemplate()
            ),

            // IoC - BootStrapper
            new Template(
                relativePathTemplate: @"[apiName].IoC\BootStrapper.cs",
                contentTemplate: GetBootStrapperTemplate()
            ),

            // IoC - Database Initializer
            new Template(
                relativePathTemplate: @"[apiName].IoC\IDbInitializer.cs",
                contentTemplate: GetSqliteDbInitializer()
            ),
            // IoC - LogConfiguration
            new Template(
                relativePathTemplate: @"[apiName].IoC\LogConfiration.cs",
                contentTemplate: GetLogConfigurationTemplate()
            )
        };
    }
    
public static List<Template> GetEntityTemplates()
    {
        return new List<Template>
        {
            // Controller
            new Template(
                relativePathTemplate: @"[apiName].API\Controllers\v[versao]\[Class]Controller.cs",
                contentTemplate: GetControllerTemplate()
            ),

            // ViewModel
            new Template(
                relativePathTemplate: @"[apiName].API\ViewModels\[Class]ViewModel.cs",
                contentTemplate: GetViewModelTemplate()
            ),

            // Mapper
            new Template(
                relativePathTemplate: @"[apiName].API\MapperExtensions\[Class]MapperExtension.cs",
                contentTemplate: GetMapperTemplate()
            ),

            // Domain - Entity
            new Template(
                relativePathTemplate: @"[apiName].Domain\Entities\[Class].cs",
                contentTemplate: GetEntityTemplate()
            ),

            // Domain - Service Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Services\I[Class]Service.cs",
                contentTemplate: GetServiceInterfaceTemplate()
            ),

            // Domain - Service
            new Template(
                relativePathTemplate: @"[apiName].Domain\Services\[Class]Service.cs",
                contentTemplate: GetServiceTemplate()
            ),
            
            // Domain - Generic Repository Interface
            new Template(
                relativePathTemplate: @"[apiName].Domain\Interfaces\Repositories\I[Class]Repository.cs",
                contentTemplate: GetGenericRepositoryInterfaceTemplate()
            ),

            // Data - Repository
            new Template(
                relativePathTemplate: @"[apiName].Data\Repositories\[Class]Repository.cs",
                contentTemplate: GetRepositoryTemplate()
            ),
        };
    }    

    private static string GetProgramTemplate() => @"
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;
using [apiName].IoC;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(
    options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddApiExplorer(p=>
    {
        p.GroupNameFormat = ""'v'VVV"";
        p.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""[apiName]"", Version = ""v1"" });
    s.SwaggerDoc(""v2"", new OpenApiInfo { Title = ""[apiName]"", Version = ""v2"" });

    var xmlFile = $""{Assembly.GetExecutingAssembly().GetName().Name}.xml"";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
    /* To generate XML documentation file, add the following to the .csproj file:
     <GenerateDocumentationFile>true</GenerateDocumentationFile>
     <NoWarn>(NoWarn);1591</NoWarn>
    */

    s.AddSecurityDefinition(""Bearer"", new OpenApiSecurityScheme
    {
        Description = ""JWT Authorization"",
        Name = ""Authorization"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ""Bearer""
    });

    s.AddSecurityRequirement(document =>
        new()
        {
            [new OpenApiSecuritySchemeReference(""Bearer"", document)] = []
        });
});

BootStrapper.RegisterServices(builder.Services, builder.Configuration);
builder.Host.UseSerilog(Log.Logger);

var app = builder.Build();
await app.Services.InitializeDatabaseAsync(); //Configuração e inicialização do banco de dados em memória
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($""/swagger/{description.GroupName}/swagger.json"",
            description.GroupName.ToUpperInvariant());
    }

    options.DocExpansion(DocExpansion.List);
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
";

    private static string GetControllerTemplate() => @"
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using [apiName].Domain.Interfaces.Services;
using [apiName].API.ViewModel;
using [apiName].API.MapperExtensions;

namespace [apiName].API.Controllers.v[versao]
{
    [ApiVersion(""[versao]"")]
    [Route(""api/v{version:apiVersion}/[controller]"")]
    [ApiController]
    public class [Class]Controller : ControllerBase
    {
        private readonly I[Class]Service _[LowerName]Service;

        public [Class]Controller(I[Class]Service [LowerName]Service)
        {
            _[LowerName]Service = [LowerName]Service;
        }

		/// <summary>
        /// Create a new [Class]
        /// </summary>
        /// <param name=""model"">[Class]</param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof([Class]Model))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]        
        [HttpPost]
        public async Task<IActionResult> Post([Class]Model model)
        {
            var [LowerName] = await _[LowerName]Service.Create(model.ToEntity());
            return Ok([LowerName].ToViewModel());
        }

        /// <summary>
        /// Get [Class] by id
        /// </summary>
        /// <param name=""id"">[Class] ID</param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof([Class]Model))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet(""{id}"")]
        public async Task<IActionResult> GetById(string id)
        {
            var [LowerName] = await _[LowerName]Service.GetById(id);
            if ([LowerName] == null)
            {
                return NotFound(""[Class] not found"");
            }

            return Ok([LowerName].ToViewModel());
        }

        /// <summary>
        /// Get all [Class]s
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<[Class]Model>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var [LowerName]s = await _[LowerName]Service.GetAll();
            return Ok([LowerName]s.ToViewModelList());
        }

        /// <summary>
        /// Update [Class] by id
        /// </summary>
        /// <param name=""id"">[Class] ID</param>
        /// <param name=""model"">[Class]</param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof([Class]Model))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut(""{id}"")]
        public async Task<IActionResult> Put(string id, [Class]Model model)
        {
            var [LowerName] = await _[LowerName]Service.Update(id, model.ToEntity());
            return Ok([LowerName].ToViewModel());
        }

        /// <summary>
        /// Delete [Class] by id
        /// </summary>
        /// <param name=""id"">[Class] ID</param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete(""{id}"")]
        public async Task<IActionResult> Delete(string id)
        {
            var ret = await _[LowerName]Service.Delete(id);
            if (ret)
            {
                return Ok(ret);
            }

            return NotFound();
        }        

    }
}
";

    private static string GetViewModelTemplate() => @"
namespace [apiName].API.ViewModel
{
    public class [Class]Model
    {
        public [Class]Model() { }
        public string Id { get; set; }
    }
}
";

    private static string GetMapperTemplate() => @"
using [apiName].API.ViewModel;
using [apiName].Domain.Entities;

namespace [apiName].API.MapperExtensions
{
    public static class [Class]Extensions
    {
        public static [Class]Model ToViewModel(this [Class] [LowerName])
        {
            if ([LowerName] == null) return null;

            return new [Class]Model
            {
                Id = [LowerName].Id
            };
        }

        public static [Class] ToEntity(this [Class]Model [LowerName])
        {
            if ([LowerName] == null) return null;

            return [Class].Factory([LowerName].Id);
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
";

    private static string GetEntityTemplate() => @"
using System;
using System.Collections.Generic;
using [apiName].Domain.SeedWork;

namespace [apiName].Domain.Entities
{
    public class [Class] : BaseEntity
    {
        protected [Class]() { }

        private [Class](string id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static [Class] Factory(string id)
        {
            [Class] resource = new [Class](id);
            return resource;
        }
    }
}
";

    private static string GetBaseEntityTemplate() => @"
namespace [apiName].Domain.SeedWork;

public abstract class BaseEntity
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
";

    private static string GetAppSettingsTemplate() => @"
namespace [apiName].Domain.SeedWork;

public class AppSettings
{
    public const string Options = ""ApiConfiguration"";
    public string Environment { get; set; }
}
";

    private static string GetServiceInterfaceTemplate() => @"
using [apiName].Domain.Entities;

namespace [apiName].Domain.Interfaces.Services
{
    public interface I[Class]Service
    {
        Task<[Class]?> GetById(string id);
        Task<IEnumerable<[Class]>> GetAll();
        Task<[Class]> Create([Class] createEntity);
        Task<[Class]> Update(string id, [Class] updateEntity);
        Task<bool> Delete(string id);
    }
}
";

    private static string GetServiceTemplate() => @"
using [apiName].Domain.Entities;
using [apiName].Domain.Interfaces.Repositories;
using [apiName].Domain.Interfaces.Services;

namespace [apiName].Domain.Services;

public class [Class]Service : I[Class]Service
{
    private readonly IUnitOfWork _unitOfWork;

    public [Class]Service(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<[Class]?> GetById(string id)
    {
        var [LowerName] = await _unitOfWork.[Class]s.GetById(id);
        return [LowerName];
    }

    public async Task<IEnumerable<[Class]>> GetAll()
    {
        var [LowerName]s = await _unitOfWork.[Class]s.GetAll();
        return [LowerName]s;
    }

    public async Task<[Class]> Create([Class] createEntity)
    {
        var [LowerName] = [Class].Factory(createEntity.Id);

        await _unitOfWork.[Class]s.Add([LowerName]);
        await _unitOfWork.SaveChanges();

        return [LowerName];
    }

    public async Task<[Class]> Update(string id, [Class] updateEntity)
    {
        var [LowerName] = await _unitOfWork.[Class]s.GetById(id);

        if ([LowerName] == null)
            throw new KeyNotFoundException($""[Class] com ID {id} não encontrado"");

        [LowerName].UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.[Class]s.Update([LowerName]);
        await _unitOfWork.SaveChanges();

        return [LowerName];
    }

    public async Task<bool> Delete(string id)
    {
        var result = await _unitOfWork.[Class]s.Delete(id);

        if (result)
            await _unitOfWork.SaveChanges();

        return result;
    }
}
";

    private static string GetRepositoryInterfaceTemplate() => @"
using System;
using System.Threading.Tasks;
using [apiName].Domain.SeedWork;

namespace [apiName].Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(string id);
        Task<bool> Exists(string id);
    }
}
";

    private static string GetGenericRepositoryInterfaceTemplate() => @"
using [apiName].Domain.Entities;

namespace [apiName].Domain.Interfaces.Repositories
{
    public interface I[Class]Repository : IRepository<[Class]>
    {
    }
}
";

    private static string GetUnitOfWorkInterfaceTemplate() => @"
using System;
using System.Threading.Tasks;

namespace [apiName].Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    I[Class]Repository [Class]s { get; }
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
}

";

    private static string GetDbConnectionFactoryTemplate() => @"
using System.Data;

namespace [apiName].Domain.Interfaces.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
";

    private static string GetRepositoryTemplate() => @"
using Dapper;
using [apiName].Domain.Entities;
using [apiName].Domain.Interfaces.Repositories;

namespace [apiName].Data.Repositories;

public class [Class]Repository : I[Class]Repository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public [Class]Repository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<[Class]?> GetById(string id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @""
            SELECT Id, CreatedAt, UpdatedAt
            FROM [Class]
            WHERE Id = @Id"";

        return await connection.QueryFirstOrDefaultAsync<[Class]>(sql, new { Id = id });
    }

    public async Task<IEnumerable<[Class]>> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @""
            SELECT Id, CreatedAt, UpdatedAt
            FROM [Class]"";

        return await connection.QueryAsync<[Class]>(sql);
    }

    public async Task<[Class]> Add([Class] entity)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @""
            INSERT INTO [Class] (Id, CreatedAt, UpdatedAt)
            VALUES (@Id, @CreatedAt, @UpdatedAt)"";

        await connection.ExecuteAsync(sql, entity);

        return entity;
    }

    public async Task<[Class]> Update([Class] entity)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @""
            UPDATE [Class]
            SET UpdatedAt = @UpdatedAt
            WHERE Id = @Id"";

        await connection.ExecuteAsync(sql, entity);

        return entity;
    }

    public async Task<bool> Delete(string id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = ""DELETE FROM [Class] WHERE Id = @Id"";

        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

        return affectedRows > 0;
    }

    public async Task<bool> Exists(string id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = ""SELECT COUNT(1) FROM [Class] WHERE Id = @Id"";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });

        return count > 0;
    }
}
";

    private static string GetOracleConnectionFactoryTemplate() => @"
using System.Data;
using [apiName].Domain.Interfaces.Repositories;
using Oracle.ManagedDataAccess.Client;

namespace [apiName].Data.Repositories;

public class OracleConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public OracleConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
    {
        return new OracleConnection(_connectionString);
    }
}
";

    private static string GetSqliteConnectionFactoryTemplate() => @"
using System.Data;
using [apiName].Domain.Interfaces.Repositories;
using Microsoft.Data.Sqlite;

namespace [apiName].Data.Repositories;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
";

    private static string GetSqliteDbInitializer() => @"
using Microsoft.Data.Sqlite;

namespace [apiName].IoC;

public interface IDbInitializer
{
    Task InitializeAsync();
}
          
public class SqliteDbInitializer : IDbInitializer
{
    private readonly string _connectionString;

    public SqliteDbInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @""
            CREATE TABLE IF NOT EXISTS [Class] (
                Id TEXT PRIMARY KEY,
                CreatedAt DATE NOT NULL,
                UpdatedAt DATE NOT NULL
            )"";

            var ret = await command.ExecuteNonQueryAsync();
            Console.WriteLine(""Database initialized successfully. Result: "" + ret);
        }
        catch (Exception e)
        {
            Console.WriteLine(""ERROR: "" + e);
            throw;
        }
    }
}
";

    private static string GetUnitOfWorkTemplate() => @"
using System.Data;
using [apiName].Domain.Interfaces.Repositories;

namespace [apiName].Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private I[Class]Repository? _[LowerName]s;
    private bool _disposed;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public I[Class]Repository [Class]s
    {
        get
        {
            _[LowerName]s ??= new [Class]Repository(_connectionFactory);
            return _[LowerName]s;
        }
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(0);
    }

    public async Task BeginTransaction()
    {
        _connection ??= _connectionFactory.CreateConnection();

        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        _transaction = _connection.BeginTransaction();

        await Task.CompletedTask;
    }

    public async Task CommitTransaction()
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

    public async Task RollbackTransaction()
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
}
";

    private static string GetBootStrapperTemplate() => @"
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using [apiName].Data.Repositories;
using [apiName].Domain.Interfaces.Repositories;
using [apiName].Domain.Interfaces.Services;
using [apiName].Domain.SeedWork;
using [apiName].Domain.Services;

namespace [apiName].IoC;

public static class BootStrapper
{
    public static IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        //Serilog
        LogConfiguration.CreateLogger();
        services.AddLogging(builder => { builder.AddSerilog(); });

        //var connectionString = configuration.GetConnectionString(""DefaultConnection"")
        //                       ?? throw new InvalidOperationException(""Connection string 'DefaultConnection' não encontrada."");
        var connectionString = ""Data Source=[apiName].db"";
        services.Configure<AppSettings>(configuration.GetSection(AppSettings.Options));

        //Services
        services.AddScoped<I[Class]Service, [Class]Service>();

        //Repositories
        services.AddScoped<I[Class]Repository, [Class]Repository>();

        services.AddSingleton<IDbConnectionFactory>(sp => new SqliteConnectionFactory(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IDbInitializer>(sp => new SqliteDbInitializer(connectionString));

        return services;
    }

    //Configuração e inicialização do banco de dados em memória
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        var initializer = serviceProvider.GetRequiredService<IDbInitializer>();
        await initializer.InitializeAsync();
    }
}
";

    private static string GetLogConfigurationTemplate() => @"
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace [apiName].IoC;

public static class LogConfiguration
{
    public static void CreateLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override(""System.Net.Http.HttpClient"", LogEventLevel.Warning)
            .MinimumLevel.Override(""Microsoft.AspNetCore"", LogEventLevel.Warning)
            .MinimumLevel.Override(""Microsoft.EntityFrameworkCore"", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(
                outputTemplate:
                ""[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}{NewLine}"")
            .CreateLogger();
    }
}
";
}