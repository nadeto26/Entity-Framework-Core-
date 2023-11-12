using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supplier 

            this.CreateMap<ImportSupplieDto, Supplier>();

            //Part

            this.CreateMap<ImportPartDto, Part>();

            //Cars

            this.CreateMap<ImportCarsDto, Car>();   
        }
    }
}
