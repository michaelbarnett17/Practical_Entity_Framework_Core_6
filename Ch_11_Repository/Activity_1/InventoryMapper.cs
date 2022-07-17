using AutoMapper;
using InventoryModels;
using InventoryModels.DTOs;

namespace Activity_1
{
    public class InventoryMapper : Profile
    {
        public InventoryMapper()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<Category, CategoryDTO>()
                .ForMember(x => x.Category, opt => opt.MapFrom(y => y.Name))
                .ReverseMap()
                .ForMember(y => y.Name, opt => opt.MapFrom(x => x.Category));
            CreateMap<CategoryDetail, CategoryDetailDTO>()
                .ForMember(x => x.Color, opt => opt.MapFrom(y => y.ColorName))
                .ForMember(x => x.Value, opt => opt.MapFrom(y => y.ColorValue))
                .ReverseMap()
                .ForMember(y => y.ColorValue, opt => opt.MapFrom(x => x.Value))
                .ForMember(y => y.ColorName, opt => opt.MapFrom(x => x.Color));
        }
    }

}
