using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Web.ViewModels.Categories
{
    public class ListAllCategoriesViewModel
    {
        public IEnumerable<ListCategoryViewModel> AllCategories { get; set; }

        public int PageCount { get; set; }

        public int ActivePage { get; set; }
    }
}
