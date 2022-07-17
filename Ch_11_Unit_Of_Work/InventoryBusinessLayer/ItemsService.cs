﻿using AutoMapper;
using EFCore_DBLibrary;
using InventoryDatabaseLayer;
using InventoryModels;
using InventoryModels.DTOs;
using System;
using System.Collections.Generic;

namespace InventoryBusinessLayer
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepo _dbRepo;
        private readonly IMapper _mapper;

        public ItemsService(InventoryDbContext dbContext, IMapper mapper)
        {
            _dbRepo = new ItemsRepo(dbContext, mapper);
            _mapper = mapper;
        }

        public List<ItemDTO> GetItems()
        {
            return _mapper.Map<List<ItemDTO>>(_dbRepo.GetItems());
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

        public int UpsertItem(CreateOrUpdateItemDTO item)
        {
            if (item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category id before insert or update");
            }
            return _dbRepo.UpsertItem(_mapper.Map<Item>(item));
        }

        public void UpsertItems(List<CreateOrUpdateItemDTO> items)
        {
            try
            {
                _dbRepo.UpsertItems(_mapper.Map<List<Item>>(items));
            }
            catch (Exception ex)
            {
                //TODO: better logging/not squelching
                Console.WriteLine($"The transaction has failed: {ex.Message}");
            }
        }

        public void DeleteItem(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid item id before deleting");
            }
            _dbRepo.DeleteItem(id);
        }

        public void DeleteItems(List<int> itemIds)
        {
            try
            {
                _dbRepo.DeleteItems(itemIds);
            }
            catch (Exception ex)
            {
                //TODO: better logging/not squelching
                Console.WriteLine($"The transaction has failed: {ex.Message}");
            }
        }
    }
}
