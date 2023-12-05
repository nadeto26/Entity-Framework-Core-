using PetStore.Data.Models;
using PetStore.Services.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Web.ViewModels.Categories
{
    public class EditCategoryViewModel : IMapFrom<Category>, IMapTo<Category>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryInputModelValidationConstants.NameMaxLength,
            MinimumLength = CategoryInputModelValidationConstants.NameMinLength,
            ErrorMessage = CategoryInputModelValidationConstants.NameLengthErrorMessage)]
        public string Name { get; set; } = null!;
    }
}
