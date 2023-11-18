namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";
        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        private static XmlHelper xmlHelper;

        

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            xmlHelper = new XmlHelper();

            ImportDespacheDto[] despatcharsDto =
                xmlHelper.Deserialize<ImportDespacheDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> validDespecher = new HashSet<Despatcher>();
            foreach(ImportDespacheDto desp in despatcharsDto)
            {
                if(!IsValid(desp))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Truck> vaidTrucks = new HashSet<Truck>();
                foreach(ImportTruckDto truckDto in desp.Trucks)
                {
                    if(!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType

                    };
                    vaidTrucks.Add(truck);
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = desp.Name,
                    Position = desp.Position,
                    Trucks = vaidTrucks
                };
                validDespecher.Add(despatcher);
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, validDespecher.Count()));
            }
            context.AddRange(validDespecher);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportClientDto[] clientDtos=
                JsonConvert.DeserializeObject < ImportClientDto[]>(jsonString);
            //десериализираме към  масив от дто-та , защото имаме масив от дто -> това го виждаме от xml(docs)

            ICollection<Client> validClients = new HashSet<Client>();
            ICollection<int> excitingTrucks = context.Trucks
                .Select(tc=> tc.Id)
                .ToArray();  

            foreach(ImportClientDto clientDto in clientDtos)
            {
                if(!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type
                    

                };

                foreach(int truckId in clientDto.TruckIds.Distinct()) //взимаме само уникалните 
                {
                    if (!excitingTrucks.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck ct = new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    };

                    client.ClientsTrucks.Add(ct);
                }
                validClients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }
            context.Clients.AddRange(validClients);
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