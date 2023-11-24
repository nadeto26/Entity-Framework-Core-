namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Artillery.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
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
            xmlHelper = new XmlHelper();

            ImportCountryDto[] cDtos = xmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            ICollection<Country> validCountry = new HashSet<Country>();

            foreach (ImportCountryDto cDto in cDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = cDto.CountryName,
                    ArmySize = cDto.ArmySize
                };

                validCountry.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(validCountry);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();

            ImportManufactureDto[] mDtos = xmlHelper.Deserialize<ImportManufactureDto[]>
                (xmlString, "Manufacturers");

            ICollection<Manufacturer> validManufactures = new HashSet<Manufacturer>();

            foreach (ImportManufactureDto mDto in mDtos)
            {
                if (!IsValid(mDto))
                {
                    sb.AppendLine(ErrorMessage);
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = mDto.ManufacturerName,
                    Founded = mDto.Founded

                };

                validManufactures.Add(manufacturer);
                string[] foundedInfo = manufacturer.Founded.Split(", ");
                string townName = foundedInfo.Length > 1 ? foundedInfo[1] : string.Empty;
                string countryName = foundedInfo.Length > 2 ? foundedInfo[2] : string.Empty;

                sb.AppendLine(string.Format(SuccessfulImportCountry, manufacturer.ManufacturerName, townName, countryName));
            }

            context.Manufacturers.AddRange(validManufactures);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            xmlHelper = new XmlHelper();
            ImportShellDto[] sDtos = xmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            StringBuilder sb = new StringBuilder();

            ICollection<Shell> validShells = new HashSet<Shell>();

            foreach (ImportShellDto sDto in sDtos)
            {
                if (!IsValid(sDto))
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

                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var validGunTypes = new string[] { "Howitzer", "Mortar", "FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun" };
            var gunsJson = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            var guns = new List<Gun>();
            var sb = new StringBuilder();

            foreach (var dto in gunsJson)
            {
                if (!IsValid(dto) ||
                    !validGunTypes.Contains(dto.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var gun = new Gun
                {
                    ManufacturerId = dto.ManufacturerId,
                    GunWeight = dto.GunWeight,
                    BarrelLength = dto.BarrelLength,
                    NumberBuild = dto.NumberBuild,
                    Range = dto.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), dto.GunType),
                    ShellId = dto.ShellId
                };

                foreach (var countryDto in dto.Countries)
                {
                    gun.CountriesGuns.Add(new CountryGun
                    {
                        CountryId = countryDto,
                        Gun = gun
                    });
                }

                guns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, dto.GunType, dto.GunWeight, dto.BarrelLength));
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
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