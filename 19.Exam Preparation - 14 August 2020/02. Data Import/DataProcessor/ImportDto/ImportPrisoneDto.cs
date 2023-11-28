using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisoneDto
    {
        [Required]
        [MaxLength(GlobalConstants.PrisonerFullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The [A-Z][a-zA-Z]*$")]
        public string Nickname { get; set; }

        public int Age { get; set; }

        public string IncarcerationDate { get; set; }

        public string? ReleaseDate { get; set; }

        public decimal? Bail { get; set; }
 
        public int? CellId { get; set; }

        public  ImportMailDto[] Mails { get; set; }


    }
}
