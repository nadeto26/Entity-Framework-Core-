namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using System.ComponentModel.DataAnnotations;
    using Medicines.Utilities;
    using Medicines.DataProcessor.ImportDtos;
    using System.Text;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using System.Globalization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var patientDTOs = JsonSerializationExtension.DeserializeFromJson<List<ImportPatientDTO>>(jsonString);
            var patientsToImport = new List<Patient>();
            StringBuilder sb = new StringBuilder();

            foreach (var pDTO in patientDTOs)
            {
                if (!IsValid(pDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient
                {
                    FullName = pDTO.FullName,
                    AgeGroup = pDTO.AgeGroup,
                    Gender = pDTO.Gender,
                };

                foreach (var med in pDTO.Medicines)
                {
                    if (patient.PatientsMedicines.Any(x=>x.MedicineId == med))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    PatientMedicine pm = new PatientMedicine
                    {
                        MedicineId = med
                    };
                    patient.PatientsMedicines.Add(pm);
                }

                patientsToImport.Add(patient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count()));
            }

            context.Patients.AddRange(patientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            var pharmacyDTOs = XmlHelper.Deserialize<List<ImportPharmacyDTO>>(xmlString,"Pharmacies");
            StringBuilder sb = new StringBuilder();
            var trueFalseList = new List<string> { "true","false"};
            var pharmacies = new List<Pharmacy>();

            foreach (var pharmaDTO in pharmacyDTOs)
            {
                if (!IsValid(pharmaDTO) || !trueFalseList.Contains(pharmaDTO.IsNonStop))
                {
                    sb.AppendLine("Invalid Data!");
                    continue;
                }

                var medicineList = new List<Medicine>();

                Pharmacy pharmacyToImport = new Pharmacy
                {
                    Name = pharmaDTO.Name,
                    PhoneNumber = pharmaDTO.PhoneNumber,
                    IsNonStop = bool.Parse(pharmaDTO.IsNonStop),
                };

                foreach (var medDTO in pharmaDTO.Medicines)
                {


                    bool validProdDate = DateTime.TryParseExact(medDTO.ProductionDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var productionDate);                    
                    
                    bool validexpiryDate = DateTime.TryParseExact(medDTO.ExpiryDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var expiryDate);

                    if (!IsValid(medDTO) || !validProdDate || !validexpiryDate || productionDate >= expiryDate || medicineList.Any(x=> x.Name == medDTO.Name && x.Producer == medDTO.Producer))
                    {
                        sb.AppendLine("Invalid Data!");
                        continue;
                    }


                    Medicine medicine = new Medicine
                    {
                        Name=medDTO.Name,
                        Price = medDTO.Price,
                        Category = (Category)medDTO.Category,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medDTO.Producer
                    };

                    medicineList.Add(medicine);
                }

                pharmacyToImport.Medicines = medicineList;
                pharmacies.Add(pharmacyToImport);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy,pharmacyToImport.Name, pharmacyToImport.Medicines.Count()));
            }

            context.Pharmacies.AddRange(pharmacies);
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
