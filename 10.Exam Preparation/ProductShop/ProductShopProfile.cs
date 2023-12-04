using AutoMapper;
using ProductShop.DTOs;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            // Za da se sluchi mappinga 
           
            // User

            this.CreateMap<ImportUserDTO, User>();

            //Product 
            this.CreateMap<ImportantProductDto, Product>();

            //Category

            this.CreateMap<ImportCategotyDto, Category>();

            //ProductCategory

            this.CreateMap<ImportCategoryProductDto,CategoryProduct>();
            this.CreateMap<Product, ExportProductsInRangeDto>()
              .ForMember(d => d.ProductName,
                  opt => opt.MapFrom(s => s.Name))
              .ForMember(d => d.ProductPrice,
                  opt => opt.MapFrom(s => s.Price))
              .ForMember(d => d.SellerName,
                  opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

        }
    }
}
