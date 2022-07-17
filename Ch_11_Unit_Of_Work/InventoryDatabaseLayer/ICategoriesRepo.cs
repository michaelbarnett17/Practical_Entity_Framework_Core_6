using InventoryModels.DTOs;

namespace InventoryDatabaseLayer;

public interface ICategoriesRepo
{
    List<CategoryDTO> ListCategoriesAndDetails();
}
