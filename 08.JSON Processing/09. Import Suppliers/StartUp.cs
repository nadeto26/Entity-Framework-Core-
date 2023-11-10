using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputJson = 
              File.ReadAllText(@"../../../Datasets/suppliers.json");

            string result = ImportSuppliers(context,inputJson);
            Console.WriteLine(result);
        }

        //Problem09

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSupplieDto[] suppliersDto
                = JsonConvert.DeserializeObject<ImportSupplieDto[]>(inputJson);

            ICollection<Supplier > validsuppliers = new List<Supplier>();
            foreach (ImportSupplieDto supplieDto in suppliersDto)
            {
                Supplier supplier = mapper.Map<Supplier>(supplieDto);

                validsuppliers.Add(supplier);

                // Here we have all valid users ready for the DB
                context.Suppliers.AddRange(validsuppliers);
                //context.SaveChanges();

             
            }
            return $"Successfully imported {validsuppliers.Count}.";

        }
        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}