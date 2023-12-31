﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Models;
using Newtonsoft.Json;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketsDto
    {
        [Required]
        [Range(typeof(decimal), "1", "100")]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }


    }
}
