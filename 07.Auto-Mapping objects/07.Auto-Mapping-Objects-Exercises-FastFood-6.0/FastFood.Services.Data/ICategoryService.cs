using FastFood.Core.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Data
{
    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryInputModel model);

        Task<IEnumerable<CategoryAllViewModel>> GetAllAsync();
    }
}
