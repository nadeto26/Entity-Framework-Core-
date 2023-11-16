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

            this.CreateMap<ImportProductDto, Product>();

            //Categort

            this.CreateMap<ImportCategory,Category>();
        }
    }
}
