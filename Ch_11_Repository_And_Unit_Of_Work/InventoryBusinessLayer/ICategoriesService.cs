using InventoryModels.DTOs;
using System.Collections.Generic;

namespace InventoryBusinessLayer;

public interface ICategoriesService
{
    List<CategoryDTO> ListCategoriesAndDetails();
}
