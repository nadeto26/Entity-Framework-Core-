namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using System.Diagnostics;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            var patientsToExport = context.Patients
                .Where(p=>p.PatientsMedicines.Count() > 0 && p.PatientsMedicines.Any(pm=>pm.Medicine.ProductionDate > DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .Select(p=> new ExportPatientDTO
                {
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Gender = p.Gender.ToString().ToLower(),
                    Medicines = p.PatientsMedicines
                    .Where(pm=> pm.Medicine.ProductionDate > DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                    .OrderByDescending(pm=>pm.Medicine.ExpiryDate)
                    .ThenBy(pm=>pm.Medicine.Price)
                    .Select(pm=> new ExportMedicineDTO
                    {
                        Name = pm.Medicine.Name,
                        Price = pm.Medicine.Price.ToString("F2"),
                        Producer = pm.Medicine.Producer,
                        BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd"),
                        Category = pm.Medicine.Category.ToString().ToLower(),
                    }).ToArray()

                })
                .OrderByDescending(p=>p.Medicines.Count())
                .ThenBy(p=>p.Name)
                .ToList();

            var result = XmlHelper.Serialize(patientsToExport, "Patients");

            return result;
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicinesToExport = context.Medicines
                .Where(m=> (int)m.Category == medicineCategory && m.Pharmacy.IsNonStop)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = m.Price.ToString("F2"),
                    Pharmacy = new
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber
                    }
                })
                .ToList();

            var result = JsonSerializationExtension.SerializeToJson(medicinesToExport);
            return result;
        }
    }
}
