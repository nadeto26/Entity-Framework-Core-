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

            //Part

            this.CreateMap<ImportPartDto, Part>()
              .ForMember(d => d.SupplierId,
                  opt => opt.MapFrom(s => s.SupplierId!.Value));

            //да се мапне към SupplierId.Value -> защото то вече е nullable в data

            //Car

            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            //Няма да го пипа – няма да го валидира 






        }
    }
}
