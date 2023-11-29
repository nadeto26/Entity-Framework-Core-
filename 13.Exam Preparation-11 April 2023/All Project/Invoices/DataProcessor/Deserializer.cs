namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
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


        static public XmlHelper xmlHelper;
        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            xmlHelper = new XmlHelper();

            ImportClientDto[] cDtos = xmlHelper.Deserialize<ImportClientDto[]>(xmlString, "Clients");
 

            StringBuilder sb = new StringBuilder();

            ICollection<Client> validClients = new HashSet<Client>();

            foreach (ImportClientDto cDto in cDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = cDto.Name,
                    NumberVat = cDto.NumberVat
                };


                foreach (ImportAdressDto aDto in cDto.Addresses)
                {
                    if (!IsValid(aDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;

                    }
                    client.Addresses.Add(new Address()
                    {
                        StreetName = aDto.StreetName,
                        StreetNumber = aDto.StreetNumber,
                        PostCode = aDto.PostCode,
                        City = aDto.City,
                        Country = aDto.Country,
                        Client = client

                    });
                    validClients.Add(client);


                }
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }
            context.Clients.AddRange(validClients);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportInvoicesDto[] idtos = JsonConvert.DeserializeObject<ImportInvoicesDto[]>(jsonString);

            ICollection<Invoice> validInvoices = new HashSet<Invoice>();
 



            foreach (ImportInvoicesDto iDto in idtos)
            {

                object currencyTypeObj;
                bool isWeaponValid = Enum.TryParse(typeof(CurrencyType), iDto.CurrencyType, out currencyTypeObj);
               
                if (iDto.IssueDate>iDto.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (iDto.Amount < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!IsValid(iDto))
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
                    CurrencyType = (CurrencyType)currencyTypeObj,
                    ClientId = iDto.ClientId
                };

                validInvoices.Add(invoice);

                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));

            }

            context.Invoices.AddRange(validInvoices);
            //context.SaveChanges();

            return sb.ToString().TrimEnd();




        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            //object categoryTypeObj;
            //bool isWeaponValid = Enum.TryParse(typeof(CategoryType),(CategoryType)pDto.CategoryType, out categoryTypeObj);

            //if (!isWeaponValid)
            //{
            //    sb.AppendLine(ErrorMessage);
            //    continue;
            //}
            StringBuilder sb = new StringBuilder();

            ImportProductDto[] pDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

            ICollection<Product> validProducts = new HashSet<Product>();
            ICollection<int> existingProductsId = context.Products
             .Select(t => t.Id)
             .ToArray();

            foreach (ImportProductDto pDto in pDtos)
            {
                if (!IsValid(pDto))
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

                foreach (int clientId in pDto.ClientsIds.Distinct())
                {
                    if (!existingProductsId.Contains(clientId))
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
