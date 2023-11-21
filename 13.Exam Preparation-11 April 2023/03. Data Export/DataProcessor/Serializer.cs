namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    public class Serializer
    {
        private static XmlHelper xmlHelper;
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            xmlHelper = new XmlHelper();

            ExportClientDto[] clientDtos =
                context.Clients
                  .Include(c => c.Invoices)
                .ThenInclude(ct => ct.Client)
                .ToArray()
                
                .Where(c=>c.Invoices.Any(i=>i.IssueDate>date))
                .Select(c=>new ExportClientDto()
                {
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    InvoicesCount = c.Invoices.Count,
                    Invoices = c.Invoices
                      .Select(i => new ExportInvoicesDto()
                      {
                          InvoiceNumber = i.Number,
                          Amount = i.Amount.ToString("F2", CultureInfo.InvariantCulture),
                          CurrencyType = (CurrencyType)i.CurrencyType,
                          DueDate = i.DueDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                      })
                      .OrderByDescending(i => i.DueDate)
                      .ToArray()
                })
                .OrderByDescending(c=>c.InvoicesCount)
                .ThenBy(c=>c.ClientName)
                .ToArray();


            return xmlHelper.Serialize(clientDtos, "Clients");

        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context.Products
                .Include(c => c.ProductsClients)
                .ThenInclude(ct => ct.Product)
                 .ToArray()
                 .Take(5)
               .Where(pr => pr.ProductsClients.Any(pr => pr.Client.Name.Length >= nameLength))
              .Select(pr=>new
              {
                  Name = pr.Name,
                  Price = pr.Price,
                  Category = pr.CategoryType.ToString(),
                  Clients = pr.ProductsClients
                   .Where(pC => pC.Client.Name.Length >= nameLength)
                  .Select(pC=>new
                  {
                      Name = pC.Client.Name,
                      NumberVat = pC.Client.NumberVat
                  })
                  
                  .OrderBy(p => p.Name)
                  .ToArray()
              })
              .OrderByDescending(p=>p.Clients.Count())
            .ThenBy(p=>p.Name)
            .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);

        }
    }
}