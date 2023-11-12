using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class ImportCustomersJson
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("birthDate")]
        public DateTime BirtDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool IsYoungerDriver { get; set; }
    }
}
