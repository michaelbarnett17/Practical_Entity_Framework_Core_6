using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using EFCore_DBLibrary;

namespace EFCore_Activity0201;

class Program
{
    private static IConfigurationRoot _configuration;
    private static DbContextOptionsBuilder<AdventureWorks2019Context> _optionsBuilder;

    static void Main(string[] args)
    {
        BuildConfiguration();
        BuilderOptions();
        ListPeople();
        Console.WriteLine("Configuration String: " + _configuration.GetConnectionString("AdventureWorks2019"));
    }

    static void BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }

    static void BuilderOptions()
    {
        _optionsBuilder = new DbContextOptionsBuilder<AdventureWorks2019Context>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AdventureWorks2019"));
    }

    static void ListPeople()
    {
        using (var db = new AdventureWorks2019Context(_optionsBuilder.Options))
        {
            var people = db.People.OrderByDescending(x => x.LastName).Take(20).ToList();

            foreach (var person in people)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
    }
}

/*
 
For initial scaffold 
1) Delete everything in class library
2) Load Scaffolder Project
3) Unload this Project
4) In Package Manager Console, run the command on next line (BE SURE TO TARGET THE CLASS LIBRARY):
Scaffold-DbContext 'Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=AdventureWorks2019;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer

*/



