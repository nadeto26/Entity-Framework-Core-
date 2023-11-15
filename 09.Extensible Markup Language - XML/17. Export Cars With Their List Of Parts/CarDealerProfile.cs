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

            this.CreateMap<Part, ExportCarPartDto>();
          

            //Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            //Няма да го пипа – няма да го валидира 

            this.CreateMap<Car,ExportCarDto>();
            this.CreateMap<Car, ExportBmwCarDto>();
            //това е досатъчно , защото са със сходни имена с models 

            this.CreateMap<Car, ExportCarWithPartsDto>()
             .ForMember(d => d.Parts,
                 opt => opt.MapFrom(s =>
                     s.PartsCars
                         .Select(pc => pc.Part)
                         .OrderByDescending(p => p.Price)
                         .ToArray()));


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
