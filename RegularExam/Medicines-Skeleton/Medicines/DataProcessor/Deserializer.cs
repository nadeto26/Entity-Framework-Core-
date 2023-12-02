namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

    
        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPationDto[] pDtos = JsonConvert.DeserializeObject<ImportPationDto[]>(jsonString);

            ICollection<Patient> validPatiens = new HashSet<Patient>();
            ICollection<int> existingMedicineIds = context.Medicines
                .Select(m => m.Id)
                .ToArray();

            foreach (ImportPationDto pDto in pDtos)
            {
                if (!IsValid(pDto) || (pDto.AgeGroup != 0 && pDto.AgeGroup != 1 && pDto.AgeGroup != 2) || (pDto.Gender != 0 && pDto.Gender != 1))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient p = new Patient()
                {
                    FullName = pDto.FullName,
                    AgeGroup = (AgeGroup)pDto.AgeGroup,
                    Gender = (Gender)pDto.Gender
                };

                foreach (int mId in pDto.Medicines)
                {
                    if (!existingMedicineIds.Contains(mId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine pM = new PatientMedicine()
                    {
                        MedicineId = mId,
                        Patient = p
                    };

                    p.PatientsMedicines.Add(pM);
                }

                sb.AppendFormat(SuccessfullyImportedPatient, p.FullName, p.PatientsMedicines.Count()).AppendLine();

                validPatiens.Add(p);
            }

            context.Patients.AddRange(validPatiens);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static XmlHelper xmlHelper;
        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {

            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            ImportPharmacyDto[] pDtos = xmlHelper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            ICollection<Pharmacy> validPharmacy = new HashSet<Pharmacy>();
            HashSet<string> uniqueMedicineKeys = new HashSet<string>();

            foreach (ImportPharmacyDto pDto in pDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine("Invalid Data!");
                    continue;
                }

                bool isNonStop;
                if (!bool.TryParse(pDto.IsNonStop.ToString(), out isNonStop))
                {
                    sb.AppendLine("Invalid Data!");
                    continue;
                }

                ICollection<Medicine> validMedicines = new HashSet<Medicine>();

                foreach (ImportMedicineDto mDto in pDto.Medicines)
                {
                    if (!IsValid(mDto))
                    {
                        sb.AppendLine("Invalid Data!");
                        continue;
                    }

                    DateTime productionDate;
                    bool isProductionDateValid = DateTime.TryParseExact(mDto.ProductionDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out productionDate);

                    DateTime expiryDate;
                    bool isExpiryDateValid = DateTime.TryParseExact(mDto.ExpiryDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out expiryDate);

                    if (productionDate >= expiryDate || !isExpiryDateValid || !isProductionDateValid || mDto.Category < 0 || mDto.Category > 4)
                    {
                        sb.AppendLine("Invalid Data!");
                        continue;
                    }

                    string medicineKey = $"{mDto.Name}_{mDto.Producer}";

                    if (uniqueMedicineKeys.Contains(medicineKey))
                    {
                        sb.AppendLine("Invalid Data!");
                        continue;
                    }

                    // Check for duplicate medicine in other pharmacies
                    if (validPharmacy.Any(p => p.Medicines.Any(m => m.Name == mDto.Name && m.Producer == mDto.Producer)))
                    {
                        sb.AppendLine("Invalid Data!");
                        continue;
                    }

                    Medicine m = new Medicine()
                    {
                        Category = (Category)mDto.Category,
                        Name = mDto.Name,
                        Price = mDto.Price,
                        ExpiryDate = expiryDate,
                        ProductionDate = productionDate,
                        Producer = mDto.Producer
                    };
                    validMedicines.Add(m);
                    uniqueMedicineKeys.Add(medicineKey);
                }

                Pharmacy p = new Pharmacy()
                {
                    IsNonStop = isNonStop,
                    Name = pDto.Name,
                    PhoneNumber = pDto.PhoneNumber,
                    Medicines = validMedicines
                };

                validPharmacy.Add(p);
                sb.AppendLine(string.Format("Successfully imported pharmacy - {0} with {1} medicines.", p.Name, validMedicines.Count()));
            }

            context.Pharmacies.AddRange(validPharmacy);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
