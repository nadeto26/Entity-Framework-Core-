using System.Reflection;

namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Positions;
    using FastFood.Model;
   

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(d => d.Name,
                    opt => opt.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(d => d.Name,
                    opt => opt.MapFrom(s => s.Name));

            // Categories
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(d => d.Name,
                    opt => opt.MapFrom(s => s.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>();

            // Items
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryId,
                    opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CategoryName,
                    opt => opt.MapFrom(s => s.Name));

            this.CreateMap<CreateItemInputModel, Item>();
            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(d => d.Category,
                    opt => opt.MapFrom(s => s.Category.Name));
        }
    }
}
