using System.ComponentModel.DataAnnotations;

namespace Sistema.Models{
    public class DateHistory{
        [Key]
        public int Id { get; set; }
        public System.DateTime? Entries { get; set; }
        public System.DateTime? Exits { get; set; }
        public int EmployeesId { get; set; }
    }
}