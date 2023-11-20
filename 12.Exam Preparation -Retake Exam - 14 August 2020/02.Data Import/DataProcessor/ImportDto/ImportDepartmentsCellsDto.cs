﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentsCellsDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Cells))]
        public ImportCellDto[] Cells { get; set; }
    }
}
