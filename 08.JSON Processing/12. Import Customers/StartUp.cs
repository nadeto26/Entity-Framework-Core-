using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.DynamicProxy.Generators;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputJson = 
              File.ReadAllText(@"../../../Datasets/customers.json");

            string result = ImportCustomers(context,inputJson);
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
                

             
            }
            context.Suppliers.AddRange(validsuppliers);
            context.SaveChanges();
            return $"Successfully imported {validsuppliers.Count}.";

        }
        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }

        //problem10

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportPartDto[] partDto
                = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            //samo tezi , koito otgovarqt 

            HashSet<Part> validParts = new HashSet<Part>();

            foreach(ImportPartDto part in partDto)
            {
                if (context.Suppliers.Any(c=>c.Id == part.SupplierId))
                {
                    Part task = mapper.Map<Part>(part);
                    validParts.Add(task);
                }

               
                
            }
            context.Parts.AddRange(validParts);
            context.SaveChanges();
            return $"Successfully imported {validParts.Count}.";
        }

        //problem11

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCarsDto[] carsDto
                = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);

             Car[] cars = mapper.Map<Car[]>(carsDto);
            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Length}.";
        }

        //Problem12 

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCustomersJson[] customersDto
                = JsonConvert.DeserializeObject<ImportCustomersJson[]>(inputJson);

            Customer[] customers
                = mapper.Map<Customer[]>(customersDto);
            context.AddRange(customers);
            context.SaveChanges();
           return  $"Successfully imported {customers.Length}.";
        }



    }
}