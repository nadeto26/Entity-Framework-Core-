﻿using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using System.IO;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
             CarDealerContext context = new CarDealerContext();
             string inputXml = File.ReadAllText("../../../Datasets/parts.xml");
              
            string input = ImportParts(context, inputXml);
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

        private static IMapper InitializeAutoMapper()
          => new Mapper(new MapperConfiguration(cfg =>
          {
              cfg.AddProfile<CarDealerProfile>();
          }));
    }
}