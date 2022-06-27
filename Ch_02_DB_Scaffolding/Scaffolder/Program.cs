using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Activity0201;

class Program
{
    private static IConfigurationRoot _configuration;

    static void Main(string[] args)
    {
        BuildConfiguration();
        Console.WriteLine("Configuration String: " + _configuration.GetConnectionString("AdventureWorks2019"));
    }

    static void BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }
}