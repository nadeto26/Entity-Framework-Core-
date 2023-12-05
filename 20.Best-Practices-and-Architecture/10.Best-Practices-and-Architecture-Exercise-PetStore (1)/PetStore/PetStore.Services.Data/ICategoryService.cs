using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Services.Data
{
    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryInputModel inputModel);

        Task<IEnumerable<ListCategoryViewModel>> GetAllAsync();

        Task<IEnumerable<ListCategoryViewModel>> GetAllWithPaginationAsync(int pageNumber);

        Task<EditCategoryViewModel> GetByIdAndPrepareForEditAsync(int id);

        Task EditCategoryAsync(EditCategoryViewModel inputModel);

        Task<bool> ExistsAsync(int id);
    }
}
