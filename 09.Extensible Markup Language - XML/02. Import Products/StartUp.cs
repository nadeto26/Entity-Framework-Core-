using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            string inputXml = 
            File.ReadAllText("../../../Datasets/products.xml");

            string result = ImportProducts(context,inputXml);
            Console.WriteLine(result);

        }

        //Problem01

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportUserDto[] userDtos =
                xmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

            ICollection<User> validUsers = new HashSet<User>();

            foreach(ImportUserDto userDto in userDtos)
            {
                if (string.IsNullOrEmpty(userDto.FirstName) || string.IsNullOrEmpty(userDto.LastName))
                {
                    continue;
                }

                User user = mapper.Map<User>(userDto);
                validUsers.Add(user);
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();
            return $"Successfully imported {validUsers.Count}";

        }

        //Problem02

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeAutoMapper();

            ImportProductDto[] productDtos
                = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach(ImportProductDto productDto in productDtos)
            {
                //if(string.IsNullOrEmpty(productDto.Name))
                //{
                //    continue;
                //}
                //if (!productDto.BuyerId.HasValue ||
                //    !context.Products.Any(pr => pr.BuyerId == productDto.BuyerId))
                //{
                //    continue;
                //}

                Product product = mapper.Map<Product>(productDto);
                validProducts.Add(product);
            }
            context.Products.AddRange(validProducts);
            context.SaveChanges();
            return $"Successfully imported {validProducts.Count}";
        }

        //Problem03

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeAutoMapper();

            ImportCategory[] importCategories =
                xmlHelper.Deserialize<ImportCategory[]>(inputXml, "Categories");

            ICollection<Category> validCategories = new HashSet<Category>();

            foreach(ImportCategory category in importCategories)
            {
                if(string.IsNullOrEmpty(category.Name))
                {
                    continue;
                }

                Category category1 = mapper.Map<Category>(category);
                validCategories .Add(category1);
            }
            context.Categories.AddRange(validCategories);
            context.SaveChanges();
            return $"Successfully imported {validCategories.Count}";
        }

        //Problem04

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeAutoMapper();

            ImportCategoriesAndProductsDto[] importCategoriesAndProductsDtos
                = xmlHelper.Deserialize<ImportCategoriesAndProductsDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> valid = new HashSet<CategoryProduct>();

            foreach(ImportCategoriesAndProductsDto category in importCategoriesAndProductsDtos)
            {
                if (!context.CategoryProducts.Any(c => c.ProductId == category.ProductId))
                {
                    continue;
                }
                else if (!context.CategoryProducts.Any(c => c.CategoryId == category.CategoryId))
                {
                    continue;
                }

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(category);
                valid.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(valid);
            context.SaveChanges();
            return $"Successfully imported {valid.Count}";
        }

        private static IMapper InitializeAutoMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}