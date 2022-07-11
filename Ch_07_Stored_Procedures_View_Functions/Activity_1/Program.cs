using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EFCore_DBLibrary;
using InventoryModels;
using Microsoft.Data.SqlClient;

namespace InventoryHelpers;

public class Program
{
    private static IConfigurationRoot _configuration;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
    private const string _loggedInUserId = "e2eb8989-a81a-4151-8e86-eb95a7961da2";


    static void Main(string[] args)
    {
        BuildOptions();
        ListInventory();
        GetItemsForListing();
        GetItemsTotalValues();
    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));
    }

    private static void EnsureItem(string name, string description, string notes)
    {
        Random r = new Random();
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            //determine if item exists:
            var existingItem = db.Items.FirstOrDefault(x => x.Name.ToLower()
                                                        == name.ToLower());

            if (existingItem == null)
            {
                //doesn't exist, add it.
                var item = new Item()
                {
                    Name = name,
                    CreatedByUserId = _loggedInUserId,
                    IsActive = true,
                    Quantity = r.Next(1, 1000),
                    Description = description,
                    Notes = notes
                };

                db.Items.Add(item);
                db.SaveChanges();
            }
        }
    }

    private static void ListInventory()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items.OrderBy(x => x.Name).ToList();
            items.ForEach(x => Console.WriteLine($"New Item: {x.Name}"));
        }
    }

    private static void GetAllActiveItemsAsPipeDelimitedString()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var isActiveParm = new SqlParameter("IsActive", 1);

            var result = db.AllItemsOutput
                            .FromSqlRaw("SELECT [dbo].[ItemNamesPipeDelimitedString] (@IsActive) AllItems", isActiveParm)
                            .FirstOrDefault();

            Console.WriteLine($"All active Items: {result.AllItems}");
        }
    }

    private static void GetItemsTotalValues()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var isActiveParm = new SqlParameter("IsActive", 1);

            var result = db.GetItemsTotalValues
                            .FromSqlRaw("SELECT * from [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParm)
                            .ToList();

            foreach (var item in result)
            {
                Console.WriteLine($"New Item] {item.Id,-10}" +
                                    $"|{item.Name,-50}" +
                                    $"|{item.Quantity,-4}" +
                                    $"|{item.TotalValue,-5}");
            }
        }
    }

    private static void GetItemsForListing()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var results = db.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing").ToList();
            foreach (var item in results)
            {
                var output = $"ITEM {item.Name}] {item.Description}";
                if (!string.IsNullOrWhiteSpace(item.CategoryName))
                {
                    output = $"{output} has category: {item.CategoryName}";
                }
                Console.WriteLine(output);
            }
        }
    }

}
