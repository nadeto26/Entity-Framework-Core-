namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System.Linq;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<ImportDepartmentsCellsDto, Cell>();

            this.CreateMap<ImportPrisonWithMailDto,Mail>();

            this.CreateMap<Mail, EncryptedMessagesDto>()
                .ForMember(d => d.Description,
                mo => mo.MapFrom(s => s.Description.Reverse()));

            this.CreateMap < Prisoner, ExportPrisonerDto> ()
                . ForMember(d=>d.IncarcerationDate,
               mo => mo.MapFrom ( s => s.IncarcerationDate.ToString("yyyy-MM-dd")))
              .ForMember(d => d.Mails,  mo => mo.MapFrom(s => s.Mails));
        }
    }
}
