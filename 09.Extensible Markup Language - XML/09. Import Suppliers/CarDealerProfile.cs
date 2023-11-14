using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        //Да мапне от едното към дрегото

        public CarDealerProfile()
        {
            //Supplier 
            
            this.CreateMap<ImportSupplierDto,Supplier>();

            



        }
    }
}
