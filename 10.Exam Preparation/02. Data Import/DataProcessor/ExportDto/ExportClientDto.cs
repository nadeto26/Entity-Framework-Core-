using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientDto
    {
        [XmlAttribute]
        public int InvoicesCount { get; set; }

        [XmlElement("ClientName")]
        public string ClientName { get; set; }

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; }

        [XmlArray("Invoices")]
        public ExportInvoicesDto[] Invoices { get; set; }
    }
}
