using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Models{
    public class Employee{
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastNames { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
}