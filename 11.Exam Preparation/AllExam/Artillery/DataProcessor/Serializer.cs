
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Artillery.Utilities;
    using Newtonsoft.Json;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s=>s.ShellWeight>=shellWeight)
                .Select(s=> new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns.Where(x => ((int)x.GunType) == 3)
                    .Select(g=> new
                    {
                       GunType = g.GunType,
                       GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range",
                    })
                    .OrderByDescending(x => x.GunWeight)
                    .ToArray()
                })
                .OrderBy(x => x.ShellWeight)
                .ToArray();

            var json = JsonConvert.SerializeObject(shells, Formatting.Indented);
            return json;
      
                
        }

        private static XmlHelper xmlHelper;
        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        { 
             xmlHelper = new XmlHelper();


            var result = context.Guns.Where(x => x.Manufacturer.ManufacturerName == manufacturer)
               .Select(x => new ExportGunDto()
               {
                   Manufacturer = x.Manufacturer.ManufacturerName,
                   GunType = x.GunType.ToString(),
                   BarrelLength = x.BarrelLength,
                   GunWeight = x.GunWeight,
                   Range = x.Range,
                   Countries = x.CountriesGuns.Where(x => x.Country.ArmySize > 4500000)
                   .Select(a => new ExportCountryDto() 
                   {
                       Country = a.Country.CountryName,
                       ArmySize = a.Country.ArmySize
                   })
                 .OrderBy(x => x.ArmySize)
                 .ToArray()
               })
               .OrderBy(x => x.BarrelLength)
               .ToArray();

            // ExportCountriesDto[] result = Mapper.Map<ExportCountriesDto[]>(guns).OrderBy(x => x.BarrelLength).ToArray();

            return xmlHelper.Serialize(result , "Guns");

        }
    }
}
