namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Artillery.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        private static XmlHelper xmlHelper;
        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {

            StringBuilder sb = new StringBuilder();
           XmlHelper xmHelper = new XmlHelper();

            ImportCountryDto[] cDtos = xmHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

           ICollection<Country> validCountries = new HashSet<Country>();

            foreach(ImportCountryDto cDto in cDtos)
            {
                if(!IsValid(cDto))
                {
                   sb.AppendLine(ErrorMessage);
                   continue;
                }

                Country country = new Country()
                {
                    CountryName = cDto.CountryName,
                    ArmySize = cDto.ArmySize
                };
                validCountries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry,country.CountryName,country.ArmySize));
            }
            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            xmlHelper = new XmlHelper();

            ImportManufacturesDto[] mDtos = xmlHelper.Deserialize<ImportManufacturesDto[]>(xmlString, "Manufacturers");

            StringBuilder sb = new StringBuilder();

            ICollection<Manufacturer> validManufucters = new HashSet<Manufacturer>();

            foreach(ImportManufacturesDto mDto in mDtos)
            {
              

                if (!IsValid(mDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = mDto.ManufacturerName,
                    Founded = mDto.Founded
                };
                validManufucters.Add(manufacturer);

                var manufacturerCountry = manufacturer.Founded.Split(", ").ToArray();
                var last = manufacturerCountry.Skip(Math.Max(0, manufacturerCountry.Count() - 2)).ToArray();

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", last)));
            }
            context.Manufacturers.AddRange(validManufucters);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
             XmlHelper xmlHelper = new XmlHelper();

            ImportShellDto[] sDtos = xmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            StringBuilder sb = new StringBuilder();

            ICollection < Shell> validShells = new HashSet<Shell>();    

            foreach(ImportShellDto sDto in sDtos)
            {
                if(!IsValid(sDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = sDto.ShellWeight,
                    Caliber = sDto.Caliber
                };
                validShells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell,shell.Caliber,shell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            //var gDtos =
            //   JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            //StringBuilder sb = new StringBuilder();

            //ICollection<Gun> validGun = new HashSet<Gun>();

            //foreach(ImportGunDto gDto in gDtos)
            //{
            //    if(!IsValid(gDto))
            //    {
            //        sb.AppendLine(ErrorMessage);
            //        continue;
            //    }
            //    object gunTypeObj;
            //    bool isGunTypeValid = Enum.TryParse(typeof(GunType), gDto.GunType, out gunTypeObj);

            //    if (!isGunTypeValid)
            //    {
            //        sb.AppendLine(ErrorMessage);
            //        continue;
            //    }

            //    Gun gun = new Gun()
            //    {
            //        ManufacturerId = gDto.ManufacturerId,
            //        GunWeight = gDto.GunWeight,
            //        BarrelLength = gDto.BarrelLength,
            //        NumberBuild = gDto.NumberBuild,
            //        Range = gDto.Range,
            //        GunType = (GunType)gunTypeObj,
            //        ShellId = gDto.ShellId
            //    };

            //    foreach(int countriesId in  gDto.Countries)
            //    {
            //        CountryGun countryGun = new CountryGun()
            //        {
            //            Gun = gun,
            //            CountryId = countriesId

            //        };
            //        gun.CountriesGuns.Add(countryGun);
            //    }
            //    validGun.Add(gun);
            //    sb.AppendLine(string.Format(SuccessfulImportGun,gun.GunType,gun.GunWeight,gun.BarrelLength));

            //}
            //context.Guns.AddRange(validGun);
            //context.SaveChanges();

            //return sb.ToString().TrimEnd();
           throw new NotImplementedException();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}