using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

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
            this.CreateMap<Car,ExportCarDto>();


            //Customer
            this.CreateMap<ImportCustomersDto, Customer>()
                .ForMember(d => d.BirthDate,
                    opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));
            // ще си го парсне 

            //Sale
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d=>d.CarId,
                opt=> opt.MapFrom(s=>s.CarId.Value));

            //защото в models(destination) e not nullable a v dto(source) nullable






        }
    }
}
