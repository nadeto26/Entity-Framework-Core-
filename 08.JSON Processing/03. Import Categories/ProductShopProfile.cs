using AutoMapper;
using ProductShop.DTOs;
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
        }
    }
}
