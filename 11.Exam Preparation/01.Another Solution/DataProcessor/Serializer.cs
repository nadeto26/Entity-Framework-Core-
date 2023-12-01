
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Newtonsoft.Json;

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

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        { 
            throw new NotImplementedException();
        }
    }
}
