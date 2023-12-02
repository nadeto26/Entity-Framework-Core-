namespace Invoices.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";

        private static XmlHelper xmlHelper;
        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();

            ImportClientDto[] cDtos = xmlHelper.Deserialize<ImportClientDto[]>(xmlString, "Clients");

            ICollection<Client> validClients = new HashSet<Client>();

            foreach (ImportClientDto cDto in cDtos)
            {
                if(!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> validAdreses = new HashSet<Address>();

                foreach(ImportAdressDto aDto in cDto.Addresses)
                {
                    if(!IsValid(aDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address()
                    {
                        StreetName = aDto.StreetName,
                        StreetNumber = aDto.StreetNumber,
                        PostCode = aDto.PostCode,
                        City = aDto.City,
                        Country = aDto.Country

                    };

                    validAdreses.Add(address);
                }

                Client client = new Client()
                {
                    Name = cDto.Name,
                    NumberVat = cDto.NumberVat,
                    Addresses = validAdreses
                };

                validClients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();


        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportInvoiceDto[] iDtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);

            ICollection<Invoice> validInvoices = new HashSet<Invoice>();

            foreach(ImportInvoiceDto iDto in iDtos)
            {
                if (!IsValid(iDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                

                Invoice invoice = new Invoice()
                {
                    Number = iDto.Number,
                    IssueDate = iDto.IssueDate,
                    DueDate = iDto.DueDate,
                    Amount = iDto.Amount,
                    CurrencyType = (CurrencyType)iDto.CurrencyType,
                    ClientId = iDto.ClientId

                };
                validInvoices.Add(invoice);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices,invoice.Number));
            }
            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {

            StringBuilder sb = new StringBuilder();

            ImportProductDto[] pDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);   

            ICollection<Product> validProducts = new HashSet<Product>();
            ICollection<int> existingProductsId = context.Products
             .Select(t => t.Id)
             .ToArray();

            foreach (ImportProductDto pDto in pDtos)
            {
                if(!IsValid(pDto))
                {
                    sb.Append(ErrorMessage);
                    continue;
                }

                Product product = new Product()
                {
                    Name = pDto.Name,
                    Price = pDto.Price,
                    CategoryType = (CategoryType)pDto.CategoryType

                };
                   
                foreach(int clientId in pDto.ClientsIds.Distinct())
                {
                    if(!existingProductsId.Contains(clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient()
                    {
                        Product = product,
                        ClientId = clientId
                    };
                    product.ProductsClients.Add(productClient);
                }
                validProducts.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count()));
            }
            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
