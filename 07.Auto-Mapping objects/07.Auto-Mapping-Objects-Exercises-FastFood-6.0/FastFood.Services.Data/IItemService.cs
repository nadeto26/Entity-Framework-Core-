using FastFood.Core.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Data
{
    public interface IItemService
    {
        Task CreateAsync(CreateItemInputModel model);

        Task<IEnumerable<ItemsAllViewModels>> GetAllAsync();

        Task<IEnumerable<CreateItemViewModel>> GetAllAvailableCategoriesAsync();
    }
}
