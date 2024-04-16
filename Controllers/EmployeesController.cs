using Microsoft.AspNetCore.Mvc;
using Sistema.Models;
using Sistema.Data;
using Newtonsoft.Json;

namespace Sistema.Controllers
{
    public class EmployeesController : Controller
    {
        
        
        private readonly ExamenContext _context;

        public EmployeesController(ExamenContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                if (HttpContext.Session.GetString("jornada") != null)
                {   
                    DateHistory newHistory = new DateHistory();
                    var employee = JsonConvert.DeserializeObject<Employee>(HttpContext.Session.GetString("empleado"));
                    newHistory.Entries = DateTime.Now;
                    newHistory.EmployeesId = employee.Id;
                    string historyJson = JsonConvert.SerializeObject(newHistory);
                    HttpContext.Session.SetString("history", historyJson);
                    HttpContext.Session.Remove("jornada");
                }
                return View(_context.Employees.ToList());
            }
            else{
            return RedirectToAction("Login");

            }

        }

        public IActionResult Login(string Email, string Password)
        {

            if (HttpContext.Session.GetString("sesion") != null)
            {
                return RedirectToAction("Index");
            }

            var employees = from employee in _context.Employees select employee;

            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Email))
            {
                employees.Where(x => x.Email == Email && x.Password == Password);

                if (employees.Count() > 0)
                {
                    HttpContext.Session.SetString("sesion", "si");
                    HttpContext.Session.SetString("jornada", "1");
                    var ObjEmployee = employees.First();
                    string employee = JsonConvert.SerializeObject(ObjEmployee);
                    HttpContext.Session.SetString("empleado", employee);
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public async Task <IActionResult> Logout()
        {
            string? datestring = HttpContext.Session.GetString("history");
            DateHistory? date = JsonConvert.DeserializeObject<DateHistory>(datestring);
            if (date != null){
                date.Exits = DateTime.Now;
                await _context.DateHistory.AddAsync(date);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("sesion");
                }
                return RedirectToAction("Index");
        }
    }
}