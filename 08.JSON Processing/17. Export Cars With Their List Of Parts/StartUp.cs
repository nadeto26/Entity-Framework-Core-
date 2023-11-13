using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Castle.DynamicProxy.Generators;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            //string inputJson = 
            //  File.ReadAllText(@"../../../Datasets/sales.json");

            string result = GetCarsWithTheirListOfParts(context);
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

        //problem13

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSaleDto[] salesDto
                = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            Sale[] sales
                = mapper.Map<Sale[]>(salesDto);
            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }


        //Problem14 -> EXPORT

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver == true)
                .Select(x => new 
                {
                    x.Name ,               
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    x.IsYoungDriver
                })
                .AsNoTracking()
                .ToArray();
            var jsonFile = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return jsonFile;
        }

        //probleblem15

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x=>x.Make == "Toyota")
                .OrderBy(x=>x.Model)
                .ThenByDescending(x=>x.TravelledDistance)
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                })
                .AsNoTracking()
            .ToArray();

            var jsonFile = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonFile;
        }

        //Problem16

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var supplier = context.Suppliers
                .Where(x => x.IsImporter==true)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      PartsCount= x.Parts.Count


                  })
                .AsNoTracking()
            .ToArray();

            var jsonFile = JsonConvert.SerializeObject(supplier, Formatting.Indented);

            return jsonFile;

        }

        //Problem17

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartsCars.Select(pc => new
                    {
                        Name = pc.Part.Name,
                        Price = $"{pc.Part.Price:F2}"
                    }).ToArray(),
                }).ToArray();

            var jsonFile = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonFile;
        }


    }
}