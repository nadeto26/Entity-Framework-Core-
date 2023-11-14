﻿using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Castle.Core.Resource;
using System.IO;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
             CarDealerContext context = new CarDealerContext();
             string inputXml = File.ReadAllText("../../../Datasets/customers.xml");
              
            string input = ImportCustomers(context, inputXml);
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

        private static IMapper InitializeAutoMapper()
          => new Mapper(new MapperConfiguration(cfg =>
          {
              cfg.AddProfile<CarDealerProfile>();
          }));
    }
}