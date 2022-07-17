using AutoMapper;
using EFCore_DBLibrary;
using InventoryDatabaseLayer;
using InventoryModels.DTOs;
using System.Collections.Generic;

namespace InventoryBusinessLayer;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepo _dbRepo;

    public CategoriesService(InventoryDbContext dbContext, IMapper mapper)
    {
        _dbRepo = new CategoriesRepo(dbContext, mapper);
    }

    public List<CategoryDTO> ListCategoriesAndDetails()
    {
        return _dbRepo.ListCategoriesAndDetails();
    }

}
