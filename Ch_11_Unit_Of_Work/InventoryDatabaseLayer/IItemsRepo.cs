using InventoryModels;
using InventoryModels.DTOs;

namespace InventoryDatabaseLayer;

public interface IItemsRepo
{
    List<Item> GetItems();
    List<ItemDTO> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
    List<GetItemsForListingDTO> GetItemsForListingFromProcedure();
    List<GetItemsTotalValueDTO> GetItemsTotalValues(bool isActive);
    List<FullItemDetailDTO> GetItemsWithGenresAndCategories();

    int UpsertItem(Item item);
    void UpsertItems(List<Item> items);
    void DeleteItem(int id);
    void DeleteItems(List<int> itemIds);
}