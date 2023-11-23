using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        [JsonProperty("Number")]
        public int Number { get; set; }

        [Required]
        [JsonProperty("IssueDate")]
        public DateTime IssueDate { get; set; }

        [Required]
        [JsonProperty("DueDate")]
        public DateTime DueDate { get; set; }

        [Required]
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }

        [Required]
        [JsonProperty("CurrencyType")]
        public int CurrencyType { get; set; }

        [JsonProperty("ClientId")]
        public int ClientId { get; set; }
    }

}
