using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Castle.Core.Resource;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
             CarDealerContext context = new CarDealerContext();
             //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
              
            string input = GetCarsWithTheirListOfParts(context);
            Console.WriteLine(input);

        }
        //Problem09

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper =  InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            //от suppliers.xml видяхме, че е []
            ImportSupplierDto[] supplierDtos
                = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            ICollection<Supplier> validSuplliers = new HashSet<Supplier>();

            foreach(ImportSupplierDto supplierDto in supplierDtos)
            {
                if (string.IsNullOrEmpty(supplierDto.Name))
                {
                    continue;
                }
                // Manual mapping without AutoMapper - ако искаме на ръка да ги мапнем
                //Supplier supplier = new Supplier()
                //{
                //    Name = supplierDto.Name,
                //    IsImporter = supplierDto.IsImporter
                //};
               // validSuplliers.Add(supplier);

                Supplier supplier = mapper.Map<Supplier>(supplierDto);
                validSuplliers.Add(supplier);
            }
            context.AddRange(validSuplliers);
            context.SaveChanges();
            return $"Successfully imported {validSuplliers.Count}";
        }

        //Problem10

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportPartDto[] partDtos =
                xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

            ICollection<Part> validParts = new HashSet<Part>();
            foreach (ImportPartDto partDto in partDtos)
            {
                if (string.IsNullOrEmpty(partDto.Name))
                {
                    continue;
                }

                if (!partDto.SupplierId.HasValue ||
                    !context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    // Missing or wrong supplier id
                    continue;
                }

                Part part = mapper.Map<Part>(partDto);
                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";

        }

        //Problem11

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportCarDto[] carDtos
                = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

            ICollection<Car> validCars = new HashSet<Car>();
            foreach (ImportCarDto cartDto in carDtos)
            {
                if(string.IsNullOrEmpty(cartDto.Make) ||
                    string.IsNullOrEmpty(cartDto.Model))
                {
                    continue;
                }
                Car car = mapper.Map<Car>(cartDto);
               
                foreach(var partDto  in cartDto.Parts.DistinctBy(p=>p.PartId)) //only unique
                {
                    if( 
                        !context.Parts.Any(p=>p.Id == partDto.PartId))
                    {
                        continue;
                    }

                    //Ръчен mapper

                    PartCar carPart = new PartCar()
                    {
                        PartId = partDto.PartId,
                    };
                   car.PartsCars.Add(carPart);
                }
                validCars.Add(car);
            }
            context.AddRange(validCars);
            context.SaveChanges();
            return $"Successfully imported {validCars.Count}";
        }

        //Problem12

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportCustomersDto[] customerDtos =
                xmlHelper.Deserialize<ImportCustomersDto[]>(inputXml, "Customers");

            ICollection<Customer> validCustomers = new HashSet<Customer>();
            foreach (ImportCustomersDto customerDto in customerDtos)
            {
                if (string.IsNullOrEmpty(customerDto.Name) ||
                    string.IsNullOrEmpty(customerDto.BirthDate))
                {
                    continue;
                }

                Customer customer = mapper.Map<Customer>(customerDto);
                validCustomers.Add(customer);
            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}";
        }

        //Problem13

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportSaleDto[] importSaleDtos =
                xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            ICollection<int> dbCarIds = context.Cars
            .Select(c => c.Id)
            .ToArray();
            ICollection<Sale> validSales = new HashSet<Sale>();

            foreach(ImportSaleDto saleDto in importSaleDtos)
            {
                if (!saleDto.CarId.HasValue ||
                     dbCarIds.All(id => id != saleDto.CarId.Value))
                {
                    continue;
                }

                Sale sale = mapper.Map<Sale>(saleDto);
                validSales.Add(sale);
            }
            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}";
        }

        //Problem14

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = InitializeAutoMapper();

            XmlHelper xmlHelper = new XmlHelper();
            ExportCarDto[] cars = context.Cars
                .Where(c=>c.TraveledDistance>2000000)
                .OrderBy(c=>c.Make)
                .ThenBy(c=>c.Model)
                .Take(10)
                .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider) //мапва ги към exportCardto
                .ToArray();


            return xmlHelper.Serialize<ExportCarDto[]>(cars, "cars");
        }

        //Problem15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ExportBmwCarDto[] bmwCar = context.Cars
                .Where(c => c.Make.ToUpper() == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportBmwCarDto>(mapper.ConfigurationProvider) //правим ги на дто
                .ToArray();

            return xmlHelper.Serialize<ExportBmwCarDto[]>(bmwCar, "cars");
        }

        //Problem17

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ExportCarWithPartsDto[] carsWithParts = context
          .Cars
          .OrderByDescending(c => c.TraveledDistance)
          .ThenBy(c => c.Model)
          .Take(5)
          .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
          .ToArray();

            return xmlHelper.Serialize(carsWithParts, "cars");
        }



        private static IMapper InitializeAutoMapper()
          => new Mapper(new MapperConfiguration(cfg =>
          {
              cfg.AddProfile<CarDealerProfile>();
          }));
    }
}