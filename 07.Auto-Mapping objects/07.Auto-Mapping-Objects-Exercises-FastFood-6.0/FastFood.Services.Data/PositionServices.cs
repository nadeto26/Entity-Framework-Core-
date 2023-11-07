using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Core.ViewModels.Positions;
using FastFood.Dataa;
using FastFood.Model;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data
{
    public class PositionServices : IPositionSevices
    {

        //dependency injection 
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public PositionServices(IMapper mapper,FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(CreatePositionInputModel inputModel)
        { 
            Position position = this.mapper.Map<Position>(inputModel);

             await  context.Positions.AddAsync(position);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PositionsAllViewModel>> GetAllAsync()
       
             
       => await this.context.Positions
           .ProjectTo<PositionsAllViewModel>(this.mapper.ConfigurationProvider)
           .ToArrayAsync();
        
    }
}