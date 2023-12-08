using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EmployeeScreeningTest.Models
{
    /// <summary>
    /// Represents an employee entity with properties corresponding to database fields.
    /// The JsonProperty attribute is used to ensure compatibility with the property naming in Cosmos DB.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the unique identifier for the employee. Mapped to 'id' in Cosmos DB.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the department the employee belongs to.
        /// </summary>
        [JsonProperty(PropertyName = "DepartmentId")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the employee.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the position or title of the employee.
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// Gets or sets the name of the department the employee works in.
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Gets or sets the number of years the employee has been with the organization.
        /// </summary>
        public int Tenure { get; set; }
    }
}
