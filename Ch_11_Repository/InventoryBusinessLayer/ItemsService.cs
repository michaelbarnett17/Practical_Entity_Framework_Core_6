using AutoMapper;
using EFCore_DBLibrary;
using InventoryDatabaseLayer;
using InventoryModels.DTOs;
using System;
using System.Collections.Generic;

namespace InventoryBusinessLayer;

public class ItemsService : IItemsService
{
    private readonly IItemsRepo _dbRepo;

    public ItemsService(InventoryDbContext dbContext, IMapper mapper)
    {
        _dbRepo = new ItemsRepo(dbContext, mapper);
    }

    public List<ItemDTO> GetItems()
    {
        return _dbRepo.GetItems();
    }

    public List<ItemDTO> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
    {
        return _dbRepo.GetItemsByDateRange(minDateValue, maxDateValue);
    }

    public List<GetItemsForListingDTO> GetItemsForListingFromProcedure()
    {
        return _dbRepo.GetItemsForListingFromProcedure();
    }

    public List<GetItemsTotalValueDTO> GetItemsTotalValues(bool isActive)
    {
        return _dbRepo.GetItemsTotalValues(isActive);
    }

    public string GetAllItemsPipeDelimitedString()
    {
        var items = GetItems();
        return string.Join('|', items);
    }

    public List<FullItemDetailDTO> GetItemsWithGenresAndCategories()
    {
        return _dbRepo.GetItemsWithGenresAndCategories();
    }
}
