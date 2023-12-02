namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Globalization;

    public class Serializer
    {
        private static XmlHelper xmlHelper;
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            xmlHelper = new XmlHelper();

            DateTime dateAsDateTime = DateTime.Parse(date); // Convert string to DateTime

            var patients = context.Patients
            .Where(p => p.PatientsMedicines.Any(m => m.Medicine.ProductionDate > dateAsDateTime))
            .OrderByDescending(p => p.PatientsMedicines.Count)
            .ThenBy(p => p.FullName)
            .Select(p => new ExportPatientDto
            {
                FullName = p.FullName,
                Gender = p.Gender.ToString(),
                AgeGroup = p.AgeGroup.ToString(),
                Medicine = p.PatientsMedicines
           .Where(m => m.Medicine.ProductionDate > dateAsDateTime)
           .OrderByDescending(m => m.Medicine.ExpiryDate)
           .ThenBy(m => m.Medicine.Price)
           .Select(m => new ExportMedicineDto
           {
               Name = m.Medicine.Name,
               Price = m.Medicine.Price.ToString("F2"),
               Category = m.Medicine.Category.ToString(),
               Producer = m.Medicine.Producer,
               BestBefore = m.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
           })
           .ToArray()
            })
         .ToArray();




            return xmlHelper.Serialize(patients, "Patients");

           
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicinesToExport = context.Medicines
      .Where(m => (int)m.Category == medicineCategory && m.Pharmacy.IsNonStop)
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

            return JsonConvert.SerializeObject(medicinesToExport, Formatting.Indented);

        }
    }
}
