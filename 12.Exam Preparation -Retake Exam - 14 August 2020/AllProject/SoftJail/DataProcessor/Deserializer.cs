namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportDepartmentsCellsDto[] departmentDtos =
                JsonConvert.DeserializeObject<ImportDepartmentsCellsDto[]>(jsonString);

            ICollection<Department> validDepartment = new HashSet<Department>();


            foreach (var departmentDto in departmentDtos)
            {
                if (!IsValid(departmentDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (!departmentDto.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if(departmentDto.Cells.Any(c=> !IsValid(c)))
                {

                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentDto.Name,


                };

                foreach(var cellDto in  departmentDto.Cells)
                {
                    Cell cell = Mapper.Map<Cell>(cellDto);
                    department.Cells.Add(cell);
                }
                validDepartment.Add(department);
                sb.AppendLine($"Imported {departmentDto.Name} with {departmentDto.Cells.Count()} cells");

            }
            context.Departments.AddRange(validDepartment);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
             StringBuilder sb = new StringBuilder();

            ImportPrisonWithMailDto[] prisonWithMailDtos =
                JsonConvert.DeserializeObject<ImportPrisonWithMailDto[]>(jsonString);

            ICollection<Prisoner> validPrisoners = new HashSet<Prisoner>();

            foreach (var prisonDto in prisonWithMailDtos)
            {
                if (!IsValid(prisonDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (prisonDto.Mails.Any(c => !IsValid(c)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (!DateTime.TryParseExact(prisonDto.IncarcerationDate,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture,
                     DateTimeStyles.None, out var incarcerationDate) ||
                     !DateTime.TryParseExact(prisonDto.ReleaseDate,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var releaseDate))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = prisonDto.FullName,
                    Nickname = prisonDto.Nickname,
                    Age = prisonDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonDto.Bail,
                    CeilId = prisonDto.CeilId,
                };

                foreach (var mailsDto in prisonDto.Mails)
                {
                    Mail mail = Mapper.Map<Mail>(mailsDto);
                    prisoner.Mails.Add(mail);
                }

                validPrisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisonDto.FullName} {prisonDto.Age} years old");
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();
            return sb.ToString();

 
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Officers");
            XmlSerializer xmlSerializer
                = new XmlSerializer(typeof(ImportOfficersPrisonersDto[]),xmlRootAttribute);

            using StringReader stringReader = new StringReader(xmlString);
            ImportOfficersPrisonersDto[] obDtos = 
                (ImportOfficersPrisonersDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Officer> validOfficers = new HashSet<Officer>();    

            foreach(var obDtosDto in obDtos)
            {
                if(!IsValid(obDtosDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if(obDtosDto.Position.ToString() != "Overseer"
                    || obDtosDto.Position.ToString() != "Guard" || obDtosDto.Position.ToString() != "Watcher"
                    || obDtosDto.Position.ToString() != "Labour")
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (obDtosDto.Weapon.ToString() != "Knife"
                 || obDtosDto.Weapon.ToString() != "FlashPulse" || obDtosDto.Weapon.ToString() != "ChainRifle"
                 || obDtosDto.Weapon.ToString() != "Pistol" || obDtosDto.Weapon.ToString() != "Sniper")
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = obDtosDto.FullName,
                    Salary = obDtosDto.Salary,
                    Position = (Position)obDtosDto.Position,
                    Weapon = (Weapon)obDtosDto.Weapon ,
                    DepartmentId = obDtosDto.DepartmentId


                };

                foreach(var pDto in obDtosDto.Prisoners)
                {
                    OfficerPrisoner of = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = pDto.Id
                    };

                    officer.OfficerPrisoners.Add(of);
                }

                validOfficers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} {officer.OfficerPrisoners.Count()} prisoners)");
            }

            context.Officers.AddRange( validOfficers );
            context.SaveChanges();

            return sb.ToString();


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