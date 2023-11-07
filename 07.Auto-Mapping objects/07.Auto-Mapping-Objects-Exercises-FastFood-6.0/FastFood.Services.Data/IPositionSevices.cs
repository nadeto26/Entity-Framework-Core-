using FastFood.Core.ViewModels.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Data
{
    public  interface IPositionSevices
    {
        //методите , които искаме да изнесе навън

        Task CreateAsync(CreatePositionInputModel inputModel);

       Task<IEnumerable<PositionsAllViewModel>> GetAllAsync();
    }
}
