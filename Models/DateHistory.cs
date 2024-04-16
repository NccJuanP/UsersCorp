using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Models{
    public class DateHistory{
        [Key]
        public int Id { get; set; }
        public string? Entries { get; set; }
        public string? Exits { get; set; }
        public int EmployeesId  { get; set; }
    }
}