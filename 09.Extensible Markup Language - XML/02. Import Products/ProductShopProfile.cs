using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Runtime.InteropServices;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //User
            this.CreateMap<ImportUserDto, User>();

            //Product

            this.CreateMap<ImportProductDto, Product>()
                .ForMember(d => d.BuyerId,
                    opt => opt.MapFrom(s => s.BuyerId.Value));

            //Categort

            this.CreateMap<ImportCategory,Category>();

            //CategoriesAndProduct

            this.CreateMap<ImportCategoriesAndProductsDto,CategoryProduct>();
        }
    }
}
