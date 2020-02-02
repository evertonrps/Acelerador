using System;
using System.Collections.Generic;
using System.IO;

namespace Acelerador
{
    class Program
    {
        static string apiName;
        static string projectDirectory;
        private static string tipo;
        private static string aggregate;
        public static string ClassName;
        const string quote = "\"";

        static void Main(string[] args)
        {


            var lista = new Dictionary<string, string>();

            lista.Add("1", "Ordering");
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

            projectDirectory =@$"{GetDirectory}eShopOnContainers\src\Services\{apiName}";

            Console.WriteLine($"Digite 1 para 1 => n ou 2 para n => n");
            tipo = Console.ReadLine();

            Console.WriteLine($"Digite o nome da entidade agregadora");
            aggregate = Console.ReadLine();

            Console.WriteLine($"Digite o nome da entidade");
            ClassName = Console.ReadLine();
            ClassName = ClassName.Replace(" ", string.Empty);

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
            string[,] array = new string[8, 3] {
            #region Controller
                {@$"{projectDirectory}\{apiName}.API\Controllers\{aggregate}\{ClassName}Controller.cs",
                    @"
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Services.[apiName].API.Controllers
{
    [Authorize]
    [Route([Quote]api/[Class][Quote])]
    public class [Class]Controller : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly I[Class]Repository _repository;

        public [Class]Controller(I[Class]Repository repository, IMapper mapper)        
        {
            _mapper = mapper;
            _repository = repository;
        }
    }
}
",
                    "" },
            #endregion
            #region Mapper
                {@$"{projectDirectory}\{apiName}.API\Mapper\{aggregate}\{ClassName}Mapper.cs",
@"using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace  Microsoft.eShopOnContainers.Services.[apiName].API.Mapper
{
    public class [Class]Mapper : Profile
    {
        public [Class]Mapper()
        {          
            //To Model            
            CreateMap<[Class], [Class]Model>();

            //To Domain
            CreateMap<[Class]Model, [Class]>();
        }
    }
}
",
                   "" },
                #endregion
            #region ViewModel	
                 {@$"{projectDirectory}\{apiName}.API\ViewModels\{aggregate}\{ClassName}ViewModel.cs",
                    @"
using System;
using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.[apiName].API.ViewModel
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
               {@$"{projectDirectory}\{apiName}.Domain\AggregatesModel\{aggregate}Aggregate\{ClassName}.cs",
@"using System;
using System.Collections.Generic;

namespace Microsoft.eShopOnContainers.Services.[apiName].Domain.AggregatesModel.[aggregate]Aggregate
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
                #endregion  
            #region Validator                                   
                    {@$"{projectDirectory}\{apiName}.Domain\AggregatesModel\{aggregate}Aggregate\{ClassName}Validator.cs",
@"
using FluentValidation;

namespace Microsoft.eShopOnContainers.Services.[apiName].Domain.AggregatesModel.[aggregate]Aggregate
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
            #region Domain.Interfaces  		       
                {@$"{projectDirectory}\{apiName}.Domain\AggregatesModel\{aggregate}Aggregate\Iterfaces\I{ClassName}Repository.cs",
@"using System;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.Services.[apiName].Domain.AggregatesModel.[aggregate]Aggregate

namespace Microsoft.eShopOnContainers.Services.[apiName].Domain.AggregatesModel.[aggregate]Aggregate.Interfaces
{
    public interface I[Class]Repository : IRepository<[Class]>
    {
    }
}", ""
               },
                #endregion
            #region Repository               
                {@$"{projectDirectory}\{apiName}.Infrastructure\Repositories\{aggregate}Aggregate\{ClassName}Repository.cs",
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.eShopOnContainers.Services.[apiName].Infrastructure.Repositories
{
    public class [Class]Repository : Repository<[Class]>, I[Class]Repository
    {
        public [Class]Repository([apiName]Context context)
        {
        }
    }
}

",
                "",

                },
	        #endregion
            #region EntityConfigurations
                     {@$"{projectDirectory}\{apiName}.Infrastructure\EntityConfigurations\{aggregate}Aggregate\{ClassName}EntityTypeConfiguration.cs",
@"
using Microsoft.eShopOnContainers.Services.[apiName].Domain.AggregatesModel.[aggregate]Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.eShopOnContainers.Services.[apiName].Infrastructure.EntityConfigurations
{
    public class [Class]EntityTypeConfiguration : IEntityTypeConfiguration<[Class]>
    {
        public void Configure(EntityTypeBuilder<[Class]> builder)
        {   
            builder.ToTable([Quote][Class][Quote]);
        }
    }
}",
                    ""
                }
            #endregion
            };

            for (int i = 0; i < array.Length / 3; i++)
            {
                string path = array[i, 0].Replace("[Class]", ClassName).Replace("[Quote]", quote).Replace("[apiName]", apiName).Replace("[aggregate]", aggregate);
                string text = array[i, 1].Replace("[Class]", ClassName).Replace("[Quote]", quote).Replace("[apiName]", apiName).Replace("[aggregate]", aggregate); ;

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
    }
}
