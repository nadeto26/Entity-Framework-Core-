using AutoMapper;
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
          string inputJson = 
             File.ReadAllText(@"../../../Datasets/products.json");

            string result = ImportProducts(context,inputJson);
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