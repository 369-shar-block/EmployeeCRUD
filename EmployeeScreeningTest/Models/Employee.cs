using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EmployeeScreeningTest.Models
{
    public class Employee
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "DepartmentId")]
        public string DepartmentId { get; set; } 

        public string? Name { get; set; }

        public int Age { get; set; }
        public string? Position { get; set; }

        public string? DepartmentName { get; set; }

        public int Tenure { get; set; }
    }
}
