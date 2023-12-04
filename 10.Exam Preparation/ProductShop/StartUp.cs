using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
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
            //File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = GetUsersWithProducts(context);
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

            // In this collection of DTOs, there can be invalid entries
            ImportCategotyDto[] categoryDtos =
                JsonConvert.DeserializeObject<ImportCategotyDto[]>(inputJson);

            ICollection<Category> validCategories = new HashSet<Category>();
            foreach (ImportCategotyDto categoryDto in categoryDtos)
            {
                if (String.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

            
  

        
        //problem05
        public static string GetProductsInRange(ProductShopContext context)
        {
            // Anonymous object + Manual Mapping
            // DTO + Manual Mapping
            // DTO + AutoMapper


            //# Anonymous object + Manual Mapping
            //var products = context.Products
            //    .Where(p => p.Price >= 500 && p.Price <= 1000)
            //    .OrderBy(p => p.Price)
            //    .Select(p => new
            //    {
            //        name = p.Name,
            //        price = p.Price,
            //        seller = p.Seller.FirstName + " " + p.Seller.LastName
            //    })
            //    .AsNoTracking()
            //    .ToArray();

            //return JsonConvert.SerializeObject(products, Formatting.Indented);

            IMapper mapper = CreateMapper();
            ExportProductsInRangeDto[] productDtos = 
                context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .AsNoTracking()
                .ProjectTo<ExportProductsInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(productDtos, Formatting.Indented);
        }
        //Problem04
       
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

        //Problem06

        public static string GetSoldProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var usersWithSoldProducts = context.Users
             .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
             .OrderBy(u => u.LastName)
             .ThenBy(u => u.FirstName)
             .Select(u => new
             {
                 u.FirstName,
                 u.LastName,
                 SoldProducts = u.ProductsSold
                     .Where(p => p.Buyer != null)
                     .Select(p => new
                     {
                         p.Name,
                         p.Price,
                         BuyerFirstName = p.Buyer.FirstName,
                         BuyerLastName = p.Buyer.LastName
                     })
                     .ToArray()
             })
             .AsNoTracking()
             .ToArray();

            return JsonConvert.SerializeObject(usersWithSoldProducts,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver
                });

        }
        private static IContractResolver ConfigureCamelCaseNaming()
        {
            return new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };
        }

        //Problem07
         public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            // This should be corrected in Judge
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = Math.Round((double)c.CategoriesProducts.Average(p => p.Product.Price), 2),
                    TotalRevenue = Math.Round((double)c.CategoriesProducts.Sum(p => p.Product.Price), 2)
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(categories,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver
                });
        }

        //Problem08

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //4 DTO edno v drugo

            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    // UserDTO
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        // ProductWrapperDTO
                        Count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                // ProductDTO
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .AsNoTracking()
                .ToArray();

            var userWrapperDto = new
            {
                UsersCount = users.Length,
                Users = users
            };

            return JsonConvert.SerializeObject(userWrapperDto,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                });
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