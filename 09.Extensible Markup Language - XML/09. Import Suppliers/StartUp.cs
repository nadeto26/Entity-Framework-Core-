using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
             CarDealerContext context = new CarDealerContext();
             string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
              
            string input = ImportSuppliers(context, inputXml);
            Console.WriteLine(input);

        }
        //Zadacha9

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

        private static IMapper InitializeAutoMapper()
          => new Mapper(new MapperConfiguration(cfg =>
          {
              cfg.AddProfile<CarDealerProfile>();
          }));
    }
}