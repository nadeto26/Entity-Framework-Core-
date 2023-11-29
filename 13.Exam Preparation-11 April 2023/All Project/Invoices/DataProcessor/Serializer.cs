namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        static XmlHelper XmlHelper;
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportClientDto[] clients =
                context.Clients
                .ToArray()
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))
                .Select(c => new
                {
                    Name = c.Name,
                    NumberVat = c.NumberVat,
                    InvoicesCount = c.Invoices.Count(),
                    Invoices = c.Invoices
                    .Select(i => new ExportInvoiceDto()
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        Currency = i.CurrencyType.ToString(),
                        DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture)
                    })
                    .ToArray()
                   .OrderBy(i => i.IssueDate)
                   .ThenByDescending(i => i.DueDate)
                })
                .OrderByDescending(i => i.InvoicesCount)
                .ThenBy(i => i.Name)
                .ToArray();

            return xmlHelper.Serialize(clients, "Clients");
        }

            public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context.Products
              .Where(p => p.ProductsClients.Any() && p.Name.Length >= nameLength)
              .ToArray()
              .Select(p => new
              {
                  Name = p.Name,
                  Price = p.Price,
                  CategoryType = p.CategoryType.ToString(),
                  Client = p.ProductsClients
                  .Where(pc => pc.Client.Name.Length >= nameLength)
                  .Select(pc => new
                  {
                      Name = pc.Client.Name,
                      NumberVat = pc.Client.NumberVat
                  })
                  .ToArray()
                  .OrderBy(p => p.Name)
              })
              .OrderByDescending(p => p.Client.Count())
              .ThenBy(p => p.Name)
              .Take(5)
              .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);

        }
    }

}
   