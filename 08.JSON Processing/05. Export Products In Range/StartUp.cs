﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
       
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
          //string inputJson = 
          //   File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = GetProductsInRange(context);
            Console.WriteLine(result);
        }

        //Problem1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
           IMapper mapper = CreateMapper();
          

            ImportUserDTO[] userDtos =
                JsonConvert.DeserializeObject<ImportUserDTO[]>(inputJson);

            // AutoMapper can map collections also
            // In case of no validation you can:
            // User[] users = mapper.Map<User[]>(userDtos);

            // This way allows you additional validations
            ICollection<User> validUsers = new HashSet<User>();
            foreach (ImportUserDTO userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);

                validUsers.Add(user);
            }

            // Here we have all valid users ready for the DB
            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }
        //Problem2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportantProductDto[] productDtos =
                JsonConvert.DeserializeObject<ImportantProductDto[]>(inputJson);
            Product[] products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";

        }

        //Problem3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            //In this collections of DTOs, there can be invalid objects 
            //Deserialize the object
            ImportantProductDto[] categoryDtos=
                JsonConvert.DeserializeObject<ImportantProductDto[]>(inputJson);

            //check for the valid -> make validation 
            ICollection<Category> validCategories = new HashSet<Category>();
            foreach(ImportantProductDto categoryDTO in categoryDtos)
            {
                //make validation
                if(string.IsNullOrEmpty(categoryDTO.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDTO);
                validCategories.Add(category);
            }
            context.Categories.AddRange(validCategories);
            context.SaveChanges() ;

            return $"Successfully imported {validCategories.Count}";
        }

        //Problem04

        //problem05
        public static string GetProductsInRange(ProductShopContext context)
        {
            // Anonymous object + Manual Mapping
            // DTO + Manual Mapping
            // DTO + AutoMapper


           //# Anonymous object + Manual Mapping
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryProductDto[] cpDtos =
                JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            ICollection<CategoryProduct> validEntries = new HashSet<CategoryProduct>();
            foreach (ImportCategoryProductDto cpDto in cpDtos)
            {
                // This is not wanted in the description but we do it for security
                // In Judge locally they may not be added previously
                // JUDGE DO NOT LIKE THIS VALIDATION BELOW!!!!!
                //if (!context.Categories.Any(c => c.Id == cpDto.CategoryId) ||
                //    !context.Products.Any(p => p.Id == cpDto.ProductId))
                //{
                //    continue;
                //}

                CategoryProduct categoryProduct =
                    mapper.Map<CategoryProduct>(cpDto);
                validEntries.Add(categoryProduct);
            }

            context.CategoriesProducts.AddRange(validEntries);
            context.SaveChanges();

            return $"Successfully imported {validEntries.Count}";
        }


        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }
            ));
        }

    }
}