using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCore_DBLibrary.DTOs
{
    public class SalesReportListingDTO
    {
        [Required]
        public int BusinessEntityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? SalesYtd { get; set; }
        public IEnumerable<string> Territories { get; set; }
        public int TotalProductsSold { get; set; }
        public int TotalOrders { get; set; }
        public string DisplayName => $"{LastName}, {FirstName}";
        public string DisplayTerritories => string.Join(",", Territories);
        public override string ToString()
        {
            return $"BID: {BusinessEntityId} |{DisplayName,25}|{DisplayTerritories,25}|" +
                $"{SalesYtd} | Orders: {TotalOrders} |" +
                $"Products Sold: {TotalProductsSold}";
        }
    }

}