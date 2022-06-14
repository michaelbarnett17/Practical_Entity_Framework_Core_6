using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using EFCore_DBLibrary;

namespace Activity_1;

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
 
For initial scaffold (Database to Code)
1) Comment everything above
2) Uncomment everything below
3) Delete everything in class library
4) In Package Manager Console, run the command on next line (BE SURE TO TARGET THE CLASS LIBRARY):
Scaffold-DbContext 'Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=AdventureWorks2019;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer

For Initial Migration (Code to Database)
1) In Package Manager Console, run the command on next line (BE SURE TO TARGET THE CLASS LIBRARY):
add-migration Initial-Migration

*/

//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;

//namespace EFCore_Activity0201;

//class Program
//{
//    private static IConfigurationRoot _configuration;

//    static void Main(string[] args)
//    {
//        BuildConfiguration();
//        Console.WriteLine("Configuration String: " + _configuration.GetConnectionString("AdventureWorks2019"));
//    }

//    static void BuildConfiguration()
//    {
//        var builder = new ConfigurationBuilder()
//                        .SetBasePath(Directory.GetCurrentDirectory())
//                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

//        _configuration = builder.Build();
//    }
//}