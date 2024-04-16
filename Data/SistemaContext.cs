using Microsoft.EntityFrameworkCore;
using Sistema.Models;

namespace Sistema.Data{
    public class ExamenContext : DbContext{
        public ExamenContext(DbContextOptions<ExamenContext> options) : base(options){

        }
        public DbSet<Employee> Employees { get; set; }
    }
}