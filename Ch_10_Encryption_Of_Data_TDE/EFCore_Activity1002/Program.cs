using System;
using EFCore_DBLibrary;
using InventoryHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using EFCore_DBLibrary.DTOs;

namespace EFCore_Activity1002
{
    class Program
    {
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<AdventureWorksContext> _optionsBuilder;

        static void Main(string[] args)
        {
            BuildOptions();
            //Console.WriteLine("List People Then Order and Take");
            //ListPeopleThenOrderAndTake();
            //Console.WriteLine("Query People, order, then list and take");
            //QueryPeopleOrderedToListAndTake();
            //Console.WriteLine("Please Enter the partial First or Last Name, or the Person Type to search for:");
            //var result = Console.ReadLine();
            //FilteredPeople(result);

            //int pageSize = 10;
            //for (int pageNumber = 0; pageNumber < 10; pageNumber++)
            //{
            //    Console.WriteLine($"Page {pageNumber + 1}");
            //    FilteredAndPagedResult(result, pageNumber, pageSize);
            //}

            ListAllSalespeople();
            ShowAllSalespeopleUsingProjection();
            //GenerateSalesReportData();
            GenerateSalesReportDataToDTO();
        }

        static void BuildOptions()
        {
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<AdventureWorksContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AdventureWorks2019"));
        }

        private static void ListPeopleThenOrderAndTake()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var people = db.People.AsNoTracking().ToList().OrderByDescending(x => x.LastName);
                foreach (var person in people.Take(10))
                {
                    Console.WriteLine($"{person.FirstName} {person.LastName}");
                }
            }
        }

        private static void QueryPeopleOrderedToListAndTake()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var query = db.People.AsNoTracking().OrderByDescending(x => x.LastName);
                var result = query.Take(10);

                foreach (var person in result)
                {
                    Console.WriteLine($"{person.FirstName} {person.LastName}");
                }
            }
        }

        private static void FilteredPeople(string filter)
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var searchTerm = filter.ToLower();
                var query = db.People.AsNoTracking().Where(x => x.LastName.ToLower().Contains(searchTerm)
                                                || x.FirstName.ToLower().Contains(searchTerm)
                                                || x.PersonType.ToLower().Equals(searchTerm));

                foreach (var person in query)
                {
                    Console.WriteLine($"{person.FirstName} {person.LastName}, {person.PersonType}");
                }
            }
        }

        private static void FilteredAndPagedResult(string filter, int pageNumber, int pageSize)
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var searchTerm = filter.ToLower();
                var query = db.People.AsNoTracking().Where(x => x.LastName.ToLower().Contains(searchTerm)
                                                || x.FirstName.ToLower().Contains(searchTerm)
                                                || x.PersonType.ToLower().Equals(searchTerm))
                                        .OrderBy(x => x.LastName)
                                        .Skip(pageNumber * pageSize)
                                        .Take(pageSize);

                foreach (var person in query)
                {
                    Console.WriteLine($"{person.FirstName} {person.LastName}, {person.PersonType}");
                }
            }
        }

        private static string GetSalespersonDetail(SalesPerson sp)
        {
            return $"ID: {sp.BusinessEntityId}\t|TID: {sp.TerritoryId}\t|Quota: {sp.SalesQuota}\t" +
                        $"|Bonus: {sp.Bonus}\t|YTDSales: {sp.SalesYtd}\t|Name: \t" +
                        $"{sp.BusinessEntity?.BusinessEntity?.FirstName ?? ""}, " +
                        $"{sp.BusinessEntity?.BusinessEntity?.LastName ?? ""}";
        }


        private static void ListAllSalespeople()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salespeople = db.SalesPeople
                    .Include(x => x.BusinessEntity)
                    .ThenInclude(y => y.BusinessEntity)
                    .AsNoTracking().ToList();

                foreach (var salesperson in salespeople)
                {
                    Console.WriteLine(GetSalespersonDetail(salesperson));
                }
            }
        }

        private static void ShowAllSalespeopleUsingProjection()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salespeople = db.SalesPeople
                                    .AsNoTracking()
                                    .Select(x => new
                                    {
                                        x.BusinessEntityId,
                                        x.BusinessEntity.BusinessEntity.FirstName,
                                        x.BusinessEntity.BusinessEntity.LastName,
                                        x.SalesQuota,
                                        x.SalesYtd,
                                        x.SalesLastYear
                                    }).ToList();


                foreach (var sp in salespeople)
                {
                    Console.WriteLine($"BID: {sp.BusinessEntityId} | Name: {sp.LastName}" +
                            $", {sp.FirstName} | Quota: {sp.SalesQuota} | " +
                            $"YTD Sales: {sp.SalesYtd} | SalesLastYear {sp.SalesLastYear}");
                }
            }
        }

        private static void GenerateSalesReportData()
        {
            Console.WriteLine("What is the minimum amount of sales?");
            var input = Console.ReadLine();
            decimal filter = 0.0m;

            if (!decimal.TryParse(input, out filter))
            {
                Console.WriteLine("Bad input");
                return;
            }

            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salesReportData = db.SalesPeople.Select(sp => new
                {
                    beid = sp.BusinessEntityId,
                    sp.BusinessEntity.BusinessEntity.FirstName,
                    sp.BusinessEntity.BusinessEntity.LastName,
                    sp.SalesYtd,
                    Territories = sp.SalesTerritoryHistories
                                    .Select(y => y.Territory.Name),
                    OrderCount = sp.SalesOrderHeaders.Count(),
                    TotalProductsSold = sp.SalesOrderHeaders
                        .SelectMany(y => y.SalesOrderDetails)
                        .Sum(z => z.OrderQty)
                }).Where(srdata => srdata.SalesYtd > filter)
                    .OrderBy(srds => srds.LastName)
                    .ThenBy(srds => srds.FirstName)
                    .ThenByDescending(srds => srds.SalesYtd)
                    .ToList();

                foreach (var srd in salesReportData)
                {
                    Console.WriteLine($"{srd.beid}| {srd.LastName}, {srd.FirstName} |" +
                                        $"YTD Sales: {srd.SalesYtd} |" +
                                        $"{string.Join(',', srd.Territories)} |" +
                                        $"Order Count: {srd.OrderCount} |" +
                                        $"Products Sold: {srd.TotalProductsSold}");
                }
            }
        }

        private static void GenerateSalesReportDataToDTO()
        {
            Console.WriteLine("What is the minimum amount of sales?");
            var input = Console.ReadLine();
            decimal filter = 0.0m;

            if (!decimal.TryParse(input, out filter))
            {
                Console.WriteLine("Bad input");
                return;
            }

            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salesReportData = db.SalesPeople.Select(sp => new SalesReportListingDto
                {
                    BusinessEntityId = sp.BusinessEntityId,
                    FirstName = sp.BusinessEntity.BusinessEntity.FirstName,
                    LastName = sp.BusinessEntity.BusinessEntity.LastName,
                    SalesYtd = sp.SalesYtd,
                    Territories = sp.SalesTerritoryHistories
                    .Select(y => y.Territory.Name),
                    TotalOrders = sp.SalesOrderHeaders.Count(),
                    TotalProductsSold = sp.SalesOrderHeaders
                            .SelectMany(y => y.SalesOrderDetails)
                            .Sum(z => z.OrderQty)
                }).Where(srdata => srdata.SalesYtd > filter)
                    .OrderBy(srds => srds.LastName)
                    .ThenBy(srds => srds.FirstName)
                    .ThenByDescending(srds => srds.SalesYtd)
                    .ToList();

                foreach (var srd in salesReportData)
                {
                    Console.WriteLine(srd.ToString());
                }
            }
        }


    }
}


