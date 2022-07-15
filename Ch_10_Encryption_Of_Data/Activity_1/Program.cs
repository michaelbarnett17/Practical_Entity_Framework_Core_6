using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EFCore_DBLibrary;
using InventoryModels;
using InventoryHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using InventoryModels.DTOs;
using AutoMapper.QueryableExtensions;

namespace Activity_1;

public class Program
{
    private static IConfigurationRoot _configuration;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
    private const string _loggedInUserId = "e2eb8989-a81a-4151-8e86-eb95a7961da2";
    private static MapperConfiguration _mapperConfig;
    private static IMapper _mapper;
    private static IServiceProvider _serviceProvider;

    static void Main(string[] args)
    {
        BuildOptions();
        BuildMapper();
        //ListInventory();
        ListInventoryWithProjection();
        GetItemsForListing();
        GetItemsForListingLinq();
        //GetAllActiveItemsAsPipeDelimitedString();
        //GetItemsTotalValues();
        //GetFullItemDetails();
        ListCategoriesAndColors();

    }

    static void BuildOptions()
    {
        _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("InventoryManager"));


    }



    private static void BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(typeof(InventoryMapper));
        _serviceProvider = services.BuildServiceProvider();

        _mapperConfig = new MapperConfiguration(cfg => {
            cfg.AddProfile<InventoryMapper>();
        });
        _mapperConfig.AssertConfigurationIsValid();
        _mapper = _mapperConfig.CreateMapper();
    }

    private static void ListInventory()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items.OrderBy(x => x.Name).ToList();
            var result = _mapper.Map<List<Item>, List<ItemDTO>>(items);
            result.ForEach(x => Console.WriteLine($"New Item: {x}"));
        }
    }

    private static void ListInventoryWithProjection()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var items = db.Items
                            .OrderBy(x => x.Name)
                            .ProjectTo<ItemDTO>(_mapper.ConfigurationProvider)
                            .ToList();
            items.ForEach(x => Console.WriteLine($"New Item: {x}"));
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

    private static void GetItemsForListingLinq()
    {
        var minDateValue = new DateTime(2021, 1, 1);
        var maxDateValue = new DateTime(2024, 1, 1);

        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var results = db.Items.Select(x => new ItemDTO
            {
                CreatedDate = x.CreatedDate,
                CategoryName = x.Category.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                Name = x.Name,
                Notes = x.Notes,
                CategoryId = x.Category.Id,
                Id = x.Id
            }).Where(x => x.CreatedDate >= minDateValue && x.CreatedDate <= maxDateValue)
            .OrderBy(y => y.CategoryName).ThenBy(z => z.Name)
            .ToList();

            foreach (var itemDTO in results)
            {
                Console.WriteLine(itemDTO);
            }
        }
    }

    private static void GetFullItemDetails()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {


            var result = db.FullItemDetailDTOs
                            .FromSqlRaw("SELECT * FROM [dbo].[vwFullItemDetails] " +
                                        "ORDER BY ItemName, GenreName, Category, PlayerName ")
                            .ToList();

            foreach (var item in result)
            {
                Console.WriteLine($"New Item] {item.Id,-10}" +
                                    $"|{item.ItemName,-50}" +
                                    $"|{item.ItemDescription,-4}" +
                                    $"|{item.PlayerName,-5}" +
                                    $"|{item.Category,-5}" +
                                    $"|{item.GenreName,-5}");
            }
        }
    }

    private static void ListCategoriesAndColors()
    {
        using (var db = new InventoryDbContext(_optionsBuilder.Options))
        {
            var results = db.Categories
                            .Include(x => x.CategoryDetail)
                            //.Select(x => x.CategoryDetail)
                            .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider).ToList();

            foreach (var c in results)
            {
                Console.WriteLine($"Category [{c.Category}] is {c.CategoryDetail.Color}");
            }
        }

    }

}
