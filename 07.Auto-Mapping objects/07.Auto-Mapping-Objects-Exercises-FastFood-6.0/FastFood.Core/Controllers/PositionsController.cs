namespace FastFood.Core.Controllers
{
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
   
    using FastFood.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Positions;

    public class PositionsController : Controller
    {
        private readonly IPositionSevices positionsService;

        public PositionsController(IPositionSevices positionsService)
        {
            this.positionsService = positionsService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            await this.positionsService.CreateAsync(model);

            return RedirectToAction("All", "Positions");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<PositionsAllViewModel> positions =
                await this.positionsService
                    .GetAllAsync();

            return View(positions.ToList());
        }
    }
}
