namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportPrisonersDto[] dDtos = JsonConvert.DeserializeObject<ImportPrisonersDto[]>(jsonString);

            ICollection<Department> validDepartments = new HashSet<Department>();
            
           

            foreach(ImportPrisonersDto dDto in dDtos)
            {
                if(!IsValid(dDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                if(dDto.Cells.Count()==0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!IsValid(dDto.Cells[0]))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Department department = new Department()
                {
                    Name = dDto.Name
                };
                

                foreach(ImportCell cDto in dDto.Cells)
                {
                    if(!IsValid(cDto))
                    {
                        sb.Append(ErrorMessage); continue;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber = cDto.CellNumber,
                        HasWindow = cDto.HasWindow,
                        Department = department
                    };
                   
                    department.Cells.Add(cell);
                }
                validDepartments.Add(department);
                sb.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count()));
                
            }
            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportPrisoneDto[] dDtos = JsonConvert.DeserializeObject<ImportPrisoneDto[]>(jsonString);

            ICollection<Prisoner> validPrisoners = new HashSet<Prisoner>();


            foreach (ImportPrisoneDto dDto in dDtos)
            {
                DateTime releaseDate;
                bool isReleaseDateValid = DateTime.TryParseExact(dDto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                DateTime incarcerationDate;
                bool isncarcerationDatValid = DateTime.TryParseExact(dDto.IncarcerationDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                if (!IsValid(dDto))
                {
                    sb.Append(ErrorMessage); continue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = dDto.FullName,
                    Nickname = dDto.Nickname,
                    Age = dDto.Age,
                    ReleaseDate =  releaseDate,
                    IncarcerationDate =  incarcerationDate,
                    Bail = dDto.Bail,
                    CellId = dDto.CellId
                 };

                foreach(ImportMailDto mDto in dDto.Mails)
                {
                    if(!IsValid(mDto))
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                    prisoner.Mails.Add(new Mail()
                    {
                        Description = mDto.Description,
                        Sender = mDto.Sender,
                        Address = mDto.Address,
                        Prisoner = prisoner
                    });
                   validPrisoners.Add(prisoner);
                  

                }

                sb.AppendLine(string.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));

            }
            context.Prisoners.AddRange(validPrisoners);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static XmlHelper xmlHelper;
        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {

            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();

            ImportOfiicerDto[] oDtos = xmlHelper.Deserialize<ImportOfiicerDto[]>(xmlString, "Officers");

            ICollection<Officer> validOfficers = new HashSet<Officer>();

            foreach (ImportOfiicerDto oDto in oDtos)
            {
                object weaponObj;
                bool isWeaponValid = Enum.TryParse(typeof(Weapon), oDto.Weapon, out weaponObj);

                object positionObj;
                bool isPositionValid = Enum.TryParse(typeof(Weapon), oDto.Weapon, out positionObj);

                if (!IsValid(weaponObj))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!IsValid(positionObj))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (oDto.Salary < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!IsValid(oDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer o = new Officer()
                {
                    FullName = oDto.FullName,
                    Salary = oDto.Salary,
                    Position = (Position)positionObj,
                    Weapon = (Weapon)weaponObj,
                    DepartmentId = oDto.DepartmentId
                };

                foreach (ImportPrisonerOfficer pDto in oDto.Prisoners)
                {
                    if (!IsValid(pDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    o.OfficerPrisoners.Add((new OfficerPrisoner
                    {
                        PrisonerId = pDto.Id
                    }));

                    validOfficers.Add(o);

                }
                sb.AppendLine(string.Format(SuccessfullyImportedOfficer,o.FullName,o.OfficerPrisoners.Count()));
            }
            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}