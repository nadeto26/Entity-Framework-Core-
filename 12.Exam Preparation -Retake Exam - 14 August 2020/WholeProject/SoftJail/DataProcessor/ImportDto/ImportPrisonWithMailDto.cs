using Newtonsoft.Json;
using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonWithMailDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        [JsonProperty(nameof(FullName))]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("^(The\\s)([A-Z][a-z]*)$")]
        [JsonProperty(nameof(Nickname))]
        public string Nickname { get; set; }

        [Required]
        [JsonProperty(nameof(Age))]
        [MaxLength(65)]
        [MinLength(18)]
        public int Age { get; set; }

        [Required]
        [JsonProperty(nameof(IncarcerationDate))]
        public DateTime IncarcerationDate { get; set; }

        [JsonProperty(nameof(ReleaseDate))]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty(nameof(Bail))]
        [MinLength(0)]
        public decimal? Bail { get; set; }

        [JsonProperty("CellId")]
        public int? CeilId { get; set; }

        [JsonProperty(nameof(Mails))]
        public ImportPrisonWithMailDto[] Mails { get; set; }
    }
}
