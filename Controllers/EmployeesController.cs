using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;
using Sistema.Data;

namespace Sistema.Controllers{
    public class EmployeesController : Controller{ 
        bool Sesion = false;
        int EmployeeId = 0;
        DateHistory newHistory = new DateHistory();
        private readonly ExamenContext _context;

        public EmployeesController(ExamenContext context){
            _context = context;
        }

        public IActionResult Index(){
            if(Sesion){
                if(EmployeeId != 0){
                    newHistory.Entries = DateTime.Now;
                    newHistory.EmployeesId = EmployeeId;
                    EmployeeId = 0;
                }
                return View(_context.Employees.ToList());
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login(string Email, string Password){

            if(Sesion){
                return RedirectToAction("Index");
            }

            var employees = from employee in _context.Employees select employee;

            if(!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Email)){
                employees.Where(x => x.Email == Email && x.Password == Password);

                if(employees.Count() > 0){
                    Sesion = true;
                    EmployeeId = employees.First().Id;
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public IActionResult Logout(){
            Sesion = false;
            newHistory.Exits = DateTime.Now;
            _context.DateHistories.Add(newHistory);
            return RedirectToAction("Login");
        }
    }
}